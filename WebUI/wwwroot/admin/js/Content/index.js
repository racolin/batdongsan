var oldSection = {};
var oldImagePages = {}
var oldSlider = {}
$(document).ready(function () {
    oldImagePages = getItemsByQueries([
        '#id-home-screen', '#position-home-screen', '#imageId-home-screen',
        '#id-bg-home', '#position-bg-home', '#imageId-bg-home',
        '#id-news-screen', '#position-news-screen', '#imageId-news-screen',
        '#id-contact-screen', '#position-contact-screen', '#imageId-contact-screen']);

    oldSection = getItemsByQueries(['#sectionId', '#sectionPosition']);
    oldSection['sectionContent'] = $("#sectionContent").summernote("code");

    oldSlider = getItemsByQueries(['#sliderProjectId', '#sliderProjectPosition']);
    oldSlider['items'] = [];
    var sliderImages = $("#slider-project").find(".slider-image");
    sliderImages.each(function (index) {
        var sliderImage = sliderImages[index];
        var key = sliderImage.id.split('-').slice(1).join('-');
        oldSlider['items'].push(getItemsByQueries(["#id-" + key, "#imageId-" + key, "#order-" + key, "#link-" + key,]));
    });
});
$("#btn-save").click(function () {
    // check image page có cái nào trống không [{id, position, imageId}]
    var newImagePages = getItemsByQueries([
        '#id-home-screen', '#position-home-screen', '#imageId-home-screen',
        '#id-bg-home', '#position-bg-home', '#imageId-bg-home',
        '#id-news-screen', '#position-news-screen', '#imageId-news-screen',
        '#id-contact-screen', '#position-contact-screen', '#imageId-contact-screen',]);

    var newSection = getItemsByQueries(['#sectionId', '#sectionPosition']);
    newSection['sectionContent'] = $("#sectionContent").summernote("code");

    var newSlider = getItemsByQueries(['#sliderProjectId', '#sliderProjectPosition']);
    newSlider['items'] = [];
    newSliderItems = [];
    var sliderImages = $("#slider-project").find(".slider-image");
    sliderImages.each(function (index) {
        var sliderImage = sliderImages[index];
        var key = sliderImage.id.split('-').slice(1).join('-');
        var v = getItemsByQueries(["#id-" + key, "#imageId-" + key, "#order-" + key, "#link-" + key,]);
        newSlider['items'].push(v);
        newSliderItems.push({
            id: v["#id-" + key] == "" ? null : v["#id-" + key],
            imageId: v["#imageId-" + key],
            order: v["#order-" + key],
            link: v["#link-" + key] == "" ? null : v["#link-" + key],
        });
    });

    // Kiểm tra xem có điền thiếu chỗ nào không
    var fillImagePages = true;
    var imagePageKeys = Object.keys(newImagePages);
    for (var i in imagePageKeys) {
        fillImagePages &&= !checkNull(newImagePages[imagePageKeys[i]], []);
    }
    var fillSection = true;
    var sectionsKeys = Object.keys(newSection);
    for (var i in sectionsKeys) {
        fillSection &&= !checkNull(newSection[sectionsKeys[i]], []);
    }
    var fillSlider = true;
    fillSlider &&= !checkNull(newSlider["#sliderProjectId"], [])
    fillSlider &&= !checkNull(newSlider["#sliderProjectPosition"], [])
    for (var index in newSlider['items']) {
        var sliderKeys = Object.keys(newSlider['items'][index]);
        for (var i in sliderKeys) {
            if (sliderKeys[i].split('-')[0] != '#link' && sliderKeys[i].split('-')[0] != '#id') {
                console.log(!checkNull(newSlider['items'][index][sliderKeys[i]], []));
                console.log(newSlider['items'][index][sliderKeys[i]]);
                console.log(sliderKeys[i].split('-')[0]);
                fillSlider &&= !checkNull(newSlider['items'][index][sliderKeys[i]], []);
            }
        }
    }
    console.log(!fillImagePages || !fillSection || !fillSlider);
    console.log(fillImagePages,fillSection,fillSlider);
    if (!fillImagePages || !fillSection || !fillSlider) {
        showMessage('warning', 'Bạn phải điền đầy đủ thông tin trước khi lưu!');
        return;
    }

    // Xem xem giá trị cũ và mới có khác nhau không để cập nhật is-update
    var isUpdateImagePages = false;
    if (!filterDifferences(oldImagePages, newImagePages)) {
        isUpdateImagePages = true;
    }

    var isUpdateSection = false;
    if (!filterDifferences(oldSection, newSection)) {
        isUpdateSection = true;
    }

    var isUpdateSlider = false;
    if (newSlider['#sliderProjectId'] == oldSlider['#sliderProjectId']
        && newSlider['#sliderProjectPosition'] == oldSlider['#sliderProjectPosition']) {
        if (newSlider['items'].length != oldSlider['items'].length) {
            isUpdateSlider = true;
        } else {
            for (var i in newSlider['items']) {
                if (!filterDifferences(newSlider['items'][i], oldSlider['items'][i])) {
                    isUpdateSlider = true;
                    break;
                }
            }
        }
    }
    console.log(!isUpdateImagePages && !isUpdateSection && !isUpdateSlider);
    console.log(isUpdateImagePages,isUpdateSection,isUpdateSlider);

    if (!isUpdateImagePages && !isUpdateSection && !isUpdateSlider) {
        showMessage('warning', 'Không phát hiện bất kỳ sự thay đổi dữ liệu nào!');
        return;
    }

    var jsonData = {
        isUpdateImagePages: isUpdateImagePages,
        isUpdateSection: isUpdateSection,
        isUpdateSlider: isUpdateSlider,
        section: {
            id: newSection['#sectionId'],
            position: newSection['#sectionPosition'],
            content: newSection['sectionContent'],
        },
        slider: {
            id: newSlider['#sliderProjectId'],
            position: newSlider['#sliderProjectPosition'],
            items: newSliderItems,
        },
        imagePages: [
            {
                id: newImagePages['#id-home-screen'],
                position: newImagePages['#position-home-screen'],
                imageId: newImagePages['#imageId-home-screen']
            },
            {
                id: newImagePages['#id-bg-home'],
                position: newImagePages['#position-bg-home'],
                imageId: newImagePages['#imageId-bg-home']
            },
            {
                id: newImagePages['#id-news-screen'],
                position: newImagePages['#position-news-screen'],
                imageId: newImagePages['#imageId-news-screen']
            },
            {
                id: newImagePages['#id-contact-screen'],
                position: newImagePages['#position-contact-screen'],
                imageId: newImagePages['#imageId-contact-screen']
            }
        ]
    }

    postJsonApiExplicit(
        '/api/admin/content/save', jsonData,
        // Success: function (data) {}
        function (data) {
            if (data != null) {
                window.location.replace('/admin/content');
            } else {
                showMessage('error', 'Có lỗi phát sinh, không thể lưu nội dung này!');
            }
        },
    // Warning: function(textMessage, data) {}
    // Error: function(message) {}
    // Error Api function() {}
    );
})