using AbstractTaskContracts.IncomeModels;
using AbstractTaskContracts.OutcomeModels;
using AbstractTasksLogic.Services;
using AutoMapper;
using MassTransit;

namespace AbstractTasksLogic.Consumers;

public class RestartTaskConsumer : BasicCrudConsumer<RestartTaskModel>
{
    public RestartTaskConsumer(IAbstractTaskService abstractTaskService, IMapper mapper,
        ILogger<RestartTaskConsumer> _logger) : base(
        abstractTaskService, mapper, _logger)
    {
    }

    public override async Task Consume(ConsumeContext<RestartTaskModel> context)
    {
        await base.Consume(context);

        await _abstractTaskService.RestartTaskAsync(context.Message.TaskId, context.Message.UserId);
        await context.RespondAsync(new RestartTaskResponse {TaskId = context.Message.TaskId});
    }
}