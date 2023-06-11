// Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.global.defaultFontFamily = 'Nunito', '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#858796';


var ctx = document.getelementbyid("mypiechart");
var mypiechart = new chart(ctx, {
    type: 'doughnut',
    data: {
        labels: ["direct", "referral", "social"],
        datasets: [{
            data: [55, 30, 15],
            backgroundcolor: ['#4e73df', '#1cc88a', '#36b9cc'],
            hoverbackgroundcolor: ['#2e59d9', '#17a673', '#2c9faf'],
            hoverbordercolor: "rgba(234, 236, 244, 1)",
        }],
    },
    options: {
        maintainaspectratio: false,
        tooltips: {
            backgroundcolor: "rgb(255,255,255)",
            bodyfontcolor: "#858796",
            bordercolor: '#dddfeb',
            borderwidth: 1,
            xpadding: 15,
            ypadding: 15,
            displaycolors: false,
            caretpadding: 10,
        },
        legend: {
            display: false
        },
        cutoutpercentage: 80,
    },
});



//Linear Chart 
$.ajax({
    url: "/manage/dashboard/getLinearChartDatas",
    type: 'GET',
    success: function (res) {
        console.log(res)
        var ctx = document.getElementById("chartLinePurple");
        var chartLinePurple = new Chart(ctx, {
            type: 'line',
            data: {
                labels: res.labels,
                datasets: [{
                    label: "Earnings",
                    lineTension: 0.3,
                    backgroundColor: "rgba(78, 115, 223, 0.05)",
                    borderColor: "rgba(78, 115, 223, 1)",
                    pointRadius: 3,
                    pointBackgroundColor: "rgba(78, 115, 223, 1)",
                    pointBorderColor: "rgba(78, 115, 223, 1)",
                    pointHoverRadius: 3,
                    pointHoverBackgroundColor: "rgba(78, 115, 223, 1)",
                    pointHoverBorderColor: "rgba(78, 115, 223, 1)",
                    pointHitRadius: 10,
                    pointBorderWidth: 2,
                    data: [0, 10000, 5000],
                }],
            },
            options: {
                maintainAspectRatio: false,
                layout: {
                    padding: {
                        left: 10,
                        right: 25,
                        top: 25,
                        bottom: 0
                    }
                },
                scales: {
                    xAxes: [{
                        time: {
                            unit: 'date'
                        },
                        gridLines: {
                            display: false,
                            drawBorder: false
                        },
                        ticks: {
                            maxTicksLimit: 7
                        }
                    }],
                    yAxes: [{
                        ticks: {
                            maxTicksLimit: 5,
                            padding: 10,
                            // Include a dollar sign in the ticks
                            callback: function (value, index, values) {
                                return '$' + number_format(value);
                            }
                        },
                        gridLines: {
                            color: "rgb(234, 236, 244)",
                            zeroLineColor: "rgb(234, 236, 244)",
                            drawBorder: false,
                            borderDash: [2],
                            zeroLineBorderDash: [2]
                        }
                    }],
                },
                legend: {
                    display: false
                },
                tooltips: {
                    backgroundColor: "rgb(255,255,255)",
                    bodyFontColor: "#858796",
                    titleMarginBottom: 10,
                    titleFontColor: '#6e707e',
                    titleFontSize: 14,
                    borderColor: '#dddfeb',
                    borderWidth: 1,
                    xPadding: 15,
                    yPadding: 15,
                    displayColors: false,
                    intersect: false,
                    mode: 'index',
                    caretPadding: 10,
                    callbacks: {
                        label: function (tooltipItem, chart) {
                            var datasetLabel = chart.datasets[tooltipItem.datasetIndex].label || '';
                            return datasetLabel + ': $' + number_format(tooltipItem.yLabel);
                        }
                    }
                }
            }
        });

    }
});

