using AbstractTaskContracts.IncomeModels;
using AbstractTaskContracts.OutcomeModels;
using AbstractTasksLogic.Services;
using AutoMapper;
using MassTransit;

namespace AbstractTasksLogic.Consumers;

public class AddTaskConsumer : BasicCrudConsumer<CreateTaskModel>
{
    public AddTaskConsumer(IAbstractTaskService abstractTaskService, IMapper mapper,
        ILogger<AddTaskConsumer> logger) : base(
        abstractTaskService, mapper, logger)
    {
    }

    public override async Task Consume(ConsumeContext<CreateTaskModel> context)
    {
        await base.Consume(context);
        var result = await _abstractTaskService.AddTaskAsync(context.Message);
        var mappedModel = _mapper.Map<TaskResponse>(result);
        _logger.LogInformation("respondedResult: {@Message}", mappedModel);
        try
        {
            await context.RespondAsync(mappedModel);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}