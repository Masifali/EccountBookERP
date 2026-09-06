$(document).ready(function () {
    var payload = {};
    //on load
    $("#thirdParty").on('click', function () {
        $("#thirdPartyLoading").show();
        $("#thirdParty").attr("disabled", true);  // enable submit button
        formDataData();
        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "/reports/patryonlyVoucherAmount",
            data: JSON.stringify(payload),
            dataType: "text",
            success: function (data) {
                window.open("/reports/general_ledger_download/?type=" + "thirdParty", "_blank");
                $("#thirdPartyLoading").hide();
            },
            complete: function () {
                $("#thirdParty").attr("disabled", false);  // enable submit button
                $("#thirdPartyLoading").hide();  // hide loading indicator
            }
        })
    });
    $("#printPayment").on('click', function () {
        $("#printPaymentLoading").show();
        $("#printPayment").attr("disabled", true);
        formDataData();
        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "/reports/patryonlyVoucherAmount",
            data: JSON.stringify(payload),
            dataType: "text",
            success: function (data) {
                window.open("/reports/general_ledger_download/?type=" + "1", "_blank");
            },
            complete: function () {
                $("#printPayment").attr("disabled", false);  // enable submit button
                $("#printPaymentLoading").hide();  // hide loading indicator

            }
        })

    });

    function formDataData() {
        payload = {
            companyId: $("#companyIds").val(),
            branchId: $("#branchIds").val(),
            voucherStatusId: $("#voucherStatusId").val(),
            financialYearId: $("#financialYearId").val(),
            fromDate: $("#fromDate").val(),
            toDate: $("#toDate").val(),
            accountCode: $("#accountCode").val(),
            paymentType: $("#paymentType").val(),
            companyIds: [$("#companyIds").val()],
            branchIds: [$("#branchIds").val()],
        }
    }

    //restrictFromAndToDate();
    //loadCompanyBranches();
    $('body').on('click', '.lockVoucher', function () {


        if ($("#lockEntry").val() === "false") {
            $.confirm({
                title: "CONFIRMATION REQUIRED",

                content: "YOUR ARE NOT AUTHORIZATION TO LOCK OR UNLOCK THIS ENRTY",
                buttons: {
                    confirm: function () {
                    },
                    cancel: function () {
                    },
                }
            });
        } else {
            var entryid = $(this).closest("tr").find(".voucherId").val();
            var voucherStatus = $(this).closest("tr").find(".entryStatus").val();
            var remarks = $(this).closest("tr").find(".remarks").val();
            var row = $(this).closest("tr");
            console.log("b  " + entryid);
            $.get("/stock/voucher_lock_by_id?voucherId=" + entryid + "&accountCode=" + $("#accountCode").val()
                + "&status=" + voucherStatus + "&remarks=" + remarks + "&callForLedger=true", function (data) {
                console.log(data);

                row.find(".lockEntry").text(data);
                if (!data) {
                    row.css("background-color", "#4dff4d");
                    row.find(".entryStatus").val(false);
                }
                if (data) {

                    row.css("background-color", "#f0f5f5");
                    row.find(".entryStatus").val(true);
                }
                buttionName = "ENTRY LOCK OR UNLOCK";
                /*  userLog();*/

            });
            $.confirm({
                title: "SUCCESS!",
                content: "REMARSK SUCCESSFULLY WAS SAVED",
                type: 'red',
                // typeAnimated: true,
            });

        }
    });

    function setRowColor() {
        $("#gl_report_datatable tbody tr").each(function () {
            if ($(this).closest("tr").find(".entryStatus").val() === "true") {
                // $(this).closest("tr").find(".entryStatus").prop(readonl)

                $(this).closest("tr").css("background-color", "#e6fff9");
            }
            console.log("setRowColor" + $(this).closest("tr").find(".entryStatus").val());
        });
    }//setRowColor()
    var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 0, maximumFractionDigits: 0});
    $("#companyIds").on("change", function () {
        loadCompanyBranches();
        console.log("dd" + $("#companyIds").val());
    });
    var mill = parseFloat(0);
    var bank = parseFloat(0);
    var fr = parseFloat(0);

    var slai = parseFloat(0);
    var bag = parseFloat(0);
    var bar = parseFloat(0);
    var other = parseFloat(0);
    var safw8 = parseFloat(0);
    var totalAmount = parseFloat(0);

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

    let searchParams = new URLSearchParams(window.location.search)
    if (searchParams.has("code") || searchParams.has("mobileNumbers")) {
        setTimeout(function () {
            $("#glReportForm").submit();
        }, 1000);
    }

    $("#accountCode").on("change", function () {
        if ($("#accountCode").val().indexOf("3312102") !== -1) {
            $("#show_sms_modal").show();
        } else {
            $("#show_sms_modal").hide();
        }
    });

    $("#show_sms_modal").on("click", function () {
        if (!$("#accountCode").val() || $("#accountCode").val() == 0) {
            $.confirm({
                title: "ENCOUNTERED AN ERROR!",
                content: "PLEASE SELECT AN ACCOUNT",
                type: 'red',
                typeAnimated: true,
            });
            return false;
        }
//		$("#sendSMSModal h4.modal-title").text($("#accountCode option:selected").text());
//		$("#sendSMSModal #recipient_number").val($("#accountCode option:selected").attr("data-mobile"));
//		
//		$("#recipient_number").val($("#accountCode option:selected").attr("data-mobile").split(",")[0].replace("+92", "0").trim());
//		
//		$("#recipient_numbers").empty();
//		$.each($("#accountCode option:selected").attr("data-mobile").split(","), function(i, item) {
//			$("#recipient_numbers").append($("<option>").text(item.replace("+92", "0").trim()));
//		});

        $("#sendSMSModal h4.modal-title").text($("#accountCode option:selected").text());

        $.get("/receivables/get_sms?accountCode=" + $("#accountCode").val(), function (data) {
            $("#sendSMSModal #message_text").val(data);
        });

        /*$.get( "/receivables/account_limit_and_balance?accountCode=" + $("#customerAccount\\.code").val(), function( data ) {
            $("input#customerAccount\\.balanceLimit").val(data.accountBalanceLimit);
            $("span#previousBalance").text(formatter.format(data.accountBalance));
            calculateTotal();
        });*/
    });

    $("#send_sms").off().on("click", function () {

        $.get("/receivables/send_sms?accountCode=" + $("#accountCode").val() + "&recipientMobile=" + $("#recipient_number").val(), function (data) {
            $.confirm({
                title: "NOTE!",
                content: data,
                type: "blue",
                typeAnimated: true,
            });
            $("#sendSMSModal").modal("toggle");
        });

    });

    $("#accountCode").on("change", function () {
        //console.log("change "+$("#customerAccount").val())
        if (!$("#accountCode").val() || $("#accountCode").val() == 0) {
            return;
        }

        $.get("/receivables/account_limit_and_balance?accountCode=" + $("#accountCode").val(), function (data) {
            //$("input#customerAccount\\.balanceLimit").val(data.accountBalanceLimit);
            console.log("change " + data.accountBalance);
            var balance = "";
            if (data.accountBalance > 0) {
                balance = "debit: " + formatter.format(data.accountBalance);
            } else {
                balance = "credit: " + formatter.format(data.accountBalance);
            }
            $("span#selectedCustomerBalance").text(balance + "  **Un:" + data.unPostVoucher);

            //calculateTotal();
        });
    });

    function calculateTotal() {

        var debitTotal = parseFloat(0);
        var creditTotal = parseFloat(0);
        //var balanceTotal = parseFloat(0);

        var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 2, maximumFractionDigits: 2});

        $("#gl_report_datatable tr").each(function () {

            // TEXT SHOW AT RIGHT OF DEBIT CREDIT AND BALANCE
            $(this).find("td").each(function (index, td) {
                // console.log("aaa  "+ index);
                if (index > 5 && index < 9)
                    $(this).css('text-align', 'right');
            });
            if ($(this).find("td").length) {

                debitTotal = debitTotal + parseFloat($(this).find("td:eq(8)").text().replace(/[^0-9\.-]+/g, "") || 0);
                creditTotal = creditTotal + parseFloat($(this).find("td:eq(9)").text().replace(/[^0-9\.-]+/g, "") || 0);
                //balanceTotal = balanceTotal + parseFloat($(this).find("td:eq(11)").text().replace(/[^0-9\.-]+/g, "") || 0);
            }
        });

        $("#gl_report_datatable span#debitTotal").text(formatter.format(debitTotal));
        $("#gl_report_datatable span#creditTotal").text(formatter.format(creditTotal));
        //$("#gl_report_datatable span#balanceTotal").text(formatter.format(debitTotal - creditTotal));
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

        if (!$("#accountCode").val() || $("#accountCode").val() == 0) {
            return;
        }
        $("#loading").show();
        generateGLReport(0);
        setRowColor();

    });
    $("#dublicate").on("click", function () {
        $("#dublicate").attr("disabled", true);  // disable submit button
        $("#loading").show();  // show loading indicator

        // Prevent the form from submitting via the browser.
        // event.preventDefault();
        generateGLReport(1);
    });

    $("#bardana").on("click", function (event) {
        event.preventDefault();
        if (!$("#accountCode").val() || $("#accountCode").val() == 0) {
            return;
        }
        generateGLReportBardana();
    });

    function generateGLReport(whichButtionPressed) {

        $("#submitGLForm").attr("disabled", true);  // disable submit button
        $('#loading').show();  // show loading indicator

        // calculate all Totals
        calculateTotal();

        if (!$("#companyIds").val() || !$("#voucherStatusId").val() || !$("#financialYearId").val() || !$("#fromDate").val() || !$("#toDate").val() || !$("#accountCode").val()) {
            alert("PLEASE SELECT ALL THE FIELDS")
            return;
        }

        $('#gl_report_datatable').DataTable().clear();
        $('#gl_report_datatable').DataTable().destroy();
        var table = $('#gl_report_datatable').DataTable({
            dom: 'Bfrtip',
            deferRender: true,
//  	        scrollY: 500,
//  	        scroller: true,
//  	        destroy: true,
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
                    title: 'GENERAL LEDGER' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    messageTop: 'FROM: ' + getFormattedDate(new Date($("#fromDate").val()))
                        + '   ' + 'TO : ' + getFormattedDate(new Date($("#toDate").val()))
                        + ' \n '
                        + '   ' + 'ACCOUNT: ' + $("#accountCode option:selected").text(),
                    messageBottom: null,
                    // messageTop: null,
                    download: 'open',
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
                        doc.pageMargins = [15, 15, 10, 20]; //1st left side ,2th header, 3th right side ,4th buttion
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
                    title: 'GENERAL LEDGER' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
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

                var colNumber = [6, 7];

                // Remove the formatting to get integer data for summation
                var intVal = function (i) {
                    if (typeof i === 'string') {
                        return i.replace(/[\$,]/g, '') * 1;
                    } else if (typeof i === 'number') {
                        return parseFloat(i);
                    } else {
                        return parseFloat(0);
                    }
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
            }
        });

        if ($("#reports_download").val() === "false") {
            table.buttons('.dt-button').remove();
        }
        var URl = "";
        if (whichButtionPressed == 1) {
            URl = "/reports/dublicate-purchase-report";
        } else {
            URl = $("#glReportForm").attr("action");
        }
        // PREPARE FORM DATA
        var formDataLedger = {
            companyId: $("#companyIds").val(),
            branchId: $("#branchIds").val(),
            voucherStatusId: $("#voucherStatusId").val(),
            financialYearId: $("#financialYearId").val(),
            fromDate: $("#fromDate").val(),
            toDate: $("#toDate").val(),
            accountCode: $("#accountCode").val(),
            paymentType: $("#paymentType").val(),
            callingFrom: "trading",
            dublicateFilter: $("#dublicateFilter").val(),
        }

        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: URl,
            data: JSON.stringify(formDataLedger),
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
                $.each(data, function (i, gLEntry) {


                    var itemValue;
                    console.log(gLEntry.itemName);
                    if (gLEntry.voucherType == "0")
                        itemValue = gLEntry.itemName;
                    else if (gLEntry.sequence == "10") {
                        itemValue = gLEntry.brokery;
                    } else {
                        itemValue = gLEntry.itemName + " - " + formatter.format(gLEntry.bags) + " - " + formatter.format(gLEntry.safiKg) + " - " + formatter.format(gLEntry.rate) + " - " + formatter.format(gLEntry.millTax) + " - " + formatter.format(gLEntry.banKTax) + " - " + formatter.format(gLEntry.freight) + " - " + formatter.format(gLEntry.silai) + " - " + formatter.format(gLEntry.baradana) + " - " + formatter.format(gLEntry.otherExp) + "  " + gLEntry.brokery;
                        bag = bag + gLEntry.bags;
                        safw8 = safw8 + gLEntry.safiKg;
                        mill = mill + gLEntry.millTax;
                        bank = bank + gLEntry.banKTax;
                        fr = fr + gLEntry.freight;
                        slai = slai + gLEntry.silai;
                        totalAmount = totalAmount + ((gLEntry.safiKg / 40) * gLEntry.rate);
                        bar = bar + gLEntry.baradana;
                        other = other + gLEntry.otherExp;

                    }
                    console.log("sss" + gLEntry.voucherCode);
                    var voucherViewLink;
                    var voucherNarration = '<textarea class="form-control remarks" th:field="\'${gLEntry.voucherNarration}\'">' + gLEntry.voucherNarration + '</textarea>';
                    if (i === 0) {
                        voucherNarration = '<textarea class="form-control remarks"  >' + gLEntry.voucherNarration + '</textarea>';
                    }
                    if (gLEntry.voucherCode) {
                        if (gLEntry.voucherCode.indexOf("STV") >= 0) {
                            voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/SaleQty/view_pjv_sjv_qty/" + gLEntry.id + ">" + gLEntry.voucherCode + "</a>";
                        } else if (gLEntry.voucherCode.indexOf("PTV") >= 0) {
                            voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/SaleQty/view_pjv_sjv_qty/" + gLEntry.id + ">" + gLEntry.voucherCode + "</a>";
                        } else if (gLEntry.voucherCode.indexOf("PJV") >= 0) {
                            voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/purchase_journal_vouchers/" + gLEntry.id + ">" + gLEntry.voucherCode + "</a>";
                        } else if (gLEntry.voucherCode.indexOf("PRV") >= 0) {
                            voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/purchase_return_vouchers/" + gLEntry.id + ">" + gLEntry.voucherCode + "</a>";
                        } else if (gLEntry.voucherCode.indexOf("SJV") >= 0) {
                            voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/sale_journal_vouchers/" + gLEntry.id + ">" + gLEntry.voucherCode + "</a>";
                        } else if (gLEntry.voucherCode.indexOf("OJV") >= 0) {
                            voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/vouchers/" + gLEntry.id + ">" + gLEntry.voucherCode + "</a>";
                        } else if (gLEntry.voucherCode.indexOf("PRO") >= 0) {
                            voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/production-voucher-view/" + gLEntry.id + ">" + gLEntry.voucherCode + "</a>";
                        } else if (gLEntry.voucherCode.indexOf("SRO") >= 0) {
                            voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/production-voucher-view/" + gLEntry.id + ">" + gLEntry.voucherCode + "</a>";
                        } else if (gLEntry.voucherCode.indexOf("PQV") >= 0) {
                            voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/purchase-quantity-vouchers/" + gLEntry.id + ">" + gLEntry.voucherCode + "</a>";
                        } else if (gLEntry.voucherCode.indexOf("SQV") >= 0) {
                            voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/sale-quantity-vouchers/" + gLEntry.id + ">" + gLEntry.voucherCode + "</a>";
                        } else {
                            voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/vouchers/" + gLEntry.id + ">" + gLEntry.voucherCode + "</a>";
                        }
                    }

                    table.row.add([
                        (i + 1),
                        getFormattedDate(new Date(gLEntry.voucherDate)),
                        gLEntry.id ? voucherViewLink : "",
                        gLEntry.bookNumber,
                        //gLEntry.partyName,
                        // "",

                        gLEntry.truck,
                        itemValue,
                        formatter.format(gLEntry.debit),
                        formatter.format(gLEntry.credit),
                        formatter.format(gLEntry.balance),
                        gLEntry.remarks,
                        voucherNarration,
                        '<td>  <input type="hidden"   class="form-control voucherId"  value="' + gLEntry.id + '"/> <input type="hidden"   class="form-control entryStatus" value="' + gLEntry.voucherStatus + '"/></td>',
                        '<a class="lockVoucher"  style="text-decoration:underline;"   href="javascript:void(0)">SAVE REM</a>',

                    ]);
                });
                table.draw();
                calculateTotal();  // calculate all Totals
                $("#submitGLForm").attr("disabled", false);  // enable submit button
                $('#loading').hide();  // hide loading indicator
                setFootterValue();
                setRowColor();
            },
            complete: function () {
                $("#submitGLForm").attr("disabled", false);  // enable submit button
                $("#dublicate").attr("disabled", false);  // disable submit button
                $('#loading').hide();  // hide loading indicator
                setRowColor();
                //window.open("/reports/general_ledger_download/?type=" + "thirdParty", "_blank");
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

// Function to format voucherNarration by splitting it into multiple lines
    function formatVoucherNarration(narration) {
        // Split by '1.', '2.', etc. (you could split by a different delimiter if needed)
        var regex = /\d+(?=\.)/g;
        var items = narration.match(regex);

        // Return formatted list with line breaks for each item
        return items.join('<br>'); // Join items with a line break
    }

    function generateGLReportBardana() {

        $("#submitGLForm").attr("disabled", true);  // disable submit button
        $('#loading').show();  // show loading indicator

        // calculate all Totals
        calculateTotal();

        if (!$("#companyIds").val() || !$("#voucherStatusId").val() || !$("#financialYearId").val() || !$("#fromDate").val() || !$("#toDate").val() || !$("#accountCode").val()) {
            alert("PLEASE SELECT ALL THE FIELDS")
            return;
        }

        $('#gl_report_datatable').DataTable().clear();
        $('#gl_report_datatable').DataTable().destroy();
        var table = $('#gl_report_datatable').DataTable({
            dom: 'Bfrtip',
            deferRender: true,
//  	        scrollY: 500,
//  	        scroller: true,
//  	        destroy: true,
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
                    title: 'GENERAL LEDGER' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    messageTop: 'FROM: ' + getFormattedDate(new Date($("#fromDate").val()))
                        + '   ' + 'TO : ' + getFormattedDate(new Date($("#toDate").val()))
                        + ' \n '
                        + '   ' + 'ACCOUNT: ' + $("#accountCode option:selected").text(),
                    messageBottom: null,
                    // messageTop: null,
                    download: 'open',
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
                        doc.pageMargins = [15, 15, 10, 20]; //1st left side ,2th header, 3th right side ,4th buttion
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
                    title: 'GENERAL LEDGER' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
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

                var colNumber = [6, 7];

                // Remove the formatting to get integer data for summation
                var intVal = function (i) {
                    if (typeof i === 'string') {
                        return i.replace(/[\$,]/g, '') * 1;
                    } else if (typeof i === 'number') {
                        return parseFloat(i);
                    } else {
                        return parseFloat(0);
                    }
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
            }
        });

        if ($("#reports_download").val() === "false") {
            table.buttons('.dt-button').remove();
        }

        // PREPARE FORM DATA
        var formData = {
            companyId: $("#companyIds").val(),
            branchId: $("#branchIds").val(),
            voucherStatusId: $("#voucherStatusId").val(),
            financialYearId: $("#financialYearId").val(),
            fromDate: $("#fromDate").val(),
            toDate: $("#toDate").val(),
            accountCode: $("#accountCode").val(),
            paymentType: $("#paymentType").val(),
        }

        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "/reports/general_ledger_bardana",
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
                console.log("sss");
                $.each(data, function (i, gLEntry) {


                    var itemValue;
                    console.log(gLEntry.itemName);
                    if (gLEntry.voucherType == "0")
                        itemValue = gLEntry.itemName;
                    else {
                        itemValue = gLEntry.itemName + " - " + formatter.format(gLEntry.bags) + " - " + formatter.format(gLEntry.safiKg) + " - " + formatter.format(gLEntry.rate) + " - " + formatter.format(gLEntry.millTax) + " - " + formatter.format(gLEntry.banKTax) + " - " + formatter.format(gLEntry.freight) + " - " + formatter.format(gLEntry.silai) + " - " + formatter.format(gLEntry.baradana) + " - " + formatter.format(gLEntry.otherExp);
                        bag = bag + gLEntry.bags;
                        safw8 = safw8 + gLEntry.safiKg;
                        mill = mill + gLEntry.millTax;
                        bank = bank + gLEntry.banKTax;
                        fr = fr + gLEntry.freight;
                        slai = slai + gLEntry.silai;
                        totalAmount = totalAmount + ((gLEntry.safiKg / 40) * gLEntry.rate);
                        bar = bar + gLEntry.baradana;
                        other = other + gLEntry.otherExp;

                    }
                    var voucherViewLink;
                    if (gLEntry.voucherCode) {
                        if (gLEntry.voucherCode.indexOf("STV") >= 0 || gLEntry.voucherCode.indexOf("PTV") >= 0) {
                            voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/SaleQty/view_pjv_sjv_qty/" + gLEntry.id + ">" + gLEntry.voucherCode + "</a>";
                        }
                        if (gLEntry.voucherCode.indexOf("PJV") >= 0) {
                            voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/purchase_journal_vouchers/" + gLEntry.id + ">" + gLEntry.voucherCode + "</a>";
                        } else if (gLEntry.voucherCode.indexOf("PRV") >= 0) {
                            voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/purchase_return_vouchers/" + gLEntry.id + ">" + gLEntry.voucherCode + "</a>";
                        } else if (gLEntry.voucherCode.indexOf("SJV") >= 0) {
                            voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/sale_journal_vouchers/" + gLEntry.id + ">" + gLEntry.voucherCode + "</a>";
                        } else if (gLEntry.voucherCode.indexOf("PQV") >= 0) {
                            voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/sale_journal_voucher_qty/" + gLEntry.id + ">" + gLEntry.voucherCode + "</a>";
                        } else if (gLEntry.voucherCode.indexOf("SRV") >= 0) {
                            voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/sale_return_vouchers/" + gLEntry.id + ">" + gLEntry.voucherCode + "</a>";
                        } else if (gLEntry.voucherCode.indexOf("OJV") >= 0) {
                            voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/vouchers/" + gLEntry.id + ">" + gLEntry.voucherCode + "</a>";
                        } else if (gLEntry.voucherCode.indexOf("JV") >= 0) {
                            voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/vouchers/" + gLEntry.id + ">" + gLEntry.voucherCode + "</a>";
                        }
                    }

                    table.row.add([
                        (i + 1),
                        getFormattedDate(new Date(gLEntry.voucherDate)),
                        gLEntry.id ? voucherViewLink : "",
                        gLEntry.bookNumber,
                        //gLEntry.partyName,
                        // "",
                        gLEntry.truck,
                        itemValue,
                        formatter.format(gLEntry.debit),
                        formatter.format(gLEntry.credit),
                        formatter.format(gLEntry.balance),
                        gLEntry.remarks,
                    ]);
                });
                table.draw();
                calculateTotal();  // calculate all Totals
                $("#submitGLForm").attr("disabled", false);  // enable submit button
                $('#loading').hide();  // hide loading indicator
                setFootterValue();
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
                    v: $('#companyIds option:selected').toArray().map(item => item.text).join(", ")
                }]);
                var r3 = Addrow(3, [{
                    k: 'A',
                    v: 'BRANCH :'
                }, {
                    k: 'B',
                    v: $('#branchIds option:selected').toArray().map(item => item.text).join(", ")
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

    function setFootterValue() {
        var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 0, maximumFractionDigits: 0});


        $("span#des").text("Bag:" + bag + " W:" + formatter.format(safw8) + " M:" + formatter.format(mill) + " B:" + formatter.format(bank) + " F:" + formatter.format(fr) + " S:" + formatter.format(slai) + " BR:" + formatter.format(bar) + " Oth:" + formatter.format(other));
        $("span#rem").text("AVG: " + formatter.format((totalAmount / safw8) * 40));
    }
});