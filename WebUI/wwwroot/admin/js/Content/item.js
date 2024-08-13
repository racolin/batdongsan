var oldIntroduce = "";
var oldSlider = {}
$(document).ready(function () {
    oldItems = getItemsByQueries([
        '#name', '#imageId-homeImageId', '#imageId-bgHomeImageId', '#imageId-newsImageId', 'input[name="status"]:checked',
        '#imageId-contactImageId', '#newsMarketSection', '#newsProjectSection']);
    oldItems['#introduceSection'] = $("#introduceSection").summernote("code");

    oldSliderItems = [];
    var sliderImages = $("#slider-project").find(".slider-image:not(.d-none)");
    sliderImages.each(function (index) {
        var sliderImage = sliderImages[index];
        var key = sliderImage.id.split('-').slice(1).join('-');
        oldSliderItems.push(getItemsByQueries(["#id-" + key, "#imageId-" + key, "#order-" + key, "#link-" + key,]));
    });
});
$("#btn-save").click(function () {
    // check image page có cái nào trống không [{id, position, imageId}]
    var newItems = getItemsByQueries([
        '#name', '#imageId-homeImageId', '#imageId-bgHomeImageId', '#imageId-newsImageId', 'input[name="status"]:checked',
        '#imageId-contactImageId', '#newsMarketSection', '#newsProjectSection']);
    newItems['#introduceSection'] = $("#introduceSection").summernote("code");

    var newSlider = [];
    var newSliderItems = [];
    var sliderImages = $("#slider-project").find(".slider-image:not(.d-none)");
    console.log(sliderImages);
    sliderImages.each(function (index) {
        var sliderImage = sliderImages[index];
        var key = sliderImage.id.split('-').slice(1).join('-');
        var temp = getItemsByQueries(["#id-" + key, "#imageId-" + key, "#order-" + key, "#link-" + key,]);
        newSlider.push(temp);
        newSliderItems.push({
            id: checkNull(temp["#id-" + key]) ? null : temp["#id-" + key],
            imageId: temp["#imageId-" + key],
            order: temp["#order-" + key],
            link: temp["#link-" + key],
        });
    });

    // Kiểm tra xem có điền thiếu chỗ nào không
    var fillItems = true;
    var itemKeys = Object.keys(newItems);
    for (var i in itemKeys) {
        fillItems &&= !checkNull(newItems[itemKeys[i]], []);
    }
    var fillSlider = true;
    for (var index in newSlider) {
        var sliderKeys = Object.keys(newSlider[index]);
        for (var i in sliderKeys) {
            if (sliderKeys[i].split('-')[0] != '#link' && sliderKeys[i].split('-')[0] != '#id') {
                fillSlider &&= !checkNull(newSlider[index][sliderKeys[i]], []);
            }
        }
    }

    if (!fillItems || !fillSlider) {
        showMessage('warning', 'Bạn phải điền đầy đủ thông tin trước khi lưu!');
        return;
    }

    // Xem xem giá trị cũ và mới có khác nhau không để cập nhật is-update
    var isUpdateIntroduceSection = false;
    if (!(oldItems["#introduceSection"] == newItems["#introduceSection"])) {
        isUpdateIntroduceSection = true;
    }

    var isUpdateItems = false;
    if (!filterDifferences(oldItems, newItems)) {
        isUpdateItems = true;
    }
    
    var isUpdateProjectSlider = false;
    if (newSlider.length != oldSliderItems.length) {
        isUpdateProjectSlider = true;
    } else {
        for (var i in newSlider) {
            if (!filterDifferences(newSlider[i], oldSliderItems[i])) {
                isUpdateProjectSlider = true;
                break;
            }
        }
    }

    if (!isUpdateItems && !isUpdateProjectSlider) {
        showMessage('warning', 'Không phát hiện bất kỳ sự thay đổi dữ liệu nào!');
        return;
    }

    var id = $("#id").val();
    id = (!id) ? null : id;
    var jsonData = {
        isUpdateProjectSlider: isUpdateProjectSlider,
        isUpdateIntroduceSection: isUpdateIntroduceSection,
        homeImageId: newItems['#imageId-homeImageId'],
        bgHomeImageId: newItems['#imageId-bgHomeImageId'],
        newsImageId: newItems['#imageId-newsImageId'],
        contactImageId: newItems['#imageId-contactImageId'],
        id: id,
        name: newItems['#name'],
        status: newItems['input[name="status"]:checked'],
        introduceSection: newItems['#introduceSection'],
        newsProjectSection: newItems['#newsProjectSection'],
        newsMarketSection: newItems['#newsMarketSection'],
        projectSlider: newSliderItems,
    }

    postJsonApiExplicit(
        '/api/admin/content/save', jsonData,
        // Success: function (data) {}
        function (data) {
            if (data != null) {
                window.location.replace('/admin/content/item?id=' + data);
            } else {
                showMessage('error', 'Có lỗi phát sinh, không thể lưu nội dung này!');
            }
        },
    // Warning: function(textMessage, data) {}
    // Error: function(message) {}
    // Error Api function() {}
    );
})