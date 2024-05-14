document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');
    var lastHighlighted;

    var culture = document.getElementById('culture').getAttribute('data-culture');

    var calendar = new FullCalendar.Calendar(calendarEl, {
        // themeSystem: 'bootstrap', // Anything for FontAwesome?
        headerToolbar: {
            left: 'prevYear,prev,next,nextYear today',
            center: 'title',
            right: 'dayGridMonth,dayGridDay'
        },
        validRange: {
            start: '2023-01-01',
            end: '2026-01-01'
        },
        timeZone: 'local',
        initialDate: new Date().toISOString().split('T')[0], // Converts current date to YYYY-MM-DD format
        navLinks: true,
        editable: false, // User cannot move the calendar event
        dayMaxEvents: true,
        events: function (fetchInfo, successCallback, failureCallback) {
            $.ajax({
                url: getCulturalDatesUrl,
                type: 'GET',
                data: {
                    culture: culture,
                    start: fetchInfo.startStr,
                    end: fetchInfo.endStr // Assuming the cultural date is as a string
                },
                success: function (data) {
                    successCallback(data); // 'data' should be an array of event objects
                }
            });
        },
        dateClick: function (info) {
            var clickedDate = info.dateStr;

            if (lastHighlighted) {
                lastHighlighted.classList.remove('highlighted');
            }
            info.dayEl.classList.add('highlighted');
            lastHighlighted = info.dayEl;

            $.ajax({
                url: '/Events/GetEventsFromEventbrite',
                type: 'GET',
                data: {
                    culture: culture,
                    date: clickedDate
                },
                success: function (response) {
                    $('#events-list').html(response); // Insert the Partial View returned by the server into the page.
                }
            });
        },
        eventClick: function (info) {
            info.jsEvent.preventDefault(); // Prevent the browser from following the link.

            var clickedDate = info.event.startStr;

            if (lastHighlighted) {
                lastHighlighted.classList.remove('highlighted');
            }
            var dayEl = calendarEl.querySelector('.fc-daygrid-day[data-date="' + clickedDate + '"]');
            if (dayEl) {
                dayEl.classList.add('highlighted');
                lastHighlighted = dayEl;
            }

            $.ajax({
                url: '/Events/GetEventsFromEventbrite',
                type: 'GET',
                data: {
                    culture: culture,
                    date: clickedDate
                },
                success: function (response) {
                    $('#events-list').html(response); // Insert the Partial View returned by the server into the page.
                }
            });
        }
    });

    calendar.render();

    // Highlight today's date and trigger dateClick event
    setTimeout(function () {
        var today = calendarEl.querySelector('.fc-daygrid-day[data-date="' + new Date().toISOString().split('T')[0] + '"]');
        if (today) {
            today.classList.add('highlighted');
            lastHighlighted = today;

            $.ajax({
                url: '/Events/GetEventsFromEventbrite',
                type: 'GET',
                data: {
                    culture: culture,
                    date: new Date().toISOString().split('T')[0]
                },
                success: function (response) {
                    $('#events-list').html(response); // Insert the Partial View returned by the server into the page.
                }
            });
        }
    }, 100);
});
