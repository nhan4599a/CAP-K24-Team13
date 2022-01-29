$(document).ready(() => {
    $('#form-input').submit(function (e) {
        e.preventDefault();
        e.stopPropagation();
        let animationLoader = new AnimationLoader('.loading-container', '/assets/user-official/checking-out.json');
        animationLoader.showAnimation(10000);
        let model = buildRequestModel();
        checkOut(userId, model.productIdList, model.fullname, model.phone, model.shippingAddress, model.orderNotes)
            .then(() => {
                toastr.success('Checkout successfully');
                animationLoader.hideAnimation();
                window.location.href = '/';
            })
            .catch(error => {
                toastr.error(error);
                animationLoader.hideAnimation();
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
    let productList = [];
    $('table.table-summary').find('tbody > tr.product-item').each((_, element) => {
        let href = $(element).find('td > a').attr('href');
        productList.push(href.substring(href.lastIndexOf('/') + 1));
    });
    return {
        fullname: fullname,
        phone: phone,
        shippingAddress: streetaddress + ward + district + townCity,
        orderNotes: orderNotes,
        productIdList: productList
    };
}