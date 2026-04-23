using AutoMapper;

using Ettad.Data.Entities.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ettad.Workflows.Service.Dtos;

namespace Ettad.Workflows.Service.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Workflow
            CreateMap<Ettad.Data.Entities.Workflows.Workflow, WorkflowDto>();
            CreateMap<WorkflowDto, Ettad.Data.Entities.Workflows.Workflow>();

            // WorkflowStep
            CreateMap<WorkflowStepCreateDto, WorkflowStep>().ReverseMap();
            CreateMap<WorkflowStep, WorkflowStepDto>()
                .ForMember(dest => dest.Transitions, opt => opt.Ignore()); // Complex mapping handled manually
            CreateMap<WorkflowStepDto, WorkflowStep>();
            
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
