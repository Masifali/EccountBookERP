window.onload = function () {
    //loadTwoLevelAccounts();
    // $('#accountDataDiv').hide();
};


function getFormData($form) {
    var unindexed_array = $form.serializeArray();
    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        console.log(indexed_array[n['name']]);
        if (n['name'] === 'supplierAccount') {
            indexed_array[n['name']] = {code: n['value']};
        } else {
            indexed_array[n['name']] = n['value'];
        }
    });

    return indexed_array;
}

function Calculate() {
    var Kgs = parseFloat($("#fWeight").val());
    var values = Kgs / 40;
    var decValues = Kgs % 40;
    if (decValues === 0) {
        $("#mun").val(values);
        $("#kg").val(0);
    } else {
        var pointValue = String(values).split(".")[0];
        var KgValue = "." + String(values).split(".")[1];
        $("#mun").val(pointValue);
        $("#kg").val(parseFloat(KgValue) * 40);
    }
}

$("#fWeight").on('input', function () {
    Calculate();
});

$("#autioWeight").on('input', function () {
    var Kgs = parseFloat($("#autioWeight").val());
    if ($("#MrA").val() === "1") {
        $("#fWeight").val(Kgs);
    }
    Calculate();
});


$(document).ready(function () {

    $('#sDate').val(new Date().toISOString().substring(0, 10));
    // $('#time').val(new Date().toISOString().substring(10, 18));
    var intervalId = window.setInterval(function () {
        getWeight();
    }, 500);

    function getWeight() {
        $.get("/kanta/get-current-weight", function (data) {
            $("#autioWeight").val(data);
            if ($("#MrA").val() === "1" && $("#bridgeId").val().length > 0) {
                $("#fWeight").val(data);
            } else {
                $("#sWeight").val(data);
            }

            Calculate();
        });
    }

    $(".select2_single").on("select2:close", (function () {
            $(this).focus();
        })
    );

// SUBMIT FORM
    $("#weightDataForm").submit(function (event) {
        // Prevent the form from submitting via the browser.
        event.preventDefault();
        addOrUpdateWeightData();
    });


    function addOrUpdateWeightData() {

        var weightDataForm = $('#weightDataForm');
        jsonData = getFormData(weightDataForm);
        jsonData.se = "asif  idres";
        console.log(jsonData);

        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: weightDataForm.attr('action'),
            data: JSON.stringify(jsonData),
            dataType: 'json',
            success: function (data) {
                // $("#modal_success").modal('show');
            },
        });
    }

    function getFormattedDate(date) {
        var year = date.getFullYear();
        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        return day + '/' + month + '/' + year;
    }


    function showAccountDataForm() {


        $('#accountDataDiv').show();

        $.get("/accounts/account_data?code=" + $("#fourLevelAccounts").val(), function (data) {

            // UPDATE FORM DATA
            $("#weightDataForm #weightType").val(data);

            $("#weightDataForm #upperLimit").val(data.accountData.upperLimit);
            $("#weightDataForm #contactPerson").val(data.accountData.contactPerson);
            $("#weightDataForm #salesman").val(data.accountData.salesman);
            $("#weightDataForm #mobile").val(data.accountData.mobile);

        });
    }

})
;