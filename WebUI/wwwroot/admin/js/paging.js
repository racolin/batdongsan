function pageControl(key, num, max) {
    var left = parseInt(document.getElementById("paging-left-" + key).value);
    var right = parseInt(document.getElementById("paging-right-" + key).value);
    if ((num > 0 && right < max) || (num < 0 && left > 1)) {
        var wrap = document.getElementById("paging-wrap-" + key);
        left = left + (num > 0 ? 1 : -1);
        right = right + (num > 0 ? 1 : -1);

        wrap.style.translate = (left * -2.7 + 2.7) + 'rem 0';
        document.getElementById("paging-left-" + key).value = left;
        document.getElementById("paging-right-" + key).value = right;

        if (left > 1) {
            document.getElementById("paging-previous-" + key).classList.add("active");
        } else {
            document.getElementById("paging-previous-" + key).classList.remove("active");
        }

        if (right < max) {
            document.getElementById("paging-next-" + key).classList.add("active");
        } else {
            document.getElementById("paging-next-" + key).classList.remove("active");
        }
    }
}

function selectPaging(key, page) {
    var current = parseInt(document.getElementById("paging-current-" + key).value);
    if (current != page && typeof selectPage === "function") {
        selectPage(key, page);
    }
}