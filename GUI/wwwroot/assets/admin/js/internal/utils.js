function renderProductTable(products) {
    if (products.length == 0) {
        $('.table-responsive.p-0').html('<p style="text-align: center">There is no product to show!</p>');
    } else {
        $('.table-responsive.p-0').html(buildProductTableHtml(products));
    }
}

function buildProductTableHtml(products) {
    var tableRowHtml = '';
    products.forEach((element, index) => {
        tableRowHtml += buildProductTableRowHtml(element, index);
    });
    return `<table class="table align-items-center mb-0">
                <thead>
                    <tr>
                        <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7"
                            style="width: 50px; min-width: 50px !important;">
                            #
                        </th>
                        <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 ps-2"
                            style="padding-left: 24px!important">
                            Name
                        </th>
                        <th class="text-center text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">
                            Price
                        </th>
                        <th class="text-center text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">
                            Status
                        </th>
                        <th class="text-secondary opacity-7">Action</th>
                    </tr>
                </thead>
                <tbody>
                    ${tableRowHtml}
                </tbody>
            </table>`;
}

function buildProductTableRowHtml(product, index) {
    return `<tr>
                <td style="display: none" id="prod-id">${product.id}</td>
                <td class="align-middle text-center" style="padding: 0;">${index + 1}</td>
                <td class="align-middle text-center">
                    <div class="d-flex px-2 py-1">
                        <div>
                            <img src="/assets/admin/img/team-2.jpg" class="avatar avatar-sm me-3 border-radius-lg" alt="user1">
                        </div>
                        <div class="d-flex flex-column">
                            <h6 class="mb-0 text-sm">${product.productName}</h6>
                            <p class="text-xs text-secondary mb-0" style="text-align: left">${product.categoryName}</p>
                        </div>
                    </div>
                </td>
                <td class="align-middle text-center">
                    <span class="text-secondary text-xs font-weight-bold">${product.price}đ</span>
                </td>
                <td class="align-middle text-center text-sm">
                    <span class="badge badge-sm bg-gradient-${!product.isDisabled ? 'success' : 'secondary'}">
                        ${!product.isDisabled ? 'Active' : 'Disabled'}
                    </span>
                </td>
                <td class="align-middle">
                    <a href="#" class="text-secondary font-weight-bold text-xs"
                        data-toggle="tooltip" data-original-title="Edit user" style="margin-right: 24px" name="btn-edit">
                        <i class="far fa-edit"></i><span> Edit</span>
                    </a>
                    ${buildDeleteButtonHtmlForProduct(product)}
                </td>
            </tr>`;
}

function buildDeleteButtonHtmlForProduct(product) {
    if (!product.isDisabled)
        return `<a href="#" class="text-secondary font-weight-bold text-xs" data-toggle="tooltip" data-original-title="Delete user"
                    name="btn-delete">
                    <i class="far fa-trash-alt"></i>
                    <span> Delete</span>
                </a>`;
    else
        return `<a href="#" class="text-secondary font-weight-bold text-xs" data-toggle="tooltip" data-original-title="Delete user"
                    name="btn-delete">
                    <i class="fas fa-check"></i>
                    <span> Enable</span>
                </a>`;
}

function renderPagination(currentPageNumber, maxPageNumber) {
    var paginationHtml = '';
    if (currentPageNumber != 1)
        paginationHtml += '<a href="#" id="previous-page">«</a>';
    for (var i = 1; i <= maxPageNumber; i++) {
        if (i == currentPageNumber)
            paginationHtml += `<a class="active">${i}</a>`;
        else
            paginationHtml += `<a class="pagination-item">${i}</a>`;
    }
    if (currentPageNumber != maxPageNumber)
        paginationHtml += '<a href="#" id="next-page">»</a>';
    $('.pagination').html(paginationHtml);
}

function renderCategoryDropdown(categories) {
    var optionsHtml = '';
    categories.forEach(category => {
        optionsHtml += `<option value="${category.id}">${category.categoryName}</option>`;
    });
    if (optionsHtml != '') {
        $('option[selected=selected]').after(optionsHtml);
    }
}