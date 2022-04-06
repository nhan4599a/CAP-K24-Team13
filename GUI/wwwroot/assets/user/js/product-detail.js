$(document).ready(() => {
    $('.product-gallery > #product-zoom-gallery > .product-gallery-item').click(function (e) {
        e.preventDefault();
        let image = $(this).data('image');
        $(this).parent().parent().children('figure').children().attr('src', image);
    });

    let path = location.pathname;
    if (path.endsWith('/'))
        path = path.slice(0, path.length - 1);
    let productId = path.split('/')[2];
    let animationLoader = new AnimationLoader('#loading-container > #animation-container', '/assets/shop-owner/img/illustrations/loading.json');
    animationLoader.showAnimation();
    getRelatedProducts(productId)
        .then(products => {
            animationLoader.hideAnimation();
            let relatedProductsHtml = '';
            products.forEach(product => {
                relatedProductsHtml += buildRelatedProductItem(product);
            });
            $('.products').html(relatedProductsHtml);
        })
        .catch(() => {
            animationLoader.hideAnimation();
        });
});