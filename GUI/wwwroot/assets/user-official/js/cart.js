$(document).ready(function () {
    $('.product > .product-media > .product-action > a.btn-product.btn-cart').click(function () {
        let productId = $(this).data('product');
        let userId = 'adf66c79-e39a-4288-fcf9-08d9df112449';
        addProductToCart(userId, productId)
            .then(() => toastr.success('Added product to cart'))
            .catch(error => toastr.error(error));
    });
});