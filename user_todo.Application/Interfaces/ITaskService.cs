using System.Collections.Generic;
using System.Threading.Tasks;
using user_todo.Domain.Entities.Model;

namespace user_todo.Infrastructure.Services
{
	public interface ITaskService
	{
		Task<UserTodoModel> createTask(UserTodoModel todo);
		Task<bool> updateTask(UserTodoModel todo);
		Task<bool> deleteTask(int id);
	}
}
