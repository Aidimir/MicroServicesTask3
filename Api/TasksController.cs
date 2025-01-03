using AbstractTaskContracts.IncomeModels;
using AbstractTaskContracts.OutcomeModels;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Api;

[Route("api/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly IRequestClient<CreateTaskModel> _createRequestClient;
    private readonly IRequestClient<DeleteTaskModel> _deleteRequestClient;
    private readonly IRequestClient<GetTaskModel> _getRequestClient;
    private readonly IRequestClient<RestartTaskModel> _restartRequestClient;

    public TasksController(IRequestClient<CreateTaskModel> createRequestClient,
        IRequestClient<RestartTaskModel> restartRequestClient, IRequestClient<GetTaskModel> getRequestClient,
        IRequestClient<DeleteTaskModel> deleteRequestClient)
    {
        _createRequestClient = createRequestClient;
        _restartRequestClient = restartRequestClient;
        _getRequestClient = getRequestClient;
        _deleteRequestClient = deleteRequestClient;
    }

    [HttpPost]
    public async Task<IActionResult> AddTask(CreateTaskModel taskRequest)
    {
        var response = await _createRequestClient.GetResponse<TaskResponse>(taskRequest);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskById(string id)
    {
        var model = new GetTaskModel {TaskId = id, UserId = null};
        var response = await _getRequestClient.GetResponse<MultipleTasksResponse>(model);

        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> RestartTask(string id)
    {
        var model = new RestartTaskModel {TaskId = id};
        var response = await _restartRequestClient.GetResponse<RestartTaskResponse>(model);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(string id)
    {
        var model = new DeleteTaskModel {TaskId = id};
        await _deleteRequestClient.GetResponse<DeleteTaskResponse>(model);

        return NoContent();
    }
}