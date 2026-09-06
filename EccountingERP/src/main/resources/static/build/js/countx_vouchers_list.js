$(document).ready(function () {

    // var intervalId = window.setInterval(function () {
    //     getVoucherList();
    // }, 1);
    //
    // var myInterval111 = setInterval(everyTime, 1000);
    // //clearInterval(intervalId)  To stop the loop you can use:
    // setInterval(function () {
    //     //code goes here that will be run every 5 seconds.
    //     console.log("hi asif ");
    // }, 1);
    $("#voucher_company").prop("selectedIndex", 1);
    $("#voucher_status").prop("selectedIndex", 1);
    $("#search_by_date").prop("selectedIndex", 2);
    $('#from_date').val(new Date().toISOString().substring(0, 10));
    $('#to_date').val(new Date().toISOString().substring(0, 10));

    $("#voucher_company").prop("selectedIndex", 1);
    $("#voucher_status").prop("selectedIndex", 1);
    $("#search_by_date").prop("selectedIndex", 2);
    $('#from_date').val(new Date().toISOString().substring(0, 10));
    $('#to_date').val(new Date().toISOString().substring(0, 10));

    // SUBMIT FORM
    $("#voucherFilterForm").submit(function (event) {

        // Prevent the form from submitting via the browser.
        event.preventDefault();
        getVoucherList();
    });

    function getFormattedDate(date) {
        var year = date.getFullYear();
        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        return day + '/' + month + '/' + year;
    }

    function getVoucherList() {

        $('#voucher_datatable').DataTable().clear();
        $('#voucher_datatable').DataTable().destroy();
        var voucherTable = $('#voucher_datatable').DataTable({
            dom: 'Bfrtip',
            deferRender: true,
//    	  	        scrollY: 500,
//    	  	        scroller: true,
//    	  	        destroy: true,
            fixedHeader: true,
            bPaginate: false,
            buttons: [

                {
                    extend: 'pdfHtml5',
                    title: 'VOUCHER LIST' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    messageTop: 'FROM: ' + getFormattedDate(new Date($("#from_date").val()))
                        + '   ' + 'TO : ' + getFormattedDate(new Date($("#to_date").val())),
                    messageBottom: null,
                    // messageTop: null,
                    download: 'open',
                    pageSize: 'A4',
                    paging: true,
                    footer: true,

                    customize: function (doc) {
                        console.log("aa");
                        doc.defaultStyle.fontSize = 8;
                        doc.pageMargins = [15, 15, 10, 20]; //1st left side ,2th header, 3th right side ,4th buttion
                        doc.styles.tableHeader.fontSize = 8;
                        doc.styles.tableFooter.fontSize = 8;
                        doc.defaultStyle.alignment = 'left';
                        doc.styles.tableHeader.alignment = 'left';
                        doc.styles.tableFooter.alignment = 'left';
                        doc.content[2].layout = {
                            hLineWidth: function (i, node) {
                                return (i === 0 || i === node.table.body.length) ? 2 : 1;
                            },
                            vLineWidth: function (i, node) {
                                return (i === 0 || i === node.table.widths.length) ? 2 : 1;
                            },
                            hLineColor: function (i, node) {
                                return (i === 0 || i === node.table.body.length) ? 'black' : 'gray';
                            },
                            vLineColor: function (i, node) {
                                return (i === 0 || i === node.table.widths.length) ? 'black' : 'gray';
                            }
                        };
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
            ],

        });
        // disable submit button
        $("#getVoucherList").attr("disabled", true);

        // PREPARE FORM DATA
        var formData = {
            company: {id: $("#voucherFilterForm #voucher_company").val(), name: ""},
            voucherType: {id: $("#voucherFilterForm #voucher_type").val(), name: ""},
            voucherStatus: {id: $("#voucherFilterForm #voucher_status").val(), name: ""},
            voucherCode: $("#voucherFilterForm #voucher_code").val(),
            inventoryVoucherType: $("#voucherFilterForm #inventory_voucher_type").val(),
            voucherNarration: $("#voucherFilterForm #voucher_narration").val(),
            searchByDate: $("#voucherFilterForm #search_by_date").val(),
            fromDate: $("#voucherFilterForm #from_date").val(),
            toDate: $("#voucherFilterForm #to_date").val(),
            postedUnPosted: $("#voucherFilterForm #voucher_posted").val(),
        }

        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: $("#voucherFilterForm").attr("action"),
            data: JSON.stringify(formData),
            dataType: "json",
            success: function (data) {
                // FILL TABLE ROWS

                voucherTable.clear().draw();

                $.each(data.voucherDetailList, function (i, voucher) {

                    var formatter = new Intl.NumberFormat('ur-PK', {
                        minimumFractionDigits: 2,
                        maximumFractionDigits: 2
                    });

                    var voucherViewLink;
                    if (voucher.voucherCode.indexOf("STV") >= 0) {
                        voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/stock_transfer_vouchers/" + voucher.id + ">VIEW</a>";
                    }
                    if (voucher.voucherCode.indexOf("PJV") >= 0) {
                        voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/purchase_journal_vouchers/" + voucher.id + ">VIEW</a>";
                    }
                    if (voucher.voucherCode.indexOf("PTV") >= 0) {
                        voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/purchase_journal_vouchers/" + voucher.id + ">VIEW</a>";
                    } else if (voucher.voucherCode.indexOf("PRV") >= 0) {
                        voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/purchase_return_vouchers/" + voucher.id + ">VIEW</a>";
                    } else if (voucher.voucherCode.indexOf("SJV") >= 0) {
                        voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/sale_journal_vouchers/" + voucher.id + ">VIEW</a>";
                    } else if (voucher.voucherCode.indexOf("STV") >= 0) {
                        voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/sale_journal_vouchers/" + voucher.id + ">VIEW</a>";
                    } else if (voucher.voucherCode.indexOf("PQV") >= 0) {
                        voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/purchase_journal_voucher_qty/" + voucher.id + ">VIEW</a>";
                    } else if (voucher.voucherCode.indexOf("SRV") >= 0) {
                        voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/sale_return_vouchers/" + voucher.id + ">VIEW</a>";
                    } else {
                        voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/vouchers/" + voucher.id + ">VIEW</a>";
                    }

                    voucherTable.row.add([
                        (i + 1),
                        voucher.voucherCode,
                        getFormattedDate(new Date(voucher.voucherDate)),
                        getFormattedDate(new Date(voucher.createdAt)) + ", " + new Date(voucher.createdAt).toLocaleString().split(",")[1].trim(),
                        getFormattedDate(new Date(voucher.updatedAt)) + ", " + new Date(voucher.updatedAt).toLocaleString().split(",")[1].trim(),
                        formatter.format(voucher.voucherAmount),
                        voucher.userName,
                        voucher.postedUnPosted,
                        voucherViewLink,
                    ]);
                });

                voucherTable.draw();
                // enable submit button
                $("#getVoucherList").attr("disabled", false);
            },
            complete: function () {
                // enable submit button
                /*$("#getVoucherList").attr("disabled", false);
                $("#voucher_datatable").DataTable( {
                    destroy: true,
                    initComplete: function () {
                        this.api().columns([1, 2, 3, 4, 5, 6, 7]).every( function () {
                            var column = this;
                            var select = $('<select><option value="">SHOW ALL</option></select>')
                                .appendTo( $(column.footer()).empty() )
                                .on( 'change', function () {
                                    var val = $.fn.dataTable.util.escapeRegex(
                                        $(this).val()
                                    );

                                    column
                                        .search( val ? '^'+val+'$' : '', true, false )
                                        .draw();
                                } );

                            column.data().unique().sort().each( function ( d, j ) {
                                select.append( '<option value="'+d+'">'+d+'</option>' )
                            } );
                        } );
                    }
                } );
                $("#voucher_datatable tfoot tr").appendTo("#voucher_datatable thead");*/

            },
        });


        //highlight
        $('#voucher_datatable tbody').on('mouseenter', 'td', function () {
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

    }


})