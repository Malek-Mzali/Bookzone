$(document).ready(function () {

    $("#newCollectionForm").submit(function (e) {
        e.preventDefault();

        var form = $(this).serialize();

        if ($("#newCollectionForm").valid()) {
            $.ajax({
                type: "POST",
                url: "/Dashboard/NewCollection/",
                data: form,
                success: function (data) {
                    if (data.data.success) {
                        $('#newCollectionForm')[0].reset();
                        $('.IdEditor').val('').trigger('change');
                        $('#IdTheme').val('').trigger('change');
                    }
                }
            });
        }
    });

});



