Number.prototype.toFixed = function (decimalPlaces) {
    return Number(Math.round(this + 'e' + decimalPlaces) + 'e-' + decimalPlaces);
}

$(document).ready(function () {

    //on load
    restrictFromAndToDate();
    //loadCompanyBranches();

    $("#companyId").on("change", function () {
        loadCompanyBranches();
    });

    function loadCompanyBranches() {

        if (!$("#companyId").val() || $("#companyId").val() == 0) {
            return;
        }

        $.get("/vouchers/company_branches?companyId=" + $("#companyId").val(), function (data) {
            $("#branchId").empty();
            $("#branchId").append("<option value='0'>ALL</option>");

            for (var i = 0, len = data.length; i < len; i++) {
                var option = "<option value = " + data[i].id + ">" + data[i].name + "</option>";
                $("#branchId").append(option);
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

    loadItemCategoryDefs();

    $("#itemCategoryId").on("change", function () {
        loadItemCategoryDefs();
    });

    function loadItemCategoryDefs() {

        if (!$("#itemCategoryId").val() || $("#itemCategoryId").val() == 0) {
            return;
        }

        $.get("/receivables/category_item_defs?categoryId=" + $("#itemCategoryId").val(), function (data) {
            $("#itemDefId").empty();
            //$("#itemDefId").append("<option>&emsp;</option>");

            for (var i = 0, len = data.length; i < len; i++) {
                var option = "<option value = " + data[i].id + ">" + data[i].formattedCode + "&emsp;" + data[i].name + "</option>";
                $("#itemDefId").append(option);
            }
        });
    }

    function calculateTotal() {

        var oTotalQTY = parseFloat(0);

        var inTotalQTY = parseFloat(0);

        var outTotalQTY = parseFloat(0);

        var prTotalQTY = parseFloat(0);

        var srTotalQTY = parseFloat(0);

        var cTotalQTY = parseFloat(0);

        $("#gl_report_datatable tr").each(function () {
            if ($(this).find("td").length) {

//	    		oTotalQTY = oTotalQTY + parseFloat($(this).find("td:eq(4)").text() || 0);
//	    		
//	    		inTotalQTY = inTotalQTY + parseFloat($(this).find("td:eq(7)").text() || 0);
//	    		
//	    		outTotalQTY = outTotalQTY + parseFloat($(this).find("td:eq(10)").text() || 0);
//	    		
////	    		prTotalQTY = prTotalQTY + parseFloat($(this).find("td:eq(13)").text() || 0);
////	    		
////	    		srTotalQTY = srTotalQTY + parseFloat($(this).find("td:eq(16)").text() || 0);
//	    		
//	    		cTotalQTY = cTotalQTY + parseFloat($(this).find("td:eq(19)").text() || 0);

            }
        });
//		
//		$("#gl_report_datatable span#oTotalQTY").text(oTotalQTY);
//		
//		$("#gl_report_datatable span#inTotalQTY").text(inTotalQTY);
//		
//		$("#gl_report_datatable span#outTotalQTY").text(outTotalQTY);
//		
////		$("#gl_report_datatable span#prTotalQTY").text(prTotalQTY);
////		
////		$("#gl_report_datatable span#srTotalQTY").text(srTotalQTY);
//		
//		$("#gl_report_datatable span#cTotalQTY").text(cTotalQTY);

    }

    function getFormattedDate(date) {
        var year = date.getFullYear();
        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        return day + '/' + month + '/' + year;
    }

    // SUBMIT FORM
    $("#glReportForm").submit(function (event) {
        // Prevent the form from submitting via the browser.
        event.preventDefault();
        generateGLReport();
    });

    function generateGLReport() {

        var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 2, maximumFractionDigits: 2});

        $("#submitGLForm").attr("disabled", true);  // disable submit button
        $('#loading').show();  // show loading indicator

        // calculate all Totals
        //calculateTotal();

        if (!$("#companyId").val() || !$("#voucherStatusId").val() || !$("#financialYearId").val() || !$("#fromDate").val() || !$("#toDate").val() || !$("#accountCode").val()) {
            alert("PLEASE SELECT ALL THE FIELDS")
            return;
        }

        $('#gl_report_datatable').DataTable().clear();
        $('#gl_report_datatable').DataTable().destroy();
        var table = $('#gl_report_datatable').DataTable({


            dom: 'Bfrtip',
            deferRender: true,

            destroy: true,
            fixedHeader: true,
            bPaginate: false,
            buttons: [
                {
                    extend: 'copy',
                    text: 'COPY',
                    title: null,
                    footer: true,
                },
                $.extend(true, {}, getExcelBuilder(), {
                    extend: 'excelHtml5',
                    text: 'EXCEL',
                    title: null,
                    footer: true,
                    exportOptions: {
                        stripNewlines: false
                    },
                }),
                /*{
                    extend: 'excelHtml5',
                    title: 'General Ledger' + ' (' + (new Date().toLocaleString()) + ')',
                    messageTop: 'COMPANY: ' + 'MST' + '\n' + 'ACCOUNT: ' + '111222333',
                    messageBottom: null,
                    stripNewlines: false
                },*/
                {
                    extend: 'pdfHtml5',
                    orientation: 'landscape',
                    title: 'STOCK REPORT - QUANTITY' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    messageTop: 'COMPANY: ' + $("#companyId option:selected").text()
                        + '\n' + 'BRANCH: ' + $("#branchId option:selected").text()
                        + '\n' + 'VOUCHER STATUS: ' + $("#voucherStatusId option:selected").text()
                        + '\n' + 'FINANCIAL YEAR: ' + $("#financialYearId option:selected").text() + " (" + getFormattedDate(new Date($("#fromDate").val())) + " - " + getFormattedDate(new Date($("#toDate").val())) + ")"
                        + '\n' + 'ACCOUNT: ' + $("#accountCode option:selected").text(),
                    messageBottom: null,
                    footer: true,
                    customize: function (doc) {
                        doc.defaultStyle.fontSize = 8;
                        doc.pageMargins = [5, 5, 5, 5];
                        doc.styles.tableHeader.fontSize = 8;
                        doc.styles.tableFooter.fontSize = 8;
                        doc.defaultStyle.alignment = 'left';
                        doc.styles.tableHeader.alignment = 'left';
                        doc.styles.tableFooter.alignment = 'left';
                    }
                },
                {
                    extend: 'print',
                    text: 'VIEW',
                    title: 'STOCK REPORT - QUANTITY' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    messageTop: 'COMPANY: ' + $("#companyId option:selected").text()
                        + '<br>' + 'BRANCH: ' + $("#branchId option:selected").text()
                        + '<br>' + 'VOUCHER STATUS: ' + $("#voucherStatusId option:selected").text()
                        + '<br>' + 'FINANCIAL YEAR: ' + $("#financialYearId option:selected").text() + " (" + getFormattedDate(new Date($("#fromDate").val())) + " - " + getFormattedDate(new Date($("#toDate").val())) + ")"
                        + '<br>' + 'ACCOUNT: ' + $("#accountCode option:selected").text(),
                    messageBottom: null,
                    footer: true,
                    autoPrint: false,
                    exportOptions: {
                        stripHtml: false,
                        stripNewlines: false
                    },
                    customize: function (win) {
                        $(win.document.body).find('table').addClass('display').css('font-size', '12px');
                        $(win.document.body).find('tr:nth-child(odd) td').each(function (index) {
                            $(this).css('background-color', 'WHITESMOKE');
                        });
                        $(win.document.body).find('tr:nth-child(even) td').each(function (index) {
                            $(this).css('background-color', 'WHITE');
                        });

                        $(win.document.body).css('background-color', 'WHITE');
                        $(win.document.body).css("color", "DIMGRAY");

                        $(win.document.body).find('h1').css('text-align', 'center');
                        //$(win.document.body).find('h1').css("color", "DIMGRAY");

                        $(win.document.body).find('div:first').css('text-align', 'center');
                        //$(win.document.body).find('div:first').css("color", "DIMGRAY");

                        $(win.document.body).find('th').css("color", "WHITE");
                        $(win.document.body).find('th').css("background-color", "DIMGRAY");
                    }
                }
            ],

            "footerCallback": function (row, data, start, end, display) {
                var api = this.api(), data;

                var colNumber = [4, 6, 7, 8];

                // Remove the formatting to get integer data for summation
                var intVal = function (i) {
                    return typeof i === 'string' ?
                        i.replace(/[^0-9\.-]+/g, "") * 1 :
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
                    //$(api.column(colNo).footer()).html(parseFloat(total).toFixed(2));
                    $(api.column(colNo).footer()).html(formatter.format(parseFloat(total).toFixed(2)));
                }

            }
        });

        if ($("#reports_download").val() === "false") {
            table.buttons('.dt-button').remove();
        }

        // PREPARE FORM DATA
        var formData = {
            companyId: $("#companyId").val(),
            branchId: $("#branchId").val(),
            voucherStatusId: $("#voucherStatusId").val(),
            financialYearId: $("#financialYearId").val(),
            fromDate: $("#fromDate").val(),
            toDate: $("#toDate").val(),
            itemDefId: 0,
            itemCategoryId: $("#itemCategoryId").val(),
            accountCode: $("#accountCode").val(),
            negativePositive: $("#negativePositive").val(),
        }

        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: $("#glReportForm").attr("action"),
            data: JSON.stringify(formData),
            dataType: "json",
            success: function (data) {

                var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 2, maximumFractionDigits: 2});

                // FILL TABLE ROWS
                $.each(data, function (i, sLEntry) {

                    table.row.add([
                        (i + 1),
                        sLEntry.cat,
                        sLEntry.code,
                        sLEntry.item,
                        formatter.format(sLEntry.qty),
                        formatter.format(sLEntry.rate),
                        formatter.format(sLEntry.amount),
                        formatter.format(sLEntry.excetQty),
                        formatter.format(sLEntry.excetRate),
                    ]);
                });

                table.draw();
                //calculateTotal();  // calculate all Totals
                $("#submitGLForm").attr("disabled", false);  // enable submit button
                $('#loading').hide();  // hide loading indicator
            },
            complete: function () {
                $("#submitGLForm").attr("disabled", false);  // enable submit button
                $('#loading').hide();  // hide loading indicator
            },
        });

        //highlight
        $('#gl_report_datatable tbody').on('mouseenter', 'td', function () {
            if (table instanceof $.fn.dataTable.Api) {
                table.rows().eq(0).each(function (index) {
                    $(table.row(index).nodes()).removeClass('highlight');
                });
                $(table.cells().nodes()).removeClass('highlight');

                var rowIdx = table.cell(this).index().row;
                var colIdx = table.cell(this).index().column;

                $(table.row(rowIdx).nodes()).addClass('highlight');
                $(table.column(colIdx).nodes()).addClass('highlight');
            }
        });
    }

    function getExcelBuilder() {
        var xlsBuilder = {
            filename: 'STOCK REPORT - QUANTITY_' + new Date().toLocaleString(),
            sheetName: 'sheet1',
            customize: function (xlsx) {
                var sheet = xlsx.xl.worksheets['sheet1.xml'];
                var downrows = 8;
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

});