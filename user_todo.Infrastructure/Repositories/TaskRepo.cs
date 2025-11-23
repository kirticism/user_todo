using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using user_todo.Domain.Entities.Model;
using user_todo.Infrastructure.Data;

namespace user_todo.Infrastructure.Repositories
{
    public class TaskRepo : ITaskRepo
    {
        private readonly UserTodoDbContext _db;

        public TaskRepo(UserTodoDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task AddAsync(UserTodoModel todo)
        {
            if (todo == null) throw new ArgumentNullException(nameof(todo));
            await _db.userTodoModel.AddAsync(todo);
        }

        public async Task DeleteAsync(UserTodoModel todo)
        {
            if (todo == null) throw new ArgumentNullException(nameof(todo));
            _db.userTodoModel.Remove(todo);
            await Task.CompletedTask;
        }

        public async Task<UserTodoModel?> GetByIdAsync(int id)
        {
            return await _db.userTodoModel.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<(IEnumerable<UserTodoModel> Items, int Total)> GetAllAsync(int pageNumber, int pageSize, string? search = null, string? category = null, string? priority = null)
        {
            var query = _db.userTodoModel.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(t => t.Title.Contains(s) || (t.Description != null && t.Description.Contains(s)));
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(t => t.Category.ToString().Equals(category, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(priority))
            {
                query = query.Where(t => t.Priority.ToString().Equals(priority, StringComparison.OrdinalIgnoreCase));
            }

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((Math.Max(1, pageNumber) - 1) * Math.Max(1, pageSize))
                .Take(Math.Max(1, pageSize))
                .ToListAsync();

            return (items, total);
        }

        public async Task<IEnumerable<UserTodoModel>> GetAllSimpleAsync()
        {
            return await _db.userTodoModel
                .OrderBy(t => t.Title)
                .Select(t => new UserTodoModel { Id = t.Id, Title = t.Title })
                .ToListAsync();
        }

        public async Task UpdateAsync(UserTodoModel todo)
        {
            if (todo == null) throw new ArgumentNullException(nameof(todo));
            _db.userTodoModel.Update(todo);
            await Task.CompletedTask;
        }
    }
}