function renderProductTable(products) {
    if (products.length == 0) {
        $('.table-responsive.p-0').html('<p style="text-align: center">There is no product to show!</p>');
    } else {
        $('.table-responsive.p-0').html(buildProductTableHtml(products));
    }
}

function buildProductTableHtml(products) {
    let tableRowHtml = '';
    products.forEach((product, index) => {
        tableRowHtml += buildProductTableRowHtml(product, index);
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
                            <img src="${getProductImageUrl(product.images[0])}" class="avatar avatar-sm me-3 border-radius-lg" alt="user1">
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
                        ${!product.isDisabled ? 'Activated' : 'Disabled'}
                    </span>
                </td>
                <td class="align-middle">
                    <a href="#" class="text-secondary font-weight-bold text-xs"
                        data-toggle="tooltip" data-original-title="Edit user" style="margin-right: 24px" name="btn-edit">
                        <i class="far fa-edit"></i><span> Edit</span>
                    </a>
                    ${buildDeleteButtonHtml(product.isDisabled)}
                </td>
            </tr>`;
}

function buildDeleteButtonHtml(isDisabled) {
    if (!isDisabled)
        return `<a href="#" class="text-secondary font-weight-bold text-xs" data-toggle="tooltip" data-original-title="Delete product"
                    name="btn-action">
                    <i class="far fa-trash-alt"></i>
                    <span> Delete</span>
                </a>`;
    else
        return `<a href="#" class="text-secondary font-weight-bold text-xs" data-toggle="tooltip" data-original-title="Active product"
                    name="btn-action">
                    <i class="fas fa-check"></i>
                    <span> Active</span>
                </a>`;
}

function renderPagination(currentPageNumber, maxPageNumber) {
    let paginationHtml = '';
    if (currentPageNumber != 1)
        paginationHtml += '<a href="#" id="previous-page">«</a>';
    for (var i = 1; i <= maxPageNumber; i++) {
        if (i == currentPageNumber)
            paginationHtml += `<a class="active">${i}</a>`;
        else
            paginationHtml += `<a href="#" class="pagination-item">${i}</a>`;
    }
    if (currentPageNumber != maxPageNumber)
        paginationHtml += '<a href="#" id="next-page">»</a>';
    $('.pagination').html(paginationHtml);
}

function renderCategoryDropdown(categories) {
    let optionsHtml = '';
    categories.forEach(category => {
        optionsHtml += `<option value="${category.id}">${category.categoryName}</option>`;
    });
    if (optionsHtml != '') {
        $('option[selected=selected]').after(optionsHtml);
    }
}

function renderCategoryTable(categories) {
    if (categories.length == 0) {
        $('.table-responsive.p-0').html('<p style="text-align: center">There is no category to show!</p>');
    } else {
        $('.table-responsive.p-0').html(buildCategoryTableHtml(categories));
    }
}

function buildCategoryTableHtml(categories) {
    let tableRowHtml = '';
    categories.forEach((category, index) => {
        tableRowHtml += buildCategoryTableRowHtml(category, index);
    });
    return `<table class="table align-items-center mb-0">
                <thead>
                    <tr>
                        <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7"
                            style="width: 70px;">
                            #
                        </th>
                        <th class="text-center text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">ID</th>
                        <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 ps-2">
                            Category name
                        </th>
                        <th class="text-center text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">
                            Special
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

function buildCategoryTableRowHtml(category, index) {
    console.log(category);
    return `<tr>
                <td style="display: none" id="cate-id">${category.id}</td>
                <td>
                    <div class="d-flex px-2 py-1">
                        <div class="d-flex flex-column justify-content-center"">
                            <p class="mb-0 text-sm">${index + 1}</p>
                        </div>
                    </div>
                </td>
                <td class="align-middle text-center text-sm">
                    <span class="badge badge-sm bg-gradient-success">${category.id}</span>
                </td>
                <td>
                    <p class="text-xs font-weight-bold mb-0" style="font-size: 50px;">${category.categoryName}</p>
                </td>
                <td class="align-middle text-center">
                    <input type="checkbox" checked="checked">
                    <span class="checkmark"></span>
                </td>
                <td class="align-middle text-center text-sm">
                    <span class="badge badge-sm bg-gradient-${!category.isDisabled ? 'success' : 'secondary'}">
                        ${!category.isDisabled ? 'Activated' : 'Disabled'}
                    </span>
                </td>
                <td class="align-middle">
                    <a href="#" class="text-secondary font-weight-bold text-xs"
                        data-toggle="tooltip" data-original-title="Edit user" style="margin-right: 24px">
                        <i class="far fa-edit"></i><span> Edit</span>
                    </a>
                    ${buildDeleteButtonHtml(category.isDisabled)}
                </td>
            </tr>`;
}

function sortList(field, direction, dataList) {
    if (!Array.isArray(dataList))
        return dataList;
    if (!dataList.every(element => element.hasOwnProperty(field)))
        return dataList;
    if (typeof (direction) !== 'string' || (direction !== 'ASC' && direction !== 'DESC'))
        return dataList;

    // sort
    return [...dataList].sort((first, second) => {
        let firstValue = first[field];
        let secondValue = second[field];

        if (firstValue === secondValue || typeof (firstValue) !== typeof (secondValue) || firstValue === null || secondValue === null)
            return 0;

        if (direction === 'ASC') {
            if (typeof (firstValue) === 'number')
                return firstValue - secondValue;
            else if (typeof (firstValue) === 'string')
                return firstValue.toLowerCase().localeCompare(secondValue.toLowerCase());
            else if (typeof (firstValue) === 'boolean')
                return firstValue ? 1 : -1;
            else
                return 0;

        } else {
            if (typeof (firstValue) === 'number')
                return secondValue - firstValue;
            else if (typeof (firstValue) === 'string')
                return secondValue.toLowerCase().localeCompare(firstValue.toLowerCase());
            else if (typeof (firstValue) === 'boolean')
                return secondValue ? 1 : -1;
            else
                return 0;
        }
    });
}

function clearTable() {
    $('.table-responsive.p-0').html('');
}