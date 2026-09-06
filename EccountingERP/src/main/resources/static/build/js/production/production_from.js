function getFormData($form) {
    var unindexed_array = $form.serializeArray();
    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });

    return indexed_array;
}

Number.prototype.toFixed = function (decimalPlaces) {
    return Number(Math.round(this + 'e' + decimalPlaces) + 'e-' + decimalPlaces);
}

$(document).ready(function () {


    var checkboxClick = "true";
//instrument select2 dropdowns
    $(".select2_single").on("select2:close", function () {
        $(this).focus();
    });

    $(".entry_add").on("click", function () {

// un-instrument select2 dropdowns
        $(this).closest("tr").find(".select2_single").select2("destroy");

        var newRow = $(this).closest("tr").clone(true);

//when you use clone, even the text is carried over. To remove it, do the following:
        newRow.children("td").children("input, span").each(function (index, element) {
            if (index != 5) {
                $(element).val("");
                $(element).text("");
                //   console.log("asf idress  " + $(element).find(".paymentDate").val());
            }
            // console.log($(this).children().length + "asf idress" + index);
        });
        newRow.children("td").each(function (index, element) {
            if ($(this).children().length < 1) {
                $(element).text("");
            }
//$(element).find(".itemCategory:first").val("");
            $(element).find(".itemDef:first").val("");

        });


        newRow.insertAfter($(this).closest("tr"));

// again instrument select2 dropdowns
        $("#purchase_order_entry_table .select2_single").select2({dropdownAutoWidth: true, width: '100%'});

// update serial numbers
        $("#purchase_order_entry_table span#serial_no").each(function (index, element) {
            $(element).text(index + 1);

            $(element).closest("tr").find("input[name*='.id']").attr("id", "saleOrderEntries" + index + ".id");
            $(element).closest("tr").find("input[name*='.id']").attr("name", "saleOrderEntries[" + index + "].id");


            $(element).closest("tr").find("select[name*='.itemDef']").attr("id", "saleOrderEntries" + index + ".itemDef");
            $(element).closest("tr").find("select[name*='.itemDef']").attr("name", "saleOrderEntries[" + index + "].itemDef");


            $(element).closest("tr").find("select[name*='.customerAccount']").attr("id", "saleOrderEntries" + index + ".customerAccount.code");
            $(element).closest("tr").find("select[name*='.customerAccount']").attr("name", "saleOrderEntries[" + index + "].customerAccount.code");


            $(element).closest("tr").find("select[name*='.millKhata']").attr("id", "saleOrderEntries" + index + ".millKhata.id");
            $(element).closest("tr").find("select[name*='.millKhata']").attr("name", "saleOrderEntries[" + index + "].millKhata.id");

            $(element).closest("tr").find("input[name*='.rate']").attr("id", "saleOrderEntries" + index + ".rate");
            $(element).closest("tr").find("input[name*='.rate']").attr("name", "saleOrderEntries[" + index + "].rate");

            $(element).closest("tr").find("input[name*='.kg']").attr("id", "saleOrderEntries" + index + ".kg");
            $(element).closest("tr").find("input[name*='.kg']").attr("name", "saleOrderEntries[" + index + "].kg");

            $(element).closest("tr").find("input[name*='.vehical']").attr("id", "saleOrderEntries" + index + ".vehical");
            $(element).closest("tr").find("input[name*='.vehical']").attr("name", "saleOrderEntries[" + index + "].vehical");

            $(element).closest("tr").find("input[name*='.paymentDate']").attr("id", "saleOrderEntries" + index + ".paymentDate");
            $(element).closest("tr").find("input[name*='.paymentDate']").attr("name", "saleOrderEntries[" + index + "].paymentDate");

            $(element).closest("tr").find("select[name*='.paymentType']").attr("id", "saleOrderEntries" + index + ".paymentType");
            $(element).closest("tr").find("select[name*='.paymentType']").attr("name", "saleOrderEntries[" + index + "].paymentType");

            // $(element).closest("tr").find("input[name*='.payPercent']").attr("id", "saleOrderEntries" + index + ".payPercent");
            // $(element).closest("tr").find("input[name*='.payPercent']").attr("name", "saleOrderEntries[" + index + "].payPercent");
            //
            // $(element).closest("tr").find("input[name*='.inWeek']").attr("id", "saleOrderEntries" + index + ".inWeek");
            // $(element).closest("tr").find("input[name*='.inWeek']").attr("name", "saleOrderEntries[" + index + "].inWeek");


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

            $(element).closest("tr").find("input[name*='.id']").attr("id", "saleOrderEntries" + index + ".id");
            $(element).closest("tr").find("input[name*='.id']").attr("name", "saleOrderEntries[" + index + "].id");


            $(element).closest("tr").find("select[name*='.itemDef']").attr("id", "saleOrderEntries" + index + ".itemDef");
            $(element).closest("tr").find("select[name*='.itemDef']").attr("name", "saleOrderEntries[" + index + "].itemDef");


            $(element).closest("tr").find("select[name*='.customerAccount']").attr("id", "saleOrderEntries" + index + ".customerAccount.code");
            $(element).closest("tr").find("select[name*='.customerAccount']").attr("name", "saleOrderEntries[" + index + "].customerAccount.code");


            $(element).closest("tr").find("select[name*='.millKhata']").attr("id", "saleOrderEntries" + index + ".millKhata.id");
            $(element).closest("tr").find("select[name*='.millKhata']").attr("name", "saleOrderEntries[" + index + "].millKhata.id");

            $(element).closest("tr").find("input[name*='.rate']").attr("id", "saleOrderEntries" + index + ".rate");
            $(element).closest("tr").find("input[name*='.rate']").attr("name", "saleOrderEntries[" + index + "].rate");

            $(element).closest("tr").find("input[name*='.kg']").attr("id", "saleOrderEntries" + index + ".kg");
            $(element).closest("tr").find("input[name*='.kg']").attr("name", "saleOrderEntries[" + index + "].kg");

            $(element).closest("tr").find("input[name*='.vehical']").attr("id", "saleOrderEntries" + index + ".vehical");
            $(element).closest("tr").find("input[name*='.vehical']").attr("name", "saleOrderEntries[" + index + "].vehical");

            $(element).closest("tr").find("input[name*='.paymentDate']").attr("id", "saleOrderEntries" + index + ".paymentDate");
            $(element).closest("tr").find("input[name*='.paymentDate']").attr("name", "saleOrderEntries[" + index + "].paymentDate");

            $(element).closest("tr").find("select[name*='.paymentType']").attr("id", "saleOrderEntries" + index + ".paymentType");
            $(element).closest("tr").find("select[name*='.paymentType']").attr("name", "saleOrderEntries[" + index + "].paymentType");

            // $(element).closest("tr").find("input[name*='.payPercent']").attr("id", "saleOrderEntries" + index + ".payPercent");
            // $(element).closest("tr").find("input[name*='.payPercent']").attr("name", "saleOrderEntries[" + index + "].payPercent");
            //
            // $(element).closest("tr").find("input[name*='.inWeek']").attr("id", "saleOrderEntries" + index + ".inWeek");
            // $(element).closest("tr").find("input[name*='.inWeek']").attr("name", "saleOrderEntries[" + index + "].inWeek");

        });

        generateTabIndexing();
        calculateTotal();
    });

    $("#company").on("change", function () {
        loadCompanyBranches();
    });

    function loadCompanyBranches() {

        if (!$("#company").val() || $("#company").val() == 0) {
            return;
        }

        $.get("/vouchers/company_branches?companyId=" + $("#company").val(), function (data) {
            $("#branch").empty();
            $("#branch").append("<option></option>");

            for (var i = 0, len = data.length; i < len; i++) {
                var option = "<option value = " + data[i].id + ">" + data[i].name + "</option>";
                $("#branch").append(option);
            }
        });

        getNextPurchaseOrderCode();
    }


    $("#branch").on("change", function () {
        getNextPurchaseOrderCode();
    });

    function getNextPurchaseOrderCode() {

        if ($("#id").val() > 0) {
            return;
        }

        if (!$("#company").val() || !$("#branch").val() || $("#financialYear").val() == 0) {
            return;
        }

        $("#purchaseOrderCode").val("");

// PREPARE FORM DATA
        var formData = {
            company: {id: $("#company").val(), name: ""},
            branch: {id: $("#branch").val(), name: ""},

            financialYear: {id: $("#financialYear").val(), name: ""},
        }

// DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "/receivables/next_sale_order_code",
            data: JSON.stringify(formData),
            dataType: "json",
            success: function (data) {
                $("#purchaseOrderCode").val(data.nextPurchaseOrderCode);
            },
        });

    }


    function loadItemCategoryDefs(thisControl) {

        if (!$(thisControl).val() || $(thisControl).val() == 0) {
            return;
        }
        if (!$(thisControl).val() || $(thisControl).val() == 0) {
            return;
        }

        $(thisControl).closest("tr").find(".itemDef").empty();
        $(thisControl).closest("tr").find(".itemDef").append("<option>&emsp;</option>");

        $.get("/receivables/category_item_defs?categoryId=" + $(thisControl).val(), function (data) {

            for (var i = 0, len = data.length; i < len; i++) {
                var option = "<option value = " + data[i].id + ">" + data[i].formattedCode + "&emsp;" + data[i].name + "</option>";
                $(thisControl).closest("tr").find(".itemDef").append(option);
            }

//$(thisControl).closest("tr").find(".itemDef").trigger("change.select2");
//$(thisControl).closest("tr").find(".itemDef").find("#inputhidden input.select2-input").trigger("input");
        });
    }

    $(".itemDef").on("change", function () {
        console.log("");
        loadItemDefDetail($(this));
    });

    function loadItemDefDetail(thisControl) {

        if (!$(thisControl).val() || $(thisControl).val() == 0) {
            return;
        }

        if (!$("#company").val() || !$("#branch").val()) {
            return;
        }

        $.get("/payables/item_def_detail?itemDefId=" + $(thisControl).val() + "&companyId=" + $("#company").val() + "&branchId=" + $("#branch").val() + "&voucherStatusId=" + $("#voucherStatus").val(), function (data) {
            $(thisControl).closest("tr").find(".availableQuantity").text(data.availableQuantity);
            $(thisControl).closest("tr").find(".standardPrice").text(data.standardRate);
            $(thisControl).closest("tr").find(".unitWeight").val(data.weight);
            $(thisControl).closest("tr").find(".feet").text(data.feet);
            $(thisControl).closest("tr").find(".pricingRule").val(data.pricingRule);
            calculateTotal();
        });
    }

