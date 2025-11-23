using Microsoft.EntityFrameworkCore;
using user_todo.Domain.Entities.Model;

namespace user_todo.Infrastructure.Data
{
	public class UserTodoDbContext : DbContext
	{
		public UserTodoDbContext(DbContextOptions<UserTodoDbContext> options) : base(options) { }

		public DbSet<UserTodoModel> userTodoModel { get; set; } = null!;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UserTodoModel>(entity =>
			{
				entity.ToTable("Todos");

				entity.Property(e => e.Priority)
					.HasConversion<string>();

				entity.Property(e => e.Category)
					.HasConversion<string>();
			});

			base.OnModelCreating(modelBuilder);
		}
	}
}