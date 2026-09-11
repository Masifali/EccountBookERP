$(document).ready(function () {

    //on load
    restrictFromAndToDate();
    getHeaders();
    //loadFromAndToAccounts();
    var buttionName = "";

    $("#companyIds").on("change", function () {
        loadCompanyBranches();
    });

    $("#reportType").on("change", function () {
        getHeaders();
    });

    $("#financialYearId").on("change", function () {
        getHeaders();
    });
    $("#profitType").on("change", function () {
        var tradingSelectedItemCodes = [
            111031001, 111041001, 111041002, 111041003, 111041004,
            111041005, 111041006, 111041007, 111041008, 111041009,
            111041010, 111041011, 111041012, 111041013, 111041014,
            111041015, 111041016

        ];
        var millSelectedItemCodes = [
            111051001, 111051002, 111051003, 111051004, 111051005, 111051006, 111051007,
            111051008, 111051010, 111061001, 111061002, 111061003, 111061004,
            111061005, 111061006, 111061007, 111061008, 111061009, 111061010, 111061011,
            111061012, 111061013, 111061014, 111061015, 111061016, 111061017, 111071001,
            111071002, 111071003, 111071004, 111071005, 111071006, 111071007, 111071008,
            111071009, 111071011, 111071012, 111071013, 111071014, 111071015,
            111071016, 111071017, 111071018, 111071019, 111071020, 111071021, 111081001,
            111081002, 111081003, 111081004, 111081005, 111081006, 111081007, 111091001,
            111091002, 111091003, 111091004, 111091005, 111091006
        ];

        if ($("#profitType").val() === 'trading') {
            $("#saleItemDefIds").val(tradingSelectedItemCodes).trigger('change');
            $("#costItemDefIds").val(tradingSelectedItemCodes).trigger('change');
        } else if ($("#profitType").val() === 'mill') {
            $("#saleItemDefIds").val(millSelectedItemCodes).trigger('change');
        } else {

            $("#saleItemDefIds").val([0]).trigger('change');
            $("#costItemDefIds").val([0]).trigger('change');

        }
    });

    function getHeaders() {
        // console.log('GETTING HEADERS ');
        $("#tblHead1, #tblHead2, #tblHead3, #tblHead4, #tblHead5, #tblHead6, #tblHead7, #tblHead8, #tblHead9, #tblHead10, #tblHead11, #tblHead12").text("-");

        $.get("/reports/getReportHeaders?reportType=" + $("#reportType option:selected").val() + "&fiscalId=" + $("#financialYearId option:selected").val(), function (data) {
            $.each(data, function (i, tBEntry) {
                var myValue = tBEntry.toString(); //.split("-");

                //var myValue = tBEntry.toString().split("-");
                //var myValue1 = tBEntry.toString().split("-","\n");
                //var myShowValue = "<span>" + myValue[0] + "</span><span>" + myValue[1] + "</span>";

                if (i === 0) {
                    $("#tblHead1").text(myValue);
                    //$("#tblHead1").find("br").replaceWith("\n").end().text()
                } else if (i === 1) {
                    $("#tblHead2").text(myValue);
                } else if (i === 2) {
                    $("#tblHead3").text(myValue);
                } else if (i === 3) {
                    $("#tblHead4").text(myValue);
                } else if (i === 4) {
                    $("#tblHead5").text(myValue);
                } else if (i === 5) {
                    $("#tblHead6").text(myValue);
                } else if (i === 6) {
                    $("#tblHead7").text(myValue);
                } else if (i === 7) {
                    $("#tblHead8").text(myValue);
                } else if (i === 8) {
                    $("#tblHead9").text(myValue);
                } else if (i === 9) {
                    $("#tblHead10").text(myValue);
                } else if (i === 10) {
                    $("#tblHead11").text(myValue);
                } else if (i === 11) {
                    $("#tblHead12").text(myValue);
                }
            });
        });
    }

    function loadCompanyBranches() {

        if (!$("#companyIds").val() || $("#companyIds").val() == 0) {
            return;
        }

        $.get("/reports/companies_branches?companyIds=" + $("#companyIds").val(), function (data) {
            $("#branchIds").empty();
            $("#branchIds").append("<option value='0'>ALL</option>");

            for (var i = 0, len = data.length; i < len; i++) {
                var option = "<option value = " + data[i].id + ">" + data[i].name + "</option>";
                $("#branchIds").append(option);
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

        $("#tb_report_datatable tr").each(function () {
            if ($(this).find("td").length) {
                // SET COLORS
                if ($(this).find("span").length) {
                    $(this).css("color", "WHITE");
                    $(this).css("font-weight", "bold");
                    $(this).css("background-color", "DIMGRAY");
                }
            }
        });

        /*var openingBalanceTotal = parseFloat(0);
        var jul = parseFloat(0);
        var aug = parseFloat(0);
        var sep = parseFloat(0);
        var oct = parseFloat(0);
        var nov = parseFloat(0);
        var dec = parseFloat(0);
        var jan = parseFloat(0);
        var feb = parseFloat(0);
        var mar = parseFloat(0);
        var apr = parseFloat(0);
        var may = parseFloat(0);
        var jun = parseFloat(0);
        var closingBalanceTotal = parseFloat(0);

        var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 2, maximumFractionDigits: 2});

        $("#tb_report_datatable tr").each(function() {
            if ($(this).find("td").length) {

                // SET COLORS
                if ($(this).find("td:eq(16)").text().trim() === "true") {
                    $(this).css("color", "WHITE");
                    $(this).css("font-weight","bold");
                }
                if ($(this).find("td:eq(1)").text().trim().length == 5) {
                    $(this).css("color", "WHITE");
                    $(this).css("background-color", "DIMGRAY");
                }
                if ($(this).find("td:eq(1)").text().trim().length == 9) {
                    //$(this).css("color", "WHITE");
                    $(this).css("background-color", "WHITE");
                }

                if ($(this).find("td:eq(1)").text().trim().length == 2){
                    openingBalanceTotal = openingBalanceTotal + parseFloat($(this).find("td:eq(3)").text().replace(/[^0-9\.-]+/g, "") || 0);
                    jul = jul + parseFloat($(this).find("td:eq(4)").text().replace(/[^0-9\.-]+/g, "") || 0);
                    aug = aug + parseFloat($(this).find("td:eq(5)").text().replace(/[^0-9\.-]+/g, "") || 0);
                    sep = sep + parseFloat($(this).find("td:eq(6)").text().replace(/[^0-9\.-]+/g, "") || 0);
                    oct = oct + parseFloat($(this).find("td:eq(7)").text().replace(/[^0-9\.-]+/g, "") || 0);
                    nov = nov + parseFloat($(this).find("td:eq(8)").text().replace(/[^0-9\.-]+/g, "") || 0);
                    dec = dec + parseFloat($(this).find("td:eq(9)").text().replace(/[^0-9\.-]+/g, "") || 0);
                    jan = jan + parseFloat($(this).find("td:eq(10)").text().replace(/[^0-9\.-]+/g, "") || 0);
                    feb = feb + parseFloat($(this).find("td:eq(11)").text().replace(/[^0-9\.-]+/g, "") || 0);
                    mar = mar + parseFloat($(this).find("td:eq(12)").text().replace(/[^0-9\.-]+/g, "") || 0);
                    apr = apr + parseFloat($(this).find("td:eq(13)").text().replace(/[^0-9\.-]+/g, "") || 0);
                    may = may + parseFloat($(this).find("td:eq(14)").text().replace(/[^0-9\.-]+/g, "") || 0);
                    jun = jun + parseFloat($(this).find("td:eq(15)").text().replace(/[^0-9\.-]+/g, "") || 0);
                    closingBalanceTotal = closingBalanceTotal + parseFloat($(this).find("td:eq(16)").text().replace(/[^0-9\.-]+/g, "") || 0);
                }
            }
        });

        $("#tb_report_datatable tfoot tr:eq(0) th:eq(3)").text(formatter.format(openingBalanceTotal));
        $("#tb_report_datatable tfoot tr:eq(0) th:eq(4)").text(formatter.format(jul));
        $("#tb_report_datatable tfoot tr:eq(0) th:eq(5)").text(formatter.format(aug));
        $("#tb_report_datatable tfoot tr:eq(0) th:eq(6)").text(formatter.format(sep));
        $("#tb_report_datatable tfoot tr:eq(0) th:eq(7)").text(formatter.format(oct));
        $("#tb_report_datatable tfoot tr:eq(0) th:eq(8)").text(formatter.format(nov));
        $("#tb_report_datatable tfoot tr:eq(0) th:eq(9)").text(formatter.format(dec));
        $("#tb_report_datatable tfoot tr:eq(0) th:eq(10)").text(formatter.format(jan));
        $("#tb_report_datatable tfoot tr:eq(0) th:eq(11)").text(formatter.format(feb));
        $("#tb_report_datatable tfoot tr:eq(0) th:eq(12)").text(formatter.format(mar));
        $("#tb_report_datatable tfoot tr:eq(0) th:eq(13)").text(formatter.format(apr));
        $("#tb_report_datatable tfoot tr:eq(0) th:eq(14)").text(formatter.format(may));
        $("#tb_report_datatable tfoot tr:eq(0) th:eq(15)").text(formatter.format(jun));
        $("#tb_report_datatable tfoot tr:eq(0) th:eq(16)").text(formatter.format(closingBalanceTotal));*/
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
    $("#tbReportForm").submit(function (event) {
        // Prevent the form from submitting via the browser.

        $("#balanceSheetPacMan").show();
        buttionName = $("#submitTBForm").text();
        userLog();
        event.preventDefault();
        generateTBReport();
    });

    function generateTBReport() {

        // disable submit button
        $("#submitTBForm").attr("disabled", true);

        // calculate all Totals
        //calculateTotal();

        //if (!$("#companyIds").val() || !$("#voucherStatusId").val() || !$("#financialYearId").val() || !$("#fromDate").val() || !$("#toDate").val()){
        if (!$("#companyIds").val() || !$("#financialYearId").val()) {
            alert("PLEASE SELECT ALL THE FIELDS")
            return;
        }

        $('#tb_report_datatable').DataTable().clear();
        $('#tb_report_datatable').DataTable().destroy();
        var table = $('#tb_report_datatable').DataTable({
            dom: 'Bfrtip',
            bSort: false,
            'bPaginate': false,
            buttons: [
                {
                    extend: 'copy',
                    text: 'COPY',
                    title: null,
                    footer: true,
                    customize: function (doc) {
                        buttionName = "COPY";
                        userLog();
                    }
                },
                // $.extend(true, {}, getExcelBuilder(), {
                //     extend: 'excelHtml5',
                //     text: 'EXCEL',
                //     title: null,
                //     footer: true,
                //     exportOptions: {
                //         stripNewlines: false
                //     }
                // }),
                /*{
                    extend: 'excelHtml5',
                    title: 'General Ledger' + ' (' + (new Date().toLocaleString()) + ')',
                    messageTop: 'COMPANY: ' + 'MST' + '\n' + 'ACCOUNT: ' + '111222333',
                    messageBottom: null,
                    stripNewlines: false
                },*/
                {
                    extend: 'pdfHtml5',
                    download: 'open',
                    orientation: 'landscape',
                    title: 'FINANCIAL POSITION COMPARISON REPORT ' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    footer: true,
                    messageTop: 'COMPANY : ' + $('#companyIds option:selected').text()
                        + '\n' + 'BRANCH: ' + $('#branchIds option:selected').text()
                        + '\n' + 'VOUCHER STATUS : ' + $("#voucherStatusId option:selected").text()
                        + '\n' + 'FINANCIAL YEAR : ' + $("#financialYearId option:selected").text(),
                    messageBottom: null,
                    exportOptions: {
                        stripNewlines: false,
                        columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12],
                    },
                    customize: function (doc) {
                        doc.defaultStyle.fontSize = 6;
                        //pageMargins [left, top, right, bottom]
                        //doc.content[2].table.body[1][1].text.length
                        doc.pageMargins = [10, 10, 10, 10];
                        doc.styles.tableHeader.fontSize = 6;
                        doc.styles.tableFooter.fontSize = 6;
                        doc.defaultStyle.alignment = 'left';
                        doc.styles.tableHeader.alignment = 'left';
                        doc.styles.tableFooter.alignment = 'left';

                        /*doc.content[2].table.body[1].every( function ( rowIdx, tableLoop, rowLoop ) {
                            rowLoop[1].fillColor = 'black';
                            // ... do something with data(), or this.node(), etc
                        });*/
                        buttionName = "PDF";
                        userLog();
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
                    text: 'VIEW',
                    title: 'FINANCIAL POSITION COMPARISON REPORT ' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    messageTop: 'COMPANY: ' + $('#companyIds option:selected').text()
                        + '<br>' + 'BRANCH: ' + $('#branchIds option:selected').text()
                        + '<br>' + 'VOUCHER STATUS: ' + $("#voucherStatusId option:selected").text()
                        + '<br>' + 'FINANCIAL YEAR: ' + $("#financialYearId option:selected").text(),
                    messageBottom: null,
                    footer: true,
                    autoPrint: false,
                    exportOptions: {
                        stripHtml: false,
                        stripNewlines: false,
                        columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12],
                    },
                    customize: function (win) {
                        $(win.document.body).find('table').addClass('display').css('font-size', '12px');
                        /*$(win.document.body).find('tr:nth-child(odd) td').each(function(index){
                            $(this).css('background-color','WHITESMOKE');
                        });
                        $(win.document.body).find('tr:nth-child(even) td').each(function(index){
                            $(this).css('background-color','WHITE');
                        });*/

                        $(win.document.body).css('background-color', 'WHITE');
                        $(win.document.body).css("color", "DIMGRAY");
                        $(win.document.body).find('h1').css('text-align', 'center');
                        $(win.document.body).find('div:first').css('text-align', 'center');

                        $(win.document.body).find("tr").each(function () {
                            if ($(this).find("td").length) {
                                // SET COLORS
                                if ($(this).find("span").length) {
                                    $(this).css("color", "WHITE");
                                    $(this).css("font-weight", "bold");
                                    $(this).css("background-color", "DIMGRAY");
                                }
                            }
                            /*if ($(this).find("td:eq(1)").text().trim().length == 2){
                                $(this).find("td").each(function(index, td){
                                    $(td).css("color", "WHITE");
                                    $(td).css("background-color", "BLACK");
                                });
                            }
                            if ($(this).find("td:eq(1)").text().trim().length == 5){
                                $(this).find("td").each(function(index, td){
                                    $(td).css("color", "WHITE");
                                    $(td).css("background-color", "DIMGRAY");
                                });
                            }
                            if ($(this).find("td:eq(1)").text().trim().length == 9){
                                $(this).find("td").each(function(index, td){
                                    //$(td).css("color", "WHITE");
                                    $(td).css("background-color", "WHITE");
                                });
                            }*/
                        });
                        buttionName = "VIEW";
                        userLog();
                    }
                }
            ]
        });

        // PREPARE FORM DATA
        let formData = {
            companyIds: $("#companyIds").val(),
            branchIds: $("#branchIds").val(),
            financialYearId: $("#financialYearId").val(),
            profitType: $("#profitType").val(),
            level: $("#level").val(),
            reportType: $("#reportType").val(),
            itemSubCategoryIds: $("#itemSubCategoryIds").val(),
            saleItemDefIds: $("#saleItemDefIds").val(),
            costItemDefIds: $("#costItemDefIds").val(),
            fromDate: $("#fromDate").val(),
            toDate: $("#toDate").val(),
        }


        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: $("#tbReportForm").attr("action"),
            data: JSON.stringify(formData),
            dataType: "json",
            success: function (data) {
                // console.log("as");
                //var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 0, maximumFractionDigits: 0});
                let formatter = new Intl.NumberFormat('ur-PK', {
                    style: "currency",
                    currency: "PKR",
                    currencySign: "accounting",
                    minimumFractionDigits: 0, maximumFractionDigits: 0
                });
                // FILL TABLE ROWS
                let rowIndex = 0;
                $('#tb_report_datatable tbody').empty();
                let strHTML = "";
                $.each(data, function (i, tBEntry) {
                    rowIndex = rowIndex + 1;
                    var rowStyle = "";
                    var colStyle = "";
                    if (tBEntry.bold) {
                        colStyle = "font-weight: bold; color: " + tBEntry.color;
                    } else {
                        colStyle = "color: " + tBEntry.color;
                    }
                    rowStyle = "background-color: " + tBEntry.backgroundColor;
                    //formatter.format(tBEntry[1]);
                    strHTML += "<tr style='" + rowStyle + "'>" +
                        "<td style='" + colStyle + "'>" + tBEntry.accountCode + "</td>" +
                        "<td style='" + colStyle + "'>" + tBEntry.accountName + "</td>" +
                        "<td style='text-align: right; vertical-align: middle;" + colStyle + "'>" + formatter.formatToParts(tBEntry[1]).map(p => p.type != 'currency' ? p.value : '').join('') + "</td>" +
                        "<td style='text-align: right; vertical-align: middle;" + colStyle + "'>" + formatter.formatToParts(tBEntry[2]).map(p => p.type != 'currency' ? p.value : '').join('') + "</td>" +
                        "<td style='text-align: right; vertical-align: middle;" + colStyle + "'>" + formatter.formatToParts(tBEntry[3]).map(p => p.type != 'currency' ? p.value : '').join('') + "</td>" +
                        "<td style='text-align: right; vertical-align: middle;" + colStyle + "'>" + formatter.formatToParts(tBEntry[4]).map(p => p.type != 'currency' ? p.value : '').join('') + "</td>" +
                        "<td style='text-align: right; vertical-align: middle;" + colStyle + "'>" + formatter.formatToParts(tBEntry[5]).map(p => p.type != 'currency' ? p.value : '').join('') + "</td>" +
                        "<td style='text-align: right; vertical-align: middle;" + colStyle + "'>" + formatter.formatToParts(tBEntry[6]).map(p => p.type != 'currency' ? p.value : '').join('') + "</td>" +
                        "<td style='text-align: right; vertical-align: middle;" + colStyle + "'>" + formatter.formatToParts(tBEntry[7]).map(p => p.type != 'currency' ? p.value : '').join('') + "</td>" +
                        "<td style='text-align: right; vertical-align: middle;" + colStyle + "'>" + formatter.formatToParts(tBEntry[8]).map(p => p.type != 'currency' ? p.value : '').join('') + "</td>" +
                        "<td style='text-align: right; vertical-align: middle;" + colStyle + "'>" + formatter.formatToParts(tBEntry[9]).map(p => p.type != 'currency' ? p.value : '').join('') + "</td>" +
                        "<td style='text-align: right; vertical-align: middle;" + colStyle + "'>" + formatter.formatToParts(tBEntry[10]).map(p => p.type != 'currency' ? p.value : '').join('') + "</td>" +
                        "<td style='text-align: right; vertical-align: middle;" + colStyle + "'>" + formatter.formatToParts(tBEntry[11]).map(p => p.type != 'currency' ? p.value : '').join('') + "</td>" +
                        "<td style='text-align: right; vertical-align: middle;" + colStyle + "'>" + formatter.formatToParts(tBEntry[12]).map(p => p.type != 'currency' ? p.value : '').join('') + "</td>" +
                        "<td style='text-align: right; vertical-align: middle;" + colStyle + "'>" + formatter.formatToParts(tBEntry[13]).map(p => p.type != 'currency' ? p.value : '').join('') + "</td>" +
                        "</tr>";
                });
                $("#tb_report_datatable tbody").append(strHTML);
                //table.draw();

                // calculate all Totals
                calculateTotal();
                // enable submit button
                $("#submitTBForm").attr("disabled", false);
                $("#balanceSheetPacMan").hide();
            },
            complete: function () {
                // enable submit button
                $("#submitTBForm").attr("disabled", false);
            },
        });

        //highlight
        /*$('#tb_report_datatable tbody').on( 'mouseenter', 'td', function () {
            
    		table.rows().eq(0).each(function (index) {
    			$(table.row(index).nodes()).removeClass('highlight');
    		});
    		$(table.cells().nodes()).removeClass('highlight');
    		
    		var rowIdx = table.cell(this).index().row;
            var colIdx = table.cell(this).index().column;
            
            $(table.row(rowIdx).nodes()).addClass('highlight');
            $(table.column(colIdx).nodes()).addClass('highlight');
        });*/

    }

    $("#btnExcel").on("click", function () {
        window.location = "/reports/download/SOFPComparisonReport/?companyIds=" + $("#companyIds").val() + "&branchIds=" + $("#branchIds").val() + "&financialYearId=" + $("#financialYearId").val() + "&level=" + $("#level").val() + "&reportType=" + $("#reportType").val() + "";
    });

    function getExcelBuilder() {
        var xlsBuilder = {
            filename: 'PROFIT OR LOSS COMPARISON REPORT ' + new Date().toLocaleString(),
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
                    v: 'PROFIT OR LOSS COMPARISON REPORT ' + ' ('
                        + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim()
                        + ')'
                }]);
                var r2 = Addrow(2, [{
                    k: 'A',
                    v: 'COMPANY :'
                }, {
                    k: 'B',
                    v: $('#companyIds option:selected').text()
                }]);
                var r3 = Addrow(3, [{
                    k: 'A',
                    v: 'BRANCH :'
                }, {
                    k: 'B',
                    v: $('#branchIds option:selected').text()
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
                    v: $("#financialYearId option:selected").text()
                }]);

                var r6 = Addrow(6, [{
                    k: 'A',
                    v: 'REPORT TYPE :'
                }, {
                    k: 'B',
                    v: $("#reportType option:selected").text()
                }]);

                sheet.childNodes[0].childNodes[1].innerHTML = r1
                    + r2
                    + r3
                    + r4
                    + r5
                    + r6
                    + sheet.childNodes[0].childNodes[1].innerHTML;

                // Loop over the cells in column `B`
                $('row', sheet).each(function (index, val) {
                    // Get the value
                    if (index > 7) {
                        if ($(val).find('c[r^="B"]').text().trim().length == 2) {
                            $(val).find('c').attr('s', '5');
                        }
                        if ($(val).find('c[r^="B"]').text().trim().length == 5) {
                            $(val).find('c').attr('s', '10');
                        }
                        if ($(val).find('c[r^="B"]').text().trim().length == 9) {
                            $(val).find('c').attr('s', '15');
                        }
                        if ($(val).find('c[r^="B"]').text().trim().length == 14) {
                            $(val).find('c').attr('s', '20');
                        }
                    }
                });
            },
            /*
             * exportOptions: { columns: [0, 1, 2, 3] }
             */
        }
        return xlsBuilder;
    }

    function userLog() {
        //console.log("a "+$("#accountCode").val());
        let userLogObj = {
            idNumber: "",
            code: "",
            buttonClick: buttionName,
            windowName: $('h2').html(),
        }

        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "/viewSaleOrder",
            data: JSON.stringify(userLogObj),
            dataType: "json",
            success: function (data) {
                successmessage = 'Data was successfully captured';
                //location.reload();
                //  $("#customerAccount\\.balanceLimit").text(successmessage);
            },
            error: function (data) {
                successmessage = 'Error';
                //  $("#customerAccount\\.balanceLimit").text(successmessage);
            },
        });
    }

});