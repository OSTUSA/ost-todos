angular.module("todos.controllers", ["todos.services"]).
    controller("LoginCtrl", ["$scope", "$http", "$location", function($scope, $http, $location) {

        $scope.login = function() {
            $http.post("/user/login", { Email: $scope.Email, Password: $scope.Password }).success(function() {
                $location.path('/lists');
            });
        };

    }]).
    controller("RegisterCtrl", ["$scope", "$http", "$location", function($scope, $http, $location) {

        $scope.register = function() {
            $http.post('/user/register', { Email: $scope.Email, Name: $scope.Name, Password: $scope.Password }).success(function() {
                $location.path('/lists');
            });
        };

    }]).
    controller("ListsCtrl", ["$scope", "TodoList", function($scope, TodoList) {

        var self = this;

        this.setLists = function() {
            TodoList.query(function (lists) {
                var items = [], buffer = [];
                lists.forEach(function(l, i) {
                    buffer.push(l);
                    if ((i + 1) % 2 == 0 && i != 0 || i == lists.length - 1) {
                        items.push({ row: buffer });
                        buffer = [];
                    }
                });
                $scope.lists = items;
            });
        };

        this.setLists();

        $scope.addList = function() {
            var todo = new TodoList({ Name: $scope.Name });
            todo.$save(function() {
                self.setLists();
                $scope.$broadcast('list:added');
            });
        };

    }]).
    controller("ListCtrl", ["$scope", "ListTodo", "Todo", function ($scope, ListTodo, Todo) {
        
        $scope.addTodo = function (title) {
            var todo = new ListTodo({ Title: title });
            todo.$save({ listId: $scope.list.Id }, function () {
                $scope.list.Todos.push(todo);
                $scope.showAddForm = 0;
                $scope.Title = "";
            });
        };

        $scope.completed = function () {
            var count = 0;
            angular.forEach($scope.list.Todos, function(todo) {
                count += todo.Completed ? 1 : 0;
            });
            return count;
        };

        $scope.clearCompleted = function () {
            var todos = [];
            angular.forEach($scope.list.Todos, function(todo) {
                todo.Completed ? Todo.delete({ id: todo.Id }) : todos.push(todo);
            });
            $scope.list.Todos = todos;
        };

    }]).
    controller("TodoCtrl", ["$scope", "Todo", function ($scope, Todo) {

        $scope.markDone = function (todo) {
            todo.Completed = true;
            if (!todo.hasOwnProperty('$update'))
                todo = new Todo(todo);
            todo.$update();
        };

        $scope.markNotDone = function(todo) {
            todo.Completed = false;
            if (!todo.hasOwnProperty('$update'))
                todo = new Todo(todo);
            todo.$update();
        };

    }]);