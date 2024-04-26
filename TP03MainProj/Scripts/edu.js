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

    });


    var urlParams = new URLSearchParams(window.location.search);
    var country = urlParams.get('country');
    if (country) {
        navigateToSection('#' + country + "-story");
        $('a[href="#' + country + '"]').next('.sub-nav').slideDown();
    } else {
        $('.sub-nav').first().slideDown();
        navigateToSection($('.toggle-link').first().attr('href'));

    }
});