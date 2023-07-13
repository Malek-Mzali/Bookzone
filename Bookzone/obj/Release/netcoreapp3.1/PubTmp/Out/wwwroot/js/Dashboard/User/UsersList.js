var dataTable;

let dataresult;

$(document).ready(function () {
    
    loadDataTable();
  
    $('#txtRole').on('change', function() {
        var array = ["Administrator", "Editor", "Individual", "Organization"]
        var element = $("#"+$('#txtRole').val()).show();
        array.forEach(function(item) {
            if ($('#txtRole').val() !== item){
                $("#"+item).hide();
            }
                
        });
    })

    $("#edituserform").submit(function(e) {

        e.preventDefault();

        var form = $(this).serialize();

        if($("#edituserform").valid()){
            $.ajax({
                type: "POST",
                url: "/Dashboard/EditUser/",
                data: form,
                success: function(data)
                {
                    if (data.data.success){
                        loadDataTable();
                        $('#editUser').modal('hide');
                    }
                }
            });
        }
    });
    
    
});





function deleteUserDash(id ) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger me-3'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "Are you sure that you want to delete this User?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "/Dashboard/DeleteUser/",
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






function EditBtnUser(obj){
    var x = $(obj).closest('tr').index()

    $("#txtId").val(dataresult.data[x].usersGroup.id);
    $("#txtEmail").val(dataresult.data[x].usersGroup.email);
    $("#txtPassword").val(dataresult.data[x].usersGroup.password);
    $("#txtPhone").val(dataresult.data[x].usersGroup.phone);
    $('#txtCountry').val(dataresult.data[x].usersGroup.country);    
    $("#txtAddress").val(dataresult.data[x].usersGroup.address);
    $("#txtPostalCode").val(dataresult.data[x].usersGroup.postalCode);
    $("#txtPhoto").attr("src","/img/profile/"+dataresult.data[x].usersGroup.photo);
    $("#txtRole").val(dataresult.data[x].usersGroup.role);
    $("#"+dataresult.data[x].usersGroup.role).show();

    
    if(dataresult.data[x].usersGroup.role === "Individual"){
        $('#txtGender').val(dataresult.data[x].individualGroup.gender);
        $("#txtFirstName").val(dataresult.data[x].individualGroup.firstname);
        $("#txtLastName").val(dataresult.data[x].individualGroup.lastname);
        $("#txtDateofBirth").val(dataresult.data[x].individualGroup.dateofBirth);

        $("#txtProfession").val(dataresult.data[x].individualGroup.profession);
        $("#txtOrganization").val(dataresult.data[x].individualGroup.organization);

    }else if(dataresult.data[x].usersGroup.role === "Editor"){
        $('#txtEditorName').val(dataresult.data[x].editorGroup.name);
        $("#txtCity").val(dataresult.data[x].editorGroup.city);
        $("#txtEditorWebsite").val(dataresult.data[x].editorGroup.website);
        $("#txtEditorAbout").val(dataresult.data[x].editorGroup.about);
        $("#txtMultiplyer").val(dataresult.data[x].editorGroup.multiplyer);

    }else if(dataresult.data[x].usersGroup.role === "Organization"){
        $('#txtOrgName').val(dataresult.data[x].organizationGroup.name);
        $("#txtShortName").val(dataresult.data[x].organizationGroup.shortName);
        $("#txOranizationWebsite").val(dataresult.data[x].organizationGroup.website);
        $("#txtOrgaAbout").val(dataresult.data[x].organizationGroup.about);
        $("#txtType").val(dataresult.data[x].organizationGroup.type);
        $("#txtIpAdress").val(dataresult.data[x].organizationGroup.ipAdress.join('\n').trim());
    }else if(dataresult.data[x].usersGroup.role === "Administrator"){
        $('#txtWebsiteAdmin').val(dataresult.data[x].administratorGroup.website);
        $("#txtAboutAdmin").val(dataresult.data[x].administratorGroup.about);

    }

    

}

function loadDataTable() {
    
    if ($.fn.DataTable.isDataTable('#UsersList')) {
        dataTable.fnDestroy();
    }
    let listype = $("#typeoflist").val();
    let name;
    if (listype=== "Individual"){
         name = listype.toLowerCase()+"Group.firstname";
    }
    else if (listype=== "Administrator"){
        name = listype.toLowerCase()+"Group.website";
    }else{
         name = listype.toLowerCase()+"Group.name"
    }
        dataTable = $('#UsersList').dataTable({
        "bAutoWidth": false, // Disable the auto width calculation 

        "ajax": {
            "url": "/Dashboard/getAllUsers/",
            "data": { 'type': listype },
            "type": "GET",
            "datatype": "json"
        },
        "drawCallback": function (settings) {
            // Here the response
            dataresult = settings.json;
        },
        "columns": [
            {"data": "usersGroup.id"},
            {
                "data": "usersGroup.photo",
                "render": function (data) {
                    return '<img src="/img/profile/' + data + '" alt="' + data + '" class="avatar avatar-md mx-3"/>';
                }
            },
            
            {"data": name },
            {"data": "usersGroup.email", },
            {"data": "usersGroup.password", },
            {"data": "usersGroup.date", },

            {
                "data": "usersGroup.id",

                "render": function (data) {

                    
                    return `
                     <div class="text-center">
                        <a class="nav-link dropdown-toggle larger-zone action" data-bs-toggle="dropdown" href="#"
                           id="dropdownMenuButton2"><i class="fa fa-ellipsis-v"></i></a>
                        <ul aria-labelledby="dropdownMenuButton2" class="dropdown-menu dropdown-menu-right" id="dropdownList2">
                            <li>
                                <a class="dropdown-item" href="/Home/Profile?id=${data}"><i  class="fas fa-eye m-r-5"></i> Profile</a>
                            </li>
                            <li>
                                <a class="dropdown-item" href="#" data-bs-toggle="modal" onclick="EditBtnUser(this)" data-bs-target="#editUser"><i class="fas fa-edit m-r-5"></i> Edit</a>
                            </li>
                            <li>
                                <a class="dropdown-item"  href="#" onclick="deleteUserDash(${data})" ><i
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