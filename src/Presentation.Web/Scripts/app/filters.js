angular.module("todos.filters", []).
    filter('incomplete', function() {
        return function(todos) {
            return todos.filter(function(todo) {
                return todo.Completed === false;
            });
        };
    }).
    filter('complete', function() {
        return function(todos) {
            return todos.filter(function(todo) {
                return todo.Completed === true;
            });
        };
    });