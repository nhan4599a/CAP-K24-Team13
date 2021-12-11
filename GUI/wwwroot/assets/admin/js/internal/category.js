$(document).ready(() => {
    var currentPageInfo = getCurrentPageInfo();
    loadCategories(currentPageInfo.pageNumber, currentPageInfo.pageSize);
    let classNames = ['active'];
    $('#nav-item-category').addClass(classNames).css('background-color', '#2f9db6');
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
        $('a[name=btn-delete]').click(function () {
            let id = $(this).parent().parent().children('#cate-id').text();
            deleteCategory(id,
                () => toastr.error(`Failed to delete category with id = ${id}, ${res.errorMessage}!`, 'Error'),
                () => toastr.success(`Deleted category with id = ${id}`, 'Success')
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