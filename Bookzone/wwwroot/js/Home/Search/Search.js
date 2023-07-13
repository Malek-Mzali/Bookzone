var collection = [];
var editor = [];
var data = dataArr


$(document).ready(function () {
    function LoadDataResult() {

        $("#SearchResult").html("")
        if (data.length === 0) {
            $("#SearchResult").append(`
            <div class="row m-auto">
                <div class="col-12">
                    <img class="img-fluid" src="/img/assets/NoResult.gif" alt=""/>
                </div>
            </div>
        `)
        } else {
            $.each(data, function (i, obj) {
                $("#SearchResult").append(`
            <div class="row m-auto">
                        <div class="col-md-4">
                            <a href="${obj.DocumentGroup.Url}"><img width="100%" height="100%" src="/img/document/${obj.DocumentGroup.CoverPage}" alt=""></a>
                        </div>
                        <div class="col-md-7">
                            <div class="book-detail-pub mt-3 mt-md-0">
                                <h2 class="book-title text-uppercase cursor-pointer " ><a class="text-body" href="${obj.DocumentGroup.Url}">${obj.DocumentGroup.OriginalTitle}</a></h2>
                            </div>
                            <div class="book-detail-pub">
                                <span><strong>${obj.DocumentGroup.DocumentType === "Ebook" ? "ISBN" : "ISSN"}</strong> : <span>${obj.DocumentGroup.DocumentType === "Ebook" ? obj.EbookGroup.Isbn : obj.EjournalGroup.Issn}</span></span>
                            </div>
                            <div class="book-detail-pub">
                                <span><strong>Publisher : </strong ><span class="txtEditor" data-id="${obj.DocumentGroup.IdEditor}"></span></span>
                            </div>
                            <div class="book-detail-pub">
                                <span><strong>Collection : </strong><span class="txtCollection" data-id="${obj.DocumentGroup.IdCollection}"></span></span>
                            </div>
                            <div class="book-detail-pub">
                                <span><strong>Author : </strong><span data-bs-toggle="modal" data-bs-target="#AuthorInfo" class="text-capitalize  txtAuthor"  data-id="${obj.DocumentGroup.IdAuthor}"></span></span>
                            </div>
                            <div class="book-detail-pub">
                                <span><strong>Publication date : </strong><span >${obj.DocumentGroup.PublicationDate}</span></span>
                            </div>
                            <div class="book-detail-pub">
                                <span><strong>Price : </strong><span >${obj.DocumentGroup.Price} DT</span></span>
                            </div>
                            

                            <div class="module line-clamp">
                                <span class="abstract">
                                    ${obj.DocumentGroup.Abstract}
                                </span>
                            </div>

                        </div>
                        <div class="col-4 col-md-1 m-auto">
                            <a href="${obj.DocumentGroup.Url}" class="text-black-50  IconHover"><i class="fa fa-eye"></i></a>
                            <a  class="text-black-50  IconHover cursor-p" data-id="${obj.DocumentGroup.Id}" onclick="AddToCart(this)"><i class="fa fa-cart-plus"></i></a>
                            <a  class="text-black-50  IconHover cursor-p" data-id="${obj.DocumentGroup.Id}" onclick="AddToWishList(this)"><i class="fa fa-heart"></i></a>
                            
                        </div>
                        <div class="col-12">
                            <div class="divider div-transparent div-dot mt-3 mb-3"></div>
                        </div>


                    </div>
        `)
            })

            $('.txtCollection').each(function (i, obj) {
                $.ajax({
                    type: "GET",
                    url: "/Dashboard/GetCollectionBy/",
                    data: "&Id=" + $(obj).attr("data-id"),
                    async: false,
                    success: function (data) {
                        if (!collection.some(o => o.id === data.data.id)) {
                            collection.push(data.data);
                        }
                        $(obj).html(data.data.title).prop('title', data.data.description);
                    }
                });
            });

            $('.txtEditor').each(function (i, obj) {
                $.ajax({
                    type: "GET",
                    url: "/Dashboard/GetUserEditorBy/",
                    data: "&Id=" + $(obj).attr("data-id"),
                    async: false,
                    success: function (data) {
                        if (!editor.some(o => o.editorGroup.id === data.data.editorGroup.id)) {
                            editor.push(data.data);
                        }
                        $(obj).html(data.data.editorGroup.name).prop('title', data.data.editorGroup.about);
                    }
                });
            });

            $('.txtAuthor').each(function (i, obj) {
                $.ajax({
                    type: "GET",
                    url: "/Dashboard/GetAuthorBy/",
                    data: "&Id=" + $(obj).attr("data-id"),
                    success: function (data) {
                        $(obj).html(data.data.name).prop('title', data.data.biography);
                    }
                });
            });

            $('.abstract').each(function () {
                var $p = $(this);
                var text = $p.text();
                var shortString = text.substring(0, 200) + '...';
                $p.text(shortString);

            });
        }

    }

    LoadDataResult()


    $.ajax({
        type: "GET",
        url: "/Dashboard/GetAllEditorByTheme/",
        data: "&Id=" + $("#txtTheme").val(),
        async: false,
        success: function (data) {
            if (data.data.length === 0) {
                $.each(editor, function (i, v) {
                    $('#PublisherList').append(`
                    <div class="form-check">
                        <input class="form-check-input publisherCheckbox" type="checkbox" value="${v.editorGroup.id}" id="${v.editorGroup.id}" checked>
                        <label class="form-check-label" for="${v.editorGroup.id}">
                            ${v.editorGroup.name}
                        </label>
                    </div>
                    `);
                })

            } else {
                $.each(data.data, function (i, v) {
                    $('#PublisherList').append(`
                    <div class="form-check">
                        <input class="form-check-input publisherCheckbox" type="checkbox" value="${v.id}" id="${v.id}" checked>
                        <label class="form-check-label" for="${v.id}">
                            ${v.name}
                        </label>
                    </div>
                    `);
                })
            }

        }
    });

    $.ajax({
        type: "GET",
        url: "/Dashboard/GetAllCollectionByTheme/",
        data: "&Id=" + $("#txtTheme").val(),
        async: false,
        success: function (data) {
            if (data.data.length === 0) {
                $.each(collection, function (i, v) {
                    $('#CollectionList').append(`
                    <div class="form-check">
                        <input class="form-check-input collectionCheckbox" type="checkbox" value="${v.id}" id="${v.id}" checked>
                        <label class="form-check-label" for="${v.id}">
                            ${v.title}
                        </label>
                    </div>
                    `);
                })

            } else {
                $.each(data.data, function (i, v) {
                    $('#CollectionList').append(`
                    <div class="form-check">
                        <input class="form-check-input collectionCheckbox" type="checkbox" value="${v.id}" id="${v.id}" checked>
                        <label class="form-check-label" for="${v.id}">
                            ${v.title}
                        </label>
                    </div>
                    `);
                })
            }
        }
    });

    $.ajax({
        type: "GET",
        async: false,
        url: "/Dashboard/GetAllAuthor/",
        success: function (data) {
            $.each(data.data, function (i, v) {
                $('#AuthorList').append(`
                    <div class="form-check">
                        <input class="form-check-input authorCheckbox" type="checkbox" value="${v.id}" id="${v.id}" checked>
                        <label class="form-check-label" for="${v.id}">
                            ${v.name}
                        </label>
                    </div>
                    `);
            })
        }
    });

    $.ajax({
        type: "GET",
        url: "/Dashboard/GetDateRange/",
        async: false,
        success: function (data) {
            $('#DateList').append(`
                <label for="customRange3" class="form-label">Publication Date</label>
                <input type="range" class="form-range" min="${data.data[0]}" max="${data.data[1]}" step="1" id="customRange3" oninput="num.value = this.value" value="${data.data[0]}">
                <output id="num">${data.data[0]}</output>
                            
            `);
        }
    });

    function sort(value) {

        if (value === "Date") {
            data.sort(function (a, b) {
                return a.DocumentGroup.PublicationDate - b.DocumentGroup.PublicationDate;
            });

        } else {
            data.sort(function (a, b) {
                return a.DocumentGroup.OriginalTitle.localeCompare(b.DocumentGroup.OriginalTitle);
            });
        }
    }

    $('#sortType').on('change', function () {
        sort(this.value)
        LoadDataResult()
    });

    $('.documentTypeCheckbox').each(function (i, obj) {
        $(obj).on('change', function () {
            var id = this.value
            if ($(this).is(":checked")) {
                $.each(dataArr.filter(function (obj) {
                    return obj.DocumentGroup.DocumentType === id;
                }), function (i, obj) {
                    data.push(obj)
                })
                sort($('#sortType').val())
                LoadDataResult()
            } else {
                data = data.filter(function (obj) {
                    return obj.DocumentGroup.DocumentType !== id;
                })
                sort($('#sortType').val())
                LoadDataResult()
            }
            var textinputs = document.querySelectorAll('.documentTypeCheckbox');
            var empty = [].filter.call(textinputs, function (el) {
                return !el.checked
            });

            if (textinputs.length === empty.length) {
                data = []
                LoadDataResult()
                return false;
            }
        })

    })

    $('.publisherCheckbox').each(function (i, obj) {
        $(obj).on('change', function () {
            var id = this.value
            if ($(this).is(":checked")) {
                $.each(dataArr.filter(function (obj) {
                    return obj.DocumentGroup.IdEditor === parseInt(id);
                }), function (i, obj) {
                    data.push(obj)
                })
                sort($('#sortType').val())
                LoadDataResult()
            } else {
                data = data.filter(function (obj) {
                    return obj.DocumentGroup.IdEditor !== parseInt(id);
                })
                sort($('#sortType').val())
                LoadDataResult()
            }
            var textinputs = document.querySelectorAll('.publisherCheckbox');
            var empty = [].filter.call(textinputs, function (el) {
                return !el.checked
            });

            if (textinputs.length === empty.length) {
                data = []
                LoadDataResult()
                return false;
            }
        })

    })

    $('.collectionCheckbox').each(function (i, obj) {
        $(obj).on('change', function () {
            var id = this.value
            if ($(this).is(":checked")) {
                $.each(dataArr.filter(function (obj) {
                    return obj.DocumentGroup.IdCollection === parseInt(id);
                }), function (i, obj) {
                    data.push(obj)
                })
                sort($('#sortType').val())
                LoadDataResult()
            } else {
                data = data.filter(function (obj) {
                    return obj.DocumentGroup.IdCollection !== parseInt(id);
                })
                sort($('#sortType').val())
                LoadDataResult()
            }
            var textinputs = document.querySelectorAll('.collectionCheckbox');
            var empty = [].filter.call(textinputs, function (el) {
                return !el.checked
            });

            if (textinputs.length === empty.length) {
                data = []
                LoadDataResult()
                return false;
            }
        })

    })

    $('.authorCheckbox').each(function (i, obj) {
        $(obj).on('change', function () {
            var id = this.value
            if ($(this).is(":checked")) {
                $.each(dataArr.filter(function (obj) {
                    return obj.DocumentGroup.IdAuthor === parseInt(id);
                }), function (i, obj) {
                    data.push(obj)
                })
                sort($('#sortType').val())
                LoadDataResult()
            } else {
                data = data.filter(function (obj) {
                    return obj.DocumentGroup.IdAuthor !== parseInt(id);
                })
                sort($('#sortType').val())
                LoadDataResult()
            }
            var textinputs = document.querySelectorAll('.authorCheckbox');
            var empty = [].filter.call(textinputs, function (el) {
                return !el.checked
            });

            if (textinputs.length === empty.length) {
                data = []
                LoadDataResult()
                return false;
            }
        })

    })

    $('.languageCheckbox').each(function (i, obj) {
        $(obj).on('change', function () {
            var id = this.value
            if ($(this).is(":checked")) {
                $.each(dataArr.filter(function (obj) {
                    return obj.DocumentGroup.OriginalLanguage === id;
                }), function (i, obj) {
                    data.push(obj)
                })
                sort($('#sortType').val())
                LoadDataResult()
            } else {
                data = data.filter(function (obj) {
                    return obj.DocumentGroup.OriginalLanguage !== id;
                })
                sort($('#sortType').val())
                LoadDataResult()
            }
            var textinputs = document.querySelectorAll('.languageCheckbox');
            var empty = [].filter.call(textinputs, function (el) {
                return !el.checked
            });

            if (textinputs.length === empty.length) {
                data = []
                LoadDataResult()
                return false;
            }
        })

    })

    $('#customRange3').on('change', function () {
        data = dataArr.filter(function (obj) {
            return obj.DocumentGroup.PublicationDate >= parseInt($("#customRange3").val());
        })
        sort($('#sortType').val())
        LoadDataResult()
    });

});