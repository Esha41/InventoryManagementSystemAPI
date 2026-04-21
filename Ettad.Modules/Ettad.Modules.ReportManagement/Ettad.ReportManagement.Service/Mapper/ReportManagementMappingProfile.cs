using AutoMapper;
using Ettad.Data.Entities;
using Ettad.ReportManagement.Service.Dtos;

namespace Ettad.ReportManagement.Service.Mapper
{
    public class ReportManagementMappingProfile : Profile
    {
        public ReportManagementMappingProfile()
        {
            // Report mappings
            CreateMap<CreateReportDto, ReportEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Url, opt => opt.Ignore()) // URL is set manually in service
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletionDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore());

            CreateMap<UpdateReportDto, ReportEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletionDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore());

            CreateMap<ReportEntity, ReportDto>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore()); // Roles are mapped separately in service

            // ScheduledReport mappings
            CreateMap<CreateScheduledReportDto, ScheduledReport>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.NextRunDate, opt => opt.Ignore()) // Calculated in service
                .ForMember(dest => dest.LastRunDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletionDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Report, opt => opt.Ignore())
                .ForMember(dest => dest.Recipients, opt => opt.Ignore()) // Recipients are mapped separately
                .ForMember(dest => dest.Executions, opt => opt.Ignore());

            CreateMap<UpdateScheduledReportDto, ScheduledReport>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.NextRunDate, opt => opt.Ignore()) // Recalculated in service
                .ForMember(dest => dest.LastRunDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletionDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Report, opt => opt.Ignore())
                .ForMember(dest => dest.Recipients, opt => opt.Ignore()) // Recipients are mapped separately
                .ForMember(dest => dest.Executions, opt => opt.Ignore());

            CreateMap<ScheduledReport, ScheduledReportDto>()
                .ForMember(dest => dest.ReportName, opt => opt.MapFrom(src => src.Report != null ? src.Report.ReportName : string.Empty))
                .ForMember(dest => dest.ReportUrl, opt => opt.MapFrom(src => src.Report != null ? src.Report.Url : string.Empty))
                .ForMember(dest => dest.Recipients, opt => opt.Ignore()); // Recipients are mapped separately with user info

            // ScheduledReportRecipient mappings
            CreateMap<CreateScheduledReportRecipientDto, ScheduledReportRecipient>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ScheduledReportId, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletionDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ScheduledReport, opt => opt.Ignore());

            CreateMap<ScheduledReportRecipient, ScheduledReportRecipientDto>()
                .ForMember(dest => dest.UserName, opt => opt.Ignore()) // UserName is fetched from user service
                .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.EmailAddress)); // EmailAddress may be fetched from user service if UserId is present

            // ScheduledReportExecution mappings
            CreateMap<ScheduledReportExecution, ScheduledReportExecutionDto>();
        }
    }
}

