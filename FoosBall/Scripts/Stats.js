jQuery(window).load(function () {

    /* ******************************************************************
     * Stats / Player View
     */
    //var playerId = 

    $("#load-player-rating-chart").on('click', function (e) {
        e.preventDefault();
        var that = this;

        var script = document.createElement('script');
        script.type = 'text/javascript'; 
        script.async = true;
        script.src = '/Scripts/highcharts.js';
        script.onload = getChartData(this);
        
        script.onreadystatechange = function () {
            if (this.readyState == 'complete') {
                getChartData(that);
            }
        };
        
        var s = document.getElementsByTagName('script')[0]; 
        s.parentNode.insertBefore(script, s);
    });
});

function getChartData(that) {
    var subjectId = $(that).attr("data-subject-id");
    var subjectName = $(that).attr("data-subject-name");
    var chartData = { DataPoints: [] };

    $.ajax({
        type: "get",
        cache: false,
        url: "/Stats/GetPlayerRatingData",
        data: { playerId: subjectId },
        success: function (data) {
            $.each(data.DataPoints, function (key, value) {
                chartData.DataPoints.push([
                    Date.UTC(
                        value.TimeSet[0], // year
                        parseInt(value.TimeSet[1]) - 1, // month (0-based)
                        value.TimeSet[2], // day
                        value.TimeSet[3], // hour
                        value.TimeSet[4],  // minute
                        value.TimeSet[5]  // second
                    ),
                    parseFloat(value.Rating.toPrecision(6))
                ]);
            });

            chartData.MinimumValue = data.MinimumValue - 50;
            chartData.MaximumValue = data.MaximumValue + 50;
            chartData.PlayerName = subjectName;
            renderChart(chartData);
        }
    });
}


function renderChart(chartData) {
    var chart;

    chart = new Highcharts.Chart({
        chart: {
            renderTo: 'player-rating-chart',
            zoomType: 'x',
            spacingRight: 10
        },
        title: {
            text: chartData.PlayerName + 's rating over time'
        },
        subtitle: {
            text: document.ontouchstart === undefined
                    ? 'Click and drag in the plot area to zoom in'
                    : 'Drag your finger over the plot to zoom in'
        },
        xAxis: {
            type: 'datetime',
            maxZoom: 14 * 24 * 36000, // hours
            title: {
                text: null
            }
        },
        yAxis: {
            title: {
                text: null
            },
            min: chartData.MinimumValue,
            max: chartData.MaximumValue,
            startOnTick: false,
            showFirstLabel: false
        },
        tooltip: {
            shared: true
        },
        legend: {
            enabled: false
        },
        plotOptions: {
            area: {
                fillColor: {
                    linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                    stops: [
                        [0, Highcharts.getOptions().colors[0]],
                        [1, 'rgba(2,0,0,0)']
                    ]
                },
                lineWidth: 1,
                marker: {
                    enabled: false,
                    states: {
                        hover: {
                            enabled: true,
                            radius: 5
                        }
                    }
                },
                shadow: false,
                states: {
                    hover: {
                        lineWidth: 1
                    }
                }
            }
        },

        series: [{
            type: 'line',
            name: 'Rating',
            pointStart: Date.UTC(2012, 09, 28, 01, 01, 01),
            //data: [[Date.UTC(2012, 09, 28, 05), 2], [Date.UTC(2012, 09, 28, 12), 4], [Date.UTC(2012, 09, 28, 18), 6]]
            data: chartData.DataPoints
        }]
    });
}