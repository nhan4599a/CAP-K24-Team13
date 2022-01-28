$(document).ready(function () {
    let userId = 'C61AF282-818D-43CA-6DA9-08D9E08DBE4D';
    $('.product > .product-media > .product-action > a.btn-product.btn-cart').click(function (e) {
        e.preventDefault();
        let productId = $(this).data('product');
        addProductToCart(userId, productId, 1)
            .then(() => toastr.success('Added product to cart'))
            .catch(error => toastr.error(error));
    });

    $('.product-details-action > .btn-product.btn-cart').click(function (e) {
        e.preventDefault();
        let productId = $(this).data('product');
        let quantity = $(this).parent().parent().children('.details-filter-row.details-row-size').eq(2)
            .find('.input-group.input-spinner').children('input').val();
        addProductToCart(userId, productId, quantity)
            .then(() => toastr.success('Added product to cart'))
            .catch(error => toastr.error(error));
    });

    $('a.btn.btn-outline-primary-2').click(function (e) {
        e.preventDefault();
        let selectedItems = [];
        $('.table.table-cart.table-mobile').find('input.form-check-input[type=checkbox]:checked')
            .each((_, element) => {
                let root = $(element).parent().parent();
                selectedItems.push({
                    productId: root.data('product'),
                    quantity: root.children('.quantity-col').find('.input-group.input-spinner > input').val()
                });
            });
        $('body').append('<form id="checkout-form"></form>');
        let form = $('form#checkout-form').attr('method', 'POST').attr('action', '/checkout');
        for (let i = 0; i < selectedItems.length; i++) {
            form = form.append(`<input type="hidden" value="${selectedItems[i].productId}" name="models[${i}].ProductId" />`)
                .append(`<input type="hidden" value="${selectedItems[i].quantity}" name="models[${i}].Quantity" />`);
        }
        form.submit();
    });

    $('.quantity-col > .cart-product-quantity > .input-group > .input-group-prepend > button.btn-decrement.btn-spinner')
        .click(function () {
            $(this).parent().parent().children('input').trigger('change');
        });

    $('.quantity-col > .cart-product-quantity > .input-group > .input-group-append > button.btn-increment.btn-spinner')
        .click(function () {
            $(this).parent().parent().children('input').trigger('change');
        });

    $('.quantity-col > .cart-product-quantity > .input-group > input').change(function () {
        let currentQuantity = $(this).val();
        let productId = $(this).parent().parent().parent().parent().data('product');
        updateCartQuantity(userId, productId, currentQuantity)
            .then(function () {
                updatePriceByQuantity(productId, currentQuantity);
                toastr.success('Update cart successfully');
            })
            .catch(error => toastr.error(error));
    });

    $('.remove-col > button.btn-remove').click(function () {
        let mostParentElement = $(this).parent().parent();
        let productId = mostParentElement.data('product');
        removeProductInCart(userId, productId)
            .then(function () {
                let originalTotalPrice = unformatPrice(mostParentElement.children('.total-col').html());
                updateCartTotal(-originalTotalPrice);
                toastr.success('Remove product in cart successfully');
                mostParentElement.remove();
            })
            .catch(error => toastr.error(error));
    });
});

function updatePriceByQuantity(productId, quantity) {
    let cartItemElement = null;
    $('table.table.table-cart').children('tbody').children().each(function () {
        if ($(this).data('product') == productId) {
            cartItemElement = $(this);
            return;
        }
    });
    if (!cartItemElement)
        return;
    let originalPrice = unformatPrice(cartItemElement.children('.price-col').html());
    let originalOldTotalPrice = unformatPrice(cartItemElement.children('.total-col').html());
    let originalNewTotalPrice = originalPrice * quantity;
    let deltaPrice = originalNewTotalPrice - originalOldTotalPrice;
    let formattedNewTotalPrice = formatPrice(originalNewTotalPrice);
    cartItemElement.children('.total-col').html(formattedNewTotalPrice);
    updateCartTotal(deltaPrice);
}

function updateCartTotal(deltaPrice) {
    let oldPriceElement = $('div.summary.summary-cart .table.table-summary .summary-subtotal').children('td').eq(1);
    let originalOldPrice = unformatPrice(oldPriceElement.html());
    let originalNewPrice = originalOldPrice + deltaPrice;
    let formattedNewPrice = formatPrice(originalNewPrice);
    oldPriceElement.html(formattedNewPrice);
}

function formatPrice(price) {
    return new Intl.NumberFormat('en-US', {
        maximumFractionDigits: 3
    }).format(price);
}

function unformatPrice(formattedPrice) {
    return parseFloat(formattedPrice.replace(/,/g, ''));
}