$("#product-form").validate({
    rules: {
        "input-name": {
            required: true,
            minlength: 3,
            maxlength: 100
        },
        "input-description": {
            required: true,
            minlength: 3,
            maxlength: 300
        },
        "input-quantity": {
            required: true,
            number: true,
            min: 1
        },
        "input-price": {
            required: true,
            number: true,
            min: 0
        },
        "input-discount": {
            required: true,
            number: true,
            min: 0
        }
    },
    messages: {
        "input-name": {
            required: "Please enter Product name",
            minlength: "Product Name must be at least 3 characters",
            maxlength: "Product Name can't exceed 50 characters"
        },
        "input-description": {
            required: "Please enter a description for Product",
            minlength: "Product description must be at least 3 characters",
            maxlength: "Product description should not exceed 300 characters"
        },
        "input-quantity": {
            required: "Please enter a quantity for Product",
            number: "Product's quantity must be a number",
            min: "Product's quantity must be greater than or equal to 1"
        },
        "input-price": {
            required: "Please enter a price for Product",
            number: "Product's price must be a number",
            min: "Product's price must greater than or equal to 0"
        },
        "input-discount": {
            required: "Please enter a discount for Product",
            number: "Product's discount must be a number",
            min: "product's discount must be greater than or equal to 0"
        }
    },
    errorElement: "em",
    errorPlacement: function (error, element) {
        // Thêm class `invalid-feedback` cho field đang có lỗi
        error.addClass("invalid-feedback");
        if (element.prop("type") === "checkbox") {
            error.insertAfter(element.parent("label"));
        } else {
            error.insertAfter(element);
        }
    },
    success: function (label, element) { },
    highlight: function (element, errorClass, validClass) {
        $(element).addClass("is-invalid").removeClass("is-valid");
    },
    unhighlight: function (element, errorClass, validClass) {
        $(element).addClass("is-valid").removeClass("is-invalid");
    }
});