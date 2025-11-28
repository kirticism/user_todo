using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using user_todo.Infrastructure.Data;
using user_todo.Infrastructure.Repositories;
using user_todo.Infrastructure.Services;
using user_todo.Domain.Entities.Model;


namespace user_todo.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("constring");
			services.AddDbContext<UserTodoDbContext>(options =>
			{
				options.UseSqlServer(connectionString);
			});

			services.AddScoped<ITaskRepo, TaskRepo>();
			services.AddScoped<ITaskService, TaskService>();
			return services;
		}
	}
}
