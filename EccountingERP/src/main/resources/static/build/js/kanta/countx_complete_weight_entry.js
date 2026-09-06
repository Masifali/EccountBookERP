$(document).ready(function () {


    // $("#voucher_company").prop("selectedIndex", 1);
    // $("#voucher_status").prop("selectedIndex", 1);
    //$("#search_by_date").prop("selectedIndex", 2);
    $('#from_date').val(new Date().toISOString().substring(0, 10));
    $('#to_date').val(new Date().toISOString().substring(0, 10));

    // SUBMIT FORM
    $("#weightFilterForm").submit(function (event) {

        // Prevent the form from submitting via the browser.
        event.preventDefault();
        getVoucherList();
    });
     $("#edit_entry_model").submit(function (event) {
      var formData = {

            id: $("#updateEntryForm #id").val(),
            deduction: $("#updateEntryForm #deduction").val(),
            location: {id:$("#updateEntryForm #location").val()},
            monshi: {id:$("#updateEntryForm #monshi").val()},
            moisture: $("#updateEntryForm #moisture").val(),
            rate: $("#updateEntryForm #rate").val(),


        }
            // Prevent the form from submitting via the browser.
            event.preventDefault();
             $.ajax({
                        type: "POST",
                        contentType: "application/json",
                        url: $("#updateEntryForm").attr("action"),
                        data: JSON.stringify(formData),
                        dataType: "text",
                        success: function (data) {
                        console.log("asiffddf");
                         $('#edit_entry_model').modal('toggle');
                      getVoucherList();
        }
        })
         })
    // UPDATE WEIGHT ENTRY
     $('body').on('click', '.weight_entry_edit', function ()  {
        // SET WEIGHT ENTRY FIELDS

        $("#updateEntryForm #id").val($(this).closest("tr").find(".id").val());
        $("#updateEntryForm #sukai").val($(this).closest("tr").find(".sukai").val());
        $("#updateEntryForm #location").val($(this).closest("tr").find(".location").val());
        $("#updateEntryForm #moisture").val($(this).closest("tr").find(".moisture").val() );
        $("#updateEntryForm #rate").val($(this).closest("tr").find(".rate").val() );
        $("#updateEntryForm #monshi").val($(this).closest("tr").find(".monshi").val());
        $("#edit_entry_model").modal("show");
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

        $('#weight_datatable').DataTable().clear();
        $('#weight_datatable').DataTable().destroy();
        var voucherTable = $('#weight_datatable').DataTable({
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
                    title: 'Daily Statement For Paid Amount' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
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
            "footerCallback": function (row, data, start, end, display) {
                var api = this.api(), data;

                var colNumber = [9];

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
                    var total = api
                        .column(colNo)
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);
                    $(api.column(colNo).footer()).html(parseFloat(total).toFixed(0));
                }
            }

        });
        // disable submit button
        $("#getWeightList").attr("disabled", true);

        // PREPARE FORM DATA
        var formData = {
            //  company: {id: $("#weightFilterForm #voucher_company").val(), name: ""},
            supplierAccount: {id: $("#weightFilterForm #supplierAccount").val(), name: ""},
            itemDef: {id: $("#weightFilterForm #itemDef").val(), name: ""},
            //     searchByDate: $("#weightFilterForm #search_by_date").val(),
            fromDate: $("#weightFilterForm #from_date").val(),
            toDate: $("#weightFilterForm #to_date").val(),

        }

        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: $("#weightFilterForm").attr("action"),
            data: JSON.stringify(formData),
            dataType: "json",
            success: function (data) {
                // FILL TABLE ROWS

                voucherTable.clear().draw();

                $.each(data, function (i, weightEntry) {
                    var supplierName = "";
                    if (weightEntry.supplierAccount != null) {
                        supplierName = weightEntry.supplierAccount.accountName;
                    }
                    voucherTable.row.add([
                        (i + 1),

                        weightEntry.vehicalNo,
                        supplierName,
                        weightEntry.number,
                        getFormattedDate(new Date(weightEntry.firstDate)) + ", " + weightEntry.firstTime,
                        getFormattedDate(new Date(weightEntry.secondDate)) + ", " + weightEntry.secondTime,
                        weightEntry.firstWeight,
                        weightEntry.secondWeight,
                        parseFloat(weightEntry.netWeight),
                        weightEntry.amount,
                        weightEntry.createdBy,
                        '<input type= "hidden" class ="deduction" value=' + weightEntry.deduction + '><input type= "hidden" class ="rate" value=' + weightEntry.rate + '><input type= "hidden" class ="location" value=' + weightEntry.location.id + '><input type= "hidden" class ="moisture" value=' + weightEntry.moisture + '><input type= "hidden" class ="id" value=' + weightEntry.id + '> <input type= "hidden" class ="monshi" value=' + weightEntry.monshi.id + '><a class="weight_entry_edit" style="text-decoration: underline;color:green" href="javascript:void(0)">EDIT</a>'

                    ]);
                });

                voucherTable.draw();
                // enable submit button
                $("#getWeightList").attr("disabled", false);
            },
            complete: function () {
                // enable submit button
                /*$("#getWeightList").attr("disabled", false);
                $("#weight_datatable").DataTable( {
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
                $("#weight_datatable tfoot tr").appendTo("#weight_datatable thead");*/

            },
        });


        //highlight
        $('#weight_datatable tbody').on('mouseenter', 'td', function () {
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