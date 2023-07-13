$(document).ready(function () {


    $.ajax({
        type: "GET",
        url: "/Dashboard/GetCollectionBy/",
        data: "&Id=" + $("#txtCollectionId").val(),
        success: function (data) {
            $("#txtCollection").html(data.data.title).prop('title', data.data.description);
        }
    });
    $.ajax({
        type: "GET",
        url: "/Dashboard/GetThemeByCollection/",
        data: "&Id=" + $("#txtCollectionId").val(),
        success: function (data) {
            $("#txtTheme").html(data.data.title).prop('title', data.data.description);
        }
    });
    $.ajax({
        type: "GET",
        url: "/Dashboard/GetUserEditorBy/",
        data: "&Id=" + $("#txtEditorId").val(),
        success: function (data) {
            $("#txtEditor").html(data.data.editorGroup.name).attr('href', "/Home/Profile?id=" + data.data.editorGroup.id);
        }
    });
    $.ajax({
        type: "GET",
        url: "/Dashboard/GetAuthorBy/",
        data: "&Id=" + $("#txtAuthorId").val(),
        success: function (data) {
            if (data.data.name !== null)
                $("#txtAuthor").html(data.data.name);
            else $("#txtAuthor").html("Author not available");
            $("#AuthorPhoto").attr("src", "/img/author/" + data.data.photo);

            if (data.data.biography === "") {
                $("#txtBiography").html("No data available");
            } else {
                $("#txtBiography").html(data.data.biography);
            }
            dataTable = $('#DocumentListAuthor').dataTable({
                "bAutoWidth": false, // Disable the auto width calculation 
                "bFilter": false,
                "bInfo": false,
                "dSorting": false,
                "bLengthChange": false,
                "pageLength": 5,
                "dom": 'rtp',
                "drawCallback": function () {
                    $(this.api().table().header()).hide();
                },
                "ajax": {
                    "url": "/Dashboard/GetAllAuthorDocument/",
                    "data": {'value': $("#txtAuthorId").val()},
                    "type": "GET",
                    "datatype": "json"
                },
                "columns": [
                    {
                        "data": "documentGroup",
                        "render": function (data) {
                            return '<a class="btn-link username" href="/Home/Document?id=' + data.id + '">' + data.originalTitle + '</a>';
                        }
                    },

                ],

                responsive: true
            });


        }
    });
    $.ajax({
        type: 'GET',
        url: '/Dashboard/GetAllDocumentSummary/',
        data: '&Id=' + $("#txtId").val(),
        success: function (data) {

            if (data.data.length === 0) {
                $('#SummaryList').append(`
                     <tr>
                        <td style="width: 80%">No summary available</td>
                        <td></td>
                        <td></td>
                     </tr>
                            
                    `);
            } else {
                $.each(data.data, function (key, value) {
                    var x = ""
                    if (typeof $("#View").attr('href') != 'undefined')
                        x = `window.open('/Home/Summary?auth=${btoa($("#txtuserID").val() + ";" + $("#txtId").val() + ";" + value.start + ";" + value.end)}   ','_blank')`
                    $('#SummaryList').append(`
                     <tr class="cursor-p" onclick="${x}">
                        <td style="width: 80%" class="align-middle">${value.title}</td>
                        <td class="text-center align-middle">${value.start}</td>
                        <td class="text-center align-middle">${value.end}</td>
                     </tr>
                    `);
                });
            }
        }
    })


    var quill = new Quill('#snow-container', {
        placeholder: 'New comment ...',
        theme: 'snow',
        formats: [
            'bold',
            'italic',
            'link',
            'list',
        ],
        modules: {
            toolbar: [
                ['bold', 'italic', 'link'],
                [{'list': 'ordered'}, {'list': 'bullet'}],
            ]
        }
    });
    LoadComments()

    $("#NewCommentForm").on("submit", function (e) {
        e.preventDefault()
        $("#NewComment").val(quill.root.innerHTML);
        var form = $(this).serialize() + "&IdDocument=" + $("#txtId").val();
        $.ajax({
            type: 'POST',
            url: '/Home/NewComment/',
            data: form,
            success: function (data) {
                if (data.data.success) {
                    LoadComments()
                    $("#NewComment").val("")
                    quill.setText('');
                }
            }
        })


    })
});

function LoadComments() {
    $('#CommentsSection').html("")
    $.ajax({
        type: 'GET',
        url: '/Home/GetAllComments/',
        data: '&IdDocument=' + $("#txtId").val(),
        success: function (data) {
            if (data.data.length === 0) {
                $('#CommentsSection').append(`
                        <div class="col-md-12">
                            <div class="media g-mb-30 media-comment border-bottom">
                                <div class="media-body  mb-3 pt-3 ">
                                    <p class="text-center">No Comment available</p>
                                </div>
                            </div>
                        </div>
                    `);
            } else {
                $.each(data.data, function (key, value) {
                    console.log(value)
                    $('#CommentsSection').append(`
                        <div class="col-md-12">
                            <div class="media g-mb-30 media-comment border-bottom">
                                <img class="d-flex g-width-50 g-height-50 rounded-circle g-mt-3 g-mr-15" src="/img/profile/${value.userPhoto}" alt="">
                                <div class="media-body  mb-3 px-3">
                                    <div class="g-mb-15">
                                        <h4 class="h4 g-color-gray-dark-v1 mb-0"><a class="text-reset text-decoration-none cursor-p" href="/Home/Profile?id=${value.idUser}">${value.userName}</a></h4>
                                        <span class="g-color-gray-dark-v4 g-font-size-12"> ${moment(`${value.date.replace("T", " ")}`, 'YYYY/MM/DD HH:mm:ss').format('YYYY-MM-DD   hh:mm A')}</span>
                                    </div>
                                    <p>${value.text}</p>

                                </div>
                                     <div class="text-right" id="${value.id}">
                                    </div>

                            </div>
                        </div>
                    `);
                    if ($("#txtRoleUser").val() === "Administrator") {
                        $('#' + value.id).append(`<a class="nav-link action text-danger cursor-p" onclick="DeleteCommentBtn(${value.id})"><i class="fa fa-trash "></i></a>`);
                    } else if ($("#txtuserID").val() === String(value.idUser)) {
                        $('#' + value.id).append(`<a class="nav-link action text-danger cursor-p" onclick="DeleteCommentBtn(${value.id})"><i class="fa fa-trash "></i></a>`);
                    }

                })
            }
        }
    })
}

function DeleteCommentBtn(id) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger mx-3'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "Are you sure that you want to delete this Comment?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "/Home/DeleteComment/",
                type: "GET",
                data:
                    {
                        "id": id
                    },
                success: function (msg) {
                    if (msg.data.success) {
                        swalWithBootstrapButtons.fire("Deleted!", msg.data.message, "success");
                        LoadComments()
                    } else {
                        swalWithBootstrapButtons.fire("Oops", msg.data.message, "error");
                    }

                },
            })


        }
    });
}

