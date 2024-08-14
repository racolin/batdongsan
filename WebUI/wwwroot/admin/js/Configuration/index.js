var oldItem = {};

$(document).ready(function () {
    oldItem = getItemsByQueries(['#id', '#sendEmail', '#sendEmailPassword', '#receiveEmail', '#contactPhone',
        '#contactZaloPhone', '#systemPhone', '#systemEmail', '#systemZaloLink', '#systemYoutubeLink', '#systemFacebookLink', 
    ]);
});

$("#btn-save").click(function () {

    // kiểm tra sự giống nhau giữa oldItem và newItem
    var newItem = getItemsByQueries(['#id', '#sendEmail', '#sendEmailPassword', '#receiveEmail', '#contactPhone',
        '#contactZaloPhone', '#systemPhone', '#systemEmail', '#systemZaloLink', '#systemYoutubeLink', '#systemFacebookLink',
    ]);
    var same = filterDifferences(oldItem, newItem);
    if (same) {
        showMessage('warning', 'Không phát hiện có sự thay đổi dữ liệu nào!');
        return;
    }

    var list = [
        { 'value': newItem['#sendEmail'], 'name': 'email gửi tin tức' },
        { 'value': newItem['#sendEmailPassword'], 'name': 'mật khẩu ứng dụng email' },
        { 'value': newItem['#receiveEmail'], 'name': 'email nhận thông báo' },
        { 'value': newItem['#contactPhone'], 'name': 'sđt liên hệ' },
        { 'value': newItem['#contactZaloPhone'], 'name': 'số Zalo liên hệ' },
        { 'value': newItem['#systemPhone'], 'name': 'sđt hệ thống' },
        { 'value': newItem['#systemEmail'], 'name': 'email hệ thống' },
        { 'value': newItem['#systemZaloLink'], 'name': 'zalo link hệ thống' },
        { 'value': newItem['#systemYoutubeLink'], 'name': 'youtube hệ thống' },
        { 'value': newItem['#systemFacebookLink'], 'name': 'facebookLink hệ thống' },
    ];

    var verifies = verify(list);

    if (verifies.length == 0) {
        postJsonApiExplicit(
            '/api/admin/configuration/update',
            {
                id: newItem['#id'],
                sendEmail: newItem['#sendEmail'],
                sendEmailPassword: newItem['#sendEmailPassword'],
                receiveEmail: newItem['#receiveEmail'],
                contactPhone: newItem['#contactPhone'],
                contactZaloPhone: newItem['#contactZaloPhone'],
                systemPhone: newItem['#systemPhone'],
                systemEmail: newItem['#systemEmail'],
                systemZaloLink: newItem['#systemZaloLink'],
                systemYoutubeLink: newItem['#systemYoutubeLink'],
                systemFacebookLink: newItem['#systemFacebookLink'],
            },
            // Success: function (data) {}
            function (data) {
                if (data != null) {
                    window.location.reload();
                } else {
                    showMessage('error', 'Có lỗi phát sinh, không thể lưu cấu hình!');
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