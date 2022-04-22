$(document).ready(() => {
    $('tbody > tr button.btn.btn-primary').click(function () {
        let productId = $(this).parent().parent().data('product');
        let invoiceId = $(this).parent().parent().data('invoice');
        let modal = $('.modal.fade').attr('data-product', productId).modal({
            backdrop: 'static',
            keyboard: false
        }).modal('show');
        var sourceButton = $(this);
        modal.find('.btn.btn-primary').off('click').click(function () {
            let star = $('input[name=rating]:checked').val();
            let comment = $('#comment').val();
            let animationLoader = new AnimationLoader('#loading-container > #animation-container', '/assets/user/checking-out.json');
            animationLoader.showAnimation();
            modal.modal('hide');
            ratingProduct(invoiceId, productId, star, comment)
                .then(() => {
                    animationLoader.hideAnimation();
                    sourceButton.prop('disabled', true);
                    toastr.success('Rating success');
                })
                .catch(error => {
                    animationLoader.hideAnimation();
                    toastr.error(error);
                });
        });
    });
});