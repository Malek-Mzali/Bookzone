$(document).ready(function() {

    var $menu_items = $("#dropdownList li a");
    $menu_items.on("click", function () {
        $('#LanguageMenu').html($(this).html());
    });
    $menu_items[0].click();

    $('#dropdownList li').find("a").click(function () {

        $('#LanguageMenu').html($(this).html());
    })


    $.ajax({
        type: 'GET',
        url: '/Home/CartCount/',
        success: function (data) {
            if (data!== 0){
                $("#myCart").attr('data-count', data)
            }   
        }
    })


    $("#AddToCart").click(function(){
        
        $.ajax({
            type: 'GET',
            url: '/Home/AddToCart/',
            data: '&Id=' + $("#txtId").val(),
            success: function (data) {
                if (data.data.success) {
                    if (data.data.extra === 0) {
                    } else {
                        $("#myCart").attr('data-count', data.data.extra)

                    }
                }
            }
        })
    });

    

});

function RemoveFromCart(id){
    $.ajax({
        type: 'GET',
        url: '/Home/RemoveFromCart/',
        data: '&Id=' + id,
        success: function (data) {
            if (data.data.success){
                if (data.data.extra === 0){
                    $("#myCart").removeAttr('data-count')
                }else{
                    $("#myCart").attr('data-count', data.data.extra)
                }
                setTimeout(function () {
                    window.location.href = '/Home/Cart/'
                })
            }
        }
    })
}