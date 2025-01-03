using AbstractTaskContracts.IncomeModels;
using AbstractTasksDomain.Models;
using AbstractTasksLogic.Services;
using AutoMapper;
using MassTransit;

namespace AbstractTasksLogic.Consumers;

public class TaskExecutedConsumer : BasicCrudConsumer<TaskExecutedModel>
{
    public TaskExecutedConsumer(IAbstractTaskService abstractTaskService, IMapper mapper,
        ILogger<TaskExecutedConsumer> _logger) : base(
        abstractTaskService, mapper, _logger)
    {
    }

    public override async Task Consume(ConsumeContext<TaskExecutedModel> context)
    {
        await base.Consume(context);
        var updatedTask = _mapper.Map<AbstractTask>(context.Message);

        var result = await _abstractTaskService.UpdateTaskAsync(updatedTask, context.Message.TaskId);
        _logger.LogInformation("Update task result: {@Message}", result);
    }
}