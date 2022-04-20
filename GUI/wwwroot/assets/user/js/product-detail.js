$(document).ready(() => {
    $('.product-gallery > #product-zoom-gallery > .product-gallery-item').click(function (e) {
        e.preventDefault();
        let image = $(this).data('image');
        $(this).parent().parent().children('figure').children().attr('src', image);
    });

    let path = location.pathname;
    if (path.endsWith('/'))
        path = path.slice(0, path.length - 1);
    let productId = path.split('/')[3];
    let shopId = $('#shop-id').html();
    let animationLoader = new AnimationLoader('#loading-container > #animation-container', '/assets/shop-owner/img/illustrations/loading.json');
    animationLoader.showAnimation();

    let promises = [getRelatedProducts(productId), getShop(shopId)];

    Promise.all(promises)
        .then(values => {
            onRelatedProductsGot(values[0]);
            onShopInfoGot(values[1]);
            animationLoader.hideAnimation();
        })
        .catch(error => {
            toastr.error(error);
            animationLoader.hideAnimation();
        });
});

function onRelatedProductsGot(products) {
    let relatedProductsHtml = '';
    products.forEach(product => {
        relatedProductsHtml += buildRelatedProductItem(product);
    });
    $('.products').html(relatedProductsHtml);
}

function onShopInfoGot(shop) {
    $('.product-cat').eq(1).children('a').html(shop.shop_Name);
}