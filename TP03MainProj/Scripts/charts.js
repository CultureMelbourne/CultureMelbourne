// charts.js

function initializeChart(country, data) {
    var chartContainerId = country.toLowerCase() + "-chart";
    var chartContainer = $("#" + chartContainerId);

    var chart = new CanvasJS.Chart(chartContainerId, {
        // Chart options
        title: {
            text: country + " Data Chart"
        },
        data: [{
            type: "column",
            dataPoints: [
                { label: "Age Distribution", y: data["age_distribution"] },
                { label: "Occupation", y: data["occupation"] },
                { label: "Population", y: data["population"] },
                { label: "Religion", y: data["religion"] }
            ]
        }]
    });

    chart.render();
}
