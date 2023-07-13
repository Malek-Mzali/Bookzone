let dataresult;

$(document).ready(function () {

    loadDataTable();
    
    $("#editBookform").submit(function(e) {

        e.preventDefault();

        var form = $(this).serialize();
        $("#txtNbPages").rules("remove", "required");
        $("#txtDateFirstIssue").rules("remove", "required");
        $("#txtTotalIssuesNb").rules("remove", "required");
        $("#txtSellingPrice").rules("remove", "required");
        $("#txtDigitalPrice").rules("remove", "required");
        $("#txtAccompanyingMaterialsNb").rules("remove", "required");
        $("#txtVolumeNb").rules("remove", "required");
        
        if($("#editBookform").valid()){
            if ($("#txtTitle").val().length > 0 && $("#txtPrice").val().length > 0 &&  $("#txtPublicationDate").val().length > 0 &&  $("#txtLanguage").val().length > 0 &&  $("#txtDocumentType").val().length > 0 &&  $("#IdEditor").val().length > 0  &&  $("#IdCollection").val().length > 0 &&  $("#txtKeywords").val().length > 0 &&  ( $("#txtIsbn").val().length > 0 ||   $("#txtIssn").val().length > 0)) {
                $.ajax({
                    type: "POST",
                    url: "/Dashboard/EditDocument/",
                    data: form,
                    success: function (data) {
                        if (data.data.success) {
                            loadDataTable();
                            $('#editDocument').modal('hide');
                        }
                    }
                });
            }
        }
    });

    $("#txtFile").click(function (){
        $('#IdFile').trigger('click');
    })
    $("#SummaryDocumentForm").submit(function(e) {

        e.preventDefault();
        var form = $(this).serialize();


            
        $.ajax({
            type: "POST",
            url: "/Dashboard/EditDocumentSummary/",
            data: form,
            success: function (data) {
                if (data.data.success) {
                    loadDataTable();
                    $('#summaryDocument').modal('hide');
                }
            }
        });
    });



});

