$(document).ready(() => {
    const chart = null;
    let animationLoader = new AnimationLoader('#loading-container > #animation-container', '/assets/shop-owner/img/illustrations/loading.json');
    animationLoader.showAnimation();
    getShopId()
        .then(shopId => {
            getStatisticOfShop(shopId, 'ByDay')
                .then(statisticResult => {
                    animationLoader.hideAnimation();
                    const context = document.getElementById('statistic-chart').getContext('2d');
                    const data = Object.entries(statisticResult)
                        .map(keyValuePair => {
                            return {
                                date: keyValuePair[0],
                                actualIncome: keyValuePair[1].actualIncome,
                                estimatedIncome: keyValuePair[1].estimatedIncome
                            };
                        });
                    chart = new Chart(context, {
                        type: 'line',
                        data: {
                            labels: data.map(obj => obj.date),
                            datasets: [
                                {
                                    label: 'Actual Income',
                                    data: data.map(obj => obj.actualIncome)
                                },
                                {
                                    label: 'Estimated Income',
                                    data: data.map(obj => obj.estimatedIncome)
                                }
                            ]
                        },
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
                })
                .catch(error => {
                    animationLoader.hideAnimation();
                    toastr.error(error, 'Error!');
                });
        });
});