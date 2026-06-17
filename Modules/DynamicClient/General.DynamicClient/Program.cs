
namespace General.DynamicClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Host.UseAutofac();
            builder.Services.AddApplication<GeneralDynamicClientModule>();

            var app = builder.Build();

            app.InitializeApplication();

            app.MapControllers();

            app.Run();
        }
    }
}
