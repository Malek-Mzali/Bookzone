$(document).ready(function () {


    $("#newAuthorForm").submit(function (e) {
        e.preventDefault();

        var form = $(this).serialize();

        if ($("#newAuthorForm").valid()) {
            $.ajax({
                type: "POST",
                url: "/Dashboard/NewAuthor/",
                data: form,
                success: function (data) {
                    if (data.data.success) {
                        var files = $('#NewPhotoUpload').prop("files");
                        if (files.length > 0) {
                            let formData = new FormData();
                            formData.append("myUploader", files[0]);
                            formData.append("id", data.data.extra);
                            formData.append("type", "author");

                            $.ajax({
                                xhr: function () {
                                    var xhr = new window.XMLHttpRequest();

                                    xhr.upload.addEventListener("progress", function (evt) {
                                        if (evt.lengthComputable) {
                                            var percentComplete = evt.loaded / evt.total;
                                            percentComplete = parseInt(percentComplete * 100);
                                            $("#productImg").hide();
                                            $("#PicturePBClass").show();
                                            $("#PicturePB").width(percentComplete + '%').html(percentComplete + '%');
                                        }
                                    }, false);

                                    return xhr;
                                },
                                type: 'POST',
                                url: "/Dashboard/OnPostMyUploader/",
                                data: formData,
                                cache: false,
                                contentType: false,
                                processData: false,
                                beforeSend: function (xhr) {
                                    xhr.setRequestHeader("XSRF-TOKEN",
                                        $('input:hidden[name="__RequestVerificationToken"]').val());
                                    $("#PicturePB").width('0%').html('0%');

                                },
                                success: function (data) {
                                    if (data.data.success) {
                                        setTimeout(function () {
                                            window.location.href = '/Dashboard/Author/'
                                        }, 3000);
                                    } else {
                                        $("#productImg").show()
                                        $("#PicturePB").hide()
                                    }
                                }
                            })

                        } else {
                            setTimeout(function () {
                                window.location.href = '/Dashboard/Author/'
                            }, 3000);
                        }
                    }

                }
            });
        }
    });

});



