Number.prototype.toFixed = function (decimalPlaces) {
    return Number(Math.round(this + 'e' + decimalPlaces) + 'e-' + decimalPlaces);
}

$(document).ready(function () {
    var formatter4 = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 4, maximumFractionDigits: 4});
    $("#save_breakdownCalculatedBit").on("click", function () {

        if ($("#inventory_costing_datatable tbody td").length > 1) {
            $.confirm({
                title: "CONFIRMATION REQUIRED",
                content: "ARE YOU SURE YOU WANT TO SAVE THIS BREAKDOWN?",
                buttons: {
                    confirm: function () {

                        // PREPARE FORM DATA
                        var formData = {
                            companyId: $("#companyId").val(),
                            branchId: $("#branchId").val(),
                            voucherStatusId: $("#voucherStatusId").val(),
                            financialYearId: $("#financialYearId").val(),
                            financialMonthId: $("#financialMonthId").val(),
                            fromDate: null,
                            toDate: null,
                        }

                        // DO POST
                        $.ajax({
                            type: "POST",
                            contentType: "application/json",
                            url: "/costing/inventory_costing_fifo_calculate_bit",
                            data: JSON.stringify(formData),
                            dataType: "json",
                            success: function (data) {
                                if (data > 0) {
                                    // SUCCESS MESSAGE
                                    window.location = location.protocol + "//" + location.host + "/vouchers/" + data;
                                } else {
                                    $.confirm({
                                        title: "ENCOUNTERED AN ERROR!",
                                        content: "NO DATA AVAILABLE, PLEASE CHANGE SELECTION CRITERIA AND TRY AGAIN",
                                        type: 'red',
                                        typeAnimated: true,
                                    });
                                }
                            },
                        });

                    },
                    cancel: function () {
                        // do nothing
                    },
                }
            });
        } else {
            $.confirm({
                title: "ENCOUNTERED AN ERROR!",
                content: "NO DATA AVAILABLE, PLEASE CHANGE SELECTION CRITERIA AND TRY AGAIN",
                type: 'red',
                typeAnimated: true,
            });
        }


    });
    $("#save_breakdown").on("click", function () {

        if ($("#inventory_costing_datatable tbody td").length > 1) {
            $.confirm({
                title: "CONFIRMATION REQUIRED",
                content: "ARE YOU SURE YOU WANT TO SAVE THIS BREAKDOWN?",
                buttons: {
                    confirm: function () {

                        // PREPARE FORM DATA
                        var formData = {
                            companyId: $("#companyId").val(),
                            branchId: $("#branchId").val(),
                            voucherStatusId: $("#voucherStatusId").val(),
                            financialYearId: $("#financialYearId").val(),
                            financialMonthId: $("#financialMonthId").val(),
                            fromDate: null,
                            toDate: null,
                        }

                        // DO POST
                        $.ajax({
                            type: "POST",
                            contentType: "application/json",
                            url: "/costing/save_inventory_costing",
                            data: JSON.stringify(formData),
                            dataType: "json",
                            success: function (data) {
                                if (data > 0) {
                                    // SUCCESS MESSAGE
                                    window.location = location.protocol + "//" + location.host + "/vouchers/" + data;
                                } else {
                                    $.confirm({
                                        title: "ENCOUNTERED AN ERROR!",
                                        content: "NO DATA AVAILABLE, PLEASE CHANGE SELECTION CRITERIA AND TRY AGAIN",
                                        type: 'red',
                                        typeAnimated: true,
                                    });
                                }
                            },
                        });

                    },
                    cancel: function () {
                        // do nothing
                    },
                }
            });
        } else {
            $.confirm({
                title: "ENCOUNTERED AN ERROR!",
                content: "NO DATA AVAILABLE, PLEASE CHANGE SELECTION CRITERIA AND TRY AGAIN",
                type: 'red',
                typeAnimated: true,
            });
        }


    });

    function getFormattedDate(date) {
        console.log(date);
        var year = date.getFullYear();
        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        return day + '/' + month + '/' + year;
    }

    function getFormattedDateISO(date) {
        return [date.getFullYear(), ('0' + (date.getMonth() + 1)).slice(-2), ('0' + date.getDate()).slice(-2)].join('-');
    }

    //on load
    restrictFromAndToDate();

    loadCompanyBranches();

    $("#companyId").on("change", function () {
        loadCompanyBranches();
    });

    function loadCompanyBranches() {

        if (!$("#companyId").val() || $("#companyId").val() == 0) {
            return;
        }

        $.get("/vouchers/company_branches?companyId=" + $("#companyId").val(), function (data) {
            $("#branchId").empty();
            /*$("#branchId").append("<option value='0'>ALL</option>");*/

            for (var i = 0, len = data.length; i < len; i++) {
                var option = "<option value = " + data[i].id + ">" + data[i].name + "</option>";
                $("#branchId").append(option);
            }
        });
    }

    loadFinancialMonths();

    var monthNames = ["JANUARY", "FEBRUARY", "MARCH", "APRIL", "MAY", "JUNE", "JULY", "AUGUST", "SEPTEMBER", "OCTOBER", "NOVEMBER", "DECEMBER"];

    $("#financialYearId").on("change", function () {
        loadFinancialMonths();
    });

    function loadFinancialMonths() {

        if (!$("#financialYearId").val() || $("#financialYearId").val() == 0) {
            return;
        }

        $.get("/costing/financial_months?financialYearId=" + $("#financialYearId").val(), function (data) {
            $("#financialMonthId").empty();
            for (var i = 0, len = data.financialYearMonths.length; i < len; i++) {
                var date = new Date(data.financialYearMonths[i].monthYear);
                var lastDayDate = new Date(date.getFullYear(), date.getMonth() + 1, 0);
                var option = "<option value = " + data.financialYearMonths[i].id + ">" + monthNames[date.getMonth()] + "&emsp;(" + getFormattedDate(date) + " - " + getFormattedDate(lastDayDate) + ")</option>";
                $("#financialMonthId").append(option);
            }
        });
    }

    $("#financialYearId").on("change", function () {
        restrictFromAndToDate();
    });

    function restrictFromAndToDate() {

        if (!$("#financialYearId").val()) {
            return;
        }

        $("#fromDate").attr("min", $("#financialYearId option:selected").attr("data-value").split("|")[0]);
        $("#fromDate").attr("max", $("#financialYearId option:selected").attr("data-value").split("|")[1]);
        $("#fromDate").val($("#financialYearId option:selected").attr("data-value").split("|")[0]);

        $("#toDate").attr("min", $("#financialYearId option:selected").attr("data-value").split("|")[0]);
        $("#toDate").attr("max", $("#financialYearId option:selected").attr("data-value").split("|")[1]);
        $("#toDate").val($("#financialYearId option:selected").attr("data-value").split("|")[1]);

    }

    function calculateTotal() {

        var saleAmountTotal = parseFloat(0.00);
        var purchaseAmountTotal = parseFloat(0.00);

        var totalWeight = parseFloat(0);
        var totalFeet = parseFloat(0);

        var qtyAllocTotal = parseFloat(0);

        var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 2, maximumFractionDigits: 2});

        $("#inventory_costing_datatable tr").each(function () {
            if ($(this).find("td").length) {
                saleAmountTotal = saleAmountTotal + parseFloat($(this).find("td:eq(5)").text().replace(/[^0-9\.-]+/g, "") || 0);
                purchaseAmountTotal = purchaseAmountTotal + parseFloat($(this).find("td:eq(10)").text().replace(/[^0-9\.-]+/g, "") || 0);

                totalWeight = totalWeight + parseFloat($(this).find("td:eq(11)").text() || 0);
                totalWeight = parseFloat((totalWeight).toFixed(2));

                totalFeet = totalFeet + parseFloat($(this).find("td:eq(12)").text() || 0);
                totalFeet = parseFloat((totalFeet).toFixed(2));

                qtyAllocTotal = qtyAllocTotal + parseFloat($(this).find("td:eq(7)").text() || 0);
            }
        });

        $("#inventory_costing_datatable span#saleAmountTotal").text(formatter.format(saleAmountTotal));
        $("#inventory_costing_datatable span#purchaseAmountTotal").text(formatter.format(purchaseAmountTotal));

        $("#inventory_costing_datatable span#totalWeight").text(totalWeight);
        $("#inventory_costing_datatable span#totalFeet").text(totalFeet);

        $("#inventory_costing_datatable span#qtyAllocTotal").text(qtyAllocTotal);
    }

    // SUBMIT FORM
    $("#inventoryCostingForm").submit(function (event) {
        // Prevent the form from submitting via the browser.
        event.preventDefault();
        getSalesBreakdown($("#inventoryCostingForm").attr("action"));
        $("#save_breakdown").removeAttr("disabled");
    });
    $("#inventoryCostingWithBit").on("click", function () {
        event.preventDefault();
        getSalesBreakdown("/costing/inventory_costing_fifo_calculate_bit");
    })

    function getSalesBreakdown(url) {

        var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 2, maximumFractionDigits: 2});

        $("#get_breakdown").attr("disabled", true);  // disable submit button
        $('#loading').show();  // show loading indicator

        // calculate all Totals
        //calculateTotal();

        if (!$("#companyId").val() || !$("#voucherStatusId").val() || !$("#financialYearId").val()) {
            alert("PLEASE SELECT ALL THE FIELDS");
            return;
        }

        $('#inventory_costing_datatable').DataTable().clear();
        $('#inventory_costing_datatable').DataTable().destroy();
        var table = $('#inventory_costing_datatable').DataTable({
            dom: 'Bfrtip',
            'bPaginate': false,
            fixedHeader: true,
//	        scrollY: 500,
//	        scroller: true,
//	        destroy: true,
            buttons: [
                {extend: 'copyHtml5', footer: true},
                {extend: 'excelHtml5', footer: true},
                {extend: 'pdfHtml5', footer: true},
                {extend: 'print', footer: true},
            ],
            footerCallback: function (row, data, start, end, display) {
                var api = this.api(), data;

                var colNumber = [4, 7, 10, 11];

                // Remove the formatting to get integer data for summation
                var intVal = function (i) {
                    return typeof i === 'string' ?
                        i.replace(/[\$,]/g, '') * 1 :
                        typeof i === 'number' ?
                            i.toFixed(2) : 0;
                };

                for (i = 0; i < colNumber.length; i++) {
                    var colNo = colNumber[i];
                    var total = api
                        .column(colNo)
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);
                    $(api.column(colNo).footer()).html(formatter.format(parseFloat(total).toFixed(2)));
                }
            },

        });

        // PREPARE FORM DATA
        var formData = {
            companyId: $("#companyId").val(),
            branchId: $("#branchId").val(),
            voucherStatusId: $("#voucherStatusId").val(),
            financialYearId: $("#financialYearId").val(),
            financialMonthId: $("#financialMonthId").val(),
            fromDate: null,
            toDate: null,
        }

        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            //  url: $("#inventoryCostingForm").attr("action"),
            url: url,
            data: JSON.stringify(formData),
            dataType: "json",
            success: function (data) {

                var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 0, maximumFractionDigits: 0});

                // FILL TABLE ROWS
                $.each(data, function (i, breakDown) {
                    console.log(breakDown.monthYear);
                    //var charView  = breakDown. ;
                    table.row.add([
                        (i + 1),
                        breakDown.itemCategoryName,
                        breakDown.itemDefCode + '&emsp;' + breakDown.itemSubCategoryName + ' ' + breakDown.itemDefName,

                        breakDown.voucherDate ? getFormattedDate(new Date(breakDown.voucherDate)) : "",
                        breakDown.quantityToAlloc,
                        breakDown.quantityAlloc,
                        breakDown.saleVoucherId ? "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/sale_journal_vouchers/" + breakDown.saleVoucherId + ">" + breakDown.saleVoucherCode + "</a>" : breakDown.purchaseVoucherCode,

                        // breakDown.purchaseVoucherId ? "<a style='text-decoration: underline;' target=_blank href="+location.protocol + "//" + location.host+"/purchase_journal_vouchers/"+breakDown.purchaseVoucherId+">"+breakDown.purchaseVoucherCode+"</a>" : "",
                        formatter4.format(breakDown.purchasePrice),
                        /* breakDown.purchaseTotalWeight,*/
                        formatter.format(breakDown.purchaseAmount),
                        formatter.format(breakDown.salePrice),
                        formatter.format(breakDown.saleAmount),
                        formatter.format(breakDown.saleAmount - breakDown.purchaseAmount),
//                        formatter.format(breakDown.salePrice),
//                        /* breakDown.saleTotalWeight,
//                         breakDown.totalFeet,*/
//                         formatter.format(breakDown.saleAmount),
                    ]);
                });

                table.draw();
                $('#inventory_costing_datatable').find('tr').each(function (ix, row) {
                    var index = ix;
                    var rowElt = row;

                    console.log("INVENTORY COSTING TABLE " + $(this).find("td:eq(5)").text());
                    if ($(this).find("td:eq(6)").text().indexOf("SJV") >= 0) {
                        // $(this).css("font-weight","bold");
                        $(this).css("background-color", "#90EE90");
                    }
                    if ($(this).find("td:eq(6)").text() == "BALANCE") {
                        $(this).css("font-weight", "bold");
                        // $(this).css("background-color", "RED");
                    }
                    if ($(this).find("td:eq(6)").text().indexOf("PJV") >= 0) {
                        // $(this).css("font-weight","bold");
                        $(this).css("background-color", "YELLOW");
                    }
                });

                //calculateTotal();  // calculate all Totals
                $("#get_breakdown").attr("disabled", false);  // enable submit button
                $('#loading').hide();  // hide loading indicator
            },
            complete: function () {
                $("#get_breakdown").attr("disabled", false);  // enable submit button
                $('#loading').hide();  // hide loading indicator
                loadCostingVouchers();
            },
        });


    }

    function getExcelBuilder() {
        var xlsBuilder = {
            filename: 'GL_' + new Date().toLocaleString(),
            sheetName: 'sheet1',
            customize: function (xlsx) {
                var sheet = xlsx.xl.worksheets['sheet1.xml'];
                var downrows = 7;
                var clRow = $('row', sheet);
                var msg;
                // update Row
                clRow.each(function () {
                    var attr = $(this).attr('r');
                    var ind = parseInt(attr);
                    ind = ind + downrows;
                    $(this).attr("r", ind);
                });

                // Update row > c
                $('row c ', sheet).each(
                    function () {
                        var attr = $(this).attr('r');
                        var pre = attr.substring(0, 1);
                        var ind = parseInt(attr.substring(
                            1, attr.length));
                        ind = ind + downrows;
                        $(this).attr("r", pre + ind);
                    });

                function Addrow(index, data) {

                    msg = '<row xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" r="'
                        + index + '">';
                    for (var i = 0; i < data.length; i++) {
                        var key = data[i].k;
                        var value = data[i].v;
                        msg += '<c t="inlineStr" r="' + key
                            + index + '">';
                        msg += '<is>';
                        msg += '<t>' + value + '</t>';
                        msg += '</is>';
                        msg += '</c>';
                    }
                    msg += '</row>';
                    return msg;
                }

                var r1 = Addrow(1, [{
                    k: 'A',
                    v: 'GENERAL LEDGER' + ' ('
                        + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim()
                        + ')'
                }]);
                var r2 = Addrow(2, [{
                    k: 'A',
                    v: 'COMPANY :'
                }, {
                    k: 'B',
                    v: $("#companyId option:selected").text()
                }]);
                var r3 = Addrow(3, [{
                    k: 'A',
                    v: 'BRANCH :'
                }, {
                    k: 'B',
                    v: $("#branchId option:selected").text()
                }]);
                var r4 = Addrow(4, [{
                    k: 'A',
                    v: 'VOUCHER STATUS :'
                }, {
                    k: 'B',
                    v: $("#voucherStatusId option:selected").text()
                }]);
                var r5 = Addrow(5, [{
                    k: 'A',
                    v: 'FINANCIAL YEAR :'
                }, {
                    k: 'B',
                    v: $("#financialYearId option:selected").text() + " (" + getFormattedDate(new Date($("#fromDate").val())) + " - " + getFormattedDate(new Date($("#toDate").val())) + ")"
                }]);
                var r6 = Addrow(6, [{
                    k: 'A',
                    v: 'ACCOUNT :'
                }, {
                    k: 'B',
                    v: $("#accountCode option:selected").text()
                }]);

                sheet.childNodes[0].childNodes[1].innerHTML = r1
                    + r2
                    + r3
                    + r4
                    + r5
                    + r6
                    + sheet.childNodes[0].childNodes[1].innerHTML;
            },
            /*
             * exportOptions: { columns: [0, 1, 2, 3] }
             */
        }
        return xlsBuilder;
    }

    function loadCostingVouchers() {
        $("#inventory_costing_vouchers").empty();
        $.get("/costing/costing_vouchers?branchId=" + $("#branchId").val(), function (data) {
            for (var i = 0, len = data.length; i < len; i++) {
                var voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/vouchers/" + data[i].voucherId + ">" + data[i].voucherCode + "</a>";
                var trHTML = "<tr><td>" + (i + 1) + "</td><td>" + voucherViewLink + "</td><td>" + getFormattedDate(new Date(data[i].monthYear)) + "</td></tr>";
                $("#inventory_costing_vouchers").append(trHTML);
            }
        });
    }

});