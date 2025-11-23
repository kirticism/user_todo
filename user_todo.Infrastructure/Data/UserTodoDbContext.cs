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

				entity.HasKey(e => e.Id);

				entity.Property(e => e.Title)
					.IsRequired()
					.HasMaxLength(200);

				entity.Property(e => e.Description)
					.HasColumnType("nvarchar(max)");

				// Store enums as strings to match NVARCHAR columns and CHECK constraints in DB
				entity.Property(e => e.Priority)
					.HasConversion<string>()
					.HasMaxLength(10)
					.IsRequired();

				entity.Property(e => e.Category)
					.HasConversion<string>()
					.HasMaxLength(50)
					.IsRequired();

				entity.Property(e => e.IsCompleted)
					.HasDefaultValue(false);

				entity.Property(e => e.CreatedAt)
					.HasDefaultValueSql("GETDATE()")
					.ValueGeneratedOnAdd();

				entity.Property(e => e.UpdatedAt)
					.IsRequired(false);
			});
		}
	}
}