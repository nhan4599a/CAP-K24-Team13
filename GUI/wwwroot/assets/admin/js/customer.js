$(document).ready(() => {
    let currentPageInfo = getCurrentPageInfo();
    loadCustomers(currentPageInfo.keyword, currentPageInfo.pageNumber, currentPageInfo.pageSize);
    $(`#pagesize-select option[value=${currentPageInfo.pageSize}]`).attr('selected', true);
    let searchTextField = $('#input-search');
    if (currentPageInfo.keyword) {
        searchTextField.parent().addClass('is-filled');
        searchTextField.val(currentPageInfo.keyword);
    }
    searchTextField.keypress(function (e) {
        if (e.which == 13) {
            e.preventDefault();
            let keyword = $(this).val().trim();
            let currentPageSize = getCurrentPageInfo().pageSize;
            window.location.href =
                `https://cap-k24-team13.herokuapp.com/admin/customer/index?keyword=${keyword}&pageNumber=1&pageSize=${currentPageSize}`;
        }
    });
    $('#pagesize-select').change(function () {
        let selectedValue = $(this).val();
        let pageSize = parseInt(selectedValue);
        let pageInfo = getCurrentPageInfo();
        moveToPage(pageInfo.keyword, pageInfo.pageNumber, pageSize);
    });
});

function loadCustomers(keyword, pageNumber, pageSize) {
    let animationLoader = new AnimationLoader('#loading-container > #animation-container', '/assets/shop-owner/img/illustrations/loading.json');
    animationLoader.showAnimation(3500);
    getCustomers(keyword, pageNumber, pageSize).then((paginatedData) => {
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
        moveToPage(currentPageInfo.keyword, currentPageInfo.pageNumber - 1, currentPageInfo.pageSize);
    });
    $('#next-page').click((e) => {
        e.preventDefault();
        let currentPageInfo = getCurrentPageInfo();
        moveToPage(currentPageInfo.keyword, currentPageInfo.pageNumber + 1, currentPageInfo.pageSize);
    });
    $('a.pagination-item').click(function (e) {
        e.preventDefault();
        let pageNumber = $(this).text();
        let currentPageInfo = getCurrentPageInfo();
        moveToPage(currentPageInfo.keyword, pageNumber, currentPageInfo.pageSize);
    });

    $('a[name=btn-action]:not(.disabled)').click(function (e) {
        e.preventDefault();
        let eventSource = $(this);
        let userId = eventSource.parent().parent().children().eq(1).children().text().trim();
        if (eventSource.children('span').text().toLowerCase().includes('unban')) {
            let animationLoader = new AnimationLoader('#loading-container > #animation-container', '/assets/shop-owner/img/illustrations/loading.json');
            animationLoader.showAnimation();
            unbanUser(userId)
                .then(() => {
                    animationLoader.hideAnimation();
                    toastr.success('User unban successfully');
                    eventSource.children('span').text(' Ban');
                    eventSource.parent().parent().children().eq(6).html('Available');
                }).catch(error => {
                    animationLoader.hideAnimation();
                    toastr.error(error);
                });
        } else {
            displayBanCustomerDialog(dayCount => {
                displayYesNoQuestion('Are you sure ?', () => {
                    let animationLoader = new AnimationLoader('#loading-container > #animation-container', '/assets/shop-owner/img/illustrations/loading.json');
                    animationLoader.showAnimation();
                    console.log(dayCount);
                    banUser(userId, dayCount)
                        .then(() => {
                            animationLoader.hideAnimation();
                            toastr.success('User ban successfully');
                            eventSource.children('span').text(' Unban');
                            eventSource.parent().parent().children().eq(6).html('Banned');
                        }).catch(error => {
                            animationLoader.hideAnimation();
                            toastr.error(error);
                        });
                });
            });
        }
    });
}

function moveToPage(keyword, pageNumber, pageSize) {
    window.location.href = `https://cap-k24-team13.herokuapp.com/admin/customer/index?keyword=${keyword}&pageNumber=${pageNumber}&pageSize=${pageSize}`;
}

function getCurrentPageInfo() {
    let url = new URL(window.location.href);
    let queryObj = url.searchParams;
    let currentPageNumber = queryObj.get('pageNumber') ? parseInt(queryObj.get('pageNumber')) : 1;
    let currentPageSize = queryObj.get('pageSize') ? parseInt(queryObj.get('pageSize')) : 5;
    let keyword = queryObj.get('keyword') ? queryObj.get('keyword') : undefined;
    return {
        pageNumber: currentPageNumber,
        pageSize: currentPageSize,
        keyword: keyword
    };
}
