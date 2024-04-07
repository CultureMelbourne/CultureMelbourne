// To Do -
// Replace 'api/getCulturalDates' with the actual API endpoint that serves cultural date data.
// Replace 'api/getEventsFromEventbrite' with the actual API endpoint that serves Eventbrite data.
document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');
    var lastHighlighted;


    var culture = document.getElementById('culture').getAttribute('data-culture');
    console.log(culture);

    var calendar = new FullCalendar.Calendar(calendarEl, {
        //themeSystem: 'bootstrap', //Anything for FontAwesome?
        headerToolbar: {
            left: 'prevYear,prev,next,nextYear today',
            center: 'title',
            right: 'dayGridMonth,dayGridweek,dayGridDay'
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
                url: getEventsFromEventbriteUrl,
                type: 'GET',
                data: {
                    culture: culture,
                    date: clickedDate
                },
                success: function (events) {
                    var eventsHeading = '<h3 style="text-decoration: underline;">Events for the date: ' + clickedDate + '</h3>';
                    var eventsHtml = events.map(function (event) {
                        return '<div class="event-detail">' +
                            '<h3>' + event.title + '</h3>' +
                            '<p>' + event.description + '</p>' +
                            '<p style="color:blue;"><a href="' + event.url + '" target="_blank">More Info</a></p>' +
                            '</div>';
                    }).join('');
                    $('#events-list').html(eventsHeading + eventsHtml);
                }
            });
        }
    });

    calendar.render();
});
