$(document).ready(function () {
    let animationLoader = new AnimationLoader('#loading-container > #animation-container', '/assets/shop-owner/img/illustrations/loading.json');
    animationLoader.showAnimation(3500);
    getShopId()
        .then(shopId => {
            getRecentOrdersOfShop(shopId)
                .then(orders => {
                    let classifiedOrderList = {};
                    classifiedOrderList[0] = [];
                    classifiedOrderList[1] = [];
                    classifiedOrderList[2] = [];
                    classifiedOrderList[3] = [];
                    classifiedOrderList[4] = [];
                    for (let order of orders) {
                        //classifiedOrderList[]
                    }
                });
        });
});