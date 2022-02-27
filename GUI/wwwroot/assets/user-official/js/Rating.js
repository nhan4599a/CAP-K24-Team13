$(document).ready(() => {
    $('#tab-2 button.btn.btn-primary').click(function () {
        let productId = $(this).parent().parent().data('product');
        let modal = $('.modal.fade').attr('data-product', productId).modal({
            backdrop: 'static',
            keyboard: false
        }).modal('show');
        modal.find('.btn.btn-primary').click(function () {
            let star = $('input[name=rating]:checked').val();
            let comment = $('#comment').val();
            getUserId().then(userId => {
                ratingProduct(userid, productId, star, comment)
                    .then(() => {
                        toastr.success('Rating Success');
                        window.location.href = `/product/index/${productId}`;
                    })
                    .catch((error) => {
                        toastr.error(error);
                    });
            });
        });
    });
});