using AbstractTaskContracts.IncomeModels;
using AbstractTaskContracts.OutcomeModels;
using AbstractTasksDomain.Models;
using AutoMapper;

namespace AbstractTasksLogic;

public class AutoMappingProfile : Profile
{
    public AutoMappingProfile()
    {
        CreateMap<AbstractTask, TaskResponse>()
            .ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.TTL, opt => opt.MapFrom(src => src.TTL))
            .ForMember(dest => dest.StatusMessage, opt => opt.MapFrom(src => src.StatusMessage));
        CreateMap<TaskExecutedModel, AbstractTask>()
            .ForMember(dest => dest.StatusMessage, opt => opt.MapFrom(src => src.StatusMessage));
    }
}