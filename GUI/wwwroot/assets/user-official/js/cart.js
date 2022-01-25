$(document).ready(function () {
    $('.product > .product-media > .product-action > a.btn-product.btn-cart').click(function () {
        let productId = $(this).data('product');
        let userId = 'adf66c79-e39a-4288-fcf9-08d9df112449';
        addProductToCart(userId, productId)
            .then(() => toastr.success('Added product to cart'))
            .catch(error => toastr.error(error));
    });

    $('.quantity-col > .cart-product-quantity > .input-group > .input-group-prepend > button.btn-decrement.btn-spinner')
        .click(function () {
            let currentQuantity = $(this).parent().parent().children('input').val();
            console.log(currentQuantity);
        });

    $('.quantity-col > .cart-product-quantity > .input-group > .input-group-append > button.btn-increment.btn-spinner')
        .click(function () {
            let currentQuantity = $(this).parent().parent().children('input').val();
            console.log(currentQuantity);
        });
});

function updatePriceByQuantity(productId, quantity) {
    $('table.table.table-cart').children('tr')
}