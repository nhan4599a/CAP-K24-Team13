$(document).ready(() => {
    $('tbody > tr button.btn.btn-primary').click(function () {
        let productId = $(this).parent().parent().data('product');
        let modal = $('.modal.fade').attr('data-product', productId).modal({
            backdrop: 'static',
            keyboard: false
        }).modal('show');
        modal.find('.btn.btn-primary').click(function () {
            let star = $('input[name=rating]:checked').val();
            let comment = $('#comment').val();
            let animationLoader = new AnimationLoader('#loading-container > #animation-container', '/assets/user/checking-out.json');
            animationLoader.showAnimation();
            getUserId().then(userId => {
                ratingProduct(userId, productId, star, comment)
                    .then(() => {
                        animationLoader.hideAnimation();
                        toastr.success('Rating Success');
                        window.location.href = `/product/index/${productId}`;
                    })
                    .catch(error => {
                        animationLoader.hideAnimation();
                        toastr.error(error);
                    });
            });
        });
    });
});