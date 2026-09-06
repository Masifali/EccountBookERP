Number.prototype.toFixed = function (decimalPlaces) {
    return Number(Math.round(this + 'e' + decimalPlaces) + 'e-' + decimalPlaces);
}

$(document).ready(function () {

    var vouchersId = [];
    $("#printButton").on('click', function () {
        // location.replace("/reports/pre-purchase-report/?branchId=" + $("#branchId").val() + "&fromDate=" + $("#fromDate").val() + "&toDate=" + $("#toDate").val() + "&financialYearId=" + $("#financialYearId").val() + "&itemDefId=" + $("#itemDefId").val() + "&millId=" + $("#millId").val() + "&branchNamed=" + $("#branchId option:selected").text() + "&accountNamed=" + $("#accountCode option:selected").text() + "&itemNamed=" + $("#itemDefId option:selected").text() + "&searchByDate=" + $("#search_by_date option:selected").text() + "&millKhataName=" + $("#millId option:selected").text() + "&sellerAccountCodeName=" + $("#sellerAccountCode option:selected").text() + "&millKhataId=" + $("#millKhataId").val() + "&accountCode=" + $("#accountCode").val() + "&sellerAccountCode=" + $("#sellerAccountCode").val() + "&status=" + $("#status").val() + "&paymentType=" + $("#paymentType").val());
        //  location.replace("/reports/print-voucher-list/?voucherRange=" + $("#voucherRange").val() + "&type=" + "purchase" + "&voucherType=" + $("#voucherType").val());
        //location.replace("/reports/print-voucher-list/?voucherRange=" + $("#voucherRange").val() + "&type=" + "purchase" + "&voucherType=" + $("#voucherType").val());
        location.replace("/reports/print-voucher-list/?voucherList=" + vouchersId);
    });

    var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 0, maximumFractionDigits: 0});

    var mill = parseFloat(0);
    var bank = parseFloat(0);
    var fr = parseFloat(0);
    var slai = parseFloat(0);
    var bag = parseFloat(0);
    var bar = parseFloat(0);
    var other = parseFloat(0);
    var safw8 = parseFloat(0);
    var totalAmount = parseFloat(0);

    var mill1 = parseFloat(0);
    var bank1 = parseFloat(0);
    var fr1 = parseFloat(0);
    var slai1 = parseFloat(0);
    var bag1 = parseFloat(0);
    var bar1 = parseFloat(0);
    var other1 = parseFloat(0);
    var safw81 = parseFloat(0);
    var totalAmount1 = parseFloat(0);
    //on load
    //restrictFromAndToDate();
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

