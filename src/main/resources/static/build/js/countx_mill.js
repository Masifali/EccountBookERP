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

    // CLEAR MILL MODAL FORM
    $("#add_mill").on("hidden.bs.modal", function () {
        $("#addMillForm").find("input").val("");
        $("#addMillForm").trigger("reset");
    })

    // UPDATE MILL
    $(".mill_edit").on("click", function () {
        // SET ITEM CATEGORY FIELDS
        $("#addMillForm #id").val($(this).closest("tr").find(".mill_id:first").val());
        $("#addMillForm #accountName").val($(this).closest("tr").find(".mill_name:first").val());
        $("#add_mill").modal("show");
    });
    // CLEAR Mill khata MODAL FORM
    $("#add_mill_khata").on("hidden.bs.modal", function () {
        $("#addMillKhataForm").find("input").val("");
        $("#addMillKhataForm").trigger("reset");
    })

    // UPDATE Mill Khata
    $(".mill_khata_edit").on("click", function () {
        console.log("aa");

        // SET Mill Khata FIELDS
        $("#addMillKhataForm #mill\\.id").val($(this).closest("tr").find(".mill_id").val());
        $("#addMillKhataForm #millKhata\\.id").val($(this).closest("tr").find(".millKhata_id").val());
        $("#addMillKhataForm #id").val($(this).closest("tr").find(".millKhata_id:first").val());
        $("#addMillKhataForm #accountName").val($(this).closest("tr").find(".millKhata_name:first").val());
        $("#addMillKhataForm #displayName").val($(this).closest("tr").find(".millKhata_displayName:first").val());
        $("#addMillKhataForm #accountCode\\.code").val($(this).closest("tr").find(".millKhata_account:first").val()).trigger('change');
        $("#add_mill_khata_model").modal("show");
    });

    function getFormattedDate(date) {
        var year = date.getFullYear();
        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        return day + '/' + month + '/' + year;
    }

    $("#add_mill_khata_model #addMillKhataForm").submit(function (event) {

        var formData = {
            mill: {id: $("#addMillKhataForm #mill\\.id").val()},
            accountCode: {code: $("#addMillKhataForm #accountCode\\.code").val()},
            accountName: $("#addMillKhataForm #accountName").val(),
            displayName: $("#addMillKhataForm #displayName").val(),
            id: $("#addMillKhataForm #id").val(),
        }
        // alert(formData);
        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: $("#addMillKhataForm").attr("action"),
            data: JSON.stringify(formData),
            dataType: "text",
            success: function (data) {
                $('#add_mill_khata_model').modal('toggle');
                // generateGLReport();
            }
        });
    });

    if ($("#mill_khata_table").length) {
        var table = $('#mill_khata_table').DataTable({
            dom: 'Bfrtip',
            deferRender: true,
            scrollY: 500,
            scroller: true,
            destroy: true,
            fixedHeader: true,
            // bPaginate: false,
            buttons: [
                {
                    extend: 'copy',
                    text: 'COPY',
                    title: null,
                    footer: true,
                },
                {
                    extend: 'excelHtml5',
                    text: 'EXCEL',
                    title: 'Mill khate' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    footer: false,
                    exportOptions: {
                        stripNewlines: false
                    }
                },

                {
                    extend: 'pdfHtml5',
                    title: 'MILL KHATE' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
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
                    title: 'MILL KHATA REPORT' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    messageTop: '',
                    messageBottom: null,
                    footer: true,
                    autoPrint: false,

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

        table.on('draw', function () {
            table.columns().indexes().each(function (idx) {
                var select = $(table.column(idx).footer()).find('select');

                if (select.val() === '') {
                    select
                        .empty()
                        .append('<option value="">ALL</option>');

                    table.column(idx, {search: 'applied'}).data().unique().sort().each(function (d, j) {
                        select.append('<option value="' + d + '">' + d + '</option>');
                    });
                }
            });
        });

        $("#mill_khata_table tfoot tr").appendTo("#mill_khata_table thead");
    }
});