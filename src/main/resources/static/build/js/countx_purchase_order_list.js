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

    $("#printPurchaseOrderList").on('click', function () {
        //alert('i m printer');
        location.replace("/reports/purchase-order-list-report/?branchId=" + $("#branch").val() + "&fromDate=" + $("#fromDate").val() + "&toDate=" + $("#toDate").val() + "&itemDefId=" + $("#item").val() + "&millId=" + $("#millKhata").val() + "&status=" + $("#status").val() + "&paymentType=" + $("#paymentType").val() + "&branchNamed=" + $("#branch option:selected").text() + "&accountNamed=" + $("#accountCode option:selected").text() + "&itemNamed=" + $("#item option:selected").text() + "&millKhataName=" + $("#millKhata option:selected").text() + "&searchByDate=" + $("#search_by_date option:selected").text() + "");
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
    $("#purchaseOrderFilterForm").submit(function (event) {
        // Prevent the form from submitting via the browser.

        event.preventDefault();
        getPurchaseOrderList();
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

    function getPurchaseOrderList() {
        $('#loading').show();
        // disable submit button
        $("#getPurchaseOrderList").attr("disabled", true);

        $('#purchase_order_datatable').DataTable().clear();
        $('#purchase_order_datatable').DataTable().destroy();
        var OrderListTable = $('#purchase_order_datatable').DataTable({
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
                    title: 'PURCHASE ORDER LIST' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
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

        });

        // PREPARE FORM DATA
        var formData = {
            company: {id: $("#purchaseOrderFilterForm #company").val(), name: ""},
            branch: {id: $("#purchaseOrderFilterForm #branch").val(), name: ""},
            voucherStatus: {id: $("#purchaseOrderFilterForm #voucherStatus").val(), name: ""},
            purchaseOrderCode: $("#purchaseOrderFilterForm #purchaseOrderCode").val(),
            pending: $("#purchaseOrderFilterForm #pending").val(),
            searchByDate: $("#purchaseOrderFilterForm #searchByDate").val(),
            fromDate: $("#purchaseOrderFilterForm #fromDate").val(),
            toDate: $("#purchaseOrderFilterForm #toDate").val(),
            accountCode: $("#purchaseOrderFilterForm #accountCode").val(),
            qty: $("#purchaseOrderFilterForm #qty").val(),
            searchByDate: $("#purchaseOrderFilterForm #search_by_date").val(),
            requestFromPjv: "0",
            item: $("#purchaseOrderFilterForm #item").val(),
            paymentType: $("#purchaseOrderFilterForm #paymentType").val(),
            status: $("#purchaseOrderFilterForm #status").val(),
        }

        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: $("#purchaseOrderFilterForm").attr("action"),
            data: JSON.stringify(formData),
            dataType: "json",
            success: function (data) {
                // FILL TABLE ROWS

                OrderListTable.clear().draw();

                $.each(data, function (i, purchaseOrder) {
                    var voucherViewLink = "";
                    $.each(purchaseOrder.vouchers, function (j, voucher) {
                        if (j == 0) {
                            if (voucher.code.includes("PTV")) {
                                voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/SaleQty/view_pjv_sjv_qty/" + voucher.id + ">" + voucher.code + "</a>";
                            } else {
                                voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/purchase_journal_vouchers/" + voucher.id + ">" + voucher.code + "</a>";

                            }
                        } else {
                            var voucherLinkMore = "";
                            if (voucher.code.includes("PTV")) {
                                voucherLinkMore = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/SaleQty/view_pjv_sjv_qty/" + voucher.id + ">" + voucher.code + "</a>";
                            } else {
                                voucherLinkMore = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/purchase_journal_vouchers/" + voucher.id + ">" + voucher.code + "</a>";

                            }
                            voucherViewLink = voucherViewLink + "/" + voucherLinkMore;
                        }
                        //  console.log(voucher + "   " + purchaseOrder.purchaseOrderEntryid);
                    });
                    var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 0, maximumFractionDigits: 0})

                    OrderListTable.row.add([
                        (i + 1),
                        '<input type= "hidden" class ="purchaseOrder" value=' + purchaseOrder.purchaseOrderId + '> <a class ="purchaseOrderCode">' + purchaseOrder.purchaseOrderCode + '<br/>' + purchaseOrder.status + '</a>',
                        '<input type= "hidden" class ="paymentType" value=' + purchaseOrder.paymentType + '>  <a class ="paymentDate">' + getFormattedDate(new Date(purchaseOrder.date)) + " , " + getFormattedDate(new Date(purchaseOrder.paymentDate)) + '</a>',

                        '<input type= "hidden" class ="status" value=' + purchaseOrder.status + '> <a class ="accountName">' + purchaseOrder.account.accountName + '(' + purchaseOrder.account.mobile + ')' + '<br/>' + purchaseOrder.millKhata + '</a>',
                        //'<a class = "itemName">' + purchaseOrder.itemDef.formattedCode + "&emsp;" + purchaseOrder.itemSubCategory.name + " " + purchaseOrder.itemDef.name + '</a>',
                        '<a class = "itemName">' + purchaseOrder.itemSubCategory.name + " " + purchaseOrder.itemDef.name + '<br/>' + purchaseOrder.paymentType + '</a>',

                        '<a class="rate">' + formatter.format(purchaseOrder.rate) + '</a>',
                        '<a class="kg">' + formatter.format(purchaseOrder.kg) + '</a>',

                        '<a class="receivedWeight">' + purchaseOrder.receivedWeight + '</a>',
                        '<a class="pendingWeight">' + purchaseOrder.pendingWeight + '</a>',
                        '<a class="vehical">' + purchaseOrder.vehical + '</a>',
                        '<a class="remarks">' + purchaseOrder.createdBy + '<br/>' + purchaseOrder.modifiedBy + '</a>',
                        voucherViewLink,
                        '<input type= "hidden" class ="after" value=' + purchaseOrder.after + '><input type= "hidden" class ="persentage" value=' + purchaseOrder.persentage + '><input type= "hidden" class ="inWeek" value=' + purchaseOrder.inWeek + '><input type= "hidden" class ="id" value=' + purchaseOrder.purchaseOrderEntryid + '> <input type= "hidden" class ="itemDefId" value=' + purchaseOrder.itemDef.id + '> <input type= "hidden" class ="millId" value=' + purchaseOrder.millKhataId + '> <input type= "hidden" class ="code" value=' + purchaseOrder.account.code + '><a style="text-decoration: underline;" target=_blank href=' + location.protocol + '//' + location.host + '/purchase_orders/' + purchaseOrder.purchaseOrderId + '>VIEW</a> / <a class="edit" style="text-decoration: underline;color:green" href="javascript:void(0)">EDIT</a>'
                    ]).draw();
                    $("#getPurchaseOrderList").attr("disabled", false);
                });
            },
            complete: function () {
                // enable submit button
                $("#getPurchaseOrderList").attr("disabled", false);
                $("#loading").hide();

            },
        });

    }

    var editRow = "";

    $('body').on('click', '.edit', function () {

        editRow = $(this);

        var now = $(this).closest("tr").find(".paymentDate").text().split(",")[1];
        var newString = now.split("/")[2].trim() + "-" + now.split("/")[1].trim() + "-" + now.split("/")[0].trim();
        var today = getFormattedDashDate(new Date(newString));

        $("#purchaseOrderEntryForm #itemDef").val($(this).closest("tr").find('.itemDefId').val()).trigger('change.select2');
        $("#purchaseOrderEntryForm #millKhata\\.id").val($(this).closest("tr").find(".millId").val()).trigger('change.select2');
        $("#purchaseOrderEntryForm #supplierAccount\\.code").val($(this).closest("tr").find(".code").val()).trigger('change.select2');
        $("#purchaseOrderEntryForm #rate").val($(this).closest("tr").find(".rate").text().trim().replace(/[^0-9\.-]+/g, "") || 0);
        $("#purchaseOrderEntryForm #kg").val($(this).closest("tr").find(".kg").text().trim().replace(/[^0-9\.-]+/g, "") || 0);
        //$("#purchaseOrderEntryForm #ton").val($(this).closest("tr").find(".ton").text().trim().replace(/[^0-9\.-]+/g, "") || 0);

        $("#purchaseOrderEntryForm #id").val($(this).closest("tr").find(".id").val().trim());
        $("#purchaseOrderEntryForm #paymentDate").val(today);//.trigger('change');
        $("#purchaseOrderEntryForm #paymentType").val($(this).closest("tr").find(".paymentType").val().trim());
        $("#purchaseOrderEntryForm #status").val($(this).closest("tr").find(".status").val().trim());
        $("#purchaseOrderEntryForm #purchaseOrder").val($(this).closest("tr").find(".purchaseOrder").val().trim());//.trigger('change');
        $("#purchaseOrderEntryForm #po").text("" + $(this).closest("tr").find(".purchaseOrderCode").text().trim());
        $("#purchaseOrderEntryForm #vehical").val($(this).closest("tr").find(".vehical").text().trim().split("-")[0].trim());
        $("#purchaseOrderEntryForm #remarks").val($(this).closest("tr").find(".remarks").text().trim());
        if ($("#purchaseEntryEdit").val() === 'true') {
            $("#editPurchaseEntryModel").modal("show");
        }
    });

    $("#purchaseOrderEntryForm").submit(function (event) {
        // Prevent the form from submitting via the browser.
        $("#saved").attr("disabled", true);
        event.preventDefault();
        getPurchaseOrderEntrySaved();
    });

    function getPurchaseOrderEntrySaved() {
        console.log($("#purchaseOrderEntryForm #supplierAccount\\.code").val());
        var formData = {

            itemDef: {id: $("#purchaseOrderEntryForm #itemDef").val(), name: ""},
            millKhata: {id: $("#purchaseOrderEntryForm #millKhata\\.id").val()},
            supplierAccount: {code: $("#purchaseOrderEntryForm #supplierAccount\\.code").val(), accountName: ""},
            rate: $("#purchaseOrderEntryForm #rate").val(),
            kg: $("#purchaseOrderEntryForm #kg").val(),
            ton: $("#purchaseOrderEntryForm #ton").val(),
            payPersentage: $("#purchaseOrderEntryForm #persentage").val(),
            inWeek: $("#purchaseOrderEntryForm #inWeek").val(),
            after: $("#purchaseOrderEntryForm #after").val(),
            id: $("#purchaseOrderEntryForm #id").val(),
            remarks: $("#purchaseOrderEntryForm #remarks").val(),
            vehical: $("#purchaseOrderEntryForm #vehical").val(),
            paymentDate: $("#purchaseOrderEntryForm #paymentDate").val(),//.trigger('change');
            paymentType: $("#purchaseOrderEntryForm #paymentType").val(),
            status: $("#purchaseOrderEntryForm #status").val(),
            purchaseOrder: {id: $("#purchaseOrderEntryForm #purchaseOrder").val()},//.trigger('change');*/
            active: "true",

        }

        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: $("#purchaseOrderEntryForm").attr("action"),
            data: JSON.stringify(formData),
            dataType: "json",
            success: function (data) {
                console.log("aaa");
                $("#saved").attr("disabled", false);
                $("#editPurchaseEntryModel").modal('toggle');
                editRow.closest("tr").find(".purchaseOrderCode").text(data.purchaseOrderCode + '   ' + data.status);
                editRow.closest("tr").find(".kg").text(data.kg);
                editRow.closest("tr").find(".pendingWeight").text(data.pendingWeight);
                editRow.closest("tr").find(".paymentDate").text(editRow.closest("tr").find(".paymentDate").text().split(",")[0].trim() + ',' + getFormattedDate(new Date(data.date)));
                editRow.closest("tr").find(".paymentType").val(data.paymentType);
                editRow.closest("tr").find(".status").val(data.status);
                editRow.closest("tr").find(".itemDefId").val(data.itemDef.id);
                editRow.closest("tr").find(".millId").val(data.millKhataId);
                editRow.closest("tr").find(".code").val(data.account.code);
                editRow.closest("tr").find(".vehical").text(data.vehical);
                //editRow.closest("tr").find(".after").val(data.after);
                editRow.closest("tr").find(".rate").text(data.rate);
                //editRow.closest("tr").find(".inWeek").val(data.inWeek);
                editRow.closest("tr").find(".remarks").text(data.remarks);
                editRow.closest("tr").find(".itemName").text(data.itemSubCategory + " " + data.itemDef.name);
                editRow.closest("tr").find(".accountName").text(data.account.accountName + '   ' + data.paymentType);
                editRow.css('background-color', 'Red');
            },
            complete: function () {
                //   $("#editPurchaseEntryModel").modal('toggle');
            }
        });
    }
})