
$(document).ready(function () {
    var events = [];
    $(".events").each(function () {
        var title = $(".title", this).text().trim();
        var description = $(".description", this).text().trim();
        var starttime = $(".starttime", this).text().trim();
        var endtime = $(".endtime", this).text().trim();
        var event = {
            "title": title,  // The event's title
            "description": description, // The event's title
            "starttime": starttime,   // The event's start time
            "endtime": endtime, // The event's end time
        };
        events.push(event);  // Add the event object to the events array
    });

    $("#calendar").fullCalendar({
        locale: 'au',  // Set locale to Australian (for example)
        events: events,  // Pass the events array to the calendar

        //Modify according to the event configuration (the url section to page section)
        // Define a function for the 'dayClick' event
        dayClick: function (date, allDay, jsEvent, view) {
            var d = new Date(date);
            var m = moment(d).format("YYYY-MM-DD");  // Format the date
            m = encodeURIComponent(m);  // Encode the date
            var uri = "/Events/Create?date=" + m;  // Construct the URI with the query string
            $(location).attr('href', uri);  // Redirect to the event creation page
        }
    });
});
