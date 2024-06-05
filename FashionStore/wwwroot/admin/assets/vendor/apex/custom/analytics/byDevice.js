var options = {
    chart: {
        height: 310,
        type: 'donut',
    },
    labels: [],
    series: [],
    legend: {
        position: 'bottom',
    },
    dataLabels: {
        enabled: false
    },
    stroke: {
        width: 8,
        colors: ['#ffffff'],
    },
    colors: ['#435EEF', '#59a2fb', '#8ec0fd'],
    tooltip: {
        y: {
            formatter: function (val) {
                return "$" + val
            }
        }
    },
}
