angular.module('todos.directives', []).
    directive('deleteList', function() {
        return {
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
            link: function(scope, elem, attrs) {
                scope.$watch('showAddForm', function(val) {
                    if (val == 0 || typeof val === "undefined") {
                        elem.focus();
                    } else {
                        elem.blur();
                    }
                });
            }
        };
    });