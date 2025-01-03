using AbstractTasksDomain.Services;
using AbstractTasksLogic;
using AbstractTasksLogic.Consumers;
using AbstractTasksLogic.Services;
using Api;
using MassTransit;
using Serilog;
using Serilog.Formatting.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("workers_appsettings.json")
    .AddEnvironmentVariables();

// Настройка Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information() // Уровень логирования
    .WriteTo.Console( // Вывод в консоль
        new JsonFormatter())
    .CreateLogger();

// Интеграция Serilog в Microsoft.Extensions.Logging
builder.Host.UseSerilog();

// Регистрация сервисов
builder.Services.AddAutoMapper(typeof(AutoMappingProfile));
builder.Services.AddRepositories(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddTransient<IAbstractTaskExecutor, TaskExecutorService>();
builder.Services.AddTransient<IAbstractTaskService, AbstractTaskService>();

// Регистрация MassTransit только с консюмерами
builder.Services.AddMassTransit(x =>
{
    // Регистрация консюмеров
    x.AddConsumer<AddTaskConsumer>();
    x.AddConsumer<DeleteTaskConsumer>();
    x.AddConsumer<GetTaskConsumer>();
    x.AddConsumer<RestartTaskConsumer>();
    x.AddConsumer<TaskExecutedConsumer>();

    var rabbitMqConfig = builder.Configuration.GetSection("RabbitMQConnection");
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitMqConfig["Host"], h =>
        {
            h.Username(rabbitMqConfig["Username"] ?? "Guest");
            h.Password(rabbitMqConfig["Password"] ?? "Guest");
        });

        cfg.ReceiveEndpoint("add-task-queue", e =>
        {
            e.PrefetchCount = 10;
            e.ConcurrentMessageLimit = 3;
            e.ConfigureConsumer<AddTaskConsumer>(context);
        });

        cfg.ReceiveEndpoint("delete-task-queue", e =>
        {
            e.PrefetchCount = 10;
            e.ConcurrentMessageLimit = 3;
            e.ConfigureConsumer<DeleteTaskConsumer>(context);
        });

        cfg.ReceiveEndpoint("get-task-queue", e =>
        {
            e.PrefetchCount = 10;
            e.ConcurrentMessageLimit = 3;
            e.ConfigureConsumer<GetTaskConsumer>(context);
        });

        cfg.ReceiveEndpoint("restart-task-queue", e =>
        {
            e.PrefetchCount = 10;
            e.ConcurrentMessageLimit = 3;
            e.ConfigureConsumer<RestartTaskConsumer>(context);
        });

        cfg.ReceiveEndpoint("executed-task-queue", e =>
        {
            e.PrefetchCount = 10;
            e.ConcurrentMessageLimit = 3;
            e.ConfigureConsumer<TaskExecutedConsumer>(context);
        });
    });
});

var app = builder.Build();

try
{
    Log.Information("Starting the application...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly!");
    throw;
}
finally
{
    Log.CloseAndFlush();
}