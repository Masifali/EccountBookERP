function getFormData($form) {
    var unindexed_array = $form.serializeArray();
    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });

    return indexed_array;
}

$(document).ready(function () {
    console.log("sddsd");

    $("#printSaleOrderList").on('click', function () {
        //alert('i m printer');
        location.replace("/reports/sale-order-list-report/?branchId=" + $("#branch").val() + "&fromDate=" + $("#fromDate").val() + "&toDate=" + $("#toDate").val() + "&itemDefId=" + $("#saleOrderFilterForm #item").val() + "&millId=" + $("#saleOrderFilterForm #millKhata").val() + "&status=" + $("#status").val() + "&paymentType=" + $("#paymentType").val() + "&branchNamed=" + $("#saleOrderFilterForm #branch option:selected").text() + "&accountNamed=" + $("#saleOrderFilterForm #accountCode option:selected").text() + "&itemNamed=" + $("#saleOrderFilterForm #item option:selected").text() + "&millKhataName=" + $("#saleOrderFilterForm #millKhata option:selected").text() + "&searchByDate=" + $("#saleOrderFilterForm #search_by_date option:selected").text());
    });

    //instrument select2 dropdowns
    $(".select2_single").on("select2:close", function () {
        $(this).focus();
    });

    $("#company").on("change", function () {
        loadCompanyBranches();
    });

    function loadCompanyBranches() {

        if (!$("#company").val() || $("#company").val() == 0) {
            return;
        }

        $.get("/vouchers/company_branches?companyId=" + $("#company").val(), function (data) {
            $("#branch").empty();
            $("#branch").append("<option></option>");

            for (var i = 0, len = data.length; i < len; i++) {
                var option = "<option value = " + data[i].id + ">" + data[i].name + "</option>";
                $("#branch").append(option);
            }
        });

    }


    // SUBMIT FORM
    $("#saleOrderFilterForm").submit(function (event) {
        // Prevent the form from submitting via the browser.
        event.preventDefault();
        getSaleOrderList();
    });

    function getFormattedDate(date) {
        var year = date.getFullYear();
        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        return day + '/' + month + '/' + year;
    }

    function getFormattedDashDate(date) {
        var year = date.getFullYear();
        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        return year + '-' + month + '-' + day;
    }

    var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 0, maximumFractionDigits: 0});

    function getSaleOrderList() {

        console.log("sddsd");
        // disable submit button
        $("#getSaleOrderList").attr("disabled", true);

        $('#sale_order_datatable').DataTable().clear();
        $('#sale_order_datatable').DataTable().destroy();
        var OrderListTable = $('#sale_order_datatable').DataTable({
            dom: 'Bfrtip',
            deferRender: true,
//  	        scrollY: 500,
//  	        scroller: true,
//  	        destroy: true,
            fixedHeader: true,
            bPaginate: false,
            buttons: [
                {
                    extend: 'pdfHtml5',
                    title: 'SALE ORDER LIST' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    messageTop: 'FROM: ' + getFormattedDate(new Date($("#fromDate").val()))
                        + '   ' + 'TO : ' + getFormattedDate(new Date($("#toDate").val())),
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
                var api = this.api();

                var colNumber = [6, 7, 8]; // Columns to sum

                // Function to remove formatting and extract numeric values
                var intVal = function (i) {
                    if (typeof i === 'string') {
                        // Attempt to extract numeric value
                        var matches = i.match(/>([\d,]+)<\/a>/);
                        return matches ? parseFloat(matches[1].replace(/,/g, '')) : parseFloat(i.replace(/,/g, '') || 0);
                    } else if (typeof i === 'number') {
                        return i;
                    }
                    return 0; // Default to 0 for other types
                };

                // Iterate over each column to calculate totals
                colNumber.forEach(function (colNo) {
                    var total = api
                        .column(colNo, {page: 'current'}) // Adjust for current page if needed
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);

                    // Update the footer with the formatted total
                    $(api.column(colNo).footer()).html(formatter.format(total.toFixed(2)));
                });
            }

        });

        // PREPARE FORM DATA
        var formData = {
            company: {id: $("#saleOrderFilterForm #company").val(), name: ""},
            branch: {id: $("#saleOrderFilterForm #branch").val(), name: ""},
            fromDate: $("#saleOrderFilterForm #fromDate").val(),
            toDate: $("#saleOrderFilterForm #toDate").val(),
            accountCode: $("#saleOrderFilterForm #accountCode").val(),
            item: $("#saleOrderFilterForm #item").val(),
            millKhata: $("#saleOrderFilterForm #millKhata").val(),
            searchByDate: $("#saleOrderFilterForm #search_by_date").val(),
            requestFromSjv: "0",
            paymentType: $("#saleOrderFilterForm #paymentType").val(),
            status: $("#saleOrderFilterForm #status").val(),
        }

        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: $("#saleOrderFilterForm").attr("action"),
            data: JSON.stringify(formData),
            dataType: "json",
            success: function (data) {

                OrderListTable.clear().draw();

                $.each(data, function (i, saleOrder) {
                    var voucherViewLink = "";
                    $.each(saleOrder.vouchers, function (j, voucher) {
                        /*  if (j == 0)
                              voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/sale_journal_vouchers/" + voucher.id + ">" + voucher.code + "</a>";
                          else
                              voucherViewLink = voucherViewLink + "/" + "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/sale_journal_vouchers/" + voucher.id + ">" + voucher.code + "</a>";
  */
                        if (j == 0) {
                            if (voucher.code.includes("STV")) {
                                voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/SaleQty/view_pjv_sjv_qty/" + voucher.id + ">" + voucher.code + "</a>";
                            } else {
                                voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/sale_journal_vouchers/" + voucher.id + ">" + voucher.code + "</a>";

                            }
                        } else {
                            var voucherLinkMore = "";
                            if (voucher.code.includes("STV")) {
                                voucherLinkMore = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/SaleQty/view_pjv_sjv_qty/" + voucher.id + ">" + voucher.code + "</a>";
                            } else {
                                voucherLinkMore = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/sale_journal_vouchers/" + voucher.id + ">" + voucher.code + "</a>";

                            }
                            voucherViewLink = voucherViewLink + "/" + voucherLinkMore;
                        }
                    });
                    var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 0, maximumFractionDigits: 0})

                    OrderListTable.row.add([
                        (i + 1),
                        '<input type= "hidden" class ="saleOrder" value=' + saleOrder.saleOrderId + '> <a class ="saleOrderCode">' + saleOrder.saleOrderCode + '<br/>' + saleOrder.status + '</a>',
                        '<input type= "hidden" class ="paymentType" value=' + saleOrder.paymentType + '> <a class ="paymentDate">' + getFormattedDate(new Date(saleOrder.date)) + " , " + getFormattedDate(new Date(saleOrder.paymentDate)) + '</a>',

                        '<input type= "hidden" class ="status" value=' + saleOrder.status + '>  <a class="accountName">' + saleOrder.account.accountName + ' <br/>' + saleOrder.millKhata + '</a>',
                        '<a class="itemName">' + saleOrder.itemSubCategory.name + " " + saleOrder.itemDef.name + '<br/>' + saleOrder.paymentType + '</a>',

                        '<a class="rate">' + formatter.format(saleOrder.rate) + '</a>',
                        '<a class="kg">' + formatter.format(saleOrder.kg) + '</a>',

                        '<a class ="receivedWeight">' + saleOrder.receivedWeight + '</a>',
                        '<a class ="pendingWeight">' + saleOrder.pendingWeight + '</a>',
                        '<a class="vehical">' + saleOrder.vehical + '</a>',
                        '<a class="remarks">' + saleOrder.remarks + '</a>',
                        voucherViewLink,
                        '<input type= "hidden" class ="after" value=' + saleOrder.after + '><input type= "hidden" class ="persentage" value=' + saleOrder.persentage + '><input type= "hidden" class ="inWeek" value=' + saleOrder.inWeek + '><input type= "hidden" class ="id" value=' + saleOrder.saleOrderEntryid + '> <input type= "hidden" class ="itemDefId" value=' + saleOrder.itemDef.id + '> <input type= "hidden" class ="millId" value=' + saleOrder.millKhataId + '> <input type= "hidden" class ="code" value=' + saleOrder.account.code + '><a style="text-decoration: underline;" target=_blank href=' + location.protocol + '//' + location.host + '/sale_orders/' + saleOrder.saleOrderId + '>VIEW</a> / <a class="edit" style="text-decoration: underline;color:green" href="javascript:void(0)">EDIT</a>'
                    ]).draw();
                });
            },
            complete: function () {
                // enable submit button
                $("#getSaleOrderList").attr("disabled", false);
            },
        });

    }

    var editRow = "";


    $('body').on('click', '.edit', function () {

        editRow = $(this);
        var now = $(this).closest("tr").find(".paymentDate").text().split(",")[1];
        var newString = now.split("/")[2].trim() + "-" + now.split("/")[1].trim() + "-" + now.split("/")[0].trim();
        var today = getFormattedDashDate(new Date(newString));

        $("#saleOrderEntryForm #itemDef").val($(this).closest("tr").find('.itemDefId').val()).trigger('change.select2');
        $("#saleOrderEntryForm #millKhata\\.id").val($(this).closest("tr").find(".millId").val()).trigger('change.select2');
        $("#saleOrderEntryForm #customerAccount\\.code").val($(this).closest("tr").find(".code").val()).trigger('change.select2');
        $("#saleOrderEntryForm #rate").val($(this).closest("tr").find(".rate").text().trim().replace(/[^0-9\.-]+/g, "") || 0);
        $("#saleOrderEntryForm #kg").val($(this).closest("tr").find(".kg").text().trim().replace(/[^0-9\.-]+/g, "") || 0);
        //$("#saleOrderEntryForm #ton").val($(this).closest("tr").find(".ton").text().trim().replace(/[^0-9\.-]+/g, "") || 0);

        $("#saleOrderEntryForm #status").val($(this).closest("tr").find(".status").val().trim());
        $("#saleOrderEntryForm #id").val($(this).closest("tr").find(".id").val().trim());
        $("#saleOrderEntryForm #paymentDate").val(today);//.trigger('change');
        $("#saleOrderEntryForm #paymentType").val($(this).closest("tr").find(".paymentType").val().trim());
        $("#saleOrderEntryForm #saleOrder").val($(this).closest("tr").find(".saleOrder").val().trim());//.trigger('change');
        $("#saleOrderEntryForm #so").text("" + $(this).closest("tr").find(".saleOrderCode").text().trim());
        $("#saleOrderEntryForm #vehical").val($(this).closest("tr").find(".vehical").text().trim().split("-")[0].trim());
        $("#saleOrderEntryForm #remarks").val($(this).closest("tr").find(".remarks").text().trim());
        if ($("#saleOrderEdit").val() === 'true') {
            $("#editSaleEntryModel").modal("show");
        }
    });

    $("#saleOrderEntryForm").submit(function (event) {
        // Prevent the form from submitting via the browser.
        $("#saved").attr("disabled", true);
        event.preventDefault();
        getSaleOrderEntrySaved();
    });

    function getSaleOrderEntrySaved() {
        //console.log($("#saleOrderEntryForm #customerAccount\\.code" ).val());
        var formData = {

            itemDef: {id: $("#saleOrderEntryForm #itemDef").val(), name: ""},
            millKhata: {id: $("#saleOrderEntryForm #millKhata\\.id").val()},
            customerAccount: {code: $("#saleOrderEntryForm #customerAccount\\.code").val(), accountName: ""},
            rate: $("#saleOrderEntryForm #rate").val(),
            kg: $("#saleOrderEntryForm #kg").val(),
            ton: $("#saleOrderEntryForm #ton").val(),

            id: $("#saleOrderEntryForm #id").val(),
            remarks: $("#saleOrderEntryForm #remarks").val(),
            vehical: $("#saleOrderEntryForm #vehical").val(),
            paymentDate: $("#saleOrderEntryForm #paymentDate").val(),//.trigger('change');
            paymentType: $("#saleOrderEntryForm #paymentType").val(),//.trigger('change');
            status: $("#saleOrderEntryForm #status").val(),
            saleOrder: {id: $("#saleOrderEntryForm #saleOrder").val(),},//.trigger('change');*/
            active: "true",
        }

        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: $("#saleOrderEntryForm").attr("action"),
            data: JSON.stringify(formData),
            dataType: "json",
            success: function (data) {
                console.log(data);
                $("#saved").attr("disabled", false);
                $("#editSaleEntryModel").modal('toggle');
                editRow.closest("tr").find(".kg").text(data.kg);
                editRow.closest("tr").find(".saleOrderCode").text(data.saleOrderCode + '  ' + data.status);
                editRow.closest("tr").find(".pendingWeight").text(parseFloat(data.kg) - parseFloat(editRow.closest("tr").find(".receivedWeight").text().replace(/[\$,]/g, '') || 0));
                editRow.closest("tr").find(".paymentDate").text(editRow.closest("tr").find(".paymentDate").text().split(",")[0].trim() + ',' + getFormattedDate(new Date(data.date)));
                editRow.closest("tr").find(".itemDefId").val(data.itemDef.id);
                editRow.closest("tr").find(".paymentType").val(data.paymentType);
                editRow.closest("tr").find(".status").val(data.status);
                editRow.closest("tr").find(".millId").val(data.millKhataId);
                editRow.closest("tr").find(".code").val(data.account.code);
                editRow.closest("tr").find(".vehical").text(data.vehical);
                // editRow.closest("tr").find(".after").val(data.after);
                editRow.closest("tr").find(".rate").text(data.rate);
                // editRow.closest("tr").find(".inWeek").val(data.inWeek);
                editRow.closest("tr").find(".remarks").text(data.remarks);
                editRow.closest("tr").find(".itemName").text(data.itemSubCategory + " " + data.itemDef.name + '   ' + data.paymentType);
                editRow.closest("tr").find(".accountName").text(data.account.accountName);
                editRow.css('background-color', 'Red');
            },
            complete: function () {
                //   $("#editSaleEntryModel").modal('toggle');
            }
        });
    }

})