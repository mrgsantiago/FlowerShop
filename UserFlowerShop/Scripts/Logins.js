$(document).ready(function () {
    $('form').submit(function (e) {
        e.preventDefault();
        var Mail = $('#Mail').val();
        var Password = $('#Password').val();

        $(".error").remove();

        if (Mail.length < 1) {
            $('#Mail').after('<span class="error">This field is required</span>');
        } else {
            var regEx = /^[A-Z0-9][A-Z0-9._%+-]{0,63}@(?:[A-Z0-9-]{1,63}.){1,125}[A-Z]{2,63}$/;
            var validEmail = regEx.test(Mail);
            if (!validEmail) {
                $('#Mail').after('<span class="error">Enter a valid email</span>');
            }
        }
        if (Password.length < 8) {
            $('#Password').after('<span class="error">Password must be atleast 8 characterslong</span>');
        }
    });
});