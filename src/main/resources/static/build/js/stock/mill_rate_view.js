Number.prototype.toFixed = function (decimalPlaces) {
    return Number(Math.round(this + 'e' + decimalPlaces) + 'e-' + decimalPlaces);
}
$(document).ready(function () {
    var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 0, maximumFractionDigits: 0});
    console.log("asif");
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
            return;
        }

        $("#fromDate").attr("min", $("#financialYearId option:selected").attr("data-value").split("|")[0]);
        $("#fromDate").attr("max", $("#financialYearId option:selected").attr("data-value").split("|")[1]);
        //$("#fromDate").val($("#financialYearId option:selected").attr("data-value").split("|")[0]);

        $("#toDate").attr("min", $("#financialYearId option:selected").attr("data-value").split("|")[0]);
        $("#toDate").attr("max", $("#financialYearId option:selected").attr("data-value").split("|")[1]);
        //$("#toDate").val($("#financialYearId option:selected").attr("data-value").split("|")[1]);

    }


    function getFormattedDate(date) {
        var year = date.getFullYear();
        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : "0" + month;
        var day = date.getDate().toString();
        day = day.length > 1 ? day : "0" + day;
        console.log("asif");
        return day + "/" + month + "/" + year;
    }

    function getMyDate(date) {
        console.log(date);
        var year = date.getFullYear();
        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : "0" + month;
        var day = date.getDate().toString();
        day = day.length > 1 ? day : "0" + day;
        console.log("asif");
        return year + "-" + month + "-" + day;
    }

    $('body').on('click', '.editMillRate', function () {

        $("#zoomer").attr('src', "");
        document.querySelectorAll('#_pics #floatleft');
        var now = new Date($(this).closest("tr").find(".entryDate").val().trim());
        var day = ("0" + now.getDate()).slice(-2);
        var month = ("0" + (now.getMonth() + 1)).slice(-2);
        var today = now.getFullYear() + "-" + (month) + "-" + (day);
        ///console.log("mySalet"+today);
        $("#millRateForm #itemDef").val($(this).closest("tr").find('.itemDefId').val()).trigger('change');
        $("#millRateForm #mill\\.id").val($(this).closest("tr").find(".millId").val()).trigger('change');
        $("#millRateForm #rate").val($(this).closest("tr").find(".rate").text().trim());

        $("#millRateForm #remarks").val($(this).closest("tr").find(".remarks").text().trim());

        $("#millRateForm #cashRate").val($(this).closest("tr").find(".cashRate").text().trim());
        $("#millRateForm #id").val($(this).closest("tr").find(".id").val().trim());
        $("#millRateForm #entryDate").val(today);//.trigger('change');

        $("#editMillRateModel").modal("show");

        // pics.appendChild(divClone);

    });
    $("#downloadMillRate").on('click', function (event) {
        // var formData = {
        //     itemDefId: $("#itemDefId").val(),
        //     millId: $("#millId").val(),
        //     fromDate: $("#fromDate").val(),
        //
        //     entryStatus: $("#entryStatus").val()
        // }
        location.replace("/stock/print-mill-rate?itemDef=" + $("#itemDefId").val() + "&mill=" + $("#millId").val() + "&date=" + $("#fromDate").val(),)
        // DO POST
        // $.ajax({
        //     type: "GET",
        //     contentType: "application/json",
        //     url: "/stock/print-mill-rate?itemDef=" + $("#itemDefId").val() + "&mill=" + $("#millId").val() + "&date=" + $("#fromDate").val(),
        //     //data: JSON.stringify(formData),
        //     dataType: "json",
        //     success: function (data) {
        //     }
        // });
    });
    $("#editMillRateModel #millRateForm").submit(function (event) {
        // Prevent the form from submitting via the browser.
        event.preventDefault();
        var formData = {
            itemDef: {id: $("#millRateForm #itemDef").val()},
            mill: {id: $("#millRateForm #mill\\.id").val()},

            rate: $("#millRateForm #rate").val(),
            cashRate: $("#millRateForm #cashRate").val(),
            remarks: $("#millRateForm #remarks").val(),
            entryDate: $("#millRateForm #entryDate").val(),
            id: $("#millRateForm #id").val(),


        }
        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: $("#millRateForm").attr("action"),
            data: JSON.stringify(formData),
            dataType: "text",
            success: function (data) {
                $('#editMillRateModel').modal('toggle');
                generateGLReport();
            }
        });
    });
    // SUBMIT FORM
    $("#gPReportForm").submit(function (event) {
        // Prevent the form from submitting via the browser.
        event.preventDefault();
        generateGLReport();
    });

    function generateGLReport() {

        $("#submitGLForm").attr("disabled", true);  // disable submit button
        $('#loading').show();  // show loading indicator

        $("#gl_report_datatable").DataTable().clear();
        $("#gl_report_datatable").DataTable().destroy();
        var table = $("#gl_report_datatable").DataTable({
            dom: "Bfrtip",
            deferRender: true,
            scrollY: 500,
            scroller: true,
            destroy: true,
            fixedHeader: true,
            // bPaginate: false,
            buttons: [
                /* {
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
                 }),*/
                /*{
                    extend: 'excelHtml5',
                    title: 'General Ledger' + ' (' + (new Date().toLocaleString()) + ')',
                    messageTop: 'COMPANY: ' + 'MST' + '\n' + 'ACCOUNT: ' + '111222333',
                    messageBottom: null,
                    stripNewlines: false
                },*/
                {
                    extend: "pdfHtml5",
                    title: "RATES" + " (" + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ")",
                    messageTop: "  " + "ITEM: " + $("#itemDefId option:selected").text() + " " + "MILL: " + $("#millId option:selected").text(),
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
                }
            ]

        });

        //console.log("form submited");
        // PREPARE FORM DATA
        console.log("i haa");
        var formData = {
            itemDefId: $("#itemDefId").val(),
            millId: $("#millId").val(),
            fromDate: $("#fromDate").val(),

            entryStatus: $("#entryStatus").val()
        }

        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "/stock/mill-rates-view",
            data: JSON.stringify(formData),
            dataType: "json",
            success: function (data) {

                // FILL TABLE ROWS
                $.each(data, function (i, millRateEntry) {

                    table.row.add([
                        '<td>' + (i + 1) + '</td>',
                        '<td><a class="date">' + getFormattedDate(new Date(millRateEntry.entryDate)) + '</a> <input type="hidden" class = "entryDate" value ="' + getMyDate(new Date(millRateEntry.entryDate)) + '"></td>',
                        '<td>' + millRateEntry.mill + '</td>',
                        '<td>' + millRateEntry.item + '</td>',
                        '<td><a class="rate">' + millRateEntry.rate + '</a> </td>',
                        '<td><a class="cashRate">' + millRateEntry.cashRate + '</a></td>',
                        '<td><a class="remarks">' + millRateEntry.remarks + '</a></td>',
                        '<td><input type= "hidden" class ="id" value=' + millRateEntry.id + '> <input type= "hidden" class ="itemDefId" value=' + millRateEntry.itemDefId + '> <input type= "hidden" class ="millId" value=' + millRateEntry.millId + '> <a  class="editMillRate" style="text-decoration: underline;color:red" href="javascript:void(0)">  EDIT</a></td>',

                    ]);
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

    /* $("#editMillRateModel").on("hidden.bs.modal", function () {
         $("#millRateForm").trigger("reset");
     });*/

});





