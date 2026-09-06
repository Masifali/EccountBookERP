$(document).ready(function () {

    function getFormData($form) {
        var unindexed_array = $form.serializeArray();
        var indexed_array = {};

        $.map(unindexed_array, function (n, i) {
            indexed_array[n['name']] = n['value'];
        });

        return indexed_array;
    }

    // SUBMIT FORM
    $("#gPReportForm").submit(function (event) {
        // Prevent the form from submitting via the browser.
        event.preventDefault();
        generateReport();
    });

    function generateReport() {

        $("#submitGLForm").attr("disabled", true);  // disable submit button
        $('#loading').show();  // show loading indicator

        $("#report_datatable").DataTable().clear();
        $("#report_datatable").DataTable().destroy();
        var table = $("#report_datatable").DataTable({
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
                    title: "Day Payments" + " (" + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ")",
                    messageTop: "  " + "Party: " + $("#accountCode option:selected").text() + " " + "FromDate: " + $("#fromDate").val() + " " + "ToDate: " + $("#toDate").val(),
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
            accountCode: $("#accountCode").val(),
            fromDate: $("#fromDate").val(),
            toDate: $("#toDate").val(),
            entryStatus: $("#entryStatus").val()
        }

        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "/payables/today-payment-view",
            data: JSON.stringify(formData),
            dataType: "json",
            success: function (data) {

                // FILL TABLE ROWS
                $.each(data, function (i, dayPayment) {

                    table.row.add([
                        '<td>' + (i + 1) + '</td>',
                        '<td><a class="date">' + getFormattedDate(new Date(dayPayment.entryDate)) + '</a> <input type="hidden" class = "entryDate" value ="' + getMyDate(new Date(dayPayment.entryDate)) + '"></td>',
                        '<td>' + dayPayment.account.accountName + '</td>',
                        '<td>' + dayPayment.bank.accountName + '</td>',
                        '<td><a class="amount">' + dayPayment.amount + '</a> </td>',
                        '<td><a class="status">' + '' + '</a></td>',
                        '<td><a class="remarks">' + dayPayment.remarks + '</a></td>',
                        '<td><input type= "hidden" class ="id" value=' + dayPayment.id + '> <input type= "hidden" class ="accountCode" value=' + dayPayment.account.code + '> <input type= "hidden" class ="bankCode" value=' + dayPayment.bank.code + '> <a  class="editDayPayment" style="text-decoration: underline;color:red" href="javascript:void(0)">  EDIT</a></td>',

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
        $('#report_datatable tbody').on('mouseenter', 'td', function () {

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


    function getFormattedDate(date) {
        var year = date.getFullYear();
        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        return day + '/' + month + '/' + year;
    }


    $('body').on('click', '.editDayPayment', function () {
        $("#editMillRateModel #millRateForm").val("");
        var now = new Date($(this).closest("tr").find(".entryDate").val().trim());
        var day = ("0" + now.getDate()).slice(-2);
        var month = ("0" + (now.getMonth() + 1)).slice(-2);
        var today = now.getFullYear() + "-" + (month) + "-" + (day);
        ///console.log("mySalet"+today);

        $("#millRateForm #bank\\.code").val($(this).closest("tr").find(".bankCode").val()).trigger('change');
        $("#millRateForm #account\\.code").val($(this).closest("tr").find(".accountCode").val()).trigger('change');
        $("#millRateForm #amount").val($(this).closest("tr").find(".amount").text().trim());

        $("#millRateForm #remarks").val($(this).closest("tr").find(".remarks").text().trim());


        $("#millRateForm #id").val($(this).closest("tr").find(".id").val().trim());
        $("#millRateForm #entryDate").val(today);//.trigger('change');

        $("#editMillRateModel").modal("show");

    });
    // CLEAR MILL MODAL FORM
    $("#editMillRateModel").on("click", function () {
        $("#addMillForm").find("input").val("");
        console.log("helo trigger");
        $("#millRateForm #bankAcount\\.code").trigger('reset');
    })
    $("#editMillRateModel #millRateForm").submit(function (event) {
        // Prevent the form from submitting via the browser.
        event.preventDefault();
        var formData = {
            account: {code: $("#millRateForm #account\\.code").val()},
            bank: {code: $("#millRateForm #bank\\.code").val()},

            amount: $("#millRateForm #amount").val(),

            remarks: $("#millRateForm #remarks").val(),
            entryDate: $("#millRateForm #entryDate").val(),
            id: $("#millRateForm #id").val(),


        }
        if ($("#millRateForm #account\\.code").val() == 0 || $("#millRateForm #bankAccount\\.code").val() == 0) {
            alert("select account plz----");
            return;
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
                generateReport();
            }
        });
    });
});