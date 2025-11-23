using System;
using System.ComponentModel.DataAnnotations;

namespace user_todo.Domain.Entities.Model
{
	public enum PriorityLevel
	{
		High,
		Medium,
		Low
	}

	public enum CategoryType
	{
		Work,
		Personal
	}

	public class UserTodoModel
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(200)]
		public string Title { get; set; } = null!;

		public string? Description { get; set; }

		public PriorityLevel Priority { get; set; }

		public CategoryType Category { get; set; }

		public bool IsCompleted { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }
	}
}
