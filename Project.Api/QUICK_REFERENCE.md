# Quick Reference: Code Quality Checklist

## 🔴 Must Fix (Critical)

1. **Remove Code Duplication** - Three identical search methods (Weapons, Ammunitions, Explosives)
2. **Remove Dead Code** - `DetectItemTypes()`, `RemoveEntityTypeKeywords()`, `NlpLite` class
3. **Add Input Validation** - Validate `Take`, `Query` length, date ranges
4. **Extract Constants** - Replace magic numbers (20, 50, 0.5, etc.)

## 🟡 Should Fix (High Priority)

5. **Move DTOs** - Extract `SearchRequest`, `SearchResponse`, `SearchResult` to separate files
6. **Improve Error Handling** - Consistent logging with context
7. **Use Options Pattern** - Replace direct `IConfiguration` access
8. **Add XML Documentation** - Document public APIs

## 🟢 Nice to Have (Medium Priority)

9. **Extract Services** - Move business logic out of controller
10. **Improve Performance** - Use HashSet for token lookups, compile regexes
11. **Add Unit Tests** - Test complex logic (expression building, scoring)
12. **Better Naming** - `_db` → `_dbContext`, `ct` → `cancellationToken`

## 📋 Code Style

- ✅ Use consistent indentation (4 spaces)
- ✅ Break long lines (>120 chars)
- ✅ Remove obvious comments
- ✅ Add "why" comments for complex logic
- ✅ Use full parameter names in public APIs

## 🎯 Top 5 Quick Wins

1. Remove unused methods (5 min)
2. Extract constants (10 min)
3. Add input validation (15 min)
4. Improve error logging (10 min)
5. Move DTOs to separate files (10 min)

**Total Time**: ~50 minutes for significant improvement!
