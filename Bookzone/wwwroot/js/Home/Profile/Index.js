$(document).ready(function () {

    $("#UpdateBtn").on("click", function () {
        $(this).hide()
        $("#submitBtn").show()
        $("#CancelBtn").show()
        $('input[readonly], select:disabled').each(function (i, obj) {
            $(obj).attr("readonly", false);
            $(obj).prop("disabled", false);
            $(".about p").hide()
            $("#about").show()

        })
    })
    $("#triggerPdp").on("click", function () {
        $("#PhotoUpload").click();
    })

    $("#CancelBtn").on("click", function () {
        location.reload()
    })

    $("#PhotoUpload").change(function () {
        var files = $('#PhotoUpload').prop("files");
        let formData = new FormData();
        formData.append("myUploader", files[0]);
        formData.append("id", $("#txtId").val());
        formData.append("type", "profile");

        $.ajax({
            type: 'POST',
            url: "/Dashboard/OnPostMyUploader/",
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            success: function (data) {
                if (data.data.success) {
                    $("#triggerPdp img").attr("src", "/img/profile/" + data.data.extra);
                }
            }
        });
    });

});