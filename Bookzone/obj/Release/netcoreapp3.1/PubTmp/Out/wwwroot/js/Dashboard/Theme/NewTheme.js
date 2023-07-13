$(document).ready(function () {
    $("#txtNewIcon").on("change keyup paste", function(){
        $("#txtIconDisplay").attr("class", $("#txtNewIcon").val());

    })
    
    $("#newThemeForm").submit(function(e) {
        e.preventDefault();

        var form = $(this).serialize();
        

        if($("#newThemeForm").valid()){
            $.ajax({
                type: "POST",
                url: "/Dashboard/NewTheme/",
                data: form,
                success: function(data)
                {
                    if (data.data.success) {
                        setTimeout(function() {window.location.href='/Dashboard/Theme/'}, 3000);
                    }
                }
            });
        }
    });

});



