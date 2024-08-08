function getApiExplicit(url, successCallback,  warningCallBack, errorCallback, errorApiCallback) {
    $.ajax({
        url: url,
        type: 'GET',
        beforeSend: loading,
        complete: loaded,
        success: function (data) {
            // warning response
            if (data.message.type == 1) {
                if (!warningCallBack == false) {
                    warningCallBack(data.message.message, data.data);
                } else {
                    showMessage('warning', data.message.name)
                }
                return;
            }
            // error response
            if (data.message.type == 2) {
                if (!errorCallback == false) {
                    errorCallback(data.message);
                } else {
                    showMessage('error', data.message.name)
                }
                return;
            }
            // success response
            if (!successCallback == false) {
                successCallback(data.data);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            // error api response
            showMessage('error', 'Không thể kết nối đến server, hãy liên hệ admin ngay!');
            if (!errorApiCallback == false) {
                errorApiCallback();
            }
        },
    });
}
function getApiImplicit(url, successCallback,  warningCallBack, errorCallback, errorApiCallback) {
    $.ajax({
        url: url,
        type: 'GET',
        success: function (data) {
            // warning response
            if (data.message.type == 1) {
                if (!warningCallBack == false) {
                    warningCallBack(data.message.message, data.data);
                }
                return;
            }
            // error response
            if (data.message.type == 2) {
                if (!errorCallback == false) {
                    errorCallback(data.message);
                }
                return;
            }
            // success response
            if (!successCallback == false) {
                successCallback(data.data);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            // error api response
            showMessage('error', 'Không thể kết nối đến server, hãy liên hệ admin ngay!');
            if (!errorApiCallback == false) {
                errorApiCallback();
            }
        },
    });
}
function postApiExplicit(url, data, successCallback, warningCallBack, errorCallback, errorApiCallback) {
    $.ajax({
        url: url,
        type: 'POST',
        data: data,//dataJson,
        datatype: 'json',
        beforeSend: loading,
        complete: loaded,
        success: function (data) {
            // warning response
            if (data.message.type == 1) {
                if (!warningCallBack == false) {
                    warningCallBack(data.message.message, data.data);
                } else {
                    showMessage('warning', data.message.name)
                }
                return;
            }
            // error response
            if (data.message.type == 2) {
                if (!errorCallback == false) {
                    errorCallback(data.message);
                } else {
                    showMessage('error', data.message.name)
                }
                return;
            }
            // success response
            if (!successCallback == false) {
                successCallback(data.data);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            // error api response
            showMessage('error', 'Không thể kết nối đến server, hãy liên hệ admin ngay!');
            if (!errorApiCallback == false) {
                errorApiCallback();
            }
        },
    });
}
function postApiImplicit(url, data, successCallback, warningCallBack, errorCallback, errorApiCallback) {
    $.ajax({
        url: url,
        type: 'POST',
        data: data,//dataJson,
        datatype: 'json',
        success: function (data) {
            // warning response
            if (data.message.type == 1) {
                if (!warningCallBack == false) {
                    warningCallBack(data.message.message, data.data);
                }
                return;
            }
            // error response
            if (data.message.type == 2) {
                if (!errorCallback == false) {
                    errorCallback(data.message);
                }
                return;
            }
            // success response
            if (!successCallback == false) {
                successCallback(data.data);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            // error api response
            showMessage('error', 'Không thể kết nối đến server, hãy liên hệ admin ngay!');
            if (!errorApiCallback == false) {
                errorApiCallback();
            }
        },
    });
}
function postJsonApiExplicit(url, data, successCallback, warningCallBack, errorCallback, errorApiCallback) {
    $.ajax({
        url: url,
        type: 'POST',
        data: JSON.stringify(data),//dataJson,
        datatype: 'json',
        contentType: 'application/json',
        beforeSend: loading,
        complete: loaded,
        success: function (data) {
            // warning response
            if (data.message.type == 1) {
                if (!warningCallBack == false) {
                    warningCallBack(data.message.message, data.data);
                } else {
                    showMessage('warning', data.message.name)
                }
                return;
            }
            // error response
            if (data.message.type == 2) {
                if (!errorCallback == false) {
                    errorCallback(data.message);
                } else {
                    showMessage('error', data.message.name)
                }
                return;
            }
            // success response
            if (!successCallback == false) {
                successCallback(data.data);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            // error api response
            showMessage('error', 'Không thể kết nối đến server, hãy liên hệ admin ngay!');
            if (!errorApiCallback == false) {
                errorApiCallback();
            }
        },
    });
}
function postJsonApiImplicit(url, data, successCallback,  warningCallBack, errorCallback, errorApiCallback) {
    $.ajax({
        url: url,
        type: 'POST',
        data: JSON.stringify(data),//dataJson,
        datatype: 'json',
        contentType: 'application/json',
        success: function (data) {
            // warning response
            if (data.message.type == 1) {
                if (!warningCallBack == false) {
                    warningCallBack(data.message.message, data.data);
                }
                return;
            }
            // error response
            if (data.message.type == 2) {
                if (!errorCallback == false) {
                    errorCallback(data.message);
                }
                return;
            }
            // success response
            if (!successCallback == false) {
                successCallback(data.data);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            // error api response
            showMessage('error', 'Không thể kết nối đến server, hãy liên hệ admin ngay!');
            if (!errorApiCallback == false) {
                errorApiCallback();
            }
        },
    });
}
function postFormDataApiExplicit(url, formData, successCallback,  warningCallBack, errorCallback, errorApiCallback) {
    $.ajax({
        url: url,
        type: 'POST',
        data: formData,//formData,
        datatype: 'json',
        enctype: 'multipart/form-data',
        contentType: false,
        processData: false,
        beforeSend: loading,
        complete: loaded,
        success: function (data) {
            // warning response
            if (data.message.type == 1) {
                if (!warningCallBack == false) {
                    warningCallBack(data.message.message, data.data);
                } else {
                    showMessage('warning', data.message.name)
                }
                return;
            }
            // error response
            if (data.message.type == 2) {
                if (!errorCallback == false) {
                    errorCallback(data.message);
                } else {
                    showMessage('error', data.message.name)
                }
                return;
            }
            // success response
            if (!successCallback == false) {
                successCallback(data.data);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            // error api response
            showMessage('error', 'Không thể kết nối đến server, hãy liên hệ admin ngay!');
            if (!errorApiCallback == false) {
                errorApiCallback();
            }
        },
    });
}
function postFormDataApiImplicit(url, formData, successCallback,  warningCallBack, errorCallback, errorApiCallback) {
    $.ajax({
        url: url,
        type: 'POST',
        data: formData,//formData,
        datatype: 'json',
        enctype: 'multipart/form-data',
        contentType: false,
        processData: false,
        success: function (data) {
            // warning response
            if (data.message.type == 1) {
                if (!warningCallBack == false) {
                    warningCallBack(data.message.message, data.data);
                }
                return;
            }
            // error response
            if (data.message.type == 2) {
                if (!errorCallback == false) {
                    errorCallback(data.message);
                }
                return;
            }
            // success response
            if (!successCallback == false) {
                successCallback(data.data);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            // error api response
            showMessage('error', 'Không thể kết nối đến server, hãy liên hệ admin ngay!');
            if (!errorApiCallback == false) {
                errorApiCallback();
            }
        },
    });
}
    