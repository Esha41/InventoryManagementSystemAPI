# DevExpress Reporting Troubleshooting Guide

## Error: "Cannot find a custom WebDocumentViewerController class descendant"

This error occurs when DevExpress Reporting cannot find your custom controller classes. Here's how to fix it:

### Solution

1. **Verify Controller Classes Exist**
   - Ensure `CustomWebDocumentViewerController.cs` exists in `Project.Api/Controllers/`
   - Ensure `CustomReportDesignerController.cs` exists in `Project.Api/Controllers/`
   - Both should inherit from their respective base classes

2. **Verify Assembly Registration**
   - In `Program.cs`, ensure the assembly containing the controllers is registered:
   ```csharp
   .AddApplicationPart(typeof(Ettad.Reporting.Controllers.ReportDesignerController).Assembly)
   ```
   - Since all controllers are in the same assembly (Project.Api), you only need one `AddApplicationPart` call

3. **Verify Namespace**
   - Controllers should be in namespace: `Ettad.Reporting.Controllers`
   - Ensure the namespace matches exactly

4. **Build and Restart**
   - Clean and rebuild the solution
   - Restart the application
   - Clear browser cache

5. **Check Controller Inheritance**
   ```csharp
   public class CustomWebDocumentViewerController : WebDocumentViewerController
   public class CustomReportDesignerController : ReportDesignerController
   ```

### Common Issues

1. **Controllers not in Controllers folder**: Move them to `Project.Api/Controllers/`
2. **Wrong namespace**: Ensure namespace is `Ettad.Reporting.Controllers`
3. **Assembly not registered**: Add the assembly to `AddApplicationPart`
4. **Build errors**: Fix any compilation errors first

### Verification

After fixing, verify the controllers are discovered:
- Check that the application starts without errors
- The error message should disappear
- DevExpress endpoints should be accessible at `/DXXRDV` and `/DXXRD`