//    $(".millKhata").on("change", function(){
//        loadMillKhate($(this));
//    });


//function loadMillKhate(thisControl) {
//
//    console.log(".millKhata");
//        if (!$(thisControl).val() || $(thisControl).val() == 0){
//            return;
//        }
//
//        $(thisControl).closest("tr").find(".millKhata").empty();
//        $(thisControl).closest("tr").find(".millKhata").append("

    //        console.log(".millKhata");
    //
    //        $.get( "/payables/mill_khate?millId=" + $(thisControl).val(), function( data ) {
    //
    //            for (var i = 0, len = data.length; i < len; i++) {
    //                var option = "
    // < option
    //     value=" + data[i].id + "> "+ data[i].name + " < /option>
    /// ";
//                $(thisControl).closest("tr").find(".millKhata").append(option);
//            }
//
//            //$(thisControl).closest("tr").find(".itemDef").trigger("change.select2");
//            //$(thisControl).closest("tr").find(".itemDef").find("#inputhidden input.select2-input").trigger("input");
//        });
//    }
    calculateTotal();

    /*$('body').on('input', '.kg', function(){

    $(this).closest("tr").find(".ton").val($(this).closest("tr").find(".kg").val()/1000);
    calculateTotal();
    });

    $('body').on('input', '.ton', function(){

    $(this).closest("tr").find(".kg").val($(this).closest("tr").find(".ton").val()*1000);
    calculateTotal();
    });*/
    $('body').on('input', '.rate', function () {

//$(this).closest("tr").find(".kg").val($(this).closest("tr").find(".ton").val()*1000);
        calculateTotal();
    });

    function calculateTotal() {

        var amountTotal = parseFloat(0);
        var standardPrice = parseFloat(0);
        var itemRate = parseFloat(0);
        var bagWeight = parseFloat(0);
        var safiKg = parseFloat(0);
        var kg = parseFloat(0);
        var totalbags = parseFloat(0);
        var katotiValue = parseFloat(0);
        var ton = parseFloat(0);
        var quantityTotalInMon = parseFloat(0);

        var quantityTotal = parseFloat(0);
        var totalWeightInKg = parseFloat(0);

        var formatter = new Intl.NumberFormat('ur-PK', {
            style: 'currency',
            currency: 'PKR',
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
        })

        $("#purchase_order_entry_table tbody tr").each(function () {

            kg = parseFloat($(this).find(".kg").val());
            ton = parseFloat($(this).find(".tun").val());

//             $(this).find(".kg").val(ton*1000);
//             $(this).find(".ton").val(kg/1000);
            itemRate = parseFloat($(this).find(".rate").val() || 0);

            $(this).find(".amount").text(((kg / 40) * itemRate).toFixed(2));


        });

        $("span#totalBalance").text(formatter.format(parseFloat($("span#previousBalance").text().replace(/[\$,]/g, '') || 0).toFixed(2) - parseFloat($("#freightCharges").val() || 0) - parseFloat($("#loadingCharges").val() || 0) + amountTotal));

        $("#purchase_order_entry_table tfoot tr:eq(0) th:eq(13)").text((amountTotal));

        $("#purchase_order_entry_table tfoot tr:eq(0) th:eq(4)").text(quantityTotal);
    }

    function calcPercentage(amount, percentage) {
        return ((amount * percentage) / 100).toFixed(4);
    }

    $("#submitForm").off().on("click", function (event) {

        $("#submitForm").addClass('disabled');
        setTimeout(function () {
            $("#submitForm").removeClass('disabled');
        }, 5000);

// Prevent the form from submitting via the browser.
        event.preventDefault();
        event.stopImmediatePropagation();
// Will immediately show the confirmation popup
        $.confirm({
            title: "CONFIRMATION REQUIRED",
            content: "ARE YOU SURE YOU WANT TO SUBMIT THIS PURCHASE ORDER",
            buttons: {
                confirm: function () {
// SUBMIT FORM
                    $("#purchaseOrderForm").submit();
                },
                cancel: function () {
                },
            }
        });
    });

