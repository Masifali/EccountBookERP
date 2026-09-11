$(document).ready(function () {
    console.log("assa");

    $("#purchasePrintCommission").on("click", function (event) {
        window.open("/reports/printVoucherWithCommission/?voucherId=" + $("#pjvoucher_id").val(), '_blank');

    });

    $("#salesPrintCommission").on("click", function (event) {

        window.open("/reports/printVoucherWithCommission/?voucherId=" + $("#sjvoucher_id").val(), '_blank');

    });

    $("#purchasePortraitPrint").on("click", function (event) {
        window.open("/reports/printVoucherWithCommission/?voucherId=" + $("#pjvoucher_id").val() + "&pageType=" + "a4", '_blank');
    });
    $("#salePortraitPrint").on("click", function (event) {
        window.open("/reports/printVoucherWithCommission/?voucherId=" + $("#sjvoucher_id").val() + "&pageType=" + "a4", '_blank');
    });
    $('#purchase_journal_voucher_entry_table').DataTable({
        dom: 'Bfrtip',
        columnDefs: [
            {
                // "targets": [9],
                // "visible": false,
            }
        ],
        buttons: [
            {
                extend: 'print',
                text: 'PRINT',
                title: '',
                messageTop: $(".x_panel:eq(1)").html() + "<br />",
                messageBottom: $("#po_footer").html(),
                footer: true,
                autoPrint: false,
                exportOptions: {
                    stripHtml: false,
                    stripNewlines: false,
                    columns: [0, 1, 2, 3, 4, 5, 6, 7],
                },
                customize: function (win) {
                    win.pageMargins = [20, 50, 50, 60];
                    //doc.styles.tableHeader.fontSize = 7;
                    //doc.styles.tableFooter.fontSize = 7;
                    $(win.document.body).find('table').addClass('display').css('font-size', '12px');
                    $(win.document.body).css('background-color', 'WHITE');
                    $(win.document.body).css("color", "DIMGRAY");
                    /*$(win.document.body).find('h1').css('text-align', 'center');
                    $(win.document.body).find('div:first').css('text-align', 'center');*/

                }
            },

            {
                extend: 'print',
                text: 'PRINT DELIVERY ORDER',
                title: $("#bName").val(),
                messageTop: $(".x_panel:eq(1)").html() + "<br />",
                messageBottom: $("#po_footer").html(),
                footer: true,
                autoPrint: false,
                exportOptions: {
                    stripHtml: false,
                    stripNewlines: false,
                    //columns: [ 0, 1, 2, 3,4, 5,6,7],
                },
                customize: function (win) {
                    $(win.document.body).find('table:eq(1) tbody tr td,th').addClass('display').css('padding', '1px 2px 1px 2px');
                    $(win.document.body).find('table:eq(1) tbody tr td,th').addClass('display').css('vertical-align', 'middle');
                    $(win.document.body).find('table').addClass('display').css('font-size', '16px');
                    $(win.document.body).css('background-color', 'WHITE');
                    $(win.document.body).css("color", "DIMGRAY");
                    $(win.document.body).find('h1').css('text-align', 'center');
                    $(win.document.body).find('div:first').css('text-align', 'center');

                }
            },

            {
                extend: 'print',
                text: 'PRINT INVOICE',
                title: 'asif',
                messageTop: '' + "<br />",
                messageBottom: 'Idreess',
                footer: true,
                autoPrint: false,
                exportOptions: {
                    stripHtml: false,
                    stripNewlines: false,
                    //columns: [ 0, 1, 2, 3,4, 5,6,7],
                },
                customize: function (win) {
                    $(win.document.body).find('table:eq(1) tbody tr td,th').addClass('display').css('padding', '1px 2px 1px 2px');
                    $(win.document.body).find('table:eq(1) tbody tr td,th').addClass('display').css('vertical-align', 'middle');
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
    $('#Invoice_pdf').DataTable({
        dom: 'Bfrtip',
        columnDefs: [
            {
                // "targets": [9],
                // "visible": false,
            }
        ],
        buttons: [

            {
                extend: 'pdfHtml5',
                text: 'OPEN PDF',
                download: 'open',
                title: $("#branchName").val() + '\n      ' + '\n Proprietor :' + $("#proprietorName").val() + ' Phone No :' + $("#proprietorPhone").val() + '\n' + '  NTN # :' + $("#ntnNumber").val() + '\n' + 'Address: ' + $("#address").val(),
                messageTop: 'Invoice#:' + $("#voucherCode").val() + '           Ref No: ' + $("#bookNumber").val() + '            Date    :  ' + getFormattedDate(new Date($("#voucherDate").val())) + '\n\n ' + 'Party Name :  ' + $("#partyName").val() + '\n\n ' + 'Product Name:  ' + $("#productName").val(),
                messageBottom: '\n\n\n Signature:',
                footer: true,
                orientation: 'landscape',
                pageSize: 'A4',

                exportOptions: {
                    columns: [0, 1, 5, 6, 7],
                    stripNewlines: true,
                },
                customize: function (doc) {
                    doc.defaultStyle.fontSize = 10;
                    doc.pageMargins = [450, 10, 10, 10];
                    doc.styles.tableHeader.fontSize = 12;
                    doc.styles.tableFooter.fontSize = 10;
                    doc.styles.tableBodyEven.alignment = 'right';
                    doc.styles.tableBodyOdd.alignment = 'right';
                    doc.styles.tableHeader.alignment = 'right';
                    doc.styles.tableFooter.alignment = 'left';
                    doc.content[0].text = doc.content[0].text.trim();
                }
            },
            {
                extend: 'pdfHtml5',
                text: 'Download PDF',
                download: '',
                title: $("#branchName").val() + '\n      ' + '\n Proprietor :' + $("#proprietorName").val() + ' Phone No :' + $("#proprietorPhone").val() + '\n' + '  NTN # :' + $("#ntnNumber").val() + '\n' + 'Address: ' + $("#address").val(),
                messageTop: 'Invoice#:' + $("#voucherCode").val() + '           Ref No: ' + $("#bookNumber").val() + '            Date    :  ' + getFormattedDate(new Date($("#voucherDate").val())) + '\n\n ' + 'Party Name :  ' + $("#partyName").val() + '\n\n ' + 'Product Name:  ' + $("#productName").val(),
                messageBottom: '\n\n\n Signature:',
                footer: true,
                orientation: 'landscape',
                pageSize: 'A4',

                exportOptions: {
                    columns: [0, 1, 5, 6, 7],
                    stripNewlines: true,
                },
                customize: function (doc) {
                    doc.defaultStyle.fontSize = 10;
                    doc.pageMargins = [450, 10, 10, 10];
                    doc.styles.tableHeader.fontSize = 12;
                    doc.styles.tableFooter.fontSize = 10;
                    //doc.defaultStyle.alignment = 'right';
                    doc.styles.tableBodyEven.alignment = 'right';
                    doc.styles.tableBodyOdd.alignment = 'right';
                    doc.styles.tableHeader.alignment = 'right';
                    doc.styles.tableFooter.alignment = 'left';
                    doc.content[0].text = doc.content[0].text.trim();
                }
            },
        ],
        bPaginate: false,
        bFilter: false,
        bInfo: false,
    });


    $('#Invoices_pdf').DataTable({
        dom: 'Bfrtip',
        columnDefs: [
            {
                // "targets": [9],
                // "visible": false,
            }
        ],
        buttons: [

            {
                extend: 'pdfHtml5',
                text: 'OPEN PDF',
                download: 'open',
                title: $("#branchName").val() + '\n      ' + '\n Proprietor :' + $("#proprietorName").val() + ' Phone No :' + $("#proprietorPhone").val() + '\n' + '  NTN # :' + $("#ntnNumber").val() + '\n' + 'Address: ' + $("#address").val(),
                messageTop: 'Invoice#:' + $("#voucherCode").val() + '           Ref No: ' + $("#bookNumber").val() + '            Date    :  ' + getFormattedDate(new Date($("#voucherDate").val())) + '\n\n ' + 'Party Name :  ' + $("#spartyName").val() + '\n\n ' + 'Product Name:  ' + $("#productName").val(),
                messageBottom: '\n\n\n Signature:',
                footer: true,
                orientation: 'landscape',
                pageSize: 'A4',

                exportOptions: {
                    columns: [0, 1, 5, 6, 7],
                    stripNewlines: true,
                },
                customize: function (doc) {
                    doc.defaultStyle.fontSize = 10;
                    doc.pageMargins = [450, 10, 10, 10];
                    doc.styles.tableHeader.fontSize = 12;
                    doc.styles.tableFooter.fontSize = 10;
                    //doc.defaultStyle.alignment = 'right';
                    doc.styles.tableBodyEven.alignment = 'right';
                    doc.styles.tableBodyOdd.alignment = 'right';
                    doc.styles.tableHeader.alignment = 'right';
                    doc.styles.tableFooter.alignment = 'left';
                    doc.content[0].text = doc.content[0].text.trim();
                }
            },
            {
                extend: 'pdfHtml5',
                text: 'Download PDF',
                download: '',
                title: $("#branchName").val() + '\n      ' + '\n Proprietor :' + $("#proprietorName").val() + ' Phone No :' + $("#proprietorPhone").val() + '\n' + '  NTN # :' + $("#ntnNumber").val() + '\n' + 'Address: ' + $("#address").val(),
                messageTop: 'Invoice#:' + $("#voucherCode").val() + '           Ref No: ' + $("#bookNumber").val() + '            Date    :  ' + getFormattedDate(new Date($("#voucherDate").val())) + '\n\n ' + 'Party Name :  ' + $("#spartyName").val() + '\n\n ' + 'Product Name:  ' + $("#productName").val(),
                messageBottom: '\n\n\n Signature:',
                footer: true,
                orientation: 'landscape',
                pageSize: 'A4',

                exportOptions: {
                    columns: [0, 1, 5, 6, 7],
                    stripNewlines: true,
                },
                customize: function (doc) {
                    doc.defaultStyle.fontSize = 10;
                    doc.pageMargins = [450, 10, 10, 10];
                    doc.styles.tableHeader.fontSize = 12;
                    doc.styles.tableFooter.fontSize = 10;
                    doc.styles.tableBodyEven.alignment = 'right';
                    doc.styles.tableBodyOdd.alignment = 'right';
                    doc.styles.tableHeader.alignment = 'right';
                    doc.styles.tableFooter.alignment = 'left';
                    doc.content[0].text = doc.content[0].text.trim();
                }
            },
        ],
        bPaginate: false,
        bFilter: false,
        bInfo: false,
    });
    console.log("asif");
    $("#post_voucher").on("click", function (event) {
        // Prevent the form from submitting via the browser.
        event.preventDefault();
        // Will immediately show the confirmation popup
        $.confirm({
            title: "CONFIRMATION REQUIRED",
            content: "ARE YOU SURE YOU WANT TO " + $("#post_voucher").text() + "?",
            buttons: {
                confirm: function () {
                    postThisVoucher();
                },
                cancel: function () {
                },
            }
        });
    });


    $("#purchasePrint").on("click", function (event) {
        $("#purchasePrint").attr("disabled", true);
        //$("#pac_man").show();
        event.preventDefault();
        //alert($("#pjvoucher_id").val());
        //var purchaseId = $(this).attr('data-id');
        $.get("/reports/voucher/" + $("#pjvoucher_id").val(), function (data) {
            window.location.href = "/reports/download_voucher/" + data;
            $("#purchasePrint").attr("disabled", false);
        });
    });

    $("#salesPrint").on("click", function (event) {
        $("#salesPrint").attr("disabled", true);
        //$("#pac_man").show();
        event.preventDefault();
        //alert($("#svoucher_id").val());
        //var purchaseId = $(this).attr('data-id');
        $.get("/reports/voucher/" + $("#sjvoucher_id").val(), function (data) {
            window.location.href = "/reports/download_voucher/" + data;
            $("#salesPrint").attr("disabled", false);
        });
    });

    function postThisVoucher() {
        voucherId = $("#pjvoucher_id").val();
        $.get("/purchase_journal_vouchers/post_voucher?voucherId=" + voucherId, function (data) {
            location.reload();
        });
    }

    function postThissjVoucher() {
        voucherId = $("#sjvoucher_id").val();
        $.get("/sale_journal_vouchers/post_voucher?voucherId=" + voucherId, function (data) {
            location.reload();
        });
    }

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

    ////////
    function getDocumentsOfVoucherEntry() {

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
    $(".upload_supporting_docs").on("click", function () {

        $("#voucherEntryDocs #msgs .alert").alert("close");
        // SET VOUCHER ENTRY ID
        console.log("click " + $("#pjvoucher_id").val());
        $("#voucherEntryDocs #voucherEntry_id_for_doc").val($("#pjvoucher_id").val());


        getDocumentsOfVoucherEntry();
    });


    $("#voucherEntryDocs").submit(function (event) {
        // Prevent the form from submitting via the browser.
        console.log("voucherEntryDocs");
        event.preventDefault();
        saveVoucherEntryDocument();
        var image = document.getElementById('output');
        image.src = null;

    });

    $("#post_sjvVoucher").click(function () {
        postThissjVoucher();
        /*if($("#sjpostVoucher").val() === "true")
        {
         if($("#sjvoucher_posted_status").val()=="false")
             postThissjVoucher();
        }*/
    });

    $(document).keydown(function (e) {
        //alert($("#upPostVoucher").val() );
        //console.log($("#upPostVoucher").val());
        if (e.altKey && e.keyCode == 80) {
            /*if ($("#postVoucher").val() === "true") {
                if ($("#voucher_posted_status").val() == "false")*/
            postThisVoucher();
            //  }
            //location.reload();
        }
        if (e.altKey && e.keyCode == 81) {
            postThisVoucher();
            //     if ($("#sjpostVoucher").val() === "true") {
            //         if ($("#sjvoucher_posted_status").val() == "false")
            //             postThissjVoucher();
            //     }
            //     //location.reload();
        }

        if ($("#unPostVoucher").val() === "true") {
            if (e.altKey && e.keyCode == 85)//u
            {
                console.log($("#voucher_posted_status").val());
                // if ($("#voucher_posted_status").val() == "true")
                postThisVoucher();
                //location.reload();
                //}
            }

            if ($("#sjeditVoucher").val() === "true") {
                if (e.altKey && e.keyCode == 71)//G
                {
                    location.href = location.protocol + "//" + location.host + "/SaleQty/pjv_sjv_qty/edit/" + $("#sjvoucher_id").val();
                }
            }

            if ($("#editVoucher").val() === "true") {
                if (e.altKey && e.keyCode == 67)//c
                {
                    location.href = location.protocol + "//" + location.host + "/SaleQty/pjv_sjv_qty/edit/" + $("#pjvoucher_id").val();
                }
            }

            if ($("#newPurchaseVoucher").val() === "true") {
                if (e.altKey && e.keyCode == 78)// n
                {
                    location.href = location.protocol + "//" + location.host + "/SaleQty/new_pjv_sjv_qty_form";
                    //$("#edit_voucher").click();
                }
            }


        }
    });

    function getFormattedDate(date) {
        var year = date.getFullYear();
        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        return day + '/' + month + '/' + year;
    }
});