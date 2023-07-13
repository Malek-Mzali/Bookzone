$(document).ready(function () {

    $("#newuserform").submit(function(e) {
        e.preventDefault();

        var form = $(this).serialize();
        var type = $("input:radio[name ='UsersGroup.Role']:checked").val();


        if($("#newuserform").valid()){
            $.ajax({
                type: "POST",
                url: "/Dashboard/NewUser/",
                data: form,
                success: function(data)
                {
                    if (data.data.success) {
                        var files = $('#NewPhotoUpload').prop("files");
                       if (files.length > 0){
                           let formData = new FormData();
                           formData.append("myUploader", files[0]);
                           formData.append("id", data.data.extra);
                           formData.append("type", "profile");

                           $.ajax({
                               type: 'POST',
                               url: "/Dashboard/OnPostMyUploader",
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
                                       setTimeout(function() {
                                           window.location.href='/Dashboard/Users/'+type
                                       }, 3000);
                                   }
                               }
                           }
                           );
                       }else{
                           setTimeout(function() {
                               window.location.href='/Dashboard/Users/'+type
                           }, 3000);                       }
                    }
                }
            });
        }
    });

});

$(function () {
    $('input[name="UsersGroup.Role"]').click(function () {
        var type = $("input:radio[name ='UsersGroup.Role']:checked").val();
        if (type === "Administrator"){
            $('#AdministratorPf').show();
            $('#IndividualPf').hide();
            $('#EditorPf').hide();
            $('#OrganizationPf').hide();

        }
        if (type === "Individual"){
            $('#AdministratorPf').hide();
            $('#IndividualPf').show();
            $('#EditorPf').hide();
            $('#OrganizationPf').hide();
        }
        if (type === "Editor"){
            $('#AdministratorPf').hide();
            $('#IndividualPf').hide();
            $('#EditorPf').show();
            $('#OrganizationPf').hide();
        }
        if (type === "Organization"){
            $('#AdministratorPf').hide();
            $('#IndividualPf').hide();
            $('#EditorPf').hide();
            $('#OrganizationPf').show();
        }
    });
});

