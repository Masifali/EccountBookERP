$(document).ready(function () {

    var table = $('#voucher_entry_table').DataTable({
        dom: 'Bfrtip',
        buttons: [
            {
                extend: 'print',
                text: 'PRINT',
                title: '',
                messageTop: $(".x_panel:eq(0)").html() + "<br />",
                messageBottom: $("#voucher_footer").html(),
                footer: true,
                autoPrint: false,
                exportOptions: {
                    stripHtml: false,
                    stripNewlines: false,
                    columns: [0, 1, 2, 3, 5, 6, 7],
                },
                customize: function (win) {
                    $(win.document.body).find('table:eq(2) tbody tr td,th').addClass('display').css('padding', '1px 2px 1px 2px');
                    $(win.document.body).find('table:eq(2) tbody tr td,th').addClass('display').css('vertical-align', 'middle');
                    $(win.document.body).find('table').addClass('display').css('font-size', '14px');
                    $(win.document.body).css('background-color', 'WHITE');
                    $(win.document.body).css("color", "DIMGRAY");
                    /*$(win.document.body).find('h1').css('text-align', 'center');
                    $(win.document.body).find('div:first').css('text-align', 'center');*/
                }
            },
            {
                extend: 'print',
                text: 'PRINT(1)',
                title: '',
                messageTop: $(".x_panel:eq(0)").html() + "<br />",
                messageBottom: $("#voucher_footer").html(),
                footer: true,
                autoPrint: false,
                exportOptions: {
                    stripHtml: false,
                    stripNewlines: false,
                    columns: [0, 2, 3, 5, 6, 7],
                },
                customize: function (win) {
                    $(win.document.body).find('table:eq(2) tbody tr td,th').addClass('display').css('padding', '1px 2px 1px 2px');
                    $(win.document.body).find('table:eq(2) tbody tr td,th').addClass('display').css('vertical-align', 'middle');
                    $(win.document.body).find('table').addClass('display').css('font-size', '14px');
                    $(win.document.body).css('background-color', 'WHITE');
                    $(win.document.body).css("color", "DIMGRAY");
                    /*$(win.document.body).find('h1').css('text-align', 'center');
                    $(win.document.body).find('div:first').css('text-align', 'center');*/
                }
            }
        ],
        bPaginate: false,
        bFilter: false,
        bInfo: false,
    });

    //highlight
    $('#voucher_entry_table tbody').on('mouseenter', 'td', function () {
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


    $("#post_voucher").confirm({
        text: "ARE YOU SURE YOU WANT TO " + $("#post_voucher").text() + "?",
        title: "CONFIRMATION REQUIRED",
        confirm: function (button) {
            postThisVoucher();
        },
        cancel: function (button) {
            // nothing to do
        },
        confirmButton: "YES",
        cancelButton: "NO",
        post: true,
        confirmButtonClass: "btn-danger",
        cancelButtonClass: "btn-default",
        dialogClass: "modal-dialog modal-md" // Bootstrap classes for large modal
    });

    function postThisVoucher() {
        voucherId = $("#voucher_id").val();
        $.get("/vouchers/post_voucher?voucherId=" + voucherId, function (data) {
            location.reload();
            /*if (data == "P"){
                $("#edit_voucher").attr("disabled", true);
                $("#post_voucher").text("UN-POST THIS VOUCHER");
                $("#voucher_posted_status").val("true");
            }
            else{
                $("#edit_voucher").attr("disabled", false);
                $("#post_voucher").text("POST THIS VOUCHER");
                $("#voucher_posted_status").val("false");
            }*/

        });
    }

    $(".allocate_invoices").on("click", function () {
        allocatePaymentToMultipleInvoices($(this));
    });

    $("#invoice_entry_table").on("input", "#invoice_payment", function () {

        var formatter = new Intl.NumberFormat("ur-PK", {minimumFractionDigits: 2, maximumFractionDigits: 2});
        $(this).closest("tr").find("#invoice_remaining_amount_due").text(formatter.format(parseFloat($(this).closest("tr").find("#invoice_amount_due").text().replace(/[^0-9\.-]+/g, "")) - parseFloat($(this).closest("tr").find("#invoice_payment").val().replace(/[^0-9\.-]+/g, ""))));
        calculateInvoicePaymentsTotal();
    });

    function allocatePaymentToMultipleInvoices(thisControl) {

        // clear table rows
        $("#invoice_entry_table tbody tr").remove();
        calculateInvoicePaymentsTotal();

        if (!$(thisControl).closest("tr").find("#voucherEntry_credit").text() || $(thisControl).closest("tr").find("#voucherEntry_credit").text() == 0 || !$("#company").attr("value")) {
            return;
        }

        var companyId = $("#company").attr("value");
        var voucherEntryId = $(thisControl).closest("tr").find("#voucherEntry_id").val();
        var accountCode = $(thisControl).closest("tr").find("#voucherEntry_account").attr("value");
        var amountToAllcoate = $(thisControl).closest("tr").find("#voucherEntry_credit").text().replace(/[^0-9\.-]+/g, "");

        $("#voucherEntry_id_for_invoice").val(voucherEntryId);
        $("#voucherEntry_amount_for_invoice").val(amountToAllcoate);
        $("#voucherEntry_account_name_for_invoice").val($(thisControl).closest("tr").find("#voucherEntry_account").text());

        $.get("/vouchers/account_invoices?companyId=" + companyId + "&voucherEntryId=" + voucherEntryId + "&accountCode=" + accountCode + "&amountToAllcoate=" + amountToAllcoate, function (data) {

            var formatter = new Intl.NumberFormat("ur-PK", {minimumFractionDigits: 2, maximumFractionDigits: 2});

            // FILL TABLE ROWS
            for (var i = 0, len = data.length; i < len; i++) {

                var inputField = "<td><input class='form-control' id='invoice_payment' required step='any' type='number' value=" + data[i].invoicePaymentAmount + "></td>";
                if ($("#voucher_posted_status").val() == true || $("#voucher_posted_status").val() == "true") {
                    inputField = "<td><input class='form-control' id='invoice_payment' required step='any' type='number' value=" + data[i].invoicePaymentAmount + " disabled=" + $("#voucher_posted_status").val() + "></td>"
                }

                $("#invoice_entry_table tbody").append(
                    "<tr>"
                    + "<td><span id='serial_no'>" + (i + 1) + "</span><input id='invoice_id' type='hidden' value=" + data[i].invoiceId + "></td>"
                    + "<td><span id='invoice_date'>" + new Date(data[i].voucherDate).toLocaleDateString() + "</span></td>"
                    + "<td><span id='invoice_code'>" + data[i].invoiceCode + "</span></td>"
                    + "<td><span id='external_invoice_code' style='text-transform:uppercase'>" + data[i].externalInvoiceCode + "</span></td>"
                    + "<td><span id='invoice_amount'>" + formatter.format(data[i].invoiceAmount) + "</span></td>"
                    + "<td><span id='invoice_amount_due'>" + formatter.format(data[i].invoiceAmountDue) + "</span></td>"
                    + inputField
                    + "<td><span id='invoice_remaining_amount_due'>" + formatter.format(data[i].invoiceAmountDue - data[i].invoicePaymentAmount) + "</span></td>"
                    + "</tr>");
            }

            calculateInvoicePaymentsTotal();

        });
    }

    function calculateInvoicePaymentsTotal() {

        var formatter = new Intl.NumberFormat("ur-PK", {minimumFractionDigits: 2, maximumFractionDigits: 2});

        var amountTotal = parseFloat(0);
        var amountDueTotal = parseFloat(0);
        var paymentTotal = parseFloat(0);
        var remAmountDueTotal = parseFloat(0);

        $("#invoice_entry_table tr").each(function () {
            if ($(this).find("td").length) {

                amountTotal = amountTotal + parseFloat($(this).find("#invoice_amount").text().replace(/[^0-9\.-]+/g, "") || 0);
                amountDueTotal = amountDueTotal + parseFloat($(this).find("#invoice_amount_due").text().replace(/[^0-9\.-]+/g, "") || 0);
                paymentTotal = paymentTotal + parseFloat($(this).find("#invoice_payment").val().replace(/[^0-9\.-]+/g, "") || 0);
                remAmountDueTotal = remAmountDueTotal + parseFloat($(this).find("#invoice_remaining_amount_due").text().replace(/[^0-9\.-]+/g, "") || 0);
            }
        });

        $("#invoice_entry_table span#amountTotal").text(formatter.format(amountTotal));
        $("#invoice_entry_table span#amountDueTotal").text(formatter.format(amountDueTotal));
        $("#invoice_entry_table span#paymentTotal").text(formatter.format(paymentTotal));
        $("#invoice_entry_table span#remAmountDueTotal").text(formatter.format(remAmountDueTotal));

        $("#applyPaymentToInvoices #account_name_label").text($("#voucherEntry_account_name_for_invoice").val());
        $("#applyPaymentToInvoices #voucher_entry_amount_label").text(formatter.format(parseFloat($("#voucherEntry_amount_for_invoice").val())));
        $("#applyPaymentToInvoices #total_un_allocated_amount_label").text(formatter.format(parseFloat($("#voucherEntry_amount_for_invoice").val()) - paymentTotal));
        $("#applyPaymentToInvoices #over_payment_label").text(formatter.format(amountDueTotal - parseFloat($("#voucherEntry_amount_for_invoice").val())));
    }

    // SUBMIT FORM
    $("#applyPaymentToInvoices").submit(function (event) {
        // Prevent the form from submitting via the browser.
        event.preventDefault();
        savePaymentToMultipleInvoices();
    });

    function savePaymentToMultipleInvoices() {

        var invoicePayments = [];

        if (parseFloat($("#invoice_entry_table span#paymentTotal").text().replace(/[^0-9\.-]+/g, "")) > parseFloat($("#voucherEntry_amount_for_invoice").val().replace(/[^0-9\.-]+/g, ""))) {
            var msg = "TOTAL ALLOCATED AMOUNT CANNOT BE GREATER THAN VOUCHER ENTRY AMOUNT";
            $("#msgs").html("<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert'>&times;</a>" + msg + "</div>");
            return;
        }

        for (var i = 0, len = $("#invoice_entry_table tbody tr").length; i < len; i++) {

            tr = $("#invoice_entry_table tbody tr")[i];

            if (parseFloat($(tr).find("#invoice_payment").val().replace(/[^0-9\.-]+/g, "")) > parseFloat($(tr).find("#invoice_amount_due").text().replace(/[^0-9\.-]+/g, ""))) {
                var msg = "INVOICE ALLOCATION AMOUNT CANNOT BE GREATER THAN INOVICE DUE AMOUNT";
                $("#msgs").html("<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert'>&times;</a>" + msg + "</div>");
                return;
            }

            if ($(tr).find("td").length) {

                var invoicePayment = {
                    voucherEntryId: $(tr).closest("form").find("#voucherEntry_id_for_invoice").val(),
                    invoice: {id: $(tr).find("#invoice_id").val()},
                    paidAmount: $(tr).find("#invoice_payment").val(),
                }

                invoicePayments.push(invoicePayment);
            }
        }

        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "/vouchers/account_invoices",
            data: JSON.stringify(invoicePayments),
            dataType: "json",
            success: function () {
                $("#apply_payment_to_invoices").modal("toggle");
                // UPDATE FORM DATA
                //$("#accountDataForm #code").val(data.accountData.code);

                // SUCCESS MESSAGE
                //window.location = data.redirectUrl;
                //$("#modal_success").modal("show");
            },
            complete: function () {
                $("#apply_payment_to_invoices").modal("toggle");
            },
        });
    }

    // VOUCHER ENTRY SUPPORTING DOCUMENTS
    $(".upload_supporting_docs").on("click", function () {
        $("#voucherEntryDocs #msgs .alert").alert("close");
        // SET VOUCHER ENTRY ID
        $("#voucherEntryDocs #voucherEntry_id_for_doc").val($(this).closest("tr").find("#voucherEntry_id").val());
        getDocumentsOfVoucherEntry();
    });

    function getDocumentsOfVoucherEntry() {
        console.log("ass");
        // clear data
        $("#voucher_entry_doc_table tbody tr").remove();
        $("#voucherEntryDocs #file").val("");
        $("#voucherEntryDocs #description").val("");

        var voucherEntryId = $("#voucherEntryDocs #voucherEntry_id_for_doc").val();

        $.get("/vouchers/voucher_entry_doc?voucherEntryId=" + voucherEntryId, function (data) {

            // FILL TABLE ROWS

            for (var i = 0, len = data.length; i < len; i++) {
                $("#voucher_entry_doc_table tbody").append(
                    "<tr>"
                    + "<td><span id='serial_no'>" + (i + 1) + "</span><input id='doc_id' type='hidden' value=" + data[i].id + "></td>"
                    + "<td><span id='file_name'>" + data[i].name + "</span></td>"
                    /*+"<td><span id='type'>"+data[i].type+"</span></td>"*/
                    + "<td><span id='desc'>" + data[i].description + "</span></td>"
                    + "<td><a style='text-decoration: underline;' id='download_doc' target=_blank href=" + location.protocol + "//" + location.host + "/vouchers/voucher_entry_doc/download?docId=" + data[i].id + ">DOWNLOAD</a></td>"
                    + "<td><a style='text-decoration: underline;' href='#' id='delete_doc'>DELETE</a></td>"
                    + "</tr>");
            }
        });
    }

    // SUBMIT FORM
    $("#voucherEntryDocs").submit(function (event) {
        // Prevent the form from submitting via the browser.
        console.log("voucherEntryDocs");
        event.preventDefault();
        saveVoucherEntryDocument();
    });

    function saveVoucherEntryDocument() {

        if ($("#voucher_posted_status").val() == true || $("#voucher_posted_status").val() == "true") {
            return;
        }

        $("#voucherEntryDocs #msgs .alert").alert("close");
        // Get form
        var formData = new FormData();
        formData.append("file", $("#voucherEntryDocs input[type=file]")[0].files[0]);
        formData.append("description", $("#voucherEntryDocs #description").val().toUpperCase());
        formData.append("voucherEntryId", $("#voucherEntryDocs #voucherEntry_id_for_doc").val());

        // DO POST
        $.ajax({
            type: "POST",
            enctype: "multipart/form-data",
            contentType: "application/json",
            url: "/vouchers/voucher_entry_doc",
            data: formData,
            processData: false, //prevent jQuery from automatically transforming the data into a query string
            contentType: false,
            cache: false,
            success: function () {
                var msg = "DOCUMENT HAS BEEN UPLOADED SUCCESSFULLY..!!";
                $("#voucherEntryDocs #msgs").html("<div class='alert alert-success'><a href='#' class='close' data-dismiss='alert'>&times;</a>" + msg + "</div>");
                // SHOW UPDATED ROWS
                getDocumentsOfVoucherEntry();
            },
            complete: function () {
                //$("#upload_supporting_docs").modal("toggle");
            },
        });
    }

    $("#voucher_entry_doc_table").on('click', 'a#delete_doc', function () {
        deleteVoucherEntryDocument($(this))
    });

    function deleteVoucherEntryDocument(thisControl) {

        if ($("#voucher_posted_status").val() == true || $("#voucher_posted_status").val() == "true") {
            var msg = "DOCUMENT CANNOT BE DELETED AS VOUCHER IS IN 'POSTED' STATE";
            $("#voucherEntryDocs #msgs").html("<div class='alert alert-success'><a href='#' class='close' data-dismiss='alert'>&times;</a>" + msg + "</div>");
            return;
        }

        $("#voucherEntryDocs #msgs .alert").alert("close");

        // DO POST
        $.ajax({
            type: "DELETE",
            contentType: "application/json",
            url: "/vouchers/voucher_entry_doc?docId=" + $(thisControl).closest("tr").find("#doc_id").val(),
            data: {_method: "delete"},
            success: function () {
                var msg = "DOCUMENT HAS BEEN DELETED SUCCESSFULLY..!!";
                $("#voucherEntryDocs #msgs").html("<div class='alert alert-success'><a href='#' class='close' data-dismiss='alert'>&times;</a>" + msg + "</div>");
                // SHOW UPDATED ROWS
                getDocumentsOfVoucherEntry();
            },
            complete: function () {
                //$("#upload_supporting_docs").modal("toggle");
            },
        });
    }

    $(document).keydown(function (e) {


        if (e.altKey && e.keyCode == 80)//p
        {
            // console.log("pasorted");
            if ($("#voucher_posted_status").val() == "false") {
                if ($("#voucherPostPr").val() === "true") {
                    postThisVoucher();
                }
            }
            //location.reload();
        }
        if (e.altKey && e.keyCode == 85)//u
        {
            //console.log($("#voucher_posted_status").val());
            if ($("#voucher_posted_status").val() == "true") {
                if ($("#unPoucherPostPr").val() === "true") {
                    postThisVoucher();
                }
            }
            //location.reload();
        }

        if (e.altKey && e.keyCode == 67)//c
        {
            if ($("#editVoucherPr").val() === "true") {
                location.href = location.protocol + "//" + location.host + "/vouchers/edit/" + $("#voucher_id").val();
            }
        }
        if (e.altKey && e.keyCode == 78)// n
        {
            location.href = location.protocol + "//" + location.host + "/vouchers/new_voucher";
            //$("#edit_voucher").click();
        }

    });
});