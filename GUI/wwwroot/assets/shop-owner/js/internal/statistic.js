$(document).ready(() => {
    
    const context = document.getElementById('statistic-chart').getContext('2d');
    const chart = new Chart(context, {
        type: 'line',
        data: data,
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'top'
                },
                title: {
                    display: true,
                    text: 'Orders statistic chart'
                }
            }
        }
    });
});