$(document).ready(function () {
    $("#CurrencyId").change(function () {
        $.ajax({
            type: 'POST',
            url: '/CurrenciesService/GetCurrencyRateById',
            dataType: 'json',
            data: { id: $(this).val() },
            success: function (value) {
                document.getElementById('CurrencyRate').value = value;
            },
            error: function (ex) {
                alert('Failed to retrieve states.' + ex);
            }
        });
        return false;
    })
});


