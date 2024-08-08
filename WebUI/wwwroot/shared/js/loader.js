var loadingCount = 0

function loading()
{
    if (loadingCount == 0) {
        var loader = $("#loader");
        loader.removeClass("d-none");
        loader.addClass("d-block");
    }
    loadingCount++;
}
function loaded()
{
    loadingCount--;
    if (loadingCount == 0) {
        var loader = $("#loader");
        loader.removeClass("d-block");
        loader.addClass("d-none");
    } 
}

function _preloading() {
    console.log("_preloading");
    var loaderWrap = $("#loader-wrap");
    loaderWrap.removeClass("loaded");
    loaderWrap.addClass("preloading");
}
function _loading() {
    console.log("_loading");
    var loaderWrap = $("#loader-wrap");
    loaderWrap.removeClass("preloading");
    loaderWrap.addClass("loading");
}
function _preloaded() {
    console.log("_preloaded");
    var loaderWrap = $("#loader-wrap");
    loaderWrap.removeClass("loading");
    loaderWrap.addClass("preloaded");
}

function _loaded() {
    console.log("_loaded");
    var loaderWrap = $("#loader-wrap");
    loaderWrap.removeClass("preloaded");
    loaderWrap.addClass("loaded");
}