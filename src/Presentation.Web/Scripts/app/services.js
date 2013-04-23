angular.module("todos.services", ['ngResource']).
    factory("TodoList", function($resource) {
        return $resource('/api/todolists/:id', { id: '@Id' });
    }).
    factory("ListTodo", function($resource) {
        return $resource('/api/todolists/:listId/todos');
    }).
    factory("Todo", function($resource) {
        return $resource('/api/todos/:id', {id:'@Id'}, {
            update: {method:'PUT'}
        });
    });