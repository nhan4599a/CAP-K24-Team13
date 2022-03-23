$(document).ready(function () {
    const orderStatusClassList = ['confirm', 'in-process', 'deliveried', 'done', 'canceled'];
    $('#input-search').keypress(function (e) {
        if (e.which == 13) {
            e.preventDefault();
            let value = $(this).val().trim();
            if (!value) {
                toastr.error('You must enter the keyword', 'Error!');
            } else {
                let key = $('#invoice-search-by').val();
                let urlEncodedValue = encodeURIComponent(value);
                window.location.href =
                    `https://cap-k24-team13.herokuapp.com/shopowner/order/sell-history?key=${key}&value=${urlEncodedValue}`;
            }
        }
    });
    let animationLoader = new AnimationLoader('#loading-container > #animation-container', '/assets/shop-owner/img/illustrations/loading.json');
    animationLoader.showAnimation(3500);
    getShopId()
        .then(shopId => {
            let columnContents = { 0: '', 1: '', 2: '', 3: '', 4: '' };
            getRecentOrdersOfShop(shopId)
                .then(orders => {
                    for (let order of orders) {
                        columnContents[order.status] += buildOrderItem(order);
                    }
                    for (let content in columnContents) {
                        $(`.board-column.${orderStatusClassList[content]} .board-column-content`)
                            .html(columnContents[content]);
                    }
                    $.getScript('/assets/shop-owner/js/internal/kanban.js')
                        .then(() => {
                            animationLoader.hideAnimation();
                        });
                })
                .catch(error => {
                    animationLoader.hideAnimation();
                    toastr.error(error, 'Error!');
                });
        });
});
