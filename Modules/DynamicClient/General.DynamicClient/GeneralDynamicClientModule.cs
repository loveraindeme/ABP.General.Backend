using General.DynamicClient.HttpApi.Client;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

namespace General.DynamicClient
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpAutofacModule),
        typeof(AbpSwashbuckleModule),
        typeof(GeneralDynamicClientHttpApiClientModule)
        )]
    public class GeneralDynamicClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddEndpointsApiExplorer();
            context.Services.AddSwaggerGen();
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

            app.UseSwagger();
            app.UseAbpSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "DynamicClient API");
            });

            app.UseAuthorization();
        }
    }
}
