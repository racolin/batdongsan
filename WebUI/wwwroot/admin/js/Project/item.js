
var oldContent = "";
var oldItem = {};

$(document).ready(function () {
    oldContent = $("#content").summernote("code");
    oldItem = getItemsByQueries(['#ud', '#name', 'input[name="type"]:checked', 'input[name="state"]:checked', 'input[name="isHighlight"]:checked', 'input[name="status"]:checked', '#order', '#address', '#imageId']);
});
$("#btn-save").on("click", function () {
    var id = $("#id").val();
    var ud = $("#ud").val();
    var name = $("#name").val();
    var type = $('input[name="type"]:checked').val();
    var state = $('input[name="state"]:checked').val();
    var isHighlight = $('input[name="isHighlight"]:checked').val();
    var status = $('input[name="status"]:checked').val();
    var content = $("#content").summernote("code");
    var order = $("#order").val();
    var address = $("#address").val();
    var imageId = $("#imageId").val();
    var isUpdateContent = true;

    if (oldContent == content) {
        isUpdateContent = false;
    }

    // kiểm tra sự giống nhau giữa oldItem và newItem
    var newItem = getItemsByQueries(['#ud', '#name', 'input[name="type"]:checked', 'input[name="state"]:checked', 'input[name="isHighlight"]:checked', 'input[name="status"]:checked', '#order', '#address', '#imageId']);
    var same = filterDifferences(oldItem, newItem);
    if (!isUpdateContent && same) {
        showMessage('warning', 'Không phát hiện có sự thay đổi dữ liệu nào!');
        return;
    }

    var list = [
        { 'value': ud, 'name': 'liên kết' },
        { 'value': name, 'name': 'tên' },
        { 'value': type, 'name': 'loại' },
        { 'value': state, 'name': 'tình trạng' },
        { 'value': content, 'name': 'nội dung' },
        { 'value': address, 'name': 'địa chỉ' },
        { 'value': status, 'name': 'trạng thái' },
        { 'value': isHighlight, 'name': 'nổi bật', 'allow': ['false'] },
        { 'value': imageId, 'name': 'ảnh' },
    ];

    var verifies = verify(list);

    if (verifies.length == 0) {
        // Save
        postJsonApiExplicit(
            '/api/admin/project/save',
            JSON.stringify({
                id: !id ? null : id,
                ud: ud,
                name: name,
                type: type,
                address: address,
                content: isUpdateContent ? content : "",
                status: status,
                state: state,
                order: !order ? null : order,
                isHighlight: isHighlight == "true" ? true : false,
                imageId: imageId,
                isUpdateContent: isUpdateContent,
            }),
            // Success: function (data) {}
            function (data) {
                if (data != null) {
                    window.location.replace('/admin/project/item?id=' + data);
                } else {
                    showMessage('error', 'Có lỗi phát sinh, không thể lưu dự án này!');
                }
            },
            // Warning: function(textMessage, data) {}
            // Error: function(message) {}
            // Error Api function() {}
        );

    } else {
        showMessage('warning', 'Vui lòng nhập ' + verifies.join(', ') + '!');
    }
});