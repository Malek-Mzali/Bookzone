$(document).ready(function () {

    $.ajax({
        type: "GET",
        url: "/Dashboard/GetAllTheme/",
        success: function (data) {
            $.each(data.data, function (key, value) {

                $('#ThemeListBook').append(`
                         <div class="p-md-5  mx-md-4 mt-md-3 p-3 flex-item">
                            <div class="bloc-icon" data-toggle="tooltip" data-placement="top" title="${value.description}">
                                <div class="img-icon">
                                    <i class="${value.icon}"></i>
                                </div>
                                                                        
                                <div class="text-icon">
                                    <a href="/home/Theme?id=${value.id}&type=Ebook" >
                                        ${value.title}
                                    </a>
                                </div>
                            </div>
                        </div>
                        
                    `);

                $('#ThemeListJournal').append(`
                         <div class="p-md-5  mx-md-4 mt-md-3 p-3 flex-item">
                            <div class="bloc-icon" data-toggle="tooltip" data-placement="top" title="${value.description}">
                                <div class="img-icon">
                                    <i class="${value.icon}"></i>
                                </div>
                                                                        
                                <div class="text-icon">
                                    <a href="/home/Theme?id=${value.id}&type=Ejournal" >
                                        ${value.title}
                                    </a>
                                </div>
                            </div>
                        </div>
                        
                    `);

            });
        }
    });

    $.ajax({
        type: "GET",
        url: "/Dashboard/GetLastDocuments/",
        success: function (data) {
            $('#LastDocuments').html('<div class="owl-carousel mb-5" ></div>\n');
            $.each(data.data, function (key, value) {
                $('.owl-carousel').append(`
                    <div class="item">
                        <div class="gallery">
                                <div class="hoverfx">
                                    <div class="figure">
                                      <a target="_blank"  href="${value.documentGroup.url}" data-id="${value.documentGroup.id}"><i class="icon fa fa-eye" ></i></a>
                                    </div>
                                    
                                    <div class="figure">
                                      <a class="cursor-p" onclick="AddToWishList(this)" data-id="${value.documentGroup.id}"><i class="icon fa fa-heart"></i></a>
                                    </div>
                                    <div class="figure">
                                      <a class="cursor-p AddToCart" onclick="AddToCart(this)" data-id="${value.documentGroup.id}"><i class="icon fa fa-shopping-cart" ></i></a>
                                    </div>
                                    
                                    <div class="price">${value.documentGroup.price} DT</div>
                                    <div class="overlay">
                                    </div>
                                    <a target="_blank"  href="${value.documentGroup.url}">
                                    <img src="/img/document/${value.documentGroup.coverPage}" alt="${value.documentGroup.originalTitle}" width="620" height="450">
                                    </a>
    
                                </div>

                            

                        <div class="desc"><a   target="_blank"  href="${value.documentGroup.url}">${value.documentGroup.originalTitle}</a></div>
                        </div>
                        
                    </div>
                    
                    `);
            });
            $(".owl-carousel").owlCarousel({
                loop: true,
                autoplay: true,
                autoplayTimeout: 2000,
                autoplayHoverPause: true,
                nav: true,
                navText: ['<i class="fa fa-angle-left" aria-hidden="true"></i>', '<i class="fa fa-angle-right" aria-hidden="true"></i>'],
                margin: 100,
                responsive: {
                    0: {
                        items: 1,
                    },
                    600: {
                        items: 3
                    },
                    1000: {
                        items: 5
                    }
                }
            });

        }
    });


});