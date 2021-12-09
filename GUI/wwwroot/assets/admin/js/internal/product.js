$(document).ready(() => {
    let currentPageInfo = getCurrentPageInfo();
    loadProducts(currentPageInfo.keyword, currentPageInfo.pageNumber, currentPageInfo.pageSize);
    $(`#pagesize-select option[value=${currentPageInfo.pageSize}]`).attr('selected', true);
    let classNames = ['active', 'bg-gradient-primary'];
    $('#nav-item-product').addClass(classNames);
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
                `https://localhost:44349/admin/product?keyword=${urlEncodedKeyword}&pageNumber=1&pageSize=${currentPageSize}`;
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
    findProducts(keyword, pageNumber, pageSize, paginatedData => {
        let products = paginatedData.data;
        renderProductTable(products);
        renderPagination(pageNumber, paginatedData.maxPageNumber);
        $('a[name=btn-edit]').click(function (e) {
            e.preventDefault();
            let index = parseInt($(this).parent().parent().children('td:nth-child(2)').text()) - 1;
            let productInfoStr = JSON.stringify(products[index]);
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
            let currentPageInfo = getCurrentPageInfo();
            moveToPage(currentPageInfo.keyword, currentPageInfo.pageNumber - 1, currentPageInfo.pageSize);
        });
        $('#next-page').click(() => {
            let currentPageInfo = getCurrentPageInfo();
            moveToPage(currentPageInfo.keyword, currentPageInfo.pageNumber + 1, currentPageInfo.pageSize);
        });
        $('a.pagination-item').click(function () {
            let pageNumber = $(this).text();
            let currentPageInfo = getCurrentPageInfo();
            moveToPage(currentPageInfo.keyword, pageNumber, currentPageInfo.pageSize);
        });
        $('#sort-select').change(function () {
            let sortDirection = $(this).val();
            clearTable();
            renderProductTable(sortList('productName', sortDirection, products));
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
