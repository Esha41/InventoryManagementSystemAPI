using AutoMapper;

using Ettad.Data.Entities.Workflows;
using Ettad.Workflows.Service.Dtos;

namespace Ettad.Workflows.Service.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Ettad.Data.Entities.Workflows.Workflow, WorkflowDto>()
                .AfterMap((src, dest, _) => WorkflowAutoRejectTriggerMapper.MapToWorkflowDto(dest, src.AutoRejectTrigger));

            CreateMap<WorkflowDto, Ettad.Data.Entities.Workflows.Workflow>()
                .ForMember(d => d.AutoRejectTrigger, opt => opt.Ignore());

            // WorkflowStep
            CreateMap<WorkflowStepCreateDto, WorkflowStep>().ReverseMap();
            CreateMap<WorkflowStep, WorkflowStepDto>()
                .ForMember(dest => dest.Transitions, opt => opt.Ignore()); // Complex mapping handled manually
            CreateMap<WorkflowStepDto, WorkflowStep>();

            CreateMap<WorkflowStepParallelRole, WorkflowStepParallelRoleDto>()
                .ForMember(d => d.RoleName, o => o.MapFrom(s => s.Role != null ? s.Role.Name : null))
                .ForMember(d => d.RoleNameAr, o => o.MapFrom(s => s.Role != null ? s.Role.NameAr : null));

            // WorkflowStepTransition
            CreateMap<WorkflowStepTransition, WorkflowStepTransitionDto>()
                .ForMember(dest => dest.TargetStep, opt => opt.Ignore()); // Complex mapping handled manually

            // WorkflowApprovalStep
            CreateMap<WorkflowApprovalStep, WorkflowApprovalStepDto>();
            CreateMap<WorkflowApprovalStepDto, WorkflowApprovalStep>();

            // WorkflowApprovalHistory
            CreateMap<WorkflowStepApprovalLog, WorkflowApprovalHistoryDto>();
            CreateMap<WorkflowApprovalHistoryDto, WorkflowStepApprovalLog>();
        }
    }

}
