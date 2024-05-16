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
        $('html, header').animate({ scrollTop: 0 }, 'slow');
    });
});

document.addEventListener("DOMContentLoaded", function () {
    var toggler = document.querySelector(".header_navbar-toggler");
    var sidebar = document.getElementById("header_sidebar");
    var closeSidebar = document.querySelector(".close-sidebar");

    toggler.addEventListener("click", function () {
        sidebar.classList.toggle("show");
        toggler.classList.toggle("hidden");
    });

    closeSidebar.addEventListener("click", function () {
        sidebar.classList.toggle("show");
        toggler.classList.toggle("hidden");
    });
});
