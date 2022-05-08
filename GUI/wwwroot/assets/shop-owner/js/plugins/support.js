$(document).ready(() => {
    document.querySelector("input.number-only").addEventListener("keypress", function (evt) {
        if (evt.which < 48 || evt.which > 57) {
            evt.preventDefault();
        }
    });
});