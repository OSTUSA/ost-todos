﻿using System.Collections.Generic;
using Core.Domain.Model.Todos;
using Core.Domain.Model.Users;

namespace Core.Domain.Model.TodoLists
{
    public class TodoList : EntityBase<TodoList>
    {
        public virtual string Name { get; set; }

        public virtual User Owner { get; set; }

        public virtual ICollection<Todo> Todos { get; set; } 

        public virtual void AddTodo(Todo todo)
        {
            todo.List = this;
            Todos.Add(todo);
        }
    }
}
