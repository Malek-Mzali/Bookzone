
let dataresult;

$(document).ready(function () {
    
    loadDataTable();

    
    $("#editCollectionform").submit(function(e) {

        e.preventDefault();

        var form = $(this).serialize();

        if($("#editCollectionform").valid()){
            $.ajax({
                type: "POST",
                url: "/Dashboard/EditCollection/",
                data: form,
                success: function(data)
                {
                    if (data.data.success){
                        loadDataTable();
                        $('#editCollection').modal('hide');
                    }
                }
            });
        }
    });
    
    
   
});




function deleteCollectionDash(id ) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger me-3'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "Are you sure that you want to delete this Collection?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "/Dashboard/DeleteCollection/",
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



function EditBtnCollection(obj){
    var x = $(obj).closest('tr').index()

    $("#txtId").val(dataresult.data[x].id);
    $("#txtTitle").val(dataresult.data[x].title);
    $("#txtShortTitle").val(dataresult.data[x].shortTitle);
    $("#txtDescription").val(dataresult.data[x].description);


    $.ajax({
        type: 'GET',
        url: '/Dashboard/GetThemeByCollection/',
        data: '&Id='+ dataresult.data[x].id
    }).then(function (data) {
        $("#IdTheme").append(new Option(data.data.title, data.data.id, true, true)).trigger('change');
    });

    $.ajax({
        type: 'GET',
        url: '/Dashboard/GetUserEditorBy/',
        data: '&Id='+ dataresult.data[x].idEditor
    }).then(function (data) {
        if (data.data.id === 0){
            $("#IdEditor").append(new Option("No Editor found", 0, true, true)).trigger('change');
        }else {
            $("#IdEditor").append(new Option(data.data.editorGroup.name, data.data.editorGroup.id, true, true)).trigger('change');
        }
    });
    
    

}

function loadDataTable() {

    if ($.fn.DataTable.isDataTable('#CollectionList')) {

        $('#CollectionList').dataTable().fnClearTable();
        $('#CollectionList').dataTable().fnDestroy();

    }
    
    var url = "/Dashboard/GetAllCollection/"
    if ($("#txtRoleUser").val() === "Editor"){
        url = "/Dashboard/GetCollectionForEditor/"
    }
    
                
    dataTable = $('#CollectionList').dataTable({
        "bAutoWidth": false, // Disable the auto width calculation 

        "ajax": {
            "url": url,
            "data": {  'id' : $("#txtUserId").val() },
            "type": "GET",
            "datatype": "json"
        },
        "drawCallback": function (settings) {
            // Here the response
            dataresult = settings.json;
        },
        "columns": [
            {"data": "id"},
            {"data": "idEditor"},
            {"data": "idTheme"},
            {"data": "title", },
            {"data": "shortTitle", },
            {"data": "description", },

            {
                "data": "id",

                "render": function (data) {


                    return `
                     <div class="text-center">
                        <a class="nav-link dropdown-toggle larger-zone action" data-bs-toggle="dropdown" href="#"
                           id="dropdownMenuButton2"><i class="fa fa-ellipsis-v"></i></a>
                        <ul aria-labelledby="dropdownMenuButton2" class="dropdown-menu dropdown-menu-right" id="dropdownList2">
                            <li>
                                <a class="dropdown-item" href="#" data-bs-toggle="modal" onclick="EditBtnCollection(this)" data-bs-target="#editCollection"><i class="fas fa-edit m-r-5"></i> Edit</a>
                            </li>
                            <li>
                                <a class="dropdown-item"  href="#" onclick="deleteCollectionDash(${data})" ><i
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