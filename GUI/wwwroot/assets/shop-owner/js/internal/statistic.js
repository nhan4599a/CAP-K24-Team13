const currentYear = new Date().getFullYear();
let currentPageInfo = getCurrentPageInfo();

const startDatePicker = MCDatepicker.create({
    dateFormat: 'dd/mm/yyyy',
    selectedDate: getDateObj(currentPageInfo.start) || new Date()
});

const endDatePicker = MCDatepicker.create({
    dateFormat: 'dd/mm/yyyy',
    selectedDate: getDateObj(currentPageInfo.end) || new Date()
});

$(document).ready(() => {
    $('#input-search').parent().remove();
    $('#statistic-strategy').change(function () {
        let strategy = $(this).val();
        $('.row .col-12 .row.justify-content-end:nth-child(2)').remove();
        if (strategy == 0) {
            $(this).parent().parent().after(`<div class="row justify-content-end mb-2">
                                                <div class="col-md-2">
                                                    <div class="form-outline">
                                                        <input type="text" id="input-start" class="form-control" placeholder="Start date" onfocus="openStartDatePicker();" />
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-outline">
                                                        <input type="text" id="input-end" class="form-control" placeholder="End date" onfocus="openEndDatePicker();" />
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <button class="btn" style="visibility: hidden">placeholder</button>
                                                </div>
                                            </div>`);
            $('#input-start').val(getDateObj(currentPageInfo.start) ? currentPageInfo.start : '');
            $('#input-end').val(getDateObj(currentPageInfo.end) ? currentPageInfo.end : '');
        } else if (strategy == 1) {
            $(this).parent().parent().after(`<div class="row justify-content-end mb-2">
                                                <div class="col-md-2">
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
                                                    <input type="text" id="input-start-year" class="form-control" placeholder="Start year" />
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
                                                    <input type="text" id="input-end-year" class="form-control" placeholder="End year" />
                                                </div>
                                                <div class="col-md-2">
                                                    <button class="btn" style="visibility: hidden">placeholder</button>
                                                </div>
                                            </div>`);
            $('#select-start-month').val(currentPageInfo.start ? (currentPageInfo.start.split('/')[0] || 0) : 0);
            $('#select-end-month').val(currentPageInfo.end ? (currentPageInfo.end.split('/')[0] || 0) : 0);
            $('#input-start-year').val(currentPageInfo.start ? (currentPageInfo.start.split('/')[1] || currentYear) : currentYear);
            $('#input-end-year').val(currentPageInfo.end ? (currentPageInfo.end.split('/')[1] || currentYear) : currentYear);
        } else if (strategy == 2) {
            $(this).parent().parent().after(`<div class="row justify-content-end mb-2">
                                                <div class="col-md-2">
                                                    <select class="form-select" id="select-start-quarter">
                                                        <option value="1">1</option>
                                                        <option value="2">2</option>
                                                        <option value="3">3</option>
                                                        <option value="4">4</option>
                                                    </select>
                                                </div>
                                                <div class="col-md-2">
                                                    <input type="text" id="input-start-year" class="form-control" placeholder="Start year" />
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
                                                    <input type="text" id="input-end-year" class="form-control" placeholder="End year" />
                                                </div>
                                                <div class="col-md-2">
                                                    <button class="btn" style="visibility: hidden">placeholder</button>
                                                </div>
                                            </div>`);
            $('#select-start-month').val(currentPageInfo.start ? (currentPageInfo.start.split('/')[0] || 0) : 0);
            $('#select-end-month').val(currentPageInfo.end ? (currentPageInfo.end.split('/')[0] || 0) : 0);
            $('#input-start-year').val(currentPageInfo.start ? (currentPageInfo.start.split('/')[1] || currentYear) : currentYear);
            $('#input-end-year').val(currentPageInfo.end ? (currentPageInfo.end.split('/')[1] || currentYear) : currentYear);
        } else if (strategy == 3) {
            $(this).parent().parent().after(`<div class="row justify-content-end mb-2">
                                                <div class="col-md-2">
                                                    <input type="text" id="input-start-year" class="form-control" value="${currentYear}" placeholder="Start year" />
                                                </div>
                                                <div class="col-md-2">
                                                    <input type="text" id="input-end-year" class="form-control" value="${currentYear}" placeholder="End year" />
                                                </div>
                                                <div class="col-md-2">
                                                    <button class="btn" style="visibility: hidden">placeholder</button>
                                                </div>
                                            </div>`);
            $('#input-start-year').val(currentPageInfo.start || currentYear);
            $('#input-end-year').val(currentPageInfo.end || currentYear);
        }
        $('#input-start-year').yearpicker({
            year: currentYear
        });
        $('#input-end-year').yearpicker({
            year: currentYear
        });
    });
    if (!isWaitingForInput()) {
        let animationLoader = new AnimationLoader('#loading-container > #animation-container', '/assets/shop-owner/img/illustrations/loading.json');
        animationLoader.showAnimation();
        getShopId()
            .then(shopId => {
                getStatisticOfShop(shopId, currentPageInfo.strategy, currentPageInfo.start, currentPageInfo.end)
                    .then(statisticResult => {
                        animationLoader.hideAnimation();
                        const context = document.getElementById('statistic-chart').getContext('2d');
                        let chart = null;
                        chart = switchChartViewMode(chart, context, currentPageInfo.viewMode, statisticResult.details);
                        $(`#statistic-strategy > option:eq(${currentPageInfo.strategy})`).prop('selected', true).parent().change();
                        $(`#statistic-view-mode > option:eq(${currentPageInfo.viewMode})`).prop('selected', true);
                        $('#statistic-view-mode').change(function () {
                            let viewMode = $(this).val();
                            currentPageInfo.viewMode = viewMode;
                            window.history.pushState({}, null,
                                `/shopowner/statistic?strategy=${currentPageInfo.strategy}&view=${viewMode}&start=${currentPageInfo.start}&end=${currentPageInfo.end}`);
                            chart = switchChartViewMode(chart, context, viewMode, statisticResult.details);
                        });
                    })
                    .catch(error => {
                        animationLoader.hideAnimation();
                        toastr.error(error, 'Error');
                    });
            });
    }
    else
        $('#statistic-strategy').change();
    $('#view-statistic').click(() => {
        let strategy = $('#statistic-strategy').val();
        let start = '';
        let end = '';
        if (strategy == 0) {
            start = $('#input-start').val();
            end = $('#input-end').val();
        } else if (strategy == 1) {
            start = $('#select-start-month').val() + '/' + $('#input-start-year').val();
            end = $('#select-end-month').val() + '/' + $('#input-end-year').val();
        } else if (strategy == 2) {
            start = $('#select-start-quarter').val() + '/' + $('#input-start-year').val();
            end = $('#select-end-quarter').val() + '/' + $('#input-end-year').val();
        } else {
            start = $('#input-start-year').val();
            end = $('#input-end-year').val();
        }
        if (!start || !end) {
            toastr.error('All fields are required!', 'Error');
            return;
        }
        let view = $('#statistic-view-mode').val();
        window.location.href = `/shopowner/statistic?strategy=${strategy}&start=${start}&end=${end}&view=${view}`;
    });
});

