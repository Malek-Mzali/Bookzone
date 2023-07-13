$(document).ready(function () {

    function SetSize() {
        var windowWidth = window.screen.width < window.outerWidth ?
            window.screen.width : window.outerWidth;
        var mobile = windowWidth < 500;

        if (mobile) {
            $("#SearchSuggest").width("90%")

        } else {
            $("#SearchSuggest").width($("#txtTerm").width())
        }
    }

    SetSize()
    $(window).on('resize', function () {
        SetSize()
    });

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
            if (data !== 0) {
                $("#myCart").attr('data-count', data)
            }
        }
    })

    $("#txtTerm").on("change paste keyup input", function () {
        $("#SearchSuggest").removeClass("invisible")
        if ($(this).val().length > 0) {
            $("#SearchSuggest").removeClass("invisible")
            $.ajax({
                type: 'POST',
                url: '/Home/AjaxSearch/',
                data: $("#SearchByTerm").serialize(),
                success: function (data) {
                    if (data.data.length > 0) {
                        $("#SearchSuggest").addClass("show").html("")
                        $.each(data.data, function (i, result) {
                            $("#SearchSuggest").append(
                                `
                                    <li class="dropdown-item">
                                    <a class="text-grey" href="${result.documentGroup.url}">${result.documentGroup.originalTitle} ${result.documentGroup.subtitle}
                                    </a>
                                    </li>
                                `
                            )
                        })
                    } else {
                        $("#SearchSuggest").addClass("invisible")
                        $("#SearchSuggest").removeClass("show").html("")
                    }

                }
            })
        } else {
            $("#SearchSuggest").removeClass("show").html("")
        }
    })


});

function AddToWishList(org) {
    $.ajax({
        type: 'GET',
        url: '/Home/AddToWishList/',
        data: '&Id=' + $(org).attr("data-id"),
        success: function (data) {
        }
    })
}

function RemoveFromWishList(org) {
    $.ajax({
        type: 'GET',
        url: '/Home/RemoveFromWishList/',
        data: '&Id=' + $(org).attr("data-id"),
        success: function (data) {
            setTimeout(function () {
                window.location.reload()
            })
        }
    })
}

function AddToCart(org) {
    $.ajax({
        type: 'GET',
        url: '/Home/AddToCart/',
        data: '&Id=' + $(org).attr("data-id"),
        success: function (data) {
            if (data.data.success) {
                if (data.data.extra === 0) {
                } else {
                    $("#myCart").attr('data-count', data.data.extra)
                }
            }
        }
    })
}

function RemoveFromCart(id) {
    $.ajax({
        type: 'GET',
        url: '/Home/RemoveFromCart/',
        data: '&Id=' + id,
        success: function (data) {
            if (data.data.success) {
                if (data.data.extra === 0) {
                    $("#myCart").removeAttr('data-count')
                } else {
                    $("#myCart").attr('data-count', data.data.extra)
                }
                setTimeout(function () {
                    window.location.href = '/Home/Cart/'
                })
            }
        }
    })
}

