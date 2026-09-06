$(document).ready(function () {

    //on load
    restrictFromAndToDate();
    //loadFromAndToAccounts();

    $("#companyIds").on("change", function () {
        loadCompanyBranches();
    });

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

    $("#level").on("change", function () {
        loadFromAndToAccounts();
    });

    function loadFromAndToAccounts() {

        if (!$("#level").val()) {
            return;
        }

        $.get("/reports/load_from_and_to_Accounts?level=" + $("#level").val(), function (data) {

            $("#fromAccountCode").empty();
            $("#toAccountCode").empty();

            $.each(data, function (i, record) {
                var option = "<option value = " + record.code + ">" + record.formattedCode + "&emsp;" + record.accountName + "</option>";
                $("#fromAccountCode").append(option);
                $("#toAccountCode").append(option);
            });

            $("#fromAccountCode option:first").attr("selected", "selected");
            $("#toAccountCode option:last").attr("selected", "selected");

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

        var openingBalanceTotal = parseFloat(0);
        var debitTotal = parseFloat(0);
        var creditTotal = parseFloat(0);
        var diff = parseFloat(0);
        var closingBalanceTotal = parseFloat(0);

        var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 2, maximumFractionDigits: 2});

        $("#tb_report_datatable tr").each(function () {
            if ($(this).find("td").length) {

//	    		$(this).find("td:eq(1)").css("text-align", "LEFT");//
////	    		  if($(this).find("td").length>3)
////	    			  {
////	    			   $(this).css('text-align', 'center');
////	    			  }
                $(this).find("td").each(function (index, td) {
                    console.log("aaa  " + index);
                    if (index > 2)
                        $(this).css('text-align', 'right');
                });

//	    		if($(this).index()>2)
//	    			{
//	    			$(this).children('td').eq($(this).index()).css('text-align', 'right');
//	    			}
//	    		 var td = $(this).children('td').eq(2);
//	    		// console.log("aaa  "+ $(this).index());
                // td.css("color", "RED");
                //td.addClass('right-align');
                // SET COLORS
                if ($(this).find("td:eq(1)").text().trim().length == 2) {
                    $(this).css("color", "WHITE");
                    $(this).css("background-color", "BLACK");
                }
                if ($(this).find("td:eq(1)").text().trim().length == 5) {
                    $(this).css("color", "WHITE");
                    $(this).css("background-color", "DIMGRAY");
                }
                if ($(this).find("td:eq(1)").text().trim().length == 9) {
                    $(this).css("color", "WHITE");
                    $(this).css("background-color", "DARKGRAY");
                }

                if ($(this).find("td:eq(1)").text().trim().length == 2) {
                    openingBalanceTotal = openingBalanceTotal + parseFloat($(this).find("td:eq(3)").text().replace(/[^0-9\.-]+/g, "") || 0);
                    debitTotal = debitTotal + parseFloat($(this).find("td:eq(4)").text().replace(/[^0-9\.-]+/g, "") || 0);
                    creditTotal = creditTotal + parseFloat($(this).find("td:eq(5)").text().replace(/[^0-9\.-]+/g, "") || 0);
                    diff = diff + parseFloat($(this).find("td:eq(6)").text().replace(/[^0-9\.-]+/g, "") || 0);
                    closingBalanceTotal = closingBalanceTotal + parseFloat($(this).find("td:eq(7)").text().replace(/[^0-9\.-]+/g, "") || 0);
                }
            }

        });

        $("#tb_report_datatable span#openingBalanceTotal").text(formatter.format(openingBalanceTotal));
        $("#tb_report_datatable span#debitTotal").text(formatter.format(debitTotal));
        $("#tb_report_datatable span#creditTotal").text(formatter.format(creditTotal));
        $("#tb_report_datatable span#diff").text(formatter.format(diff));
        $("#tb_report_datatable span#closingBalanceTotal").text(formatter.format(closingBalanceTotal));
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
        event.preventDefault();
        generateTBReport();
    });

    function generateTBReport() {

        // disable submit button
        $("#submitTBForm").attr("disabled", true);

        // calculate all Totals
        calculateTotal();

        if (!$("#companyIds").val() || !$("#level").val() || !$("#voucherStatusId").val() || !$("#financialYearId").val() || !$("#fromDate").val() || !$("#toDate").val()) {
            alert("PLEASE SELECT ALL THE FIELDS")
            return;
        }

        $('#tb_report_datatable').DataTable().clear();
        $('#tb_report_datatable').DataTable().destroy();
        var table = $('#tb_report_datatable').DataTable({
            dom: 'Bfrtip',
            'bPaginate': false,
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
                    download: 'open',
                    title: 'TRIAL BALANCE' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    footer: true,
                    messageTop: 'COMPANY : ' + $('#companyIds option:selected').toArray().map(item => item.text).join(", ")
                        + '\n' + 'BRANCH: ' + $('#branchIds option:selected').toArray().map(item => item.text).join(", ")
                        + '\n' + 'VOUCHER STATUS : ' + $("#voucherStatusId option:selected").text()
                        + '\n' + 'FINANCIAL YEAR : ' + $("#financialYearId option:selected").text() + " (" + getFormattedDate(new Date($("#fromDate").val())) + " - " + getFormattedDate(new Date($("#toDate").val())) + ")"
                        + '\n' + 'FROM ACCOUNT : ' + $("#fromAccountCode option:selected").text()
                        + '\n' + 'TO ACCOUNT : ' + $("#toAccountCode option:selected").text(),
                    messageBottom: null,
                    customize: function (doc) {
                        doc.defaultStyle.fontSize = 8;
                        //pageMargins [left, top, right, bottom]
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
                    title: 'TRIAL BALANCE' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    messageTop: 'COMPANY: ' + $('#companyIds option:selected').toArray().map(item => item.text).join(", ")
                        + '<br>' + 'BRANCH: ' + $('#branchIds option:selected').toArray().map(item => item.text).join(", ")
                        + '<br>' + 'VOUCHER STATUS: ' + $("#voucherStatusId option:selected").text()
                        + '<br>' + 'FINANCIAL YEAR: ' + $("#financialYearId option:selected").text() + " (" + getFormattedDate(new Date($("#fromDate").val())) + " - " + getFormattedDate(new Date($("#toDate").val())) + ")"
                        + '<br>' + 'FROM ACCOUNT : ' + $("#fromAccountCode option:selected").text()
                        + '<br>' + 'TO ACCOUNT : ' + $("#toAccountCode option:selected").text(),
                    messageBottom: null,
                    footer: true,
                    autoPrint: false,
                    exportOptions: {
                        stripHtml: false,
                        stripNewlines: false
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
                            if ($(this).find("td:eq(1)").text().trim().length == 2) {
                                $(this).find("td").each(function (index, td) {
                                    $(td).css("color", "WHITE");
                                    $(td).css("background-color", "BLACK");
                                });
                            }
                            if ($(this).find("td:eq(1)").text().trim().length == 5) {
                                $(this).find("td").each(function (index, td) {
                                    $(td).css("color", "WHITE");
                                    $(td).css("background-color", "DIMGRAY");
                                });
                            }
                            if ($(this).find("td:eq(1)").text().trim().length == 9) {
                                $(this).find("td").each(function (index, td) {
                                    $(td).css("color", "WHITE");
                                    $(td).css("background-color", "DARKGRAY");
                                });
                            }
                        });

                    }
                }
            ]
        });

        // PREPARE FORM DATA
        var formData = {
            companyIds: $("#companyIds").val(),
            accountThirdLevel: $("#accountThirdLevel").val(),
            branchIds: $("#branchIds").val(),
            level: $("#level").val(),
            fromAccountCode: $("#fromAccountCode").val(),
            toAccountCode: $("#toAccountCode").val(),
            voucherStatusId: $("#voucherStatusId").val(),
            financialYearId: $("#financialYearId").val(),
            fromDate: $("#fromDate").val(),
            toDate: $("#toDate").val(),
            upperRange: $("#upperRange").val(),
            lowerRange: $("#lowerRange").val(),
        }

        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: $("#tbReportForm").attr("action"),
            data: JSON.stringify(formData),
            dataType: "json",
            success: function (data) {

                var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 0, maximumFractionDigits: 0});
                // FILL TABLE ROWS
                var rowIndex = 0;
                /* $.each(data, function (i, tBEntry) {
                     rowIndex = rowIndex + 1;

                     table.row.add([
                         rowIndex,
                         tBEntry.accountFormattedCode,
                         tBEntry.accountName,
                         formatter.format(tBEntry.openingBalance),
                         formatter.format(tBEntry.debit),
                         formatter.format(tBEntry.credit),
                         formatter.format(tBEntry.diff),
                         formatter.format(tBEntry.closingBalance),
                     ]);

                     if ($("#level").val() > 1) {
                         $.each(tBEntry.children, function (j, secondLevel) {
                             rowIndex = rowIndex + 1;
                             table.row.add([
                                 rowIndex,
                                 secondLevel.accountFormattedCode,
                                 secondLevel.accountName,
                                 formatter.format(secondLevel.openingBalance),
                                 formatter.format(secondLevel.debit),
                                 formatter.format(secondLevel.credit),
                                 formatter.format(secondLevel.diff),
                                 formatter.format(secondLevel.closingBalance),
                             ]);

                             if ($("#level").val() > 2) {
                                 $.each(secondLevel.children, function (k, thirdLevel) {
                                     rowIndex = rowIndex + 1;
                                     table.row.add([
                                         rowIndex,
                                         thirdLevel.accountFormattedCode,
                                         thirdLevel.accountName,
                                         formatter.format(thirdLevel.openingBalance),
                                         formatter.format(thirdLevel.debit),
                                         formatter.format(thirdLevel.credit),
                                         formatter.format(thirdLevel.diff),
                                         formatter.format(thirdLevel.closingBalance),
                                     ]);

                                     if ($("#level").val() > 3) {
                                         $.each(thirdLevel.children, function (l, fourthLevel) {
                                             rowIndex = rowIndex + 1;
                                             table.row.add([
                                                 rowIndex,
                                                 fourthLevel.accountFormattedCode,
                                                 fourthLevel.accountName,
                                                 formatter.format(fourthLevel.openingBalance),
                                                 formatter.format(fourthLevel.debit),
                                                 formatter.format(fourthLevel.credit),
                                                 formatter.format(fourthLevel.diff),
                                                 formatter.format(fourthLevel.closingBalance),
                                             ]);
                                         });
                                     }

                                 });
                             }

                         });
                     }

                 });*/
                var selectElement = document.getElementById("myDivId");
                //  selectElement.classList.add("select2_single");
                // $(selectElement).;
                //  $(selectElement).select2();
                /* let selectElement = document.createElement("select");
                 selectElement.setAttribute("class", "form-control selectElement");
                 //  selectElement.addClass('select2_single');
                 selectElement = document.querySelector('.selectElement');
                 selectElement.className += ' select2_single';
                 //selectElement.className += ' select2_single';
                 // selectElement.setAttribute("id", "party");

 // create the option element and add it to the select element
                 const optionElement = document.createElement("option");
                 optionElement.setAttribute("value", "a1");
                 optionElement.textContent = "asif idrees";
                 selectElement.appendChild(optionElement);*/
                $.each(data, function (i, tBEntry) {
                    rowIndex = rowIndex + 1;

                    if ($("#level").val() > 1) {
                        $.each(tBEntry.children, function (j, secondLevel) {
                            rowIndex = rowIndex + 1;

                            if ($("#level").val() > 2) {
                                $.each(secondLevel.children, function (k, thirdLevel) {
                                    rowIndex = rowIndex + 1;

                                    if ($("#level").val() > 3) {
                                        $.each(thirdLevel.children, function (l, fourthLevel) {
                                            var optionEt = document.createElement("option");
                                            optionEt.setAttribute("value", fourthLevel.accountFormattedCode);
                                            optionEt.textContent = fourthLevel.accountName;
                                            // selectElement.appendChild(optionEt);
                                            console.log(fourthLevel.accountName);
                                        })
                                        $.each(thirdLevel.children, function (l, fourthLevel) {
                                            rowIndex = rowIndex + 1;
                                            table.row.add([
                                                rowIndex,
                                                fourthLevel.accountFormattedCode,
                                                selectElement.outerHTML,
                                                fourthLevel.accountName,
                                                (fourthLevel.openingBalance),
                                                (fourthLevel.debit),
                                                (fourthLevel.credit),
                                                (fourthLevel.diff),
                                                (fourthLevel.closingBalance),
                                            ])
                                            ;
                                        });
                                    }

                                });
                            }

                        });
                    }

                });
                table.draw();

                // calculate all Totals
                calculateTotal();
                // enable submit button
                $("#submitTBForm").attr("disabled", false);
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

    function getExcelBuilder() {
        var xlsBuilder = {
            filename: 'TB_' + new Date().toLocaleString(),
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
                    v: 'TRIAL BALANCE' + ' ('
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
                    v: 'FROM ACCOUNT :'
                }, {
                    k: 'B',
                    v: $("#fromAccountCode option:selected").text(),
                }]);
                var r7 = Addrow(7, [{
                    k: 'A',
                    v: 'TO ACCOUNT :'
                }, {
                    k: 'B',
                    v: $("#toAccountCode option:selected").text(),
                }]);

                sheet.childNodes[0].childNodes[1].innerHTML = r1
                    + r2
                    + r3
                    + r4
                    + r5
                    + r6
                    + r7
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

    $("#sheet").on("click", function (event) {

        $("#sheet").attr("disabled", true);
        $("#pac_man").show();
        event.preventDefault();

        var formData = {
            companyIds: $("#companyIds").val(),
            branchIds: $("#branchIds").val(),
            level: $("#level").val(),
            fromAccountCode: $("#fromAccountCode").val(),
            toAccountCode: $("#toAccountCode").val(),
            voucherStatusId: $("#voucherStatusId").val(),
            financialYearId: $("#financialYearId").val(),
            fromDate: $("#fromDate").val(),
            toDate: $("#toDate").val(),
            upperRange: $("#upperRange").val(),
            lowerRange: $("#lowerRange").val(),
            accountThirdLevel: $("#accountThirdLevel").val(),
        }
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "/reports/closing-balance-sheet",
            data: JSON.stringify(formData),
            dataType: "text",
            success: function (data) {
                $("#pac_man").hide();
                //console.log("  asif"+data);
                window.location.href = "/reports/closing-balance-sheet";
                $("#sheet").attr("disabled", false);
            },
            complete: function () {

            },
        });

    });
    $("#debitList").on("click", function (event) {

        $("#debitList").attr("disabled", true);
        $("#pac_man").show();
        event.preventDefault();

        var formData = {
            companyIds: $("#companyIds").val(),
            branchIds: $("#branchIds").val(),
            level: $("#level").val(),
            fromAccountCode: $("#fromAccountCode").val(),
            toAccountCode: $("#toAccountCode").val(),
            voucherStatusId: $("#voucherStatusId").val(),
            financialYearId: $("#financialYearId").val(),
            fromDate: $("#fromDate").val(),
            toDate: $("#toDate").val(),
            upperRange: $("#upperRange").val(),
            lowerRange: $("#lowerRange").val(),
        }
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "/reports/debit_list",
            data: JSON.stringify(formData),
            dataType: "text",
            success: function (data) {
                $("#pac_man").hide();
                //console.log("  asif"+data);
                window.location.href = "/reports/downlaod_debit_list";
                $("#debitList").attr("disabled", false);
            },
            complete: function () {

            },
        });

    });

    $("#creditList").on("click", function (event) {

        $("#creditList").attr("disabled", true);
        $("#pac_man").show();
        event.preventDefault();

        var formData = {
            companyIds: $("#companyIds").val(),
            branchIds: $("#branchIds").val(),
            level: $("#level").val(),
            fromAccountCode: $("#fromAccountCode").val(),
            toAccountCode: $("#toAccountCode").val(),
            voucherStatusId: $("#voucherStatusId").val(),
            financialYearId: $("#financialYearId").val(),
            fromDate: $("#fromDate").val(),
            toDate: $("#toDate").val(),
            upperRange: $("#upperRange").val(),
            lowerRange: $("#lowerRange").val(),
        }
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "/reports/credit_list",
            data: JSON.stringify(formData),
            dataType: "text",
            success: function (data) {
                $("#pac_man").hide();
                //console.log("  asif"+data);
                window.location.href = "/reports/download_credit_list";
                $("#creditList").attr("disabled", false);
            },
            complete: function () {

            },
        });

    });
});