$(document).ready(function () {
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

            for (let i = 0, len = data.length; i < len; i++) {
                let option = "<option value = " + data[i].id + ">" + data[i].fullName + "</option>";
                $("#branchIds").append(option);
            }

            $("#branchIds").val(0).change();
        });
    }

    // SUBMIT FORM
    $("#tbReportForm").submit(function (event) {
        // Prevent the form from submitting via the browser.
        $("#balanceSheetPacMan").show();
        //buttionName = $("#submitTBForm").text();
        //userLog();
        event.preventDefault();
        generateTBReport();
    });

    function generateTBReport() {
        // disable submit button
        $("#submitTBForm").attr("disabled", true);

        $('#tb_report_datatable').DataTable().clear();
        $('#tb_report_datatable').DataTable().destroy();
        let tableName = $('#tb_report_datatable').DataTable({
            dom: "Bfrtip",
            deferRender: true,
            destroy: true,
            autoWidth: false,
            // fixedHeader: {
            //     header: true,
            //     footer: true
            // },
            scrollX: true,
            fixedColumns: {
                left: 2,
                right: 0
            },
            bPaginate: true,
            scrollY: 500,
            scroller: true,
            scrollCollapse: true,
            // style_cell: {
            //     'whiteSpace': 'normal',
            //     'height': 'auto',
            // },
            columnDefs: [
                {"width": "70px", "targets": 0},
                {"className": "cellStyling", "width": "300px", "targets": 1},
                // { "width": "10%", "targets": [2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22], },
                {
                    targets: [5, 6, 7, 8, 9, 10],
                    className: "text-right", width: '80px',
                    render: $.fn.dataTable.render.number(",", ".", 0, "")
                },
                {
                    render: function (data, type, full, meta) {
                        let instantText = (data != null && data.length > 35) ? data.substr(0, 35) : data == null ? "" : data;
                        return '<div class="text-overflow" title=' + '"' + data + '"' + '>' + instantText + '</div>';
                    },
                    targets: [1]
                },
                // {
                //     render: function (data, type, full, meta) {
                //         return "<div class='text-wrap width-200'>" + data + "</div>";
                //     },
                //     targets: 1
                // }
            ],
            buttons: [
                {
                    extend: 'copy',
                    text: 'COPY',
                    title: null,
                    footer: true,
                    customize: function (doc) {
                        //buttionName = "COPY";
                        //userLog();
                    }
                },
                {
                    extend: 'pdfHtml5',
                    download: 'open',
                    orientation: 'landscape',
                    title: 'CUSTOMER SALES REPORT' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    footer: true,
                    messageTop: 'COMPANY : ' + $('#companyIds option:selected').text()
                        + '\n' + 'BRANCH: ' + $('#branchIds option:selected').text()
                        + '\n' + 'REPORT TYPE : ' + $("#reportType option:selected").text()
                        + '\n' + 'CUSTOMER : ' + $("#accountCode option:selected").text()
                        + '\n' + 'FINANCIAL YEAR : ' + $("#financialYearId option:selected").text(),
                    messageBottom: null,
                    exportOptions: {
                        stripNewlines: false,
                        columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14],
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
                        //buttionName = "PDF";
                        //userLog();
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
                    title: 'CUSTOMER SALE REPORT' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    messageTop: 'COMPANY : ' + $('#companyIds option:selected').text()
                        + '\n' + 'BRANCH: ' + $('#branchIds option:selected').text()
                        + '\n' + 'REPORT TYPE : ' + $("#reportType option:selected").text()
                        + '\n' + 'CUSTOMER : ' + $("#accountCode option:selected").text()
                        + '\n' + 'FINANCIAL YEAR : ' + $("#financialYearId option:selected").text(),
                    messageBottom: null,
                    footer: true,
                    autoPrint: false,
                    exportOptions: {
                        stripHtml: false,
                        stripNewlines: false,
                        columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14],
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
                        //buttionName = "VIEW";
                        //userLog();
                    }
                }
            ],
            // "footerCallback": function (row, data, start, end, display) {
            //     var api = this.api(), data;
            //
            //     var colNumber = [2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14];
            //
            //     // Remove the formatting to get integer data for summation
            //     var intVal = function (i) {
            //         if (typeof i === 'string') {
            //             return i.replace(/[\$,]/g, '') * 1;
            //         } else if (typeof i === 'number') {
            //             return parseFloat(i);
            //         } else {
            //             return parseFloat(0);
            //         }
            //     };
            //
            //     for (i = 0; i < colNumber.length; i++) {
            //         var colNo = colNumber[i];
            //         var total = api
            //             .column(colNo)
            //             .data()
            //             .reduce(function (a, b) {
            //                 return intVal(a) + intVal(b);
            //             }, 0);
            //         //$(api.column(colNo).footer()).html(parseFloat(total).toFixed(2));
            //         $(api.column(colNo).footer()).html(formatter.format(parseFloat(total).toFixed(2)));
            //     }
            // },
        });
        // tableName.columns.adjust().draw();
        //tableName.header()
        //$('#tb_report_datatable tbody').empty();
        // PREPARE FORM DATA
        let formData = {
            companyIds: $("#companyIds").val(),
            branchIds: $("#branchIds").val(),
            vendorIds: $("#vendorIds").val(),
            monthlyPaymentTarget: $("#monthlyPaymentTarget").val(),
            filterVendors: $("#filterVendors").val(),
            modeOfPayment: $("#modeOfPayment").val(),
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
                let formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 2, maximumFractionDigits: 2});
                // FILL TABLE ROWS
                let rowIndex = 0;
                let closingBalance = 0;

                $.each(data, function (i, tBEntry) {
                    rowIndex = rowIndex + 1;

                    closingBalance = (parseFloat(tBEntry.currentStock) + parseFloat(tBEntry.purchaseBookingQuantity)) - parseFloat(tBEntry.salesOrderQuantity);

                    tableName.row.add([
                        tBEntry.partyName,
                        tBEntry.itemSubCategory,
                        tBEntry.itemCategory,
                        tBEntry.itemDefCode,
                        tBEntry.itemDef,
                        tBEntry.currentStock,
                        tBEntry.purchaseBookingQuantity,
                        0,
                        tBEntry.lastRate,
                        tBEntry.salesOrderQuantity,
                        closingBalance,
                        "",
                    ]);

                });
                tableName.draw();
                setTableFooter();
                //console.log('Column 1: ' + column1)

                //setTableFooter(formatter, column1, column2, column3)
                // enable submit button
                $("#submitTBForm").attr("disabled", false);
                $("#balanceSheetPacMan").hide();
            },
            complete: function () {
                // enable submit button
                $("#submitTBForm").attr("disabled", false);
            },
        });
    }

    function setTableFooter() {
        //console.log('IN CALCULATION FUNCTION')
        let formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 2, maximumFractionDigits: 2});
        let column1Total = parseFloat(0);
        let column2Total = parseFloat(0);
        let column3Total = parseFloat(0);

        //console.log('STARTING LOOP IN CALCULATION FUNCTION')
        $("#tb_report_datatable tr").each(function () {
            column1Total = column1Total + parseFloat($(this).find("td:eq(5)").text().replace(/[^0-9\.-]+/g, "") || 0);
            column2Total = column2Total + parseFloat($(this).find("td:eq(6)").text().replace(/[^0-9\.-]+/g, "") || 0);
            column3Total = column3Total + parseFloat($(this).find("td:eq(9)").text().replace(/[^0-9\.-]+/g, "") || 0);

            //console.log('V1: ' + column1Total + ' V2: ' + column2Total + ' V3:' + column3Total);
        });

       // console.log(' -- ' + $("#tb_report_datatable span#currentStockTotal").text());

        $("span#currentStockTotal").text(column1Total);
        $("span#pendingOrderTotal").text(column2Total);
        $("span#salesOrderTotal").text(column3Total);
    }

    function getFormattedDate(date) {
        let year = date.getFullYear();
        let month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        let day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        return day + '/' + month + '/' + year;
    }

    $("#btnExcel").on("click", function () {
        //console.log("IN FUNCTION");

        //console.log("?companyIds=" + $("#companyIds").val() + "&branchIds=" + $("#branchIds").val() + "&vendorIds=" + $("#vendorIds").val() + "&monthlyPaymentTarget=" + $("#monthlyPaymentTarget").val() + "&filterVendors=" + $("#filterVendors").val() + "&modeOfPayment=" + $("#modeOfPayment").val() + "&fromDate=" + $("#fromDate").val() + "&toDate=" + $("#toDate").val());

        window.location = "/reports/download/vendorPaymentScheduleReport/?companyIds=" + $("#companyIds").val() + "&branchIds=" + $("#branchIds").val() + "&vendorIds=" + $("#vendorIds").val() + "&monthlyPaymentTarget=" + $("#monthlyPaymentTarget").val() + "&filterVendors=" + $("#filterVendors").val() + "&modeOfPayment=" + $("#modeOfPayment").val() + "&fromDate=" + $("#fromDate").val() + "&toDate=" + $("#toDate").val() + "";
    });

});