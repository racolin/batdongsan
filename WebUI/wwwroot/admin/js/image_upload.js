function readURL(input, id) {
    console.log(input.files && input.files[0])
    if (input.files && input.files[0]) {

        var reader = new FileReader();

        reader.onload = function (e) {
            $('#' + id + ' .image-upload-wrap').addClass("d-none");

            $('#' + id + ' .file-upload-image').attr('src', e.target.result);
            $('#' + id + ' .file-upload-content').removeClass("d-none");
            $('#' + id + '-ext').text(input.files[0].name.split('.').pop());
        };

        reader.readAsDataURL(input.files[0]);

    } else {
        removeUpload();
    }
}

function removeUpload(id) {
    $('#' + id + ' .file-upload-input').replaceWith($('<input class="file-upload-input" type="file" onchange="readURL(this, \'' + id + '\');loadedImage(\'has-' + id + '\')" accept="image/*">'));
    $('#' + id + ' .file-upload-content').addClass("d-none");
    $('#' + id + ' .image-upload-wrap').removeClass("d-none");
    $('#' + id + ' .btn-copy').remove();
}
$('.image-upload-wrap').on('dragover', function () {
    $(this).addClass('image-dropping');
});
$('.image-upload-wrap').on('dragleave', function () {
    $(this).removeClass('image-dropping');
});
 