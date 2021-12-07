$(document).ready(() => {
    var currentPageInfo = getCurrentPageInfo();
    loadProducts(currentPageInfo.keyword, currentPageInfo.pageNumber, currentPageInfo.pageSize);
    $(`#pagesize-select option[value=${currentPageInfo.pageSize}]`).attr('selected', true);
    let classNames = ['active', 'bg-gradient-primary'];
    $('#nav-item-product').addClass(classNames);
    var searchTextField = $('#input-search');
    if (currentPageInfo.keyword) {
        searchTextField.parent().addClass('is-filled');
        searchTextField.val(currentPageInfo.keyword);
    }
    searchTextField.keypress(function (e) {
        if (e.which == 13) {
            e.preventDefault();
            var keyword = $(this).val().trim();
            var urlEncodedKeyword = encodeURIComponent(keyword);
            var currentPageSize = getCurrentPageInfo().pageSize;
            window.location.href =
                `https://localhost:44349/admin/product?keyword=${urlEncodedKeyword}&pageNumber=1&pageSize=${currentPageSize}`;
        }
    });
});

function loadProducts(keyword, pageNumber, pageSize) {
    findProducts(keyword, pageNumber, pageSize, paginatedData => {
        var products = paginatedData.data;
        renderProductTable(products);
        renderPagination(pageNumber, paginatedData.maxPageNumber);
        $('a[name=btn-edit]').click(function (e) {
            e.preventDefault();
            var index = parseInt($(this).parent().parent().children('td:nth-child(2)').text()) - 1;
            var productInfoStr = JSON.stringify(products[index]);
            window.localStorage.setItem('editting-product', productInfoStr);
            window.location.href = "/admin/product/edit";
        });
        $('a[name=btn-delete]').click(function (e) {
            let id = $(this).parent().parent().children('#prod-id').text();
            if (!confirm(`Are you sure to product with id = ${id}`))
                return;
            e.preventDefault();
            deleteProduct(id, () => {
                toastr.success(`Deleted product with id = ${id}`, 'Success');
                $(this).parent().parent().children('td:nth-child(5)').children()
                    .removeClass('bg-gradient-success')
                    .addClass('bg-gradient-secondary')
                    .text('Disabled');
                $(this).remove();
            }, () => toastr.error(`Failed to delete product with id = ${id}, ${res.errorMessage}!`, 'Error'));
        });
        $('#previous-page').click(() => {
            var currentPageInfo = getCurrentPageInfo();
            moveToPage(currentPageInfo.keyword, currentPageInfo.pageNumber - 1, currentPageInfo.pageSize);
        });
        $('#next-page').click(() => {
            var currentPageInfo = getCurrentPageInfo();
            moveToPage(currentPageInfo.keyword, currentPageInfo.pageNumber + 1, currentPageInfo.pageSize);
        });
        $('a.pagination-item').click(function () {
            var pageNumber = $(this).text();
            var currentPageInfo = getCurrentPageInfo();
            moveToPage(currentPageInfo.keyword, pageNumber, currentPageInfo.pageSize);
        });
        $('#pagesize-select').change(function () {
            var checkedValue = $(this).val();
            var pageSize = parseInt(checkedValue);
            var pageInfo = getCurrentPageInfo();
            moveToPage(pageInfo.keyword, pageInfo.pageNumber, pageSize);
        });
    });
}

function moveToPage(keyword, pageNumber, pageSize) {
    if (keyword == '')
        window.location.href = `https://localhost:44349/admin/product?pageNumber=${pageNumber}&pageSize=${pageSize}`;
    else
        window.location.href = `https://localhost:44349/admin/product?keyword=${keyword}&pageNumber=${pageNumber}&pageSize=${pageSize}`;
}

function getCurrentPageInfo() {
    var url = new URL(window.location.href);
    var queryObj = url.searchParams;
    var currentPageNumber = queryObj.get('pageNumber') ? parseInt(queryObj.get('pageNumber')) : 1;
    var currentPageSize = queryObj.get('pageSize') ? parseInt(queryObj.get('pageSize')) : 5;
    var currentKeyword = queryObj.get('keyword') ? queryObj.get('keyword') : '';
    return {
        pageNumber: currentPageNumber,
        pageSize: currentPageSize,
        keyword: currentKeyword
    };
}
