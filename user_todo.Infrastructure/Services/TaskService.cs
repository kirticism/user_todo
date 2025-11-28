using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        //CREATE & UPDATE TODO TASK
        public async Task<UserTodoModel> createTask(UserTodoModel todo)
        {
            if (todo == null)
                throw new ArgumentNullException(nameof(todo));

            try
            {
                // bool titleExists = await _db.userTodoModel
                //     .AnyAsync(t => t.Title == todo.Title && t.Id != todo.Id);

                // if (titleExists)
                //     throw new Exception($"A task with the title '{todo.Title}' already exists.");

                if (todo.Id != 0)
                {
                    var existing = await _db.userTodoModel.FirstOrDefaultAsync(t => t.Id == todo.Id);

                    if (existing == null)
                        throw new Exception($"Task with id {todo.Id} does not exists");

                    existing.Title = todo.Title;
                    existing.Description = todo.Description;
                    existing.Priority = todo.Priority;
                    existing.Category = todo.Category;
                    existing.IsCompleted = todo.IsCompleted;
                    existing.UpdatedAt = DateTime.UtcNow;

                    await _db.SaveChangesAsync();
                    return existing;
                }

                todo.CreatedAt = DateTime.UtcNow;
                todo.UpdatedAt = null;

                await _db.userTodoModel.AddAsync(todo);
                await _db.SaveChangesAsync();

                return todo;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to save task: " + ex.Message);
            }
        }

        //DELETE TASK
        public async Task<bool> deleteTask(int id)
        {
            try
            {
                var existing = await _db.userTodoModel.FirstOrDefaultAsync(t => t.Id == id);

                if (existing == null)
                    throw new Exception($"Task with ID {id} does not exists");

                _db.userTodoModel.Remove(existing);
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete task: " + ex.Message);
            }
        }
    }
}
