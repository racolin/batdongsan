// Alert
function showAlert(title, content, action, canDismiss) {
    var isClick = false;
    $("#alert-action").off("click")
    $("#alert-action").click(function () {
        if (!isClick) {
            action();
            isClick = true;
        }
    $('#alert').modal('hide');
    });
    if (!canDismiss == false) {
        $("#alert-dismiss").removeClass("d-none");
        $("#alert-close").removeClass("d-none");
    }
    $("#alert-label").text(title);
    $("#alert-content").html(content);

    $('#alert').modal('show');

    $('#alert').on('hidden.bs.modal', function (event) {

        if (!canDismiss == false) {
            $("#alert-dismiss").addClass("d-none");
            $("#alert-close").addClass("d-none");
        }
        $("#alert-label").text("");
        $("#alert-content").html("");
    })
}

// Message
var messageCount = 0;
function showMessage(type, content, zIndex) {
    var message = $(".message-" + type + ".d-none");
    if (!message == false) {
        var item = message.clone();
        
        item.find("span").text(content);
        $("#message-wrap").append(item);
        item.removeClass("d-none");
        item.addClass("show");
        var timeOut = setTimeout(() => { item.removeClass("show"); setTimeout(() => { item.remove(); }, 500) }, 4000);
        item.hover(() => {clearTimeout(timeOut);},
            () => {
                timeOut = setTimeout(
                    () => {
                        item.removeClass("show");
                        setTimeout(() => { item.remove(); }, 500);
                    }, 4000);
            });
        item.on('closed.bs.alert', function () {
            clearTimeout(timeOut);
        })
    }
}