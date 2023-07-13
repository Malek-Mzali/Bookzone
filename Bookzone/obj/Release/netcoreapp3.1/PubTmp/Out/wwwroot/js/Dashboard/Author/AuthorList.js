
let dataresult;

$(document).ready(function () {
    
    loadDataTable();

    
    $("#editAuthorform").submit(function(e) {

        e.preventDefault();

        var form = $(this).serialize();

        if($("#editAuthorform").valid()){
            $.ajax({
                type: "POST",
                url: "/Dashboard/EditAuthor/",
                data: form,
                success: function(data)
                {
                    if (data.data.success){
                        loadDataTable();
                        $('#editAuthor').modal('hide');
                    }
                }
            });
        }
    });
    
    
   
});




function deleteAuthorDash(id ) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger me-3'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "Are you sure that you want to delete this Author?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "/Dashboard/DeleteAuthor/",
                type: "GET",
                data:
                    {
                        "id": id
                    },
                success: function (msg) {
                    if (msg.data.success) {
                        swalWithBootstrapButtons.fire("Deleted!", msg.data.message, "success");
                        loadDataTable();
                    } else {
                        swalWithBootstrapButtons.fire("Oops", msg.data.message, "error");
                    }

                },
            })
        } });
}



function EditBtnAuthor(obj){
    var x = $(obj).closest('tr').index()

    $("#txtId").val(dataresult.data[x].id);
    $("#txtName").val(dataresult.data[x].name);
    $("#txtBiography").val(dataresult.data[x].biography);
    $("#txtPhoto").attr('src', "/img/author/"+dataresult.data[x].photo)

}

function ViewBtnAuthor(obj){
    var x = $(obj).closest('tr').index()

    $("#AuthorPhoto").attr("src", "/img/author/"+dataresult.data[x].photo)


    if(dataresult.data[x].biography === ""){
        $("#txtBiography2").html("No data available");
    }else {
        $("#txtBiography2").html(dataresult.data[x].biography);
    }
    
    console.log($("#txtId").val())
    
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
            "data": { 'value': dataresult.data[x].id},
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

function loadDataTable() {

    if ($.fn.DataTable.isDataTable('#AuthorList')) {

        $('#AuthorList').dataTable().fnClearTable();
        $('#AuthorList').dataTable().fnDestroy();

    }

    dataTable = $('#AuthorList').dataTable({
        "bAutoWidth": false, // Disable the auto width calculation 

        "ajax": {
            "url": "/Dashboard/GetAllAuthor/",
            "type": "GET",
            "datatype": "json"
        },
        "drawCallback": function (settings) {
            // Here the response
            dataresult = settings.json;
        },
        "columns": [
            {"data": "id"},
            {
                "data": "photo",
                "render": function (data) {
                    return '<img src="/img/author/' + data + '" alt="' + data + '" class="avatar avatar-md mx-3"/>';
                }
            },
            {"data": "name", },
            {"data": "biography", },

            {
                "data": "id",

                "render": function (data) {


                    return `
                     <div class="text-center">
                        <a class="nav-link dropdown-toggle larger-zone action" data-bs-toggle="dropdown" href="#"
                           id="dropdownMenuButton2"><i class="fa fa-ellipsis-v"></i></a>
                        <ul aria-labelledby="dropdownMenuButton2" class="dropdown-menu dropdown-menu-right" id="dropdownList2">
                            <li>
                                <a class="dropdown-item" href="#" data-bs-toggle="modal" onclick="ViewBtnAuthor(this)" data-bs-target="#AuthorInfo"><i class="fas fa-eye m-r-5"></i> View</a>
                            </li>
                            
                            <li>
                                <a class="dropdown-item" href="#" data-bs-toggle="modal" onclick="EditBtnAuthor(this)" data-bs-target="#editAuthor"><i class="fas fa-edit m-r-5"></i> Edit</a>
                            </li>
                            <li>
                                <a class="dropdown-item"  href="#" onclick="deleteAuthorDash(${data})" ><i
                                    class="fas fa-trash m-r-5"></i> Delete</a>
                            </li>
                        </ul>
                    </div>
                    `;
                },
            }
        ],
        "columnDefs": [
            {"class": "dt-center", "targets": "_all"},
        ],


        responsive: true
    });
}