$.getScript("https://www.gstatic.com/charts/loader.js", function () {
    google.charts.load('current', { 'packages': ['corechart'] });
    google.charts.setOnLoadCallback(myCallback);
    function myCallback() {

        $.ajax({
            url: '/DataForChartsHelper/GetColumnChartDataSpendings',
            type: 'POST',
            data: {
                PeriodBegin: $("#PeriodBegin").val(),
                PeriodEnd: $("#PeriodEnd").val(),
                PeriodType: $("#PeriodTypes").val(),
                selectedSpendingCategory: $("#SpendingCategoryId").val(),
                selectedSpendingSubCategory: $("#SpendingSubCategoryId").val(),
                selectedImportanceCategory: $("#ImportanceCategoryId").val()
            },
            success: function (jsonData) {
          
             

                var data = google.visualization.arrayToDataTable(jsonData);
                //var view = new google.visualization.DataView(data);
                //view.setColumns([0, 1, {
                //    calc: 'stringify',
                //    sourceColumn: 1,
                //    type: 'string',
                //    role: 'annotation'
                //}]);
                var chart = new google.visualization.ColumnChart(document.getElementById('column_chart_div'));
                var options = {
                    //width: 300,
                    //  height: 400,
                    title: "Потрачено по категориям расходов" ,
                    legend: { position: 'top', maxLines: 3 },

                    animation: {
                        duration: 1000,
                        easing: 'in'
                    },
                    hAxis: { viewWindow: { min: 0, max: 5 } },
                    legend: { position: "none" },
              
                    isStacked: true,
                };
                var MAX = 20;
             
                var prevButton = document.getElementById('b1');
                var nextButton = document.getElementById('b2');
                var changeZoomButton = document.getElementById('b3');
                function drawChart() {
                    // Disabling the button while the chart is drawing.
                    prevButton.disabled = true;
                    nextButton.disabled = true;
                    changeZoomButton.disabled = true;
                    google.visualization.events.addListener(chart, 'ready',
                        function () {
                            prevButton.disabled = options.hAxis.viewWindow.min <= 0;
                            nextButton.disabled = options.hAxis.viewWindow.max >= MAX;
                            changeZoomButton.disabled = false;
                        });
                    chart.draw(data, options);
                }
                prevButton.onclick = function () {
                    options.hAxis.viewWindow.min -= 1;
                    options.hAxis.viewWindow.max -= 1;
                    drawChart();
                }
                nextButton.onclick = function () {
                    options.hAxis.viewWindow.min += 1;
                    options.hAxis.viewWindow.max += 1;
                    drawChart();
                }
                var zoomed = false;
                changeZoomButton.onclick = function () {
                    if (zoomed) {
                        options.hAxis.viewWindow.min = 0;
                        options.hAxis.viewWindow.max = 5;
                    } else {
                        options.hAxis.viewWindow.min = 0;
                        options.hAxis.viewWindow.max = MAX;
                    }
                    zoomed = !zoomed;
                    drawChart();
                }
                drawChart();
              
            },
            error: function () {
                alert("Ошибка загрузки данных. Повторите пожалуйста");
            }
        });
    };
});






////    f          //  art() /Spendings/GetColumnChartDataSpendings({
            //  url: '/S'GET'n
            //ta: { PeriodBegin: $("#PeriodBegin").val(), PeriodEnd: $("#PeriodEnd").val() }, { Dat    in: $("#//teBegin").val(), PeriodEnd//$("#PeriodEnd").val() },
////  //    async: false
////        }).responseText;

////    Populat//nChart(jsonData, "column-chart");

////    }

////  //function PopulationChart(jsonData, chart_type) {
////        // Create o// data table out of JSON data loaded from server.
////        var da// = new google.visu//ization.DataTable(jsonData);
////    var chart;
////    var options = {title: 'Ord//s by city' };

/////    switch (chart_type) {

////    default:
////    chart = new google.visualization.ColumnC//rt(document.ge//lementById('col//n_chart_div'))});////    break;
////        }

////   //hart.draw//ata, options);
////    return false;
////    }
////});