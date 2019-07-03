// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$('#upload-image-button').click(function () {
    $('#upload-image').click();
});

var input_file = document.getElementById('upload-image');

var choose_cartoon_images_input = document.getElementById('choose_cartoon_images_input');

if (choose_cartoon_images_input) {

choose_cartoon_images_input.onchange = function () {
    $('.selected-images-preview').children().remove();
    if (choose_cartoon_images_input.files && choose_cartoon_images_input.files[0]) {
        for (var i = 0; i < choose_cartoon_images_input.files.length; i++) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('.selected-images-preview').append('<div class="col-md-2">'
                    + '<img src="' + e.target.result + '" class="img-fluid" alt="" />'
                    + '</div>');
            }

            reader.readAsDataURL(choose_cartoon_images_input.files[i]);
        }
    }
    }
}
var add_image = $('#add-image');

if (input_file) {
    input_file.onclick = function () {
        this.value = null;
    };
    input_file.onchange = function () {
        var _data = new FormData();
        _data.append("file", document.getElementById('upload-image').files[0]);
        $.ajax({
            type: "POST",
            url: "/Home/AddFile",
            data: _data,
            processData: false,
            contentType: false,
            success: function (data) {
                if (data.Status) {
                    $("#image-list > div:nth-child(" + (1) + ")").after('<div class="col-md-3 p-1 mb-1">' +
                        '<div class="border border-primary text-center h-100 d-flex justify-content-center align-items-center">' +
                        '<img src="' + data.Link + '" class="img-fluid" />' +
                        '</div>' +
                        '</div>')
                }
            },
            error: function (err) {
                console.log(err)
            }
        })
    };
}




var file_name_selector = "";
$('#choose-image').click(function () {
    file_name_selector = $(this).data('input');
    $('#modal-image-select').modal();
})

$(document).on('click', '#image-list > div img', function () {
    $(file_name_selector).val($(this).attr('src'));
})