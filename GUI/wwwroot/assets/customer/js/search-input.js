
$("#keyword").on('keyup', function () {
    let listData = `<li>tìm kiếm sản phẩm</li><li>tìm kiếm cửa hàng</li>`;
    document.getElementsByClassName(".search-selection").innerHTML = listData;
    $(".search-selection").show();
    console.log("typed");
});