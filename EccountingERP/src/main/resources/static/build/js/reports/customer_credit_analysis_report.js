Number.prototype.toFixed = function (decimalPlaces) {
    return Number(Math.round(this + 'e' + decimalPlaces) + 'e-' + decimalPlaces);
};

$(document).ready(function () {
    var formatter = new Intl.NumberFormat("ur-PK", {minimumFractionDigits: 0, maximumFractionDigits: 0});

    var dummyData = "";
    var table = "";
    var rowIndex = 0;

    //  check or uncheck radio buttion
    $("#dateIncluded").click(function () {
        $("input[name='" + $(this).attr("name") + "']:radio").not(this).removeData("chk");
        $(this).data("chk", !$(this).data("chk"));
        $(this).prop("checked", $(this).data("chk"));
        if ($("#dateIncluded").is(':checked')) {
            $("#dateId").hide();
        } else {
            $("#dateId").show();
        }
    });

    $("#summary").on("change", function () {
        $("#reportType").text($("#summary option:selected").text());
    });

    $("#companyId").on("change", function () {
        loadCompanyBranches();
    });

    function loadCompanyBranches() {

        if (!$("#companyId").val() || $("#companyId").val() === 0) {
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

            for (var i = 0, len = data.length; i < len; i++) {
                var option = "<option value = " + data[i].id + ">" + data[i].formattedCode + "&emsp;" + data[i].name + "</option>";
                $("#itemDefId").append(option);
            }
        });
    }

    // Saleem Added Function
    function addCommas(nStr) {
        nStr += "";
        x = nStr.split(".");
        x1 = x[0];
        x2 = x.length > 1 ? "." + x[1] : "";
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1)) {
            x1 = x1.replace(rgx, "$1,$2");
        }
        return x1 + x2;
    }

    function formatNum(value) {
        value = parseFloat(value);
        var output = addCommas(value.toFixed(2));
        return value < 0 ? "(" + output.replace("-", "") + ")" : output;
    }
    // Saleem Add Number function

    function getFormattedDate(date) {
        var year = date.getFullYear();
        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : "0" + month;
        var day = date.getDate().toString();
        day = day.length > 1 ? day : "0" + day;
        return day + "/" + month + "/" + year;
    }

    // SUBMIT FORM
    $("#customerCreditAnalysisReport").submit(function (event) {
        // Prevent the form from submitting via the browser.
        event.preventDefault();
        createTable();
        ajaxCall();
    });

    function ajaxCall() {
        $("#submitGLForm").attr("disabled", true);  // disable submit button
        $("#loading").show();

        //$("#sM_report_datatable").DataTable().clear();
        //$("#sM_report_datatable").DataTable().destroy();
        //table = $("#sM_report_datatable").DataTable();
        // PREPARE FORM DATA
        // var summary = 'false';
        // if ($("#summary").val() === "1")
        //     summary = 'true';

        var formData = {
            companyId: $("#companyId").val(),
            branchId: $("#branchId").val(),
            negativePositive: $("#negativePositive").val(),
            fromDate: $("#fromDate").val(),
            toDate: $("#toDate").val(),
            accountCode: $("#accountCode").val(),
            averageDaysFilter: $("#averageDaysFilter").val(),
            dateIncluded: $("#dateIncluded").is(":checked")
        };

        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: $("#customerCreditAnalysisReport").attr("action"),
            data: JSON.stringify(formData),
            dataType: "json",
            success: function (data) {
                dummyData = data;
                $.each(dummyData, function (i, account) {
                    var totalSales = account.saleSum;
                    var salesCustomer = account.sales;
                    var closingBalance = account.closing;
                    var salesPercentage = 0;
                    if(totalSales > 0 && salesCustomer > 0){
                        salesPercentage = (salesCustomer / totalSales) * 100;
                    }
                    var amt1 = parseFloat(salesPercentage);
                    var reciptRate = 0;
                    var collectionRate = account.collection;
                    var openBalanceRate = account.opening;
                    var totalsRate = salesCustomer + openBalanceRate;
                    if(collectionRate > 0 && totalsRate>0){
                        reciptRate = (collectionRate / totalsRate) * 100;
                    }
                    var myDays = "0";
                    var myAvgDays = account.avg2Days;
                    var rating = account.rating;

                    var myFirstCheck = parseFloat(account.opening) + parseFloat(account.collection) + parseFloat(account.sales) + parseFloat(account.closing);
                    var mySecondCheck = parseFloat(account.collection) + parseFloat(account.sales);
                    var myThirdCheck = parseFloat(account.sales);

                    // if(parseFloat(myFirstCheck) <= 1 ){
                    //     myDays = "Zero Balances";
                    //     rating = "-";
                    // } else if(parseFloat(mySecondCheck) < 1 ){
                    //     myDays = "No Activity";
                    //     rating = "-";
                    // } else if(parseFloat(myThirdCheck) < 1 ){
                    //     myDays = "No Sales";
                    //     rating = "-";
                    // } else {
                    //     var myCal = (closingBalance / salesCustomer) * myAvgDays;
                    //     var oneAmt = parseFloat(myCal);
                    //     myDays = oneAmt.toFixed(2);
                    //     console.log("Average Days " + myAvgDays + " Actual Rating " + account.rating);
                    // }
                    // if(closingBalance > 0 && salesCustomer > 0) {
                    //     var myCal = (closingBalance / salesCustomer) * myAvgDays;
                    //     var oneAmt = parseFloat(myCal);
                    //     myDays = oneAmt.toFixed(2);
                    // } else if(closingBalance == 0 && salesCustomer > 0) {
                    //     myDays = "No Outstanding Balance";
                    //     rating = "-";
                    // } else if((collectionRate + salesCustomer) == 0) {
                    //     myDays = "No Activity";
                    //     rating = "-";
                    // } else if((collectionRate + salesCustomer + openBalanceRate + closingBalance) == 0) {
                    //     myDays = "Zero Balance";
                    //     rating = "-";
                    // } else if(closingBalance > 0 && salesCustomer == 0) {
                    //     myDays = "No Sales No Collection";
                    //     rating = "-";
                    // } else {
                    //     myDays = "-";
                    //     rating = "-";
                    // }

                    var amt = parseFloat(reciptRate);

                    //reciptRate = formatter.format(parseFloat(reciptRate).toFixed(2));
                   // console.log(" TEST ENTRY CHECK JS " + amt.toFixed(2) + " " + amt1.toFixed(2));
                    table.row.add([
                        account.partyCode,
                        account.partyName,
                        "<div style='text-align: right'>" + account.opening + "</div>",
                        "<div style='text-align: right'>" + account.sales + "</div>",
                        "<div style='text-align: right'>" + account.collection + "</div>",
                        "<div style='text-align: right'>" + account.closing + "</div>",
                        "<div style='text-align: right'>" + amt1.toFixed(2) + "</div>",
                        "<div style='text-align: right'>" + amt.toFixed(2) + "</div>",
                        "<div style='text-align: center'>" + account.avgDays + "</div>",
                        "<div style='text-align: right'>" + account.limit + "</div>",
                        "<div style='text-align: center'>" + rating + "</div>",
                        account.firstEntryDate,
                        account.lastEntryDate
                    ]);
                    // var columnName =sLEntry.itemName;
                    // if(parseFloat(sLEntry.closingQty)!=parseFloat(sLEntry.qty))
                    //    columnName ="<a style='color: #009900;'>"+sLEntry.itemName+"</a>";
                    // table.row.add([
                    //
                    // ]);
                });
                table.draw();
                $("#submitGLForm").attr("disabled", false);  // enable submit button
                $("#loading").hide();  // hide loading indicator
                // if balance is seleted set table account to selection
            },
            complete: function () {
                $("#submitGLForm").attr("disabled", false);  // enable submit button
                $("#loading").hide();  // hide loading indicator
            }
        });
    }

    function createTable() {
        $("#sM_report_datatable").DataTable().clear();
        $("#sM_report_datatable").DataTable().destroy();

        table = $("#sM_report_datatable").DataTable({
            dom: "Bfrtip",
            deferRender: true,
            // scrollY: 500,
            // scroller: true,
            destroy: true,
            fixedHeader: true,
            bPaginate: false,
            buttons: [
                {
                    extend: "copy",
                    text: "COPY",
                    title: null,
                    footer: true,
                },
                $.extend(true, {}, getExcelBuilder(), {
                    extend: "excelHtml5",
                    text: "EXCEL",
                    title: null,
                    footer: true,
                    exportOptions: {
                        stripNewlines: false
                    }
                }),

                {
                    extend: "pdfHtml5",
                    title: "CUSTOMER CREDIT ANALYSIS REPORT" + " (" + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ")",
                    messageTop: "COMPANY: " + $("#companyId option:selected").text()
                        + "  " + "BRANCH: " + $("#branchId option:selected").text()
                        + "  \n " + "Period: " + $("#fromDate").val() + "  To  " + $("#toDate").val(),
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
                        doc.defaultStyle.alignment = "left";
                        doc.styles.tableHeader.alignment = "left";
                        doc.styles.tableFooter.alignment = "left";
                        doc["footer"] = function (page, pages) {
                            return {
                                columns: [
                                    "",
                                    {
                                        // This is the right column
                                        alignment: "right",
                                        text: ["page ", {text: page.toString()}, " of ", {text: pages.toString()}]
                                    }
                                ],
                                margin: [10, 0]// [left or right , up or down]
                            };
                        };
                    }
                },
                {
                    extend: "print",
                    title: "CUSTOMER CREDIT ANALYSIS REPORT" + " (" + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ")",
                    messageTop: "COMPANY: " + $("#companyId option:selected").text()
                        + "   " + "BRANCH: " + $("#branchId option:selected").text()
                        + "   " + "ACCOUNT: " + $("#accountCode option:selected").text()
                        + "  \n " + "Period: " + $("#fromDate").val() + "  To  " + $("#toDate").val(),
                    messageBottom: null,
                    footer: true,
                    autoPrint: false,
                    // exportOptions: {
                    // 	columns: [0, 1, 2, 3,4,5 ,6,7],
                    // 	stripNewlines: false,
                    // },
                    customize: function (win) {
                        $(win.document.body).find("table").addClass("display").css("font-size", "12px");
                        $(win.document.body).find("tr:nth-child(odd) td").each(function (index) {
                            $(this).css("background-color", "WHITESMOKE");
                        });
                        $(win.document.body).find("tr:nth-child(even) td").each(function (index) {
                            $(this).css("background-color", "WHITE");
                        });

                        $(win.document.body).css("background-color", "WHITE");
                        $(win.document.body).css("color", "DIMGRAY");

                        $(win.document.body).find("h1").css("text-align", "center");
                        //$(win.document.body).find('h1').css("color", "DIMGRAY");

                        $(win.document.body).find("div:first").css("text-align", "center");
                        //$(win.document.body).find('div:first').css("color", "DIMGRAY");

                        $(win.document.body).find("th").css("color", "WHITE");
                        $(win.document.body).find("th").css("background-color", "DIMGRAY");
                    }
                }
            ],
            "footerCallback": function (row, data, start, end, display) {
                return;
                var api = this.api(), data;

                var colNumber = [4, 5, 6, 8];

                // Remove the formatting to get integer data for summation
                var intVal = function (i) {
                    return typeof i === "string" ?
                        i.replace(/[\$,]/g, "") * 1 :
                        typeof i === "number" ?
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
            }
        });
    }

    function getExcelBuilder() {
        var xlsBuilder = {
            filename: "CUSTOMER CREDIT ANALYSIS REPORT" + new Date().toLocaleString(),
            sheetName: "sheet1",
            customize: function (xlsx) {
                var sheet = xlsx.xl.worksheets["sheet1.xml"];
                var downrows = 8;
                var clRow = $("row", sheet);
                var msg;
                // update Row
                clRow.each(function () {
                    var attr = $(this).attr("r");
                    var ind = parseInt(attr);
                    ind = ind + downrows;
                    $(this).attr("r", ind);
                });

                // Update row > c
                $("row c ", sheet).each(
                    function () {
                        var attr = $(this).attr("r");
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
                    v: 'CUSTOMER CREDIT ANALYSIS REPORT' + ' ('
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
                    v: 'BALANCE :'
                }, {
                    k: 'B',
                    v: $("#balances option:selected").text()
                }]);
                var r5 = Addrow(5, [{
                    k: 'A',
                    v: 'PERIOD :'
                }, {
                    k: 'B',
                    v: $("#financialYearId option:selected").text() + " (" + getFormattedDate(new Date($("#fromDate").val())) + " - " + getFormattedDate(new Date($("#toDate").val())) + ")"
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