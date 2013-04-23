angular.module('todos.directives', []).
    directive('deleteList', function() {
        return {
            restrict: 'A',
            link: function(scope, elem, attrs, ctrl) {
                elem.on('click', function() {
                    if (confirm('Are you sure?')) {
                        scope.list.$delete(function () {
                            scope.$emit('list:deleted');
                            elem.closest('.span6').remove();
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
            link: function (scope, elem, attrs) {
                scope.$watch('list.Todos.length', function(newVal, oldVal) {
                    if (newVal > oldVal)
                        elem.focus();
                });
            }
        };
    }).
    directive('stopEvent', function () {
        return {
            restrict: 'A',
            link: function(scope, element, attr) {
                element.bind(attr.stopEvent, function(e) {
                    e.stopPropagation();
                });
            }
        };
    });