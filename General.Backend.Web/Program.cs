using Serilog;
using Serilog.Events;

namespace General.Backend.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var isDevelopment = IsDevelopmentEnvironment();
            if (!isDevelopment)
            {
                Directory.SetCurrentDirectory(AppContext.BaseDirectory);
            }
            InitializeLog();
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog();
            builder.Host.UseAutofac();
            builder.Services.AddApplication<GeneralWebModule>();
            builder.Services.AddRazorPages();

            var app = builder.Build();

            app.InitializeApplication();
            app.MapRazorPages();

            try
            {
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "程序异常停止");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void InitializeLog()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("serilog.json", optional: false, reloadOnChange: false);
            // 定义Serilog配置
            Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .ReadFrom.Configuration(builder.Build())
            .CreateLogger();
        }

        private static bool IsDevelopmentEnvironment()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
            return string.Equals(environmentName, Environments.Development, StringComparison.OrdinalIgnoreCase);
        }
    }
}
