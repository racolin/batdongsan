var oldItem = {};

$(document).ready(function () {
    oldItem = getItemsByQueries(['#id', '#username', '#name', '#email', '#phone', '#dateOfBirth', 'input[name="gender"]:checked']);
});

$("#btn-save").click(function () {

    // kiểm tra sự giống nhau giữa oldItem và newItem
    var newItem = getItemsByQueries(['#id', '#username', '#name', '#email', '#phone', '#dateOfBirth', 'input[name="gender"]:checked']);
    var same = filterDifferences(oldItem, newItem);
    if (same) {
        showMessage('warning', 'Không phát hiện có sự thay đổi dữ liệu nào!');
        return;
    }

    var list = [
        { 'value': newItem['#username'], 'name': 'tên đăng nhập' },
        { 'value': newItem['#email'], 'name': 'email' },
        { 'value': newItem['#username'], 'name': 'tên' },
        { 'value': newItem['#phone'], 'name': 'số điện thoại' },
        { 'value': newItem['input[name="gender"]:checked'], 'name': 'giới tính' },
    ];

    var verifies = verify(list);

    if (verifies.length == 0) {
        postApiExplicit(
            '/api/admin/user/save',
            $("#form-save-user").serialize(),
            // Success: function (data) {}
            function (data) {
                if (data != null) {
                    window.location.replace("/admin/user/item?id=" + data);
                } else {
                    showMessage('error', 'Có lỗi phát sinh, không thể lưu người dùng này');
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

$("#btn-lock").click(function () {
    postJsonApiExplicit(
        '/api/admin/user/set-lock',
        { userId: $("#id").val(), isLock: true },
        // Success: function (data) {}
        function (data) {
            if (data != null) {
                window.location.reload()
            } else {
                showMessage('error', 'Có lỗi phát sinh, không thể khóa người dùng này!');
            }
        },
        // Warning: function(textMessage, data) {}
        // Error: function(message) {}
        // Error Api function() {}
    );
});

$("#btn-unlock").click(function () {
    postJsonApiExplicit(
        '/api/admin/user/set-lock',
        { userId: $("#id").val(), isLock: false },
        // Success: function (data) {}
        function (data) {
            if (data != null) {
                window.location.reload()
            } else {
                showMessage('error', 'Có lỗi phát sinh, không thể mở khóa cho người dùng này!');
            }
        },
        // Warning: function(textMessage, data) {}
        // Error: function(message) {}
        // Error Api function() {}
    );
});