function updateIsLockItem(id, islock, oldIsLock, isLockName) {
    var checkedIsLockList = [];
    const regexp = /[?|&]isLock=([^&|$]*)/g;
    const str = window.location.search;

    const array = [...str.matchAll(regexp)];
    for (var i = 0; i < array.length; i++) {
        checkedIsLockList.push(array[i][1])
    }
    if (checkedIsLockList.length != 0 && (!checkedIsLockList.includes(islock))) {
        window.location.reload();
    }

    var item = $("#item-" + id);

    item.find(".item-td-is-lock").text(isLockName);
    item.find(".item-td-is-lock").attr("data-is-lock", islock);

    console.log(item.find(".item-td-is-lock") , "item-td-" + islock);
    item.find(".item-td-is-lock").addClass("item-td-" + islock);
    item.find(".item-td-is-lock").removeClass("item-td-" + oldIsLock);
}

function updateIsLock(userId, isLock, isLockName) {
    var item = $("#item-" + userId);
    var oldIsLock = item.find(".item-td-is-lock").attr("data-is-lock");
    console.log(oldIsLock, isLock);
    console.log(typeof (oldIsLock), typeof (isLock));
    if (oldIsLock != isLock) {
        updateIsLockUser(userId, isLock, function () { updateIsLockItem(userId, isLock, oldIsLock, isLockName) });
    }
}
function updateIsLockUser(userId, isLock, success) {

    // Save
    postJsonApiExplicit(
        '/api/admin/user/set-lock',
        {
            userId: userId,
            isLock: isLock == 'true',
        },
        // Success: function (data) {}
        function (data) {
            if (data != null) {
                showMessage('success', 'Cập nhật người dùng thành công!');
                success();
            } else {
                showMessage('error', 'Có lỗi phát sinh, không thể cập nhật người dùng!');
            }
        },
        // Warning: function(textMessage, data) {}
        // Error: function(message) {}
        // Error Api function() {}
    );
}