function createChart(countryName, containerId, dataList) {
    var countryData = dataList.filter(d => d.CountryName === countryName);
    var dataPoints = countryData.map(d => {
        var totalPopulation = d.AgeDistributions.reduce((a, b) => a + b, 0);
        return { label: d.CensusYear.toString(), y: totalPopulation };
    });

    var chart = new CanvasJS.Chart(containerId, {
        theme: "light2",
        animationEnabled: true,
        title: {
            text: "Population distribution - " + countryName
        },
        axisY: {
            title: "Population (million)"
        },
        data: [{
            type: "column",
            dataPoints: dataPoints
        }],
    });

    chart.render();

    handleResize(chart);
}

function handleResize(chart) {
    window.addEventListener('resize', function () {
        chart.render();
    });
}