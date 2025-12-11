using AutoMapper;
using Ettad.Workflows.Service.DTO;

using Ettad.Data.Entities.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                .ForMember(dest => dest.AllowedSkipTargetIds, opt => opt.MapFrom(src => src.Transitions.Select(t => t.TargetWorkflowStepId).ToList()));
            CreateMap<WorkflowStepDto, WorkflowStep>();

            // WorkflowApprovalStep
            CreateMap<WorkflowApprovalStep, WorkflowApprovalStepDto>();
            CreateMap<WorkflowApprovalStepDto, WorkflowApprovalStep>();

            // WorkflowApprovalHistory
            CreateMap<WorkflowStepApprovalLog, WorkflowApprovalHistoryDto>();
            CreateMap<WorkflowApprovalHistoryDto, WorkflowStepApprovalLog>();
        }
    }

}
