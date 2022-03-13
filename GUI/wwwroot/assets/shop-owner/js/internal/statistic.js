$(document).ready(() => {
    let animationLoader = new AnimationLoader('#loading-container > #animation-container', '/assets/shop-owner/img/illustrations/loading.json');
    animationLoader.showAnimation();
    getShopId()
        .then(shopId => {
            let currentPageInfo = getCurrentPageInfo();
            getStatisticOfShop(shopId, currentPageInfo.strategy)
                .then(statisticResult => {
                    animationLoader.hideAnimation();
                    const context = document.getElementById('statistic-chart').getContext('2d');
                    let chart = null;
                    chart = switchChartViewMode(chart, context, currentPageInfo.viewMode, statisticResult.details);
                    $(`#statistic-strategy > option:eq(${currentPageInfo.strategy})`).prop('selected', true);
                    $(`#statistic-view-mode > option:eq(${currentPageInfo.viewMode})`).prop('selected', true);
                    $('#statistic-view-mode').change(function () {
                        let viewMode = $(this).val();
                        currentPageInfo.viewMode = viewMode;
                        window.history.pushState({}, null, `statistic?view=${viewMode}&strategy=${currentPageInfo.strategy}`);
                        chart = switchChartViewMode(chart, context, viewMode, statisticResult.details);
                    });
                    $('#statistic-strategy').change(function () {
                        let strategy = $(this).val();
                        window.location.href = `statistic?view=${currentPageInfo.viewMode}&strategy=${strategy}`;
                    });
                })
                .catch(error => {
                    animationLoader.hideAnimation();
                    toastr.error(error, 'Error!');
                });
        });
});

function switchChartViewMode(chart, context, viewMode, data) {
    if (viewMode != 0 && viewMode != 1)
        return;
    let result = Object.entries(data)
        .map(keyValuePair => ({
            date: keyValuePair[0],
            income: keyValuePair[1].income,
            total: !keyValuePair[1].data ? 0 : keyValuePair[1].data.total,
            new: !keyValuePair[1].data ? 0 : keyValuePair[1].data.new,
            succeed: !keyValuePair[1].data ? 0 : keyValuePair[1].data.succeed,
            canceled: !keyValuePair[1].data ? 0 : keyValuePair[1].data.canceled
        }));
    if (chart)
        chart.destroy();
    if (viewMode == 0) {
        chart = new Chart(context, {
            type: 'line',
            data: {
                labels: result.map(obj => obj.date),
                datasets: [
                    {
                        label: 'Actual Income',
                        data: result.map(obj => obj.income),
                        borderColor: 'rgb(75, 192, 192)',
                        backgroundColor: 'rgb(176, 212, 212)'
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
                },
                scales: {
                    y: {
                        title: {
                            display: true,
                            text: 'Income (VND)'
                        }
                    }
                }
            }
        });
    } else if (viewMode == 1) {
        chart = new Chart(context, {
            type: 'bar',
            data: {
                labels: result.map(obj => obj.date),
                datasets: [
                    {
                        label: 'New orders',
                        data: result.map(obj => obj.new),
                        borderColor: 'rgb(75, 192, 192)',
                        backgroundColor: 'rgb(176, 212, 212)'
                    },
                    {
                        label: 'Succeed Income',
                        data: result.map(obj => obj.succeed),
                        borderColor: 'rgb(255, 99, 132)',
                        backgroundColor: 'rgb(224, 155, 170)'
                    },
                    {
                        label: 'Canceled Income',
                        data: result.map(obj => obj.canceled)
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
                },
                scales: {
                    y: {
                        title: {
                            display: true,
                            text: '# of orders'
                        }
                    }
                }
            }
        });
    }
    return chart;
}

function getCurrentPageInfo() {
    let url = new URL(window.location.href);
    let queryObj = url.searchParams;
    let currentViewMode = queryObj.get('view') || 0;
    let currentStrategy = queryObj.get('strategy') || 0;
    return {
        viewMode: currentViewMode,
        strategy: currentStrategy
    }
}