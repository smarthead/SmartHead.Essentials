using System;
using System.Threading.Tasks;
using Extensions.Hosting.AsyncInitialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SmartHead.Essentials.Application.Seed
{
    public class MigrationsInitializer : IAsyncInitializer
    {
        private readonly IServiceProvider _provider;

        public MigrationsInitializer(IServiceProvider serviceProvider)
        {
            _provider = serviceProvider;
        }

        public async Task InitializeAsync()
        {
            using var scope = _provider.CreateScope();
            await using var context = scope.ServiceProvider.GetRequiredService<DbContext>();
            await context.Database.MigrateAsync();
        }
    }
}
