$(document).ready(function () {

    let formatter = new Intl.NumberFormat("ur-PK", {minimumFractionDigits: 0, maximumFractionDigits: 0});
    let formatterDecimal = new Intl.NumberFormat("ur-PK", {minimumFractionDigits: 0, maximumFractionDigits: 4});

    let buttonName = "";

    //  check or uncheck radio buttion
    $("#dateIncluded").click(function () {
        $("input[name='" + $(this).attr("name") + "']:radio").not(this).removeData("chk");
        $(this).data("chk", !$(this).data("chk"));
        $(this).prop("checked", $(this).data("chk"));
        if ($("#dateIncluded").is(":checked")) {
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


    // SUBMIT FORM
    $("#stockMovementReport").submit(function (event) {
        // Prevent the form from submitting via the browser.
        buttonName = $("#submitGLForm").text();
        userLog();
        event.preventDefault();
        generateGLReport();
    });

    function generateGLReport() {
        $('#gl_report_datatable').DataTable().clear();
        $('#gl_report_datatable').DataTable().destroy();
        let table = $('#gl_report_datatable').DataTable({
            dom: 'Bfrtip',
            'bPaginate': false,
            destroy: true,
            columnDefs: [
                {
                    type: 'num',
                    targets: 5,
                    className: "text-right",
                    render: $.fn.dataTable.render.number(",", ".", 2, "")
                },
                {
                    type: 'num',
                    targets: 6,
                    className: "text-right",
                    render: $.fn.dataTable.render.number(",", ".", 2, "")
                },
                {
                    type: 'num',
                    targets: 7,
                    className: "text-right",
                    render: $.fn.dataTable.render.number(",", ".", 2, "")
                },
                {
                    type: 'num',
                    targets: 8,
                    className: "text-right",
                    render: $.fn.dataTable.render.number(",", ".", 2, "")
                },
                {
                    type: 'num',
                    targets: 9,
                    className: "text-right",
                    render: $.fn.dataTable.render.number(",", ".", 2, "")
                },
            ],
            buttons: [
                {
                    extend: 'copy',
                    text: 'COPY',
                    title: null,
                    footer: true,
                    exportOptions: {
                        columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
                    },
                    customize: function (doc) {
                        buttonName = "COPY";
                        userLog();
                    }
                },
                {
                    extend: 'pdfHtml5',
                    download: 'open',
                    title: 'INVENTORY PROFITABILITY REPORT' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    messageTop: null,
                    messageBottom: null,
                    footer: true,
                    exportOptions: {
                        columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
                        stripNewlines: false,
                    },
                    customize: function (doc) {
                        doc.defaultStyle.fontSize = 6;
                        doc.pageMargins = [10, 10, 10, 10];
                        doc.styles.tableHeader.fontSize = 6;
                        doc.styles.tableFooter.fontSize = 6;
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
                                margin: [10, 0]// [left or right, up or down]
                            }
                        });
                        buttonName = "PDF";
                        userLog();
                    }
                },
                {
                    extend: 'print',
                    text: 'VIEW',
                    title: 'INVENTORY PROFITABILITY REPORT' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    messageTop: null,
                    messageBottom: null,
                    footer: true,
                    autoPrint: false,
                    exportOptions: {
                        stripHtml: false,
                        stripNewlines: false,
                        columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
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
                        buttionName = "VIEW";
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
                                margin: [10, 0]// [left or right, up or down]
                            }
                        });

                    }
                }
            ],
            "footerCallback": function (row, data, start, end, display) {
                var api = this.api(), data;

                var colNumber = [5, 6, 7, 8, 9];

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
                    var total = api.column(colNo).data().reduce(
                        function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);
                    //$(api.column(colNo).footer()).html(parseFloat(total).toFixed(2));
                    //if ($("#userFullName").val() === 'mst')
                    $(api.column(colNo).footer()).html(formatter.format(parseFloat(total).toFixed(2)));
                }
            }
        });


        //AJAX CALL

        $("#submitGLForm").attr("disabled", true);  // disable submit button
        $("#loading").show();
        // PREPARE FORM DATA
        let summary = "false";
        if ($("#summary").val() === "1")
            summary = "true";

        let formData = {
            companyId: $("#companyId").val(),
            branchId: $("#branchId").val(),


            name: $("#name").val(),

            fromDate: $("#fromDate").val(),
            toDate: $("#toDate").val(),
            itemDefId: $("#itemDefId").val(),
            itemCategoryId: $("#itemCategoryId").val(),
            accountCode: $("#accountCode").val(),
            dateIncluded: $("#dateIncluded").is(':checked'),
            summary: summary,

            //brand: $("#brand").val(),
        };

        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: $("#glReportForm").attr("action"),
            data: JSON.stringify(formData),
            dataType: "json",
            success: function (data) {

                $.each(data, function (i, sLEntry) {
                    let costAmount = parseFloat(sLEntry.qty) * parseFloat(sLEntry.realCost);
                    let profitAmount = parseFloat(sLEntry.saleAmount) - parseFloat(costAmount);

                    if (parseFloat(sLEntry.realCost) === 0) {
                        profitAmount = parseFloat(sLEntry.saleAmount) - parseFloat(sLEntry.costAmount);
                    }

                    if (summary === "true") {
                        table.row.add([
                            (i + 1),
                            sLEntry.catName,
                            sLEntry.itemCode,
                            sLEntry.item,
                            sLEntry.uom,
                            sLEntry.qty,
                            sLEntry.saleAmount,
                            sLEntry.costAmount,
                            profitAmount,
                            sLEntry.realCost
                        ]);
                    } else {
                        table.row.add([
                            (i + 1),
                            sLEntry.catName,
                            sLEntry.itemCode,
                            sLEntry.item,
                            sLEntry.uom,
                            sLEntry.qty,
                            sLEntry.saleAmount,
                            costAmount,
                            profitAmount,
                            sLEntry.realCost
                        ]);
                    }

                });

                table.draw();
                $("#submitGLForm").attr("disabled", false);  // enable submit button
                $('#loading').hide();  // hide loading indicator
            },
            complete: function () {
                $("#submitGLForm").attr("disabled", false);  // enable submit button
                $('#loading').hide();  // hide loading indicator
            },
        });

        //AJAX CALL
    }

    function userLog() {

        let userLogObj = {
            idNumber: "",
            code: "",
            buttonClick: buttonName,
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
            },
            error: function (data) {
                successmessage = 'Error';
            },
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

});