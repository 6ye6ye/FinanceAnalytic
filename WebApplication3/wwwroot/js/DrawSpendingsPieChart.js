$.getScript("https://www.gstatic.com/charts/loader.js", function () {
    google.charts.load('current', { 'packages': ['corechart'] });
    google.charts.setOnLoadCallback(myCallback);
      function myCallback() {

        $.ajax({
            url: '/DataForChartsHelper/GetPieChartDataSpendingImp',
            type: 'POST',
            data: { PeriodBegin: $("#PeriodBegin").val(), PeriodEnd: $("#PeriodEnd").val() },
            success: function (chartsdata) {
                if (chartsdata) {
                    var data = new google.visualization.DataTable();
                    data.addColumn('string', 'Категорияя важности');
                    data.addColumn('number', 'Потрачено');
                    $(chartsdata).each(function (x, y) {
                        data.addRow([y.name, y.cost]);
                    });
                    var chart = new google.visualization.PieChart(document.getElementById('chart_spendImp_div'));
                    var options = {
                        title: "По категория важности",
                        width: '50%',
                        height: '50%',
                        legend: { position: 'top', maxLines: 3 },
                        bar: { groupWidth: '75%' },
                 
                        colors: ['#e0440e', '#e67e3e', '#e46328', '#f6c386', '#eca06e','#ffdc96'],
                        isStacked: true,
                    };
                    chart.draw(data, options);
                }
            },
            error: function () {
                alert("Ошибка загрузки данных. Повторите пожалуйста");
            }
        });
        $.ajax({
            url: '/DataForChartsHelper/GetPieChartDataSpendingCat',
            type: "POST",
            data: { PeriodBegin: $("#PeriodBegin").val(), PeriodEnd: $("#PeriodEnd").val() },
            success: function (chartsdata) {
                if (chartsdata) {
                    var data = new google.visualization.DataTable();
                    data.addColumn('string', 'Категорияя расходов');
                    data.addColumn('number', 'Потрачено');
                    $(chartsdata).each(function (x, y) {
                        data.addRow([y.name, y.cost]);
                    });
                    var chart = new google.visualization.PieChart(document.getElementById('chart_spendCat_div'));
                    var options = {
                        title: "По категориям расходов",
                        width: '50%',
                        height: '50%',
                        legend: { position: 'top', maxLines: 3 },

                        bar: { groupWidth: '75%' },
                        colors: ['#e0440e', '#e67e3e', '#e46328', '#f6c386', '#eca06e', '#ffdc96'],
                        isStacked: true,
                    };
                    chart.draw(data, options);
                }
            },
            error: function () {
                alert("Ошибка загрузки данных. Повторите пожалуйста");
            }
        });
    };
    $(document).ready(function () {
        $(window).resize(function () {
            myCallback();
        });
    });
});