//		$("#gl_report_datatable tr").each(function() {
//	    	if ($(this).find("td").length) {
//	    		
//	    		inTotal = inTotal + parseFloat($(this).find("td:eq(6)").text() || 0);
//	    		prTotal = prTotal + parseFloat($(this).find("td:eq(7)").text() || 0);
//	    		outTotal = outTotal + parseFloat($(this).find("td:eq(9)").text() || 0);
//	    		srTotal = srTotal + parseFloat($(this).find("td:eq(9)").text() || 0);
//		    	
//	    	}
//	    });
//		
//		$("#gl_report_datatable span#inTotal").text(inTotal);
//		$("#gl_report_datatable span#prTotal").text(prTotal);
//		$("#gl_report_datatable span#outTotal").text(outTotal);
//		$("#gl_report_datatable span#srTotal").text(srTotal);

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
        generateGLReport(0);
    });

    // DUBLICATE VOUCHERS
    $("#dublicate").on("click", function () {
        $("#dublicate").attr("disabled", true);  // disable submit button
        $("#loading").show();  // show loading indicator

        // Prevent the form from submitting via the browser.
        // event.preventDefault();
        generateGLReport(1);
    });

    function generateGLReport(whichButtonIsPressed) {


        $("#submitGLForm").attr("disabled", true);  // disable submit button
        $("#loading").show();  // show loading indicator

        // calculate all Totals
        calculateTotal();

        if (!$("#companyId").val() || !$("#financialYearId").val() || !$("#fromDate").val() || !$("#toDate").val() || !$("#accountCode").val()) {
            alert("PLEASE SELECT ALL THE FIELDS");
            return;
        }

        $('#gl_report_datatable').DataTable().clear();
        $('#gl_report_datatable').DataTable().destroy();
        var table = $('#gl_report_datatable').DataTable({
            dom: 'Bfrtip',
            deferRender: true,
//	        scrollY: 500,
//	        scroller: true,
//	        destroy: true,
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
                    title: 'PURCHASE REPORT' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    messageTop: 'COMPANY: ' + $("#companyId option:selected").text()
                        + '  ' + 'BRANCH: ' + $("#branchId option:selected").text()
                        + '  ' + 'ITEM: ' + $("#itemDefId option:selected").text()
                        + '   ' + 'ACCOUNT: ' + $("#accountCode option:selected").text()
                        + ' ' + 'FINANCIAL YEAR: ' + $("#financialYearId option:selected").text() + " (" + getFormattedDate(new Date($("#fromDate").val())) + " - " + getFormattedDate(new Date($("#toDate").val())) + ")"
                        + ' ' + ' Search by :' + $("#search_by_date option:selected").text(),
                    messageBottom: null,

                    download: 'open',

                    messageBottom: null,
                    pageSize: 'A4',
                    paging: true,
                    footer: true,
                    exportOptions: {
                        columns: [0, 1, 2, 3, 4, 5, 6, 7, 8],
                        stripNewlines: false,
                    },
                    customize: function (doc) {
                        console.log("aa");
                        doc.defaultStyle.fontSize = 8;
                        doc.pageMargins = [1, 15, 1, 20]; //1st left side ,2th header, 3th right side ,4th buttion
                        doc.styles.tableHeader.fontSize = 8;
                        doc.styles.tableFooter.fontSize = 8;
                        doc.defaultStyle.alignment = 'left';
                        doc.styles.tableHeader.alignment = 'left';
                        doc.styles.tableFooter.alignment = 'left';
                        doc.content[2].layout = {
                            hLineWidth: function (i, node) {
                                return (i === 0 || i === node.table.body.length) ? 2 : 1;
                            },
                            vLineWidth: function (i, node) {
                                return (i === 0 || i === node.table.widths.length) ? 2 : 1;
                            },
                            hLineColor: function (i, node) {
                                return (i === 0 || i === node.table.body.length) ? 'black' : 'gray';
                            },
                            vLineColor: function (i, node) {
                                return (i === 0 || i === node.table.widths.length) ? 'black' : 'gray';
                            }
                        };
                        doc.content[1].layout = {
                            hLineWidth: function (i, node) {
                                return (i === 0 || i === node.table.body.length) ? 2 : 1;
                            },
                            vLineWidth: function (i, node) {
                                return (i === 0 || i === node.table.widths.length) ? 2 : 1;
                            },
                            hLineColor: function (i, node) {
                                return (i === 0 || i === node.table.body.length) ? 'black' : 'gray';
                            },
                            vLineColor: function (i, node) {
                                return (i === 0 || i === node.table.widths.length) ? 'black' : 'gray';
                            }
                        };
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
                    title: 'PURCHASE REPORT' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    messageTop: 'COMPANY: ' + $("#companyId option:selected").text()
                        + '  ' + 'BRANCH: ' + $("#branchId option:selected").text()
                        + '  ' + 'ITEM: ' + $("#itemDefId option:selected").text()
                        + '   ' + 'ACCOUNT: ' + $("#accountCode option:selected").text(),
                    messageBottom: null,
                    download: 'open',
                    footer: true,
                    autoPrint: false,
                    exportOptions: {
                        columns: [0, 1, 2, 3, 4, 5, 6, 7, 8],
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

                var colNumber = [8, 10];

                // Remove the formatting to get integer data for summation
                var intVal = function (i) {
                    return typeof i === 'string' ?
                        //  i.replace(/[\$,]/g, '') * 1 :
                        i.replace(/[^0-9\.-]+/g, "") * 1 :
                        typeof i === 'number' ?
                            i.toFixed(2) : 0;
                };

                for (i = 0; i < colNumber.length; i++) {
                    // console.log(colNumber[1])
                    var colNo = colNumber[i];
                    var total = api
                        .column(colNo)
                        .data()
                        .reduce(function (a, b) {
                            console.log(intVal(a));
                            return intVal(a) + intVal(b);
                        }, 0);
                    console.log(total);
                    $(api.column(colNo).footer()).html(formatter.format(parseFloat(total).toFixed(2)));
                }
            }
        });


        // PREPARE FORM DATA
        var formData = {
            companyId: $("#companyId").val(),
            branchId: $("#branchId").val(),
            voucherStatusId: $("#voucherStatusId").val(),
            financialYearId: $("#financialYearId").val(),
            fromDate: $("#fromDate").val(),
            toDate: $("#toDate").val(),
            itemDefId: $("#itemDefId").val(),
            itemCategoryId: $("#itemCategoryId").val(),
            accountCode: $("#accountCode").val(),
            serchByDate: $("#search_by_date").val(),
            voucherType: $("#voucherType").val(),
            callingFrom: "trading",
            dublicateFilter: $("#dublicateFilter").val(),
            millKhataId: $("#millKhataId").val(),
            millId: $("#millId").val(),
        }
        ajaxCall(table, formData, whichButtonIsPressed);
    }

    function ajaxCall(table, formData, whichButtonIsPressed) {
        // DO POST


        var URl = "";
        if (whichButtonIsPressed == 1) {
            URl = "/reports/dublicate-purchase-report";
        } else {
            URl = $("#glReportForm").attr("action");
        }
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: URl,
            data: JSON.stringify(formData),
            dataType: "json",
            success: function (data) {
                mill = parseFloat(0);
                bank = parseFloat(0);
                fr = parseFloat(0);
                slai = parseFloat(0);
                bag = parseFloat(0);
                bar = parseFloat(0);
                other = parseFloat(0);
                safw8 = parseFloat(0);
                totalAmount = parseFloat(0);
                // FILL TABLE ROWS
                vouchersId = [];
                var purchaseAmount = parseFloat(0);
                var profit = parseFloat(0);
                $.each(data, function (i, sLEntry) {
                    vouchersId.push(sLEntry.id);
                    
                    var voucherViewLink;
                    if (sLEntry.voucherCode) {

                        if (sLEntry.voucherCode.indexOf("STV") >= 0) {
                            bag = bag + sLEntry.bags;
                            safw8 = safw8 + sLEntry.safiKg;
                            mill = mill + sLEntry.millTax;
                            bank = bank + sLEntry.banKTax;
                            fr = fr + sLEntry.freight;
                            slai = slai + sLEntry.silai;
                            totalAmount = totalAmount + ((sLEntry.safiKg / 40) * sLEntry.rate);
                            bar = bar + sLEntry.baradana;
                            other = other + sLEntry.otherExp;

                            profit = parseFloat(sLEntry.amount) - parseFloat(purchaseAmount);
                            if ($("#tradingProfit").val() === "false") {
                                profit = 0;
                            }
                            //purchaseAmount = parseFloat(sLEntry.amount);
                            voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/SaleQty/view_pjv_sjv_qty/" + sLEntry.id + ">" + sLEntry.voucherCode + "</a>";
                        } else if (sLEntry.voucherCode.indexOf("PTV") >= 0) {
                            bag1 = bag1 + sLEntry.bags;
                            safw81 = safw81 + sLEntry.safiKg;
                            mill1 = mill1 + sLEntry.millTax;
                            bank1 = bank1 + sLEntry.banKTax;
                            fr1 = fr1 + sLEntry.freight;
                            slai1 = slai1 + sLEntry.silai;
                            totalAmount1 = totalAmount1 + ((sLEntry.safiKg / 40) * sLEntry.rate);
                            bar1 = bar + sLEntry.baradana;
                            other1 = other + sLEntry.otherExp;
                            profit = 0;
                            purchaseAmount = parseFloat(sLEntry.amount);
                            voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/SaleQty/view_pjv_sjv_qty/" + sLEntry.id + ">" + sLEntry.voucherCode + "</a>";
                        } else if (sLEntry.voucherCode.indexOf("PJV") >= 0) {
                            voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/purchase_journal_vouchers/" + sLEntry.id + ">" + sLEntry.voucherCode + "</a>";
                        }
//			    		else if (sLEntry.voucherCode.indexOf("PTV") >= 0){
//			    			voucherViewLink = "<a style='text-decoration: underline;' target=_blank href="+location.protocol + "//" + location.host+"/SaleQty/view_pjv_sjv_qty/"+sLEntry.id+">"+sLEntry.voucherCode+"</a>";
//			    		}
                        else if (sLEntry.voucherCode.indexOf("PRV") >= 0) {
                            voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/purchase_return_vouchers/" + sLEntry.id + ">" + sLEntry.voucherCode + "</a>";
                        } else if (sLEntry.voucherCode.indexOf("SJV") >= 0) {
                            voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/sale_journal_vouchers/" + sLEntry.id + ">" + sLEntry.voucherCode + "</a>";
                        }
//			    		else if (sLEntry.voucherCode.indexOf("STV") >= 0){
//			    			voucherViewLink = "<a style='text-decoration: underline;' target=_blank href="+location.protocol + "//" + location.host+"/SaleQty/view_pjv_sjv_qty/"+sLEntry.id+">"+sLEntry.voucherCode+"</a>";
//			    		}
                        else if (sLEntry.voucherCode.indexOf("PQV") >= 0) {
                            voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/sale_journal_voucher_qty/" + sLEntry.id + ">" + sLEntry.voucherCode + "</a>";
                        } else if (sLEntry.voucherCode.indexOf("SRV") >= 0) {
                            voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/sale_return_vouchers/" + sLEntry.id + ">" + sLEntry.voucherCode + "</a>";
                        }
                    }

                    table.row.add([
                        (i + 1),
                        getFormattedDate(new Date(sLEntry.voucherDate)),
                        getFormattedDate(new Date(sLEntry.modifyDate)),
                        sLEntry.id ? voucherViewLink : "",
                        sLEntry.bookNumber,
                        sLEntry.partyName,
                        sLEntry.truck,

                        sLEntry.itemName + " , " + formatter.format(sLEntry.bags) + " , " + formatter.format(sLEntry.safiKg) + " , " + formatter.format(sLEntry.rate) + " , " + formatter.format(sLEntry.millTax) + " , " + formatter.format(sLEntry.banKTax) + " , " + formatter.format(sLEntry.freight) + " , " + formatter.format(sLEntry.silai) + " , " + formatter.format(sLEntry.baradana) + " , " + formatter.format(sLEntry.otherExp),

                        formatter.format(sLEntry.amount),
                        sLEntry.remarks,
                        profit.toFixed(0),
                    ]);
                    //purchaseAmount = parseFloat(sLEntry.amount);
                });

                table.draw();

                calculateTotal();  // calculate all Totals
                $("#submitGLForm").attr("disabled", false);  // enable submit button
                $('#loading').hide();  // hide loading indicator
                setFootterValue();
                $("#dublicate").attr("disabled", false);
                $('#loading').hide();  // hide loading indicator
            },
            complete: function () {
                $("#submitGLForm").attr("disabled", false);  // enable submit button
                $('#loading').hide();  // hide loading indicator
                $("#dublicate").attr("disabled", false);
                $('#loading').hide();  // hide loading indicator
            },
        });

        //highlight
        $('#gl_report_datatable tbody').on('mouseenter', 'td', function () {

            table.rows().eq(0).each(function (index) {
                $(table.row(index).nodes()).removeClass('highlight');
            });
            //$(table.cells().nodes()).removeClass('highlight');

            var rowIdx = table.cell(this).index().row;
            //var colIdx = table.cell(this).index().column;

            $(table.row(rowIdx).nodes()).addClass('highlight');
            //$(table.column(colIdx).nodes()).addClass('highlight');
        });
    }

    function getExcelBuilder() {
        var xlsBuilder = {
            filename: 'PR_' + new Date().toLocaleString(),
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
                    v: 'PURCHASE REPORT' + ' ('
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


        $("span#des").text("Bag:" + bag + " W:" + formatter.format(safw8) + " M:" + formatter.format(mill) + " B:" + formatter.format(bank) + " F:" + formatter.format(fr) + " S:" + formatter.format(slai) + " BR:" + formatter.format(bar) + " Oth:" + formatter.format(other) +
            "\r\n" + "Bag:" + bag + " W:" + formatter.format(safw81) + " M:" + formatter.format(mill1) + " B:" + formatter.format(bank1) + " F:" + formatter.format(fr1) + " S:" + formatter.format(slai1) + " BR:" + formatter.format(bar1) + " Oth:" + formatter.format(other1));
        $("span#rem").text("AVG: " + formatter.format((totalAmount / safw8) * 40));
    }
});