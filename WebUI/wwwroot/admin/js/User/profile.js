var oldItem = {};
$(document).ready(function () {
    oldItem = getItemsByQueries(['#name', '#dateOfBirth', 'input[name="gender"]:checked']);
});
$("#btn-save").click(function () {

    // kiểm tra sự giống nhau giữa oldItem và newItem
    var newItem = getItemsByQueries(['#name', '#dateOfBirth', 'input[name="gender"]:checked']);
    var same = filterDifferences(oldItem, newItem);
    if (same) {
        showMessage('warning', 'Không phát hiện có sự thay đổi dữ liệu nào!');
        return;
    }

    var list = [
        { 'value': newItem['#name'], 'name': 'tên' },
        { 'value': newItem['input[name="gender"]:checked'], 'name': 'giới tính' },
    ];

    var verifies = verify(list);

    if (verifies.length == 0) {
        postApiExplicit(
            '/api/admin/user/update-profile',
            $("#form-update-profile").serialize(),
            // Success: function (data) {}
            function (data) {
                if (data != null) {
                    window.location.reload();
                } else {
                    showMessage('error', 'Có lỗi phát sinh, không thể cập nhật thông tin cá nhân!');
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