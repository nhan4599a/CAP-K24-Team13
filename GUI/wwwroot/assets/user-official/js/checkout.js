$('#form-input').submit(function (e) {
    e.preventDefault();
    e.stopPropagation();
    console.log(buildRequestModel());
});
function buildRequestModel() {
    let fullname = $("#input-fullname").val();
    let phone = $('#input-phone').val();
    let streetaddress = $("#input-streetaddress").val();
    let ward = $("#input-ward").val();
    let district = $("#input-district").val();
    let townCity = $("#input-towncity").val();
    let orderNotes = $("#input-ordernotes").val();
    return {
        fullname: fullname,
        phone: phone,
        shippingAddress: streetaddress + ward + district + townCity,
        orderNotes: orderNotes
    };
}