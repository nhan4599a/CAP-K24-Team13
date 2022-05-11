$(document).ready(function () {
    $('.product > .product-media > .product-action > a.btn-product.btn-cart').click(function (e) {
        e.preventDefault();
        let productId = $(this).data('product');
        let searchPageProductCartRootElement = $(this).parent().parent().parent();
        let imageUrl = searchPageProductCartRootElement.children('.product-media').find('a > img').attr('src');
        let name = searchPageProductCartRootElement.find('.product-body > .product-title > a').html();
        let price = searchPageProductCartRootElement.find('.product-body > .product-price > span.new-price').text();
        getUserId().then(userId => {
            addProductToCart(userId, productId, 1)
                .then(() => {
                    toastr.success('Added product to cart');
                    let product = {
                        id: productId,
                        name: name,
                        imageUrl: imageUrl,
                        price: unformatPrice(price),
                        quantity: 1
                    };
                    updateDropdownCart(product);
                })
                .catch(error => toastr.error(error));
        }).catch(() => toastr.error('You must signed in before you can add product to cart'));
    });

    $('.product-details-action > .btn-product.btn-cart').click(function (e) {
        e.preventDefault();
        let productId = $(this).data('product');
        let quantity = $(this).parent().parent().children('.details-filter-row.details-row-size')
            .find('.input-group.input-spinner').children('input').val();
        let productDetailRootElement = $('div.product-details');
        let name = productDetailRootElement.find('h1.product-title').html();
        let price = unformatPrice(productDetailRootElement.find('div.product-price > span.new-price').html());
        let imageUrl = $('div#product-zoom-gallery > a.product-gallery-item:first-child').data('image');
        getUserId().then(userId => {
            addProductToCart(userId, productId, quantity)
                .then(() => {
                    toastr.success('Added product to cart');
                    let product = {
                        id: productId,
                        name: name,
                        price: price,
                        quantity: quantity,
                        imageUrl: imageUrl
                    }
                    updateDropdownCart(product);
                })
                .catch(error => toastr.error(error));
        }).catch(() => toastr.error('You must signed in before you can add product to cart'));
    });

    $('div.dropdown-cart-products > div.product > a.btn-remove').click(function (e) {
        e.preventDefault();
        let productId = $(this).parent().data('product');
        getUserId().then(userId => {
            removeProductInCart(userId, productId)
                .then(function () {
                    toastr.success('Remove product in cart successfully');
                    deleteDropdownCartItem(productId);
                })
                .catch(error => toastr.error(error));
        });
    });

    $('.summary.summary-cart > a.btn.btn-outline-primary-2').click(function (e) {
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
        if (selectedItems.length == 0) {
            toastr.error('Please Select Product');
            return;
        }
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

    $('.quantity-col > .cart-product-quantity > .input-group > input').change(_.debounce(function () {
        let currentQuantity = $(this).val();
        let productId = $(this).parent().parent().parent().parent().data('product');
        getUserId().then(userId => {
            updateCartQuantity(userId, productId, currentQuantity)
                .then(function () {
                    updatePriceByQuantity(productId, currentQuantity);
                    let productItemElement = findProductItem($('div.dropdown-cart-products'), '.product', productId);
                    if (!productItemElement)
                        return;
                    productItemElement.find('.product-cart-details span.cart-product-qty').html(currentQuantity);
                    let price = unformatPrice(productItemElement.find('.cart-product-info').children('.base-price').html());
                    updateDropdownCartTotal(-price);
                    toastr.success('Update cart successfully');
                })
                .catch(error => toastr.error(error));
        });
    }, 200));

    $('.remove-col > button.btn-remove').click(function () {
        let mostParentElement = $(this).parent().parent();
        let productId = mostParentElement.data('product');
        getUserId().then(userId => {
            removeProductInCart(userId, productId)
                .then(function () {
                    let originalTotalPrice = unformatPrice(mostParentElement.children('.total-col').html());
                    updateCartTotal(-originalTotalPrice);
                    toastr.success('Remove product in cart successfully');
                    mostParentElement.remove();
                    deleteDropdownCartItem(productId);
                })
                .catch(error => toastr.error(error));
        });
    });
});

function updatePriceByQuantity(productId, quantity) {
    let cartItemElement = findProductItem($('table.table.table-cart'), 'tr', productId);
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

function updateDropdownCartTotal(deltaPrice) {
    if (typeof deltaPrice != 'number')
        deltaPrice = parseInt(deltaPrice);
    let cartTotalElement = $('div.dropdown-cart-total');
    if (!cartTotalElement.children('.cart-total-price').text()) {
        cartTotalElement.children('span').eq(0).text('Total');
        cartTotalElement.children('.cart-total-price').text(formatPrice(deltaPrice));
    } else {
        let oldPriceElement = cartTotalElement.children('.cart-total-price');
        let originalOldPrice = unformatPrice(oldPriceElement.html());
        let originalNewPrice = originalOldPrice + deltaPrice;
        let formattedNewPrice = formatPrice(originalNewPrice);
        oldPriceElement.html(formattedNewPrice);
    }
}

function buildSingleProductItem(product) {
    return `<div class="product" data-product="${product.id}">
                <div class="product-cart-details">
                    <h4 class="product-title">
                        <a href="/product/index/${product.id}">${product.name}</a>
                    </h4>
                    <span class="cart-product-info">
                        <span class="cart-product-qty">${product.quantity}</span>
                        x
                        <span class="base-price">${formatPrice(product.price)}</span>
                    </span>
                </div>
                <figure class="product-image-container">
                    <a class="product-image" href="/product/index/${product.id}">
                        <img src="${product.imageUrl}" alt="${product.name}" />
                    </a>
                </figure>
                <a href="#" class="btn-remove" title="Remove Product">
                    <i class="icon-close"></i>
                </a>
            </div>`;
}

function findProductItem(rootElement, selector, productId) {
    let result = null;
    rootElement.find(selector).each(function () {
        if ($(this).data('product') == productId) {
            result = $(this);
            return;
        }
    });
    return result;
}

function updateDropdownCart(product) {
    if (typeof product.quantity != 'number')
        product.quantity = parseInt(product.quantity);
    if (typeof product.price != 'number')
        product.price = parseFloat(product.price);
    var currentCartCountValue = $('.cart-count').html();
    if (!currentCartCountValue)
        $('.cart-count').html(product.quantity);
    let rootDropdownCartElement = $('div.dropdown-cart-products');
    let productItemElement = findProductItem(rootDropdownCartElement, '.product', product.id);
    if (productItemElement == null) {
        rootDropdownCartElement.find('.product').filter((_, element) => !$(element).attr('data-product')).remove();
        rootDropdownCartElement.find('.dropdown-cart-total').before(buildSingleProductItem(product));
        attachRemoveButtonInDropdownCart();
        $('.cart-count').html(parseInt(currentCartCountValue) + 1);
    }
    else {
        let cartItemQty = productItemElement.find('span.cart-product-qty');
        let currentQty = parseInt(cartItemQty.html());
        cartItemQty.html(currentQty + product.quantity);
    }
    updateDropdownCartTotal(product.quantity * product.price);
}

function deleteDropdownCartItem(productId) {
    let rootDropdownCartElement = $('div.dropdown-cart-products');
    let productItemElement = findProductItem(rootDropdownCartElement, '.product', productId);
    let productElementCount = rootDropdownCartElement.children('.product').length;
    if (productItemElement == null)
        return;
    let cartCountValue = parseInt($('.cart-count').html());
    if (!cartCountValue || cartCountValue == 0)
        return;
    $('.cart-count').html(cartCountValue - 1);
    if (productElementCount == 1) {
        rootDropdownCartElement.html(buildEmptyDropdownCart());
        return;
    }
    let quantity = parseInt(productItemElement.find('.cart-product-info').children('.cart-product-qty').html());
    let price = unformatPrice(productItemElement.find('.cart-product-info').children('.base-price').html());
    productItemElement.remove();
    updateDropdownCartTotal(-(quantity * price));
}

function buildEmptyDropdownCart() {
    return `<div class="product">
                <h4 class="product-title" style="padding-left: 20px;">
                    There are no products in cart
                </h4>
            </div>
            <div class="dropdown-cart-total">
                <span></span>
                <span class="cart-total-price"></span>
            </div>
            <div class="dropdown-cart-action" style="display: block">
                <a href="/cart" class="btn btn-primary" style="display: block;">
                    View Cart
                </a>
            </div>`;
}

function buildDropdownCartTotal(price) {
    return `<div class="dropdown-cart-total">
                <span>Total</span>
                <span class="cart-total-price">${formatPrice(price)}</span>
            </div>`;
}

function attachRemoveButtonInDropdownCart() {
    $('div.dropdown-cart-products > div.product > a.btn-remove').click(function (e) {
        e.preventDefault();
        let productId = $(this).parent().data('product');
        getUserId().then(userId => {
            removeProductInCart(userId, productId)
                .then(function () {
                    toastr.success('Remove product in cart successfully');
                    deleteDropdownCartItem(productId);
                }).catch(error => toastr.error(error));
        });
    });
}