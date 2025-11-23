using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using user_todo.Infrastructure.Data;
using user_todo.Infrastructure.Repositories;
using user_todo.Infrastructure.Services;
using user_todo.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// DB context
var connectionString = builder.Configuration.GetConnectionString("constring");
builder.Services.AddDbContext<UserTodoDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// Infrastructure DI
builder.Services.AddInfrastructure(builder.Configuration);

// Repo + service (these are also inside AddInfrastructure, but keeping for clarity)
builder.Services.AddScoped<ITaskRepo, TaskRepo>();
builder.Services.AddScoped<ITaskService, TaskService>();

// Fix for the exception you got
builder.Services.AddAuthorization();

// Add controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger for development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
