
function RenderLineChart(model, chartName, elementId) {
    var ctx = document.getElementById(elementId).getContext('2d');

    var xAxis = [];
    var yAxis = [];

    console.log(model);
    $.each(model.data_Axis_X, function (index, item) {
        xAxis.push(item.toString());
    });
    $.each(model.data_Axis_Y, function (index, item) {
        let num = Number(item.toString());
        yAxis.push(num);
    });

    var myChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: xAxis,
            datasets: [{
                label: chartName,
                data: yAxis,
                borderColor: 'rgb(255, 99, 132)',
                borderWidth: 1
            }]
        },
        options: {
            fill: false,
            pointBackgroundColor: 'rgb(224, 198, 112,)',
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        }
    });
}




