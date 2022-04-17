$(document).ready(() => {
    var currentPageInfo = getCurrentPageInfo();
    loadCategories(currentPageInfo.pageNumber, currentPageInfo.pageSize);
    $('#input-search').parent().remove();
    $(`#pagesize-select option[value=${currentPageInfo.pageSize}]`).attr('selected', true);
    let classNames = ['active', 'bg-gradient-primary'];
    $('#nav-item-category').addClass(classNames);

    $('#pagesize-select').change(function () {
        let selectedValue = $(this).val();
        let pageSize = parseInt(selectedValue);
        let pageInfo = getCurrentPageInfo();
        moveToPage(pageInfo.pageNumber, pageSize);
    });
});

function loadCategories(pageNumber, pageSize) {
    let animationLoader = new AnimationLoader('#loading-container > #animation-container', '/assets/shop-owner/img/illustrations/loading.json');
    animationLoader.showAnimation();
    getShopId().then(shopId => {
        getCategories(shopId, pageNumber, pageSize).then((paginatedData) => {
            onCategoriesLoaded(paginatedData);
            animationLoader.hideAnimation();
        }).catch(() => {
            animationLoader.hideAnimation();
            toastr.error('Failed to load categories', 'Error');
        });
    });
}

function onCategoriesLoaded(paginatedData) {
    let categories = paginatedData.data;
    renderCategoryTable(categories);
    renderPagination({
        hasPreviousPage: paginatedData.hasPreviousPage,
        hasNextPage: paginatedData.hasNextPage,
        pageNumber: paginatedData.pageNumber,
        maxPageNumber: paginatedData.maxPageNumber
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
    $('a[name=btn-action]').click(function (e) {
        e.preventDefault();
        let categoryId = $(this).parent().parent().children('td:first-child').html();
        location.href = `/shopowner/product/add?category=${categoryId}`;
    });
}

function moveToPage(pageNumber, pageSize) {
    window.location.href = `https://cap-k24-team13.herokuapp.com/shopowner/category?pageNumber=${pageNumber}&pageSize=${pageSize}`;
}

function getCurrentPageInfo() {
    let url = new URL(window.location.href);
    let queryObj = url.searchParams;
    let currentPageNumber = queryObj.get('pageNumber') ? parseInt(queryObj.get('pageNumber')) : 1;
    let currentPageSize = queryObj.get('pageSize') ? parseInt(queryObj.get('pageSize')) : 5;
    return { pageNumber: currentPageNumber, pageSize: currentPageSize };
}