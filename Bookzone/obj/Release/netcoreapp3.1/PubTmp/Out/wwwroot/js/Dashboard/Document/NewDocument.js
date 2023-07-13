$(document).ready(function () {
    $('input[name="TypeDocument"]').click(function () {
        var type = $("input:radio[name ='TypeDocument']:checked").val();
        if (type === "Database"){
            $('#Database').show();
            $('#Marc21').hide();
            $('#Z39').hide();
        }

        if (type === "Marc21"){
            $('#Database').hide();
            $('#Marc21').show();
            $('#Z39').hide();
        }
        if (type === "Z39"){
            $('#Database').hide();
            $('#Marc21').hide();
            $('#Z39').show();
        }
    });



    
    $("#newdocumentform").submit(function(e) {
        e.preventDefault();

        var form = $(this).serialize();
        var type = $("#txtDocumentType").val();

        $("#txtNbPages").rules("remove", "required");
        $("#txtDateFirstIssue").rules("remove", "required");
        $("#txtTotalIssuesNb").rules("remove", "required");
        $("#txtSellingPrice").rules("remove", "required");
        $("#txtDigitalPrice").rules("remove", "required");
        $("#txtAccompanyingMaterialsNb").rules("remove", "required");
        $("#txtVolumeNb").rules("remove", "required");


        

        if($("#newdocumentform").valid()){
            if ($("#txtTitle").val().length > 0 && $("#txtPrice").val().length > 0 &&  $("#txtPublicationDate").val().length > 0 &&  $("#txtLanguage").val().length > 0 &&  $("#txtDocumentType").val().length > 0 &&  $("#IdEditor").val().length > 0  &&  $("#IdCollection").val().length > 0 &&  $("#txtKeywords").val().length > 0 &&  ( $("#txtIsbn").val().length > 0 ||   $("#txtIssn").val().length > 0)) {
                $.ajax({
                    type: "POST",
                    url: "/Dashboard/NewDocument/",
                    data: form,
                    success: function (data) {
                        if (data.data.success) {
                            var files = $('#NewPhotoUpload').prop("files");
                            if (files.length > 0) {
                                let formData = new FormData();
                                formData.append("myUploader", files[0]);
                                formData.append("id", data.data.extra);
                                formData.append("type", "document");

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
                                        if (!data.data.success) {
                                            $("#productImg").show()
                                            $("#PicturePB").hide()
                                        }
                                    }
                                })

                            }
                            var doc = $('#NewIdFile').prop("files");
                            if (doc.length > 0) {
                                let formData = new FormData();
                                formData.append("myUploader", doc[0]);
                                formData.append("id", data.data.extra);
                                formData.append("type", "documentFile");

                                $.ajax({
                                    xhr: function () {
                                        var xhr = new window.XMLHttpRequest();

                                        xhr.upload.addEventListener("progress", function (evt) {
                                            if (evt.lengthComputable) {
                                                var percentComplete = evt.loaded / evt.total;
                                                percentComplete = parseInt(percentComplete * 100);
                                                $("#NewIdFile").hide();
                                                $("#DocumentPBClass").show();
                                                $("#DocumentPB").width(percentComplete + '%').html(percentComplete + '%');
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
                                        $("#DocumentePB").width('0%').html('0%');

                                    },
                                    success: function (data) {
                                        if (data.data.success) {
                                            setTimeout(function () {
                                                window.location.href = '/Dashboard/Documents/' + type
                                            }, 3000);
                                        }else{
                                            $.ajax({
                                                url: "/Dashboard/DeleteDocument/",
                                                type: "GET",
                                                data:
                                                    {
                                                        "id": data.data.extra
                                                    },
                                            })
                                        }
                                    }
                                })

                            }
                        }
                    }
                });
            }
        }
    });

});