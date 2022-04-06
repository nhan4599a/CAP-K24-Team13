$(document).ready(() => {
    var currentPageInfo = getCurrentPageInfo();
    loadCategories(currentPageInfo.pageNumber, currentPageInfo.pageSize);
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
    $('a[name=btn-edit]').click(function (e) {
        e.preventDefault();
        let index = parseInt($(this).parent().parent().children('td:nth-child(2)').text()) - 1;
        let categoryInfoStr = JSON.stringify(categories[index]);
        window.localStorage.setItem('editting-category', categoryInfoStr);
        window.location.href = "/shopowner/category/edit";
    });
    $('a[name=btn-action]').click(function (e) {
        e.preventDefault();
        let id = $(this).parent().parent().children('#cate-id').text();
        let name = $(this).parent().parent().children('td:nth-child(4)').children().text();
        let action = $(this).text().trim().toLowerCase();
        if (action !== 'deactivate' && action !== 'activate')
            return;
        displayCascadeQuestionDialog(`Do you want to ${action} ${name}`, {
            shouldShowCascadeButton: true,
            shouldShowNonCascadeButton: action === 'activate'
        }, option => {
            let shouldBeCascade = option === 'cascade';
            let command = {
                id: id,
                isActivateCommand: action === 'activate',
                shouldBeCascade: action === 'activate' ? shouldBeCascade : true
            };
            let successCallback = command.isActivateCommand ? () => {
                toastr.success(`Activated ${name}`, 'Success');
                $(this).parent().parent().children('td:nth-child(5)').children()
                    .removeClass('bg-gradient-secondary')
                    .addClass('bg-gradient-success')
                    .text('Activated');
                $(this).parent().prepend(buildEditButtonHtml());
                $(this).children('span').text(' Deactivate');
                $(this).children('i').removeClass().addClass('far fa-trash-alt');
                $('a[name=btn-edit]').click(function (e) {
                    e.preventDefault();
                    let index = parseInt($(this).parent().parent().children('td:nth-child(2)').text()) - 1;
                    let categoryInfoStr = JSON.stringify(categories[index]);
                    window.localStorage.setItem('editting-category', categoryInfoStr);
                    window.location.href = "/shopowner/category/edit";
                });
            } : () => {
                toastr.success(`Deactivated ${name}`, 'Success');
                $(this).parent().parent().children('td:nth-child(5)').children()
                    .removeClass('bg-gradient-success')
                    .addClass('bg-gradient-secondary')
                    .text('Deactivated');
                $(this).parent().children('*[name="btn-edit"]').remove();
                $(this).children('span').text(' Activate');
                $(this).children('i').removeClass().addClass('fas fa-check');
            };
            let animationLoader = new AnimationLoader('#loading-container > #animation-container', '/assets/shop-owner/img/illustrations/loading.json');
            animationLoader.showAnimation();
            activateCategory(command).then(() => {
                successCallback();
                animationLoader.hideAnimation()
            }).catch(() => {
                animationLoader.hideAnimation();
                toastr.error(`Failed to ${action} ${name}`);
            });
        });
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