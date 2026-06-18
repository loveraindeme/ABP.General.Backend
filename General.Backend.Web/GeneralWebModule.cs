using General.Backend.Application;
using General.Backend.Application.Options;
using General.Backend.Domain.Shared;
using General.Backend.Domain.Shared.Helper;
using General.Backend.EntityFrameworkCore;
using General.InformationCollection.Application;
using General.InformationCollection.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.ApiExploring;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Volo.Abp.VirtualFileSystem;

namespace General.Backend.Web
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpAutofacModule),
        typeof(AbpSwashbuckleModule),
        typeof(GeneralApplicationModule),
        typeof(GeneralInformationCollectionApplicationModule),
        typeof(GeneralEntityFrameworkCoreModule)
        )]
    public class GeneralWebModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var env = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            Configure<AbpJsonOptions>(options =>
            {
                options.OutputDateTimeFormat = ConstantHelper.DateTimeFormat;
                options.InputDateTimeFormats = [ConstantHelper.DateTimeFormat, ConstantHelper.DateFormat];
            });

            Configure<InformationCollectionOptions>(options =>
            {
                options.ModuleName = "General";
            });

            ConfigureCors(context);
            ConfigureCache();
            ConfigureAuthentication(context, configuration);
            ConfigureAutoApiController();
            ConfigureSwagger(context);
            ConfigureVirtualFileSystem(env);
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("GeneralCors");
            app.UseAbpRequestLocalization();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseAbpSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "General API");
                // 折叠Api
                //options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                // 去除Model显示
                //options.DefaultModelsExpandDepth(-1);
            });

            app.UseConfiguredEndpoints(endpoints =>
            {
                // dynamic or static client proxy call the API, need cancel the authorization
                // endpoints.MapControllers().RequireAuthorization();
            });
        }

        private static void ConfigureCors(ServiceConfigurationContext context)
        {
            context.Services.AddCors(options => options.AddPolicy("GeneralCors", policy => policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod()));
        }

        private void ConfigureCache()
        {
            Configure<AbpDistributedCacheOptions>(options =>
            {
                options.GlobalCacheEntryOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);
                options.GlobalCacheEntryOptions.SlidingExpiration = TimeSpan.FromMinutes(20);
            });
        }

        private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection(JwtOptions.JwtOption);
            Configure<JwtOptions>(jwtConfig);
            var jwtOption = new JwtOptions();
            jwtConfig.Bind(jwtOption);

            context.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    // 验证秘钥
                    ValidateIssuerSigningKey = true,
                    // 秘钥
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.Secret)),
                    // 验证颁发者
                    ValidateIssuer = true,
                    // 颁发者
                    ValidIssuer = jwtOption.Issuer,
                    // 验证订阅者
                    ValidateAudience = true,
                    // 订阅者
                    ValidAudience = jwtOption.Audience,
                    // 验证有效时间
                    ValidateLifetime = true,
                    // 验证时间有效性的允许的时钟偏移量
                    ClockSkew = TimeSpan.FromMinutes(5)
                };
                jwtBearerOptions.Events = new JwtBearerEvents
                {
                    OnMessageReceived = (context) =>
                    {
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = (context) =>
                    {
                        return Task.CompletedTask;
                    }
                };
            });
        }

        private void ConfigureAutoApiController()
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(GeneralApplicationModule).Assembly);
                options.ConventionalControllers.Create(typeof(GeneralInformationCollectionApplicationModule).Assembly);
            });
        }

        private void ConfigureSwagger(ServiceConfigurationContext context)
        {
            Configure<AbpRemoteServiceApiDescriptionProviderOptions>(options =>
            {
                options.SupportedResponseTypes.Clear();
            });
            context.Services.AddAbpSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "General API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
                options.HideAbpEndpoints();
                options.UseInlineDefinitionsForEnums();
                Directory.GetFiles(AppContext.BaseDirectory, "*.xml").ToList().ForEach(file =>
                {
                    options.IncludeXmlComments(file, true);
                });

                // 添加授权认证
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 在下方输入Bearer {token} 即可，注意两者之间有空格",
                    // jwt默认的参数名称
                    Name = "Authorization",
                    // jwt默认存放Authorization信息的位置
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    // 把所有方法配置为增加bearer头部信息
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme // 和AddSecurityDefinition方法指定的方案名称一致即可
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
        {
            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<GeneralDomainSharedModule>(
                        Path.Combine(
                            hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}{typeof(GeneralDomainSharedModule).Assembly.GetName().Name}"));
                });
            }
        }
    }
}
