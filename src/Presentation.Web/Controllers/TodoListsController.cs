using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Core.Domain.Model;
using Core.Domain.Model.TodoLists;
using Core.Domain.Model.Todos;
using Presentation.Web.Models.Display;
using Presentation.Web.Models.Input;

namespace Presentation.Web.Controllers
{
    public class TodoListsController : ControllerBase
    {
        private IRepository<TodoList> _repo;

        public TodoListsController(IRepository<TodoList> repo)
        {
            _repo = repo;
        }

        [Authorize]
        public IEnumerable<TodoListDisplay> Get()
        {
            var todos = _repo.FindBy(x => x.Owner == LoadUser());
            var displays = todos.Select(x => new TodoListDisplay()
                {
                    Name = x.Name,
                    Id = x.Id,
                    Todos = x.Todos.Select(t => new TodoDisplay() { Id = t.Id, Title = t.Title, Completed = t.Completed }).ToList()
                }).ToList();
            return displays;
        }

        [Authorize]
        [HttpPost]
        public HttpResponseMessage Post(TodoListInput list)
        {
            var entity = new TodoList()
                {
                    Name = list.Name,
                    Owner = LoadUser()
                };
            _repo.Store(entity);
            return Request.CreateResponse(HttpStatusCode.OK, new TodoListDisplay() { Name = entity.Name, Id = entity.Id });
        }

        [Authorize]
        [HttpDelete]
        public HttpResponseMessage Delete(long Id)
        {
            var list = _repo.Load(Id);
            _repo.Delete(list);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [Authorize]
        [HttpPost]
        public HttpResponseMessage Todos(long Id, TodoInput todoInput)
        {
            var todo = new Todo() { Title = todoInput.Title, Completed = false };
            var list = _repo.Get(Id);
            list.AddTodo(todo);
            _repo.Store(list);
            return Request.CreateResponse(HttpStatusCode.OK, new TodoDisplay { Title = todoInput.Title, Id = todo.Id, Completed = false });
        }

        [Authorize]
        [HttpGet]
        public IEnumerable<TodoDisplay> Todos(long Id)
        {
            var list = _repo.Get(Id);
            return
                list.Todos.Select(t => new TodoDisplay() { Id = t.Id, Title = t.Title, Completed = t.Completed }).ToList();
            ;
        }
    }
}