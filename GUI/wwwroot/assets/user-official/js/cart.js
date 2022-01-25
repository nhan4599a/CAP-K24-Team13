$(document).ready(function () {
    $('.product > .product-media > .product-action > a.btn-product.btn-cart').click(function () {
        let productId = $(this).data('product');
        let userId = 'B8A936EB-3904-4DBE-D29F-08D9E0150BF3';
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