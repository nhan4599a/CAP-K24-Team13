$(document).ready(() => {
    $('.product-gallery > #product-zoom-gallery > .product-gallery-item').click(function (e) {
        e.preventDefault();
        let image = $(this).data('image');
        $(this).parent().parent().children('figure').children().attr('src', image);
    })
})