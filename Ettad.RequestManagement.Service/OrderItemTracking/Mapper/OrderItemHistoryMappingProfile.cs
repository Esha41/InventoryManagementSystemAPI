using AutoMapper;
using Ettad.Data.Entities;
using Ettad.Data.Interfaces.Services;

namespace Ettad.RequestManagement.Service.OrderItemTracking.Mapper
{
    public class OrderItemHistoryMappingProfile : Profile
    {
        public OrderItemHistoryMappingProfile()
        {
            CreateMap<OrderItemHistory, OrderItemHistoryDto>()
                .ForMember(dest => dest.OrderRequestNo, opt => opt.Ignore())
                .ForMember(dest => dest.ItemName, opt => opt.Ignore())
                .ForMember(dest => dest.ItemNo, opt => opt.Ignore())
                .ForMember(dest => dest.DepartmentNameAr, opt => opt.Ignore())
                .ForMember(dest => dest.DepartmentNameEn, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedByUserNameEn, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedByUserNameAr, opt => opt.Ignore())
                .ForMember(dest => dest.WorkflowStepName, opt => opt.Ignore());
        }
    }
}
