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
            CreateMap<Ettad.Data.Entities.Workflows.Workflow, WorkflowDto>();
            CreateMap<WorkflowDto, Ettad.Data.Entities.Workflows.Workflow>();

            CreateMap<WorkflowStepCreateDto, WorkflowStep>().ReverseMap();

            CreateMap<WorkflowStep, WorkflowStepDto>();
            CreateMap<WorkflowStepDto ,WorkflowStep>();

            CreateMap<WorkflowApprovalHistory, WorkflowApprovalHistoryDto>();
            CreateMap<WorkflowApprovalHistoryDto,WorkflowApprovalHistory>();
        }
    }

}
