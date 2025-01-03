using AbstractTasksDomain.Models;

namespace AbstractTasksDomain.Services;

public interface IAbstractTaskExecutor
{
    public Task ExecuteAsync(AbstractTask task, string taskId, string userId,
        CancellationToken cancellationToken = default);
}