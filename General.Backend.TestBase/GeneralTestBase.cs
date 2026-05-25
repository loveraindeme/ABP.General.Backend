using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;
using Volo.Abp.Uow;

namespace General.Backend.TestBase
{
    public abstract class GeneralTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }

        protected virtual async Task WithUnitOfWorkAsync(AbpUnitOfWorkOptions options, Func<Task> func)
        {
            using (var scope = ServiceProvider.CreateAsyncScope())
            {
                var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
                using (var uow = uowManager.Begin(options))
                {
                    await func();
                    await uow.CompleteAsync();
                }
            }
        }

        protected virtual async Task<TResult> WithUnitOfWorkAsync<TResult>(AbpUnitOfWorkOptions options, Func<Task<TResult>> func)
        {
            using (var scope = ServiceProvider.CreateAsyncScope())
            {
                var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
                using (var uow = uowManager.Begin(options))
                {
                    var result = await func();
                    await uow.CompleteAsync();
                    return result;
                }
            }
        }

        protected virtual async Task WithUnitOfWorkAsync(Func<Task> func)
        {
            await WithUnitOfWorkAsync(new AbpUnitOfWorkOptions(), func);
        }

        protected virtual async Task<TResult> WithUnitOfWorkAsync<TResult>(Func<Task<TResult>> func)
        {
            return await WithUnitOfWorkAsync(new AbpUnitOfWorkOptions(), func);
        }
    }
}
