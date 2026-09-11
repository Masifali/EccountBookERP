function getFormData($form) {
    var unindexed_array = $form.serializeArray();
    var indexed_array = {};

    var getFocusBookNumber = parseFloat(0);
    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });

    return indexed_array;
}

var qtyInMan = parseFloat(0);
Number.prototype.toFixed = function (decimalPlaces) {
    return Number(Math.round(this + 'e' + decimalPlaces) + 'e-' + decimalPlaces);
}
$(".select2_single").on("change", function () {
    console.log("change " + $(".select2_single").val());
    if (!$(".select2_single").val() || $(".select2_single").val() == 0) {
        return;
    }

    $.get("/receivables/account_limit_and_balance?accountCode=" + $(".select2_single").val(), function (data) {

    });
});


$(document).ready(function () {

    getAppParties();

    function getAppParties() {
        $.get("/app/getAllAccounts", function (data) {

            for (var i = 0, len = data.length; i < len; i++) {

                var option = "<option value = " + data[i].code + ">" + data[i].formattedCode + " &emsp;" + data[i].accountName + "</option>";
                var option1 = "<option value = " + data[i].code + ">" + data[i].accountName + "</option>";

                $("#itemStock\\.account\\.code").append(option);
                $("#itemStock\\.thirdParty\\.code").append(option);
                $("#brokerAccount").append(option);

            }
        });

    }

    $("#details").on('click', function (event) {
        event.preventDefault();
        $("#detailsAccount").toggle();
    })
///	console.log("aa");
    var formatterNoFloat = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 0, maximumFractionDigits: 0});
    var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 2, maximumFractionDigits: 2});

    function calculateAmountEntriesTotal() {

        var debitTotal = parseFloat(0);
        var creditTotal = parseFloat(0);


        $("#voucher_entry_table tbody tr").each(function () {
            debitTotal = debitTotal + parseFloat($(this).find(".voucherEntry_debit").val() || 0);
            creditTotal = creditTotal + parseFloat($(this).find(".voucherEntry_credit").val() || 0);
        });

        $("#voucher_entry_table span#debitTotal").text(formatter.format(debitTotal));
        $("#voucher_entry_table span#creditTotal").text(formatter.format(creditTotal));

        if ($("#voucher_entry_table span#debitTotal").text() == $("#voucher_entry_table span#creditTotal").text()) {
            $("#voucher_entry_table span#debitTotal").css("color", "#73879C");
            $("#voucher_entry_table span#creditTotal").css("color", "#73879C");
        } else {
            $("#voucher_entry_table span#debitTotal").css("color", "red");
            $("#voucher_entry_table span#creditTotal").css("color", "red");
        }
    }


    $("#branch").focus();

    getFocusBookNumber = 1;


    $(".itemDef").on("change", function () {

        loadItemDefDetail($(this));
        //		$("#itemStock\\.itemDef").focus();
        getFocusBookNumber = 1;
        calculateTotal();
    });

    function loadItemDefDetail(thisControl) {

        //  console.log("sddsds");
        if (!$(thisControl).val() || $(thisControl).val() == 0) {
            return;
        }

        if (!$("#branch").val()) {
            return;
        }

        $.get("/payables/item_def_detail?itemDefId=" + $(thisControl).val() + "&companyId=" + 1 + "&branchId=" + $("#branch").val() + "&voucherStatusId=" + "0", function (data) {
            $("#pricingRule").val(data.pricingRule);
            $("#weightParBag").val(data.weightParBag);
            calculateTotal();
        });
    }

    $("#itemStock\\.account\\.code").on("change", function () {
        $("#voucher_entry_table tbody").find("tr:eq(0) .voucherEntry_account:eq(0)").val($(this).val());
        $("#voucher_entry_table tbody").find("tr:eq(0) .voucherEntry_account:eq(0)").closest("td").find("span").text($(this).find("option:selected").text());

        calculateTotal();
    });
    $("#frightAccount").on("change", function () {
        $("#voucher_entry_table tbody").find("tr:eq(3) .voucherEntry_account:eq(0)").val($(this).val());
        $("#voucher_entry_table tbody").find("tr:eq(3) .voucherEntry_account:eq(0)").closest("td").find("span").text($(this).find("option:selected").text());

        calculateTotal();
    });
    $("#brokerAccount").on("change", function () {
        $("#voucher_entry_table tbody").find("tr:eq(9) .voucherEntry_account:eq(0)").val($(this).val());
        //$("#voucher_entry_table tbody").find("tr:eq(10) .voucherEntry_account:eq(1)").val($(this).val());
        $("#voucher_entry_table tbody").find("tr:eq(9) .voucherEntry_account:eq(0)").closest("td").find("span").text($(this).find("option:selected").text());
    });

    // change account
    $("#itemStock\\.itemStockEntries0\\.bags").on("input", function () {

        calculateTotal();
    });

    $("#itemStock\\.brokriAmount").blur(function () {
        var amount = $("#itemStock\\.itemStockEntries0\\.amount").val().replace(/[^0-9\.-]+/g, "");//.replace(/[^0-9\.-]+/g, "")
        // console.log("totalAmount   ");
        if (parseFloat(amount) == 0)
            alert("Amount is zero");
        else {
            if ($("#brokeryByMan").val() === 'true') {
                $("#itemStock\\.brokriRate").val(($("#itemStock\\.brokriAmount").val() / qtyInMan).toFixed(8));
                $("#brokriValue").val($("#itemStock\\.brokriRate").val());
            } else {
                $("#itemStock\\.brokriRate").val(($("#itemStock\\.brokriAmount").val() / amount).toFixed(8));
                $("#brokriValue").val($("#itemStock\\.brokriRate").val() * 100);
            }
            //console.log($("#itemStock\\.brokriAmount").val() / amount);
            //  $("#itemStock\\.brokriRate").val(($("#itemStock\\.brokriAmount").val() / amount).toFixed(8));
            calculateTotal();
        }
        // $("#brokriValue").val($("#itemStock\\.brokriRate").val() * 100);
    });

