function loadReligionChart(countryName, religionReports) {
    if (!religionReports || !religionReports.length || !religionReports[0].data) {
        console.warn('No religion data available for', countryName);
        return;
    }

    var religionData = religionReports[0].data;
    var chartData = [{
        values: religionData.map(item => item.Total),
        text: "Total",
        backgroundColor: '#007BFF'
    }];
    var title = countryName.charAt(0).toUpperCase() + countryName.slice(1);

    var config = {
        type: 'bar',
        title: {
            text: title + ' Religious Distribution',
            fontSize: 18,
            adjustLayout: true
        },
        scaleX: {
            labels: religionData.map(item => item.Religion_Name),
            item: { angle: -15 }
        },
        scaleY: {
            'min-value': 0,
            'max-value': Math.max(...religionData.map(item => item.Total)) * 1.1,
            'step': Math.max(...religionData.map(item => item.Total)) / 10
        },
        plot: {
            animation: { effect: 3, method: 5, sequence: 1, speed: 800 },
            tooltip: { text: '%t: %v' }
        },
        series: chartData
    };

    var chartContainerId = countryName.toLowerCase() + '-religion-chart';

    zingchart.render({
        id: chartContainerId,
        data: config,
        height: '500px',
        width: '100%'
    });

    window.addEventListener('resize', function () {
        zingchart.exec(chartContainerId, 'resize', {
            width: document.getElementById(chartContainerId).clientWidth,
            height: document.getElementById(chartContainerId).clientHeight || 500
        });
    });
}
function loadOccupationChart(countryName, occupationReports) {
    if (!occupationReports || !occupationReports.length || !occupationReports[0].data) {
        console.warn('No occupation data available for', countryName);
        return;
    }

    var occupationData = occupationReports[0].data;
    var chartData = occupationData.map(function (item) {
        return { values: [item.Total], text: item.Occupation };
    });
    var title = countryName.charAt(0).toUpperCase() + countryName.slice(1);

    var config = {
        type: 'pie',
        title: {
            text: title + ' Occupation Distribution',
            fontSize: 18,
            adjustLayout: true
        },
        plot: {
            valueBox: [
                {
                    text: '%t\n%npv%',
                    placement: 'out',
                    fontColor: '#333',
                    fontSize: '12px',
                    fontWeight: 'bold'
                }
            ],
            tooltip: { text: '%t: %v (%npv%)' },
            animation: { effect: 3, method: 5, sequence: 1, speed: 800 }
        },
        series: chartData
    };

    var chartContainerId = countryName.toLowerCase() + '-occupation-chart';

    zingchart.render({
        id: chartContainerId,
        data: config,
        height: '500px',
        width: '100%'
    });

    window.addEventListener('resize', function () {
        zingchart.exec(chartContainerId, 'resize', {
            width: document.getElementById(chartContainerId).clientWidth,
            height: document.getElementById(chartContainerId).clientHeight || 500
        });
    });
}


function loadPopulation(countryName, populations) {
    // Dynamically generate the chart container ID based on the country name.
    var chartContainerId = countryName.toLowerCase() + '-population-chart';
    var chartContainer = $('#' + chartContainerId);

    // Configuration for the chart with animation options
    var title = countryName;
    title = title.charAt(0).toUpperCase() + title.slice(1);
    var myConfig = {
        type: 'stream',  // Chart type as line graph.
        title: {
            text: title + ' Population Change Over Years',  // Dynamic title.
            fontSize: 24,
        },
        scaleX: {
            label: { text: 'Year' },
            // Map CensusYear to scale labels.
            values: populations.map(item => item.CensusYear)
        },
        scaleY: {
            label: { text: 'Total Population' }
        },
        series: [{
            values: populations.map(item => item.Total_Population)
        }],
        plot: {
            animation: {
                effect: 'ANIMATION_SLIDE_LEFT',  // Animation effect to slide in from the left
                sequence: 1,  // Start animation sequence
                speed: 800  // Speed of the animation in milliseconds
            }
        }
    };

    // Check if the chart has already been rendered.
    if (zingchart.exec(chartContainerId, 'getdata')) {
        // Update the existing chart with new data.
        zingchart.exec(chartContainerId, 'setseriesvalues', {
            graphid: 0,
            all: true,
            values: [populations.map(item => item.Total_Population)]
        });
    } else {
        // Render the chart for the first time with animation.
        zingchart.render({
            id: chartContainerId,
            data: myConfig,
            height: '500px',
            width: '100%'
        });
    }
}
