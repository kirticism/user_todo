using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace user_todo.Domain.Entities.Model
{
    public class PagedResult<T>
    {
        public IEnumerable<T> items { get; set; } = new List<T>();
        public int totalCount { get; set; }
    }
}