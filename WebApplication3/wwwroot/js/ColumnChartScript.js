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
            success: function (djson) {
    
                 djson[0].push({ role: 'annotation' });
                for (var i = 1; i < djson.length; i++) {
                    var total=0;
                    
                    for (var j = 1; j < djson[0].length-1; j++) {
                        total += djson[i][djson[i].length - j];
                    }
                   // djson[i][djson[i].length - 1] = 0;
                    djson[i].push(total);

                }
                var data = google.visualization.arrayToDataTable(djson);
                //var view = new google.visualization.DataView(data);
                //view.setColumns([0, 1, {
                //    calc: 'stringify',
                //    sourceColumn: 1,
                //    type: 'string',
                //    role: 'annotation'
                //}]);
                var chart = new google.visualization.BarChart(document.getElementById('column_chart_div'));
                var options = {
                    //width: 300,
                  //  height: 400,
                    title: "Потрачено по категориям расходов",
                    legend: { position: 'top', maxLines: 3 },
                    animation: {
                        duration: 1000,
                        easing: 'out'
                    },
                    legend: { position: "none" },
              
                    isStacked: true,
                };
              

                chart.draw(data, options);
              
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