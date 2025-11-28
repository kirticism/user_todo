using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using user_todo.Domain.Entities.Model;
using user_todo.Infrastructure.Data;
using user_todo.Domain.Entities.Enum;

namespace user_todo.Infrastructure.Repositories
{
    public class TaskRepo : ITaskRepo
    {
        private readonly UserTodoDbContext _db;

        public TaskRepo(UserTodoDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        //GET TASK BY ID
        public async Task<UserTodoModel?> GetByIdAsync(int id)
        {
            return await _db.userTodoModel.FirstOrDefaultAsync(t => t.Id == id);
        }
        //GET ALL RECORDS OF TASK
        public async Task<PagedResult<UserTodoModel>> GetAllAsync(int pageNumber = 1, int pageSize = 5)
        {
            var query = _db.userTodoModel.AsQueryable();

            query = query.OrderBy(t => t.IsCompleted)
                        .ThenByDescending(t => t.Priority == PriorityLevel.High ? 3 :
                                                t.Priority == PriorityLevel.Medium ? 2 : 1);
            var total = await query.CountAsync();

            var items = await query
                .Skip((Math.Max(1, pageNumber) - 1) * Math.Max(1, pageSize))
                .Take(Math.Max(1, pageSize))
                .ToListAsync();

            return new PagedResult<UserTodoModel>
            {
                items = items,
                totalCount = total
            };
        }
    }
}