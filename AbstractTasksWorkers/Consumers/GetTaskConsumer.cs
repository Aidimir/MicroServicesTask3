using AbstractTaskContracts.IncomeModels;
using AbstractTaskContracts.OutcomeModels;
using AbstractTasksLogic.Services;
using AutoMapper;
using MassTransit;

namespace AbstractTasksLogic.Consumers;

public class GetTaskConsumer : BasicCrudConsumer<GetTaskModel>
{
    public GetTaskConsumer(IAbstractTaskService abstractTaskService, IMapper mapper,
        ILogger<GetTaskConsumer> _logger) : base(
        abstractTaskService, mapper, _logger)
    {
    }

    public override async Task Consume(ConsumeContext<GetTaskModel> context)
    {
        await base.Consume(context);
        var result = await _abstractTaskService.GetTasksAsync(context.Message.TaskId, context.Message.UserId);
        var mappedModels = result.Select(task => _mapper.Map<TaskResponse>(task)).ToList();

        _logger.LogInformation("respondedResult: {@Message}", mappedModels);
        await context.RespondAsync(new MultipleTasksResponse {Tasks = mappedModels});
    }
}