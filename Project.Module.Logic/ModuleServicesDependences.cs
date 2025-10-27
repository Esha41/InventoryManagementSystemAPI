//using MediatR;
//using Microsoft.Extensions.DependencyInjection;
//using Ettad.Module.Logic.Behaviors;
//using Ettad.Module.Logic.Validators.Project;


//namespace Ettad.Module.Logic
//{
//    public static class ModuleServicesDependences
//    {
//        public static IServiceCollection AddLogicServices(this IServiceCollection service)
//        {
//            service.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
//            service.AddValidatorsFromAssemblyContaining<CreateProductValidator>();

//            service.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
//            service.AddValidatorsFromAssemblyContaining<CreateProductValidator>();

//            return service;
//        }
//    }
//}
