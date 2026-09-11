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
    var formatterNoFloat = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 0, maximumFractionDigits: 0});
    var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 2, maximumFractionDigits: 2});
    // var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 2, maximumFractionDigits: 2});

    $(".select2_single").on("select2:close", function () {
        $(this).focus();
    });
    $("#details").on("click", function (event) {
        $("#voucherEnrtiesList").toggle();
        event.preventDefault();
    })
    $('select#company option:not(:selected)').attr('disabled', true);
    $('select#branch option:not(:selected)').attr('disabled', true);

    $("#voucher_entry_table tbody").find("tr:eq(0) .voucherEntry_account:eq(0)").val($("#itemStock\\.account\\.code").val());
    $("#voucher_entry_table tbody").find("tr:eq(0) .voucherEntry_account:eq(0)").closest("td").find("span").text($("#itemStock\\.account\\.code").find("option:selected").text());

    // $("#itemStock\\.account\\.code").val(0).change();

    $("#voucherStatus").on("change", function () {
        if ($("#voucherStatus").val() == "M") {
            $("#itemStock\\.salesTax").val("0.00");
            $("#itemStock\\.furtherSalesTax").val("0.00");
            $("#itemStock\\.advanceIncomeTax").val("0.00");
        }
    });

    // change account
    $("#itemStock\\.account\\.code").on("change", function () {
        $("#voucher_entry_table tbody").find("tr:eq(0) .voucherEntry_account:eq(0)").val($(this).val());
        $("#voucher_entry_table tbody").find("tr:eq(0) .voucherEntry_account:eq(0)").closest("td").find("span").text($(this).find("option:selected").text());
    });

    // change account
    $("#brokerAccount").on("change", function () {
        $("#voucher_entry_table tbody").find("tr:eq(9) .voucherEntry_account:eq(0)").val($(this).val());
        $("#voucher_entry_table tbody").find("tr:eq(9) .voucherEntry_account:eq(0)").closest("td").find("span").text($(this).find("option:selected").text());
    });

