$(document).ready(function () {

    var buttionName = "";

    // SUBMIT FORM
    $("#SAHReportForm").submit(function (event) {
        // Prevent the form from submitting via the browser.
        buttionName = $("#submitSAHForm").text();
        userLog();
        event.preventDefault();


    });
    $("#printCheckBtn").off().on("click", function (event) {
        var accountEntry = [];


        var entry =
            {

                partyName: $("#partyName option:selected").text(),
                bankName: $("#bankname option:selected").text(),
                date: $("#checkDate").val(),
                // address: address,
                amount: $("#amount").val(),
            }
        accountEntry.push(entry);

        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "/bankAccounts/print-check",
            data: JSON.stringify(accountEntry),
            dataType: "text",
            success: function (data) {
                // successmessage = 'Data was succesfully captured';
                /*if (data.result === 'ok')
                    location.reload();*/
                console.log(data);
                window.open("/bankAccounts/print-check?fileName=" + data, '_blank');
                target = "_blank"
            },
            error: function (data) {
                successmessage = 'Error';
            },
        });
        //location.replace("/bankAccounts/back-account-list?backAccountsRequest=" + accountEntry + "&partyName=" + $("#code option:selected").text());
        //console.log("sdsd");
    });

    function getFormattedDate(date) {
        var year = date.getFullYear();
        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        return day + '/' + month + '/' + year;
    }

    function getExcelBuilder() {

        return "";
    }


});