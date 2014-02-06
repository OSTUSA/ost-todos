angular.module("todos.services", ['ngResource']).
    factory("TodoList", ['$resource', function($resource) {
        return $resource('/api/todolists/:id', { id: '@Id' });
    }]).
    factory("ListTodo", ['$resource', function($resource) {
        return $resource('/api/todolists/:listId/todos');
    }]).
    factory("Todo", ['$resource', function($resource) {
        return $resource('/api/todos/:id', {id:'@Id'}, {
            update: {method:'PUT'}
        });
    }]);