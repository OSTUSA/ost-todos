using System.Collections.Generic;

namespace Presentation.Web.Models.Display
{
    public class TodoListDisplay
    {
        public string Name { get; set; }

        public long Id { get; set; }

        public List<TodoDisplay> Todos { get; set; } 
    }
}