using System.Collections.Generic;
using System.Threading.Tasks;
using user_todo.Domain.Entities.Model;
// using user_todo.Infrastructure.Repositories;

namespace user_todo.Infrastructure.Repositories
{
	public interface ITaskRepo
	{
		Task<UserTodoModel?> GetByIdAsync(int id);
		Task<PagedResult<UserTodoModel>> GetAllAsync(int pageNumber = 1, int pageSize = 5);
	}
}
