using AbstractTasksDomain.Models;

namespace AbstractTasksDomain.Services;

public interface IAbstractTaskExecutor
{
    public Task ExecuteAsync(AbstractTask task, string taskId, CancellationToken cancellationToken = default);
}