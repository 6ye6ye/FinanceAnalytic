$(document).ready(function () {
    $("#SpendingCategoryId").change(function () {
    
        let dropdown = $('#SpendingSubCategoryId');
        dropdown.empty();
        var defaultE = document.createElement("option");
        defaultE.value = "";
        defaultE.text = "Все подкатегории";
        dropdown.append(defaultE);
        $.ajax({

            type: 'POST',
            url: '/SpendingSubCategories/GetSpendingSubCategories',
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


