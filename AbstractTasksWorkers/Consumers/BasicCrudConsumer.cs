using AbstractTasksLogic.Services;
using AutoMapper;
using MassTransit;

namespace AbstractTasksLogic.Consumers;

public abstract class BasicCrudConsumer<T> : IConsumer<T> where T : class
{
    protected readonly IAbstractTaskService _abstractTaskService;
    protected readonly ILogger<BasicCrudConsumer<T>> _logger;
    protected readonly IMapper _mapper;

    protected BasicCrudConsumer(IAbstractTaskService abstractTaskService, IMapper mapper,
        ILogger<BasicCrudConsumer<T>> logger)
    {
        _abstractTaskService = abstractTaskService;
        _mapper = mapper;
        _logger = logger;
    }

    public virtual async Task Consume(ConsumeContext<T> context)
    {
        _logger.LogInformation("Consuming message: {@Message}", context.Message);
    }
}