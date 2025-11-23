using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using user_todo.Infrastructure.Data;
using user_todo.Infrastructure.Repositories;
using user_todo.Infrastructure.Services;
using user_todo.Infrastructure;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// DB context
var connectionString = builder.Configuration.GetConnectionString("constring");
builder.Services.AddDbContext<UserTodoDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// Centralized infrastructure DI registration (DbContext, repos, services)
builder.Services.AddInfrastructure(builder.Configuration);

// Register controllers and configure JSON to accept enum strings
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Fix for the exception you got
builder.Services.AddAuthorization();

// Add Swagger
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
