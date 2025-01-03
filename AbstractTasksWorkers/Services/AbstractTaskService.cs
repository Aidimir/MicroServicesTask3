using AbstractTaskContracts.IncomeModels;
using AbstractTasksDal;
using AbstractTasksDal.Entities;
using AbstractTasksDomain.Models;
using AbstractTasksDomain.Services;

namespace AbstractTasksLogic.Services;

public interface IAbstractTaskService
{
    public Task<AbstractTask> AddTaskAsync(CreateTaskModel model);
    public Task DeleteTaskAsync(string id, string userId);
    public Task<AbstractTask> UpdateTaskAsync(AbstractTask model, string userId);
    public Task RestartTaskAsync(string id, string userId);
    public Task<IEnumerable<AbstractTask>> GetTasksAsync(string? taskId, string userId);
}

public class AbstractTaskService : IAbstractTaskService
{
    private readonly IAbstractTaskExecutor _abstractTaskExecutor;
    private readonly ITaskContext _taskContext;

    public AbstractTaskService(ITaskContext taskContext, IAbstractTaskExecutor abstractTaskExecutor)
    {
        _taskContext = taskContext;
        _abstractTaskExecutor = abstractTaskExecutor;
    }

    public async Task<AbstractTask> AddTaskAsync(CreateTaskModel model)
    {
        var entityModel = new TaskEntity
        {
            Id = Guid.NewGuid(),
            Description = model.Description,
            TTL = model.TTL,
            Created = DateTime.UtcNow,
            Status = "",
            StatusMessage = "Just added",
            Data = model.Data,
            UserId = Guid.Parse(model.UserId),
            ExecutionStartedAt = null,
            ExecutionFinishedAt = null
        };

        var result = await _taskContext.AddTaskAsync(entityModel);
        var domainModel = new AbstractTask
        {
            Id = result.Id.ToString(),
            Data = result.Data,
            Description = result.Description,
            TTL = result.TTL,
            Status = result.Status,
            StatusMessage = result.StatusMessage,
            ExecutionStartedAt = result.ExecutionStartedAt,
            ExecutionFinishedAt = result.ExecutionFinishedAt
        };

        _abstractTaskExecutor.ExecuteAsync(domainModel, result.Id.ToString(), entityModel.UserId.ToString());

        return domainModel;
    }

    public async Task DeleteTaskAsync(string id, string userId)
    {
        await _taskContext.RemoveTaskAsync(id, userId);
    }

    public async Task<AbstractTask> UpdateTaskAsync(AbstractTask model, string userId)
    {
        var existingEntity = await _taskContext.GetTaskByIdAsync(model.Id, userId);
        existingEntity.Description = model.Description;
        existingEntity.TTL = model.TTL;
        existingEntity.Status = model.Status;
        existingEntity.ExecutionStartedAt = model.ExecutionStartedAt;
        existingEntity.ExecutionFinishedAt = model.ExecutionFinishedAt;
        existingEntity.StatusMessage = model.StatusMessage;

        var result = await _taskContext.UpdateTaskAsync(existingEntity);
        var task = GetDomainModelFromEntity(result);
        return task;
    }

    public async Task RestartTaskAsync(string id, string userId)
    {
        var existingEntity = await _taskContext.GetTaskByIdAsync(id, userId);
        var domainModel = GetDomainModelFromEntity(existingEntity);

        _abstractTaskExecutor.ExecuteAsync(domainModel, id, existingEntity.UserId.ToString());
    }

    public async Task<IEnumerable<AbstractTask>> GetTasksAsync(string? taskId, string userId)
    {
        if (taskId != null)
        {
            var entity = await _taskContext.GetTaskByIdAsync(taskId, userId);
            var domainModel = GetDomainModelFromEntity(entity);

            return [domainModel];
        }

        var entities = await _taskContext.GetTasksByUserIdAsync(userId);
        if (!entities.Any())
            return new List<AbstractTask>();

        var result = entities.Select(GetDomainModelFromEntity).ToList();

        return result;
    }

    private AbstractTask GetDomainModelFromEntity(TaskEntity entity)
    {
        return new AbstractTask
        {
            Data = entity.Data,
            TTL = entity.TTL,
            Status = entity.Status,
            StatusMessage = entity.StatusMessage,
            ExecutionStartedAt = entity.ExecutionStartedAt,
            ExecutionFinishedAt = entity.ExecutionFinishedAt,
            Description = entity.Description,
            Id = entity.Id.ToString()
        };
    }
}