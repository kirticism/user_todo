using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;              // <- This one was missing
using user_todo.Domain.Entities.Model;
using user_todo.Infrastructure.Repositories;
using user_todo.Infrastructure.Data;

namespace user_todo.Infrastructure.Services
{
    public class TaskService : ITaskService
    {
        private readonly UserTodoDbContext _db;

        public TaskService(UserTodoDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<UserTodoModel> createTask(UserTodoModel todo)
        {
            if (todo == null) throw new ArgumentNullException(nameof(todo));
            todo.CreatedAt = DateTime.UtcNow;
            todo.UpdatedAt = null;

            await _db.userTodoModel.AddAsync(todo);
            await SaveChangesAsync();
            return todo;
        }

        public async Task<bool> deleteTask(int id)
        {
            var existing = await _db.userTodoModel.FirstOrDefaultAsync(t => t.Id == id);
            if (existing == null) return false;

            _db.userTodoModel.Remove(existing);
            await SaveChangesAsync();
            return true;
        }

        public async Task<bool> updateTask(UserTodoModel todo)
        {
            if (todo == null) throw new ArgumentNullException(nameof(todo));

            var existing = await _db.userTodoModel.FirstOrDefaultAsync(t => t.Id == todo.Id);
            if (existing == null) return false;

            existing.Title = todo.Title;
            existing.Description = todo.Description;
            existing.Priority = todo.Priority;
            existing.Category = todo.Category;
            existing.IsCompleted = todo.IsCompleted;
            existing.UpdatedAt = DateTime.UtcNow;

            _db.userTodoModel.Update(existing);
            await SaveChangesAsync();
            return true;
        }

        private async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }
    }
}
