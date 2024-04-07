// To Do -
// Replace 'api/getCulturalDates' with the actual API endpoint that serves cultural date data.
// Replace 'api/getEventsFromEventbrite' with the actual API endpoint that serves Eventbrite data.
document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');

    //var culture = '@ViewBag.Culture'; // For retreiving the culture name from the server-side ViewBag


    var culture = document.getElementById('culture').getAttribute('data-culture');
    console.log(culture);

    var calendar = new FullCalendar.Calendar(calendarEl, {
        headerToolbar: {
            left: 'prevYear,prev,next,nextYear today',
            center: 'title',
            right: 'dayGridMonth,dayGridweek,dayGridDay'
        },
        initialDate: '2024-01-01',
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
            $.ajax({
                url: getEventsFromEventbriteUrl,
                type: 'GET',
                data: {
                    culture: culture,
                    date: clickedDate
                },
                success: function (events) {
                    var eventsHtml = events.map(function (event) {
                        return '<div class="event-detail">' +
                            '<h3>' + event.title + '</h3>' +
                            '<p>' + event.description + '</p>' +
                            '<p><a href="' + event.url + '" target="_blank">More Info</a></p>' +
                            '</div>';
                    }).join('');
                    $('#events-list').html(eventsHtml);
                }
            });
        }
        //dayClick: function (date, allDay, jsEvent, view) {
        //    var clickedDate = date.toISOString();
        //    $.ajax({
        //        url: getEventsFromEventbriteUrl,
        //        type: 'GET',
        //        data: {
        //            culture: culture,
        //            date: clickedDate
        //        },
        //        success: function (events) {
        //            // Assuming 'events' is an array of strings
        //            var eventsHtml = events.map(function (event) {
        //                return '<li>' + event + '</li>';
        //            }).join('');
        //            $('#events-list').html('<ul>' + eventsHtml + '</ul>');
        //        }
        //    });
        //}
    });

    calendar.render();
});
