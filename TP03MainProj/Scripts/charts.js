function LoadAgeDistributes(countryName, data) {
    var ageDistributions = data.ageDistributions;
    var decadePopulation = {};
    var ageGroups = [
        "0-4", "5-9", "10-14", "15-19", "20-24", "25-29", "30-34", "35-39",
        "40-44", "45-49", "50-54", "55-59", "60-64", "65-69", "70-74", "75-79",
        "80-84", "85-89", "90-94", "95-99", "100-104", "105-109", "110-114"
    ];

    var colors = [
        "#FFD700", "#FF8C00", "#FF6347", "#FF4500", "#FF1493", "#FF00FF",
        "#DB7093", "#B22222", "#8B0000", "#8B008B", "#800080", "#4B0082",
        "#2F4F4F", "#00FFFF", "#00FF7F", "#008000", "#006400", "#0000FF",
        "#000080", "#00008B", "#0000CD", "#4682B4", "#1E90FF"
    ];

    ageDistributions.forEach(function (distribution) {
        var censusYear = distribution.CensusYear;
        var decade = Math.floor(censusYear / 10) * 10;
        if (!decadePopulation[decade]) {
            decadePopulation[decade] = { totalPopulation: 0, ageGroups: {} };
            ageGroups.forEach(function (age) {
                decadePopulation[decade].ageGroups[age] = 0;
            });
        }

        ageGroups.forEach(function (age, index) {
            var agePopulation = distribution.AgeDistributions[index];
            decadePopulation[decade].ageGroups[age] += agePopulation;
            decadePopulation[decade].totalPopulation += agePopulation;
        });
    });

    var dataSeries = ageGroups.map(function (age, index) {
        return {
            type: "stackedColumn",
            name: age,
            showInLegend: true,
            color: colors[index % colors.length],
            dataPoints: []
        };
    });

    Object.keys(decadePopulation).sort((a, b) => a - b).map(Number).forEach(function (decade) {
        dataSeries.forEach(function (series) {
            series.dataPoints.push({
                x: decade,
                y: decadePopulation[decade].ageGroups[series.name],
                label: decade.toString()
            });
        });
    });

    var options = {
        animationEnabled: true,
        title: {
            text: "Population Distribution by Age in " + countryName
        },
        axisX: {
            title: "Decade",
            interval: 10,
            valueFormatString: "####"
        },
        axisY: {
            title: "Population"
        },
        legend: {
            cursor: "pointer",
            itemclick: function (e) {
                var active = e.dataSeries.visible;
                e.chart.options.data.forEach(function (data) {
                    data.visible = false; // initially turn everything off
                });
                e.dataSeries.visible = !active; // toggle the clicked series
                e.chart.render();
            }
        },
        data: dataSeries
    };

    var chartContainer = document.getElementById(countryName + "-age");
    var chart = new CanvasJS.Chart(chartContainer, options);
    chart.render();
}



function LoadOccupationData(countryName, data) {
    var occupationData = data.occupation;
    var years = {};
    var occupationCategories = new Set();

    // Organize data by year and collect all occupations
    occupationData.forEach(function (item) {
        if (!years[item.Year]) {
            years[item.Year] = [];
        }
        years[item.Year].push({
            label: item.Occupation,
            y: item.Total
        });
        occupationCategories.add(item.Occupation);
    });

    // Prepare the data series for the chart
    var chartData = [];
    occupationCategories.forEach(function (occupation) {
        var dataPoints = [];
        Object.keys(years).forEach(function (year) {
            var yearData = years[year].find(o => o.label === occupation);
            if (yearData && yearData.y > 0) { // Only add data points if y > 0
                dataPoints.push({
                    label: year,
                    y: yearData.y
                });
            }
        });
        chartData.push({
            type: "column",
            name: occupation,
            showInLegend: true,
            dataPoints: dataPoints
        });
    });

    // Create the chart
    var chart = new CanvasJS.Chart(countryName + "-occupa", {
        animationEnabled: true,
        theme: "light2",
        title: {
            text: "Occupational Distribution Over Years in " + countryName
        },
        axisX: {
            title: "Year",
            interval: 1,
            labelAngle: -45
        },
        axisY: {
            title: "Total Individuals",
            labelFormatter: function (e) {
                return CanvasJS.formatNumber(e.value, "#,##0");
            }
        },
        toolTip: {
            shared: true,
            contentFormatter: function (e) {
                var content = "Year: " + e.entries[0].dataPoint.label + "<br/>";
                e.entries.forEach(function (entry) {
                    content += entry.dataSeries.name + ": " + entry.dataPoint.y + "<br/>";
                });
                return content;
            }
        },
        legend: {
            cursor: "pointer",
            itemclick: function (e) {
                if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
                    e.dataSeries.visible = false;
                } else {
                    e.dataSeries.visible = true;
                }
                e.chart.render();
            }
        },
        data: chartData
    });

    chart.render();
}
