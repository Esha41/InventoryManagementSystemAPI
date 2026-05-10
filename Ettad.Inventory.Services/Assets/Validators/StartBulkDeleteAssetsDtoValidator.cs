using System;
using System.Collections.Generic;
using System.Linq;
using Ettad.Inventory.Service.Assets.Dtos;
using FluentValidation;

namespace Ettad.Inventory.Service.Assets.Validators
{
    public class StartBulkDeleteAssetsDtoValidator : AbstractValidator<StartBulkDeleteAssetsDto>
    {
        /// <summary>Hard cap per request body for ExplicitIds.</summary>
        public const int MaxExplicitIds = 25_000;

        public StartBulkDeleteAssetsDtoValidator()
        {
            RuleFor(x => x.Scope).IsInEnum();

            RuleFor(x => x.BatchId).Null().When(x => x.Scope != AssetBulkDeletionScopeDto.Batch);
            RuleFor(x => x.DepotId).Null().When(x => x.Scope != AssetBulkDeletionScopeDto.Depot);

            RuleFor(x => x.BatchId)
                .NotNull().GreaterThan(0)
                .When(x => x.Scope == AssetBulkDeletionScopeDto.Batch);

            RuleFor(x => x.DepotId)
                .NotNull().GreaterThan(0)
                .When(x => x.Scope == AssetBulkDeletionScopeDto.Depot);

            RuleFor(x => x.AssetIds)
                .Must(list => list == null || list.Count == 0)
                .When(x => x.Scope != AssetBulkDeletionScopeDto.ExplicitIds)
                .WithMessage("AssetIds must be empty unless scope is ExplicitIds.");

            RuleFor(x => x.AssetIds)
                .NotEmpty()
                .When(x => x.Scope == AssetBulkDeletionScopeDto.ExplicitIds);

            RuleFor(x => x.AssetIds)
                .Must(list => list != null && list.Count <= MaxExplicitIds)
                .When(x => x.Scope == AssetBulkDeletionScopeDto.ExplicitIds)
                .WithMessage($"At most {MaxExplicitIds} asset ids allowed per explicit delete request.");

            RuleFor(x => x.AssetIds)
                .Must(list => list != null && list.All(id => id > 0))
                .When(x => x.Scope == AssetBulkDeletionScopeDto.ExplicitIds)
                .WithMessage("All asset ids must be positive.");
        }
    }
}
