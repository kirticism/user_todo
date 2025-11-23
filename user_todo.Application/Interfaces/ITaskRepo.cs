using System.Collections.Generic;
using System.Threading.Tasks;
using user_todo.Domain.Entities.Model;

namespace user_todo.Infrastructure.Repositories
{
	public interface ITaskRepo
	{
		Task<(IEnumerable<UserTodoModel> Items, int Total)> GetAllAsync(int pageNumber, int pageSize, string? search = null, string? category = null, string? priority = null);
		Task<UserTodoModel?> GetByIdAsync(int id);
		Task<IEnumerable<UserTodoModel>> GetAllSimpleAsync();
	}
}
