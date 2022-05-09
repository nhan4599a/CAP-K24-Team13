$(document).ready(() => {
    $('#accordion-payment > .card:nth-child(1) a.collapsed').click();
    $('#form-input').submit(function (e) {
        e.preventDefault();
        e.stopPropagation();
        let model = buildRequestModel();
        if (!model.paymentMethod) {
            toastr.error('You must choose payment method', 'Error');
            return;
        }
        let animationLoader = new AnimationLoader('#loading-container > #animation-container', '/assets/user/checking-out.json');
        if (model.paymentMethod == 'CoD') {
            animationLoader.setAnimationCompletedCallback(() => {
                showCompletedModal();
            });
        }
        animationLoader.showAnimation(10000);
        getUserId()
            .then(userId => {
                checkOut(userId, model.productIdList, model.fullname, model.phone,
                        model.shippingAddress, model.orderNotes, model.paymentMethod)
                    .then(result => {
                        animationLoader.hideAnimation(true);
                        if (model.paymentMethod != 'CoD') {
                            $('body').append('<form id="payment-form"></form>');
                            let form = $('form#payment-form').attr('method', 'POST').attr('action', `/checkout/payment?method=${model.paymentMethod}`);
                            form.append(`<input type="hidden" value="${result}" name="paymentRefId" />`);
                            form.submit();
                        }
                    })
                    .catch(error => {
                        toastr.error(error);
                        animationLoader.hideAnimation();
                    });
            });
    });
});

function buildRequestModel() {
    let fullname = $("#input-fullname").val();
    let phone = $('#input-phone').val();
    let streetaddress = $("#input-streetaddress").val();
    let ward = $("#input-ward").val();
    let district = $("#input-district").val();
    let townCity = $("#input-towncity").val();
    let orderNotes = $("#input-ordernotes").val();
    let paymentMethod = $('#accordion-payment > .card a:not(.collapsed)').data('payment');
    let productList = [];
    $('table.table-summary').find('tbody > tr.product-item').each((_, element) => {
        let href = $(element).find('td > a').attr('href');
        productList.push(href.substring(href.lastIndexOf('/') + 1));
    });
    return {
        fullname: fullname,
        phone: phone,
        shippingAddress: streetaddress + ' ' + ward + ' ' + district + ' ' + townCity,
        orderNotes: orderNotes,
        productIdList: productList,
        paymentMethod: paymentMethod
    };
}

function showCompletedModal() {
    let modal = $('.modal.fade').modal({
        backdrop: 'static',
        keyboard: false
    }).modal('show');
    modal.on('shown.bs.modal', () => {
        let animationLoader = new AnimationLoader('.modal .modal-body > #animation-container', '/assets/user/checked-out.json');
        animationLoader.setAnimationLoop(0);
        animationLoader.showAnimation();
    });
    modal.on('hidden.bs.modal', () => {
        setTimeout(() => {
            window.location.href = '/order-history';
        }, 500);
    });
}