var oldItem = getItemsByQueries(['#name', '#link', '#title', '#alt', '#type']);

function loadedImage(id) {
    $('#' + id).val('true');
}

function removedImage(id) {
    $('#' + id).val('false');
}

$('#type').on('change', function () {
    var value = $(this).val();
    if (value == '1') {
        $('#small-image').addClass('d-none');
    } else if (value == '2') {
        $('#small-image').removeClass('d-none');
    }
});

$("#btn-save").on("click", function () {
    var type = $("#type").val();
    if (type == '2') {
        var largeExt = $("#large-image-ext").text();
        var smallExt = $("#small-image-ext").text();
        if (largeExt != smallExt) {
            showMessage('warning', 'Định dạnh ảnh lớn và ảnh thu nhỏ phải giống nhau!');
            return;
        }
    }

    var id = $("#id").val();

    var newItem = getItemsByQueries(['#name', '#link', '#title', '#alt', '#type']);

    var hasLargeImage = $("#has-large-image").val();
    var hasSmallImage = $("#has-small-image").val();

    // Kiểm tra lỗi chưa điền dữ liệu
    var list = [
        { 'value': newItem['#type'], 'name': 'loại ảnh' },
        { 'value': newItem['#name'], 'name': 'tên file' },
        { 'value': newItem['#alt'], 'name': 'text thay thế' },
        { 'value': newItem['#title'], 'name': 'tiêu đề' },
        { 'value': hasLargeImage, 'name': 'ảnh lớn' },
    ];
    if (type == '2') {
        list.push({ 'value': hasSmallImage, 'name': 'ảnh thu nhỏ' });
    }

    var verifies = verify(list);
    if (verifies.length != 0) {
        showMessage('warning', 'Vui lòng nhập ' + verifies.join(', ') + '!');
        return;
    } 

    var formData = new FormData();
    var large = $("#large-image .file-upload-input").prop("files");
    if (large.length != 0) {
        formData.append("large", large[0]);
    }
    var small = $("#small-image .file-upload-input").prop("files");
    if (small.length != 0) {
        formData.append("small", small[0]);
    }

    // kiểm tra sự giống nhau giữa oldItem và newItem
    var same = filterDifferences(oldItem, newItem);
    if (same && large.length == 0 && small.length == 0) {
        showMessage('warning', 'Không phát hiện có sự thay đổi dữ liệu nào!');
        return;
    } 

    formData.append("id", id);
    formData.append("type", newItem['#type']);
    formData.append("link", newItem['#link']);
    formData.append("title", newItem['#title']);
    formData.append("alt", newItem['#alt']);
    formData.append("name", newItem['#name']);

    // Save
    postFormDataApiExplicit(
        '/api/admin/image/save', formData,
        // Success: function (data) {}
        function (data) {
            if (data != null) {
                window.location.replace('/admin/image/item?id=' + data);
            } else {
                showMessage('error', 'Có lỗi phát sinh, không thể lưu ảnh!');
            }
        },
        // Warning: function(textMessage, data) {}
        // Error: function(message) {}
        // Error Api function() {}
    );
});

$("#btn-reference").on("click", function () {
    var id = $("#id").val();
    // Get references
    getApiExplicit(
        '/api/admin/image/get-references?id=' + id,
        // Success: function (data) {}
        function (data) {
            if (data != null) {
                var refsImage = $("#references-image");
                refsImage.empty()
                $("#references-title").removeClass("d-none");
                if (data.length == 0) {
                    refsImage.append("<p>Ảnh này hiện không được sử dụng.</p>")
                }
                for (var i = 0; i < data.length; i++) {
                    var refsData = data[i].references; 
                    for (var j = 0; j < refsData.length; j++) {
                        refsImage.append($(`<a class='btn reference' title='Chuyển đến trang ${data[i].udName}: ${refsData[j].name}' href='/admin/${data[i].ud}/item?id=${refsData[j].id}' target='_blank'>${data[i].udName}: ${refsData[j].name}</a>`))
                    }
                }
            } else {
                showMessage('error', 'Có lỗi phát sinh, không thể lấy danh sách tham chiếu!');
            }
        },
        // Warning: function(textMessage, data) {}
        // Error: function(message) {}
        // Error Api function() {}
    );
});

$("#btn-remove").on("click", function () {
    // Remove
    getApiExplicit(
        '/api/admin/image/delete?id=' + $("#id").val(),
        // Success: function (data) {}
        function (data) {
            if (data != null) {
                window.location.replace('/admin/image');
            } else {
                showMessage('error', 'Có lỗi phát sinh, chưa xóa được ảnh!');
            }
        },
        // Warning: function(textMessage, data) {}
        // Error: function(message) {}
        // Error Api function() {}
    );
});