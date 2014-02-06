using System.Collections.Generic;
using Core.Domain.Model.Todos;
using Core.Domain.Model.Users;

namespace Core.Domain.Model.TodoLists
{
    public class TodoList : EntityBase<TodoList>
    {
        public virtual string Name { get; set; }

        public virtual User Owner { get; set; }

        private ICollection<Todo> todos = new HashSet<Todo>();
        public virtual ICollection<Todo> Todos { get { return todos; } set { todos = value; } }
        
        public virtual Todo AddTodo(Todo todo)
        {
            todo.List = this;
            Todos.Add(todo);
            return todo;
        }
    }
}
