using AbstractTaskContracts.IncomeModels;
using AbstractTaskContracts.OutcomeModels;
using AbstractTasksLogic.Services;
using AutoMapper;
using MassTransit;

namespace AbstractTasksLogic.Consumers;

public class DeleteTaskConsumer : BasicCrudConsumer<DeleteTaskModel>
{
    public DeleteTaskConsumer(IAbstractTaskService abstractTaskService, IMapper mapper,
        ILogger<DeleteTaskConsumer> _logger) : base(
        abstractTaskService, mapper, _logger)
    {
    }

    public override async Task Consume(ConsumeContext<DeleteTaskModel> context)
    {
        await base.Consume(context);
        await _abstractTaskService.DeleteTaskAsync(context.Message.TaskId, context.Message.UserId);

        await context.RespondAsync(new DeleteTaskResponse
            {Success = true, Message = $"Task {context.Message.TaskId} was successfully deleted"});
    }
}