// SUBMIT FORM
    $("#purchaseOrderForm").submit(function () {


        if (!$("#company").val() || $("#company").val() == 0 || !$("#branch").val() || $("#branch").val() == 0) {
            $.confirm({
                title: "ENCOUNTERED AN ERROR!",
                content: "PLEASE SELECT COMPANY/BRANCH",
                type: 'red',
                typeAnimated: true,
            });
// Prevent the form from submitting via the browser.
            return false;
        }

        var itemEmpty = false;
        var kgEmpty = false;
        var accountEmpty = false;
        var millEmpty = false;
        var itemAmountZero = false;
        var itemDuplicateExists = false;
        var itemDuplicate = "";
        var itemArray = [];
        $("#purchase_order_entry_table tbody tr").each(function () {

            if (!$(this).find(".itemDef").val() || $(this).find(".itemDef").val() == 0) {
                itemEmpty = true;
            }
            if (!$(this).find(".kg").val() || $(this).find(".kg").val() == 0) {
                kgEmpty = true;
            }

//            if ($(this).find(".itemDef").val() && $.inArray($(this).find(".itemDef").val(), itemArray) >= 0){
//                itemDuplicateExists = true;
//                itemDuplicate = $(this).find(".itemDef option:selected").text();
//            }
            if (!$(this).find(".customerAccount").val() || $(this).find(".customerAccount").val() == 0) {
                accountEmpty = true;
                return false;
            }
            if (!$(this).find(".millKhata").val() || $(this).find(".millKhata").val() == 0) {
                millEmpty = true;
                return false;
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
        if (kgEmpty) {
            $.confirm({
                title: "ENCOUNTERED AN ERROR!",
                content: "PLEASE SELECT TON  EACH ROW",
                type: 'red',
                typeAnimated: true,
            });
// Prevent the form from submitting via the browser.
            return false;
        }
        if (accountEmpty) {
            $.confirm({
                title: "ENCOUNTERED AN ERROR!",
                content: "PLEASE SELECT ACCOUNT FOR EACH ROW",
                type: 'red',
                typeAnimated: true,
            });
// Prevent the form from submitting via the browser.
            return false;
        }
        if (millEmpty) {
            $.confirm({
                title: "ENCOUNTERED AN ERROR!",
                content: "PLEASE SELECT MILL FOR EACH ROW",
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
        if ($("#purchaseOrderForm").data("submitted")) {
            return false;
        } else {
            $("#purchaseOrderForm").data("submitted", true);
        }
    });

    generateTabIndexing();

    function generateTabIndexing() {
        var tabIndex = 0;
        $("#purchase_order_entry_table tbody td").each(function (i) {
            console.log($(this).find(".supplierAccount").val());
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


//$("#sale_order_entry_table select, input:not(:hidden)").each(function (i) { $(this).attr('tabindex', i + 1); });


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

// We need to capture the [Shift] key and check the [Enter] key either way.
        if (e.shiftKey) {
            enterKey()
        } else {
            enterKey()
        }
    });


})