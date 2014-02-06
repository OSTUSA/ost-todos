using System.Collections.Generic;
using Core.Domain.Model.TodoLists;
using DevOne.Security.Cryptography.BCrypt;

namespace Core.Domain.Model.Users
{
    public class User : EntityBase<User>
    {
        public virtual string Email { get; set; }

        public virtual string Name { get; set; }

        public virtual string Password { get; set; }

        private ICollection<TodoList> lists = new HashSet<TodoList>();
        public virtual ICollection<TodoList> Lists { get { return lists; } set { lists = value; } }
        
        public virtual TodoList AddList(TodoList list)
        {
            list.Owner = this;
            Lists.Add(list);
            return list;
        }

        public virtual void HashPassword()
        {
            var salt = BCryptHelper.GenerateSalt(10);
            Password = BCryptHelper.HashPassword(Password, salt);
        }

        public virtual bool IsAuthenticated(string password)
        {
            return BCryptHelper.CheckPassword(password, Password);
        }
    }
}
