document.addEventListener('DOMContentLoaded', function () {
    var sidebar = document.querySelector('.sidebar');
    var toggleBtn = document.querySelector('.sidebar-toggle-btn');

    // Expand the sidebar on hover
    sidebar.addEventListener('mouseover', function () {
        sidebar.classList.remove('collapsed');
        sidebar.classList.add('expanded');
    });

    // Collapse the sidebar when the mouse leaves
    sidebar.addEventListener('mouseout', function () {
        sidebar.classList.remove('expanded');
        sidebar.classList.add('collapsed');
    });

    // Optionally, expand the sidebar on button click (useful for mobile users)
    toggleBtn.addEventListener('click', function () {
        sidebar.classList.remove('collapsed');
        sidebar.classList.add('expanded');
    });
});
