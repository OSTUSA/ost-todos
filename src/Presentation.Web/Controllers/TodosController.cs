using System.Net;
using System.Net.Http;
using System.Web.Http;
using Core.Domain.Model;
using Core.Domain.Model.Todos;
using Presentation.Web.Models.Display;
using Presentation.Web.Models.Input;

namespace Presentation.Web.Controllers
{
    public class TodosController : ControllerBase
    {
        private IRepository<Todo> _repo;

        public TodosController(IRepository<Todo> repo)
        {
            _repo = repo;
        }

        [Authorize]
        public HttpResponseMessage Put(long Id, TodoInput todoInput)
        {
            var todo = _repo.Get(Id);
            todo.Completed = todoInput.Completed;
            _repo.Store(todo);
            return Request.CreateResponse(HttpStatusCode.OK, new TodoDisplay() {Id = Id, Title = todo.Title, Completed = todo.Completed});
        }

        [Authorize]
        [HttpDelete]
        public HttpResponseMessage Delete(long Id)
        {
            var todo = _repo.Load(Id);
            _repo.Delete(todo);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}
