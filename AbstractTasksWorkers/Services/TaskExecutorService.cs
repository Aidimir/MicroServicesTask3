using AbstractTaskContracts.IncomeModels;
using AbstractTasksDomain.Models;
using AbstractTasksDomain.Services;
using MassTransit;

namespace AbstractTasksLogic.Services;

public class TaskExecutorService : IAbstractTaskExecutor
{
    private readonly ILogger<TaskExecutorService> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public TaskExecutorService(IPublishEndpoint publishEndpoint, ILogger<TaskExecutorService> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task ExecuteAsync(AbstractTask task, string taskId, CancellationToken cancellationToken = default)
    {
        try
        {
            // Лог: начало выполнения задачи
            _logger.LogInformation("Task {TaskId} is starting execution. Initial status: {Status}", taskId,
                task.Status);

            // Переводим задачу в статус "Ожидает выполнения"
            task.Status = "Awaiting";
            task.StatusMessage = "Awaiting task to be executed";
            await PublishStatusChangedAsync(task, taskId);
            _logger.LogInformation("Task {TaskId} status updated to {Status}", taskId, task.Status);

            // Имитация ожидания 5 sec перед началом выполнения
            await Task.Delay(5000, cancellationToken);

            // Переводим задачу в статус "Выполняется"
            task.Status = "Executing";
            task.StatusMessage = "Executing task";
            task.ExecutionStartedAt = DateTime.UtcNow;
            await PublishStatusChangedAsync(task, taskId);
            _logger.LogInformation("Task {TaskId} execution started at {StartTime}. Status: {Status}", taskId,
                task.ExecutionStartedAt, task.Status);

            // Генерация случайного времени выполнения (5-1.5*TTL секунд)
            var random = new Random();
            var executionTime = random.Next(5, (int) (task.TTL * 1.5)) * 1000; // миллисекунды
            var ttl = task.TTL * 1000; // TTL в миллисекундах

            _logger.LogInformation("Task {TaskId} execution time: {ExecutionTime}ms, TTL: {TTL}ms", taskId,
                executionTime, ttl);

            if (executionTime > ttl)
            {
                // Задача превышает TTL, отменяем выполнение
                await Task.Delay(ttl, cancellationToken);

                task.Status = "Canceled";
                task.StatusMessage = "TTL is shorter than the execution time";
                task.ExecutionFinishedAt = DateTime.UtcNow;
                await PublishStatusChangedAsync(task, taskId);

                _logger.LogWarning("Task {TaskId} canceled due to exceeding TTL. Finished at {FinishTime}", taskId,
                    task.ExecutionFinishedAt);
                return;
            }

            // Выполнение задачи
            await Task.Delay(executionTime, cancellationToken);

            // Завершаем задачу, если выполнение уложилось в TTL
            task.Status = "Finished";
            task.StatusMessage = "Finished task succesfully";
            task.ExecutionFinishedAt = DateTime.UtcNow;
            await PublishStatusChangedAsync(task, taskId);

            _logger.LogInformation("Task {TaskId} successfully finished at {FinishTime}", taskId,
                task.ExecutionFinishedAt);
        }
        catch (Exception ex)
        {
            // Обработка ошибок
            task.Status = "Canceled";
            task.StatusMessage = $"Exception caught while executing task. {ex.Message}";
            task.ExecutionFinishedAt = DateTime.UtcNow;
            await PublishStatusChangedAsync(task, taskId);

            _logger.LogError(ex, "Task {TaskId} failed with an exception. Status set to {Status}", taskId, task.Status);
        }
    }

    private async Task PublishStatusChangedAsync(AbstractTask task, string taskId)
    {
        await _publishEndpoint.Publish(new TaskExecutedModel
        {
            TaskId = taskId,
            Status = task.Status,
            StatusMessage = task.StatusMessage,
            ExecutionFinishedAt = task.ExecutionFinishedAt,
            ExecutionStartedAt = task.ExecutionStartedAt,
            TTL = task.TTL
        });

        _logger.LogInformation("Status change of Task {TaskId} published to the queue. Current status: {Status}",
            taskId, task.Status);
    }
}