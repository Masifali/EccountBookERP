$(document).ready(function () {

    function getFormData($form) {
        var unindexed_array = $form.serializeArray();
        var indexed_array = {};

        $.map(unindexed_array, function (n, i) {
            indexed_array[n['name']] = n['value'];
        });

        return indexed_array;
    }

    // update serial numbers
    $("table").each(function (i) {
        $(this).find("span#serial_no").each(function (index, element) {
            $(element).text(index + 1);
        });
    });

    // CLEAR ITEM CATEGORY MODAL FORM
    $("#add_item_category").on("hidden.bs.modal", function () {
        $("#addItemCategoryForm").find("input").val("");
        $("#addItemCategoryForm").trigger("reset");
    })

    // UPDATE ITEM CATEGORY
    $(".item_category_edit").on("click", function () {
        // SET ITEM CATEGORY FIELDS
        $("#addItemCategoryForm #id").val($(this).closest("tr").find(".itemCategory_id:first").val());
        $("#addItemCategoryForm #name").val($(this).closest("tr").find(".itemCategory_name:first").val());
        $("#add_item_category").modal("show");
    });

    // CLEAR SUB ITEM CATEGORY MODAL FORM
    $("#add_item_sub_category").on("hidden.bs.modal", function () {
        $("#addItemSubCategoryForm").find("input").val("");
        $("#addItemSubCategoryForm").trigger("reset");
    })

    // UPDATE SUB ITEM CATEGORY
    $(".item_sub_category_edit").on("click", function () {
        // SET SUB ITEM CATEGORY FIELDS
        $("#addItemSubCategoryForm #itemCategory").val($(this).closest("tr").find(".itemCategory_id").val());
        $("#addItemSubCategoryForm #id").val($(this).closest("tr").find(".itemSubCategory_id:first").val());
        $("#addItemSubCategoryForm #name").val($(this).closest("tr").find(".itemSubCategory_name:first").val());
        $("#add_item_sub_category").modal("show");
    });

    // CLEAR ITEM DEF MODAL FORM
    $("#add_item_def").on("hidden.bs.modal", function () {
        $("#stockOpeningForm").trigger("reset");
    })

    // UPDATE ITEM DEF
    $(".item_def_edit").on("click", function () {
        // SET ITEM DEF FIELDS
        $("#stockOpeningForm #itemSubCategory").val($(this).closest("tr").find(".itemSubCategory_id").val());
        $("#stockOpeningForm #id").val($(this).closest("tr").find(".itemDef_id").val());
        $("#stockOpeningForm #formattedCode").val($(this).closest("tr").find(".itemDef_formattedCode").val());
        $("#stockOpeningForm #name").val($(this).closest("tr").find(".itemDef_name").val());
        $("#stockOpeningForm #standardRate").val($(this).closest("tr").find("td:eq(5)").find("input").val().trim());
        $("#stockOpeningForm #weightParBag").val($(this).closest("tr").find("td:eq(6)").text().trim());
//    	$("#stockOpeningForm #weight").val($(this).closest("tr").find("td:eq(7)").text().trim());
//    	$("#stockOpeningForm #feet").val($(this).closest("tr").find("td:eq(8)").text().trim());
        $("#stockOpeningForm #conversionValue").val($(this).closest("tr").find("td:eq(7)").text().trim());
        $("#stockOpeningForm #pricingRule").val($(this).closest("tr").find("td:eq(8)").text().trim());
        $("#stockOpeningForm #saleRate").val($(this).closest("tr").find("td:eq(9)").text().trim());
        $("#stockOpeningForm #itemExpAccounts").val($(this).closest("tr").find("td:eq(10)").text().trim());
        $("#stockOpeningForm #sameExpAccounts").val($(this).closest("tr").find("td:eq(11)").text().trim());
        $("#add_item_def").modal("show");
    });

    // GET NEW ITEM DEF MODAL FORM
    $("#itemSubCategory").on("change", function () {
        $.get("/stocks/get_new_item_code?itemSubCategoryId=" + $("#itemSubCategory").val(), function (data) {
            $("#formattedCode").val(data);
        });
    });

    // SUBMIT FORM
    $("#stockOpeningForm").submit(function (event) {
        if ($("#itemSubCategory").val() < 1) {
            // Prevent the form from submitting via the browser.
            event.preventDefault();
            $.confirm({
                title: "ENCOUNTERED AN ERROR!",
                content: "PLEASE SELECT ITEM CATEGORY TO PROCEED",
                type: 'red',
                typeAnimated: true,
            });
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


    if ($("#item_sub_category_table").length) {
        var itemSubCategoryTable = $("#item_sub_category_table").DataTable({
            dom: 'Bfrtip',
            bPaginate: false,
            scrollCollapse: true,

            buttons: [
                {
                    extend: 'copy',
                    text: 'COPY',
                    title: null,
                    footer: false,
                },
                {
                    extend: 'excelHtml5',
                    text: 'EXCEL',
                    title: 'ITEM SUB CATEGORY' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    footer: false,
                    exportOptions: {
                        stripNewlines: false
                    }
                },
                {
                    extend: 'pdfHtml5',
                    download: 'open',
                    title: 'ITEM SUB CATEGORY' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    customize: function (doc) {
                        doc.defaultStyle.fontSize = 8;
                        //pageMargins [left, top, right, bottom]
                        doc.pageMargins = [5, 5, 5, 5];
                        doc.styles.tableHeader.fontSize = 8;
                        doc.styles.tableFooter.fontSize = 8;
                        doc.defaultStyle.alignment = 'left';
                        doc.styles.tableHeader.alignment = 'left';
                        doc.styles.tableFooter.alignment = 'left';
                    }
                },
                {
                    extend: 'print',
                    text: 'PRINT',
                    title: '',
                    messageTop: 'ITEM SUB CATEGORY',
                    autoPrint: false,
                    exportOptions: {
                        stripHtml: false,
                        stripNewlines: false,
                        columns: [0, 1, 2],
                    },
                    customize: function (win) {
                        $(win.document.body).find('table').addClass('display').css('font-size', '12px');
                        $(win.document.body).css('background-color', 'WHITE');
                        $(win.document.body).css("color", "DIMGRAY");
                        $(win.document.body).find('h1').css('text-align', 'center');
                        $(win.document.body).find('div:first').css('text-align', 'center');

                    }
                }
            ],
            initComplete: function () {
                this.api().columns([1]).every(function () {
                    var column = this;
                    var select = $('<select><option value="">ALL</option></select>')
                        .appendTo($(column.footer()).empty())
                        .on('change', function () {
                            var val = $.fn.dataTable.util.escapeRegex(
                                $(this).val()
                            );

                            column
                                .search(val ? '^' + val + '$' : '', true, false)
                                .draw();
                        });

                    column.data().unique().sort().each(function (d, j) {
                        select.append('<option value="' + d + '">' + d + '</option>')
                    });
                });
            }
        });

        itemSubCategoryTable.on('draw', function () {
            itemSubCategoryTable.columns().indexes().each(function (idx) {
                var select = $(itemSubCategoryTable.column(idx).footer()).find('select');

                if (select.val() === '') {
                    select
                        .empty()
                        .append('<option value="">ALL</option>');

                    itemSubCategoryTable.column(idx, {search: 'applied'}).data().unique().sort().each(function (d, j) {
                        select.append('<option value="' + d + '">' + d + '</option>');
                    });
                }
            });
        });

        $("#item_sub_category_table tfoot tr").appendTo("#item_sub_category_table thead");
    }


    if ($("#stock_opening_table").length) {
        var stockOpeningTable = $("#stock_opening_table").DataTable({
            dom: 'Bfrtip',
            bPaginate: false,
            scrollCollapse: true,

            buttons: [
                {
                    extend: 'copy',
                    text: 'COPY',
                    title: null,
                    footer: false,
                },
                {
                    extend: 'excelHtml5',
                    text: 'EXCEL',
                    title: 'ITEM DEFINITION' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    footer: false,
                    exportOptions: {
                        stripNewlines: false
                    }
                },
                {
                    extend: 'pdfHtml5',
                    title: 'ITEM DEFINITION' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    customize: function (doc) {
                        doc.defaultStyle.fontSize = 8;
                        //pageMargins [left, top, right, bottom]
                        doc.pageMargins = [5, 5, 5, 5];
                        doc.styles.tableHeader.fontSize = 8;
                        doc.styles.tableFooter.fontSize = 8;
                        doc.defaultStyle.alignment = 'left';
                        doc.styles.tableHeader.alignment = 'left';
                        doc.styles.tableFooter.alignment = 'left';
                    }
                },
                {
                    extend: 'print',
                    text: 'PRINT',
                    title: '',
                    messageTop: 'ITEM DEFINITION',
                    autoPrint: false,
                    exportOptions: {
                        stripHtml: false,
                        stripNewlines: false,
                        columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9],
                    },
                    customize: function (win) {
                        $(win.document.body).find('table').addClass('display').css('font-size', '12px');
                        $(win.document.body).css('background-color', 'WHITE');
                        $(win.document.body).css("color", "DIMGRAY");
                        $(win.document.body).find('h1').css('text-align', 'center');
                        $(win.document.body).find('div:first').css('text-align', 'center');

                    }
                }
            ],
            initComplete: function () {
                this.api().columns([1, 2]).every(function () {
                    var column = this;
                    var select = $('<select><option value="">ALL</option></select>')
                        .appendTo($(column.footer()).empty())
                        .on('change', function () {
                            var val = $.fn.dataTable.util.escapeRegex(
                                $(this).val()
                            );

                            column
                                .search(val ? '^' + val + '$' : '', true, false)
                                .draw();
                        });

                    column.data().unique().sort().each(function (d, j) {
                        select.append('<option value="' + d + '">' + d + '</option>')
                    });
                });
            }
        });


        /*stockOpeningTable.on('draw', function () {
            stockOpeningTable.columns().indexes().each( function ( idx ) {
              var select = $(stockOpeningTable.column( idx ).footer()).find('select');

              if ( select.val() === '' ) {
                select
                  .empty()
                  .append('<option value="">ALL</option>');

                stockOpeningTable.column(idx, {search:'applied'}).data().unique().sort().each( function ( d, j ) {
                  select.append( '<option value="'+d+'">'+d+'</option>' );
                });
              }
            });
          });

        $("#stock_opening_table tfoot tr").appendTo("#stock_opening_table thead");*/
    }

    $("#submitAllRates").click(function () {

        // Will immediately show the confirmation popup
        $.confirm({
            title: "CONFIRMATION REQUIRED",
            content: "ARE YOU SURE YOU WANT TO SAVE ALL THE RATES?",
            buttons: {
                confirm: function () {

                    var entries = {};
                    $("#stock_opening_table tbody tr").each(function () {
                        entries[$(this).find(".itemDef_id").val()] = $(this).find(".standardRate").val();
                    });

                    // DO POST
                    $.ajax({
                        type: "POST",
                        contentType: "application/json",
                        url: "/stocks/update_bulk_standard_rates",
                        data: JSON.stringify(entries),
                        success: function (data) {
                            location.reload();
                        },
                    });

                },
                cancel: function () {
                },
            }
        });

    });


});