//
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
        $("#sale_entry_table .select2_single").select2({dropdownAutoWidth: true, width: '100%'});

        // update serial numbers
        $("#sale_entry_table span#serial_no").each(function (index, element) {
            $(element).text(index + 1);

            $(element).closest("tr").find("input[name*='.id']").attr("id", "itemStock.itemStockEntries" + index + ".id");
            $(element).closest("tr").find("input[name*='.id']").attr("name", "itemStock.itemStockEntries[" + index + "].id");


            $(element).closest("tr").find("select[name*='.itemDef']").attr("id", "itemStock.itemStockEntries" + index + ".itemDef");
            $(element).closest("tr").find("select[name*='.itemDef']").attr("name", "itemStock.itemStockEntries[" + index + "].itemDef");

            $(element).closest("tr").find("input[name*='.itemQuantity']").attr("id", "itemStock.itemStockEntries" + index + ".itemQuantity");
            $(element).closest("tr").find("input[name*='.itemQuantity']").attr("name", "itemStock.itemStockEntries[" + index + "].itemQuantity");

            $(element).closest("tr").find("input[name*='.saleRate']").attr("id", "itemStock.itemStockEntries" + index + ".saleRate");
            $(element).closest("tr").find("input[name*='.saleRate']").attr("name", "itemStock.itemStockEntries[" + index + "].saleRate");

            $(element).closest("tr").find("input[name*='.totalKg']").attr("id", "itemStock.itemStockEntries" + index + ".totalKg");
            $(element).closest("tr").find("input[name*='.totalKg']").attr("name", "itemStock.itemStockEntries[" + index + "].totalKg");
            $(element).closest("tr").find("input[name*='.bags']").attr("id", "itemStock.itemStockEntries" + index + ".bags");
            $(element).closest("tr").find("input[name*='.bags']").attr("name", "itemStock.itemStockEntries[" + index + "].bags");

            $(element).closest("tr").find("input[name*='.amount']").attr("id", "itemStock.itemStockEntries" + index + ".amount");
            $(element).closest("tr").find("input[name*='.amount']").attr("name", "itemStock.itemStockEntries[" + index + "].amount");

        });

        generateTabIndexing();
        calculateTotalSjv();
        // calculateTotalPjv();

    });

    $(".entry_delete").on("click", function () {

        if ($(this).parents("tbody").find("tr").length > 1) {
            $(this).closest("tr").remove();
        }

        // update serial numbers
        $("#sale_entry_table span#serial_no").each(function (index, element) {
            $(element).text(index + 1);

            $(element).closest("tr").find("input[name*='.id']").attr("id", "itemStock.itemStockEntries" + index + ".id");
            $(element).closest("tr").find("input[name*='.id']").attr("name", "itemStock.itemStockEntries[" + index + "].id");


            $(element).closest("tr").find("select[name*='.itemDef']").attr("id", "itemStock.itemStockEntries" + index + ".itemDef");
            $(element).closest("tr").find("select[name*='.itemDef']").attr("name", "itemStock.itemStockEntries[" + index + "].itemDef");

            $(element).closest("tr").find("input[name*='.itemQuantity']").attr("id", "itemStock.itemStockEntries" + index + ".itemQuantity");
            $(element).closest("tr").find("input[name*='.itemQuantity']").attr("name", "itemStock.itemStockEntries[" + index + "].itemQuantity");

            $(element).closest("tr").find("input[name*='.saleRate']").attr("id", "itemStock.itemStockEntries" + index + ".saleRate");
            $(element).closest("tr").find("input[name*='.saleRate']").attr("name", "itemStock.itemStockEntries[" + index + "].saleRate");

            $(element).closest("tr").find("input[name*='.totalKg']").attr("id", "itemStock.itemStockEntries" + index + ".totalKg");
            $(element).closest("tr").find("input[name*='.totalKg']").attr("name", "itemStock.itemStockEntries[" + index + "].totalKg");

            $(element).closest("tr").find("input[name*='.bags']").attr("id", "itemStock.itemStockEntries" + index + ".bags");
            $(element).closest("tr").find("input[name*='.bags']").attr("name", "itemStock.itemStockEntries[" + index + "].bags");

            $(element).closest("tr").find("input[name*='.amount']").attr("id", "itemStock.itemStockEntries" + index + ".amount");
            $(element).closest("tr").find("input[name*='.amount']").attr("name", "itemStock.itemStockEntries[" + index + "].amount");

        });

        generateTabIndexing();
        calculateTotalSjv();
        // calculateTotalPjv();
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

    $("#itemStock\\.millTaxRate").on("input", function () {
        calculateAmountEntriesTotal();
    });

    $("#itemStock\\.bankTaxRate").on("input", function () {
        calculateAmountEntriesTotal();
    });

    $("#itemStock\\.silaiRate").on("input", function () {
        calculateAmountEntriesTotal();
    });

    $("#itemStock\\.bardanaRate").on("input", function () {
        calculateAmountEntriesTotal();
    });

    $("#itemStock\\.unloadingRate").on("input", function () {
        calculateAmountEntriesTotal();
    });

    $("#itemStock\\.otherExp").on("input", function () {
        calculateAmountEntriesTotal();
    });

    $("#itemStock\\.freightCharges").on("input", function () {
        calculateAmountEntriesTotal();
    });

    $("#itemStock\\.brokriRate").on("input", function () {
        calculateAmountEntriesTotal();
    });

    $("#itemStock\\.brokriAmount").blur(function () {
        var amount = $("#sale_entry_table tfoot tr:eq(0) th:eq(6)").text().replace(/[^0-9\.-]+/g, "");//.replace(/[^0-9\.-]+/g, "")
        //  console.log("totalAmount   ");
        if (parseFloat(amount) == 0)
            alert("Amount is zero");
        else {
            console.log($("#itemStock\\.brokriAmount").val() / amount);
            $("#itemStock\\.brokriRate").val(($("#itemStock\\.brokriAmount").val() / amount).toFixed(8));
            calculateTotal();
        }
    });

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
        });

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

        var creditCharges = parseFloat(0);
        var debitCharges = parseFloat(0);
        var qtyInMan = parseFloat(0);
        var invoiceAmountTotal = parseFloat(0);
        var barokiRate = parseFloat(0);
        var brokriAmount = parseFloat(0);

        var totalAmount = parseFloat(0);
        var saleRate = parseFloat(0);
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
        $("#sale_entry_table tbody tr").each(function () {
            $(this).find(".bags").val($(this).find(".itemQuantity").val());
            itemQty = itemQty + parseFloat($(this).find(".itemQuantity").val());
            totalKg = totalKg + parseFloat($(this).find(".totalKg").val());
            invoiceAmountTotal = invoiceAmountTotal + parseFloat($(this).find(".itemQuantity").val()) * parseFloat($(this).find(".saleRate").val()).toFixed(2);
        });
        bags = $("#sale_entry_table tfoot tr:eq(0) th:eq(5)").text(); //$("#itemStock\\.itemStockEntries0\\.bags").val();
        barokiRate = $("#itemStock\\.brokriRate").val();
        brokriAmount = $("#itemStock\\.brokriAmount").val();
        totalAmount = $("#itemStock\\.totalAmount").val();
        // saleRate = $("#itemStock\\.itemStockEntries0\\.saleRate").val();
        // saleAmount = $("#itemStock\\.itemStockEntries0\\.amount").val();
        saleAmount = invoiceAmountTotal;//$("#sale_entry_table tfoot tr:eq(0) th:eq(6)").text();
        console.log("Sale Amount " + saleAmount);
        // $("#sale_entry_table tbody tr").each(function () {
        //     saleAmount = saleAmount + parseFloat($(this).find(".amount").val() || 0);
        // });

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

        deduction = 0;
        bagWeightDeduction = 0;
        totalBagWeightDeduction = 0;
        otherBagWeight = 0;


        //$("#itemStock\\.itemStockEntries0\\.amount").val(formatter.format(invoiceAmountTotal));

        $("#itemStock\\.bankTaxAmount").val(formatterNoFloat.format(invoiceAmountTotal * bankTaxRate / 100));

        $("#itemStock\\.millTaxAmount").val(formatterNoFloat.format(itemQty * millTaxRate));

        $("#itemStock\\.silaiAmount").val(formatterNoFloat.format(bags * silaiRate));

        $("#itemStock\\.bardanaAmount").val(formatterNoFloat.format(bags * bardanaRate));

        $("#itemStock\\.unloadingCharges").val(formatterNoFloat.format(bags * unloadingRate));

        $("#itemStock\\.brokriAmount").val((invoiceAmountTotal * barokiRate).toFixed(0));

        brokriAmount = $("#itemStock\\.brokriAmount").val();
        bankTaxAmount = $("#itemStock\\.bankTaxAmount").val();
        millTaxAmount = $("#itemStock\\.millTaxAmount").val();
        bardanaAmount = $("#itemStock\\.bardanaAmount").val();
        silaiAmount = $("#itemStock\\.silaiAmount").val();
        unloadingAmount = $("#itemStock\\.unloadingCharges").val();
        otherExp = $("#itemStock\\.otherExp").val();
        freightCharges = $("#itemStock\\.freightCharges").val();

        $("#itemStock\\.otherExp").val(formatterNoFloat.format(otherExp).replace(/[^0-9\.-]+/g, ""));
        $("#itemStock\\.freightCharges").val(formatterNoFloat.format(freightCharges).replace(/[^0-9\.-]+/g, ""));


        // saleAmount = $("#itemStock\\.itemStockEntries0\\.amount").val();
        /*creditCharges = parseFloat(bankTaxAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(millTaxAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(freightCharges.replace(/[^0-9\.-]+/g, ""));
        debitCharges = parseFloat(bardanaAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(silaiAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(otherExp.replace(/[^0-9\.-]+/g, "")) + parseFloat(unloadingAmount.replace(/[^0-9\.-]+/g, ""));*/
        if (otherExp > 0) {
            creditCharges = parseFloat(bankTaxAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(millTaxAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(freightCharges.replace(/[^0-9\.-]+/g, ""));
            debitCharges = parseFloat(bardanaAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(silaiAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(otherExp.replace(/[^0-9\.-]+/g, "")) + parseFloat(unloadingAmount);
        } else {
            creditCharges = parseFloat(bankTaxAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(millTaxAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(freightCharges.replace(/[^0-9\.-]+/g, ""));
            debitCharges = -parseFloat(otherExp.replace(/[^0-9\.-]+/g, "")) + parseFloat(bardanaAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(silaiAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(unloadingAmount);

        }
        // console.log(saleAmount.replace(/[^0-9\.-]+/g, ""));
        $("#itemStock\\.netAmount").val(formatter.format(saleAmount - creditCharges + debitCharges));

        $("#voucher_entry_table tbody tr:eq(0) td:eq(4) input").val(parseFloat(saleAmount) - creditCharges + debitCharges);
        $("#voucher_entry_table tbody tr:eq(1) td:eq(5) input").val(parseFloat(saleAmount));
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
        //   check other expenses are negative or
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
        $("#voucher_entry_table tbody tr:eq(9) td:eq(3) textarea").text("BROKERY");
        $("#voucher_entry_table tbody tr:eq(10) td:eq(4) input").val(brokriAmount.replace(/[^0-9\.-]+/g, ""));
        $("#voucher_entry_table tbody tr:eq(10) td:eq(5) input").val(0);
        $("#voucher_entry_table tbody tr:eq(10) td:eq(3) textarea").text("BROKERY");
    }

    $(".itemDef").on("change", function () {
        loadItemDefDetail($(this));
    });

    function loadItemDefDetail(thisControl) {

        if (!$(thisControl).val() || $(thisControl).val() == 0) {
            return;
        }

        if (!$("#company").val() || !$("#branch").val()) {
            return;
        }

        $.get("/receivables/item_def_detail?itemDefId=" + $(thisControl).val() + "&companyId=" + $("#company").val() + "&branchId=" + $("#branch").val() + "&voucherStatusId='0'", function (data) {
            //$(thisControl).closest("tr").find(".availableQuantity").text(data.availableQuantity);
//	    	$(thisControl).closest("tr").find(".standardPrice").text(data.standardRate);
//	    	$(thisControl).closest("tr").find(".unitWeight").val(data.weight);
//	    	$(thisControl).closest("tr").find(".feet").text(data.feet);
            $(thisControl).closest("tr").find(".pricingRule").val(data.pricingRule);
            $("#pricingRule").val(data.pricingRule);
            $("#weightParBag").val(data.weightParBag);
            calculateTotalSjv();
            //calculateTotalPjv();
        });
    }

    var checkboxClick = "true";
    calculateTotalSjv();
    //calculateTotalPjv();


    // $("#prucahse_entry_table").on("input", "input", function () {
    //     calculateTotalSjv();
    //     calculateTotalPjv();
    // });
    $("#sale_entry_table").on("input", "input", function () {
        calculateTotalSjv();
        // calculateTotalPjv();
    });

    var totalKgUseForProduction = parseFloat(0);

    function calculateTotalSjv() {
        totalKgUseForProduction = parseFloat(0);
        var amountTotal = parseFloat(0);
        var bagsTotal = parseFloat(0);
        var itemRate = parseFloat(0);
        var safiKg = parseFloat(0);
        var quantityTotalInMon = parseFloat(0);
        var quantity = parseFloat(0);
        console.log("I M IN Total SJV");
        $("#sale_entry_table tbody tr").each(function () {
            $(this).find(".bags").val($(this).find(".itemQuantity").val());
            $(this).find(".totalKg").val(parseFloat($(this).find(".itemQuantity").val()) * parseFloat($("#weightParBag").val()));
            itemRate = parseFloat($(this).find(".saleRate").val() || 0);
            safiKg = parseFloat($(this).find(".totalKg").val() || 0);
            totalKgUseForProduction = totalKgUseForProduction + safiKg;
            // quantity = (safiKg / 40).toFixed(2);


            // $(this).find(".itemQuantity").val(quantity);

            console.log("SAFI KG " + safiKg + " Quantity " + quantity + " item quantity " + parseFloat($(this).find(".itemQuantity").val() || 0));
            quantityTotalInMon = quantityTotalInMon + parseFloat($(this).find(".itemQuantity").val() || 0);
            bagsTotal = bagsTotal + parseFloat($(this).find(".bags").val() || 0);
            $(this).find(".amount").text(parseFloat($(this).find(".itemQuantity").val() * itemRate).toFixed(2));
            amountTotal = amountTotal + parseFloat($(this).find(".itemQuantity").val() * itemRate).toFixed(2);
            // $(this).find(".percentage").val(calcPercentage(safiKg));
        });
        $("#sale_entry_table tfoot tr:eq(0) th:eq(4)").text(totalKgUseForProduction);
        $("#sale_entry_table tfoot tr:eq(0) th:eq(2)").text(quantityTotalInMon);
        //  $("#sale_entry_table tfoot tr:eq(0) th:eq(5)").text(bagsTotal);
        $("#sale_entry_table tfoot tr:eq(0) th:eq(5)").text(amountTotal);

        calculateAmountEntriesTotal();
    }

    $("#sale_entry_table").on("input", "input", function () {
        calculateTotalSjv();
    });

    function calcPercentage(productionWeight) {
        var safiKg = parseFloat(0);
        console.log("safiKg:" + safiKg);
        $("#sale_entry_table tbody tr").each(function () {
            safiKg = safiKg + parseFloat($(this).find(".safiKg").val() || 0);
        });
        return ((productionWeight / safiKg) * 100).toFixed(0);
    }

    function calcPercentage40(productionWeight) {
        var safiKg = parseFloat(0);
        console.log("safiKg:" + safiKg);
        $("#sale_entry_table tbody tr").each(function () {
            safiKg = safiKg + parseFloat($(this).find(".safiKg").val() || 0);
        });
        return ((productionWeight / safiKg) * 40).toFixed(0);
    }

    $("#submitForm").off().on("click", function (event) {
        $("#submitForm").addClass('disabled');
        setTimeout(function () {
            $("#submitForm").removeClass('disabled');
        }, 10000);
        //console.log(${itemStock.itemStockEntries});
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
        var form = document.getElementById("voucherForm");
        var formData = new FormData(form);

        // Create an object to store the form data
        var voucherData = {};

        // Iterate over the FormData entries and store them in the object
        for (var pair of formData.entries()) {
            voucherData[pair[0]] = pair[1];
        }

        // Now you have the voucher data in the voucherData object
        // You can use it as needed, for example, send it to a server via AJAX

        // Example: Logging the voucher data to the console
        console.log(voucherData);
        var itemEmpty = false;
        var itemAmountZero = false;
        var itemDuplicateExists = false;
        var itemDuplicate = "";
        var itemArray = [];
        $("#sale_entry_table tbody tr").each(function () {
            if (!$(this).find(".itemDef").val() || $(this).find(".itemDef").val() == 0) {
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
        $("#prucahse_entry_table tbody td").each(function (i) {
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
            if (e.which === 13 && !self.is('textarea,div[contenteditable=true]')) { // [Enter] key
                // If not a regular hyperlink/button/textarea
                if ($.inArray(self, focusable) && (!self.is('button'))) {
                    // Then prevent the default [Enter] key behavior from submitting the form
                    e.preventDefault();
                } // Otherwise, follow the link/button as by design, or put new line in textarea

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