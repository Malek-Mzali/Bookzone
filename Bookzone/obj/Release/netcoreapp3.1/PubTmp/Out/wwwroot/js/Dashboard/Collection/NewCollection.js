$(document).ready(function () {

    $("#newCollectionForm").submit(function(e) {
        e.preventDefault();

        var form = $(this).serialize();

        if($("#newCollectionForm").valid()){
            $.ajax({
                type: "POST",
                url: "/Dashboard/NewCollection/",
                data: form,
                success: function(data)
                {
                    if (data.data.success) {
                        setTimeout(function() {window.location.href='/Dashboard/Collection/'}, 3000);
                    }
                }
            });
        }
    });

});



