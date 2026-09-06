function getFormData($form) {
    var unindexed_array = $form.serializeArray();
    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });

    return indexed_array;
}

//document.mainForm.onclick = function(){
//	console.log()
//    var gender = document.querySelector('input[name = gender]:checked').value;
//    result.innerHTML = 'You Gender: '+gender;
//}
Number.prototype.toFixed = function (decimalPlaces) {
    return Number(Math.round(this + 'e' + decimalPlaces) + 'e-' + decimalPlaces);
}

$(document).ready(function () {

    var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 2, maximumFractionDigits: 2});
    $(".select2_single").on("select2:close", function () {
        $(this).focus();
    });

    $('select#company option:not(:selected)').attr('disabled', true);
    $('select#itemStock\\.branch option:not(:selected)').attr('disabled', true);

    $("#voucherStatus").on("change", function () {
        if ($("#voucherStatus").val() == "M") {
            $("#itemStock\\.salesTax").val("0.00");
            $("#itemStock\\.furtherSalesTax").val("0.00");
            $("#itemStock\\.advanceIncomeTax").val("0.00");
        }
    });

    $(".entry_add").on("click", function () {

        // un-instrument select2 dropdowns
        $(this).closest("tr").find(".select2_single").select2("destroy");

        var newRow = $(this).closest("tr").clone(true);

        //when you use clone, even the text is carried over. To remove it, do the following:
        newRow.children("td").children("input, span").each(function (index, element) {
            $(element).val("0");
            $(element).text("");
        });
        newRow.children("td").each(function (index, element) {
            if ($(this).children().length < 1) {
                $(element).text("");
                $(element).val("");
                //console.log(index+ " element  "+element)
            }
            $(element).find(".itemCategory:first").val();
            $(element).find(".itemDef:first").val("");
        });

        newRow.insertAfter($(this).closest("tr"));

        // again instrument select2 dropdowns
        $("#purchase_order_entry_table .select2_single").select2({dropdownAutoWidth: true, width: '100%'});

        // update serial numbers
        $("#purchase_order_entry_table span#serial_no").each(function (index, element) {
            $(element).text(index + 1);

            $(element).closest("tr").find("input[name*='.id']").attr("id", "itemStock.itemStockEntries" + index + ".id");
            $(element).closest("tr").find("input[name*='.id']").attr("name", "itemStock.itemStockEntries[" + index + "].id");

            $(element).closest("tr").find("select[name*='.itemCategory']").attr("id", "itemStock.itemStockEntries" + index + ".itemCategory");
            $(element).closest("tr").find("select[name*='.itemCategory']").attr("name", "itemStock.itemStockEntries[" + index + "].itemCategory");

            $(element).closest("tr").find("select[name*='.itemDef']").attr("id", "itemStock.itemStockEntries" + index + ".itemDef");
            $(element).closest("tr").find("select[name*='.itemDef']").attr("name", "itemStock.itemStockEntries[" + index + "].itemDef");

            $(element).closest("tr").find("input[name*='.itemQuantity']").attr("id", "itemStock.itemStockEntries" + index + ".itemQuantity");
            $(element).closest("tr").find("input[name*='.itemQuantity']").attr("name", "itemStock.itemStockEntries[" + index + "].itemQuantity");

            $(element).closest("tr").find("input[name*='.price']").attr("id", "itemStock.itemStockEntries" + index + ".price");
            $(element).closest("tr").find("input[name*='.price']").attr("name", "itemStock.itemStockEntries[" + index + "].price");

            $(element).closest("tr").find("input[name*='.tuch']").attr("id", "itemStock.itemStockEntries" + index + ".tuch");
            $(element).closest("tr").find("input[name*='.tuch']").attr("name", "itemStock.itemStockEntries[" + index + "].tuch");

            $(element).closest("tr").find("input[name*='.katoti']").attr("id", "itemStock.itemStockEntries" + index + ".katoti");
            $(element).closest("tr").find("input[name*='.katoti']").attr("name", "itemStock.itemStockEntries[" + index + "].katoti");

            $(element).closest("tr").find("input[name*='.totalKg']").attr("id", "itemStock.itemStockEntries" + index + ".totalKg");
            $(element).closest("tr").find("input[name*='.totalKg']").attr("name", "itemStock.itemStockEntries[" + index + "].totalKg");

            $(element).closest("tr").find("input[name*='.safiKg']").attr("id", "itemStock.itemStockEntries" + index + ".safiKg");
            $(element).closest("tr").find("input[name*='.safiKg']").attr("name", "itemStock.itemStockEntries[" + index + "].safiKg");

            $(element).closest("tr").find("input[name*='.bags']").attr("id", "itemStock.itemStockEntries" + index + ".bags");
            $(element).closest("tr").find("input[name*='.bags']").attr("name", "itemStock.itemStockEntries[" + index + "].bags");

            $(element).closest("tr").find("input[name*='.bagWeight']").attr("id", "itemStock.itemStockEntries" + index + ".bagWeight");
            $(element).closest("tr").find("input[name*='.bagWeight']").attr("name", "itemStock.itemStockEntries[" + index + "].bagWeight");
        });

        generateTabIndexing();
        calculateTotal();

    });

    $(".entry_delete").on("click", function () {

        if ($(this).parents("tbody").find("tr").length > 1) {
            $(this).closest("tr").remove();
        }

        // update serial numbers
        $("#purchase_order_entry_table span#serial_no").each(function (index, element) {
            $(element).text(index + 1);

            $(element).closest("tr").find("input[name*='.id']").attr("id", "itemStock.itemStockEntries" + index + ".id");
            $(element).closest("tr").find("input[name*='.id']").attr("name", "itemStock.itemStockEntries[" + index + "].id");

            $(element).closest("tr").find("select[name*='.itemCategory']").attr("id", "itemStock.itemStockEntries" + index + ".itemCategory");
            $(element).closest("tr").find("select[name*='.itemCategory']").attr("name", "itemStock.itemStockEntries[" + index + "].itemCategory");

            $(element).closest("tr").find("select[name*='.itemDef']").attr("id", "itemStock.itemStockEntries" + index + ".itemDef");
            $(element).closest("tr").find("select[name*='.itemDef']").attr("name", "itemStock.itemStockEntries[" + index + "].itemDef");

            $(element).closest("tr").find("input[name*='.itemQuantity']").attr("id", "itemStock.itemStockEntries" + index + ".itemQuantity");
            $(element).closest("tr").find("input[name*='.itemQuantity']").attr("name", "itemStock.itemStockEntries[" + index + "].itemQuantity");

            $(element).closest("tr").find("input[name*='.price']").attr("id", "itemStock.itemStockEntries" + index + ".price");
            $(element).closest("tr").find("input[name*='.price']").attr("name", "itemStock.itemStockEntries[" + index + "].price");

            $(element).closest("tr").find("input[name*='.tuch']").attr("id", "itemStock.itemStockEntries" + index + ".tuch");
            $(element).closest("tr").find("input[name*='.tuch']").attr("name", "itemStock.itemStockEntries[" + index + "].tuch");

            $(element).closest("tr").find("input[name*='.katoti']").attr("id", "itemStock.itemStockEntries" + index + ".katoti");
            $(element).closest("tr").find("input[name*='.katoti']").attr("name", "itemStock.itemStockEntries[" + index + "].katoti");

            $(element).closest("tr").find("input[name*='.totalKg']").attr("id", "itemStock.itemStockEntries" + index + ".totalKg");
            $(element).closest("tr").find("input[name*='.totalKg']").attr("name", "itemStock.itemStockEntries[" + index + "].totalKg");

            $(element).closest("tr").find("input[name*='.safiKg']").attr("id", "itemStock.itemStockEntries" + index + ".safiKg");
            $(element).closest("tr").find("input[name*='.safiKg']").attr("name", "itemStock.itemStockEntries[" + index + "].safiKg");

            $(element).closest("tr").find("input[name*='.bags']").attr("id", "itemStock.itemStockEntries" + index + ".bags");
            $(element).closest("tr").find("input[name*='.bags']").attr("name", "itemStock.itemStockEntries[" + index + "].bags");

            $(element).closest("tr").find("input[name*='.bagWeight']").attr("id", "itemStock.itemStockEntries" + index + ".bagWeight");
            $(element).closest("tr").find("input[name*='.bagWeight']").attr("name", "itemStock.itemStockEntries[" + index + "].bagWeight");


        });

        generateTabIndexing();
        calculateTotal();
    });


    restrictVoucherDate();

    $("#financialYear").on("change", function () {
        restrictVoucherDate();
    });

    function restrictVoucherDate() {

        if (!$("#financialYear").val()) {
            return;
        }

        $("#voucherDate").attr("min", $("#financialYear option:selected").attr("data-value").split("|")[0]);
        $("#voucherDate").attr("max", $("#financialYear option:selected").attr("data-value").split("|")[1]);
    }

    // calculate total on load
    calculateAmountEntriesTotal();

    function calculateAmountEntriesTotal() {

        var debitTotal = parseFloat(0);
        var creditTotal = parseFloat(0);

        var formatter = new Intl.NumberFormat('ur-PK', {
            style: 'currency',
            currency: 'PKR',
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
        })

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

    /*$("#purchase_order_entry_table tbody tr .itemCategory").each(function() {
        loadItemCategoryDefs($(this));
    });*/

    $(".itemCategory").on("change", function () {
        loadItemCategoryDefs($(this));
    });

    function loadItemCategoryDefs(thisControl) {

        if (!$(thisControl).val() || $(thisControl).val() == 0) {
            return;
        }

        $.get("/receivables/category_item_defs?categoryId=" + $(thisControl).val(), function (data) {
            $(thisControl).closest("tr").find(".itemDef").empty();
            $(thisControl).closest("tr").find(".itemDef").append("<option>&emsp;</option>");

            for (var i = 0, len = data.length; i < len; i++) {
                var option = "<option value = " + data[i].id + ">" + data[i].formattedCode + "&emsp;" + data[i].name + "</option>";
                $(thisControl).closest("tr").find(".itemDef").append(option);
            }
        });
    }

    $(".itemDef").on("change", function () {
        loadItemDefDetail($(this));
    });

    function loadItemDefDetail(thisControl) {

        if (!$(thisControl).val() || $(thisControl).val() == 0) {
            return;
        }

        if (!$("#company").val() || !$("#branch").val() || !$("#voucherStatus").val()) {
            return;
        }

        $.get("/receivables/item_def_detail?itemDefId=" + $(thisControl).val() + "&companyId=" + $("#company").val() + "&branchId=" + $("#branch").val() + "&voucherStatusId=" + $("#voucherStatus").val(), function (data) {
            $(thisControl).closest("tr").find(".availableQuantity").text(data.availableQuantity);
//	    	$(thisControl).closest("tr").find(".standardPrice").text(data.standardRate);
//	    	$(thisControl).closest("tr").find(".unitWeight").val(data.weight);
//	    	$(thisControl).closest("tr").find(".feet").text(data.feet);
//	    	$(thisControl).closest("tr").find(".pricingRule").val(data.pricingRule);
            calculateTotal();
        });
    }

    // change account
    $("#itemStock\\.account\\.code").on("change", function () {
        $("#voucher_entry_table tbody").find("tr:eq(0) .voucherEntry_account:eq(0)").val($(this).val());
        $("#voucher_entry_table tbody").find("tr:eq(0) .voucherEntry_account:eq(0)").closest("td").find("span").text($(this).find("option:selected").text());
        console.log($(".voucherId").val());
        $.get("/payables/account_balance_witout_a_voucher_amount?accountCode=" + $(".select2_single").val(), "voucherCode=" + $(".voucherId").val(), function (data) {


            $("span#previousBalance").text(formatter.format(data.accountBalance));
            //$("span#previousBalanceOfSelectedCustomer").text(formatter.format(data.accountBalance));

            calculateTotal();
        });
    });

//	$("#freightDropDown").on("change", function(){
//		$("#voucher_entry_table tbody").find("tr:eq(3) .voucherEntry_account:eq(0)").val($(this).val());
//		$("#voucher_entry_table tbody").find("tr:eq(3) .voucherEntry_account:eq(0)").closest("td").find("span").text($(this).find("option:selected").text());
//	});

    //calculateTotal();

    var checkboxClick = "true";
    calculateTotal();

    $(".comm").click(function () {
        console.log("aa  ");

        if (checkboxClick === "true")
            checkboxClick = "false";
        else
            checkboxClick = "true";
    });
    $("#purchase_order_entry_table").on("input", "input", function () {
        calculateTotal();
    });
    $("#tax_table").on("input", "input", function () {
        calculateTotal();
    });
    $("#Credit_table").on("input", "input", function () {
        calculateTotal();
    });

    function calculateTotal() {

        var amountTotal = parseFloat(0);
        var standardPrice = parseFloat(0);
        var itemRate = parseFloat(0);
        var bagWeight = parseFloat(0);
        var safiKg = parseFloat(0);
        var totalKg = parseFloat(0);

        var TotalsafiKg = parseFloat(0);
        var AlltotalKg = parseFloat(0);

        var totalBags = parseFloat(0);
        var totalBagsMoreThanOneRow = parseFloat(0);
        var katotiValue = parseFloat(0);
        var tuchValue = parseFloat(0);

        var katoti = parseFloat(0);
        var tuch = parseFloat(0);

        var TotalkatotiValue = parseFloat(0);
        var TotaltuchValue = parseFloat(0);
        var quantityTotalInMon = parseFloat(0);

        var quantityTotal = parseFloat(0);
        var totalWeightInKg = parseFloat(0);
        $("#purchase_order_entry_table tbody tr").each(function () {

            // console.log("aa  "+checkboxClick);

            itemRate = parseFloat($(this).find(".price").val() || 0);
            katoti = parseFloat($(this).find(".katoti").val() || 0);
            tuch = parseFloat($(this).find(".tuch").val() || 0);
            totalBags = parseFloat($(this).find(".bags").val() || 0);
            bagWeight = parseFloat($(this).find(".bagWeight").val() || 0);
            quantityTotalInMon = parseFloat($(this).find(".itemQuantity").val() || 0);

            totalKg = parseFloat($(this).find(".totalKg").val() || 0);

            katotiValue = katoti * totalBags;
            tuchValue = tuch * totalBags;

            if (checkboxClick === "true") {

                $(this).find(".safiKg").val((totalKg - ((katotiValue + tuchValue))).toFixed(2));
                safiKg = parseFloat($(this).find(".safiKg").val() || 0);

                $(this).find(".itemQuantity").val((safiKg / 40).toFixed(3));
                quantityTotalInMon = parseFloat($(this).find(".itemQuantity").val() || 0);
                $(this).find(".amount").text((quantityTotalInMon * itemRate).toFixed(2));
            } else {
                $(this).find(".amount").text((quantityTotalInMon * itemRate).toFixed(2));
                $(this).find(".totalKg").val("0");
                $(this).find(".safiKg").val("0");
                $(this).find(".katoti").val("0");
                $(this).find(".tuch").val("0");
                $(this).find(".bags").val("0");
                $(this).find(".bagWeight").val("0");
            }
            amountTotal = amountTotal + parseFloat($(this).find(".amount").text() || 0);
            quantityTotal = quantityTotal + quantityTotalInMon;

            totalBagsMoreThanOneRow = totalBagsMoreThanOneRow + totalBags;

            TotalkatotiValue = TotalkatotiValue + katotiValue;
            TotaltuchValue = TotaltuchValue + tuchValue;

            TotalsafiKg = TotalsafiKg + safiKg;
            AlltotalKg = AlltotalKg + totalKg;
        });
        console.log(totalBags);
        $("#purchase_order_entry_table tfoot tr:eq(0) th:eq(11)").text(totalBagsMoreThanOneRow);
        $("#purchase_order_entry_table tfoot tr:eq(0) th:eq(7)").text(TotaltuchValue);
        $("#purchase_order_entry_table tfoot tr:eq(0) th:eq(8)").text(TotalkatotiValue);
        $("#purchase_order_entry_table tfoot tr:eq(0) th:eq(9)").text(AlltotalKg);
        $("#purchase_order_entry_table tfoot tr:eq(0) th:eq(10)").text(TotalsafiKg);
        $("#purchase_order_entry_table tfoot tr:eq(0) th:eq(13)").text(formatter.format(amountTotal));

        $("#purchase_order_entry_table tfoot tr:eq(0) th:eq(4)").text(quantityTotal);

        var loadingCharges = parseFloat(($("#tax_table tbody tr:eq(0) #itemStock\\.loadingRate").val() * totalBagsMoreThanOneRow).toFixed(1));
        loadingCharges = loadingCharges.toFixed(0);
        $("#tax_table tbody tr:eq(0) th:eq(2)").text(loadingCharges);

        var freightChargesCredit = parseFloat(($("#Credit_table tbody tr:eq(0) #itemStock\\.freightRateCredit").val()));
        freightChargesCredit = freightChargesCredit.toFixed(0);
        $("#Credit_table tbody tr:eq(0) th:eq(2)").text(freightChargesCredit);

        var freightCharges = parseFloat(($("#tax_table tbody tr:eq(1) #itemStock\\.freightRate").val()));
        freightCharges = freightCharges.toFixed(0);
        $("#tax_table tbody tr:eq(1) th:eq(2)").text(freightCharges);

        var barDanaCreditAmount = parseFloat(($("#Credit_table tbody tr:eq(1) #itemStock\\.barDanaCreditRate").val() * totalBagsMoreThanOneRow).toFixed(0));
        barDanaCreditAmount = barDanaCreditAmount.toFixed(0);
        $("#Credit_table tbody tr:eq(1) th:eq(2)").text(barDanaCreditAmount.toFixed(0));

        var netAmount = freightChargesCredit + amountTotal - freightCharges + barDanaCreditAmount;
        $("#tax_table tbody tr:eq(2) th:eq(2)").text(formatter.format(netAmount));

        var salesTaxAmount = parseFloat(calcPercentage(amountTotal, $("#tax_table tbody tr:eq(3) #itemStock\\.salesTax").val()).toFixed(2));
        salesTaxAmount = parseFloat((salesTaxAmount).toFixed(2));
        $("#tax_table tbody tr:eq(3) th:eq(2)").text(formatter.format(salesTaxAmount));

        var commissionAmount = parseFloat(calcPercentage(amountTotal, $("#tax_table tbody tr:eq(4) #itemStock\\.commission").val()).toFixed(0));
        commissionAmount = parseFloat((commissionAmount).toFixed(2));
        $("#tax_table tbody tr:eq(4) th:eq(2)").text(formatter.format(commissionAmount));

        var bardanaDebitAmount = parseFloat(($("#tax_table tbody tr:eq(5) #itemStock\\.bardanaDebit").val() * totalBagsMoreThanOneRow).toFixed(0));
        bardanaDebitAmount = parseFloat((bardanaDebitAmount).toFixed(2));
        $("#tax_table tbody tr:eq(5) th:eq(2)").text(formatter.format(bardanaDebitAmount));

        var Totalamount = netAmount + salesTaxAmount + commissionAmount - bardanaDebitAmount;
        var invoiceAmount = Totalamount;
        Totalamount = parseFloat((Totalamount).toFixed(2));
        $("#tax_table tbody tr:eq(6) th:eq(2)").text(formatter.format(Totalamount));

        var previousBalance = parseFloat($("span#previousBalance").text().replace(/[\$,]/g, '') || 0).toFixed(2);
        Totalamount = -Totalamount + previousBalance;
        //$("#tax_table tbody tr:eq(7) th:eq(2)").text(formatter.format(previousBalance));
        $("#tax_table tbody tr:eq(8) th:eq(2)").text(formatter.format(Totalamount));

        $("#voucher_entry_table tbody tr:eq(0) td:eq(5) input").val(parseFloat(invoiceAmount).toFixed(2));
        $("#voucher_entry_table tbody tr:eq(1) td:eq(4) input").val(amountTotal + loadingCharges);
        $("#voucher_entry_table tbody tr:eq(2) td:eq(5) input").val(loadingCharges);
        $("#voucher_entry_table tbody tr:eq(3) td:eq(5) input").val(freightCharges);
        $("#voucher_entry_table tbody tr:eq(3) td:eq(4) input").val(freightChargesCredit);
        $("#voucher_entry_table tbody tr:eq(4) td:eq(4) input").val(salesTaxAmount);
        $("#voucher_entry_table tbody tr:eq(5) td:eq(4) input").val(commissionAmount);
        $("#voucher_entry_table tbody tr:eq(6) td:eq(5) input").val(bardanaDebitAmount);
        $("#voucher_entry_table tbody tr:eq(6) td:eq(4) input").val(barDanaCreditAmount);

        calculateAmountEntriesTotal();

    }


    function calcPercentage(amount, percentage) {
        return ((amount * percentage) / 100).toFixed(4);
    }

    $(".unloadingRate").on("input", function () {
        $(".unloadingCharges").val(($(".unloadingRate").val() * $("#purchase_order_entry_table tfoot tr:eq(9) th:eq(1)").text()).toFixed(2));
    });

    /*$("#calculateForm").on("click", function (event) {
        event.preventDefault();
        $("#voucherForm").attr("action", $(location).attr("pathname")).submit();
    });
    $("#submitForm").on("click", function (event) {
        event.preventDefault();
        $("#voucherForm").attr("action", "/sale_journal_voucher/add_or_update_voucher").submit();
    });*/

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
                content: "PLEASE SELECT CUSTOMER ACCOUNT",
                type: 'red',
                typeAnimated: true,
            });
            // Prevent the form from submitting via the browser.
            return false;
        }

        var itemEmpty = false;
        var itemAmountZero = false;
        var itemDuplicateExists = false;
        var itemDuplicate = "";
        var itemArray = [];
        $("#purchase_order_entry_table tbody tr").each(function () {
            if (!$(this).find(".itemCategory").val() || $(this).find(".itemCategory").val() == 0 || !$(this).find(".itemDef").val() || $(this).find(".itemDef").val() == 0) {
                itemEmpty = true;
            }

            if (parseFloat($(this).find(".amount").text()) == 0) {
                itemAmountZero = true;
            }

            if ($(this).find(".itemDef").val() && $.inArray($(this).find(".itemDef").val(), itemArray) >= 0) {
                itemDuplicateExists = true;
                itemDuplicate = $(this).find(".itemDef option:selected").text();
            } else {
                itemArray.push($(this).find(".itemDef").val());
            }
        });

        if (itemEmpty) {
            $.confirm({
                title: "ENCOUNTERED AN ERROR!",
                content: "PLEASE SELECT ITEM CAT/DEF FOR EACH ROW",
                type: 'red',
                typeAnimated: true,
            });
            // Prevent the form from submitting via the browser.
            return false;
        }

        if (itemAmountZero) {
            $.confirm({
                title: "ENCOUNTERED AN ERROR!",
                content: "ITEM AMOUNT MUST BE GREATER THEN ZERO",
                type: 'red',
                typeAnimated: true,
            });
            // Prevent the form from submitting via the browser.
            return false;
        }

        if (itemDuplicateExists) {
            $.confirm({
                title: "ENCOUNTERED AN ERROR!",
                content: "DUPLICATE ITEM EXISTS: " + itemDuplicate,
                type: 'red',
                typeAnimated: true,
            });
            // Prevent the form from submitting via the browser.
            return false;
        }

        // prevent double submit
        if ($("#voucherForm").data("submitted")) {
            return false;
        } else {
            $("#voucherForm").data("submitted", true);
        }
    });


    generateTabIndexing();

    function generateTabIndexing() {
        var tabIndex = 0;
        $("#purchase_order_entry_table tbody td").each(function (i) {
            if ($(this).find("select.select2_single").length) {
                tabIndex = tabIndex + 1;
                $(this).find("select.select2_single:eq(0)").attr('tabindex', tabIndex);

            } else if ($(this).find("input:not(:hidden)").length) {
                tabIndex = tabIndex + 1;
                $(this).find("input:not(:hidden):eq(0)").attr('tabindex', tabIndex);

            } else if ($(this).find("a").length) {
                tabIndex = tabIndex + 1;
                $(this).find("a:eq(0)").attr('tabindex', tabIndex);

            }
        });
    }

    $("input[type=number]").on("focus", function () {
        $(this).on("keydown", function (event) {

            var self = $(':focus'), form = self.parents('form:eq(0)'), focusable;  //self getting the current focus

            if (event.keyCode === 38 || event.keyCode === 40) {
//	        	
//	        	 focusable = form.find(':input:enabled:not([readonly], input:hidden, button:hidden, textarea:hidden), textarea:enabled:not([readonly] textarea:hidden), a.entry_add')
//		          .not(function () {   // do not include inputs with hidden parents
//		              return $(this).parent().is(':hidden');
//		          });
//	        	  focusable.eq(focusable.index(self) + 8).focus();
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

        // Set self as the current item in focus
        var self = $(':focus'),
            // Set the form by the current item in focus
            form = self.parents('form:eq(0)'),
            focusable;
        console.log("upkey " + e.which);
        // Array of Indexable/Tab-able items
        //focusable = form.find('input').filter(':visible');
        focusable = form.find(':input:enabled:not([readonly], input:hidden, button:hidden, textarea:hidden), textarea:enabled:not([readonly] textarea:hidden), a.entry_add')
            .not(function () {   // do not include inputs with hidden parents
                return $(this).parent().is(':hidden');
            });

        function enterKey() {
            if (e.which === 13 && !self.is('textarea,div[contenteditable=true]')) { // [Enter] key

                // If not a regular hyperlink/button/textarea
                if ($.inArray(self, focusable) && (!self.is('button'))) {
                    // Then prevent the default [Enter] key behaviour from submitting the form
                    e.preventDefault();
                } // Otherwise follow the link/button as by design, or put new line in textarea

                if (self.is('a.entry_add')) {
                    self.click();
                    focusable = form.find(':input:enabled:not([readonly], input:hidden, button:hidden, textarea:hidden), textarea:enabled:not([readonly] textarea:hidden), a.entry_add')
                        .not(function () {   // do not include inputs with hidden parents
                            return $(this).parent().is(':hidden');
                        });
                }

                // Focus on the next item (either previous or next depending on shift)
                if (comingFromSelect2) {
                    focusable.eq(focusable.index(self) + (e.shiftKey ? -1 : 0)).focus();
                    console.log(" index " + focusable.index(self));
                    comingFromSelect2 = false;
                } else {
                    console.log(" else " + focusable.index(self));
                    if (focusable.index(self) < 0) {
                        return false;
                    }
                    focusable.eq(focusable.index(self) + (e.shiftKey ? -1 : 1)).focus();
                }
                //focusable.eq(focusable.index(self) + (e.shiftKey ? -1 : 1)).focus();

                return false;
            }
        }

        // We need to capture the [Shift] key and check the [Enter] key either way.
        if (e.shiftKey) {
            enterKey()
        } else {
            enterKey()
        }
    });

});