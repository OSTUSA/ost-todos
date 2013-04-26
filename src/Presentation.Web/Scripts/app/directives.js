angular.module('todos.directives', []).
    directive('deleteList', function() {
        return {
            restrict: 'A',
            link: function(scope, elem, attrs, ctrl) {
                elem.on('click', function() {
                    if (confirm('Are you sure?')) {
                        scope.list.$delete(function() {
                            scope.$emit('list:deleted');
                        });
                    }
                });
            }
        };
    }).
    directive('showAddList', function() {
        return {
            restrict: 'A',
            link: function(scope, elem, attrs) {
                var modal = angular.element('#addListModal');
                modal.on('shown', function() {
                    $(this).find('input').focus();
                });
                elem.on('click', function(e) {
                    modal.modal('show');
                });
                elem.scope().$on('list:added', function() {
                    modal.modal('hide');
                    modal.find('input').val('');
                });
            }
        };
    }).
    directive('giveFocus', function() {
        return {
            restrict: 'A',
            link: function(scope, elem, attrs) {
                scope.$watch('list.Todos.length', function(newVal, oldVal) {
                    if (newVal > oldVal)
                        elem.focus();
                });
                elem.focus();
            }
        };
    }).
    directive('brandLink', function() {
        return {
            restrict: 'A',
            link: function (scope, elem, attrs) {
                scope.$watch('isLoggedIn()', function(newVal, oldVal) {
                    if (newVal) {
                        elem.attr('href', '/#lists');
                    } else {
                        elem.attr('href', '/#login');
                    } 
                });
            }
        };
    });