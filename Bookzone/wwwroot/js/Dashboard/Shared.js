$(document).ready(function () {

    $('.txtDocumentType').on('change', function () {
        if ($('.txtDocumentType').val() === "Ebook") {
            $(".Ejournal").hide()
            $(".Ebook").show()
            $("#Ejournal").hide()
            $("#Ebook").show()
        } else {
            $(".Ejournal").show()
            $(".Ebook").hide()
        }
        $('#tab-tab').html($('.txtDocumentType').val())
        $('#MarcType-tab').html($('.txtDocumentType').val())
        $('#GoogleApiType-tab').html($('.txtDocumentType').val())

    })
    $('#txtDocumentType').on('change', function () {
        if ($('#txtDocumentType').val() === "Ebook") {
            $("#Ejournal").hide()
            $("#Ebook").show()
        } else {
            $("#Ejournal").show()
            $("#Ebook").hide()
        }
        $('#tab-tab').html($('#txtDocumentType').val())

    })

    function formatRepoTheme(repo) {
        if (repo.loading) {
            return repo.text;
        }

        var $container = $(
            "<div class='bloc-icon'>" +
            "<div class='img-icon border-radius-md'>" +
            "<i class='select2-result-repository__avatar' id='txtIconDisplay'></i>" +
            "</div>" +
            "<div class='select2-result-repository__title text-icon'></div>" +


            "</div>"
        );

        $container.find(".select2-result-repository__title").text(repo.text);
        $container.find(".select2-result-repository__avatar").attr("class", repo.icon);


        return $container;
    }

    function formatRepoEditor(repo) {

        if (repo.loading) {
            return repo.text;
        }

        var $container = $(
            "<div class='bloc-icon'>" +
            "<div class='img-icon border-radius-md'>" +
            "<img src=''  class=\"img-icon select2-result-repository__avatar border-radius-md\">" +
            "</div>" +
            "<div class='select2-result-repository__title text-icon'></div>" +


            "</div>"
        );

        $container.find(".select2-result-repository__title").text(repo.text);
        $container.find(".select2-result-repository__avatar").attr("src", "/img/profile/" + repo.icon);


        return $container;
    }

    function formatRepoCollection(repo) {
        if (repo.loading) {
            return repo.text;
        }

        var $container = $(
            "<div class='bloc-icon'>" +
            "<div class='select2-result-repository__title text-icon'></div>" +


            "</div>"
        );

        $container.find(".select2-result-repository__title").text(repo.text);


        return $container;
    }

    function formatRepoAuthor(repo) {
        if (repo.loading) {
            return repo.text;
        }

        var $container = $(
            "<div class='bloc-icon'>" +
            "<div class='img-icon border-radius-md'>" +
            "<img src=''  class=\"img-icon select2-result-repository__avatar border-radius-md\">" +
            "</div>" +
            "<div class='select2-result-repository__title text-icon'></div>" +


            "</div>"
        );

        $container.find(".select2-result-repository__title").text(repo.text);
        $container.find(".select2-result-repository__avatar").attr("src", "/img/author/" + repo.icon);


        return $container;
    }


    $('#IdTheme').select2({
        ajax: {
            url: "/Dashboard/GetAllTheme/",
            dataType: 'json',
            processResults: function (data, query) {

                if (query.term === undefined) {
                    return {
                        results: $.map(data.data, function (item) {
                            return {
                                id: item.id,
                                text: item.title,
                                icon: item.icon
                            }
                        }),

                    };
                } else {
                    let term = new RegExp(query.term, 'gi'), matchedResults = [];
                    data.data.forEach(function (item) {
                        if (item.title.match(term)) {
                            matchedResults.push({
                                id: item.id,
                                text: item.title,
                                icon: item.icon
                            });
                        }
                    });
                    return {results: matchedResults};
                }

            },

        },
        placeholder: 'Search for a Theme',
        templateResult: formatRepoTheme,


    });

    $('.IdCollection').select2({placeholder: 'Search for a Collection'});

    if ($("#txtRoleUser").val() !== "Editor") {
        $('.IdEditor').select2({
            ajax: {
                url: "/Dashboard/GetAllUsers/",
                data: {type: "Editor"},
                dataType: 'json',
                processResults: function (data, query) {
                    if (query.term === undefined) {
                        return {
                            results: $.map(data.data, function (item) {
                                return {
                                    id: item.editorGroup.id,
                                    text: item.editorGroup.name,
                                    icon: item.usersGroup.photo
                                }
                            }),

                        };
                    } else {
                        let term = new RegExp(query.term, 'gi'), matchedResults = [];
                        data.data.forEach(function (item) {
                            if (item.editorGroup.name.match(term)) {
                                matchedResults.push({
                                    id: item.editorGroup.id,
                                    text: item.editorGroup.name,
                                    icon: item.usersGroup.photo
                                });
                            }
                        });
                        return {results: matchedResults};
                    }

                },

            },
            placeholder: 'Search for a Editor',
            templateResult: formatRepoEditor,


        });
    } else {
        $('.IdEditor').select2({
            ajax: {
                url: "/Dashboard/GetUserEditorBy/",
                data: "&Id=" + $("#txtUserId").val(),
                dataType: 'json',
                async: false,
                processResults: function (data, query) {

                    if (query.term === undefined) {
                        var matchedResults = []
                        matchedResults.push({
                            id: data.data.editorGroup.id,
                            text: data.data.editorGroup.name,
                            icon: data.data.usersGroup.photo
                        });

                        return {results: matchedResults};

                    } else {
                        let term = new RegExp(query.term, 'gi'), matchedResults = [];
                        if (data.data.editorGroup.name.match(term)) {
                            matchedResults.push({
                                id: data.data.editorGroup.id,
                                text: data.data.editorGroup.name,
                                icon: data.data.usersGroup.photo
                            });
                        }
                        return {results: matchedResults};
                    }

                },

            },
            placeholder: 'Search for a Editor',
            templateResult: formatRepoEditor,


        });

        $.ajax({
            type: 'GET',
            url: '/Dashboard/GetUserEditorBy/',
            data: '&Id=' + $("#txtUserId").val()
        }).then(function (data) {
            $(".IdEditor").append(new Option(data.data.editorGroup.name, data.data.editorGroup.id, true, true)).trigger('change');
        });
    }


    $(".IdEditor").on("select2:select", function () {
        $(".IdCollection").val([]);
        $('.IdCollection').select2({
            ajax: {
                url: "/Dashboard/GetCollectionForEditor/",
                data: '&Id=' + $(this).val(),
                dataType: 'json',
                processResults: function (data, query) {

                    if (query.term === undefined) {
                        return {
                            results: $.map(data.data, function (item) {
                                return {
                                    id: item.id,
                                    text: item.title,
                                }
                            }),

                        };
                    } else {
                        let term = new RegExp(query.term, 'gi'), matchedResults = [];
                        data.data.forEach(function (item) {
                            if (item.title.match(term)) {
                                matchedResults.push({
                                    id: item.id,
                                    text: item.title,
                                });
                            }
                        });
                        return {results: matchedResults};
                    }

                },

            },
            placeholder: 'Search for a Collection',
            templateResult: formatRepoCollection,
        });

    });


    $('.IdAuthor').select2({
        ajax: {
            url: "/Dashboard/GetAllAuthor/",
            dataType: 'json',
            processResults: function (data, query) {

                if (query.term === undefined) {
                    return {
                        results: $.map(data.data, function (item) {
                            return {
                                id: item.id,
                                text: item.name,
                                icon: item.photo
                            }
                        }),

                    };
                } else {
                    let term = new RegExp(query.term, 'gi'), matchedResults = [];
                    data.data.forEach(function (item) {
                        if (item.name.match(term)) {
                            matchedResults.push({
                                id: item.id,
                                text: item.name,
                                icon: item.photo
                            });
                        } else {
                            var newOption = new Option("No author", 0, true, true);
                            $('.IdAuthor').append(newOption).trigger('change');
                        }
                    });
                    return {results: matchedResults};
                }

            },

        },
        placeholder: 'Search for a Author',
        templateResult: formatRepoAuthor,


    });


    $("#PhotoUpload").change(function () {
        var files = $('#PhotoUpload').prop("files");
        let formData = new FormData();
        formData.append("myUploader", files[0]);
        formData.append("id", $("#txtId").val());
        formData.append("type", $("#PhotoUpload").data("type"));

        $.ajax({
            xhr: function () {
                var xhr = new window.XMLHttpRequest();

                xhr.upload.addEventListener("progress", function (evt) {
                    if (evt.lengthComputable) {
                        var percentComplete = evt.loaded / evt.total;
                        percentComplete = parseInt(percentComplete * 100);
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
                    $("#txtPhoto").attr("src", "/img/" + $("#PhotoUpload").data("type") + "/" + data.data.extra);
                }
            }
        });
    });

    $("#IdFile").change(function () {

        var doc = $('#IdFile').prop("files");


        if (doc.length > 0) {
            let formData = new FormData();
            formData.append("myUploader", doc[0]);
            formData.append("id", $("#txtId").val());
            formData.append("type", "documentFile");

            $.ajax({
                xhr: function () {
                    var xhr = new window.XMLHttpRequest();

                    xhr.upload.addEventListener("progress", function (evt) {
                        if (evt.lengthComputable) {
                            var percentComplete = evt.loaded / evt.total;
                            percentComplete = parseInt(percentComplete * 100);
                            $("#IdFile").hide();
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
                        $("#txtFile").val(data.data.extra);
                    }
                }
            })

        }
    });

    $("#deletePicture").click(function () {
        $("#PhotoUpload").val(null);

        let formData = new FormData();
        formData.append("myUploader", null);
        formData.append("id", $("#txtId").val());
        formData.append("type", $("#PhotoUpload").data("type"));

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
                    $("#txtPhoto").attr("src", "/img/" + $("#PhotoUpload").data("type") + "/default.png")
                }
            }
        })

    })

});