//	  $("#itemStock\\.itemStockEntries0\\.totalKatoti").on("input", function () {
    $("#itemStock\\.itemStockEntries0\\.totalKatoti").blur(function () {
        $("#itemStock\\.itemStockEntries0\\.katoti").val($("#itemStock\\.itemStockEntries0\\.totalKatoti").val() / $("#itemStock\\.itemStockEntries0\\.bags").val());
        //  console.log("asif");
        calculateTotal();
    });

    $("#itemStock\\.itemStockEntries0\\.totalBagWeightDeduction").blur("input", function () {
        $("#itemStock\\.itemStockEntries0\\.bagWeightDeduction").val($("#itemStock\\.itemStockEntries0\\.totalBagWeightDeduction").val() / $("#itemStock\\.itemStockEntries0\\.bags").val());
        calculateTotal();
    });

    $("#itemStock\\.itemStockEntries0\\.katoti").on("input", function () {

        calculateTotal();
    });
    $("#itemStock\\.itemStockEntries0\\.bagWeightDeduction").on("input", function () {
        // $("#itemStock\\.itemStockEntries0\\.bagWeightDeduction").val($("#itemStock\\.itemStockEntries0\\.totalBagWeightDeduction").val()/ $("#itemStock\\.itemStockEntries0\\.bags").val());
        calculateTotal();
    });

    $("#itemStock\\.itemStockEntries0\\.price").on("input", function () {

        calculateTotal();
    });
    $("#itemStock\\.itemStockEntries0\\.totalKg").on("input", function () {

        calculateTotal();
    });

    $("#itemStock\\.brokriRate").on("input", function () {
        calculateTotal();
    });
    $("#itemStock\\.millTaxRate").on("input", function () {
        calculateTotal();
    });
    $("#itemStock\\.bankTaxRate").on("input", function () {

        calculateTotal();
    });
    $("#itemStock\\.silaiRate").on("input", function () {

        calculateTotal();
    });
    $("#itemStock\\.unloadingRate").on("input", function () {

        calculateTotal();
    });
    $("#itemStock\\.bardanaRate").on("input", function () {

        calculateTotal();
    });

    $("#itemStock\\.otherExp").on("input", function () {

        calculateTotal();
    });

    $("#itemStock\\.freightCharges").on("input", function () {

        calculateTotal();
    });

    $("#brokriValue").on("input", function () {
        if ($("#brokeryByMan").val() === 'true') {
            $("#itemStock\\.brokriRate").val(($("#brokriValue").val()));
        } else {
            $("#itemStock\\.brokriRate").val(($("#brokriValue").val() / 100));
        }
        //   $("#itemStock\\.brokriRate").val(($("#brokriValue").val() / 100));
        calculateTotal();
    });
    $("#itemStock\\.brokeryRate").on("input", function () {

        // if($("#itemStock\\.brokeryRate").val()>0){
        //   $("#itemStock\\.brokeryRate").val(-$("#itemStock\\.brokeryRate").val());
        // }
        var borkyValue = ($("#itemStock\\.brokeryRate").val() / 100).toFixed(10);
        var amountt = parseFloat($("#itemStock\\.itemStockEntries0\\.amount").val().replace(/[^0-9\.-]+/g, "")) * parseFloat(borkyValue);
        if ($("#brokeryByMan").val() === 'true') {
            borkyValue = ($("#itemStock\\.brokeryRate").val()).toFixed(10);
            amountt = qtyInMan * parseFloat(borkyValue);
        }

        console.log(amountt);
        $("#itemStock\\.otherExp").val(parseFloat(amountt).toFixed(0));
        calculateTotal();
    });
    $("#voucherDate").on("blur", function () {
        getPendingSaleOrderList();
    });
    $("#itemStock\\.marketFee").on("input", function () {

        calculateTotal();
    });
    $("#itemStock\\.gisahi").on("input", function () {

        calculateTotal();
    });
