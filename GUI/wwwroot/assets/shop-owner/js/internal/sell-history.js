$(document).ready(() => {
    let currentPageInfo = getCurrentPageInfo();
    loadInvoices(currentPageInfo.key, currentPageInfo.value, currentPageInfo.pageNumber, currentPageInfo.pageSize);
    $(`#pagesize-select option[value=${currentPageInfo.pageSize}]`).attr('selected', true);
    let searchTextField = $('#input-search');
    if (currentPageInfo.keyword) {
        searchTextField.parent().addClass('is-filled');
        searchTextField.val(currentPageInfo.keyword);
    }
    searchTextField.keypress(function (e) {
        if (e.which == 13) {
            e.preventDefault();
            let value = $(this).val().trim();
            let urlEncodedValue = encodeURIComponent(value);
            let currentPageSize = getCurrentPageInfo().pageSize;
            window.location.href =
                `https://cap-k24-team13.herokuapp.com/shopowner/order/sell-history?key=${urlEncodedValue}&value=${urlEncodedValue}&pageNumber=1&pageSize=${currentPageSize}`;
        }
    });

    $('#pagesize-select').change(function () {
        let selectedValue = $(this).val();
        let pageSize = parseInt(selectedValue);
        let pageInfo = getCurrentPageInfo();
        moveToPage(pageInfo.key, pageInfo.value, pageInfo.pageNumber, pageSize);
    });
});

function loadInvoices(key, value, pageNumber, pageSize) {
    let animationLoader = new AnimationLoader('#loading-container > #animation-container', '/assets/shop-owner/img/illustrations/loading.json');
    animationLoader.showAnimation(3500);
    getShopId().then(shopId => {
        findInvoices(shopId, key, value, pageNumber, pageSize).then(paginatedData => {
            animationLoader.hideAnimation();
            onLoadInvoicesCompleted(paginatedData);
        }).catch(() => {
            animationLoader.hideAnimation();
            toastr.error('Failed to load list of products', 'Error');
        })
    });
}

function onLoadInvoicesCompleted(paginatedData) {
    let invoices = paginatedData.data;
    renderInvoiceTable(invoices);
    renderPagination({
        hasPreviousPage: paginatedData.hasPreviousPage,
        hasNextPage: paginatedData.hasNextPage,
        pageNumber: paginatedData.pageNumber,
        maxPageNumber: paginatedData.maxPageNumber
    });
    $('#previous-page').click((e) => {
        e.preventDefault();
        let currentPageInfo = getCurrentPageInfo();
        moveToPage(currentPageInfo.key, currentPageInfo.value, currentPageInfo.pageNumber - 1, currentPageInfo.pageSize);
    });
    $('#next-page').click((e) => {
        e.preventDefault();
        let currentPageInfo = getCurrentPageInfo();
        moveToPage(currentPageInfo.key, currentPageInfo.value, currentPageInfo.pageNumber + 1, currentPageInfo.pageSize);
    });
    $('a.pagination-item').click(function (e) {
        e.preventDefault();
        let pageNumber = $(this).text();
        let currentPageInfo = getCurrentPageInfo();
        moveToPage(currentPageInfo.key, currentPageInfo.value, pageNumber, currentPageInfo.pageSize);
    });
    $('a[name=btn-action]').click(function (e) {
        e.preventDefault();
        let invoiceCode = $(this).parent().parent().children('td').eq(1).text();
        let animationLoader = new AnimationLoader('#loading-container > #animation-container', '/assets/shop-owner/img/illustrations/loading.json');
        animationLoader.showAnimation();
        $(this).css('display', 'none');
        getUserId().then(userId => {
            reportInvoice(invoiceCode, userId).then(() => {
                toastr.success('Report was sent to admin');
                animationLoader.hideAnimation();
                $(this).remove();
            }).catch(error => {
                toastr.error(error);
                animationLoader.hideAnimation();
                $(this).css('display', 'initial');
            });
        });
    });
}

function moveToPage(key, value, pageNumber, pageSize) {
    if (!key)
        window.location.href = `https://cap-k24-team13.herokuapp.com/shopowner/order/sell-history?pageNumber=${pageNumber}&pageSize=${pageSize}`;
    else
        window.location.href = `https://cap-k24-team13.herokuapp.com/shopowner/order/sell-history?key=${key}&value=${value}&pageNumber=${pageNumber}&pageSize=${pageSize}`;
}

function getCurrentPageInfo() {
    let url = new URL(window.location.href);
    let queryObj = url.searchParams;
    let currentPageNumber = queryObj.get('pageNumber') ? parseInt(queryObj.get('pageNumber')) : 1;
    let currentPageSize = queryObj.get('pageSize') ? parseInt(queryObj.get('pageSize')) : 5;
    let currentKey = queryObj.get('key') ? queryObj.get('key') : null;
    let currentValue = queryObj.get('value') ? queryObj.get('value') : null;
    return {
        pageNumber: currentPageNumber,
        pageSize: currentPageSize,
        key: currentKey,
        value: currentValue
    };
}
