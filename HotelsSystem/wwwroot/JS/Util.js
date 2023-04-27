
window.blazorExtensions = {
    WriteCookie: function (value,name) {

        var expires;

        var date = new Date();
        date.setTime(date.getTime() + (1 * 60 * 60 * 1000));
        expires = "; expires=" + date.toGMTString();

        document.cookie = name + "=" + value + expires + "; path=/";
    },
    getCookie: (name) => {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);
        if (parts.length === 2) {
            const cookie = parts.pop().split(';').shift();
            blazorExtensions.WriteCookie(cookie,name)

            return cookie;
        }
    },getLangCookie: () => {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; .AspNetCore.Culture=`);
        if (parts.length === 2) {
            const cookie = parts.pop().split(';').shift();

            return cookie;
        }
    },
    delete_cookie: (name) => {
        document.cookie = name + '=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    }
}

$(document).off('input', '.dec')

$(document).on('input', '.dec', function (evt) {
    formatCurrency($(this));
});

$(document).on('input blur paste', '.int', function () {
    this.value = this.value.replace(/\D/g, '');
})

$(document).on('input', '.ratio', function (evt) {
    var position = this.selectionStart;
    var oldval = this.value;
    var newval = this.value.replace(/[^0-9.-]/g, '').replace(/^(-)|-+/g, '$1').replace(/^([^.]*\.)|\.+/g, '$1');
    this.value = newval;
    if (oldval != newval) {
        this.selectionEnd = position - 1;
    }
});
function formatNumber(n) {
    // format number 1000000 to 1,234,567
    return n.replace(/[^0-9\-]/g, "").replace(/(?!^)-/g, '').replace(/\B(?=(\d{3})+(?!\d))/g, ",")
}

function formatCurrency(input, blur) {
    // appends $ to value, validates decimal side
    // and puts cursor back in right position.

    // get input value
    var input_val = input.val();

    // don't validate empty input
    if (input_val === "") { return; }

    // original length
    var original_len = input_val.length;

    // initial caret position
    var caret_pos = input.prop("selectionStart");

    // check for decimal
    if (input_val.indexOf(".") >= 0) {
        // get position of first decimal
        // this prevents multiple decimals from
        // being entered
        var decimal_pos = input_val.indexOf(".");

        // split number by decimal point
        var left_side = input_val.substring(0, decimal_pos);
        var right_side = input_val.substring(decimal_pos);

        // add commas to left side of number
        left_side = formatNumber(left_side);

        // validate right side
        right_side = formatNumber(right_side);

        // On blur make sure 2 numbers after decimal
        if (blur === "blur") {
            right_side += "00";
        }

        // Limit decimal to only 2 digits
        right_side = right_side.substring(0, 2);

        // join number by .
        input_val = left_side + "." + right_side;
    } else {
        // no decimal entered
        // add commas to number
        // remove all non-digits
        input_val = formatNumber(input_val);
        input_val = input_val;

        // final formatting
        if (blur === "blur") {
            input_val += ".00";
        }
    }

    // send updated string to input
    input.val(input_val);

    // put caret back in the right position
    var updated_len = input_val.length;
    caret_pos = updated_len - original_len + caret_pos;
    //input[0].setSelectionRange(caret_pos, caret_pos);
}

function HideOffcanvas() {
    $('.offcanvas.show').offcanvas('hide')
}
function ShowOffcanvas(id) {
    $('#' + id).offcanvas('show')
}
function ShowModal(id) {
    $('#' + id).modal('show')
}
function HideModal() {
    $('.modal.show').modal('hide')
}