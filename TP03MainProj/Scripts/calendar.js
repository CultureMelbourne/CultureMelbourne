
document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');
    var lastHighlighted;


    var culture = document.getElementById('culture').getAttribute('data-culture');

    var calendar = new FullCalendar.Calendar(calendarEl, {
        //themeSystem: 'bootstrap', //Anything for FontAwesome?
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
        editable: false, // User cannot move the calender event
        dayMaxEvents: true,
        events: function (fetchInfo, successCallback, failureCallback) {
            $.ajax({
                url: getCulturalDatesUrl,
                type: 'GET',
                // The below will actually depend on the format of the data being received
                data: {
                    culture: culture,
                    start: fetchInfo.startStr,
                    end: fetchInfo.endStr// Assuming the cultural date is as a string
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
        }

    });

    calendar.render();
});
