document.addEventListener('DOMContentLoaded', function () {
    fetch('/api/admin/adminapi/chartdata')
        .then(response => {
            console.log('Fetching data...');
            return response.json();
        })
        .then(data => {
            console.log('Data fetched:', data);

            var options = {
                chart: {
                    height: 235,
                    type: 'bar',
                    stacked: true,
                    toolbar: {
                        show: false
                    },
                    zoom: {
                        enabled: true
                    }
                },
                plotOptions: {
                    bar: {
                        horizontal: false,
                    },
                },
                dataLabels: {
                    enabled: true
                },
                series: data.series.map(series => ({
                    name: series.name,
                    data: series.data
                })),
                xaxis: {
                    categories: data.categories,
                },
                legend: {
                    position: 'bottom',
                    offsetY: 0,
                },
                grid: {
                    borderColor: '#e0e6ed',
                    strokeDashArray: 5,
                    xaxis: {
                        lines: {
                            show: true
                        }
                    },
                    yaxis: {
                        lines: {
                            show: false,
                        }
                    },
                    padding: {
                        top: 0,
                        right: 0,
                        bottom: 10,
                        left: 10
                    },
                },
                yaxis: {
                    show: false,
                },
                fill: {
                    opacity: 1
                },
                tooltip: {
                    y: {
                        formatter: function (val) {
                            return val.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
                        },
                        title: {
                            formatter: function (seriesName) {
                                return seriesName === 'Đơn hàng' ? 'Đơn hàng' : 'Doanh thu';
                            }
                        }
                    }
                },
                colors: ['#435EEF', '#59a2fb'],
            };

            var chart = new ApexCharts(
                document.querySelector("#orders"),
                options
            );
            chart.render();
        })
        .catch(error => console.error('Lỗi khi lấy dữ liệu:', error));
});
