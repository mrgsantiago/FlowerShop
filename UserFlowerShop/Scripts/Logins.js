////$(document).ready(function () {
////    $('form').submit(function (e) {
////        e.preventDefault();
////        var Mail = $('#Mail').val();
////        var Password = $('#Password').val();

////        $(".error").remove();

////        if (Mail.length < 1) {
////            $('#Mail').after('<span class="error">This field is required</span>');
////        } else {
////            var regEx = /^[A-Z0-9][A-Z0-9._%+-]{0,63}@(?:[A-Z0-9-]{1,63}.){1,125}[A-Z]{2,63}$/;
////            var validEmail = regEx.test(Mail);
////            if (!validEmail) {
////                $('#Mail').after('<span class="error">Enter a valid email</span>');
////            }
////        }
////        if (Password.length < 8) {
////            $('#Password').after('<span class="error">Password must be atleast 8 characterslong</span>');
////        }
////    });
////});
function Autorization(e) {
    var login = $('#Mail').val();
    var password = $('#Password').val();
    
    let result = true;
    $(".error").remove();
    $("#Mail").removeClass("is-invalid")
    $("#Password").removeClass("is-invalid")
    if (login.length < 1) {
        result = false;
        $("#Mail").toggleClass("is-invalid")
        $('#Mail').before($('<label>', {
            'for': 'Mail',
            'class': "form-label error text-danger",
            'text': 'Неверный логин!'
        }));
    }
    if (password.length < 1) {
        $("#Password").toggleClass("is-invalid")
        $('#Password').before($('<label>', {
            'for': 'Password',
            'class': "form-label error text-danger",
            'text': 'Неверный пароль!'
        }));
    }
    else {
        if (result) {


            let object = {
                Login: login,
                Password: password,
            };
            let obj = JSON.stringify(object)
            $.ajax({
                type: 'POST',
                url: '/Home/Login',
                beforeSend: function () {
                    $("#LoginInSystem").prop("disabled", true);
                },
                contentType: 'application/json; charset=utf-8',
                data: obj,
                success: function (result) {
                    if (result.AllResult) {
                        
                        location.href = "/Home/Index";
                    }
                    else {
                        if (!result.LoginCheck) {
                            $("#LoginUser").toggleClass("is-invalid")
                            $('#LoginUser').before($('<label>', {
                                'for': 'LoginUser',
                                'style': 'display:block',
                                'class': "form-label error text-danger",
                                'text': 'Неверный логин!'
                            }));
                        }
                        if (!result.PasswordCheck) {

                            $("#Password").toggleClass("is-invalid")
                            $('#Password').before($('<label>', {
                                'for': 'PasswordUser',
                                'class': "form-label error text-danger",
                                'text': 'Неверный пароль!'
                            }));
                        }
                    }
                    $("#LoginInSystem").prop("disabled", false);
                },
                error: function (data) {
                    alert(data.responseText);
                }
            });

        }

    }
}