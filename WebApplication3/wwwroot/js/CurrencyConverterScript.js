
$(document).ready(function () {
    $("#button_convert").click(function () {
        let MSumInCurrency = Number($('#SumInCurrency').val());
        var MCurrencyRate = Number($('#CurrencyRate').val());
        var val = MSumInCurrency * MCurrencyRate;
        document.getElementById('Sum').value = val;


    })
});


