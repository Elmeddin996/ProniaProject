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



