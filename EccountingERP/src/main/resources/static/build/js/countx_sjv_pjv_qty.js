function getFormData($form) {
    var unindexed_array = $form.serializeArray();
    var indexed_array = {};

    var getFocusBookNumber = parseFloat(0);
    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });

    return ifndexed_array;
}

Number.prototype.toFixed = function (decimalPlaces) {
    return Number(Math.round(this + 'e' + decimalPlaces) + 'e-' + decimalPlaces);
}

$(document).ready(function () {

    $("#details").on('click', function (event) {
        event.preventDefault();
        $("#detailsAccount").toggle();
    })

    var formatterNoFloat = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 0, maximumFractionDigits: 0});
    var formatterOneFloat = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 1, maximumFractionDigits: 1});
    var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 2, maximumFractionDigits: 2});

    $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.bags").focus();
    getFocusBookNumber = 1;
    /// getPendingPurchaseOrderList();
    //  getPendingSaleOrderList();
    //debugger;
    getAppParties();

    function getAppParties() {
        $.get("/app/getAllAccounts", function (data) {
            for (var i = 0, len = data.length; i < len; i++) {
                var option = "<option value = " + data[i].code + ">" + data[i].formattedCode + " &emsp;" + data[i].accountName + "</option>";
                var option1 = "<option value = " + data[i].code + ">" + data[i].accountName + "</option>";
                $("#sjvVoucher\\.itemStock\\.account\\.code").append(option);
                $("#pjvVoucher\\.itemStock\\.account\\.code").append(option);
                $("#saleBrokerAccount").append(option1);
                $("#purchaseBrokerAccount").append(option1);
            }
        });

    }

    $("#pjvVoucher\\.vehicalNumber").on("input", function () {

        $("#sjvVoucher\\.vehicalNumber").val($(this).val());
    });

    $("#pjvVoucher\\.bookNumber").on("input", function () {

        $("#sjvVoucher\\.bookNumber").val($(this).val());

    });
    $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.totalKg").on("input", function () {

        $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.totalKg").val($(this).val());

    });
    $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.bags").on("input", function () {

        $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.bags").val($(this).val());
    });
    $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.bagWeightDeduction").on("input", function () {

        $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.bagWeightDeduction").val($(this).val());

    });
    $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.katoti").on("input", function () {

        $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.katoti").val($(this).val());

    });
    $("#pjvVoucher\\.itemStock\\.freightCharges").on("input", function () {

        $("#sjvVoucher\\.itemStock\\.freightCharges").val($(this).val());

    });
    $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.builtyWeight").on("input", function () {

        $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.builtyWeight").val($(this).val());
    });
    $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.kantaWeight").on("input", function () {

        $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.kantaWeight").val($(this).val());
    });
    $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.moisture").on("input", function () {

        $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.moisture").val($(this).val());
    });
    $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.permintNumber").on("input", function () {

        $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.permintNumber").val($(this).val());
    });
    /*$("#pjvVoucher\\.bookNumber").on("change", function(){
            console.log("My change event" + $(this).val());
        $("#sjvVoucher\\\.bookNumber").val($(this).val());
    });*/

    //$("#sjvVoucher\\\.bookNumber").val($("#pjvVoucher\\.bookNumber").val());

    // Start My Controls
    $(".itemDef").on("change", function () {
        loadItemDefDetail($(this));
    });

    $(".millKhata").on("change", function () {

        $("#sjvVoucher\\.itemStock\\.account\\.code").removeClass('select2_single');
        $.get("/accounts/get-account-by-millkhata?millkhtaId=" + $(".millKhata").val(), function (date) {
            $("#sjvVoucher\\.itemStock\\.account\\.code").val(date).trigger('change');

        });
    });
    $("#saleBrokerAccount").on("change", function () {

        $("#sjvVoucher_entry_table tbody").find("tr:eq(9) .sjvoucherEntry_account:eq(0)").val($(this).val());
        $("#sjvVoucher_entry_table tbody").find("tr:eq(9) .sjvoucherEntry_account:eq(0)").closest("td").find("span").text($(this).find("option:selected").text());
    });
    $("#purchaseBrokerAccount").on("change", function () {

        $("#pjvVoucher\\.voucherEntries9\\.account\\.code").val($(this).val());
        $("#pjvVoucher\\.voucherEntries9\\.account\\.code").closest("td").find("span").text($(this).find("option:selected").text());
    });
    // these set view account name beacuse show sale and purchase  account these two set span values

    /* $("#sjvVoucher_entry_table tbody").find("tr:eq(0) .sjvoucherEntry_account:eq(0)").closest("td").find("span").text($(this).find("option:selected").text());
     $("#pjvVoucher_entry_table tbody").find("tr:eq(0) .voucherEntry_account:eq(0)").closest("td").find("span").text($("#pjvVoucher\\.itemStock\\.account\\.code").find("option:selected").text());
 */

    /* $("#sjvVoucher_entry_table tbody").find("tr:eq(0) .sjvoucherEntry_account:eq(0)").closest("td").find("span").text($(this).find("option:selected").text());
     $("#pjvVoucher_entry_table tbody").find("tr:eq(0) .voucherEntry_account:eq(0)").closest("td").find("span").text($("#pjvVoucher\\.itemStock\\.account\\.code").find("option:selected").text());
 */

    $("#sjvVoucher\\.itemStock\\.account\\.code").on("change", function () {

        $("#sjvVoucher_entry_table tbody").find("tr:eq(0) .sjvoucherEntry_account:eq(0)").val($(this).val());
        $("#sjvVoucher_entry_table tbody").find("tr:eq(0) .sjvoucherEntry_account:eq(0)").closest("td").find("span").text($(this).find("option:selected").text());
        /// set purchase party in voucher_entry
        $("#pjvVoucher_entry_table tbody").find("tr:eq(0) .voucherEntry_account:eq(0)").val($("#pjvVoucher\\.itemStock\\.account\\.code").val());
        $("#pjvVoucher_entry_table tbody").find("tr:eq(0) .voucherEntry_account:eq(0)").closest("td").find("span").text($("#pjvVoucher\\.itemStock\\.account\\.code").find("option:selected").text());

        $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.price").val(parseFloat(0));

        getPendingSaleOrderList();
        calculateTotal();
        /*  var code = $("#sjvVoucher\\.itemStock\\.account\\.code").val();
          $.get("/accounts/get-millKhata-by-account?accountCode=" + code, function (date) {

              $(".millKhata").val(date).trigger('change');
          });*/
    });
    // these are use set party account voucher_entry
    $("#sjvVoucher_entry_table tbody").find("tr:eq(0) .sjvoucherEntry_account:eq(0)").closest("td").find("span").text("");
    $("#pjvVoucher_entry_table tbody").find("tr:eq(0) .voucherEntry_account:eq(0)").closest("td").find("span").text("");

    $("#sjvVoucher_entry_table tbody").find("tr:eq(0) .sjvoucherEntry_account:eq(0)").val($("#sjvVoucher\\.itemStock\\.account\\.code").val());
    $("#sjvVoucher_entry_table tbody").find("tr:eq(0) .sjvoucherEntry_account:eq(0)").closest("td").find("span").text($(this).find("option:selected").text());

    $("#pjvVoucher_entry_table tbody").find("tr:eq(0) .voucherEntry_account:eq(0)").val($("#pjvVoucher\\.itemStock\\.account\\.code").val());
    $("#pjvVoucher_entry_table tbody").find("tr:eq(0) .voucherEntry_account:eq(0)").closest("td").find("span").text($(this).find("option:selected").text());

    $("#pjvVoucher\\.itemStock\\.account\\.code").on("change", function () {

        $("#pjvVoucher_entry_table tbody").find("tr:eq(0) .voucherEntry_account:eq(0)").val($(this).val());
        $("#pjvVoucher_entry_table tbody").find("tr:eq(0) .voucherEntry_account:eq(0)").closest("td").find("span").text($(this).find("option:selected").text());

        $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.price").val(parseFloat(0));
        getPendingPurchaseOrderList();
        pjcalculateTotal();
    });


    // on change events
    $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.bags").on("input", function () {
        //console.log("Purchas Voucher Total Function call From bags = " + $(this).val());
        pjcalculateTotal();
    });

    $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.totalKatoti").blur(function () {
        $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.katoti").val($("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.totalKatoti").val() / $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.bags").val());
        //console.log("asfi");
        pjcalculateTotal();
    });

    $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.totalBagWeightDeduction").blur("input", function () {
        $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.bagWeightDeduction").val($("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.totalBagWeightDeduction").val() / $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.bags").val());
        pjcalculateTotal();
    });

    $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.katoti").on("input", function () {
        pjcalculateTotal();
    });

    $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.bagWeightDeduction").on("input", function () {
        pjcalculateTotal();
    });

    $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.price").on("input", function () {
        pjcalculateTotal();
    });

    $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.totalKg").on("input", function () {
        pjcalculateTotal();
    });

    $("#pjvVoucher\\.itemStock\\.brokriRate").on("input", function () {
        pjcalculateTotal();
    });

    $("#pjvVoucher\\.itemStock\\.millTaxRate").on("input", function () {
        pjcalculateTotal();
    });

    $("#pjvVoucher\\.itemStock\\.bankTaxRate").on("input", function () {
        pjcalculateTotal();
    });

    $("#pjvVoucher\\.itemStock\\.silaiRate").on("input", function () {
        pjcalculateTotal();
    });

    $("#pjvVoucher\\.itemStock\\.unloadingRate").on("input", function () {
        pjcalculateTotal();
    });

    $("#pjvVoucher\\.itemStock\\.bardanaRate").on("input", function () {
        pjcalculateTotal();
    });

    $("#pjvVoucher\\.itemStock\\.otherExp").on("input", function () {
        pjcalculateTotal();
    });

    $("#pjvVoucher\\.itemStock\\.freightCharges").on("input", function () {
        pjcalculateTotal();
    });
    // on change events

    $("#pjvVoucher\\.itemStock\\.brokriAmount").blur(function () {
        var amount = $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.amount").val().replace(/[^0-9\.-]+/g, "");//.replace(/[^0-9\.-]+/g, "")
        console.log("totalAmount   ");
        if (parseFloat(amount) == 0)
            alert("Amount is zero");
        else {
            //console.log($("#pjvVoucher\\.itemStock\\.brokriAmount").val()/ amount);
            if ($("#brokeryByMan").val() === 'true') {
                $("#pjvVoucher\\.itemStock\\.brokriRate").val(($("#pjvVoucher\\.itemStock\\.brokriAmount").val() / qtyInMan).toFixed(8));
                // $("#sjvVoucher\\.itemStock\\.brokriAmount").val((qtyInMan * barokiRate).toFixed(0));
            } else {
                $("#pjvVoucher\\.itemStock\\.brokriRate").val(($("#pjvVoucher\\.itemStock\\.brokriAmount").val() / amount).toFixed(8));
            }
            pjcalculateTotal();
        }
    });


    // On Change Sales Voucher Events
    $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.bags").on("input", function () {
        //console.log("Sales Voucher Total Function call From bags = " + $(this).val());
        calculateTotal();
    });

    $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.totalKatoti").blur(function () {
        $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.katoti").val($("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.totalKatoti").val() / $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.bags").val());
        //console.log("asfi");
        calculateTotal();
    });

    $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.totalBagWeightDeduction").blur("input", function () {
        $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.bagWeightDeduction").val($("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.totalBagWeightDeduction").val() / $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.bags").val());
        calculateTotal();
    });

    $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.katoti").on("input", function () {
        calculateTotal();
    });

    $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.bagWeightDeduction").on("input", function () {
        calculateTotal();
    });

    $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.price").on("input", function () {
        calculateTotal();
    });

    $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.totalKg").on("input", function () {
        calculateTotal();
    });

    $("#sjvVoucher\\.itemStock\\.brokriRate").on("input", function () {
        calculateTotal();
    });

    $("#sjvVoucher\\.itemStock\\.millTaxRate").on("input", function () {
        calculateTotal();
    });

    $("#sjvVoucher\\.itemStock\\.bankTaxRate").on("input", function () {
        calculateTotal();
    });

    $("#sjvVoucher\\.itemStock\\.silaiRate").on("input", function () {
        calculateTotal();
    });

    $("#sjvVoucher\\.itemStock\\.unloadingRate").on("input", function () {
        calculateTotal();
    });

    $("#sjvVoucher\\.itemStock\\.bardanaRate").on("input", function () {
        calculateTotal();
    });

    $("#sjvVoucher\\.itemStock\\.otherExp").on("input", function () {
        calculateTotal();
    });
    $("#sjvVoucher\\.itemStock\\.freightCharges").on("input", function () {
        calculateTotal();
    });
    // on change events


    $("#sjvVoucher\\.itemStock\\.brokriAmount").blur(function () {
        var amount = $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.amount").val().replace(/[^0-9\.-]+/g, "");//.replace(/[^0-9\.-]+/g, "")
        //console.log("totalAmount   ");
        if (parseFloat(amount) == 0)
            alert("Amount is zero");
        else {
            //console.log($("#sjvVoucher\\.itemStock\\.brokriAmount").val()/ amount);
            if ($("#brokeryByMan").val() === 'true') {
                $("#sjvVoucher\\.itemStock\\.brokriRate").val(($("#sjvVoucher\\.itemStock\\.brokriAmount").val() / qtyInMan).toFixed(8));
                // $("#sjvVoucher\\.itemStock\\.brokriAmount").val((qtyInMan * barokiRate).toFixed(0));
            } else {
                $("#sjvVoucher\\.itemStock\\.brokriRate").val(($("#sjvVoucher\\.itemStock\\.brokriAmount").val() / amount).toFixed(8));
            }
            // $("#sjvVoucher\\.itemStock\\.brokriRate").val(($("#sjvVoucher\\.itemStock\\.brokriAmount").val() / amount).toFixed(8));
            calculateTotal();
        }
    });

    $("#pjvVoucher\\.itemStock\\.brokriAmount").blur(function () {
        var amount = $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.amount").val().replace(/[^0-9\.-]+/g, "");//.replace(/[^0-9\.-]+/g, "")
        //console.log("totalAmount   ");
        if (parseFloat(amount) == 0)
            alert("Amount is zero");
        else {
            //console.log($("#pjvVoucher\\.itemStock\\.brokriAmount").val()/ amount);
            $("#pjvVoucher\\.itemStock\\.brokriRate").val(($("#pjvVoucher\\.itemStock\\.brokriAmount").val() / amount).toFixed(8));
            pjcalculateTotal();
        }
    });
    $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.itemDef").on("change", function () {
        getPendingSaleOrderList();
        getPendingPurchaseOrderList();
    });

    $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.saleOrderEntry").on("change", function () {
        var selectedText = $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.saleOrderEntry option:selected").text();
        var arr = selectedText.split('/');
        $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.price").val(parseFloat(arr[2].trim()));
        calculateTotal();

    });

    $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.purchaseOrderEntry").on("change", function () {
        var selectedText = $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.purchaseOrderEntry option:selected").text();
        var arr = selectedText.split('/');
        $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.price").val(parseFloat(arr[2].trim()));
        pjcalculateTotal();

    });

    $("input[type=number]").on("focus", function () {
        $(this).on("keydown", function (event) {
            if (event.keyCode === 38 || event.keyCode === 40) {
                event.preventDefault();
            }
        });
    });

    $("input[type=number]").on("focus", function () {
        $(this).on("keydown", function (event) {
            if (event.keyCode === 38 || event.keyCode === 40) {
                event.preventDefault();
            }
        });
    });

    $("input[type=number]").on("focus", function () {
        $(this).on("keydown", function (event) {
            if (event.keyCode === 38 || event.keyCode === 40) {
                event.preventDefault();
            }
        });
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

    $('input[type=number]').on('wheel', function (e) {
        return false;
    });

    $('select').on('wheel', function (e) {
        return false;
    });

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
                    //console.log("key "+currentTabIndex);
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
            validateAndSubmit();
            //location.reload();
        }
    });

    calculateTotal();
    pjcalculateTotal();

    // End On CHange Purchase Voucher Events

    // End My Controls

    // Start Custom Function //
    getPendingPurchaseOrderList();

    function getPendingPurchaseOrderList() {
        var id = $("#purchaseOrderEntryId").val();

        // PREPARE FORM DATA
        var formData = {

            branch: {id: $("#pjvVoucher\\.itemStock\\.branch").val(), name: ""},
            accountCode: $("#pjvVoucher\\.itemStock\\.account\\.code").val(),
            fromDate: $("#pjvVoucher\\.voucherDate").val(),
            item: $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.itemDef").val(),
            millKhata: $(".millKhata").val(),
            qty: "",
            requestFromPjv: "1",
        }
        //console.log($("#pjvVoucher\\.itemStock\\.branch").val());
        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "/payables/pending_purchase_order_list",
            data: JSON.stringify(formData),
            dataType: "json",
            success: function (data) {

                $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.purchaseOrderEntry").empty();
                $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.purchaseOrderEntry").append("<option></option>");

                for (var i = 0, len = data.length; i < len; i++) {
                    var option = "<option value = " + data[i].purchaseOrderEntryid + ">" + data[i].itemSubCategory.name + " " + data[i].itemDef.name + ' / ' + data[i].pendingWeight / 1000 + ' / ' + data[i].rate + ' / ' + data[i].date + "(" + data[i].paymentDate.split("-")[2] + ")" + ' / ' + data[i].millKhata + "</option>";
                    $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.purchaseOrderEntry").append(option);
                }
                console.log("aaaa " + id);
                // this id  which Entry is saved in voucher beacuse on load page empty the dropdown
                $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.purchaseOrderEntry").val(id).trigger();
            },
            complete: function () {
                // enable submit button
            },
        });

    }

    getPendingSaleOrderList();

    function getPendingSaleOrderList() {
        //console.log("finction");
        var entryID = $("#saleOrderEntryId").val();
        var formData = {

            branch: {id: $("#pjvVoucher\\.itemStock\\.branch").val(), name: ""},
            accountCode: $("#sjvVoucher\\.itemStock\\.account\\.code").val(),
            item: $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.itemDef").val(),
            fromDate: $("#pjvVoucher\\.voucherDate").val(),
            millKhata: $(".millKhata").val(),
            qty: "",
            requestFromSjv: "1",
        }
        //console.log($("#pjvVoucher\\.branch").val());
        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "/receivables/pending_sale_order_list",
            data: JSON.stringify(formData),
            dataType: "json",
            success: function (data) {
                $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.saleOrderEntry").empty();
                $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.saleOrderEntry").append("<option></option>");

                for (var i = 0, len = data.length; i < len; i++) {
                    var option = "<option value = " + data[i].saleOrderEntryid + ">" + data[i].itemSubCategory.name + " " + data[i].itemDef.name + ' / ' + data[i].pendingWeight / 1000 + ' / ' + data[i].rate + ' / ' + data[i].date + "(" + data[i].paymentDate.split("-")[2] + ")" + ' / ' + data[i].millKhata + "</option>";
                    $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.saleOrderEntry").append(option);
                }
                // this id  which Entry is saved in voucher beacuse on load page empty the dropdown
                $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.saleOrderEntry").val(entryID).trigger();
            },
            complete: function () {
                // enable submit button
            },
        });
    }


    function loadItemDefDetail(thisControl) {
        if (!$(thisControl).val() || $(thisControl).val() == 0) {
            return;
        }
        if (!$("#pjvVoucher\\.itemStock\\.branch").val()) {
            return;
        }
        //console.log("check it")
        $.get("/payables/item_def_detail?itemDefId=" + $(thisControl).val() + "&companyId=" + 1 + "&branchId=" + $("#pjvVoucher\\.itemStock\\.branch").val() + "&voucherStatusId=" + "0", function (data) {
            $("#pricingRule").val(data.pricingRule);
            $("#weightParBag").val(data.weightParBag);
            // $("#itemStock\\.itemStockEntries0\\.itemQuantity").val(data.weightParBag);
            //console.log('hi! its me loaditem' + data.pricingRule);
            calculateTotal();
            //sjcalculateTotal();
        });
    }

    function calculateAmountEntriesTotal() {

        var debitTotal = parseFloat(0);
        var creditTotal = parseFloat(0);

        $("#sjvVoucher_entry_table tbody tr").each(function () {
            debitTotal = debitTotal + parseFloat($(this).find(".sjvoucherEntry_debit").val() || 0);
            creditTotal = creditTotal + parseFloat($(this).find(".sjvoucherEntry_credit").val() || 0);
        });

        $("#sjvVoucher_entry_table span#sjdebitTotal").text(formatter.format(debitTotal));
        $("#sjvVoucher_entry_table span#sjcreditTotal").text(formatter.format(creditTotal));

        if ($("#sjvVoucher_entry_table span#sjdebitTotal").text() == $("#sjvVoucher_entry_table span#sjcreditTotal").text()) {
            $("#sjvVoucher_entry_table span#sjdebitTotal").css("color", "#73879C");
            $("#sjvVoucher_entry_table span#sjcreditTotal").css("color", "#73879C");
        } else {
            $("#sjvVoucher_entry_table span#sjdebitTotal").css("color", "red");
            $("#sjvVoucher_entry_table span#sjcreditTotal").css("color", "red");
        }
    }

    function pjcalculateAmountEntriesTotal() {

        var debitTotal = parseFloat(0);
        var creditTotal = parseFloat(0);

        $("#pjvVoucher_entry_table tbody tr").each(function () {
            debitTotal = debitTotal + parseFloat($(this).find(".pjvoucherEntry_debit").val() || 0);
            creditTotal = creditTotal + parseFloat($(this).find(".pjvoucherEntry_credit").val() || 0);
        });

        $("#pjvVoucher_entry_table span#pjdebitTotal").text(formatter.format(debitTotal));
        $("#pjvVoucher_entry_table span#pjcreditTotal").text(formatter.format(creditTotal));

        if ($("#pjvVoucher_entry_table span#pjdebitTotal").text() == $("#pjvVoucher_entry_table span#pjcreditTotal").text()) {
            $("#pjvVoucher_entry_table span#pjdebitTotal").css("color", "#73879C");
            $("#pjvVoucher_entry_table span#pjcreditTotal").css("color", "#73879C");
        } else {
            $("#pjvVoucher_entry_table span#pjdebitTotal").css("color", "red");
            $("#pjvVoucher_entry_table span#pjcreditTotal").css("color", "red");
        }
    }

    function calculateTotal() {
        //console.log('i m in calculate total');
        var amountTotal = parseFloat(0);
        var creditCharges = parseFloat(0);
        var debitCharges = parseFloat(0);
        var standardPrice = parseFloat(0);
        var itemRate = parseFloat(0);
        var discount = parseFloat(0);
        var discount_amount = parseFloat(0);
        var discount_total_amount = parseFloat(0);
        var TotalamountWithOutDiscount = parseFloat(0);
        var itemQty = parseFloat(0);
        var itemTotalQty = parseFloat(0);

        var allCharges = parseFloat(0);
        var qtyInMan = parseFloat(0);
        var invoiceAmountTotal = parseFloat(0);
        var barokiRate = parseFloat(0);
        var brokriAmount = parseFloat(0);
        //var productAmount = parseFloat(0);
        var totalAmount = parseFloat(0);
        var price = parseFloat(0);
        var saleAmount = parseFloat(0);
        var price = parseFloat(0);
        var purchaseAmount = parseFloat(0);
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
        var comParMan = parseFloat(0);
        var comParManAmount = parseFloat(0);
        //sjvVoucher.itemStock.itemStockEntries[0].bags
        //console.log('i m start calculate total');
        bags = $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.bags").val();
        barokiRate = $("#sjvVoucher\\.itemStock\\.brokriRate").val();
        brokriAmount = $("#sjvVoucher\\.itemStock\\.brokriAmount").val();

        totalAmount = $("#sjvVoucher\\.itemStock\\.totalAmount").val();
        // SALES VOUCHER CALCULATIONS
        price = $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.price").val();
        saleAmount = $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.amount").val();

        millTaxRate = $("#sjvVoucher\\.itemStock\\.millTaxRate").val();
        millTaxAmount = $("#sjvVoucher\\.itemStock\\.millTaxAmount").val();

        bankTaxRate = $("#sjvVoucher\\.itemStock\\.bankTaxRate").val();
        bankTaxAmount = $("#sjvVoucher\\.itemStock\\.bankTaxAmount").val();

        silaiRate = $("#sjvVoucher\\.itemStock\\.silaiRate").val();
        silaiAmount = $("#sjvVoucher\\.itemStock\\.silaiAmount").val();

        bardanaRate = $("#sjvVoucher\\.itemStock\\.bardanaRate").val();
        bardanaAmount = $("#sjvVoucher\\.itemStock\\.bardanaAmount").val();

        unloadingRate = parseFloat(0); //$("#sjvVoucher\\.itemStock\\.unloadingRate").val();
        unloadingAmount = parseFloat(0); //$("#sjvVoucher\\.itemStock\\.unloadingCharges").val();

        otherExp = $("#sjvVoucher\\.itemStock\\.otherExp").val();
        freightRate = $("#sjvVoucher\\.itemStock\\.freightRate").val();
        freightCharges = $("#sjvVoucher\\.itemStock\\.freightCharges").val();

        itemQty = $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.itemQuantity").val();
        totalKg = $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.totalKg").val();
        safiKg = $("#sjvVoucher\\.itemStock\\.brokriAmount").val();
        deduction = $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.katoti").val();
        bagWeightDeduction = $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.bagWeightDeduction").val();
        totalBagWeightDeduction = $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.totalBagWeightDeduction").val();
        otherBagWeight = $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.otherBagWeight").val();

        totalDeduction = (bags * deduction).toFixed(0);
        totalBagWeightDeduction = (bags * bagWeightDeduction).toFixed(0);

        safiKg = totalKg - totalDeduction - totalBagWeightDeduction;
        if ($("#pricingRule").val() == "KG") {
            qtyInMan = safiKg / parseFloat($("#weightParBag").val());
        } else {
            qtyInMan = bags;
        }

        invoiceAmountTotal = qtyInMan * price;

        if (comParMan === "") {
            comParMan = parseFloat(0);
        }
        comParManAmount = comParMan * qtyInMan;

        var int_part = Math.trunc(qtyInMan); // returns 3
        var float_part = Number((qtyInMan - int_part).toFixed(2));

        $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.manWithKg").val(int_part + "- " + (float_part * parseFloat($("#weightParBag").val())).toFixed(0));
        console.log("a   " + int_part + "-" + (float_part * parseFloat($("#weightParBag").val())));
        $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.totalKatoti").val(totalDeduction);

        $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.totalBagWeightDeduction").val(totalBagWeightDeduction);

        $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.itemQuantity").val(safiKg);

        $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.otherBagWeight").val((safiKg / bags).toFixed(2))

        $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.amount").val(formatterOneFloat.format(invoiceAmountTotal));

        $("#sjvVoucher\\.itemStock\\.bankTaxAmount").val(formatterNoFloat.format(invoiceAmountTotal * bankTaxRate / 100));

        $("#sjvVoucher\\.itemStock\\.millTaxAmount").val(formatter.format(qtyInMan * millTaxRate));

        $("#sjvVoucher\\.itemStock\\.silaiAmount").val(formatterNoFloat.format(bags * silaiRate));

        $("#sjvVoucher\\.itemStock\\.bardanaAmount").val(formatterNoFloat.format(bags * bardanaRate));

        $("#sjvVoucher\\.itemStock\\.unloadingCharges").val(formatterNoFloat.format(bags * unloadingRate));

        //console.log("helo ");
        if ($("#brokeryByMan").val() === 'true') {
            $("#sjvVoucher\\.itemStock\\.brokriAmount").val((qtyInMan * barokiRate).toFixed(0));
        } else {
            $("#sjvVoucher\\.itemStock\\.brokriAmount").val((invoiceAmountTotal * barokiRate).toFixed(0));
        }
        //$("#sjvVoucher\\.itemStock\\.brokriAmount").val((invoiceAmountTotal * barokiRate).toFixed(0));

        brokriAmount = $("#sjvVoucher\\.itemStock\\.brokriAmount").val();
        bankTaxAmount = $("#sjvVoucher\\.itemStock\\.bankTaxAmount").val().replace(/[^0-9\.-]+/g, "");
        millTaxAmount = $("#sjvVoucher\\.itemStock\\.millTaxAmount").val();
        bardanaAmount = $("#sjvVoucher\\.itemStock\\.bardanaAmount").val().replace(/[^0-9\.-]+/g, "");
        // bardanaAmount = parseFloat(bardanaRate);
        silaiAmount = $("#sjvVoucher\\.itemStock\\.silaiAmount").val();
        unloadingAmount = parseFloat(0); //$("#sjvVoucher\\.itemStock\\.unloadingCharges").val();
        otherExp = $("#sjvVoucher\\.itemStock\\.otherExp").val();
        freightCharges = $("#sjvVoucher\\.itemStock\\.freightCharges").val();

        $("#sjvVoucher\\.itemStock\\.otherExp").val(formatterNoFloat.format(otherExp).replace(/[^0-9\.-]+/g, ""));
        $("#sjvVoucher\\.itemStock\\.freightCharges").val(formatterNoFloat.format(freightCharges).replace(/[^0-9\.-]+/g, ""));

        saleAmount = $("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.amount").val();

        console.log($("#sjvVoucher\\.itemStock\\.account\\.code").find("option:selected").text());
        // allCharges = parseFloat(bankTaxAmount) + parseFloat(millTaxAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(bardanaAmount) + parseFloat(silaiAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(unloadingAmount) + parseFloat(otherExp.replace(/[^0-9\.-]+/g, "")) + parseFloat(freightCharges.replace(/[^0-9\.-]+/g, ""));

        // This only for Hi Tach Mill for Almadin commission shop kasur
        if (
            $("#millTaxRateBehaviour").val() === "true" &&
            $("#sjvVoucher\\.itemStock\\.account\\.code").find("option:selected").text().includes("HI TECH FEED")
        ) {
            $("#sjvmillTaxRateLabel").text("COM/MAN");
            millTaxAmount = formatter.format(qtyInMan * 5);
            var persengateSmount = calcPercentage(parseFloat(millTaxAmount.replace(/[^0-9\.-]+/g, "")), 12);
            $("#sjvVoucher\\.itemStock\\.bankTaxAmount").val(persengateSmount);
            $("#sjvVoucher\\.itemStock\\.bankTaxRate").val((persengateSmount * 100) / invoiceAmountTotal);
            millTaxAmount = formatter.format(qtyInMan * 0);
            creditCharges = parseFloat(persengateSmount) - parseFloat(millTaxAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(freightCharges.replace(/[^0-9\.-]+/g, ""));
        } else {
            $("#sjvmillTaxRateLabel").text("COM(W.H) TAX(-)");
            creditCharges = parseFloat(bankTaxAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(millTaxAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(freightCharges.replace(/[^0-9\.-]+/g, ""));

        }
        debitCharges = parseFloat(bardanaAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(silaiAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(otherExp.replace(/[^0-9\.-]+/g, "")) + parseFloat(unloadingAmount);

        console.log(saleAmount.replace(/[^0-9\.-]+/g, ""));
        $("#sjvVoucher\\.itemStock\\.netAmount").val(formatter.format(saleAmount.replace(/[^0-9\.-]+/g, "") - creditCharges + debitCharges));

        $("#sjvVoucher_entry_table tbody tr:eq(0) td:eq(4) input").val(parseFloat(saleAmount.replace(/[^0-9\.-]+/g, "")) - creditCharges + debitCharges);
        $("#sjvVoucher_entry_table tbody tr:eq(1) td:eq(5) input").val(parseFloat(saleAmount.replace(/[^0-9\.-]+/g, "")));
        if (parseFloat(otherExp.replace(/[^0-9\.-]+/g, "")) > 0) {

            $("#sjvVoucher_entry_table tbody tr:eq(7) td:eq(5) input").val((otherExp.replace(/[^0-9\.-]+/g, "")));
            $("#sjvVoucher_entry_table tbody tr:eq(7) td:eq(4) input").val(0);

        } else {
            $("#sjvVoucher_entry_table tbody tr:eq(7) td:eq(4) input").val(otherExp.replace(/[^0-9\.-]+/g, "") * (-1));
            $("#sjvVoucher_entry_table tbody tr:eq(7) td:eq(5) input").val(0);

        }
        $("#sjvVoucher_entry_table tbody tr:eq(2) td:eq(5) input").val(unloadingAmount);
        $("#sjvVoucher_entry_table tbody tr:eq(2) td:eq(4) input").val(0);
        $("#sjvVoucher_entry_table tbody tr:eq(3) td:eq(4) input").val(freightCharges.replace(/[^0-9\.-]+/g, ""));
        $("#sjvVoucher_entry_table tbo CVZXdy tr:eq(3) td:eq(5) input").val(0);
        $("#sjvVoucher_entry_table tbody tr:eq(4) td:eq(4) input").val(millTaxAmount.replace(/[^0-9\.-]+/g, ""));
        $("#sjvVoucher_entry_table tbody tr:eq(4) td:eq(5) input").val(0);
        $("#sjvVoucher_entry_table tbody tr:eq(5) td:eq(4) input").val(bankTaxAmount.replace(/[^0-9\.-]+/g, ""));
        $("#sjvVoucher_entry_table tbody tr:eq(5) td:eq(5) input").val(0);
        $("#sjvVoucher_entry_table tbody tr:eq(6) td:eq(5) input").val(silaiAmount.replace(/[^0-9\.-]+/g, ""));
        $("#sjvVoucher_entry_table tbody tr:eq(6) td:eq(4) input").val(0);
        /*  $("#sjvVoucher_entry_table tbody tr:eq(7) td:eq(5) input").val(otherExp.replace(/[^0-9\.-]+/g, ""));
          $("#sjvVoucher_entry_table tbody tr:eq(7) td:eq(4) input").val(0);*/
        $("#sjvVoucher_entry_table tbody tr:eq(8) td:eq(5) input").val(bardanaAmount.replace(/[^0-9\.-]+/g, ""));
        $("#sjvVoucher_entry_table tbody tr:eq(8) td:eq(4) input").val(0);
        $("#sjvVoucher_entry_table tbody tr:eq(9) td:eq(5) input").val(brokriAmount.replace(/[^0-9\.-]+/g, ""));
        $("#sjvVoucher_entry_table tbody tr:eq(9) td:eq(4) input").val(0);
        $("#sjvVoucher_entry_table tbody tr:eq(9) td:eq(3) textarea").text("BROKRY");
        $("#sjvVoucher_entry_table tbody tr:eq(10) td:eq(4) input").val(brokriAmount.replace(/[^0-9\.-]+/g, ""));
        $("#sjvVoucher_entry_table tbody tr:eq(10) td:eq(5) input").val(0);
        $("#sjvVoucher_entry_table tbody tr:eq(10) td:eq(3) textarea").text("BROKRY");
        calculateAmountEntriesTotal();
    }

    function pjcalculateTotal() {
        calculateTotal();
        //console.log('i m in calculate total');
        var amountTotal = parseFloat(0);
        var creditCharges = parseFloat(0);
        var debitCharges = parseFloat(0);
        var standardPrice = parseFloat(0);
        var itemRate = parseFloat(0);
        var discount = parseFloat(0);
        var discount_amount = parseFloat(0);
        var discount_total_amount = parseFloat(0);
        var TotalamountWithOutDiscount = parseFloat(0);
        var itemQty = parseFloat(0);
        var itemTotalQty = parseFloat(0);

        var allCharges = parseFloat(0);
        var qtyInMan = parseFloat(0);
        var invoiceAmountTotal = parseFloat(0);
        var barokiRate = parseFloat(0);
        var brokriAmount = parseFloat(0);
        //var productAmount = parseFloat(0);
        var totalAmount = parseFloat(0);
        var price = parseFloat(0);
        var saleAmount = parseFloat(0);
        var price = parseFloat(0);
        var purchaseAmount = parseFloat(0);
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
        //sjvVoucher.itemStock.itemStockEntries[0].bags
        //console.log('i m start calculate total');
        bags = $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.bags").val();
        barokiRate = $("#pjvVoucher\\.itemStock\\.brokriRate").val();
        brokriAmount = $("#pjvVoucher\\.itemStock\\.brokriAmount").val();

        totalAmount = $("#pjvVoucher\\.itemStock\\.totalAmount").val();
        // SALES VOUCHER CALCULATIONS
        price = $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.price").val();
        saleAmount = $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.amount").val();

        millTaxRate = $("#pjvVoucher\\.itemStock\\.millTaxRate").val();
        millTaxAmount = $("#pjvVoucher\\.itemStock\\.millTaxAmount").val();

        bankTaxRate = $("#pjvVoucher\\.itemStock\\.bankTaxRate").val();
        bankTaxAmount = $("#pjvVoucher\\.itemStock\\.bankTaxAmount").val();

        silaiRate = $("#pjvVoucher\\.itemStock\\.silaiRate").val();
        silaiAmount = $("#pjvVoucher\\.itemStock\\.silaiAmount").val();

        bardanaRate = $("#pjvVoucher\\.itemStock\\.bardanaRate").val();
        bardanaAmount = $("#pjvVoucher\\.itemStock\\.bardanaAmount").val();

        unloadingRate = parseFloat(0); //$("#sjvVoucher\\.itemStock\\.unloadingRate").val();
        unloadingAmount = parseFloat(0); //$("#sjvVoucher\\.itemStock\\.unloadingCharges").val();

        otherExp = $("#pjvVoucher\\.itemStock\\.otherExp").val();
        freightRate = $("#pjvVoucher\\.itemStock\\.freightRate").val();
        freightCharges = $("#pjvVoucher\\.itemStock\\.freightCharges").val();

        itemQty = $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.itemQuantity").val();
        totalKg = $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.totalKg").val();
        safiKg = $("#pjvVoucher\\.itemStock\\.brokriAmount").val();
        deduction = $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.katoti").val();
        bagWeightDeduction = $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.bagWeightDeduction").val();
        totalBagWeightDeduction = $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.totalBagWeightDeduction").val();
        otherBagWeight = $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.otherBagWeight").val();

        totalDeduction = (bags * deduction).toFixed(0);
        totalBagWeightDeduction = (bags * bagWeightDeduction).toFixed(0);

        safiKg = totalKg - totalDeduction - totalBagWeightDeduction;
        if ($("#pricingRule").val() == "KG") {
            qtyInMan = safiKg / parseFloat($("#weightParBag").val());
        } else {
            qtyInMan = bags;
        }

        invoiceAmountTotal = qtyInMan * price;

        var int_part = Math.trunc(qtyInMan); // returns 3
        var float_part = Number((qtyInMan - int_part).toFixed(2));

        $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.manWithKg").val(int_part + "- " + (float_part * parseFloat($("#weightParBag").val())).toFixed(0));
        //console.log("a   "+int_part+"-"+(float_part*40));
        $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.totalKatoti").val(totalDeduction);

        $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.totalBagWeightDeduction").val(totalBagWeightDeduction);

        $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.itemQuantity").val(safiKg);

        $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.otherBagWeight").val((safiKg / bags).toFixed(2))

        $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.amount").val(formatter.format(invoiceAmountTotal));

        $("#pjvVoucher\\.itemStock\\.bankTaxAmount").val(formatterNoFloat.format(invoiceAmountTotal * bankTaxRate / 100));

        $("#pjvVoucher\\.itemStock\\.millTaxAmount").val(formatterNoFloat.format(qtyInMan * millTaxRate));

        $("#pjvVoucher\\.itemStock\\.silaiAmount").val(formatterNoFloat.format(bags * silaiRate));

        $("#pjvVoucher\\.itemStock\\.bardanaAmount").val(formatterNoFloat.format(bags * bardanaRate));

        $("#pjvVoucher\\.itemStock\\.unloadingCharges").val(formatterNoFloat.format(bags * unloadingRate));

        console.log("helo ");
        if ($("#brokeryByMan").val() === 'true') {
            $("#pjvVoucher\\.itemStock\\.brokriAmount").val((qtyInMan * barokiRate).toFixed(0));
        } else {
            $("#pjvVoucher\\.itemStock\\.brokriAmount").val((invoiceAmountTotal * barokiRate).toFixed(0));
        }
        brokriAmount = $("#pjvVoucher\\.itemStock\\.brokriAmount").val();
        bankTaxAmount = $("#pjvVoucher\\.itemStock\\.bankTaxAmount").val().replace(/[^0-9\.-]+/g, "");
        millTaxAmount = $("#pjvVoucher\\.itemStock\\.millTaxAmount").val();
        bardanaAmount = $("#pjvVoucher\\.itemStock\\.bardanaAmount").val().replace(/[^0-9\.-]+/g, "");
        silaiAmount = $("#pjvVoucher\\.itemStock\\.silaiAmount").val();
        unloadingAmount = parseFloat(0); //$("#sjvVoucher\\.itemStock\\.unloadingCharges").val();
        otherExp = $("#pjvVoucher\\.itemStock\\.otherExp").val();
        freightCharges = $("#pjvVoucher\\.itemStock\\.freightCharges").val();

        $("#pjvVoucher\\.itemStock\\.otherExp").val(formatterNoFloat.format(otherExp).replace(/[^0-9\.-]+/g, ""));
        $("#pjvVoucher\\.itemStock\\.freightCharges").val(formatterNoFloat.format(freightCharges).replace(/[^0-9\.-]+/g, ""));

        purchaseAmount = $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.amount").val();
        /*  if (otherExp > 0) {
              creditCharges = parseFloat(otherExp.replace(/[^0-9\.-]+/g, "")) + parseFloat(bankTaxAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(millTaxAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(freightCharges.replace(/[^0-9\.-]+/g, ""));
              debitCharges = parseFloat(bardanaAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(silaiAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(unloadingAmount);
          } else {*/
        creditCharges = parseFloat(otherExp.replace(/[^0-9\.-]+/g, "")) + parseFloat(bankTaxAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(millTaxAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(freightCharges.replace(/[^0-9\.-]+/g, ""));
        debitCharges = parseFloat(bardanaAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(silaiAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(unloadingAmount);
        //}
        //allCharges = parseFloat(bankTaxAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(millTaxAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(bardanaAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(silaiAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(unloadingAmount.replace(/[^0-9\.-]+/g, "")) + parseFloat(otherExp.replace(/[^0-9\.-]+/g, "")) + parseFloat(freightCharges.replace(/[^0-9\.-]+/g, ""));
        console.log(purchaseAmount);
        $("#pjvVoucher\\.itemStock\\.netAmount").val(formatter.format(purchaseAmount.replace(/[^0-9\.-]+/g, "") - creditCharges + debitCharges));

        $("#pjvVoucher_entry_table tbody tr:eq(0) td:eq(5) input").val(purchaseAmount.replace(/[^0-9\.-]+/g, "") - creditCharges + debitCharges);
        $("#pjvVoucher_entry_table tbody tr:eq(1) td:eq(4) input").val(purchaseAmount.replace(/[^0-9\.-]+/g, ""));

        $("#pjvVoucher_entry_table tbody tr:eq(2) td:eq(4) input").val(unloadingAmount);
        $("#pjvVoucher_entry_table tbody tr:eq(2) td:eq(5) input").val(0);
        $("#pjvVoucher_entry_table tbody tr:eq(3) td:eq(5) input").val(freightCharges.replace(/[^0-9\.-]+/g, ""));
        $("#pjvVoucher_entry_table tbody tr:eq(3) td:eq(4) input").val(0);
        $("#pjvVoucher_entry_table tbody tr:eq(4) td:eq(5) input").val(millTaxAmount.replace(/[^0-9\.-]+/g, ""));
        $("#pjvVoucher_entry_table tbody tr:eq(4) td:eq(4) input").val(0);
        $("#pjvVoucher_entry_table tbody tr:eq(5) td:eq(5) input").val(bankTaxAmount.replace(/[^0-9\.-]+/g, ""));
        $("#pjvVoucher_entry_table tbody tr:eq(5) td:eq(4) input").val(0);
        $("#pjvVoucher_entry_table tbody tr:eq(6) td:eq(4) input").val(silaiAmount.replace(/[^0-9\.-]+/g, ""));
        if (parseFloat(otherExp.replace(/[^0-9\.-]+/g, "")) < 0) {

            $("#pjvVoucher_entry_table tbody tr:eq(7) td:eq(4) input").val(otherExp.replace(/[^0-9\.-]+/g, "") * (-1));
            $("#pjvVoucher_entry_table tbody tr:eq(7) td:eq(5) input").val(0);

        } else {

            $("#pjvVoucher_entry_table tbody tr:eq(7) td:eq(5) input").val((otherExp.replace(/[^0-9\.-]+/g, "")));
            $("#pjvVoucher_entry_table tbody tr:eq(7) td:eq(4) input").val(0);
        }
        // $("#pjvVoucher_entry_table tbody tr:eq(7) td:eq(5) input").val(0);
        $("#pjvVoucher_entry_table tbody tr:eq(8) td:eq(4) input").val(bardanaAmount.replace(/[^0-9\.-]+/g, ""));
        $("#pjvVoucher_entry_table tbody tr:eq(8) td:eq(5) input").val(0);
        $("#pjvVoucher_entry_table tbody tr:eq(9) td:eq(5) input").val(brokriAmount.replace(/[^0-9\.-]+/g, ""));
        $("#pjvVoucher_entry_table tbody tr:eq(9) td:eq(4) input").val(0);
        $("#pjvVoucher_entry_table tbody tr:eq(9) td:eq(3) textarea").text("BROKRY");

        $("#pjvVoucher_entry_table tbody tr:eq(10) td:eq(4) input").val(brokriAmount.replace(/[^0-9\.-]+/g, ""));
        $("#pjvVoucher_entry_table tbody tr:eq(10) td:eq(5) input").val(0);
        $("#pjvVoucher_entry_table tbody tr:eq(10) td:eq(3) textarea").text("BROKRY");

        pjcalculateAmountEntriesTotal();


    }

    function calcPercentage(amount, percentage) {
        return ((amount * percentage) / 100).toFixed(4);
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
        validateAndSubmit();
    });

    function validateAndSubmit() {
        if ($("#sjvVoucher\\.itemStock\\.itemStockEntries0\\.bags").val() < 1
            || $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.bags").val() < 1) {
            $.confirm({
                title: "Kindly Enter BAG number",
                type: 'red',
                animation: 'Rotate',
                icon: 'fa fa-warning',
                icon: 'glyphicon glyphicon-heart',
                autoClose: 'cancel|9000',
                content: "This will be not saved with out Bags",//.addClass('display').css('font-size', '12px'),

            });
            return
        }
        if ($("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.itemDef").val() < 1) {
            $.confirm({
                title: "Kindly Select Itme",
                type: 'red',
                animation: 'Rotate',
                icon: 'fa fa-warning',
                icon: 'glyphicon glyphicon-heart',
                autoClose: 'cancel|9000',
                content: "This will be not saved with out Item",//.addClass('display').css('font-size', '12px'),

            });
            return

        }
        if ($("#sjvVoucher\\.itemStock\\.account\\.code").val() < 1 || $("#pjvVoucher\\.itemStock\\.account\\.code").val() < 1) {
            $.confirm({
                title: "Kindly Select Both Parties",
                type: 'red',
                animation: 'Rotate',
                icon: 'fa fa-warning',
                icon: 'glyphicon glyphicon-heart',
                autoClose: 'cancel|9000',
                content: "This will be not saved with out Both Parties",//.addClass('display').css('font-size', '12px'),

            });


        } else {
            $("#svoucherForm").submit();
            $("#pjvVoucher\\.itemStock\\.itemStockEntries0\\.bags").val(0);
        }
    }
});