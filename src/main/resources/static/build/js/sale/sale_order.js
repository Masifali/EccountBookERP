/**
 * sale_order.js
 * Interactive Client-Side Logic for Sale Order Definition (JDK 17)
 */
$(document).ready(function () {
    let lineItems = [];

    // Initialize Screen & Lookups
    initScreen();

    function initScreen() {
        const today = new Date().toISOString().split('T')[0];
        $("#orderDate").val(today);
        
        loadCustomers();
        loadItems();
        loadWarehouses();
        loadPackings();
        loadHistory();
    }

    // Tab Switching
    $(".erp-tab-button").on("click", function () {
        $(".erp-tab-button").removeClass("active");
        $(this).addClass("active");
        const target = $(this).data("target");
        $(".tab-content .tab-pane").removeClass("active");
        $(target).addClass("active");
        if (target === "#tabHistory") {
            loadHistory();
        }
    });

    function loadCustomers() {
        $.get("/api/sale/common/customers", function (data) {
            let $select = $("#customerId");
            $select.empty().append('<option value="">(Select Customer)</option>');
            $.each(data, function (i, c) {
                $select.append(`<option value="${c.id}">${c.accountCode} - ${c.accountTitle}</option>`);
            });
        });
    }

    function loadItems() {
        $.get("/api/sale/common/items", function (data) {
            let $select = $("#lineItemId");
            $select.empty().append('<option value="">(Select Item)</option>');
            $.each(data, function (i, item) {
                $select.append(`<option value="${item.id}" data-code="${item.itemCode}" data-name="${item.itemName}" data-price="${item.tradePrice}">${item.itemCode} - ${item.itemName}</option>`);
            });
        });
    }

    function loadWarehouses() {
        $.get("/api/sale/common/warehouses", function (data) {
            let $select = $("#warehouseId");
            $select.empty().append('<option value="">(Select Warehouse)</option>');
            $.each(data, function (i, w) {
                $select.append(`<option value="${w.id}">${w.warehouseName}</option>`);
            });
        });
    }

    function loadPackings() {
        $.get("/api/sale/common/packings", function (data) {
            let $select = $("#linePackingId");
            $select.empty().append('<option value="">(Select Packing)</option>');
            $.each(data, function (i, p) {
                $select.append(`<option value="${p.id}">${p.packingTitle}</option>`);
            });
        });
    }

    $("#lineItemId").on("change", function () {
        const price = $(this).find(":selected").data("price") || 0;
        $("#lineRate").val(price);
    });

    // Add Line Item
    $("#btnAddLine").on("click", function () {
        const itemId = parseInt($("#lineItemId").val());
        const itemCode = $("#lineItemId").find(":selected").data("code");
        const itemName = $("#lineItemId").find(":selected").data("name");
        const qty = parseFloat($("#lineQty").val()) || 0;
        const rate = parseFloat($("#lineRate").val()) || 0;

        if (!itemId || qty <= 0) {
            alert("Please select an item and enter quantity > 0");
            return;
        }

        const amt = (qty * rate).toFixed(2);
        lineItems.push({
            itemId: itemId,
            itemCode: itemCode,
            itemName: itemName,
            quantity: qty,
            rate: rate,
            amount: parseFloat(amt)
        });

        renderItemsGrid();
        $("#lineQty").val("");
    });

    function renderItemsGrid() {
        let $tbody = $("#grdItems tbody");
        $tbody.empty();

        if (lineItems.length === 0) {
            $tbody.append(`<tr><td colspan="6" style="text-align:center; color:#94a3b8;">No items added to order</td></tr>`);
            return;
        }

        $.each(lineItems, function (idx, item) {
            $tbody.append(`<tr>
                <td style="font-weight:bold; color:#00796B;">${item.itemCode}</td>
                <td>${item.itemName}</td>
                <td style="text-align:right;">${item.quantity.toFixed(2)}</td>
                <td style="text-align:right;">${item.rate.toFixed(2)}</td>
                <td style="text-align:right; font-weight:bold;">${item.amount.toFixed(2)}</td>
                <td style="text-align:center;">
                    <button type="button" class="btn-erp btn-delete-line" data-idx="${idx}" style="padding:0 4px; color:#dc2626;"><i class="fa fa-trash"></i></button>
                </td>
            </tr>`);
        });

        $(".btn-delete-line").on("click", function () {
            const idx = $(this).data("idx");
            lineItems.splice(idx, 1);
            renderItemsGrid();
        });
    }

    // Save Order
    $("#btnSave").on("click", function () {
        const customerId = parseInt($("#customerId").val());
        if (!customerId) {
            alert("Please select a customer.");
            return;
        }
        if (lineItems.length === 0) {
            alert("Please add at least one line item.");
            return;
        }

        const payload = {
            voucherCode: parseInt($("#voucherCode").val()) || null,
            orderDate: $("#orderDate").val(),
            customerId: customerId,
            warehouseId: parseInt($("#warehouseId").val()) || null,
            remarks: $("#remarks").val(),
            lineItems: lineItems
        };

        $.ajax({
            url: "/sale/sale-order/api/save",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(payload),
            success: function (res) {
                if (res.success) {
                    alert(res.message);
                    resetForm();
                } else {
                    alert("Save failed: " + res.message);
                }
            },
            error: function (xhr) {
                alert("Error saving order: " + (xhr.responseJSON ? xhr.responseJSON.message : "Server error"));
            }
        });
    });

    $("#btnNew, #btnRefresh").on("click", function () {
        resetForm();
    });

    function resetForm() {
        lineItems = [];
        renderItemsGrid();
        $("#customerId").val("");
        $("#remarks").val("");
        $.get("/api/sale/common/next-code/20", function (res) {
            $("#voucherCode").val(res.nextCode);
        });
    }

    function loadHistory() {
        $.get("/sale/sale-order/api/history", function (data) {
            let $tbody = $("#grdHistory tbody");
            $tbody.empty();
            if (!data || data.length === 0) {
                $tbody.append(`<tr><td colspan="4" style="text-align:center; color:#94a3b8;">No sale orders found</td></tr>`);
                return;
            }
            $.each(data, function (i, o) {
                $tbody.append(`<tr>
                    <td style="font-weight:bold; color:#00796B;">${o.voucherCode}</td>
                    <td>${o.orderDate || ''}</td>
                    <td>${o.customerName || ''}</td>
                    <td style="text-align:right; font-weight:bold;">${parseFloat(o.totalAmount || 0).toFixed(2)}</td>
                </tr>`);
            });
        });
    }
});
