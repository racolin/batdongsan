function updateStatusItem(id, status, oldStatus, statusName) {
    var checkedStatusList = [];
    const regexp = /[?|&]status=([^&|$]*)/g;
    const str = window.location.search;

    const array = [...str.matchAll(regexp)];
    for (var i = 0; i < array.length; i++) {
        checkedStatusList.push(array[i][1])
    }
    if (checkedStatusList.length != 0 && (!checkedStatusList.includes(status))) {
        window.location.reload();
    }

    var item = $("#item-" + id);

    item.find(".item-td-status").text(statusName);
    item.find(".item-td-status").attr("data-status", status);

    item.find(".item-td-status").addClass("item-td-" + status);
    item.find(".item-td-status").removeClass("item-td-" + oldStatus);
}

function removeItem(id) {
    showAlert(
        "Xóa bài viết",
        "Bạn có thực sự muốn xóa bài viết này không?",
        function () {
            removeNews(id);
        },
        true
    );
}
function updateStatus(id, status, statusName) {
    var item = $("#item-" + id);
    var oldStatus = item.find(".item-td-status").attr("data-status");
    if (oldStatus != status) {
        updateStatusNews(id, status, function () { updateStatusItem(id, status, oldStatus, statusName) });
    }
}
function updateStatusNews(id, status, success) {

    // Save
    postJsonApiExplicit(
        '/api/admin/news/update-status',
        {
            id: id,
            status: status,
        },
        // Success: function (data) {}
        function (data) {
            if (data != null) {
                showMessage('success', 'Cập nhật bài viết thành công!');
                success();
            } else {
                showMessage('error', 'Có lỗi phát sinh, không thể cập nhật bài viết!');
            }
        },
        // Warning: function(textMessage, data) {}
        // Error: function(message) {}
        // Error Api function() {}
    );
}
function removeNews(id) {
    getApiExplicit(
        '/api/admin/news/remove?id=' + id,
        // Success: function (data) {}
        function (data) {
            if (data != null) {
                window.location.reload();
            } else {
                showMessage('error', 'Có lỗi phát sinh, không thể xóa bài viết!');
            }
        },
        // Warning: function(textMessage, data) {}
        // Error: function(message) {}
        // Error Api function() {}
    );
}