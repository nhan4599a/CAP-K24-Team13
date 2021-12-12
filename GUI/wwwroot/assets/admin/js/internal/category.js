$(document).ready(() => {
    var currentPageInfo = getCurrentPageInfo();
    loadCategories(currentPageInfo.pageNumber, currentPageInfo.pageSize);
    let classNames = ['active', 'bg-gradient-primary'];
    $('#nav-item-category').addClass(classNames);
});

function loadCategories(pageNumber, pageSize) {
    let animationLoader = new AnimationLoader('#loading-container', '/assets/admin/img/illustrations/loading.json');
    animationLoader.showAnimation();
    getCategories(pageNumber, pageSize, paginatedData => {
        let categories = paginatedData.data;
        renderCategoryTable(categories);
        renderPagination(pageNumber, paginatedData.maxPageNumber);
        $('a[name=btn-edit]').click(function (e) {
            e.preventDefault();
            let index = parseInt($(this).parent().parent().children('td:nth-child(2)').text()) - 1;
            let categoryInfoStr = JSON.stringify(categories[index]);
            window.localStorage.setItem('editting-category', categoryInfoStr);
            window.location.href = "/admin/category/edit";
        });
        $('a[name=btn-action]').click(function (e) {
            e.preventDefault();
            let action = $(this).text().trim().toLowerCase();
            if (action != 'delete' && action != 'active') {
                alert('something went wrong!');
                return;
            }
            let id = $(this).parent().parent().children('#cate-id').text();
            if (!confirm(`Are you sure to category with id = ${id}`))
                return;
            if (action == 'delete')
                deleteCategory(id,
                    () => {
                        toastr.success(`Deleted category with id = ${id}`, 'Success');
                        $(this).parent().parent().children('td:nth-child(6)').children()
                            .removeClass('bg-gradient-success')
                            .addClass('bg-gradient-secondary')
                            .text('Disabled');
                        $(this).children('span').text(' Active');
                        $(this).children('i').removeClass().addClass('fas fa-check');
                    },
                    () => toastr.error(`Failed to delete category with id = ${id}!`, 'Error')
                );
            else
                activeCategory(id,
                    () => {
                        toastr.success(`Activated category with id = ${id}`, 'Success');
                        $(this).parent().parent().children('td:nth-child(6)').children()
                            .removeClass('bg-gradient-secondary')
                            .addClass('bg-gradient-success')
                            .text('Activated');
                        $(this).children('span').text(' Delete');
                        $(this).children('i').removeClass().addClass('far fa-trash-alt');
                    },
                    () => toastr.error(`Failed to active category with id = ${id}!`, 'Error')
                );
        });

        $('#previous-page').click(() => {
            let currentPageInfo = getCurrentPageInfo();
            moveToPage(currentPageInfo.pageNumber - 1, currentPageInfo.pageSize);
        });
        $('#next-page').click(() => {
            let currentPageInfo = getCurrentPageInfo();
            moveToPage(currentPageInfo.pageNumber + 1, currentPageInfo.pageSize);
        });
        $('a.pagination-item').click(function () {
            let pageNumber = $(this).text();
            moveToPage(pageNumber, getCurrentPageInfo().pageSize);
        });
        animationLoader.hideAnimation();
    });
}

function moveToPage(pageNumber, pageSize) {
    window.location.href = `https://localhost:44349/category?pageNumber=${pageNumber}&pageSize=${pageSize}`;
}

function getCurrentPageInfo() {
    let url = new URL(window.location.href);
    let queryObj = url.searchParams;
    let currentPageNumber = queryObj.get('pageNumber') ? parseInt(queryObj.get('pageNumber')) : 1;
    let currentPageSize = queryObj.get('pageSize') ? parseInt(queryObj.get('pageSize')) : 5;
    return { pageNumber: currentPageNumber, pageSize: currentPageSize };
}