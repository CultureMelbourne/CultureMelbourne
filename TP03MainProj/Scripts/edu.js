$(document).ready(function () {
    function navigateToSection(sectionId) {
        $('.story-item').hide();
        $(sectionId).show();
        var targetSection = $(sectionId);
        if (targetSection.length) {
            $('html, body').animate({
                scrollTop: targetSection.offset().top - 50
            }, 500);
            window.history.pushState(null, null, '?country=' + sectionId.replace('#', '').replace('-story', ''));
        } else {
            console.error("Section not found:", sectionId);
        }

    }
    $('.toggle-link').click(function (e) {
        e.preventDefault();
        $(this).siblings('.sub-nav').slideToggle();
        $('.sub-nav').not($(this).siblings('.sub-nav')).slideUp();
        var sectionId = $(this).attr('href');
        navigateToSection(sectionId);

        // 调用函数来发送Ajax请求
        var countryName = sectionId.replace('#', '').replace('-story', '');
        loadCountryData(countryName);
    });

    var urlParams = new URLSearchParams(window.location.search);
    var country = urlParams.get('country');
    if (country) {
        navigateToSection('#' + country + "-story");
        $('a[href="#' + country + '"]').next('.sub-nav').slideDown();

        // 页面加载完成后，调用函数来发送Ajax请求
        loadCountryData(country);
    } else {
        $('.sub-nav').first().slideDown();
        navigateToSection($('.toggle-link').first().attr('href'));
    }

    // 创建一个函数来发送Ajax请求
    function loadCountryData(countryName) {
        $.ajax({
            url: '/Education/GetCountryData',
            type: 'POST',
            data: { countryName: countryName },
            success: function (data) {
                loadChart(countryName, data);
            }
        });
    }

    function loadChart(countryName, data) {

        LoadAgeDistributes(countryName, data);
        LoadOccupationData(countryName, data);


    }


});
