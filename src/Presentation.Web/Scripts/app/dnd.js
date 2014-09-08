function dragAndDrop() {
    //alert("lists page loaded");
    var todoItems = document.querySelectorAll('.todos .ng-scope');
    [].forEach.call(todoItems, function (todoItem) {
        col.setAttribute('draggable', 'true');
        //todoItem.addEventListener('dragstart', handleDragStart, false);
    });
}




function handleDragStart(e) {
    dragSrcEl = e.target;
    e.dataTransfer.effectAllowed = 'move';
    e.dataTransfer.setData('text/html', e.target.innerHTML);
}

function handleDragOver(e) {
    if (e.preventDefault) {
        e.preventDefault(); // Necessary. Allows us to drop.
    }
    e.dataTransfer.dropEffect = 'move';
    return false;
}

//function handleDragEnter(e) {
//    //e.target.classList.add('over');
//}

//function handleDragLeave(e) {
//    //e.target.classList.remove('over');
//}

function handleDrop(e) {
    if (e.stopPropagation) {
        e.stopPropagation();
    }

    // Prevent labels from being dragged into checkboxes
    if (dragSrcEl != e.target && e.target.tagName != "INPUT") {
        dragSrcEl.innerHTML = e.target.innerHTML;
        e.target.innerHTML = e.dataTransfer.getData('text/html');
    }
    var x = $(e.target).index();

    //alert(e.target + " - " + x);

    return false;
}

//function handleDragEnd(e) {
//    var todoItems = document.querySelectorAll('.todos .ng-scope');
//    [].forEach.call(todoItems, function (todoItem) {
//        //todoItem.classList.remove('over');
//    });
//}