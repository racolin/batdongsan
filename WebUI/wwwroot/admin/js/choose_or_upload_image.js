
function clearDisplayImage(key) {
    $("#img-display-wrap-" + key).addClass("d-none");
    $("#control-btn-" + key).removeClass("d-none");
    $("#img-display-" + key).attr('src', '');
}
function setDisplayImage(key, src) {
    $("#img-display-" + key).attr('src', src);
    $("#img-display-wrap-" + key).removeClass("d-none");
    $("#control-btn-" + key).addClass("d-none");
}
function upload(key) {
    var title = $("#title-" + key).val();
    var alt = $("#alt-" + key).val();
    var name = $("#name-" + key).val();

    var list = [
        { 'value': name, 'name': 'tên file' },
        { 'value': alt, 'name': 'text thay thế' },
        { 'value': title, 'name': 'tiêu đề' },
    ];

    var formData = new FormData();
    var large = $("#image-" + key + " .file-upload-input").prop("files");
    if (large.length != 0) {
        formData.append("large", large[0]);
    } else {
        $("#message-" + key).text('Hãy tải ảnh lên trước khi lưu!');
        return;
    }
    console.log(list);
    var verifies = verify(list);

    if (verifies.length == 0) {
        formData.append("type", 1);
        formData.append("title", title);
        formData.append("alt", alt);
        formData.append("name", name);

        // Save
        postFormDataApiExplicit(
            '/api/admin/image/save', formData,
            // Success: function (data) {}
            function (data) {
                if (data != null) {
                    selectedImageId(key, data);
                    setDisplayImage(key, $('#img-' + key).attr('src'));

                    $("#message-" + key).text("");
                    $("#title-" + key).val("");
                    $("#alt-" + key).val("");
                    $("#name-" + key).val("");
                    removeUpload('image-' + key);
                    $('#upload-image-' + key).modal('hide');

                } else {
                    $("#message-" + key).text('Có lỗi phát sinh, không thể lưu ảnh!');
                }
            },
            // Warning: function(textMessage, data) {}
            // Error: function(message) {}
            // Error Api function() {}
        );

    } else {
        $("#message-" + key).text('Vui lòng nhập ' + verifies.join(', ') + '!');
    }
}

function loadedImage(id) {

}
