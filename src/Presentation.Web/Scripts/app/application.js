"use strict";

var application = angular.module("todos", ["todos.controllers", "ngCookies", 'todos.directives', 'todos.filters']).
    config(["$routeProvider", function($routeProvider) {
        $routeProvider.when('/lists', { templateUrl: '/Scripts/app/partials/lists.html', controller: 'ListsCtrl' });
        $routeProvider.when('/login', { templateUrl: '/Scripts/app/partials/login.html', controller: 'LoginCtrl' });
        $routeProvider.when('/register', { templateUrl: '/Scripts/app/partials/register.html', controller: 'RegisterCtrl' });
        $routeProvider.otherwise({ redirectTo: '/login' });
    }]).
    config(["$httpProvider", function($httpProvider) {
        var interceptor = ['$rootScope', '$q', function($rootScope, $q) {
            function success(response) {
                return response;
            }
            
            function error(response) {
                if (response.status === 401) {
                    var deferred = $q.defer();
                    $rootScope.$broadcast('event:loginRequired');
                    return deferred.promise;
                }
                return $q.reject(response);
            }

            return function(promise) {
                return promise.then(success, error);
            };
        }];
        $httpProvider.responseInterceptors.push(interceptor);
    }]);

application.run(['$rootScope', '$location', '$http', '$cookies', function ($scope, $location, $http, $cookies) {

    $scope.$on('event:loginRequired', function (event) {
        return $location.path('/login');
    });

    $scope.logout = function() {
        $http.get("/user/logout").success(function () {
            $location.path('/login');
        });
    };

    $scope.isLoggedIn = function() {
        return $cookies.hasOwnProperty(".ASPXAUTH");
    };
}]);
