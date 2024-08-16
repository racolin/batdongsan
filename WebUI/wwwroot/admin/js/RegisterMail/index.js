function initRadio(name, value) {
    $("input[name='" + name + "']").prop("checked", false);
    $("input[name='" + name + "'][value='" + value + "']").prop("checked", true);
}

var oldItem = {};

function showDetail(id) {
    $("input[name=id]").val(id);

    var item = $("#item-" + id);
    $("#item-email").text(item.find(".item-td-email").text())
    $("#item-createdDate").text(item.find(".item-td-createdDate").text())
    var state = item.find(".item-td-state").attr("data-state");
    initRadio('item-state', state);

    $("#modal-detail").modal('show');

    oldItem = getItemsByQueries(['input[name="item-state"]:checked']);
}

function closeModalDetail() {
    $("#modal-detail").modal('hide');
}

function updateStateItem(id, state, oldState, stateName) {
    var checkedStates = [];
    const regexp = /[?|&]state=([^&|$]*)/g;
    const str = window.location.search;

    const array = [...str.matchAll(regexp)];
    for (var i = 0; i < array.length; i++) {
        checkedStates.push(array[i][1])
    }
    if (checkedStates.length != 0 && (!checkedStates.includes(state))) {
        window.location.reload();
    }

    var item = $("#item-" + id);
    item.find(".item-td-state").text(stateName);
    item.find(".item-td-state").attr("data-state", state);

    item.find(".item-td-state").addClass("item-td-" + state);
    item.find(".item-td-state").removeClass("item-td-" + oldState);
}
function updateItemAndCloseModal(state) {
    var id = $("input[name=id]").val();
    var item = $("#item-" + id);
    item.find(".item-td-state").text($('input[name="item-state"][value="' + state + '"] + label').text());
    item.find(".item-td-state").attr("data-state", state);

    item.find(".item-td-state").addClass("item-td-" + state);
    item.find(".item-td-state").removeClass("item-td-" + oldItem['input[name="item-state"]:checked']);
    oldItem = getItemsByQueries(['input[name="item-state"]:checked']);

    closeModalDetail();
}

function removeItem(id) {
    showAlert(
        "Xóa email đăng ký",
        "Bạn có thực sự muốn xóa email đăng ký này không?",
        function () {
            removeRegisterMail(id);
        },
        true
    );
}
$("#btn-save").on("click", function () {

    // kiểm tra sự giống nhau giữa oldItem và newItem
    var newItem = getItemsByQueries(['input[name="item-state"]:checked']);
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
        updateStateRegisterMail(
            $("input[name=id]").val(),
            newItem['input[name="item-state"]:checked'],
            function () {
                updateItemAndCloseModal(newItem['input[name="item-state"]:checked']);
            }
        );
    } else {
        showMessage('warning', 'Vui lòng nhập ' + verifies.join(', ') + '!');
    }
});

$("#btn-remove").on("click", function () {
    closeModalDetail();
    showAlert(
        "Xóa email đăng ký",
        "Bạn có thực sự muốn xóa email đăng ký này không?",
        function () {
            removeRegisterMail($("input[name=id]").val())
        },
        true
    );
})
function updateState(id, state, stateName) {
    var item = $("#item-" + id);
    var oldState = item.find(".item-td-state").attr("data-state");
    if (oldState != state) {
        updateStateRegisterMail(id, state, function () { updateStateItem(id, state, oldState, stateName) });
    }
}
function updateStateRegisterMail(id, state, success) {

    // Save
    postJsonApiExplicit(
        '/api/admin/registermail/update-state',
        {
            id: id,
            state: state,
        },
        // Success: function (data) {}
        function (data) {
            if (data != null) {
                showMessage('success', 'Cập nhật email đăng ký thành công!');
                success();
            } else {
                showMessage('error', 'Có lỗi phát sinh, không thể cập nhật email đăng ký!');
            }
        },
        // Warning: function(textMessage, data) {}
        // Error: function(message) {}
        // Error Api function() {}
    );
}
function removeRegisterMail(id) {
    getApiExplicit(
        '/api/admin/registermail/remove?id=' + id,
        // Success: function (data) {}
        function (data) {
            if (data != null) {
                window.location.reload();
            } else {
                showMessage('error', 'Có lỗi phát sinh, không thể xóa email đăng ký!');
            }
        },
        // Warning: function(textMessage, data) {}
        // Error: function(message) {}
        // Error Api function() {}
    );
}