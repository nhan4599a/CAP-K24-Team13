$(document).ready(() => {
    let currentPageInfo = getCurrentPageInfo();
    loadCustomers(currentPageInfo.pageNumber, currentPageInfo.pageSize);
    $(`#pagesize-select option[value=${currentPageInfo.pageSize}]`).attr('selected', true);

    $('#pagesize-select').change(function () {
        let selectedValue = $(this).val();
        let pageSize = parseInt(selectedValue);
        let pageInfo = getCurrentPageInfo();
        moveToPage(pageInfo.pageNumber, pageSize);
    });
});

function loadCustomers(pageNumber, pageSize) {
    let animationLoader = new AnimationLoader('#loading-container > #animation-container', '/assets/shop-owner/img/illustrations/loading.json');
    animationLoader.showAnimation(3500);
    getCustomers(pageNumber, pageSize).then((paginatedData) => {
        onLoadCustomersCompleted(paginatedData);
        animationLoader.hideAnimation();
    }).catch(() => {
        animationLoader.hideAnimation();
        toastr.error('Failed to load list of customers', 'Error');
    }); 
}

function onLoadCustomersCompleted(paginatedData) {
    let customers = paginatedData.data;
    renderCustomerTable(customers);
    renderPagination({
        hasPreviousPage: paginatedData.hasPreviousPage,
        hasNextPage: paginatedData.hasNextPage,
        pageNumber: paginatedData.pageNumber,
        maxPageNumber: paginatedData.maxPageNumber
    });
    $('#previous-page').click((e) => {
        e.preventDefault();
        let currentPageInfo = getCurrentPageInfo();
        moveToPage(currentPageInfo.pageNumber - 1, currentPageInfo.pageSize);
    });
    $('#next-page').click((e) => {
        e.preventDefault();
        let currentPageInfo = getCurrentPageInfo();
        moveToPage(currentPageInfo.pageNumber + 1, currentPageInfo.pageSize);
    });
    $('a.pagination-item').click(function (e) {
        e.preventDefault();
        let pageNumber = $(this).text();
        let currentPageInfo = getCurrentPageInfo();
        moveToPage(pageNumber, currentPageInfo.pageSize);
    });

    $('a[name=btn-action]').click(function (e) {
        e.preventDefault();
        let eventSource = $(this);
        let userId = eventSource.parent().parent().children().eq(1).children().text().trim();
        let animationLoader = new AnimationLoader('#loading-container > #animation-container', '/assets/shop-owner/img/illustrations/loading.json');
        animationLoader.showAnimation();
        unbanUser(userId)
            .then(() => {
                animationLoader.hideAnimation();
                toastr.success('User unban successfully');
                eventSource.parent().remove();
            }).catch(error => {
                animationLoader.hideAnimation();
                toastr.error(error);
            });
    });
}

function moveToPage(pageNumber, pageSize) {
    window.location.href = `https://cap-k24-team13.herokuapp.com/admin/customer/index?pageNumber=${pageNumber}&pageSize=${pageSize}`;
}

function getCurrentPageInfo() {
    let url = new URL(window.location.href);
    let queryObj = url.searchParams;
    let currentPageNumber = queryObj.get('pageNumber') ? parseInt(queryObj.get('pageNumber')) : 1;
    let currentPageSize = queryObj.get('pageSize') ? parseInt(queryObj.get('pageSize')) : 5;
    return {
        pageNumber: currentPageNumber,
        pageSize: currentPageSize
    };
}
