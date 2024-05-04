$(document).ready(function () {
    // Check if the window is scrolled and toggle button visibility
    $(window).scroll(function () {
        if ($(this).scrollTop() > 20) {
            $('#scrollToTopBtn').fadeIn();
        } else {
            $('#scrollToTopBtn').fadeOut();
        }
    });

    // Animate the scroll to the top when the button is clicked
    $('#scrollToTopBtn').click(function () {
        $('html, body').animate({ scrollTop: 0 }, 'slow');
    });
});