//  
//     $("#itemStock\\.account\\.code").on("change", function(){
//		
//		//$("#voucher\\.bookNumber").focus();
//		$("#voucher\\.bookNumber").val("asif");
////		$("#voucher_entry_table tbody").find("tr:eq(3) .voucherEntry_account:eq(0)").val($(this).val());
////		$("#voucher_entry_table tbody").find("tr:eq(3) .voucherEntry_account:eq(0)").closest("td").find("span").text($(this).find("option:selected").text());
//	});

    //calculateTotal();

    generateTabIndexing();

    function generateTabIndexing() {

        $("#branch").attr('tabindex', 1);
        $("#voucherDate").attr('tabindex', 2);
        $("#vehicalNumber").attr('tabindex', 3);
        $("#itemStock\\.itemStockEntries0\\.itemDef").attr('tabindex', 4);
        $("#bookNumber").attr('tabindex', 5);
        $("#itemStock\\.itemStockEntries0\\.bags").attr('tabindex', 6);
        $("#itemStock\\.itemStockEntries0\\.totalKg").attr('tabindex', 7);
        $("#itemStock\\.itemStockEntries0\\.bagWeightDeduction").attr('tabindex', 8);
        $("#itemStock\\.itemStockEntries0\\.totalBagWeightDeduction").attr('tabindex', 9);
        $("#itemStock\\.itemStockEntries0\\.katoti").attr('tabindex', 10);
        $("#itemStock\\.itemStockEntries0\\.totalKatoti").attr('tabindex', 11);


        $("#itemStock\\.account\\.code").attr('tabindex', 12);
        $("#itemStock\\.invoiceDueDate").attr('tabindex', 13);
        $("#itemStock\\.itemStockEntries0\\.price").attr('tabindex', 14);
        $("#itemStock\\.itemStockEntries0\\.amount").attr('tabindex', -1);
        $("#itemStock\\.millTaxRate").attr('tabindex', 15);
        $("#itemStock\\.bankTaxRate").attr('tabindex', 16);
        $("#itemStock\\.silaiRate").attr('tabindex', 17);
        $("#itemStock\\.bardanaRate").attr('tabindex', 18);
        $("#itemStock\\.unloadingRate").attr('tabindex', 19);
        $("#itemStock\\.otherExp").attr('tabindex', 20);
        $("#itemStock\\.freightCharges").attr('tabindex', 21);
        $("#itemStock\\.totalAmount").attr('tabindex', -1);

        $("#voucherNarration").attr('tabindex', 22);
        $("#ppCredit").attr('tabindex', 23);
        $("#ppDebit").attr('tabindex', 24);
        $("#juteCredit").attr('tabindex', 25);
        $("#juteDebit").attr('tabindex', 26);
        $("#rem").attr('tabindex', 27);
        $("#brokerAccount").attr('tabindex', 28);
        $("#itemStock\\.brokriRate").attr('tabindex', 29)
    }

    function calculateTotal() {
        var amountTotal = parseFloat(0);
        var standardPrice = parseFloat(0);
        var itemRate = parseFloat(0);
        var discount = parseFloat(0);
        var discount_amount = parseFloat(0);
        var discount_total_amount = parseFloat(0);
        var TotalamountWithOutDiscount = parseFloat(0);
        var itemQty = parseFloat(0);
        var itemTotalQty = parseFloat(0);

        var creditCharges = parseFloat(0);
        var debitCharges = parseFloat(0);
        qtyInMan = parseFloat(0);
        var invoiceAmountTotal = parseFloat(0);
        var barokiRate = parseFloat(0);
        var brokriAmount = parseFloat(0);
        //var productAmount = parseFloat(0);
        var totalAmount = parseFloat(0);
        var price = parseFloat(0);
        var saleAmount = parseFloat(0);
        var millTaxRate = parseFloat(0);
        var millTaxAmount = parseFloat(0);
        var bankTaxRate = parseFloat(0);
        var bankTaxAmount = parseFloat(0);
        var silaiRate = parseFloat(0);
        var silaiAmount = parseFloat(0);
        var bardanaRate = parseFloat(0);
        var bardanaAmount = parseFloat(0);
        var unloadingRate = parseFloat(0);
        var unloadingAmount = parseFloat(0);
        var otherExp = parseFloat(0);
        var freightRate = parseFloat(0);
        var freightCharges = parseFloat(0);
        var itemQty = parseFloat(0);
        var totalKg = parseFloat(0);
        var safiKg = parseFloat(0);
        var deduction = parseFloat(0);
        var totalDeduction = parseFloat(0);
        var bags = parseFloat(0);
        var bagWeightDeduction = parseFloat(0);
        var totalBagWeightDeduction = parseFloat(0);
        var otherBagWeight = parseFloat(0);
        var gisahi = parseFloat(0);
        var marketFee = parseFloat(0);
        bags = $("#itemStock\\.itemStockEntries0\\.bags").val();

        barokiRate = $("#itemStock\\.brokriRate").val();
        brokriAmount = $("#itemStock\\.brokriAmount").val();
        totalAmount = $("#itemStock\\.totalAmount").val();
        price = $("#itemStock\\.itemStockEntries0\\.price").val();
        saleAmount = $("#itemStock\\.itemStockEntries0\\.amount").val();
        millTaxRate = $("#itemStock\\.millTaxRate").val();
        millTaxAmount = $("#itemStock\\.millTaxAmount").val();
        bankTaxRate = $("#itemStock\\.bankTaxRate").val();
        bankTaxAmount = $("#itemStock\\.bankTaxAmount").val();
        silaiRate = $("#itemStock\\.silaiRate").val();
        silaiAmount = $("#itemStock\\.silaiAmount").val();
        bardanaRate = $("#itemStock\\.bardanaRate").val();
        bardanaAmount = $("#itemStock\\.bardanaAmount").val();
        unloadingRate = $("#itemStock\\.unloadingRate").val();
        unloadingAmount = $("#itemStock\\.unloadingCharges").val();
        otherExp = $("#itemStock\\.otherExp").val();
        freightRate = $("#itemStock\\.freightRate").val();
        freightCharges = $("#itemStock\\.freightCharges").val();
        itemQty = $("#itemStock\\.itemStockEntries0\\.itemQuantity").val();
        totalKg = $("#itemStock\\.itemStockEntries0\\.totalKg").val();
        safiKg = $("#itemStock\\.brokriAmount").val();
        deduction = $("#itemStock\\.itemStockEntries0\\.katoti").val();
        bagWeightDeduction = $("#itemStock\\.itemStockEntries0\\.bagWeightDeduction").val();
        totalBagWeightDeduction = $("#itemStock\\.itemStockEntries0\\.totalBagWeightDeduction").val();
        otherBagWeight = $("#itemStock\\.itemStockEntries0\\.otherBagWeight").val();

        totalDeduction = (bags * deduction).toFixed(0);
        totalBagWeightDeduction = (bags * bagWeightDeduction).toFixed(1);

        safiKg = totalKg - totalDeduction - totalBagWeightDeduction;
        if ($("#pricingRule").val() == "KG") {
            qtyInMan = safiKg / parseFloat($("#weightParBag").val());

        } else {
            qtyInMan = bags;
        }

        invoiceAmountTotal = qtyInMan * price;

        var int_part = Math.trunc(qtyInMan); // returns 3
        var float_part = Number((qtyInMan - int_part).toFixed(2));

        $("#itemStock\\.itemStockEntries0\\.manWithKg").val(int_part + "- " + (float_part * parseFloat($("#weightParBag").val())).toFixed(0));
        console.log("a   " + int_part + "-" + (float_part * parseFloat($("#weightParBag").val())));
        $("#itemStock\\.itemStockEntries0\\.totalKatoti").val(totalDeduction);

        $("#itemStock\\.itemStockEntries0\\.totalBagWeightDeduction").val(totalBagWeightDeduction);

        $("#itemStock\\.itemStockEntries0\\.itemQuantity").val(safiKg);

        $("#itemStock\\.itemStockEntries0\\.otherBagWeight").val((safiKg / bags).toFixed(2))

        $("#itemStock\\.itemStockEntries0\\.amount").val(formatter.format(invoiceAmountTotal));

        $("#itemStock\\.bankTaxAmount").val(formatterNoFloat.format(invoiceAmountTotal * bankTaxRate / 100));

        $("#itemStock\\.millTaxAmount").val(formatterNoFloat.format(qtyInMan * millTaxRate));

        $("#itemStock\\.silaiAmount").val(formatterNoFloat.format(bags * silaiRate));

        $("#itemStock\\.bardanaAmount").val(formatterNoFloat.format(bags * bardanaRate));

        $("#itemStock\\.unloadingCharges").val(formatterNoFloat.format(bags * unloadingRate));

        // console.log("helo ");
        if ($("#brokeryByMan").val() === 'true') {
            $("#itemStock\\.brokriAmount").val((qtyInMan * barokiRate).toFixed(0));
        } else {
            $("#itemStock\\.brokriAmount").val((invoiceAmountTotal * barokiRate).toFixed(2));
        }

        brokriAmount = $("#itemStock\\.brokriAmount").val();
        bankTaxAmount = $("#itemStock\\.bankTaxAmount").val();
        millTaxAmount = $("#itemStock\\.millTaxAmount").val();
        bardanaAmount = $("#itemStock\\.bardanaAmount").val();
        silaiAmount = $("#itemStock\\.silaiAmount").val();
        unloadingAmount = $("#itemStock\\.unloadingCharges").val();
        otherExp = $("#itemStock\\.otherExp").val();
        freightCharges = $("#itemStock\\.freightCharges").val();

        //$("#itemStock\\.otherExp").val(formatterNoFloat.format(otherExp).replace(/[^0-9\.-]+/g, ""));
        $("#itemStock\\.freightCharges").val(formatterNoFloat.format(freightCharges).replace(/[^0-9\.-]+/g, ""));

        saleAmount = $("#itemStock\\.itemStockEntries0\\.amount").val();
        /*creditCharges = parseFloat(bankTaxAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(millTaxAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(freightCharges.replace(/[^0-9\.-]+/g, ""));
        debitCharges = parseFloat(bardanaAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(silaiAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(otherExp.replace(/[^0-9\.-]+/g, "")) + parseFloat(unloadingAmount.replace(/[^0-9\.-]+/g, ""));*/
        /*if (otherExp > 0) {*/
        creditCharges = parseFloat(bankTaxAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(millTaxAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(freightCharges.replace(/[^0-9\.-]+/g, ""));
        debitCharges = parseFloat(bardanaAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(silaiAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(otherExp.replace(/[^0-9\.-]+/g, "")) + parseFloat(unloadingAmount.replace(/[^0-9\.-]+/g, ""));
        /* }
         else {
             creditCharges = parseFloat(bankTaxAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(millTaxAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(freightCharges.replace(/[^0-9\.-]+/g, ""));
             debitCharges = -parseFloat(otherExp.replace(/[^0-9\.-]+/g, "")) + parseFloat(bardanaAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(silaiAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(unloadingAmount);

         }*/
        marketFee = $("#itemStock\\.marketFee").val();
        //gisahi = $("#itemStock\\.gisahi").val();
        debitCharges = debitCharges + parseFloat(marketFee);// + parseFloat(gisahi);
        console.log(saleAmount.replace(/[^0-9\.-]+/g, ""));
        $("#itemStock\\.netAmount").val(formatter.format(saleAmount.replace(/[^0-9\.-]+/g, "") - creditCharges + debitCharges));

        $("#voucher_entry_table tbody tr:eq(0) td:eq(4) input").val(parseFloat(saleAmount.replace(/[^0-9\.-]+/g, "")) - creditCharges + debitCharges);
        $("#voucher_entry_table tbody tr:eq(1) td:eq(5) input").val(parseFloat(saleAmount.replace(/[^0-9\.-]+/g, "")));
        $("#voucher_entry_table tbody tr:eq(2) td:eq(5) input").val(unloadingAmount.replace(/[^0-9\.-]+/g, ""));
        $("#voucher_entry_table tbody tr:eq(2) td:eq(4) input").val(0);
        $("#voucher_entry_table tbody tr:eq(3) td:eq(4) input").val(freightCharges.replace(/[^0-9\.-]+/g, ""));
        $("#voucher_entry_table tbody tr:eq(3) td:eq(5) input").val(0);
        $("#voucher_entry_table tbody tr:eq(4) td:eq(4) input").val(millTaxAmount.replace(/[^0-9\.-]+/g, ""));
        $("#voucher_entry_table tbody tr:eq(4) td:eq(5) input").val(0);
        $("#voucher_entry_table tbody tr:eq(5) td:eq(4) input").val(bankTaxAmount.replace(/[^0-9\.-]+/g, ""));
        $("#voucher_entry_table tbody tr:eq(5) td:eq(5) input").val(0);
        $("#voucher_entry_table tbody tr:eq(6) td:eq(5) input").val(silaiAmount.replace(/[^0-9\.-]+/g, ""));
        $("#voucher_entry_table tbody tr:eq(6) td:eq(4) input").val(0);
        //**-------------------------------
        //   check other expenxes are nagative or
        if (parseFloat(otherExp.replace(/[^0-9\.-]+/g, "")) > 0) {
            $("#voucher_entry_table tbody tr:eq(7) td:eq(5) input").val(otherExp.replace(/[^0-9\.-]+/g, ""));
            $("#voucher_entry_table tbody tr:eq(7) td:eq(4) input").val(0);
        } else {
            /*  debitCharges = debitCharges + (otherExp.replace(/[^0-9\.-]+/g, "")) * (-1);
              creditCharges = creditCharges + (otherExp.replace(/[^0-9\.-]+/g, "")) * (-1);*/
            $("#voucher_entry_table tbody tr:eq(7) td:eq(4) input").val((otherExp.replace(/[^0-9\.-]+/g, "")) * (-1));
            $("#voucher_entry_table tbody tr:eq(7) td:eq(5) input").val(0);
        }

        $("#voucher_entry_table tbody tr:eq(8) td:eq(5) input").val(bardanaAmount.replace(/[^0-9\.-]+/g, ""));
        $("#voucher_entry_table tbody tr:eq(8) td:eq(4) input").val(0);
        $("#voucher_entry_table tbody tr:eq(9) td:eq(5) input").val(brokriAmount.replace(/[^0-9\.-]+/g, ""));
        $("#voucher_entry_table tbody tr:eq(9) td:eq(4) input").val(0);
        $("#voucher_entry_table tbody tr:eq(9) td:eq(3) textarea").text("BROKRY");
        $("#voucher_entry_table tbody tr:eq(10) td:eq(4) input").val(brokriAmount.replace(/[^0-9\.-]+/g, ""));
        $("#voucher_entry_table tbody tr:eq(10) td:eq(5) input").val(0);
        $("#voucher_entry_table tbody tr:eq(10) td:eq(3) textarea").text("BROKRY");
//
        calculateAmountEntriesTotal();

    }


    function calcPercentage(amount, percentage) {
        return ((amount * percentage) / 100).toFixed(4);
    }

    $("#itemStock\\.itemStockEntries0\\.itemDef").on("change", function () {
        getPendingSaleOrderList();
    });

    $(".unloadingRate").on("input", function () {
        $(".unloadingCharges").val(($(".unloadingRate").val() * $("#sale_order_entry_table tfoot tr:eq(9) th:eq(1)").text()).toFixed(2));
    });


    $("#submitForm").off().on("click", function (event) {

        $("#submitForm").addClass('disabled');
        setTimeout(function () {
            $("#submitForm").removeClass('disabled');
        }, 10000);

        // Prevent the form from submitting via the browser.
        event.preventDefault();
        event.stopImmediatePropagation();
        // Will immediately show the confirmation popup
        $.confirm({
            title: "CONFIRMATION REQUIRED",
            content: "ARE YOU SURE YOU WANT TO SUBMIT THIS VOUCHER",
            buttons: {
                confirm: function () {
                    // SUBMIT FORM
                    $("#voucherForm").submit();
                },
                cancel: function () {
                },
            }
        });
    });

    // SUBMIT FORM
    $("#voucherForm").submit(function () {

        if (!$("#itemStock\\.account\\.code").val() || $("#itemStock\\.account\\.code").val() == 0) {
            $.confirm({
                title: "ENCOUNTERED AN ERROR!",
                content: "PLEASE SELECT PARY ACCOUNT",
                type: 'red',
                typeAnimated: true,
            });
            // Prevent the form from submitting via the browser.
            return false;
        }

        //console.log(parseFloat($("#itemStock\\.itemStockEntries0\\.bags").val()));
        if (!parseFloat($("#itemStock\\.itemStockEntries0\\.bags").val()) > 0) {
            $.confirm({
                title: "ENCOUNTERED AN ERROR!",
                content: "PLEASE ENTER NUMBER OF BAGS",
                type: 'red',
                typeAnimated: true,
            });
            // Prevent the form from submitting via the browser.

            return false;
        }
//		
        if (!$("#itemStock\\.itemStockEntries0\\.itemDef").val() || $("#itemStock\\.itemStockEntries0\\.itemDef").val() == 0) {
            $.confirm({
                title: "ENCOUNTERED AN ERROR!",
                content: "KINDLY SELECTE  PRODUCT  NAME",
                type: 'red',
                typeAnimated: true,
            });
            // Prevent the form from submitting via the browser.
            return false;
        }
//		
//		if (itemDuplicateExists){
//			$.confirm({
//			    title: "ENCOUNTERED AN ERROR!",
//			    content: "DUPLICATE ITEM EXISTS: " + itemDuplicate,
//			    type: 'red',
//			    typeAnimated: true,
//			});
//    		// Prevent the form from submitting via the browser.
//    		return false;
//    	}
//		
//		// prevent double submit
//		if($("#voucherForm").data("submitted")){
//			return false;
//		}
//	    else{
//	    	$("#voucherForm").data("submitted", true);
//	    }
    });

    /*	$("#branch").on("change", function(){
            getPendingSaleOrderList();

        });*/
    $("#itemStock\\.account\\.code").on("change", function () {
        getPendingSaleOrderList();

    });
    getPendingSaleOrderList();

    function getPendingSaleOrderList() {
        console.log("finction");
        var formData = {

            branch: {id: $("#branch").val(), name: ""},
            accountCode: $("#itemStock\\.account\\.code").val(),
            item: $("#itemStock\\.itemStockEntries0\\.itemDef").val(),
            fromDate: $("#voucherDate").val(),
            qty: "",
            requestFromSjv: "1",
        }
        console.log($("#branch").val());
        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "/receivables/pending_sale_order_list",
            data: JSON.stringify(formData),
            dataType: "json",
            success: function (data) {

                $("#itemStock\\.itemStockEntries0\\.saleOrderEntry").empty();
                $("#itemStock\\.itemStockEntries0\\.saleOrderEntry").append("<option></option>");

                for (var i = 0, len = data.length; i < len; i++) {
                    var option = "<option value = " + data[i].saleOrderEntryid + ">" + data[i].itemSubCategory.name + " " + data[i].itemDef.name + ' / ' + data[i].pendingWeight / 1000 + ' / ' + data[i].rate + ' / ' + data[i].date + "(" + data[i].paymentDate.split("-")[2] + ")" + ' / ' + data[i].millKhata + "</option>";
                    $("#itemStock\\.itemStockEntries0\\.saleOrderEntry").append(option);
                }
//				
            },
            complete: function () {
                // enable submit button
                //	$("#getPurchaseOrderList").attr("disabled", false);

            },
        });

    }

    $("#itemStock\\.itemStockEntries0\\.saleOrderEntry").on("change", function () {
        var selectedText = $("#itemStock\\.itemStockEntries0\\.saleOrderEntry option:selected").text();
        var arr = selectedText.split('/');
        $("#itemStock\\.itemStockEntries0\\.price").val(parseFloat(arr[2].trim()));
        calculateTotal();

    });

    $("input[type=number]").on("focus", function () {
        $(this).on("keydown", function (event) {
            if (event.keyCode === 38 || event.keyCode === 40) {
                event.preventDefault();
            }
        });
    });

    $('input[type=number]').on('wheel', function (e) {
        return false;
    });
    $('select').on('wheel', function (e) {
        return false;
    });

    // Catch the keydown for the entire document
    $(document).keydown(function (e) {
        //   console.log("asif idrees");
        // Set self as the current item in focus
        var self = $(':focus'),
            // Set the form by the current item in focus
            form = self.parents('form:eq(0)'),
            focusable;

        // Array of Indexable/Tab-able items
        //focusable = form.find('input').filter(':visible');
        focusable = form.find(':input:enabled:not([readonly], input:hidden, button:hidden, textarea:hidden), textarea:enabled:not([readonly] textarea:hidden), a.entry_add')
            .not(function () {   // do not include inputs with hidden parents
                return $(this).parent().is(':hidden');
            });

        function enterKey() {
            let key = e.charCode || e.keyCode || 0 //get the key code

            if (key == 13 && !e.shiftKey) { //If enter key
                e.preventDefault();
                var code = e.which,
                    elm = document.activeElement, //capture the current element for later
                    currentTabIndex = elm.tabIndex,
                    nextTabIndex = code == 13 ? currentTabIndex + 1 : null
                $('[tabindex]').filter(function () {
                    nextTabIndex = nextTabIndex - getFocusBookNumber
                    return this.tabIndex == nextTabIndex;
                }).focus();
                getFocusBookNumber = 0;
            } else if (key == 13 && e.shiftKey) { //If enter key
                e.preventDefault();

                var code = e.which,

                    elm = document.activeElement, //capture the current element for later
                    currentTabIndex = elm.tabIndex,

                    nextTabIndex = code == 13 ? currentTabIndex - 1 : null
                $('[tabindex]').filter(function () {
                    console.log("key " + currentTabIndex);
                    return this.tabIndex == nextTabIndex;
                }).focus();
            }

        }

        // We need to capture the [Shift] key and check the [Enter] key either way.
        if (e.shiftKey) {
            enterKey()
        } else {
            enterKey()
        }

        if (e.altKey && e.keyCode == 83) {
            $("#voucherForm").submit();
            //location.reload();
        }
    });
    calculateTotal();
})