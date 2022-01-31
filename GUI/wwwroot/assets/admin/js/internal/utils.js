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
                            Quantity
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
                    <span class="text-secondary text-xs font-weight-bold">${product.quantity}</span>
                </td>
                <td class="align-middle text-center">
                    <span class="text-secondary text-xs font-weight-bold">${product.price}đ</span>
                </td>
                <td class="align-middle text-center text-sm">
                    <span class="badge badge-sm bg-gradient-${!product.isDisabled ? 'success' : 'secondary'}">
                        ${!product.isDisabled ? 'Activated' : 'Deactivated'}
                    </span>
                </td>
                <td class="align-middle">
                    ${buildActionButtonHtml(product.isDisabled)}
                </td>
            </tr>`;
}

function buildActionButtonHtml(isDisabled) {
    if (!isDisabled)
        return `
                ${buildEditButtonHtml()}
                <a href="#" class="text-secondary font-weight-bold text-xs" data-toggle="tooltip"
                    name="btn-action">
                    <i class="far fa-trash-alt"></i>
                    <span> Deactivate</span>
                </a>`;
    else
        return `<a href="#" class="text-secondary font-weight-bold text-xs" data-toggle="tooltip"
                    name="btn-action">
                    <i class="fas fa-check"></i>
                    <span> Activate</span>
                </a>`;
}

function buildEditButtonHtml() {
    return `<a href="#" class="text-secondary font-weight-bold text-xs"
                data-toggle="tooltip" data-original-title="Edit user" style="margin-right: 24px" name="btn-edit">
                <i class="far fa-edit"></i><span> Edit</span>
            </a>`;
}

function renderPagination(paginationObject) {
    let paginationHtml = '';
    if (paginationObject.hasPreviousPage)
        paginationHtml += '<a href="#" id="previous-page">«</a>';
    for (let i = 1; i <= paginationObject.maxPageNumber; i++) {
        if (i == paginationObject.pageNumber)
            paginationHtml += `<a class="active">${i}</a>`;
        else
            paginationHtml += `<a href="#" class="pagination-item">${i}</a>`;
    }
    if (paginationObject.hasNextPage)
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
                            style="width: 50px; min-width: 50px !important;">
                            #
                        </th>
                        <th class="text-center text-uppercase text-secondary text-xxs font-weight-bolder opacity-7"
                            style="padding-left: 24px!important">
                            ID
                        </th>
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
    return `<tr>
                <td style="display: none" id="cate-id">${category.id}</td>
                <td class="align-middle text-center">${index + 1}</td>
                <td class="align-middle text-center text-sm">
                    <span class="badge badge-sm bg-gradient-success">${category.id}</span>
                </td>
                <td class="align-middle text-center">
                    <div class="d-flex px-2 py-1">
                        <div>
                            <img src="${getCategoryImageUrl(category.image)}?${Date.now() / 1000}" class="avatar avatar-sm me-3 border-radius-lg">
                        </div>
                        <div class="d-grid flex-column" style="align-self: center">
                            <h6 class="mb-0 text-sm">${category.categoryName}</h6>
                        </div>
                    </div>
                </td>
                <td class="align-middle text-center">
                    <input type="checkbox" checked="checked">
                    <span class="checkmark"></span>
                </td>
                <td class="align-middle text-center text-sm">
                    <span class="badge badge-sm bg-gradient-${!category.isDisabled ? 'success' : 'secondary'}">
                        ${!category.isDisabled ? 'Activated' : 'Deactivated'}
                    </span>
                </td>
                <td class="align-middle">
                    ${buildActionButtonHtml(category.isDisabled)}
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

function displayCascadeQuestionDialog(question, buttonOption = {}, confirmedCallback) {
    if (!buttonOption.cascadeButtonText)
        buttonOption.cascadeButtonText = 'Yes and cascade';
    if (!buttonOption.nonCascadeButtonText)
        buttonOption.nonCascadeButtonText = "Non cascade";
    if (!buttonOption.cancelButtonText)
        buttonOption.cancelButtonText = 'Cancel';
    if (buttonOption.shouldShowCascadeButton === null || buttonOption.shouldShowCascadeButton === undefined)
        buttonOption.shouldShowCascadeButton = false;
    if (buttonOption.shouldShowNonCascadeButton === null || buttonOption.shouldShowNonCascadeButton === undefined)
        buttonOption.shouldShowNonCascadeButton = false;
    if (!question)
        throw new Error('question must have a value');
    if (!typeof confirmedCallback === 'function')
        throw new Error('confirmedCallback must be a function');
    var modalHtml = `<div class="modal fade" id="question-modal" tabindex="-1" role="dialog"
                        aria-labelledby="exampleModalLabel" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-centered" role="document">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h5 class="modal-title" id="exampleModalLabel">Question</h5>
                                    </div>
                                    <div class="modal-body">
                                        ${question}
                                    </div>
                                    <div class="modal-footer">
                                        ${buttonOption.shouldShowCascadeButton ?
                                            `<button type="button"
                                                class="btn bg-gradient-primary-dark my-shadow text-white"
                                                data-action="cascade">
                                                ${buttonOption.cascadeButtonText}
                                            </button>` : ''}
                                        ${buttonOption.shouldShowNonCascadeButton ?
                                            `<button type="button"
                                                class="btn bg-gradient-primary-dark my-shadow text-white"
                                                data-action="non-cascade">
                                                ${buttonOption.nonCascadeButtonText}
                                            </button>` : ''}
                                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal"
                                            data-action="cancel">
                                            ${buttonOption.cancelButtonText}
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>`;
    $('body').append(modalHtml);
    $('#question-modal').modal({
        backdrop: 'static',
        keyboard: false
    }).modal('show');
    $('#question-modal').on('question-answered', function (_, source) {
        confirmedCallback(source.attr('data-action'));
    });
    $('#question-modal > .modal-dialog > .modal-content > .modal-footer > button:not(:last-child)')
        .click(function () {
            $('#question-modal').modal('hide');
            $('#question-modal').trigger('question-answered', [$(this)]);
        });
    $('#question-modal').on('hidden.bs.modal', function() {
        $(this).modal('dispose');
        $(this).remove();
    });
}

function displayYesNoQuestion(question, confirmCallback) {
    displayCascadeQuestionDialog(question, {
        shouldShowCascadeButton: false,
        shouldShowNonCascadeButton: true,
        nonCascadeButtonText: 'Yes',
        cancelButtonText: 'No'
    }, confirmCallback);
}