function switchChartViewMode(chart, context, viewMode, data) {
    if (viewMode != 0 && viewMode != 1)
        return;
    let result = Object.entries(data)
        .map(keyValuePair => ({
            date: keyValuePair[0],
            income: !keyValuePair[1] ? 0 : keyValuePair[1].income,
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
                        borderColor: 'rgb(66, 115, 121)',
                        backgroundColor: 'rgb(75, 192, 192)'
                    },
                    {
                        label: 'Succeed Income',
                        data: result.map(obj => obj.succeed),
                        borderColor: 'rgba(181, 224, 90, 1)',
                        backgroundColor: 'rgba(153, 191, 74, 1)'
                    },
                    {
                        label: 'Canceled Income',
                        data: result.map(obj => obj.canceled),
                        borderColor: 'rgb(255, 99, 132)',
                        backgroundColor: 'rgb(224, 155, 170)'

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
    let currentStartDate = queryObj.get('start');
    let currentEndDate = queryObj.get('end');
    return {
        viewMode: currentViewMode,
        strategy: currentStrategy,
        start: currentStartDate,
        end: currentEndDate
    }
}

function isWaitingForInput() {
    let url = new URL(window.location.href);
    let queryObj = url.searchParams;
    return !(queryObj.get('strategy') && queryObj.get('start') && queryObj.get('end'));
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

function getDateObj(str) {
    if (typeof(str) != 'string')
        return null;
    if (/\d[1-9]\/\d[1-9]\/\d{4}/.test(str)) {
        let parts = str.split('/');
        if (parts.length != 3)
            return null;
        let day = parseInt(parts[0]);
        let month = parseInt(parts[1]);
        let year = parseInt(parts[2]);
        if (month < 1 || month > 12)
            return null;
        if (day < 1 || day > getDayNumberOfMonth(month, year))
            return null;
        return new Date(year, month - 1, day);
    }
    return null;
}

function isLeapYear(year) {
    return (year % 100 === 0) ? (year % 400 === 0) : (year % 4 === 0);
}

function getDayNumberOfMonth(month, year) {
    return (month === 2) ? (isLeapYear(year) ? 29 : 31) :
        (month <= 7 ? (month % 2 === 1 ? 31 : 30) : (month % 2 === 1 ? 30 : 31));
}
