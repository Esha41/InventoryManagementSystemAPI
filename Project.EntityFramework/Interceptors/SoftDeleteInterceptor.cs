using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Ettad.CrossCutting.Comman.Base;
using Ettad.Application.Common.Interfaces;
using System.Runtime.InteropServices;
using Ettad.CrossCutting.Comman.Time;
using Ettad.CrossCutting.Comman.Interface;

namespace Ettad.EntityFramework.Interceptors
{
    public class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public SoftDeleteInterceptor(ICurrentUserService currentUserService, IDateTimeProvider dateTimeProvider)
        {
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
        }

        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            if (eventData.Context != null)
            {
                ProcessSoftDelete(eventData.Context);
            }

            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            if (eventData.Context != null)
            {
                ProcessSoftDelete(eventData.Context);
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void ProcessSoftDelete(DbContext context)
        {
            var entries = context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Deleted);

            foreach (var entry in entries)
            {
                // Check if entity implements ISoftDeletable or inherits from FullAuditEntity
                if (entry.Entity is ISoftDeletable softDeletableEntity)
                {
                    // Prevent the actual deletion
                    entry.State = EntityState.Modified;

                    // Set soft delete properties
                    var deletionDate = _dateTimeProvider.Now;
                    softDeletableEntity.IsDeleted = true;
                    softDeletableEntity.DeletionDate = deletionDate;
                    softDeletableEntity.DeletedBy = _currentUserService.UserId;

                    // Also update ModificationDate for consistency
                    if (entry.Entity is AuditEntity<long> auditEntity)
                    {
                        auditEntity.ModificationDate = deletionDate;
                    }
                }
            }
        }
    }
}

