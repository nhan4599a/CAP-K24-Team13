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
            console.log('star: ' + star);
            console.log('comment: ' + comment);
            ratingProduct('0B008236-860D-44BB-3328-08D9E8AFEA5C', productId, star, comment)
                .then(() => {
                    toastr.success('Rating Success !!!')
                })
                .catch(() => {
                    toastr.error('Rating Fail')
                })
        });
    });
});