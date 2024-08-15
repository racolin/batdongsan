function initRadio(name, value) {
    $("input[name='" + name + "']").prop("checked", false);
    $("input[name='" + name + "'][value='" + value + "']").prop("checked", true);
}

var oldItem = {};

function showDetail(id) {
    $("input[name=id]").val(id);

    var item = $("#item-" + id);
    $("#item-name").text(item.find(".item-td-name").text())
    $("#item-email").text(item.find(".item-td-email").text())
    $("#item-phone").text(item.find(".item-td-phone").text())
    $("#item-content").text(item.find(".item-td-content").text())
    $("#item-address").text(item.find(".item-td-address").text())
    $("#item-note").text(item.find(".item-td-note").text())
    var state = item.find(".item-td-state").attr("data-state");
    initRadio('item-state', state);

    $("#modal-detail").modal('show');

    oldItem = getItemsByQueries(['input[name="item-state"]:checked', 'textarea[name="note"]']);
}

function closeModalDetail() {
    $("#modal-detail").modal('hide');
}

function updateStateItem(id, state, oldState, stateName) {
    var item = $("#item-" + id);
    item.find(".item-td-state").text(stateName);
    item.find(".item-td-state").attr("data-state", state);

    item.find(".item-td-state").addClass("item-td-" + state);
    item.find(".item-td-state").removeClass("item-td-" + oldState);
}
function updateItemAndCloseModal(state, note) {
    var id = $("input[name=id]").val();
    var item = $("#item-" + id);
    item.find(".item-td-note").text(note)
    item.find(".item-td-state").text($('input[name="item-state"][value="' + state + '"] + label').text());
    item.find(".item-td-state").attr("data-state", state);

    item.find(".item-td-state").addClass("item-td-" + state);
    item.find(".item-td-state").removeClass("item-td-" + oldItem['input[name="item-state"]:checked']);
    oldItem = getItemsByQueries(['input[name="item-state"]:checked', 'textarea[name="note"]']);

    closeModalDetail();
}

function removeItem(id) {
    showAlert(
        "Xóa liên hệ",
        "Bạn có thực sự muốn xóa liên hệ này không?",
        function () {
            removeContact(id);
        },
        true
    );
}
$("#btn-save").on("click", function () {

    // kiểm tra sự giống nhau giữa oldItem và newItem
    var newItem = getItemsByQueries(['input[name="item-state"]:checked', 'textarea[name="note"]']);
    var same = filterDifferences(oldItem, newItem);
    if (same) {
        showMessage('warning', 'Không phát hiện có sự thay đổi dữ liệu nào!');
        return;
    }

    var list = [
        { 'value': newItem['input[name="item-state"]:checked'], 'name': 'tình trạng' },
    ];

    var verifies = verify(list);

    if (verifies.length == 0) {
        updateContact(
            $("input[name=id]").val(),
            newItem['input[name="item-state"]:checked'],
            newItem['textarea[name="note"]'],
            function () {
                updateItemAndCloseModal(newItem['input[name="item-state"]:checked'], newItem['textarea[name="note"]']);
            }
        );
    } else {
        showMessage('warning', 'Vui lòng nhập ' + verifies.join(', ') + '!');
    }
});

$("#btn-remove").on("click", function () {
    closeModalDetail();
    showAlert(
        "Xóa liên hệ",
        "Bạn có thực sự muốn xóa liên hệ này không?",
        function () {
            removeContact($("input[name=id]").val())
        },
        true
    );
})
function updateState(id, state, stateName) {
    var item = $("#item-" + id);
    var oldState = item.find(".item-td-state").attr("data-state");
    if (oldState != state) {
        updateStateContact(id, state, function () { updateStateItem(id, state, oldState, stateName) });
    }
}
function updateContact(id, state, note, success) {

    // Save
    postJsonApiExplicit(
        '/api/admin/contact/update',
        {
            id: id,
            state: state,
            note: note,
        },
        // Success: function (data) {}
        function (data) {
            if (data != null) {
                success();
                showMessage('success', 'Cập nhật liên hệ thành công!');
            } else {
                showMessage('error', 'Có lỗi phát sinh, không thể cập nhật liên hệ!');
            }
        },
        // Warning: function(textMessage, data) {}
        // Error: function(message) {}
        // Error Api function() {}
    );
}
function updateStateContact(id, state, success) {

    // Save
    postJsonApiExplicit(
        '/api/admin/contact/update-state',
        {
            id: id,
            state: state,
        },
        // Success: function (data) {}
        function (data) {
            if (data != null) {
                showMessage('success', 'Cập nhật liên hệ thành công!');
                success();
            } else {
                showMessage('error', 'Có lỗi phát sinh, không thể cập nhật liên hệ!');
            }
        },
        // Warning: function(textMessage, data) {}
        // Error: function(message) {}
        // Error Api function() {}
    );
}
function removeContact(id) {
    getApiExplicit(
        '/api/admin/contact/remove?id=' + id,
        // Success: function (data) {}
        function (data) {
            if (data != null) {
                window.location.reload();
            } else {
                showMessage('error', 'Có lỗi phát sinh, không thể xóa liên hệ!');
            }
        },
        // Warning: function(textMessage, data) {}
        // Error: function(message) {}
        // Error Api function() {}
    );
}