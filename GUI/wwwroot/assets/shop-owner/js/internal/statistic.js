const startDatePicker = MCDatepicker.create({
    dateFormat: 'dd/mm/yyyy'
});

const endDatePicker = MCDatepicker.create({
    dateFormat: 'dd/mm/yyyy'
});

$(document).ready(() => {
    if (isWaitingForInput()) {
        $('#statistic-strategy').change(function () {
            let strategy = $(this).val();
            if (strategy == 0) {
                $(this).parent().after(`<div class="col-md-2">
                                            <div class="form-outline">
                                                <i class="far fa-calendar-alt trailing pe-auto" style="cursor: pointer" onclick="openStartDatePicker()"></i>
                                                <input type="text" id="input-start" class="form-control form-icon-trailing" placeholder="Start date" />
                                                <label class="form-label" for="input-start">Start date/label>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-outline">
                                                <i class="far fa-calendar-alt trailing pe-auto" style="cursor: pointer" onclick="openEndDatePicker()"></i>
                                                <input type="text" id="input-end" class="form-control form-icon-trailing" placeholder="End date" />
                                                <label class="form-label" for="input-end">End date</label>
                                            </div>
                                        </div>`);
            } else if (strategy == 1) {
                $(this).parent().after(`<div class="col-md-2">
                                            <select class="form-select" id="select-start-month">
                                                <option value="1">January</option>
                                                <option value="2">February</option>
                                                <option value="3">March</option>
                                                <option value="4">April</option>
                                                <option value="5">May</option>
                                                <option value="6">June</option>
                                                <option value="7">July</option>
                                                <option value="8">August</option>
                                                <option value="9">September</option>
                                                <option value="10">October</option>
                                                <option value="11">November</option>
                                                <option value="12">December</option>
                                            </select>
                                        </div>
                                        <div class="col-md-2">
                                            <input type="text" id="input-start-year" class="form-control form-icon-trailing" placeholder="Start year" />
                                            <label class="form-label" for="input-start-year">Start year/label>
                                        </div>
                                        <div class="col-md-2">
                                            <select class="form-select" id="select-end-month">
                                                <option value="1">January</option>
                                                <option value="2">February</option>
                                                <option value="3">March</option>
                                                <option value="4">April</option>
                                                <option value="5">May</option>
                                                <option value="6">June</option>
                                                <option value="7">July</option>
                                                <option value="8">August</option>
                                                <option value="9">September</option>
                                                <option value="10">October</option>
                                                <option value="11">November</option>
                                                <option value="12">December</option>
                                            </select>
                                        </div>
                                        <div class="col-md-2">
                                            <input type="text" id="input-end-year" class="form-control form-icon-trailing" placeholder="End year" />
                                            <label class="form-label" for="input-end-year">End year/label>
                                        </div>`);
            } else if (strategy == 2) {
                $(this).parent().after(`<div class="col-md-2">
                                            <select class="form-select" id="select-start-quarter">
                                                <option value="1">1</option>
                                                <option value="2">2</option>
                                                <option value="3">3</option>
                                                <option value="4">4</option>
                                            </select>
                                        </div>
                                        <div class="col-md-2">
                                            <input type="text" id="input-start-year" class="form-control form-icon-trailing" placeholder="Start year" />
                                            <label class="form-label" for="input-start-year">Start year/label>
                                        </div>
                                        <div class="col-md-2">
                                            <select class="form-select" id="select-end-quarter">
                                                <option value="1">1</option>
                                                <option value="2">2</option>
                                                <option value="3">3</option>
                                                <option value="4">4</option>
                                            </select>
                                        </div>
                                        <div class="col-md-2">
                                            <input type="text" id="input-end-year" class="form-control form-icon-trailing" placeholder="End year" />
                                            <label class="form-label" for="input-end-year">End year/label>
                                        </div>`);
            } else if (strategy == 3) {
                $(this).parent().after(`<div class="col-md-2">
                                            <input type="text" id="input-start-year" class="form-control form-icon-trailing" placeholder="Start year" />
                                            <label class="form-label" for="input-start-year">Start year/label>
                                        </div>
                                        <div class="col-md-2">
                                            <input type="text" id="input-end-year" class="form-control form-icon-trailing" placeholder="End year" />
                                            <label class="form-label" for="input-end-year">End year/label>
                                        </div>`);
            }
            $('#input-start-year').yearpicker();
            $('#input-end-year').yearpicker();
        });
    }
    $('#view-statistic').click(() => {
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
});

function switchChartViewMode(chart, context, viewMode, data) {
    if (viewMode != 0 && viewMode != 1)
        return;
    let result = Object.entries(data)
        .map(keyValuePair => ({
            date: keyValuePair[0],
            income: !keyValuePair[1] ? 0 : keyValuePair[1].income,
            total: !keyValuePair[1] ? 0 : keyValuePair[1].data.total,
            new: !keyValuePair[1] ? 0 : keyValuePair[1].data.new,
            succeed: !keyValuePair[1] ? 0 : keyValuePair[1].data.succeed,
            canceled: !keyValuePair[1] ? 0 : keyValuePair[1].data.canceled
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

function isWaitingForInput() {
    let url = new URL(window.location.href);
    let queryObj = url.searchParams;
    return queryObj.get('strategy') && queryObj.get('start') && queryObj.get('end');
}

startDatePicker.onSelect((_, selectedDate) => {
    $('#input-start').val(selectedDate).addClass('active');
});

endDatePicker.onSelect((_, selectedDate) => {
    $('#input-end').val(selectedDate).addClass('active');
});

function openStartDatePicker() {
    startDatePicker.open();
}

function openEndDatePicker() {
    endDatePicker.open();
}