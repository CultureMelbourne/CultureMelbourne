
$(document).ready(function () {

    // Function to navigate to specific sections
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

    // Toggle sections and load data
    $('.toggle-link').click(function (e) {
        e.preventDefault();
        $(this).siblings('.sub-nav').slideToggle();
        $('.sub-nav').not($(this).siblings('.sub-nav')).slideUp();
        var sectionId = $(this).attr('href');
        navigateToSection(sectionId);
        var countryName = sectionId.replace('#', '').replace('-story', '');
        loadCountryData(countryName);
    });

    // Check URL parameters to display charts or default view
    var urlParams = new URLSearchParams(window.location.search);
    var country = urlParams.get('country');
    if (country) {
        navigateToSection('#' + country + "-story");
        loadCountryData(country);
    } else {
        navigateToSection($('.toggle-link').first().attr('href'));
    }

    // Load data and display charts
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
        loadPopulation(countryName, data.populations);
        loadOccupationChart(countryName, data.occupation);
        loadReligionChart(countryName, data.religions);
    }

    // Change event for selecting chart type
    $('.chart-type-selector').change(function () {
        var selectedChart = $(this).val();
        var countryId = $(this).closest('.story-item').attr('id').split('-')[0];
        $('.chart-container', `#${countryId}-data`).hide(); // Hide all charts
        $(`#${countryId}-${selectedChart}`).show(); // Show the selected chart
    });
});
