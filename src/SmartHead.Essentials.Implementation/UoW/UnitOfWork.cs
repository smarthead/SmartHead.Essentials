using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartHead.Essentials.Abstractions.Behaviour;
using SmartHead.Essentials.Abstractions.Ddd.Interfaces;
using SmartHead.Essentials.Extensions;

namespace SmartHead.Essentials.Implementation.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly DbContext Context;

        public UnitOfWork(DbContext context)
        {
            Context = context;
        }

        public virtual bool Commit()
        {
            CreationTimeCommit();
            ModificationTimeCommit();

            return Context.SaveChanges() > 0;
        }

        public virtual async Task<bool> CommitAsync(CancellationToken ct = default)
        {
            CreationTimeCommit();
            ModificationTimeCommit();

            return await Context.SaveChangesAsync(ct) > 0;
        }

        protected virtual void CreationTimeCommit() =>
            Context.ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added && x.Entity is IHasCreationTime)
                .ForEach(x =>
                {
                    if (x.Entity is IHasCreationTime entity)
                        entity.CreationTime = DateTime.UtcNow;
                });

        protected virtual void ModificationTimeCommit() =>
            Context.ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Modified && x.Entity is IHasModificationTime)
                .ForEach(x =>
                {
                    if (x.Entity is IHasModificationTime entity)
                        entity.LastModificationTime = DateTime.UtcNow;
                });
    }
}