$(document).ready(function () {
    $("#IncomeCategoryId").change(function () {
    
        let dropdown = $('#IncomeSubCategoryId');
        dropdown.empty();
        var defaultE = document.createElement("option");
        defaultE.value = "";
        defaultE.text = "Не выбранно";
        dropdown.append(defaultE);
        //dropdown.prop('selectedIndex', 0);
        $.ajax({

            type: 'POST',
            url: '/IncomeSubCategories/GetIncomeSubCategories',
            dataType: 'json',
            data: { id: $(this).val() },
            success: function (items) {
                $.each(items, function (_key, _value) {
                    dropdown.append($('<option></option>').attr('value', _value.value).text(_value.text));
                });
            },
            error: function (ex) {
                alert('Failed to retrieve states.' + ex);
            }
        });
        return false;
    })
});


