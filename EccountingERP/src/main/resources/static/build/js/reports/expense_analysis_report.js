Number.prototype.toFixed = function (decimalPlaces) {
    return Number(Math.round(this + 'e' + decimalPlaces) + 'e-' + decimalPlaces);
}

$(document).ready(function () {
    var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 0, maximumFractionDigits: 0});

    let dummyData = "";
    let table = "";
    let rowIndex = 0;
    let allTotalValue = parseFloat(0);

    //  check or uncheck radio buttion
    // $("#dateIncluded").click(function () {
    //     $("input[name='" + $(this).attr("name") + "']:radio").not(this).removeData("chk");
    //     $(this).data("chk", !$(this).data("chk"));
    //     $(this).prop("checked", $(this).data("chk"));
    //     if ($("#dateIncluded").is(':checked')) {
    //         $("#dateId").hide();
    //     } else {
    //         $("#dateId").show();
    //     }
    // });

    // $("#summary").on("change", function () {
    //     $("#reportType").text($("#summary option:selected").text());
    // });

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

            for (let i = 0, len = data.length; i < len; i++) {
                let option = "<option value = " + data[i].id + ">" + data[i].formattedCode + "&emsp;" + data[i].name + "</option>";
                $("#itemDefId").append(option);
            }
        });
    }

    function getFormattedDate(date) {
        let year = date.getFullYear();
        let month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        let day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        return day + '/' + month + '/' + year;
    }

    // Saleem Added Function
    function addCommas(nStr) {
        nStr += '';
        x = nStr.split('.');
        x1 = x[0];
        x2 = x.length > 1 ? '.' + x[1] : '';
        let rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1)) {
            x1 = x1.replace(rgx, '$1,$2');
        }
        return x1 + x2;
    }

    function formatNum(value) {
        value = parseFloat(value);
        let output = addCommas(value.toFixed(2));
        return value < 0 ? '(' + output.replace('-', '') + ')' : output;
    }
    // Saleem Add Number function

    // SUBMIT FORM
    $("#expenseAnalysisReport").submit(function (event) {
        // Prevent the form from submitting via the browser.
        event.preventDefault();
        createTable();
        ajaxCall();
    });

    function ajaxCall() {
        $("#submitGLForm").attr("disabled", true);  // disable submit button
        $('#loading').show();
        // PREPARE FORM DATA
        // var summary = 'false';
        // if ($("#summary").val() === "1")
        //     summary = 'true';

        let formData = {
            companyId: $("#companyId").val(),
            branchId: $("#branchId").val(),
            //negativePositive: $("#negativePositive").val(),
            fromDate: $("#fromDate").val(),
            toDate: $("#toDate").val(),
            accountCode: $("#accountCode").val(),
            level: $("#level").val(),
            balances: $("#balances").val(),
            financialYearId: $("#financialYearId").val(),
            dateIncluded: $("#dateIncluded").prop('checked')
            //dateIncluded: $("#dateIncluded").is(':checked')
        };

        // console.log(formData);

        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: $("#expenseAnalysisReport").attr("action"),
            data: JSON.stringify(formData),
            dataType: "json",
            success: function (data) {
                dummyData = data;
                // console.log(dummyData);
                allTotalValue = parseFloat(0.00);
                $("span#allTotal").text('');

                $.each(dummyData, function (i, account) {

                    // var columnName =sLEntry.itemName;
                    // if(parseFloat(sLEntry.closingQty)!=parseFloat(sLEntry.qty))
                    // columnName ="<a style='color: #009900;'>"+sLEntry.itemName+"</a>";
                    // accLevelTotal

                    let myPercentage = parseFloat(0.00);

                    myPercentage = (account.totalAmount / account.accLevelTotal) * 100;

                    if(isNaN(myPercentage)) {
                        myPercentage = 0;
                    }

                    //"<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/reports/general_ledger/?code=" + gLEntry.accountFormattedCode.replaceAll("-", "") + "&mobileNumbers=" + gLEntry.accountContactMobile.replace(", ", ",").replace("+92", "0").trim() + "&fromDate=" + $("#fromDate").val() + "&toDate=" + $("#toDate").val() + "&financialYearId=" + $("#financialYearId").val() + "&companyId=" + $("#companyId").val() + "&branchId=" + $("#branchId").val() + ">" + gLEntry.accountFormattedCode + "</a>",

                    allTotalValue = account.accLevelTotal;
                    // console.log("code " + account.accountFullCode + " " + account.accountFullCode.length);
                    table.row.add([
                        account.accountFullCode.length > 9 ? "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/reports/general_ledger/?code=" + account.accountCode + "&mobileNumbers=" + account.accountContactMobile.replace(", ", ",").replace("+92", "0").trim() + "&fromDate=" + $("#fromDate").val() + "&toDate=" + $("#toDate").val() + "&financialYearId=" + $("#financialYearId").val() + "&companyId=" + $("#companyId").val() + "&branchId=" + $("#branchId").val() + ">" + account.accountFullCode + "</a>" : account.accountFullCode,
                        account.accountName,
                        account.totalAmount,
                        myPercentage,
                        account.lastPercent
                    ]);
                });
                table.draw();
                setFooterValue();
                $("#submitGLForm").attr("disabled", false);  // enable submit button
                $('#loading').hide();  // hide loading indicator
                // if balance is selected set table account to selection
            },
            complete: function () {
                $("#submitGLForm").attr("disabled", false);  // enable submit button
                $('#loading').hide();  // hide loading indicator
            },
        });

    }
    function setFooterValue(){
        //allTotalValue
        $("span#allTotal").text(formatNum(allTotalValue));
    }

    function createTable() {
        $("#exp_report_datatable").DataTable().clear();
        $("#exp_report_datatable").DataTable().destroy();
        table = $('#exp_report_datatable').DataTable({
            dom: 'Bfrtip',
            deferRender: true,
            // scrollY: 500,
            // scroller: true,
            destroy: true,
            fixedHeader: true,
            bPaginate: false,
            columnDefs: [
                {
                    type: 'num',
                    targets: [2,3,4],
                    className: "text-right",
                    render: $.fn.dataTable.render.number(",", ".", 0, "")
                },
            ],
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

                {
                    extend: 'pdfHtml5',
                    title: 'EXPENSE ANALYSIS REPORT' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    messageTop: 'COMPANY: ' + $("#companyId option:selected").text()
                        + '  ' + 'BRANCH: ' + $("#branchId option:selected").text()
                        + '  \n ' + 'Period: ' + $("#fromDate").val() + "  To  " + $("#toDate").val(),
                    messageBottom: null,
                    footer: true,
                    // exportOptions: {
                    // 	columns: [0, 1, 2, 3,4,5 ,6,7],
                    // 	stripNewlines: false,
                    // },
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
                    title: 'EXPENSE ANALYSIS REPORT' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    messageTop: 'COMPANY: ' + $("#companyId option:selected").text()
                        + '  ' + 'BRANCH: ' + $("#branchId option:selected").text()
                        + '   ' + 'ACCOUNT: ' + $("#accountCode option:selected").text()
                        + '  \n ' + 'Period: ' + $("#fromDate").val() + "  To  " + $("#toDate").val(),
                    messageBottom: null,
                    footer: true,
                    autoPrint: false,
                    // exportOptions: {
                    // 	columns: [0, 1, 2, 3,4,5 ,6,7],
                    // 	stripNewlines: false,
                    // },
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
                return;

                var api = this.api(), data;

                let colNumber = [4, 5, 6, 8];

                // Remove the formatting to get integer data for summation
                let intVal = function (i) {
                    return typeof i === 'string' ?
                        i.replace(/[\$,]/g, '') * 1 :
                        typeof i === 'number' ?
                            i.toFixed(2) : 0;
                };

                for (i = 0; i < colNumber.length; i++) {
                    let colNo = colNumber[i];
                    let total = api
                        .column(colNo)
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);
                    $(api.column(colNo).footer()).html(formatter.format(parseFloat(total).toFixed(2)));
                }
            }
        });
    }

    function getExcelBuilder() {
        let xlsBuilder = {
            filename: 'EXPENSE ANALYSIS REPORT' + new Date().toLocaleString(),
            sheetName: 'sheet1',
            customize: function (xlsx) {
                let sheet = xlsx.xl.worksheets['sheet1.xml'];
                let downrows = 8;
                let clRow = $('row', sheet);
                let msg;
                // update Row
                clRow.each(function () {
                    let attr = $(this).attr('r');
                    let ind = parseInt(attr);
                    ind = ind + downrows;
                    $(this).attr("r", ind);
                });

                // Update row > c
                $('row c ', sheet).each(
                    function () {
                        let attr = $(this).attr('r');
                        let pre = attr.substring(0, 1);
                        let ind = parseInt(attr.substring(1, attr.length));
                        ind = ind + downrows;
                        $(this).attr("r", pre + ind);
                    });

                function Addrow(index, data) {

                    msg = '<row xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" r="'
                        + index + '">';
                    for (let i = 0; i < data.length; i++) {
                        let key = data[i].k;
                        let value = data[i].v;
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

                let r1 = Addrow(1, [{
                    k: 'A',
                    v: 'INVENTORY FORECASTING' + ' ('
                        + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim()
                        + ')'
                }]);
                let r2 = Addrow(2, [{
                    k: 'A',
                    v: 'COMPANY :'
                }, {
                    k: 'B',
                    v: $("#companyId option:selected").text()
                }]);
                let r3 = Addrow(3, [{
                    k: 'A',
                    v: 'BRANCH :'
                }, {
                    k: 'B',
                    v: $("#branchId option:selected").text()
                }]);
                let r4 = Addrow(4, [{
                    k: 'A',
                    v: 'BALANCE :'
                }, {
                    k: 'B',
                    v: $("#balances option:selected").text()
                }]);
                let r5 = Addrow(5, [{
                    k: 'A',
                    v: 'PERIOD :'
                }, {
                    k: 'B',
                    v: $("#financialYearId option:selected").text() + " (" + getFormattedDate(new Date($("#fromDate").val())) + " - " + getFormattedDate(new Date($("#toDate").val())) + ")"
                }]);

                let r7 = Addrow(7, [{
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
                    + r7
                    + sheet.childNodes[0].childNodes[1].innerHTML;
            },
            /*
             * exportOptions: { columns: [0, 1, 2, 3] }
             */
        }
        return xlsBuilder;
    }

});