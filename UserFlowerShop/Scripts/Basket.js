function DeleteProduct(item) {
    let id = item.id
    $("#DeleteProduct").modal("show")
    $(".sureDelete").attr("id", id)

}

function CancelDelete() {
    $(".sureDelete").removeAttr("id")
    $("#DeleteProduct").modal("hide")
}

function EndDelete(item) {
    let id = item.id
    let object = {
        ID: id
    };
    let obj = JSON.stringify(object)
    $.ajax({
        type: 'POST',
        url: '/BasketContents/Delete',
        contentType: 'application/json; charset=utf-8',
        data: obj,
        success: function (data) {
            $(".sureDelete").removeAttr("id")
            $("#DeleteProduct").modal("hide")
            location.reload();
        },
        error: function (data) {
            alert(data.responseText);
        }
    });

}
$('.DropAmount').change(function () {
    //Use $option (with the "$") to see that the variable is a jQuery object
    let option = $(this).find('option:selected');
    //Added with the EDIT
    var value = option.val();//to get content of "value" attrib
    let idGood = $(this).attr("id");
    let object = {
        idGood: idGood,
        amount: value
    };
    let obj = JSON.stringify(object)
    $.ajax({
        type: 'POST',
        url: '/BasketContents/ChangeElement',
        contentType: 'application/json; charset=utf-8',
        data: obj,
        success: function (data) {
           
            //let element = $(".ForPrice#" + idGood + "")
            //element.text(data.newPrice)
            $(".DropAmount#" + idGood + "").parent().next().children().text(data.newPrice)
            
            let PricesMassive = $(".ForPrice");
            let AllPrice = 0;
            for (let i = 0; i < PricesMassive.length; i++) {

                AllPrice += parseFloat($(PricesMassive[i]).text());
            }
            $("#ForAllPrice").text(AllPrice);

        },
        error: function (data) {
            alert(data.responseText);
        }
    });
});

function AddOrder() {
    if ($("#ForAllPrice").text() != 0) {
        let optionPoint = $("#DropIssuePoints").find('option:selected');
        var valuePoint = optionPoint.val();
        let optionMethod = $("#DropPayMethod").find('option:selected');
        var valueMethod = optionMethod.val();
        let object = {
            pay: valueMethod,
            point: valuePoint
        };
        let obj = JSON.stringify(object)
        $.ajax({
            type: 'POST',
            url: '/BasketContents/CreateOrder',
            contentType: 'application/json; charset=utf-8',
            data: obj,
            success: function (data) {
                setTimeout(function () { window.location.reload(); }, 3000);
                $("#CreateOrder").modal("show");

            },
            error: function (data) {
                alert(data.responseText);
            }
        });
    }


}

