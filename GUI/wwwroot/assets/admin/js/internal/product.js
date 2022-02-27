$(document).ready(() => {
    let currentPageInfo = getCurrentPageInfo();
    loadProducts(currentPageInfo.keyword, currentPageInfo.pageNumber, currentPageInfo.pageSize);
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
            let urlEncodedKeyword = encodeURIComponent(keyword);
            let currentPageSize = getCurrentPageInfo().pageSize;
            window.location.href =
                `https://cap-k24-team13.herokuapp.com/shopowner/product?keyword=${urlEncodedKeyword}&pageNumber=1&pageSize=${currentPageSize}`;
        }
    });

    $('#pagesize-select').change(function () {
        let selectedValue = $(this).val();
        let pageSize = parseInt(selectedValue);
        let pageInfo = getCurrentPageInfo();
        moveToPage(pageInfo.keyword, pageInfo.pageNumber, pageSize);
    });
});

function loadProducts(keyword, pageNumber, pageSize) {
    let animationLoader = new AnimationLoader('#loading-container > #animation-container', '/assets/admin/img/illustrations/loading.json');
    animationLoader.showAnimation(3500);
    findProducts(keyword, pageNumber, pageSize).then((paginatedData) => {
        onLoadProductsCompleted(paginatedData);
        animationLoader.hideAnimation();
    }).catch(() => {
        animationLoader.hideAnimation();
        toastr.error('Failed to load list of products', 'Error');
    });
}

function onLoadProductsCompleted(paginatedData) {
    let products = paginatedData.data;
    renderProductTable(products);
    renderPagination({
        hasPreviousPage: paginatedData.hasPreviousPage,
        hasNextPage: paginatedData.hasNextPage,
        pageNumber: paginatedData.pageNumber,
        maxPageNumber: paginatedData.maxPageNumber
    });
    $('a[name=btn-edit]').click(function (e) {
        e.preventDefault();
        let index = parseInt($(this).parent().parent().children('td:nth-child(2)').text()) - 1;
        let productInfoStr = JSON.stringify(products[index]);
        window.localStorage.setItem('editting-product', productInfoStr);
        window.location.href = "/shopowner/product/edit";
    });
    $('a[name=btn-action]').click(function (e) {
        e.preventDefault();
        let action = $(this).text().trim().toLowerCase();
        let id = $(this).parent().parent().children('#prod-id').text();
        let name = $(this).parent().parent().children('td:nth-child(3)')
            .children().children('div:nth-child(2)').children('h6').text();
        if (action !== 'deactivate' && action !== 'activate')
            return;
        displayCascadeQuestionDialog(`Do you want to ${action} ${name}`, {
            shouldShowNonCascadeButton: true
        }, () => {
            let isActivateCommand = action === 'activate';
            let successCallback = isActivateCommand ? () => {
                toastr.success(`Activated ${name}`, 'Success');
                $(this).parent().parent().children('td:nth-child(6)').children()
                    .removeClass('bg-gradient-secondary')
                    .addClass('bg-gradient-success')
                    .text('Activated');
                $(this).parent().prepend(buildEditButtonHtml());
                $('a[name=btn-edit]').click(function (e) {
                    e.preventDefault();
                    let index = parseInt($(this).parent().parent().children('td:nth-child(2)').text()) - 1;
                    let productInfoStr = JSON.stringify(products[index]);
                    window.localStorage.setItem('editting-product', productInfoStr);
                    window.location.href = "/shopowner/product/edit";
                });
                $(this).children('span').text(' Deactivate');
                $(this).children('i').removeClass().addClass('far fa-trash-alt');
            } : () => {
                toastr.success(`Deactivated ${name}`, 'Success');
                $(this).parent().parent().children('td:nth-child(6)').children()
                    .removeClass('bg-gradient-success')
                    .addClass('bg-gradient-secondary')
                    .text('deactivated');
                $(this).parent().children('*[name="btn-edit"]').remove();
                $(this).children('span').text(' Activate');
                $(this).children('i').removeClass().addClass('fas fa-check');
            };
            let animationLoader = new AnimationLoader('#loading-container > #animation-container', '/assets/admin/img/illustrations/loading.json');
            animationLoader.showAnimation();
            activateProduct(id, isActivateCommand).then(() => {
                successCallback();
                animationLoader.hideAnimation();
            }).catch(err => {
                animationLoader.hideAnimation();
                toastr.error(`Failed to ${action} ${name}, ${err}`, 'Error')
            });
        });
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
    $('#sort-select').change(function () {
        let sortDirection = $(this).val();
        clearTable();
        renderProductTable(sortList('productName', sortDirection, products));
    });
}

function moveToPage(keyword, pageNumber, pageSize) {
    if (keyword == '')
        window.location.href = `https://cap-k24-team13.herokuapp.com/shopowner/product?pageNumber=${pageNumber}&pageSize=${pageSize}`;
    else
        window.location.href = `https://cap-k24-team13.herokuapp.com/shopowner/product?keyword=${keyword}&pageNumber=${pageNumber}&pageSize=${pageSize}`;
}

function getCurrentPageInfo() {
    let url = new URL(window.location.href);
    let queryObj = url.searchParams;
    let currentPageNumber = queryObj.get('pageNumber') ? parseInt(queryObj.get('pageNumber')) : 1;
    let currentPageSize = queryObj.get('pageSize') ? parseInt(queryObj.get('pageSize')) : 5;
    let currentKeyword = queryObj.get('keyword') ? queryObj.get('keyword') : '';
    return {
        pageNumber: currentPageNumber,
        pageSize: currentPageSize,
        keyword: currentKeyword
    };
}
