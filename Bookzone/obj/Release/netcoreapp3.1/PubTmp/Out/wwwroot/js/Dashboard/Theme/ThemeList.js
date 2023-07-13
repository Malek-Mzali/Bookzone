
let dataresult;

$(document).ready(function () {
    
    loadDataTable();
    

    $("#editThemeform").submit(function(e) {

        e.preventDefault();

        var form = $(this).serialize();

        if($("#editThemeform").valid()){
            $.ajax({
                type: "POST",
                url: "/Dashboard/EditTheme/",
                data: form,
                success: function(data)
                {
                    if (data.data.success){
                        loadDataTable();
                        $('#editTheme').modal('hide');
                    }
                }
            });
        }
    });

    $("#txtIcon").on("change keyup paste", function(){
        $("#txtIconDisplay").attr("class", $("#txtIcon").val());
        
    })


});





function deleteThemeDash(id ) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger me-3'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "Are you sure that you want to delete this Theme?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "/Dashboard/DeleteTheme/",
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






function EditBtnTheme(obj){
    var x = $(obj).closest('tr').index()
    $("#txtId").val(dataresult.data[x].id);
    $("#txtTitle").val(dataresult.data[x].title);
    $("#txtShortTitle").val(dataresult.data[x].shortTitle);
    $("#txtDescription").val(dataresult.data[x].description);
    $("#txtIcon").val(dataresult.data[x].icon);
    $("#txtIconDisplay").attr("class", dataresult.data[x].icon);
    
}

function loadDataTable() {

    if ($.fn.DataTable.isDataTable('#ThemesList')) {

        $('#ThemesList').dataTable().fnClearTable();
        $('#ThemesList').dataTable().fnDestroy();

    }

    dataTable = $('#ThemesList').dataTable({
        "bAutoWidth": false, // Disable the auto width calculation 

        "ajax": {
            "url": "/Dashboard/GetAllTheme/",
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
                "data": "icon",
                "render": function (data) {
                    return `                                 
                    <div class="img-icon">
                        <i class="${data}"></i>
                    </div>
                    `;
                }
            },

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
                                <a class="dropdown-item" href="/Home/ebooks?id=${data}"><i  class="fas fa-eye m-r-5"></i> View</a>
                            </li>
                            <li>
                                <a class="dropdown-item" href="#" data-bs-toggle="modal" onclick="EditBtnTheme(this)" data-bs-target="#editTheme"><i class="fas fa-edit m-r-5"></i> Edit</a>
                            </li>
                            <li>
                                <a class="dropdown-item"  href="#" onclick="deleteThemeDash(${data})" ><i
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