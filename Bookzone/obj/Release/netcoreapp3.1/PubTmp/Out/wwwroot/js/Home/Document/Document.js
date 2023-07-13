$(document).ready(function() {
    

    $.ajax({
        type: "GET",
        url: "/Dashboard/GetCollectionBy/",
        data: "&Id="+$("#txtCollectionId").val(),
        success: function (data) {
            $("#txtCollection").html(data.data.title).prop('title', data.data.description);
        }
    });
    $.ajax({
        type: "GET",
        url: "/Dashboard/GetThemeByCollection/",
        data: "&Id="+$("#txtCollectionId").val(),
        success: function (data) {
            $("#txtTheme").html(data.data.title).prop('title', data.data.description);
        }
    });
    $.ajax({
        type: "GET",
        url: "/Dashboard/GetUserEditorBy/",
        data: "&Id="+$("#txtEditorId").val(),
        success: function (data) {
            $("#txtEditor").html(data.data.editorGroup.name).prop('title', data.data.editorGroup.about);
        }
    });
    $.ajax({
        type: "GET",
        url: "/Dashboard/GetAuthorBy/",
        data: "&Id="+$("#txtAuthorId").val(),
        success: function (data) {
            $("#txtAuthor").html(data.data.name);
            $("#AuthorPhoto").attr("src","/img/author/"+data.data.photo);

            if(data.data.biography === ""){
                $("#txtBiography").html("No data available");
            }else {
                $("#txtBiography").html(data.data.biography);
            }
            dataTable = $('#DocumentListAuthor').dataTable({
                "bAutoWidth": false, // Disable the auto width calculation 
                "bFilter": false,
                "bInfo": false,
                "dSorting":false,
                "bLengthChange": false,
                "pageLength" : 5,
                "dom": 'rtp',
                "drawCallback": function() {
                    $(this.api().table().header()).hide();
                },
                "ajax": {
                    "url": "/Dashboard/GetAllAuthorDocument/",
                    "data": { 'value': $("#txtAuthorId").val() },
                    "type": "GET",
                    "datatype": "json"
                },
                "columns": [
                    {
                        "data": "documentGroup",
                        "render": function (data) {
                            return '<a class="btn-link username" href="/Home/Document?id='+data.id+'">'+data.originalTitle+'</a>';
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
            
            if (data.data.length === 0){
                $('#SummaryList').append(`
                     <tr>
                        <td style="width: 80%">No summary available</td>
                        <td></td>
                        <td></td>
                     </tr>
                            
                    `);
            }else {
                $.each(data.data, function (key, value) {
                    var x = ""
                    if (typeof $("#View").attr('href') != 'undefined')
                        x = `window.open('${$("#View").attr('href')}'+'#page=${value.start}','_blank')`
                    $('#SummaryList').append(`
                     <tr class="cursor-p" onclick="${x}">
                        <td style="width: 80%">${value.title}</td>
                        <td>${value.start}</td>
                        <td>${value.end}</td>
                     </tr>
                    `);
                });
            }
        }
    })


});


