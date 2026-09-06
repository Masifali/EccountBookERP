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

/*function updateFrightAccount(selectElement) {
    const selectedValue = selectElement.value;
    const selectedText = selectElement.options[selectElement.selectedIndex].text;

    // Identify the associated row (e.g., by a selected state or a global reference)
    const targetRow = document.querySelector("#pjvExpancesList tbody tr.selected");

    if (targetRow) {
        // Update the account code hidden input
        const accountCodeInput = targetRow.querySelector(".accountCode");
        if (accountCodeInput) accountCodeInput.value = selectedValue;

        // Update the account name display
        const nameCell = targetRow.querySelector(".name");
        if (nameCell) nameCell.textContent = selectedText;
    }
}*/

$(document).ready(function () {

    $("#itemStock\\.account\\.code").focus();
    $("#frightAccount").on("change", function () {
        $("#pjvExpancesList tbody").find("tr:eq(5) .accountCode:eq(0)").val($(this).val());
    });

    $("#pjvExpancesList tbody tr").slice(0, 2).hide();
    getAppParties();

    function getAppParties() {
        $.get("/app/getAllAccounts", function (data) {

            for (var i = 0, len = data.length; i < len; i++) {

                var option = "<option value = " + data[i].code + ">" + data[i].formattedCode + " &emsp;" + data[i].accountName + "</option>";
                var option1 = "<option value = " + data[i].code + ">" + data[i].accountName + "</option>";
                $("#itemStock\\.account\\.code").append(option);
                $("#itemStock\\.thirdParty\\.code").append(option);
            }
        });

    }

    $("#pjvExpancesList tbody").on("input", function () {
        console.log("Change detected in table row");
        calculateTotal();
    });
    var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 2, maximumFractionDigits: 2});
    $(".select2_single").on("select2:close", function () {
        $(this).focus();
    });

    $('select#company option:not(:selected)').attr('disabled', true);
    $('select#itemStock\\.branch option:not(:selected)').attr('disabled', true);

    /*  $("#voucherStatus").on("change", function () {
          if ($("#voucherStatus").val() == "M") {
              $("#itemStock\\.salesTax").val("0.00");
              $("#itemStock\\.furtherSalesTax").val("0.00");
              $("#itemStock\\.advanceIncomeTax").val("0.00");
          }
      });*/

    $(".entry_add").on("click", function () {

        // un-instrument select2 dropdowns
        $(this).closest("tr").find(".select2_single").select2("destroy");

        var newRow = $(this).closest("tr").clone(true);
        var narration = $(this).closest("tr").find(".narration").val()
        //when you use clone, even the text is carried over. To remove it, do the following:
        newRow.children("td").children("input, span").each(function (index, element) {
            $(element).val("0");
            $(element).text("");
        });
        newRow.children("td").each(function (index, element) {
            if ($(this).children().length < 1) {
                $(element).text("");
            }
            $(element).find(".itemCategory:first").val("");
            $(element).find(".itemDef:first").val("");
            $(element).find(".narration").val(narration);
        });

        newRow.insertAfter($(this).closest("tr"));

        // again instrument select2 dropdowns
        $("#sale_order_entry_table .select2_single").select2({dropdownAutoWidth: true, width: '100%'});

        // update serial numbers
        $("#sale_order_entry_table span#serial_no").each(function (index, element) {
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

            $(element).closest("tr").find("input[name*='.price']").attr("id", "itemStock.itemStockEntries" + index + ".price");
            $(element).closest("tr").find("input[name*='.price']").attr("name", "itemStock.itemStockEntries[" + index + "].price");
            /* $(element).closest("tr").find("input[name*='.narration']").attr("id", "itemStock.itemStockEntries" + index + ".narration");
             $(element).closest("tr").find("input[name*='.narration']").attr("name", "itemStock.itemStockEntries[" + index + "].narration");*/

//
//			$(element).closest("tr").find( "input[name*='.discount']" ).attr("id", "itemStock.itemStockEntries" + index + ".discount");
//			$(element).closest("tr").find( "input[name*='.discount']" ).attr("name", "itemStock.itemStockEntries[" + index + "].discount");


        });

        generateTabIndexing();
        calculateTotal();

    });

    $(".entry_delete").on("click", function () {

        if ($(this).parents("tbody").find("tr").length > 1) {
            $(this).closest("tr").remove();
        }

        // update serial numbers
        $("#sale_order_entry_table span#serial_no").each(function (index, element) {
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

            /*   $(element).closest("tr").find("input[name*='.narration']").attr("id", "itemStock.itemStockEntries" + index + ".narration");
               $(element).closest("tr").find("input[name*='.narration']").attr("name", "itemStock.itemStockEntries[" + index + "].narration");*/

//			$(element).closest("tr").find( "input[name*='.discount']" ).attr("id", "itemStock.itemStockEntries" + index + ".discount");
//			$(element).closest("tr").find( "input[name*='.discount']" ).attr("name", "itemStock.itemStockEntries[" + index + "].discount");
//
//			$(element).closest("tr").find( "input[name*='.discountAmount']" ).attr("id", "itemStock.itemStockEntries" + index + ".discountAmount");
//			$(element).closest("tr").find( "input[name*='.discountAmount']" ).attr("name", "itemStock.itemStockEntries[" + index + "].discountAmount");

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

        //  $("#voucherDate").attr("min", $("#financialYear option:selected").attr("data-value").split("|")[0]);
        //   $("#voucherDate").attr("max", $("#financialYear option:selected").attr("data-value").split("|")[1]);
    }

    // calculate total on load
    calculateAmountEntriesTotal();

    function calculateAmountEntriesTotal() {


    }

    /*$("#sale_order_entry_table tbody tr .itemCategory").each(function() {
        loadItemCategoryDefs($(this));
    });*/
    $("#itemStock\\.account\\.code").on("change", function () {
        console.log("change " + $(".select2_single").val());
        if (!$(".select2_single").val() || $(".select2_single").val() == 0) {

        }

        /*$.get( "/receivables/account_limit_and_balance?accountCode=" + $(".select2_single").val(), function( data ) {
            //$("input#customerAccount\\.balanceLimit").val(data.accountBalanceLimit);
            $("span#previousBalance").text(formatter.format(data.accountBalance));
            $("span#previousBalanceOfSelectedCustomer").text(formatter.format(data.accountBalance));

            calculateTotal();
        });*/
    });


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

        if (!$("#company").val() || !$("#branch").val()) {
            return;
        }

        $.get("/receivables/item_def_detail?itemDefId=" + $(thisControl).val() + "&companyId=" + $("#company").val() + "&branchId=" + $("#branch").val() + "&voucherStatusId=" + '0', function (data) {
            $(thisControl).closest("tr").find(".availableQuantity").text(data.openingQty);
            $(thisControl).closest("tr").find(".price").val(data.standardRate);
            $(thisControl).closest("tr").find(".avgPrice").text(data.openingRate);
            $(thisControl).closest("tr").find(".itemCategory").empty();

            /*    var option = "<option value = " + data.itemCategory.id + ">" + data.itemCategory.name + "&emsp;" + data.itemCategory.code + "</option>";
                $(thisControl).closest("tr").find(".itemCategory").append(option);*/
            calculateTotal();
        });
    }

    // change account
    $("#itemStock\\.account\\.code").on("change", function () {
        $("#voucher_entry_table tbody").find("tr:eq(0) .voucherEntry_account:eq(0)").val($(this).val());
        $("#voucher_entry_table tbody").find("tr:eq(0) .voucherEntry_account:eq(0)").closest("td").find("span").text($(this).find("option:selected").text());
    });

    $("#frightAccount").on("change", function () {
        var selectedValue = $(this).val();
        var rowIndex = $(this).data("index");
        console.log("Row:", rowIndex, "Selected Value:", selectedValue);
        $(this).closest("tr").find(".frightAccount").val(selectedValue);

        var selectedValue = $(this).val();
        var rowIndex = $(this).data("index");
        console.log("Row:", rowIndex, "aafter Value:", selectedValue);
        // you can now trigger ajax or other logic here
    });
//	$("#freightDropDown").on("change", function(){
//		$("#voucher_entry_table tbody").find("tr:eq(3) .voucherEntry_account:eq(0)").val($(this).val());
//		$("#voucher_entry_table tbody").find("tr:eq(3) .voucherEntry_account:eq(0)").closest("td").find("span").text($(this).find("option:selected").text());
//	});

    calculateTotal();


    $("#sale_order_entry_table").on("input", "input", function () {
        calculateTotal();
    });
    $("#tax_table").on("input", "input", function () {
        calculateTotal();
    });
    $("#Credit_table").on("input", "input", function () {
        calculateTotal();
    });
    var calculationBy = parseFloat(0);
    var operator = parseFloat(0);
    var totalExpances = parseFloat(0);
    var expance = parseFloat(0);
    var expanceRate = parseFloat(0);

    function calculateTotal() {

        var amountTotal = parseFloat(0);
        var invoiceAmountTotal = parseFloat(0);
        var standardPrice = parseFloat(0);
        var itemRate = parseFloat(0);
        var discount = parseFloat(0);
        var discount_amount = parseFloat(0);
        var discount_total_amount = parseFloat(0);
        var TotalamountWithOutDiscount = parseFloat(0);
        var itemQty = parseFloat(0);
        var itemTotalQty = parseFloat(0);

        $("#sale_order_entry_table tbody tr").each(function () {

            itemRate = parseFloat($(this).find(".price").val() || 0);
            //discount =  parseFloat($(this).find(".discount").val()|| 0);
            itemQty = parseFloat($(this).find(".itemQuantity").val() || 0);
            itemTotalQty = itemTotalQty + itemQty;
            TotalamountWithOutDiscount = itemRate * itemQty;
            //discount_amount = calcPercentage(TotalamountWithOutDiscount,discount);
            //amountTotal = TotalamountWithOutDiscount -discount_amount;
            amountTotal = TotalamountWithOutDiscount;
            //discount_total_amount = discount_total_amount+discount_amount;
            //$(this).find(".discountAmount").text(discount_amount);
            $(this).find(".amount").text(amountTotal.toFixed(2));
            console.log(amountTotal);
            invoiceAmountTotal = invoiceAmountTotal + amountTotal
        });
        $("#sale_order_entry_table tfoot tr:eq(0) th:eq(4)").text(formatter.format(itemTotalQty));
        $("#sale_order_entry_table tfoot tr:eq(0) th:eq(7)").text(formatter.format(invoiceAmountTotal));

        calculationBy = parseFloat(0);
        operator = parseFloat(0);
        totalExpances = parseFloat(0);
        expance = parseFloat(0);
        expanceRate = parseFloat(0);

        $("#pjvExpancesList tbody tr").each(function () {
            console.log("aassasaas");
            expanceRate = parseFloat($(this).find(".rate").val());
            calculationBy = $(this).find(".calculationBy").val();
            operator = $(this).find(".operator").val();
            expance = parseFloat($(this).find(".rate").val());
            /*if (calculationBy === 'man') {
                $(this).find(".expAmount").val((expanceRate * (safiKg / 40)).toFixed(2));
            }*/
            /* if (calculationBy === 'bag') {
                 $(this).find(".expAmount").val((expanceRate * bags).toFixed(2));
             }*/
            if (calculationBy === 'percentage') {
                $(this).find(".expAmount").val(((expanceRate / 100) * invoiceAmountTotal).toFixed(2));
                //   totalExpances = totalExpances - parseFloat($(this).find(".expAmount").val());
            }
            if (calculationBy === 'lamsam') {
                $(this).find(".expAmount").val(expanceRate);
                // totalExpances = totalExpances - parseFloat($(this).find(".expAmount").val());
            }
            if (calculationBy === 'qty') {
                $(this).find(".expAmount").val((expanceRate * itemTotalQty).toFixed(2));
                //      totalExpances = totalExpances - parseFloat($(this).find(".expAmount").val());
            }
            if (operator === "-") {
                totalExpances = totalExpances - parseFloat($(this).find(".expAmount").val());
                console.log(totalExpances);
            } else if (operator === "+") {
                totalExpances = totalExpances + parseFloat($(this).find(".expAmount").val());
            }
        });
        $("span#totalExpances").text(formatter.format(totalExpances));
        $("span#netAmount").text(formatter.format(totalExpances + invoiceAmountTotal));
        var previousBalance = parseFloat($("span#partyBalance").text().replace(/[\$,]/g, '') || 0).toFixed(2);
        if (previousBalance > 0) {
            $("span#previousBalance").css("color", "green");
            $("span#previousBalanceOfSelectedCustomer").css("color", "green");
        } else {
            $("span#previousBalance").css("color", "red");
            $("span#previousBalanceOfSelectedCustomer").css("color", "red");
        }
        //  $("#tax_table tbody tr:eq(6) th:eq(2)").text(formatter.format(bardanaDebitAmount));
        //$("#tax_table tbody tr:eq(7) th:eq(2)").text(formatter.format(previousBalance);
        //   $("#tax_table tbody tr:eq(8) th:eq(2)").text(formatter.format(amountWithSalesTax + bardanaDebitAmount - previousBalance));


        calculateAmountEntriesTotal();

    }


    function calcPercentage(amount, percentage) {
        return ((amount * percentage) / 100).toFixed(4);
    }

    $(".unloadingRate").on("input", function () {
        $(".unloadingCharges").val(($(".unloadingRate").val() * $("#sale_order_entry_table tfoot tr:eq(9) th:eq(1)").text()).toFixed(2));
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
        $("#sale_order_entry_table tbody tr").each(function () {
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
        $("#sale_order_entry_table tbody td").each(function (i) {
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
                    comingFromSelect2 = false;
                } else {
                    if (focusable.index(self) < 0) {
                        return false;
                    }
                    focusable.eq(focusable.index(self) + (e.shiftKey ? -1 : 1)).focus();
                }
                //focusable.eq(focusable.index(self) + (e.shiftKey ? -1 : 1)).focus();

                return false;
            }
        }

        if (e.altKey && e.keyCode == 83) {
            $("#voucherForm").submit();
            //location.reload();
        }
        // We need to capture the [Shift] key and check the [Enter] key either way.
        if (e.shiftKey) {
            enterKey()
        } else {
            enterKey()
        }
    });

})