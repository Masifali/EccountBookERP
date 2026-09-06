$(document).ready(function () {

    let formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 2, maximumFractionDigits: 2});
    let tableName = "";

    loadingValues();

    function loadingValues() {
        $(".companyIds, .itemCategoryIds").val(0).change();
    }

    function getFormattedDate(date) {
        let year = date.getFullYear();
        let month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        let day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        return day + '/' + month + '/' + year;
    }

    $("#minStockDays").on("input", function () {
        $("#myDaysHeading").text($(this).val() + " DAYS DEMAND");
    });

    $("#fromDate, #toDate").on("blur", function(){
        $("#demandBasis").change();
    })

    $("#demandBasis").on("change", function(){

        let formData = {
            companyIds: $("#companyIds").val(),
            itemCategoryIds: $("#itemCategoryIds").val(),
            itemDefIds: $("#itemDefIds").val(),
            demandBasis: $("#demandBasis").val(),
            dayDifference: $("#dayDifference").val(),
            purchaseRate: $("#purchaseRate").val(),
            purchaseRateUOM: $("#purchaseRateUOM").val(),
            minStockDays: $("#minStockDays").val(),
            minStockAvailable: $("#minStockAvailable").val(),
            reportType: $("#reportType").val(),
            fromDate: $("#fromDate").val(),
            toDate: $("#toDate").val(),
            shape: $("#shape").val(),
            shapeSize: $("#shapeSize").val(),
            gauge: $("#gauge").val(),
            UOM: $("#UOM").val(),
            consignment: $("#consignment").val(),
        }

        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "/reports/getStockDemandAnalysisDates",
            data: JSON.stringify(formData),
            dataType: "json",
            success: function (data) {
                let formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 2, maximumFractionDigits: 2});
                // FILL TABLE ROWS
                let rowIndex = 0;
                $.each(data, function (i, tBEntry) {
                    $("#fromDate").val(tBEntry.mStart);
                    $("#toDate").val(tBEntry.mEnd);
                    $("#dayDifference").val(tBEntry.mDays);
                });
                $("#balanceSheetPacMan").hide();
            },
            complete: function () {
                // enable submit button
                $("#balanceSheetPacMan").hide();
            },
        });
    });

    // $("#minStockDays").on("blur", function(){
    //     $("#myDaysHeading").text($(this).val() + " DAYS DEMAND");
    //     updateTableValues($(this).val());
    // });

    function updateTableValues(stockDays) {
        console.log(tableName);
        $(tableName).each(function () {
            let row = $(this);
            let perDay = row.find("td:eq(3)").text();
            let updateValue = parseFloat(perDay) * parseFloat(stockDays);
            updateValue = formatter.format(updateValue);
            row.find("td:eq(10)").text(updateValue);
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

        // if (!$("#companyIds").val() || !$("#financialYearId").val()) {
        //     alert("PLEASE SELECT ALL THE FIELDS")
        //     return;
        // }


        $('#tb_report_datatable').DataTable().clear();
        $('#tb_report_datatable').DataTable().destroy();
        tableName = $('#tb_report_datatable').DataTable({
            dom: "Bfrtip",
            deferRender: true,
            destroy: true,
            fixedHeader: true,
            bPaginate: false,
            columnDefs: [
                {
                    targets: 3,
                    className: "text-right",
                    //render: $.fn.dataTable.render.number(",", ".", 0, "")
                }, {
                    targets: 4,
                    className: "text-right",
                    //render: $.fn.dataTable.render.number(",", ".", 0, "")
                }, {
                    targets: 5,
                    className: "text-right",
                    //render: $.fn.dataTable.render.number(",", ".", 0, "")
                }, {
                    targets: 6,
                    className: "text-right",
                    //render: $.fn.dataTable.render.number(",", ".", 0, "")
                }, {
                    targets: 7,
                    className: "text-right",
                    //render: $.fn.dataTable.render.number(",", ".", 0, "")
                }, {
                    targets: 8,
                    className: "text-right",
                    //render: $.fn.dataTable.render.number(",", ".", 0, "")
                }, {
                    targets: 9,
                    className: "text-right",
                    //render: $.fn.dataTable.render.number(",", ".", 0, "")
                }, {
                    targets: 10,
                    className: "text-right",
                    //render: $.fn.dataTable.render.number(",", ".", 0, "")
                }, {
                    targets: 11,
                    className: "text-right",
                    //render: $.fn.dataTable.render.number(",", ".", 0, "")
                }, {
                    targets: 12,
                    className: "text-right",
                    //render: $.fn.dataTable.render.number(",", ".", 0, "")
                }, {
                    targets: 13,
                    className: "text-right",
                    //render: $.fn.dataTable.render.number(",", ".", 0, "")
                },
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
        //$('#tb_report_datatable tbody').empty();
        // PREPARE FORM DATA
        let formData = {
            companyIds: $("#companyIds").val(),
            itemCategoryIds: $("#itemCategoryIds").val(),
            itemDefIds: $("#itemDefIds").val(),
            demandBasis: $("#demandBasis").val(),
            dayDifference: $("#dayDifference").val(),
            purchaseRate: $("#purchaseRate").val(),
            purchaseRateUOM: $("#purchaseRateUOM").val(),
            minStockDays: $("#minStockDays").val(),
            minStockAvailable: $("#minStockAvailable").val(),
            reportType: $("#reportType").val(),
            fromDate: $("#fromDate").val(),
            toDate: $("#toDate").val(),
            shape: $("#shape").val(),
            shapeSize: $("#shapeSize").val(),
            gauge: $("#gauge").val(),
            UOM: $("#UOM").val(),
            consignment: $("#consignment").val(),
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
                $.each(data, function (i, tBEntry) {
                    rowIndex = rowIndex + 1;

                    tableName.row.add([
                        tBEntry.itemCategory,
                        tBEntry.itemFormattedCode,
                        tBEntry.itemName,
                        formatter.format(tBEntry.perDay),
                        formatter.format(tBEntry.perWeek),
                        formatter.format(tBEntry.currentStock),
                        formatter.format(tBEntry.purchaseQty),
                        formatter.format(tBEntry.saleOrderQty),
                        formatter.format(tBEntry.availQty),
                        formatter.format(tBEntry.stockAvail),
                        formatter.format(tBEntry.daysCalculation),
                        formatter.format(tBEntry.excessShort),
                        formatter.format(0),
                        formatter.format(tBEntry.estimatedCost)
                    ]);
                    //tableName.draw();
                });
                tableName.draw();
                setTableFooter();
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

    function setTableFooter() {
        //console.log('IN CALCULATION FUNCTION')
        let formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 2, maximumFractionDigits: 2});
        let column1Total = parseFloat(0);
        let column2Total = parseFloat(0);
        let column3Total = parseFloat(0);
        let column4Total = parseFloat(0);
        let column5Total = parseFloat(0);
        let column6Total = parseFloat(0);
        let column7Total = parseFloat(0);
        let column8Total = parseFloat(0);
        let column9Total = parseFloat(0);
        let column10Total = parseFloat(0);

        //console.log('STARTING LOOP IN CALCULATION FUNCTION')
        $("#tb_report_datatable tr").each(function () {
            column1Total = column1Total + parseFloat($(this).find("td:eq(3)").text().replace(/[^0-9\.-]+/g, "") || 0);
            column2Total = column2Total + parseFloat($(this).find("td:eq(4)").text().replace(/[^0-9\.-]+/g, "") || 0);
            column3Total = column3Total + parseFloat($(this).find("td:eq(5)").text().replace(/[^0-9\.-]+/g, "") || 0);
            column4Total = column4Total + parseFloat($(this).find("td:eq(6)").text().replace(/[^0-9\.-]+/g, "") || 0);
            column5Total = column5Total + parseFloat($(this).find("td:eq(7)").text().replace(/[^0-9\.-]+/g, "") || 0);
            column6Total = column6Total + parseFloat($(this).find("td:eq(8)").text().replace(/[^0-9\.-]+/g, "") || 0);
            column7Total = column7Total + parseFloat($(this).find("td:eq(10)").text().replace(/[^0-9\.-]+/g, "") || 0);
            column8Total = column8Total + parseFloat($(this).find("td:eq(11)").text().replace(/[^0-9\.-]+/g, "") || 0);
            column9Total = column9Total + parseFloat($(this).find("td:eq(12)").text().replace(/[^0-9\.-]+/g, "") || 0);
            column10Total = column10Total + parseFloat($(this).find("td:eq(13)").text().replace(/[^0-9\.-]+/g, "") || 0);

            //console.log('V1: ' + column1Total + ' V2: ' + column2Total + ' V3:' + column3Total);
        });

        // console.log(' -- ' + $("#tb_report_datatable span#currentStockTotal").text());

        $("span#column1").text(formatter.format(column1Total));
        $("span#column2").text(formatter.format(column2Total));
        $("span#column3").text(formatter.format(column3Total));
        $("span#column4").text(formatter.format(column4Total));
        $("span#column5").text(formatter.format(column5Total));
        $("span#column6").text(formatter.format(column6Total));
        $("span#column7").text(formatter.format(column7Total));
        $("span#column8").text(formatter.format(column8Total));
        $("span#column9").text(formatter.format(column9Total));
        $("span#column10").text(formatter.format(column10Total));
    }


});