function loadDataTable() {

    if ($.fn.DataTable.isDataTable('#BooksList')) {
        dataTable.fnDestroy();
    }
    let typeBook = $("#typeBook").val();
    let name;
    if (typeBook=== "Ejournal"){
        name = typeBook.toLowerCase()+"Group.issn";
    }else{
        name = typeBook.toLowerCase()+"Group.isbn"
    }
    var extra = ""
    if ($("#txtRoleUser").val() === "Editor"){
        extra =   $("#txtUserId").val()
    }

    dataTable = $('#BooksList').dataTable({
        "bAutoWidth": false, // Disable the auto width calculation 

        "ajax": {
            "url": "/Dashboard/GetAllDocuments/",
            "data": { 'type': typeBook, 'extra' : extra },
            "type": "GET",
            "datatype": "json"
        },
        "drawCallback": function (settings) {
            dataresult = settings.json;
        },
        
        "columns": [
            {"data": "documentGroup.id"},
            {
                "data": "documentGroup.coverPage",
                "render": function (data) {
                    return '<img src="/img/document/' + data + '" alt="' + data + '" class="avatar avatar-md mx-3"/>';
                }
            },
            {"data": "documentGroup.originalTitle" },
            {"data": name },
            {"data": "documentGroup.price", },
            {"data": "documentGroup.publicationDate", },
            {
                "data": "documentGroup.id",

                "render": function (data) {


                    return `
                     <div class="text-center">
                        <a class="nav-link dropdown-toggle larger-zone action" data-bs-toggle="dropdown" href="#"
                           id="dropdownMenuButton3"><i class="fa fa-ellipsis-v"></i></a>
                        <ul aria-labelledby="dropdownMenuButton3" class="dropdown-menu dropdown-menu-right" id="dropdownList2">
                            <li>
                                <a class="dropdown-item" href="/Home/Document?id=${data}"><i  class="fas fa-eye m-r-5"></i> View</a>
                            </li>
                            <li>
                                <a class="dropdown-item" href="#" data-bs-toggle="modal" onclick="EditBtnDocument(this)" data-bs-target="#editDocument"><i class="fas fa-edit m-r-5"></i> Edit</a>
                            </li>
                            <li>
                                <a class="dropdown-item" href="#" data-bs-toggle="modal" onclick="SummaryBtnDocument(this)" data-bs-target="#summaryDocument"><i class="fas fa-bookmark m-r-5"></i> Summary</a>
                            </li>
                            <li>
                                <a class="dropdown-item"  href="#" onclick="DeleteDocumentDash(${data})" ><i
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

function EditBtnDocument(obj){
    
    
    var x = $(obj).closest('tr').index()
    $("#txtId").val(dataresult.data[x].documentGroup.id);
    $("#txtTitle").val(dataresult.data[x].documentGroup.originalTitle);

    $.ajax({
        type: 'GET',
        url: '/Dashboard/GetCollectionBy/',
        data: '&Id='+ dataresult.data[x].documentGroup.idCollection
    }).then(function (data) {
        $("#IdCollection").append(new Option(data.data.title, data.data.id, true, true)).trigger('change');
    });

    $.ajax({
        type: 'GET',
        url: '/Dashboard/GetUserEditorBy/',
        data: '&Id='+ dataresult.data[x].documentGroup.idEditor
    }).then(function (data) {
        
        $("#IdEditor").append(new Option(data.data.editorGroup.name, data.data.editorGroup.id, true, true)).trigger('change');
    });

    $.ajax({
        type: 'GET',
        url: '/Dashboard/GetAuthorBy/',
        data: '&Id='+ dataresult.data[x].documentGroup.idAuthor
    }).then(function (data) {
        if (data.data.id === 0){
            $("#IdAuthor").append(new Option("No author found", 0, true, true)).trigger('change');
        }else{
            $("#IdAuthor").append(new Option(data.data.name, data.data.id, true, true)).trigger('change');
        }
    });
    
    if (dataresult.data[x].documentGroup.file !== ""){
        $("#IdFile").hide()
        $("#txtFile").show().val(dataresult.data[x].documentGroup.file)
    }else{
        $("#IdFile").show()
        $("#txtFile").hide()
    }
    
    $("#txtLanguage").val(dataresult.data[x].documentGroup.originalLanguage);
    $('#txtPrice').val(dataresult.data[x].documentGroup.price);
    $("#txtPublicationDate").val(dataresult.data[x].documentGroup.publicationDate);
    $("#txtKeywords").val(dataresult.data[x].documentGroup.keyword.join('\n').trim());
    $("#txtDocumentType").val(dataresult.data[x].documentGroup.documentType);
    $("#txtDoi").val(dataresult.data[x].documentGroup.doi);
    $("#txtMarcRecordNumber").val(dataresult.data[x].documentGroup.marcRecordNumber);
    $("#txtTitlesVariants").val(dataresult.data[x].documentGroup.titlesVariants);
    $("#txtSubtitle").val(dataresult.data[x].documentGroup.subtitle);
    $("#txtTranslator").val(dataresult.data[x].documentGroup.translator);
    $("#txtAccessType").val(dataresult.data[x].documentGroup.accessType);
    $("#txtState").val(dataresult.data[x].documentGroup.state);
    $("#txtSellingPrice").val(dataresult.data[x].documentGroup.sellingPrice);
    $("#txtDigitalPrice").val(dataresult.data[x].documentGroup.digitalPrice);
    $("#txtPhysicalDescription").val(dataresult.data[x].documentGroup.physicalDescription);
    $("#txtAccompanyingMaterials").val(dataresult.data[x].documentGroup.accompanyingMaterials);
    $("#txtAccompanyingMaterialsNb").val(dataresult.data[x].documentGroup.accompanyingMaterialsNb);
    $("#txtVolumeNb").val(dataresult.data[x].documentGroup.volumeNb);
    $("#txtForeword").val(dataresult.data[x].documentGroup.foreword);
    $("#txtAbstract").val(dataresult.data[x].documentGroup.abstract);
    $("#txtNotes").val(dataresult.data[x].documentGroup.notes);
    $("#txtPhoto").attr("src","/img/document/"+dataresult.data[x].documentGroup.coverPage);
    
    $("#"+dataresult.data[x].documentGroup.documentType).show();
    



    if(dataresult.data[x].documentGroup.documentType === "Ebook"){
        $('#txtIsbn').val(dataresult.data[x].ebookGroup.isbn);
        $('#txtGenre').val(dataresult.data[x].ebookGroup.genre);
        $('#txtEditionNum').val(dataresult.data[x].ebookGroup.editionNum);
        $('#txtEditionPlace').val(dataresult.data[x].ebookGroup.editionPlace);
        $('#txtNbPages').val(dataresult.data[x].ebookGroup.nbPages);
        $('#txtCategory').val(dataresult.data[x].ebookGroup.category);

        
    }else{
        $('#txtIssn').val(dataresult.data[x].ejournalGroup.issn);
        $("#txtFrequency").val(dataresult.data[x].ejournalGroup.frenquency);
        $('#txtTotalIssuesNb').val(dataresult.data[x].ejournalGroup.totalIssuesNb);
        $("#txtDateFirstIssue").val(dataresult.data[x].ejournalGroup.dateFirstIssue);
        $('#txtJournalScope').val(dataresult.data[x].ejournalGroup.journalScope);
        $("#txtImpactFactor").val(dataresult.data[x].ejournalGroup.impactFactor);
    }



}
function DeleteDocumentDash(id ) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger me-3'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "Are you sure that you want to delete this Document?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "/Dashboard/DeleteDocument/",
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
function addSummary(obj, id){
    $(
        `
                    <div class="row" id="${obj+100}">
                        <input type="hidden" name="Id" value="0">
                        <input id="txtIdSummary" type="hidden" name="IdDocument" value="${id}">
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
                            <a onclick="addSummary(${obj+100}, ${id})" href="#"> <i class="fa fa-plus" aria-hidden="true"></i></a>
                        </div>
                        <div class="col-6 col-sm-1 mt-3 mt-sm-5 text-center">
                            <a onclick="deleteSummary(${obj+100}, ${id})" href="#"> <i class="fa fa-minus" aria-hidden="true"></i></a>
                        </div>
                        <div class="col-12 mt-2">
                            <hr/>
                        </div>
                    </div>
                    
                            
                    `
    ).insertAfter( $( "#"+obj ) );

}
function deleteSummary(obj){
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger me-3'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "Are you sure that you want to delete this summary?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "/Dashboard/DeleteDocumentSummary/",
                type: "GET",
                data:
                    {
                        "id": obj
                    },
            })
                
            $("#" + obj).remove();
        }
    })
                
   
}
function SummaryBtnDocument(obj) {    
    $('#DocumentSummary').empty()


    var x = $(obj).closest('tr').index()
    var id = dataresult.data[x].documentGroup.id;


    $.ajax({
        type: 'GET',
        url: '/Dashboard/GetAllDocumentSummary/',
        data: '&Id=' + dataresult.data[x].documentGroup.id,
        success: function (data) {
            if (data.data.length === 0){
                $('#DocumentSummary').append(`
                    <div class="row" id="${x}">
                    
                        <input type="hidden" name="Id" value="0">
                        <input id="txtIdSummary" type="hidden" name="IdDocument" value="${id}">
                        <div class="col-12 col-sm-6">
                            <label>Title</label>&nbsp;<span style="color:red;">*</span>&nbsp;<span class="text-danger" data-valmsg-replace="true"></span>
                            <input name="Title" class=" form-control" data-val="true" data-val-required="Required field" required>
                        </div>
                        <div class="col-6 col-sm-2 mt-3 mt-sm-0">
                            <label>Start</label>&nbsp;<span style="color:red;">*</span>&nbsp;<span class="text-danger" data-valmsg-replace="true"></span>
                            <input name="Start" type="number" class=" form-control" data-val="true" data-val-required="Required field" min="0" step="1" required >
                        </div>
                        <div class="col-6 col-sm-2 mt-3 mt-sm-0">
                            <label>End</label>&nbsp;<span style="color:red;">*</span>&nbsp;<span class="text-danger" data-valmsg-replace="true"></span>
                            <input name="End" type="number" class=" form-control" data-val="true" data-val-required="Required field"  min="0" step="1" required >
                        </div>
                        <div class="col-6 col-sm-1 mt-3 mt-sm-5 text-center">
                            <a  onclick="addSummary(${x}, ${id})" href="#"> <i class="fa fa-plus" aria-hidden="true"></i></a>
                        </div>
                        <div class="col-6 col-sm-1 mt-3 mt-sm-5 text-center">
                            <a onclick="deleteSummary(${x}, ${id})" href="#"> <i class="fa fa-minus" aria-hidden="true"></i></a>
                        </div>
                        <div class="col-12 mt-2">
                            <hr/>
                        </div>
                    </div>
                    
                            
                    `);
            }else {
                $.each(data.data, function (key, value) {
                    $('#DocumentSummary').append(`
                    <div class="row" id="${value.id}">
                        <input name="Id" type="hidden" value="${value.id}">
                        <input id="txtIdSummary" type="hidden" name="IdDocument" value="${id}">
                        <div class="col-12 col-sm-6">
                            <label>Title</label>&nbsp;<span style="color:red;">*</span>&nbsp;<span class="text-danger" data-valmsg-replace="true"></span>
                            <input name="Title" class=" form-control" data-val="true" data-val-required="Required field" value="${value.title}" required>
                        </div>
                        <div class="col-6 col-sm-2 mt-3 mt-sm-0">
                            <label>Start</label>&nbsp;<span style="color:red;">*</span>&nbsp;<span class="text-danger" data-valmsg-replace="true"></span>
                            <input name="Start" type="number" class=" form-control" data-val="true" data-val-required="Required field" min="0" step="1"  value="${value.start}" required>
                        </div>
                        <div class="col-6 col-sm-2 mt-3 mt-sm-0">
                            <label>End</label>&nbsp;<span style="color:red;">*</span>&nbsp;<span class="text-danger" data-valmsg-replace="true"></span>
                            <input name="End" type="number" class=" form-control" data-val="true" data-val-required="Required field" min="0" step="1"  value="${value.end}" required>
                        </div>
                        <div class="col-6 col-sm-1 mt-3 mt-sm-5 text-center">
                            <a  onclick="addSummary(${value.id}, ${id})" href="#"> <i class="fa fa-plus" aria-hidden="true"></i></a>
    
                        </div>
                        <div class="col-6 col-sm-1 mt-3 mt-sm-5 text-center">
                            <a  onclick="deleteSummary(${value.id}, ${id})" href="#"> <i class="fa fa-minus" aria-hidden="true"></i></a>
                        </div>
                        <div class="col-12 mt-2">
                            <hr/>
                        </div>
                    </div>
                    
                            
                    `);
                });
            }
        }
    })


}