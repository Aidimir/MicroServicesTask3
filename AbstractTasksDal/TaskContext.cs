using AbstractTasksDal.Entities;
using Microsoft.EntityFrameworkCore;

namespace AbstractTasksDal;

public interface ITaskContext
{
    public Task<TaskEntity> AddTaskAsync(TaskEntity task);
    public Task RemoveTaskAsync(string id, string UserId);
    public Task<TaskEntity> UpdateTaskAsync(TaskEntity task);
    public Task<TaskEntity> GetTaskByIdAsync(string id, string userId);
    public Task<List<TaskEntity>> GetTasksByUserIdAsync(string userId);
}

public class TaskContext : DbContext, ITaskContext
{
    public TaskContext(DbContextOptions<TaskContext> options) : base(options)
    {
    }

    private DbSet<TaskEntity> _tasks { get; set; }

    public async Task<TaskEntity> AddTaskAsync(TaskEntity task)
    {
        await _tasks.AddAsync(task);
        await SaveChangesAsync();
        return task;
    }

    public async Task RemoveTaskAsync(string id, string userId)
    {
        var existingTask = await GetTaskByIdAsync(id, userId);
        _tasks.Remove(existingTask);
        await SaveChangesAsync();
    }

    public async Task<TaskEntity> UpdateTaskAsync(TaskEntity task)
    {
        _tasks.Update(task);
        await SaveChangesAsync();
        return task;
    }

    public async Task<TaskEntity> GetTaskByIdAsync(string id, string userId)
    {
        var task = await _tasks
            .Where(t => string.Equals(t.UserId.ToString(), userId, StringComparison.OrdinalIgnoreCase))
            .FirstOrDefaultAsync(t => string.Equals(t.Id.ToString(), id));
        
        if (task is null)
            throw new KeyNotFoundException("No task was found for this user");

        return task;
    }

    public async Task<List<TaskEntity>> GetTasksByUserIdAsync(string userId)
    {
        var tasks = await _tasks.Where(task => task.UserId.ToString() == userId).ToListAsync();
        if (tasks is null)
            throw new KeyNotFoundException("Tasks not found for this user");

        return tasks;
    }
}