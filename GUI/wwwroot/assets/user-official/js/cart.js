$(document).ready(function () {
    $('.product > .product-media > .product-action > a.btn-product.btn-cart').click(function () {
        let productId = $(this).data('product');
        let userId = '';
        addProductToCart(userId, productId)
            .then(() => toastr.success('Added product to cart'))
            .catch(error => toastr.error(error));
    });
});