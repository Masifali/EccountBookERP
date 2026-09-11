$(document).ready(function () {
    var buttionName = "";
    //instrument select2 dropdowns
    $(".select2_single").on("select2:close", function () {
        $(this).focus();
    });

    $("#update").on("click", function () {

        if (!$("#account\\.code") || $("#account\\.code").val() == 0) {

            $.confirm({
                title: "SELECTION MISSING",
                content: "KINDLY SELECT SUPPLIER",
                type: 'red',
                typeAnimated: true,

            });

            return;
        }
        // setTimeout(function() { popupwin.close();}, 1000);
        location.href = "/add_or_update_bank_account/by_code?code=" + $("#account\\.code").val();
    });
    var bankAccountIds = [];
    $("#account\\.code").on("change", function () {
        $("#conform").text("");
        $("#loading").show();

        loadBankAccountByCode();
        bankAccountIds = [];
    });

    $("#code").on("change", function () {
        $("#conform").text("");
        $.get("/accounts/get_account?code=" + $("#code").val(), function (data) {
            $("#mobile").val(data.mobile);
        });
    });   //

    $("#send_ready_account").find("tbody").on('click', '.delete', function () {
        $(this).closest("tr").remove();
    });

    //$("#downlaodBackAccounts").off().on("click", function (event) {

    // function accountEntries() {
    $("#downlaodBackAccounts").off().on("click", function (event) {
        downloadSendBankAccount();

    });
    $("#downlaodBackAccount").off().on("click", function (event) {
        downloadSendBankAccount();

    });

    function downloadSendBankAccount() {
        var accountEntry = [];
        $("#send_ready_account tbody tr").each(function () {

            var bankName = $(this).closest("tr").find("td:eq(0)").text();
            var accountTitle = $(this).closest("tr").find("td:eq(1)").text();
            var branchCode = $(this).closest("tr").find("td:eq(2)").text();
            var accountNumber = $(this).closest("tr").find("td:eq(3)").text();
            var address = $(this).closest("tr").find("td:eq(4)").text();
            var amount = $(this).closest("tr").find("#number").val();
            var singleParty = $(this).closest("tr").find("#singleParty").val();
            // var amount1 = $(this).closest("tr").find("td:eq(5)").text();
            var entry =
                {
                    bankName: bankName,
                    accountTitle: accountTitle,
                    branchCode: branchCode,
                    accountNumber: accountNumber,
                    address: address,
                    amount: amount,
                    singleParty: singleParty,
                }
            accountEntry.push(entry);
        });
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "/bankAccounts/back-account-list?partyName=" + $("#code option:selected").text(),
            data: JSON.stringify(accountEntry),
            dataType: "text",
            success: function (data) {
                // successmessage = 'Data was succesfully captured';
                /*if (data.result === 'ok')
                    location.reload();*/
                let jsonData = typeof data === 'string' ? JSON.parse(data) : data;

                // Clean leading/trailing whitespace (including non-breaking space)
                let cleanFileName = jsonData.fileName.trim();
                location.replace("/bankAccounts/back-account-list?fileName=" + cleanFileName);
            },
            error: function (data) {
                successmessage = 'Error';
            },
        });
    }

    $("#bank_name_entry_table").find("tbody").on('click', '.send_account', function () {

        $("#conform").text("");
        var bankName = $(this).closest("tr").find("td:eq(1)").text();
        var accountTitle = $(this).closest("tr").find("td:eq(2)").text();
        var branchCode = $(this).closest("tr").find("td:eq(3)").text();
        var accountNumber = $(this).closest("tr").find("td:eq(4)").text();
        var address = $(this).closest("tr").find("td:eq(5)").text();
        //  $(this).css("background-color", "GREEN");
        // console.log($(this).background.color);
        // bankAccountIds.push($(this).closest("tr").find("#entryId").val());
        $(".accounts").val() + $(".accounts").val("BankName:" + bankName + " accountTitle:" + accountTitle + " branchCode:" + branchCode + "</br>" + "<br>" + "</b>");
        //   var roww = $("#send_ready_account").find("tr:first");

        $("#send_ready_account").find("tbody").append("<tr><td>" + bankName + "</td>" +
            "<td>" + accountTitle + "</td>" +
            "<td>" + branchCode + "</td>" +
            "<td>" + accountNumber + "</td>" +
            "<td>" + address + "</td>" +
            "<td><input type='number' style='form-control;from-group' value='0'  onclick='select()' id ='number'></td>" +
            "<td><input type='text' style='form-control;from-group' value='0' id ='singleParty' onclick='select()'></td>" +
            "<td> <a class='delete'  style='text-decoration:underline;'   href='javascript:void(0)'>DELETE</a> </td>" +
            "</tr>");

    });


    function loadBankAccountByCode() {

        $('#bank_name_entry_table').DataTable().clear();
        $('#bank_name_entry_table').DataTable().destroy();
        var bankAccountTable = $('#bank_name_entry_table').DataTable({
            dom: 'Bfrtip',
            'bPaginate': false,
            destroy: true,
            buttons: [],
        });
        if (!$("#account\\.code") || $("#account\\.code").val() == 0) {

            return;
        }

        $.get("/bank_account/by_code?code=" + $("#account\\.code").val(), function (data) {

            $.each(data.bankAccountEntries, function (i, bankAccountEntry) {

                bankAccountTable.row.add([
                    i + 1,
                    '<a class="name">' + bankAccountEntry.bankName.name + '</a>',
                    '<a class="accountTitle">' + bankAccountEntry.accountTitle + '</a>',
                    '<a class="branchCode">' + bankAccountEntry.branchCode + '</a>',
                    '<a class="accountNumber">' + bankAccountEntry.accountNumber + '</a>',
                    '<td><a class="address">' + bankAccountEntry.address + '</a> <input type= "hidden" id ="entryId" value=' + bankAccountEntry.id + '></td>',
                    '<a class="send_account"  style="text-decoration:underline;"   href="javascript:void(0)">SEND ACCOUNT</a>',
                ]);
            });
            bankAccountTable.draw();
        });

        $("#loading").hide();
    }

    $("#editBankAccount").off().on("click", function (event) {
        event.preventDefault();
        event.stopImmediatePropagation();

        $.get("/add_or_update_bank_account/by_code?code=" + $("#account\\.code").val(), function (data) {

        });
        //console.log("sdsd");
    });

    function userLog() {

        var userLogObj = {
            idNumber: "0",
            code: "0",
            buttonClick: buttionName,

            windowName: $('h2').html(),
        }

        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "/viewSaleOrder",
            data: JSON.stringify(userLogObj),
            dataType: "json",
            success: function (data) {
                successmessage = 'Data was succesfully captured';
                //location.reload();
                //  $("#customerAccount\\.balanceLimit").text(successmessage);
            },
            error: function (data) {
                successmessage = 'Error';
                //  $("#customerAccount\\.balanceLimit").text(successmessage);
            },
        });
    }

});

