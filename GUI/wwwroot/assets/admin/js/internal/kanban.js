let dragContainer = document.querySelector('.drag-container');
let itemContainers = [].slice.call(document.querySelectorAll('.board-column-content'));
let columnGrids = [];
let boardGrid;
let currentGrid = null;
let oldPosition = null;
// Init the column grids so we can drag those items around.
itemContainers.forEach(function (container) {
    let grid = new Muuri(container, {
        items: '.board-item',
        dragEnabled: true,
        dragSort: function () {
            return columnGrids;
        },
        dragContainer: dragContainer,
        dragAutoScroll: {
            targets: (item) => {
                return [
                    { element: window, priority: 0 },
                    { element: item.getGrid().getElement().parentNode, priority: 1 },
                ];
            }
        },
    })
        .on('dragInit', function (item) {
            item.getElement().style.width = item.getWidth() + 'px';
            item.getElement().style.height = item.getHeight() + 'px';
        })
        .on('dragReleaseEnd', function (item) {
            item.getElement().style.width = '';
            item.getElement().style.height = '';
            item.getGrid().refreshItems([item]);
        })
        .on('layoutStart', function () {
            boardGrid.refreshItems().layout();
        })
        .on('dragStart', function (item) {
            console.log(item);
            console.log(item.getGrid());
            currentGrid = item.getGrid();
        })
        .on('dragEnd', function (item) {
            let destinationGrid = item.getGrid();
            if (currentGrid == destinationGrid)
                return;
            let newStatus = $(destinationGrid._element).parent().parent().data('status');
            let orderId = $(item._element).children().children().children('input:first-child').val();
            changeOrderStatus(orderId, newStatus)
                .then(() => {
                    currentGrid = destinationGrid;
                    toastr.success('Change status successfully');
                })
                .catch(error => {
                    destinationGrid.send(item, currentGrid, currentGrid.getItems().length - 1, {
                        appendTo: dragContainer
                    });
                    toastr.error(error);
                });
        });

    columnGrids.push(grid);
});

// Init board grid so we can drag those columns around.
boardGrid = new Muuri('.board', {
    dragEnabled: false,
    dragHandle: '.board-column-header'
});