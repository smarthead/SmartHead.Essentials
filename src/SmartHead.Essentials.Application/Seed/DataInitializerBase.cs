using System;
using System.Threading.Tasks;
using Extensions.Hosting.AsyncInitialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SmartHead.Essentials.Application.Seed
{
    public abstract class DataInitializerBase : IAsyncInitializer
    {
        protected readonly IServiceProvider ServiceProvider;

        protected DataInitializerBase(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public async Task InitializeAsync()
        {
            using var scope = ServiceProvider.CreateScope();
            await using var context = scope.ServiceProvider.GetRequiredService<DbContext>();
            await InitializeAsync(context);
            await context.SaveChangesAsync();
        }

        protected abstract Task InitializeAsync(DbContext dbContext);
    }
}