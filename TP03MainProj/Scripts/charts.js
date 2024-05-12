function loadReligionChart(countryName, religionReports) {
    // Ensure the religion data is available and valid
    if (!religionReports || !religionReports.length || !religionReports[0].data) {
        console.warn('No religion data available for', countryName);
        return;
    }

    // Using the first report's data if there are multiple
    var religionData = religionReports[0].data;
    console.log(religionData); // Log to verify the data

    // Prepare data for the pie chart
    var chartData = religionData.map(function (item) {
        return {
            values: [item.Total],
            text: item.Religion_Name
        };
    });
    var title = countryName;
    title = title.charAt(0).toUpperCase() + title.slice(1);
    // ZingChart configuration
    var config = {
        type: 'pie',
        title: {
            text: title + ' Religious Distribution',
            fontSize: 18,
            adjustLayout: true
        },
        plot: {
            valueBox: [
                {
                    text: '%t\n%npv%', // Shows the religion name and percentage
                    placement: 'out',
                    fontColor: '#333',
                    fontSize: '12px',
                    fontWeight: 'bold'
                }
            ],
            tooltip: {
                text: '%t: %v (%npv%)'
            },
            animation: {
                effect: 3,
                method: 5,
                sequence: 1,
                speed: 800
            }
        },
        series: chartData
    };

    // Render the chart
    zingchart.render({
        id: countryName.toLowerCase() + '-religion-chart', // Specific ID for the religion chart
        data: config,
        height: 400,
        width: '100%'
    });
}

function loadOccupationChart(countryName, occupationReports) {
    // Check if occupation data is available
    if (!occupationReports || !occupationReports.length || !occupationReports[0].data) {
        console.warn('No occupation data available for', countryName);
        return;
    }

    // Extracting the first report's data
    var occupationData = occupationReports[0].data;

    // Prepare data for the pie chart
    var chartData = occupationData.map(function (item) {
        return {
            values: [item.Total],
            text: item.Occupation
        };
    });
    var title = countryName;
    title = title.charAt(0).toUpperCase() + title.slice(1);
    // ZingChart configuration for pie chart
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
                    text: '%t\n%npv%', // Shows the text and percentage value of each segment
                    placement: 'out', // Place labels outside the chart
                    fontColor: '#333',
                    fontSize: '12px',
                    fontWeight: 'bold'
                }
            ],
            tooltip: {
                text: '%t: %v (%npv%)' // Detailed tooltip showing name, value, and percentage
            },
            animation: {
                effect: 3, // Specifies the animation effect
                method: 5, // Specifies the animation method
                sequence: 1, // Order of animation
                speed: 800 // Speed in milliseconds
            }
        },
        series: chartData
    };

    // Render the chart
    zingchart.render({
        id: countryName.toLowerCase() + '-occupation-chart', // Element ID to render the chart
        data: config,
        height: 400,
        width: '100%'
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
