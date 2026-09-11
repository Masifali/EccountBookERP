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
//
    $(".sjv_entry_add").on("click", function () {

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

            $(element).closest("tr").find("input[name*='.id']").attr("id", "sjvVoucher.itemStock.itemStockEntries" + index + ".id");
            $(element).closest("tr").find("input[name*='.id']").attr("name", "sjvVoucher.itemStock.itemStockEntries[" + index + "].id");


            $(element).closest("tr").find("select[name*='.itemDef']").attr("id", "sjvVoucher.itemStock.itemStockEntries" + index + ".itemDef");
            $(element).closest("tr").find("select[name*='.itemDef']").attr("name", "sjvVoucher.itemStock.itemStockEntries[" + index + "].itemDef");

            $(element).closest("tr").find("input[name*='.itemQuantity']").attr("id", "sjvVoucher.itemStock.itemStockEntries" + index + ".itemQuantity");
            $(element).closest("tr").find("input[name*='.itemQuantity']").attr("name", "sjvVoucher.itemStock.itemStockEntries[" + index + "].itemQuantity");

            $(element).closest("tr").find("input[name*='.price']").attr("id", "sjvVoucher.itemStock.itemStockEntries" + index + ".price");
            $(element).closest("tr").find("input[name*='.price']").attr("name", "sjvVoucher.itemStock.itemStockEntries[" + index + "].price");

            $(element).closest("tr").find("input[name*='.itemQuantity']").attr("id", "sjvVoucher.itemStock.itemStockEntries" + index + ".itemQuantity");
            $(element).closest("tr").find("input[name*='.itemQuantity']").attr("name", "sjvVoucher.itemStock.itemStockEntries[" + index + "].itemQuantity");

            $(element).closest("tr").find("input[name*='.safiKg']").attr("id", "sjvVoucher.itemStock.itemStockEntries" + index + ".safiKg");
            $(element).closest("tr").find("input[name*='.safiKg']").attr("name", "sjvVoucher.itemStock.itemStockEntries[" + index + "].safiKg");

        });

        generateTabIndexing();
        calculateTotalSjv();
        // calculateTotalPjv();

    });

    $(".sjv_entry_delete").on("click", function () {

        if ($(this).parents("tbody").find("tr").length > 1) {
            $(this).closest("tr").remove();
        }

        // update serial numbers
        $("#sale_entry_table span#serial_no").each(function (index, element) {
            $(element).text(index + 1);

            $(element).closest("tr").find("input[name*='.id']").attr("id", "sjvVoucher.itemStock.itemStockEntries" + index + ".id");
            $(element).closest("tr").find("input[name*='.id']").attr("name", "sjvVoucher.itemStock.itemStockEntries[" + index + "].id");


            $(element).closest("tr").find("select[name*='.itemDef']").attr("id", "sjvVoucher.itemStock.itemStockEntries" + index + ".itemDef");
            $(element).closest("tr").find("select[name*='.itemDef']").attr("name", "sjvVoucher.itemStock.itemStockEntries[" + index + "].itemDef");

            $(element).closest("tr").find("input[name*='.itemQuantity']").attr("id", "sjvVoucher.itemStock.itemStockEntries" + index + ".itemQuantity");
            $(element).closest("tr").find("input[name*='.itemQuantity']").attr("name", "sjvVoucher.itemStock.itemStockEntries[" + index + "].itemQuantity");

            $(element).closest("tr").find("input[name*='.price']").attr("id", "sjvVoucher.itemStock.itemStockEntries" + index + ".price");
            $(element).closest("tr").find("input[name*='.price']").attr("name", "sjvVoucher.itemStock.itemStockEntries[" + index + "].price");

            $(element).closest("tr").find("input[name*='.itemQuantity']").attr("id", "sjvVoucher.itemStock.itemStockEntries" + index + ".itemQuantity");
            $(element).closest("tr").find("input[name*='.itemQuantity']").attr("name", "sjvVoucher.itemStock.itemStockEntries[" + index + "].itemQuantity");

            $(element).closest("tr").find("input[name*='.safiKg']").attr("id", "sjvVoucher.itemStock.itemStockEntries" + index + ".safiKg");
            $(element).closest("tr").find("input[name*='.safiKg']").attr("name", "sjvVoucher.itemStock.itemStockEntries[" + index + "].safiKg");


        });

        generateTabIndexing();
        calculateTotalSjv();
        // calculateTotalPjv();
    });


    $(".pjv_entry_add").on("click", function () {

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
        $("#prucahse_entry_table .select2_single").select2({dropdownAutoWidth: true, width: '100%'});

        // update serial numbers
        $("#prucahse_entry_table span#serial_no").each(function (index, element) {
            $(element).text(index + 1);

            $(element).closest("tr").find("input[name*='.id']").attr("id", "pjvVoucher.itemStock.itemStockEntries" + index + ".id");
            $(element).closest("tr").find("input[name*='.id']").attr("name", "pjvVoucher.itemStock.itemStockEntries[" + index + "].id");


            $(element).closest("tr").find("select[name*='.itemDef']").attr("id", "pjvVoucher.itemStock.itemStockEntries" + index + ".itemDef");
            $(element).closest("tr").find("select[name*='.itemDef']").attr("name", "pjvVoucher.itemStock.itemStockEntries[" + index + "].itemDef");

            $(element).closest("tr").find("input[name*='.itemQuantity']").attr("id", "pjvVoucher.itemStock.itemStockEntries" + index + ".itemQuantity");
            $(element).closest("tr").find("input[name*='.itemQuantity']").attr("name", "pjvVoucher.itemStock.itemStockEntries[" + index + "].itemQuantity");

            $(element).closest("tr").find("input[name*='.price']").attr("id", "pjvVoucher.itemStock.itemStockEntries" + index + ".price");
            $(element).closest("tr").find("input[name*='.price']").attr("name", "pjvVoucher.itemStock.itemStockEntries[" + index + "].price");

            $(element).closest("tr").find("input[name*='.itemQuantity']").attr("id", "pjvVoucher.itemStock.itemStockEntries" + index + ".itemQuantity");
            $(element).closest("tr").find("input[name*='.itemQuantity']").attr("name", "pjvVoucher.itemStock.itemStockEntries[" + index + "].itemQuantity");

            $(element).closest("tr").find("input[name*='.safiKg']").attr("id", "pjvVoucher.itemStock.itemStockEntries" + index + ".safiKg");
            $(element).closest("tr").find("input[name*='.safiKg']").attr("name", "pjvVoucher.itemStock.itemStockEntries[" + index + "].safiKg");

            $(element).closest("tr").find("input[name*='.percentage']").attr("id", "pjvVoucher.itemStock.itemStockEntries" + index + ".percentage");
            $(element).closest("tr").find("input[name*='.percentage']").attr("name", "pjvVoucher.itemStock.itemStockEntries[" + index + "].percentage");

            $(element).closest("tr").find("input[name*='.percentage40']").attr("id", "pjvVoucher.itemStock.itemStockEntries" + index + ".percentage40");
            $(element).closest("tr").find("input[name*='.percentage40']").attr("name", "pjvVoucher.itemStock.itemStockEntries[" + index + "].percentage40");

        });

        generateTabIndexing();
        // calculateTotalSjv();
        calculateTotalPjv();

    });

    $(".pjv_entry_delete").on("click", function () {

        if ($(this).parents("tbody").find("tr").length > 1) {
            $(this).closest("tr").remove();
        }

        // update serial numbers
        $("#prucahse_entry_table span#serial_no").each(function (index, element) {
            $(element).text(index + 1);

            $(element).closest("tr").find("input[name*='.id']").attr("id", "pjvVoucher.itemStock.itemStockEntries" + index + ".id");
            $(element).closest("tr").find("input[name*='.id']").attr("name", "pjvVoucher.itemStock.itemStockEntries[" + index + "].id");


            $(element).closest("tr").find("select[name*='.itemDef']").attr("id", "pjvVoucher.itemStock.itemStockEntries" + index + ".itemDef");
            $(element).closest("tr").find("select[name*='.itemDef']").attr("name", "pjvVoucher.itemStock.itemStockEntries[" + index + "].itemDef");

            $(element).closest("tr").find("input[name*='.itemQuantity']").attr("id", "pjvVoucher.itemStock.itemStockEntries" + index + ".itemQuantity");
            $(element).closest("tr").find("input[name*='.itemQuantity']").attr("name", "pjvVoucher.itemStock.itemStockEntries[" + index + "].itemQuantity");

            $(element).closest("tr").find("input[name*='.price']").attr("id", "pjvVoucher.itemStock.itemStockEntries" + index + ".price");
            $(element).closest("tr").find("input[name*='.price']").attr("name", "pjvVoucher.itemStock.itemStockEntries[" + index + "].price");

            $(element).closest("tr").find("input[name*='.itemQuantity']").attr("id", "pjvVoucher.itemStock.itemStockEntries" + index + ".itemQuantity");
            $(element).closest("tr").find("input[name*='.itemQuantity']").attr("name", "pjvVoucher.itemStock.itemStockEntries[" + index + "].itemQuantity");

            $(element).closest("tr").find("input[name*='.safiKg']").attr("id", "pjvVoucher.itemStock.itemStockEntries" + index + ".safiKg");
            $(element).closest("tr").find("input[name*='.safiKg']").attr("name", "pjvVoucher.itemStock.itemStockEntries[" + index + "].safiKg");

            $(element).closest("tr").find("input[name*='.percentage']").attr("id", "pjvVoucher.itemStock.itemStockEntries" + index + ".percentage");
            $(element).closest("tr").find("input[name*='.percentage']").attr("name", "pjvVoucher.itemStock.itemStockEntries[" + index + "].percentage");

            $(element).closest("tr").find("input[name*='.percentage40']").attr("id", "pjvVoucher.itemStock.itemStockEntries" + index + ".percentage40");
            $(element).closest("tr").find("input[name*='.percentage40']").attr("name", "pjvVoucher.itemStock.itemStockEntries[" + index + "].percentage40");

        });

        generateTabIndexing();
        // calculateTotalSjv();
        calculateTotalPjv();
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
            //$(thisControl).closest("tr").find(".availableQuantity").text(data.availableQuantity);
//	    	$(thisControl).closest("tr").find(".standardPrice").text(data.standardRate);
//	    	$(thisControl).closest("tr").find(".unitWeight").val(data.weight);
//	    	$(thisControl).closest("tr").find(".feet").text(data.feet);
//	    	$(thisControl).closest("tr").find(".pricingRule").val(data.pricingRule);
            calculateTotalSjv();
            calculateTotalPjv();
        });
    }

    var checkboxClick = "true";
    calculateTotalSjv();
    calculateTotalPjv();


    $("#prucahse_entry_table").on("input", "input", function () {
        calculateTotalSjv();
        calculateTotalPjv();
    });
    $("#sale_entry_table").on("input", "input", function () {
        calculateTotalSjv();
        calculateTotalPjv();
    });
    var totalKgUseForProduction = parseFloat(0);

    function calculateTotalPjv() {

        var amountTotal = parseFloat(0);
        var itemRate = parseFloat(0);
        var safiKg = parseFloat(0);
        var totalSafiKg = parseFloat(0);
        var quantityTotalInMon = parseFloat(0);
        var totalPercentage = parseFloat(0);
        var totalPercentage40 = parseFloat(0);
        $("#prucahse_entry_table tbody tr").each(function () {

            itemRate = parseFloat($(this).find(".price").val() || 0).toFixed(4);

            safiKg = parseFloat($(this).find(".safiKg").val() || 0);
            totalSafiKg = totalSafiKg + safiKg;
            $(this).find(".itemQuantity").val((safiKg / 40).toFixed(2));
            quantityTotalInMon = parseFloat($(this).find(".itemQuantity").val() || 0);
            $(this).find(".amount").text((quantityTotalInMon * itemRate).toFixed(2));
            amountTotal = amountTotal + (quantityTotalInMon * itemRate).toFixed(2);
            $(this).find(".percentage").val(calcPercentage(safiKg));
            $(this).find(".percentage40").val(calcPercentage40(safiKg));
            console.log($(this).find(".percentage").val());
            totalPercentage = totalPercentage + parseFloat($(this).find(".percentage").val());
            totalPercentage40 = totalPercentage40 + parseFloat($(this).find(".percentage40").val());
        });
        $("#prucahse_entry_table tfoot tr:eq(0) th:eq(3)").text(totalSafiKg);
        $("#prucahse_entry_table tfoot tr:eq(0) th:eq(5)").text(amountTotal);
        $("#prucahse_entry_table tfoot tr:eq(0) th:eq(7)").text(totalPercentage);
        $("#prucahse_entry_table tfoot tr:eq(0) th:eq(6)").text(totalPercentage40);
        calculateAmountEntriesTotal();

    }

    function calculateTotalSjv() {
        totalKgUseForProduction = parseFloat(0);
        var amountTotal = parseFloat(0);
        var itemRate = parseFloat(0);
        var safiKg = parseFloat(0);
        var quantityTotalInMon = parseFloat(0);

        $("#sale_entry_table tbody tr").each(function () {

            itemRate = parseFloat($(this).find(".price").val() || 0);

            safiKg = parseFloat($(this).find(".safiKg").val() || 0);
            totalKgUseForProduction = totalKgUseForProduction + safiKg
            $(this).find(".itemQuantity").val((safiKg / 40).toFixed(2));
            quantityTotalInMon = parseFloat($(this).find(".itemQuantity").val() || 0);
            $(this).find(".amount").text((quantityTotalInMon * itemRate).toFixed(2));
            amountTotal = amountTotal + (quantityTotalInMon * itemRate).toFixed(2);
            // $(this).find(".percentage").val(calcPercentage(safiKg));
        });
        $("#sale_entry_table tfoot tr:eq(0) th:eq(3)").text(totalKgUseForProduction);
        $("#sale_entry_table tfoot tr:eq(0) th:eq(5)").text(amountTotal);
        calculateAmountEntriesTotal();

    }

    // $("#sale_entry_table").on("input", "input", function () {
    //     calculateTotalSjv();
    // });

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


        var itemEmpty = false;
        var itemAmountZero = false;
        var itemDuplicateExists = false;
        var itemDuplicate = "";
        var itemArray = [];
        $("#prucahse_entry_table tbody tr").each(function () {
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