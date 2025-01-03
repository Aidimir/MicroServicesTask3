using System.IdentityModel.Tokens.Jwt;
using AbstractTaskContracts.IncomeModels;
using Api;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient(); // Регистрируем IHttpClientFactory
builder.Services.AddSingleton<JwtSecurityTokenHandler, CustomJwtTokenHandler>(); // Регистрируем наш кастомный TokenHandler

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMassTransit(x =>
{
    var rabbitMqConfig = builder.Configuration.GetSection("RabbitMQConnection");
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitMqConfig["Host"], h =>
        {
            h.Username(rabbitMqConfig["Username"] ?? "Guest");
            h.Password(rabbitMqConfig["Password"] ?? "Guest");
        });
    });

    x.AddRequestClient<CreateTaskModel>();
    x.AddRequestClient<RestartTaskModel>();
    x.AddRequestClient<DeleteTaskModel>();
    x.AddRequestClient<GetTaskModel>();
});
builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddSwaggerGen(setup =>
{
    // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {jwtSecurityScheme, Array.Empty<string>()}
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var serviceProvider = builder.Services.BuildServiceProvider();
        options.TokenHandlers.Clear();
        options.TokenHandlers.Add(serviceProvider.GetRequiredService<JwtSecurityTokenHandler>());
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();