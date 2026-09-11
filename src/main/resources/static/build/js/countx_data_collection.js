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
    $("#add_collection").on("hidden.bs.modal", function () {
        $("#addCollectionForm").find("input").val("");
        $("#addCollectionForm").trigger("reset");
    })

    // UPDATE MILL
    $(".edit").on("click", function () {
        // SET ITEM CATEGORY FIELDS
        console.log($(this).closest("tr").find(".active").val());
        var province = "<option selected=true value=" + "'" + $(this).closest("tr").find(".province").val() + "'" + ">" + $(this).closest("tr").find(".province").val() + "</option>";
        //$("#accountDataForm #province").empty();
        $("#addCollectionForm #province").append(province);
        var district = "<option selected=true value=" + "'" + $(this).closest("tr").find(".district").val() + "'" + ">" + $(this).closest("tr").find(".district").val() + "</option>";
        //$("#accountDataForm #district").empty();
        $("#addCollectionForm #district").append(district);

        var option = "<option selected=true value=" + "'" + $(this).closest("tr").find(".city").val() + "'" + ">" + $(this).closest("tr").find(".city").val() + "</option>";
        $("#addCollectionForm #city").append(option);

        var option = "<option selected=true value=" + "'" + $(this).closest("tr").find(".itemDef_id").val() + "'" + ">" + $(this).closest("tr").find(".itemDef").val() + "</option>";
        $("#addCollectionForm #itemDef").append(option);

        console.log($(this).closest("tr").find(".province").val());
        $("#addCollectionForm #id").val($(this).closest("tr").find(".collection_id:first").val());
        $("#addCollectionForm #bsName").val($(this).closest("tr").find(".bsName").val());
        $("#addCollectionForm #phone").val($(this).closest("tr").find(".phone").val());
        $("#addCollectionForm #mobile").val($(this).closest("tr").find(".mobile").val());

        $("#addCollectionForm #email").val($(this).closest("tr").find(".email").val());
        $("#addCollectionForm #address").val($(this).closest("tr").find(".address").val());
        $("#addCollectionForm #trader").val($(this).closest("tr").find(".trader").val());
        $("#addCollectionForm #miller").val($(this).closest("tr").find(".miller").val());
        $("#addCollectionForm #broker").val($(this).closest("tr").find(".broker").val());
        $("#add_collection").modal("show");
    });

    $("#mobile").blur(function () {
        //console.log("blur");
        mobileNoExistInDataCollection();
    });

    function mobileNoExistInDataCollection() {
        mobileNo = $("#addCollectionForm #mobile").val();
        $.get("/accounts/mobileNoExistInDataCollection?mobileNo=" + mobileNo, function (data) {
            if (data != "" && data != null) {
                console.log("number:" + mobileNo);
                $("#addCollectionForm #id").val(data.id);
                $("#addCollectionForm #bsName").val(data.bsName);
                $("#addCollectionForm #phone").val(data.phone);
                $("#addCollectionForm #mobile").val(data.mobile);
                $("#addCollectionForm #itemDef").val(data.itemDef);
                $("#addCollectionForm #status").val(data.status);
                $("#addCollectionForm #email").val(data.email);
                $("#addCollectionForm #address").val(data.address);
            } else {

            }
        });
    }

    function getFormattedDate(date) {
        var year = date.getFullYear();
        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        return day + '/' + month + '/' + year;
    }


    if ($("#data_collection_table").length) {
        var itemSubCategoryTable = $("#data_collection_table").DataTable({
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
                    title: 'Data Collection' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    footer: false,
                    exportOptions: {
                        stripNewlines: false
                    }
                },
                {
                    extend: 'pdfHtml5',
                    download: 'open',
                    title: 'Data Collection' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
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
//	                 exportOptions: {
//	                     stripHtml: false,
//	                     stripNewlines: false,
//	                     columns: [ 0, 1, 2],
//	                 },
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
                this.api().columns([4, 5, 6, 9, 10]).every(function () {
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


    }


});