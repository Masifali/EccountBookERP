Number.prototype.toFixed = function (decimalPlaces) {
    return Number(Math.round(this + 'e' + decimalPlaces) + 'e-' + decimalPlaces);
}
$(document).ready(function () {
    var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 2, maximumFractionDigits: 2});
    //console.log("bilala");
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

        }


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
            $("#itemDefId").append("<option value='0'>ALL</option>");

            for (var i = 0, len = data.length; i < len; i++) {
                var option = "<option value = " + data[i].id + ">" + data[i].formattedCode + "&emsp;" + data[i].name + "</option>";
                $("#itemDefId").append(option);
            }
        });
    }

    function calculateTotal() {

        var inTotal = parseFloat(0);
        var prTotal = parseFloat(0);
        var outTotal = parseFloat(0);
        var srTotal = parseFloat(0);

        $("#gl_report_datatable tr").each(function () {
            if ($(this).find("td").length) {

                inTotal = inTotal + parseFloat($(this).find("td:eq(6)").text() || 0);
                prTotal = prTotal + parseFloat($(this).find("td:eq(7)").text() || 0);
                outTotal = outTotal + parseFloat($(this).find("td:eq(8)").text() || 0);
                srTotal = srTotal + parseFloat($(this).find("td:eq(10)").text() || 0);

            }
        });

        $("#gl_report_datatable span#inTotal").text(inTotal);
        $("#gl_report_datatable span#prTotal").text(prTotal);
        $("#gl_report_datatable span#outTotal").text(outTotal);
        $("#gl_report_datatable span#srTotal").text(srTotal);

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


        $("#submitGLForm").attr("disabled", true);  // disable submit button
        $('#loading').show();  // show loading indicator

        // calculate all Totals
        calculateTotal();

        if (!$("#companyId").val() || !$("#fromDate").val() || !$("#toDate").val() || !$("#accountCode").val()) {
            alert("PLEASE SELECT ALL THE FIELDS")
            return;
        }

        $('#gl_report_datatable').DataTable().clear();
        $('#gl_report_datatable').DataTable().destroy();
        var table = $('#gl_report_datatable').DataTable({
            dom: 'Bfrtip',
            deferRender: true,
            // scrollY: 500,
            // scroller: true,
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
                    title: 'SALE REPORT' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    messageTop: 'COMPANY: ' + $("#companyId option:selected").text()
                        + '  ' + 'BRANCH: ' + $("#branchId option:selected").text()
                        + '  ' + 'ITEM: ' + $("#itemDefId option:selected").text()
                        + '   ' + 'ACCOUNT: ' + $("#accountCode option:selected").text(),
                    messageBottom: null,
                    footer: true,
                    exportOptions: {
                        columns: [0, 1, 2, 3, 4, 5, 6, 7],
                        stripNewlines: false,
                    },
                    customize: function (doc) {
                        doc.defaultStyle.fontSize = 8;
                        doc.pageMargins = [15, 15, 10, 20]; //1st left side ,2th header, 3th right side ,4th buttion
                        doc.styles.tableHeader.fontSize = 8;
                        doc.styles.tableFooter.fontSize = 8;
                        doc.defaultStyle.alignment = 'left';
                        doc.styles.tableHeader.alignment = 'left';
                        doc.styles.tableFooter.alignment = 'left';
                        doc['footer'] = (function (page, pages) {
                            return {
                                columns: [
                                    '',
                                    {
                                        // This is the right column
                                        alignment: 'right',
                                        text: ['page ', {text: page.toString()}, ' of ', {text: pages.toString()}]
                                    }
                                ],
                                margin: [10, 0]// [left or right , up or down]
                            }
                        });
                    }
                },
                {
                    extend: 'print',
                    title: 'SALE REPORT' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    messageTop: 'COMPANY: ' + $("#companyId option:selected").text()
                        + '  ' + 'BRANCH: ' + $("#branchId option:selected").text()
                        + '  ' + 'ITEM: ' + $("#itemDefId option:selected").text()
                        + '   ' + 'ACCOUNT: ' + $("#accountCode option:selected").text(),
                    messageBottom: null,
                    footer: true,
                    autoPrint: false,
                    exportOptions: {
                        columns: [0, 1, 2, 3, 4, 5, 6, 7],
                        stripNewlines: false,
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

                var colNumber = [10];

                // Remove the formatting to get integer data for summation
                var intVal = function (i) {
                    return typeof i === 'string' ?
                        //  i.replace(/[\$,]/g, '') * 1 :
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
                            console.log(a + " == " + b + "  total:" + total)
                            return intVal(a) + intVal(b);
                        }, 0);
                    $(api.column(colNo).footer()).html(formatter.format(parseFloat(total).toFixed(2)));
                }
            }
        });


        // PREPARE FORM DATA
        var formData = {
            companyId: $("#companyId").val(),
            branchId: $("#branchId").val(),
            voucherStatusId: '0',//$("#voucherStatusId").val(),
            financialYearId: '0',// $("#financialYearId").val(),
            fromDate: $("#fromDate").val(),
            toDate: $("#toDate").val(),
            itemDefId: $("#itemDefId").val(),
            itemCategoryId: $("#itemCategoryId").val(),
            accountCode: $("#accountCode").val(),
            serchByDate: '0',//$("#search_by_date").val(),
        }

        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: $("#glReportForm").attr("action"),
            data: JSON.stringify(formData),
            dataType: "json",
            success: function (data) {
                $("#grossProfit").text(data.grossProfit);
                $("#expenses").text(data.expenses);
                $("#netProfit").text(data.netProfit);
                $("#others").text(data.others);
                $.each(data.entries, function (i, sLEntry) {

                    var voucherViewLink;
                    if (sLEntry.voucherCode) {
                        if (sLEntry.voucherCode.indexOf("STV") >= 0) {
                            voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/SaleQty/view_pjv_sjv_qty/" + sLEntry.id + ">" + sLEntry.voucherCode + "</a>";
                        } else if (sLEntry.voucherCode.indexOf("SJV") >= 0) {
                            voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/sale_journal_vouchers/" + sLEntry.id + ">" + sLEntry.voucherCode + "</a>";
                        } else if (sLEntry.voucherCode.indexOf("SRO") >= 0) {
                            voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/production-voucher-view/" + sLEntry.id + ">" + sLEntry.voucherCode + "</a>";
                        } else if (sLEntry.voucherCode.indexOf("SQV") >= 0) {
                            voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/sale-quantity-vouchers/" + sLEntry.id + ">" + sLEntry.voucherCode + "</a>";
                        } else {
                            voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/vouchers/" + sLEntry.id + ">" + sLEntry.voucherCode + "</a>";
                        }
                    }
                    table.row.add([
                        (i + 1),


                        sLEntry.formatedCode,
                        sLEntry.accountName,
                        sLEntry.openingQty,
                        sLEntry.itemName,
                        sLEntry.itemQuantity,
                        /*sLEntry.fRate,*/
                        sLEntry.openingRate,
                        sLEntry.saleRate,
                        getFormattedDate(new Date(sLEntry.voucherDate)),
                        sLEntry.voucherCode ? voucherViewLink : "",
                        /*  formatter.format(sLEntry.fProfit),*/
                        formatter.format(sLEntry.profit),

                        sLEntry.createdBy,

                    ]);
                });

                table.draw();

                calculateTotal();  // calculate all Totals
                $("#submitGLForm").attr("disabled", false);  // enable submit button
                // hide loading indicator
                setFootterValue();
            },
            complete: function () {
                $("#submitGLForm").attr("disabled", false);  // enable submit button
                $('#loading').hide();  // hide loading indicator
            },
        });

        //highlight
        /*$('#gl_report_datatable tbody').on( 'mouseenter', 'td', function () {

            table.rows().eq(0).each(function (index) {
                $(table.row(index).nodes()).removeClass('highlight');
            });
            //$(table.cells().nodes()).removeClass('highlight');

            var rowIdx = table.cell(this).index().row;
            //var colIdx = table.cell(this).index().column;

            $(table.row(rowIdx).nodes()).addClass('highlight');
            //$(table.column(colIdx).nodes()).addClass('highlight');
        });*/
    }

    function getExcelBuilder() {
        var xlsBuilder = {
            filename: 'SL_' + new Date().toLocaleString(),
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
                    v: 'SALES REPORT' + ' ('
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
                    v: 'ITEM :'
                }, {
                    k: 'B',
                    v: $("#itemDefId option:selected").text()
                }]);
                var r7 = Addrow(7, [{
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
                    + r7
                    + sheet.childNodes[0].childNodes[1].innerHTML;
            },
            /*
             * exportOptions: { columns: [0, 1, 2, 3] }
             */
        }
        return xlsBuilder;
    }

    function setFootterValue() {
        var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 0, maximumFractionDigits: 0});


    }
});