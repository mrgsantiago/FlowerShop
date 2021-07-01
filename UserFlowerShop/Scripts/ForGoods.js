function InfoGoods(item) {
    let id = item.id
    let object = {
        id: id
    };
    let obj = JSON.stringify(object)
    $.ajax({
        type: 'POST',
        url: '/Goods/LoadInfoGoods',
        contentType: 'application/json; charset=utf-8',
        data: obj,
        success: function (data) {
            $('#LoadInfoGoods').html(data);
        },
        error: function (data) {
            alert(data.responseText);
        }
    });
    $('#ModalInfoGood').modal('show');

}
function LoadGoods(item) {
    let id = item.id
    let object = {
        id: id
    };
    let obj = JSON.stringify(object)
    $.ajax({
        type: 'POST',
        url: '/Goods/ElementGoods',
        contentType: 'application/json; charset=utf-8',
        data: obj,
        success: function (data) {
            $('#ElementGoods').html(data);
        },
        error: function (data) {
            alert(data.responseText);
        }
    });
}
function LoadGoodsTwo(item) {
    let id = item.id
    let object = {
        id: id
    };
    let obj = JSON.stringify(object)
    $.ajax({
        type: 'POST',
        url: '/Goods/ElementGoodsTwo',
        contentType: 'application/json; charset=utf-8',
        data: obj,
        success: function (data) {
            $('#ElementGoodsTwo').html(data);
        },
        error: function (data) {
            alert(data.responseText);
        }
    });
}

function ClickMinus(item) {
    const ValMinus = 1
    let Input = $(item).parent().next()
    let ValInput = parseInt(Input.val())
    if (ValInput - ValMinus > 0) {
        Input.val(ValInput - ValMinus)
    
    }
    else {
        Input.val(0)
      
    }


}
function ClickButton(item) {
    $(".quaternary-heading").css({ 'font-weight': '100' })
    $(item).css({ 'font-weight': '700' })
}

function ClickPlus(item) {
    const ValPlus = 1
    let Input = $(item).parent().prev()
    let maxCount = $("#MaxCount").text()
    let ValInput = parseInt(Input.val())
    if (ValInput + ValPlus <= maxCount) {
        Input.val(ValInput + ValPlus)
      
    }
    else {
        Input.val(maxCount)
       
    }
}

function CloseNoPresence() {
    $("#ModalErrorForCount").modal("hide");
}

function AddInBasket() {

    let price = parseFloat($(".PriceHead").text());

    let IdGoods = $("#IDGood").text();
    let Count = $("#CurrentWeight").val();
    if (Count == 0) {
        $("#ModalErrorForCount").modal("show");
    }
    else {
        $.ajax({
        type: 'POST',
        url: '/Home/CheckAuthenticated',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            //if (data.check) { window.location.href = "" }
            if (data.check) {
                let object = {
                    IDGoods: IdGoods,
                    Price: price,
                    Amount: Count
                };
                let obj = JSON.stringify(object)
                $.ajax({
                    type: 'POST',
                    url: '/BasketContents/Create',
                    contentType: 'application/json; charset=utf-8',
                    data: obj,
                    success: function (data) {
                        //if (data.check) { window.location.href = "" }
                        if (data.Presence) {
                            if (data.AcceptableAmount === 0) {
                                $('#ModalOfBasket').modal('show');
                            }
                            else {
                                $('#limit').text(data.AcceptableAmount)
                                $('#ModalLimit').modal('show');
                            }
                        }
                        else {
                            $('#ModalOfError').modal('show');
                        }
                    },
                    error: function (data) {
                        alert(data.responseText);
                    }
                });
            }
            else {
                $('#ModalLoginError').modal('show');
               
            }
        },
        error: function (data) {
            alert(data.responseText);
        }
    });
    }
    

}