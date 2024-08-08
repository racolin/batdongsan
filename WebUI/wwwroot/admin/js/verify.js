function verify(list) {
    var names = [];
    for (var i = 0; i < list.length; i++) {
        if (checkNull(list[i]['value'], list[i]['allow'])) {
            names.push(list[i]['name']);
        }
    }
    return names;
}
function checkNull(value, allow) {
    if (allow != undefined && allow.includes(value)) {
        return false;
    }
    return value == undefined || value == null || value == '' || value == 'false';
}

function filterDifferences(oldItem, newItem) {
    var keys = Object.keys(newItem);
    var count = 0;
    for (var i = 0; i < keys.length; i++) {
        if (newItem[keys[i]] == oldItem[keys[i]]) {
            count++;
        }
    }
    return count == keys.length;
}

function getItemsByQueries(queries) {
    var item = {}
    for (var i = 0; i < queries.length; i++) {
        item[queries[i]] = $(queries[i]).val();
    }
    return item;
}