using GoodBurger.Api.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GoodBurger.Api.Infrastructure.Data.Interceptors
{
    public class AuditableEntityInterceptor(AppUser appUser) : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);

            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public void UpdateEntities(DbContext? context)
        {
            if (context == null) return;

            var dateTime = DateTime.UtcNow;

            foreach (var entry in context.ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted || entry.HasChangedOwnedEntities())
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Entity.CreatedBy = appUser.UserId;
                        entry.Entity.CreatedAt = dateTime;
                    }
                    else if (entry.State == EntityState.Deleted)
                    {
                        entry.State = EntityState.Modified;
                        entry.Entity.DeletedBy = appUser.UserId;
                        entry.Entity.DeletedAt = dateTime;
                    }

                    entry.Entity.UpdatedBy = appUser.UserId;
                    entry.Entity.UpdatedAt = dateTime;
                }
            }
        }
    }

    public static class Extensions
    {
        public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
            entry.References.Any(r =>
                r.TargetEntry != null &&
                r.TargetEntry.Metadata.IsOwned() &&
                (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
    }
}
