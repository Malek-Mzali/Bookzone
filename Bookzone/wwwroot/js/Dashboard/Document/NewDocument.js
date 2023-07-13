$(document).ready(function () {
    var sum, auth, DocId, MarcData, GoogleApiData;

    $("#ListRecords").select2().prop("disabled", true);

    $('input[name="TypeDocument"]').click(function () {
        var type = $("input:radio[name ='TypeDocument']:checked").val();
        if (type === "Database") {
            $('#Database').show();
            $('#Marc21').hide();
            $('#GoogleApi').hide();
        }

        if (type === "Marc21") {
            $('#Database').hide();
            $('#Marc21').show();
            $('#GoogleApi').hide();
        }
        if (type === "GoogleApi") {
            $('#Database').hide();
            $('#Marc21').hide();
            $('#GoogleApi').show();
        }
    });

    $("#nextdata").click(function (e) {
        e.preventDefault();
        var type = $("input:radio[name ='TypeDocument']:checked").val();
        if (type === "Marc21") {
            $('#MarcFile').trigger('click');
        }
    })

    $('#MarcFile').on("change", function () {

        var files = $('#MarcFile').prop("files");
        let formData = new FormData();
        formData.append("MarcFile", files[0]);
        $.ajax({
            xhr: function () {
                var xhr = new window.XMLHttpRequest();

                xhr.upload.addEventListener("progress", function (evt) {
                    if (evt.lengthComputable) {
                        var percentComplete = evt.loaded / evt.total;
                        percentComplete = parseInt(percentComplete * 100);
                        $("#Upload").html(percentComplete + "%");
                    }
                }, false);

                return xhr;
            },
            type: 'POST',
            url: "/Dashboard/NewDocumentMarc/",
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
                $('#loader').removeClass('hidden')

            },
            success: function (data) {

                if (!$.isEmptyObject(data.data)) {
                    MarcData = data.data
                    var recordList = []
                    $.each(data.data, function (i, v) {
                        recordList.push({
                            id: i,
                            text: v.title,
                        });
                    })
                    $("#ListRecords").select2({
                        data: recordList,
                    }).prop("disabled", false);

                }
            },
            complete: function () {
                $('#loader').addClass('hidden')
            },
        })
    })

    $("#ListRecords").on("change", function () {
        var id = $(this).select2('data')[0].id
        var documentInfo = MarcData[id].documentInfo
        var author = MarcData[id].author
        var summary = MarcData[id].documentSummaryTitle
        sum = summary;
        auth = author

        $("#RecordMarc").show()

        $("#OriginalTitle").val(documentInfo.documentGroup.originalTitle)
        $("#OriginalLanguage").val(documentInfo.documentGroup.originalLanguage !== null ? documentInfo.documentGroup.originalLanguage : "")
        $("#PublicationDate").val(documentInfo.documentGroup.publicationDate !== null ? documentInfo.documentGroup.publicationDate : "")
        $(".txtDocumentType").val(documentInfo.documentGroup.documentType).attr("disabled", true).trigger('change');
        $("#Keyword").val(documentInfo.documentGroup.keyword.join("\n"))
        $("#Doi").val(documentInfo.documentGroup.doi !== null ? documentInfo.documentGroup.doi : "")
        $("#MarcRecordNumber").val(documentInfo.documentGroup.marcRecordNumber !== null ? documentInfo.documentGroup.marcRecordNumber : "")
        $("#TitlesVariants").val(documentInfo.documentGroup.titlesVariants !== null ? documentInfo.documentGroup.titlesVariants : "")
        $("#Subtitle").val(documentInfo.documentGroup.subtitle !== null ? documentInfo.documentGroup.subtitle : "")
        $("#Translator").val(documentInfo.documentGroup.translator !== null ? documentInfo.documentGroup.translator : "")
        $("#Country").val(documentInfo.documentGroup.country !== null ? documentInfo.documentGroup.country : "")
        $("#State").val(documentInfo.documentGroup.state !== null ? documentInfo.documentGroup.state : "")
        $("#PhysicalDescription").val(documentInfo.documentGroup.physicalDescription !== null ? documentInfo.documentGroup.physicalDescription : "")
        $("#VolumeNb").val(documentInfo.documentGroup.volumeNb !== null ? documentInfo.documentGroup.volumeNb : "")
        $("#Abstract").html(documentInfo.documentGroup.abstract !== null ? documentInfo.documentGroup.abstract : "")
        $("#Notes").html(documentInfo.documentGroup.notes !== null ? documentInfo.documentGroup.notes : "")

        if (documentInfo.documentGroup.documentType === "Ebook") {
            $("#Isbn").val(documentInfo.ebookGroup.isbn !== null ? documentInfo.ebookGroup.isbn : "")
            $("#EditionNum").val(documentInfo.ebookGroup.editionNum !== null ? documentInfo.ebookGroup.editionNum : "")
            $("#EditionPlace").val(documentInfo.ebookGroup.editionPlace !== null ? documentInfo.ebookGroup.editionPlace : "")
            $("#Genre").val(documentInfo.ebookGroup.genre !== null ? documentInfo.ebookGroup.genre : "")
            $("#Category").val(documentInfo.ebookGroup.category !== null ? documentInfo.ebookGroup.category : "")
            $("#NbPages").val(documentInfo.ebookGroup.nbPages !== null ? documentInfo.ebookGroup.nbPages : "")

            var Code = $("#Isbn").val();
            if (Code === "") {
                Code = $("#OriginalTitle").val()
            }
            $.ajax({
                type: "GET",
                url: "/Dashboard/GetAmazonBookCover",
                data: "Code=" + Code + "&Language=" + $("#OriginalLanguage").val(),
                async: false,
                success: function (data) {
                    if (data.data !== null) {
                        $("#GoogleApiBtn").show()
                        $("#MarcCoverApi").show().attr("src", data.data)
                    } else {
                        $.getJSON('https://www.googleapis.com/books/v1/volumes?q=isbn:' + documentInfo.ebookGroup.isbn, function (data) {
                            if (data.totalItems > 0) {
                                $("#GoogleApiBtn").show()
                                $("#MarcCoverApi").show()
                                try {
                                    var BookCover = data.items[0].volumeInfo.imageLinks.thumbnail.replace("zoom=1", "zoom=1")
                                    $("#MarcCoverApi").attr("src", BookCover)
                                } catch {
                                    $("#GoogleApiBtn").hide()
                                    $("#MarcCoverApi").hide()
                                }
                            } else {
                                $("#GoogleApiBtn").hide()
                                $("#MarcCoverApi").hide()
                            }
                        });
                    }
                }
            })


        } else {
            $("#Issn").val(documentInfo.ejournalGroup.issn !== null ? documentInfo.ejournalGroup.issn : "")
            $("#Frequency").val(documentInfo.ejournalGroup.frequency !== null ? documentInfo.ejournalGroup.frequency : "")
            $("#TotalIssuesNb").val(documentInfo.ejournalGroup.totalIssuesNb !== null ? documentInfo.ejournalGroup.totalIssuesNb : "")
            $("#DateFirstIssue").val(documentInfo.ejournalGroup.dateFirstIssue !== null ? documentInfo.ejournalGroup.dateFirstIssue : "")
            $("#JournalScope").val(documentInfo.ejournalGroup.journalScope !== null ? documentInfo.ejournalGroup.journalScope : "")
            $("#ImpactFactor").val(documentInfo.ejournalGroup.impactFactor !== null ? documentInfo.ejournalGroup.impactFactor : "")
        }

        if (author !== null) {
            $('.IdAuthor').append(new Option(author, 0, false, true)).trigger('change');

        } else $('.IdAuthor').append(new Option("No Author were found", -1, false, true)).trigger('change');

        if (summary.length > 0) {
            $("#Summary-tab").show()
            $("#Summary").html("")
            $.each(summary, function (x, t) {
                $("#Summary").append(
                    `
                        <div class="row mt-2" id="${x}">
                        <input class="docID" id="" type="hidden" name="IdDocument" >
                        <input type="hidden" name="Id" value="0">
                        <div class="col-12 col-sm-6">
                            <label>Title</label>&nbsp;<span style="color:red;">*</span>&nbsp;<span class="text-danger" data-valmsg-replace="true"></span>
                            <input name="Title" class=" form-control" data-val="true" data-val-required="Required field" required value="${t}">
                        </div>
                        <div class="col-6 col-sm-2 mt-3 mt-sm-0">
                            <label>Start</label>&nbsp;<span style="color:red;">*</span>&nbsp;<span class="text-danger" data-valmsg-replace="true"></span>
                            <input name="Start" type="number" class=" form-control" data-val="true" data-val-required="Required field" value="0" min="0" step="1" required >
                        </div>
                        <div class="col-6 col-sm-2 mt-3 mt-sm-0">
                            <label>End</label>&nbsp;<span style="color:red;">*</span>&nbsp;<span class="text-danger" data-valmsg-replace="true"></span>
                            <input name="End" type="number" class=" form-control" data-val="true" data-val-required="Required field" value="0" min="0" step="1" required >
                        </div>
                        <div class="col-6 col-sm-1 mt-3 mt-sm-5 text-center">
                            <a  onclick="addSummary(${x})" href="#"> <i class="fa fa-plus" aria-hidden="true"></i></a>
                        </div>
                        <div class="col-6 col-sm-1 mt-3 mt-sm-5 text-center">
                            <a onclick="deleteSummary(${x})" href="#"> <i class="fa fa-minus" aria-hidden="true"></i></a>
                        </div>
                        <div class="col-12 mt-2">
                            <hr/>
                        </div>
                    </div>

                `
                )
            })

        } else
            $("#Summary-tab").hide()


        setFormHeight()
    })

    $("#newdocumentDatabase").data("validator").settings.ignore = "";

    $("#newdocumentDatabase").submit(function (e) {
        e.preventDefault();


        var type = $(".txtDocumentType").val();

        if (type === "Ebook") {
            $(".issn").val("00000000");
        } else {
            $(".isbn").val("0000000000000");
        }
        var form = $(this).serialize();

        if ($("#newdocumentDatabase").valid()) {
            $.ajax({
                type: "POST",
                url: "/Dashboard/NewDocument/",
                data: form,
                success: function (data) {
                    if (data.data.success) {
                        var files = null
                        $('.NewPhotoUpload').each(function (i, obj) {
                            if ($(obj).prop("files").length > 0) {
                                files = $(this).prop("files")
                            }
                        })
                        if (files !== null) {
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
                                            $(".productImg").hide();
                                            $(".PicturePBClass").show();
                                            $(".PicturePB").width(percentComplete + '%').html(percentComplete + '%');
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
                                    $(".PicturePB").width('0%').html('0%');

                                },
                                success: function (data) {
                                    if (!data.data.success) {
                                        $(".productImg").show()
                                        $(".PicturePB").hide()
                                    }
                                }
                            })

                        }
                        var doc = null
                        $('.NewIdFile').each(function (i, obj) {
                            if ($(obj).prop("files").length > 0) {
                                doc = $(this).prop("files")
                            }
                        })
                        if (doc !== null) {
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
                                            $(".NewIdFile").hide();
                                            $(".DocumentPBClass").show();
                                            $(".DocumentPB").width(percentComplete + '%').html(percentComplete + '%');
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
                                    $(".DocumentePB").width('0%').html('0%');

                                },
                                success: function (data) {
                                    if (data.data.success) {
                                        setTimeout(function () {
                                            window.location.href = '/Dashboard/Documents/' + type
                                        }, 3000);
                                    } else {
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

    });

    $("#newdocumentMarc").data("validator").settings.ignore = "";

    $("#newdocumentMarc").submit(function (e) {
        e.preventDefault();

        var type = $(".txtDocumentType").val();

        if (type === "Ebook") {
            $(".issn").val("00000000");
        } else {
            $(".isbn").val("0000000000000");
        }
        var form = $(this).serializeArray()
        form.push({
            name: "DocumentGroup.DocumentType",
            value: type
        })


        if ($("#newdocumentMarc").valid()) {
            if (auth !== null) {
                $.ajax({
                    type: "POST",
                    url: "/Dashboard/NewAuthor",
                    data: "Name=" + $('.IdAuthor').select2('data')[0].text,
                    async: false,
                    success: function (data) {
                        form.find(item => item.name === "DocumentGroup.IdAuthor").value = data.data.extra;
                    }
                })
            } else {
                form.find(item => item.name === "DocumentGroup.IdAuthor").value = 0;

            }
            $.ajax({
                type: "POST",
                url: "/Dashboard/NewDocument/",
                data: form,
                async: false,
                success: function (data) {
                    if (data.data.success) {
                        DocId = data.data.extra

                        var files = null
                        $('.NewPhotoUpload').each(function (i, obj) {
                            if ($(obj).prop("files").length > 0) {
                                files = $(this).prop("files")
                            }
                        })


                        if (files !== null) {
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
                                            $(".productImg").hide();
                                            $(".PicturePBClass").show();
                                            $(".PicturePB").width(percentComplete + '%').html(percentComplete + '%');
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
                                    $(".PicturePB").width('0%').html('0%');

                                },
                                success: function (data) {
                                    if (!data.data.success) {
                                        $(".productImg").show()
                                        $(".PicturePB").hide()
                                    }
                                }
                            })

                        }
                        if ($("#GoogleApiBtnCover1").is(":checked")) {

                            if ($("#MarcCoverApi").prop('src') !== "")
                                $.ajax({
                                    type: 'POST',
                                    url: "/Dashboard/GoogleCover/",
                                    data: {url: $("#MarcCoverApi").prop('src'), id: data.data.extra},
                                    success: function (data) {
                                    }
                                })
                        }

                        var doc = null
                        $('.NewIdFile').each(function (i, obj) {
                            if ($(obj).prop("files").length > 0) {
                                doc = $(this).prop("files")
                            }
                        })

                        if (doc !== null) {
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
                                            $(".NewIdFile").hide();
                                            $(".DocumentPBClass").show();
                                            $(".DocumentPB").width(percentComplete + '%').html(percentComplete + '%');
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
                                    $(".DocumentePB").width('0%').html('0%');

                                },
                                success: function (data) {
                                    if (data.data.success) {
                                        if (sum.length > 0) {
                                            $.each(form, function (i, obj) {
                                                if (obj.name === "IdDocument") {
                                                    obj.value = DocId;
                                                }
                                            })
                                            $.ajax({
                                                url: "/Dashboard/EditDocumentSummary/",
                                                type: "POST",
                                                data: form,
                                                success: function (data) {
                                                    if (data.data.success) {
                                                        setTimeout(function () {
                                                            window.location.href = '/Dashboard/Documents/' + type
                                                        }, 3000);
                                                    }
                                                }
                                            })
                                        } else {
                                            setTimeout(function () {
                                                window.location.href = '/Dashboard/Documents/' + type
                                            }, 3000);
                                        }

                                    } else {
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
            })

        }

    });

    $("#SearchGoogleApiBtn").on("click", function () {
        let searchValue = $("#SearchGoogleApi").val()

        $.getJSON('https://www.googleapis.com/books/v1/volumes?q=' + searchValue, function (data) {
            if (data.totalItems === 0) {

            } else {

                $("#SearchGoogleApiBtn").hide()
                $("#SearchGoogleApi").hide()
                var recordList = []
                $.each(data.items, function (i, v) {
                    recordList.push({
                        id: i,
                        text: v.volumeInfo.title,
                    });
                })
                $("#GoogleApiListRecords").select2({
                    data: recordList,
                }).show();
                GoogleApiData = data.items
                setFormHeight()

            }


        });
    })
    $("#GoogleApiListRecords").on("change", function () {
        $("#GoogleApiCoverApi").attr("href", "")
        $(".NewIdFile").show();
        $(".DocumentPBClass").hide();

        var id = $(this).select2('data')[0].id
        $.getJSON(GoogleApiData[id].selfLink, function (data) {

            var documentInfo = data.volumeInfo
            var author = data.volumeInfo.authors !== undefined ? GoogleApiData[id].volumeInfo.authors[0] : null

            $("#GoogleApiOriginalTitle").val(documentInfo.title)
            const languageNames = new Intl.DisplayNames(["en"], {
                type: 'language'
            });
            var year = new Date(documentInfo.publishedDate).getFullYear();
            $("#GoogleApiOriginalLanguage").val(languageNames.of(documentInfo.language) !== undefined ? languageNames.of(documentInfo.language) : "")
            $("#GoogleApiPublicationDate").val(!isNaN(year) ? year : "")
            $(".txtDocumentType").val("Ebook").attr("disabled", true).trigger('change');
            $("#GoogleApiSubtitle").val(documentInfo.subtitle !== undefined ? documentInfo.subtitle : "")
            $("#GoogleApiAbstract").html(documentInfo.description !== undefined ? documentInfo.description : "")
            try {
                $("#GoogleApiKeyword").val(documentInfo.categories.join("\n"))
            } catch {
                $("#GoogleApiKeyword").val("")
            }
            $("#GoogleApiCategory").val(documentInfo.categories !== undefined ? documentInfo.categories[0] : "")
            try {
                if (documentInfo.industryIdentifiers[0].type !== "OTHER")
                    if (documentInfo.industryIdentifiers[0].identifier !== undefined) {
                        $("#GoogleApiIsbn").val(documentInfo.industryIdentifiers[0].identifier)
                    } else if (documentInfo.industryIdentifiers[0].identifier !== undefined) {
                        $("#GoogleApiIsbn").val(documentInfo.industryIdentifiers[1].identifier)
                    } else {
                        $("#GoogleApiIsbn").val("")
                    }
            } catch {

            }
            var physical = "";
            if (documentInfo.dimensions !== undefined) {
                $.each(documentInfo.dimensions, function (i, data) {
                    physical += data + "."
                })
            }
            $("#GoogleApiPhysicalDescription").val(physical)
            $("#GoogleApiGenre").val(documentInfo.maturityRating !== undefined ? documentInfo.maturityRating : "")
            $("#GoogleApiNbPages").val(documentInfo.pageCount !== undefined ? documentInfo.pageCount : "0")
            try {
                $("#GoogleApiBtnCover2").show()
                $("#GoogleApiCoverApi").show()

                var Code = $("#GoogleApiIsbn").val();
                if (Code === "") {
                    Code = $("#GoogleApiOriginalTitle").val()
                }
                $.ajax({
                    type: "GET",
                    url: "/Dashboard/GetAmazonBookCover",
                    data: "Code=" + Code + "&Language=" + $("#GoogleApiOriginalLanguage").val(),
                    async: false,
                    success: function (data) {
                        if (data.data !== null)
                            $("#GoogleApiCoverApi").attr("src", data.data)
                        else {
                            var BookCover = documentInfo.imageLinks.thumbnail.replace("zoom=1", "zoom=1")
                            $("#GoogleApiCoverApi").attr("src", BookCover)
                        }
                    }
                })


            } catch {
                $("#GoogleApiBtnCover2").hide()
                $("#GoogleApiCoverApi").hide()
            }

            if (author !== null) {
                $('.IdAuthor').append(new Option(author, 0, false, true)).trigger('change');

            } else $('.IdAuthor').append(new Option("No Author were found", -1, false, true)).trigger('change');

            $("#RecordGoogleApi").show()

            setFormHeight()
        })

    })

    $("#newdocumentGoogleApi").data("validator").settings.ignore = "";
    $("#newdocumentGoogleApi").submit(function (e) {
        e.preventDefault();

        var type = $(".txtDocumentType").val();

        if (type === "Ebook") {
            $(".issn").val("00000000");
        } else {
            $(".isbn").val("0000000000000");
        }

        var form = $(this).serializeArray()
        form.push({
            name: "DocumentGroup.DocumentType",
            value: type
        })


        if ($("#newdocumentGoogleApi").valid()) {
            if (auth !== null) {
                $.ajax({
                    type: "POST",
                    url: "/Dashboard/NewAuthor",
                    data: "Name=" + $('.IdAuthor').select2('data')[0].text,
                    async: false,
                    success: function (data) {
                        form.find(item => item.name === "DocumentGroup.IdAuthor").value = data.data.extra;
                    }
                })
            } else {
                form.find(item => item.name === "DocumentGroup.IdAuthor").value = 0;

            }
            $.ajax({
                type: "POST",
                url: "/Dashboard/NewDocument/",
                data: form,
                async: false,
                success: function (data) {
                    console.log(data.data.success === true)
                    if (data.data.success === true) {
                        DocId = data.data.extra

                        var files = null
                        $('.NewPhotoUpload').each(function (i, obj) {
                            if ($(obj).prop("files").length > 0) {
                                files = $(this).prop("files")
                            }
                        })


                        if (files !== null) {
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
                                            $(".productImg").hide();
                                            $(".PicturePBClass").show();
                                            $(".PicturePB").width(percentComplete + '%').html(percentComplete + '%');
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
                                    $(".PicturePB").width('0%').html('0%');

                                },
                                success: function (data) {
                                    if (!data.data.success) {
                                        $(".productImg").show()
                                        $(".PicturePB").hide()
                                    }
                                }
                            })

                        }
                        if ($("#GoogleApiBtnCover2").is(":checked")) {

                            if ($("#GoogleApiCoverApi").prop('src') !== "")
                                $.ajax({
                                    type: 'POST',
                                    url: "/Dashboard/GoogleCover/",
                                    data: {url: $("#GoogleApiCoverApi").prop('src'), id: data.data.extra},
                                    success: function (data) {
                                    }
                                })
                        }

                        var doc = null
                        $('.NewIdFile').each(function (i, obj) {
                            if ($(obj).prop("files").length > 0) {
                                doc = $(this).prop("files")
                            }
                        })

                        if (doc !== null) {
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
                                            $(".NewIdFile").hide();
                                            $(".DocumentPBClass").show();
                                            $(".DocumentPB").width(percentComplete + '%').html(percentComplete + '%');
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
                                    $(".DocumentePB").width('0%').html('0%');

                                },
                                success: function (data) {
                                    if (data.data.success) {
                                        /*setTimeout(function () {
                                            window.location.href = '/Dashboard/Documents/' + type
                                        }, 3000);
                                        
                                    
                                         */
                                    } else {
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
            })

        }

    });

    $('form input').on('keypress', function (e) {
        return e.which !== 13;
    });

});


function addSummary(obj) {
    var newId = obj + 100
    $(
        `
                    <div class="row" id="${newId}">
                        <input type="hidden" name="Id" value="0">
                        <input class="docID" id="" type="hidden" name="IdDocument" >
                        <div class="col-12 col-sm-6">
                            <label>Title</label>&nbsp;<span style="color:red;">*</span>&nbsp;<span class="text-danger" data-valmsg-replace="true"></span>
                            <input name="Title" class=" form-control" data-val="true" data-val-required="Required field" required>
                        </div>
                        <div class="col-6 col-sm-2 mt-3 mt-sm-0">
                            <label>Start</label>&nbsp;<span style="color:red;">*</span>&nbsp;<span class="text-danger" data-valmsg-replace="true"></span>
                            <input name="Start" type="number" class=" form-control" data-val="true" data-val-required="Required field" min="0" step="1" required>
                        </div>
                        <div class="col-6 col-sm-2 mt-3 mt-sm-0">
                            <label>End</label>&nbsp;<span style="color:red;">*</span>&nbsp;<span class="text-danger" data-valmsg-replace="true"></span>
                            <input name="End" type="number" class=" form-control" data-val="true" data-val-required="Required field" min="0" step="1" required>
                        </div>
                        <div class="col-6 col-sm-1 mt-3 mt-sm-5 text-center">
                            <a onclick="addSummary(${newId})" href="#"> <i class="fa fa-plus" aria-hidden="true"></i></a>
                        </div>
                        <div class="col-6 col-sm-1 mt-3 mt-sm-5 text-center">
                            <a onclick="deleteSummary(${newId})" href="#"> <i class="fa fa-minus" aria-hidden="true"></i></a>
                        </div>
                        <div class="col-12 mt-2">
                            <hr/>
                        </div>
                    </div>
                    
                            
                    `
    ).insertAfter($("#" + obj));

}

function deleteSummary(obj) {
    $("#" + obj).remove();
}