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
    if (status == "Active") {
        $(".btn-remove-wrap").removeClass("d-none");
        item.find(".btn-remove-wrap").addClass("d-none");
        var oldActive = $("td p.item-td-Active");
        oldActive.removeClass("item-td-Active").addClass("item-td-Draft");
        oldActive.attr("data-status", "Draft");
        oldActive.text("Nháp");
    }

    item.find(".item-td-status").text(statusName);
    item.find(".item-td-status").attr("data-status", status);

    item.find(".item-td-status").addClass("item-td-" + status);
    item.find(".item-td-status").removeClass("item-td-" + oldStatus);
}

function removeItem(id) {
    showAlert(
        "Xóa nội dung web",
        "Bạn có thực sự muốn xóa nội dung web này không?",
        function () {
            removeContent(id);
        },
        true
    );
}
function updateStatus(id, status, statusName) {
    var item = $("#item-" + id);
    var oldStatus = item.find(".item-td-status").attr("data-status");
    if (oldStatus != status) {
        updateStatusContent(id, status, function () { updateStatusItem(id, status, oldStatus, statusName) });
    }
}
function updateStatusContent(id, status, success) {

    // Save
    postJsonApiExplicit(
        '/api/admin/content/update-status',
        {
            id: id,
            status: status,
        },
        // Success: function (data) {}
        function (data) {
            if (data != null) {
                showMessage('success', 'Cập nhật nội dung web thành công!');
                success();
            } else {
                showMessage('error', 'Có lỗi phát sinh, không thể cập nhật nội dung web!');
            }
        },
        // Warning: function(textMessage, data) {}
        // Error: function(message) {}
        // Error Api function() {}
    );
}
function removeContent(id) {
    getApiExplicit(
        '/api/admin/content/remove?id=' + id,
        // Success: function (data) {}
        function (data) {
            if (data != null) {
                window.location.reload();
            } else {
                showMessage('error', 'Có lỗi phát sinh, không thể xóa nội dung web!');
            }
        },
        // Warning: function(textMessage, data) {}
        // Error: function(message) {}
        // Error Api function() {}
    );
}