/*
       $('body').on('click', '.send_account', function(){

   //	 $('#bank_name_entry_table tbody').on( 'click', 'tr', function () {  this also working for row click
           $("#conform").text("");
               var bankName = $(this).closest("tr").find("td:eq(1)").text();
               var accountTitle= $(this).closest("tr").find("td:eq(2)").text();
               var branchCode= $(this).closest("tr").find("td:eq(3)").text();
               var accountNumber= $(this).closest("tr").find("td:eq(4)").text();
               var address= $(this).closest("tr").find("td:eq(5)").text();

               var row = $(this).parent();
            if (!$("#code") || $("#code").val() == 0){
                alert("Select Customer Account");
                   return;
               }
            if ($("#mobile").val().length>11||$("#mobile").val().length<11){

                alert("Enter Corrent Moblie Number  "+$("#mobile").val());
                   return;
               }
            $.confirm({

                   title: "CONFIRMATION REQUIRED",
                   content: "ARE YOU SURE YOU WANT TO SEND ? ====> \n"+bankName+"   "+  accountTitle,
                   type: 'red',
                   typeAnimated: true,
                   buttons: {
                       confirm: function () {

                           var sendBankAccount = {
                                bankName:bankName,// $(this).find("td:eq(1)").text(),
                                accountTitle: accountTitle,//$(this).find("td:eq(2)").text(),
                                branchCode: branchCode,//$(this).find("td:eq(3)").text(),
                                accountNumber: accountNumber,//$(this).find("td:eq(4)").text(),
                                address: address,//$(this).find("td:eq(5)").text(),
                                mobile:$("#mobile").val(),
                                phone:$("#phone").val()  ,
                                code:$("#code").val()
                             }

                        $.ajax({
                               type : "POST",
                               contentType : "application/json",
                               url : "/bank_account/send_sms",
                               data : JSON.stringify(sendBankAccount),
                               dataType : "json",
                               success:function(data){
   //	    		    			successmessage = 'successfully Send';
                                   $("#conform").text(""+bankName+"  <---->  "+accountTitle+" <---->  "+accountNumber +" <----> "+" ( "+data.reply+")");

                               },
                               error: function(data) {
                                   successmessage = 'Error';
                                   $("#conform").text(successmessage+" ( "+data.reply+")");
                               },
                           });
                       },
                       cancel: function () {
                       },
                   }
               });

           } );*/
