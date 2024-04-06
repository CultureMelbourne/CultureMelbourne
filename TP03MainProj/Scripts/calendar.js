// To Do - 
// Replace 'api/getCulturalDates' with the actual API endpoint that serves cultural date data.
// Replace 'api/getEventsFromEventbrite' with the actual API endpoint that serves Eventbrite data.
document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');

    var culture = '@ViewBag.Culture'; // For retreiving the culture name from the server-side ViewBag

    var calendar = new FullCalendar.Calendar(calendarEl, {
        headerToolbar: {
            left: 'prevYear,prev,next,nextYear today',
            center: 'title',
            right: 'dayGridMonth,dayGridweek,dayGridDay'
        },
        intialDate: '2024-01-01',
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
                    end: fetchInfo.startStr// Assuming the cultural date is as a string
                },
                success: function (data) {
                    successCallback(data); // 'data' should be an array of event objects
                }
            });
        },

        dayClick: function (date, allDay, jsEvent, view) {
            var clickedDate = date.toISOString();
            $.ajax({
                url: getEventsFromEventbriteUrl,
                type: 'GET',
                data: {
                    culture: culture,
                    date: clickedDate
                },
                success: function (events) {
                    // Assuming 'events' is an array of strings
                    var eventsHtml = events.map(function (event) {
                        return '<li>' + event + '</li>';
                    }).join('');
                    $('#events-list').html('<ul>' + eventsHtml + '</ul>');
                }
            });
        }
    });

    calendar.render();
});
