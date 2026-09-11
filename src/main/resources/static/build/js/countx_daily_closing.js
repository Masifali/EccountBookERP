$(document).ready(function () {

    // Function to parse number safely (removes commas)
    function parseNumber(value) {
        if (!value) return 0;
        return parseFloat(value.toString().replace(/,/g, '')) || 0;
    }

    // Auto calculate row amount (qty * note)
    function calculateRowAmount(row) {
        let qty = parseNumber(row.find("input[name$='.qty']").val());
        let note = parseNumber(row.find("input[name$='.note']").val());
        row.find(".amount").val((qty * note).toFixed(0));
    }

    calculateTotals()
    calculateDifference();

    // Calculate table totals
    function calculateTotals() {
        let qtyTotal = 0;
        let amountTotal = 0;

        $("#closing_entry_table tbody tr").each(function () {
            let qty = parseNumber($(this).find("input[name$='.qty']").val());
            let note = parseNumber($(this).find("input[name$='.note']").val());
            $(this).find(".amount").val((qty * note).toFixed(0));
            let amount = parseNumber($(this).find(".amount").val());
            qtyTotal += qty;
            amountTotal += amount;
        });

        $("#qtyTotal").text(qtyTotal.toFixed(0));
        $("#amountTotal").text(amountTotal.toFixed(0));
    }


    // Calculate difference: (cash + debit + stock) - (profit + credit)
    function calculateDifference() {
        let cash = parseNumber($("#cash").val());
        let debit = parseNumber($("#debit").val());
        let stock = parseNumber($("#stock").val());
        let profit = parseNumber($("#profit").val());
        let credit = parseNumber($("#credit").val());

        let diff = (cash + debit + stock) - (profit + credit);
        $("#diff").text("Difference : " + diff.toLocaleString(undefined, {
            minimumFractionDigits: 0,
            maximumFractionDigits: 0
        }));
        if (diff < 0) {
            $("#diff").css("color", "green"); // negative => green
        } else if (diff > 0) {
            $("#diff").css("color", "red");   // positive => red
        } else {
            $("#diff").css("color", "black"); // zero => black
        }
    }

    // Trigger calculations when inputs change
    $("#closing_entry_table").on("input", "input[name$='.qty'], input[name$='.note']", function () {
        //let row = $(this).closest("tr");
        //calculateRowAmount(row);
        calculateTotals();
        calculateDifference();
    });

    $("#cash, #debit, #stock, #profit, #credit").on("input", function () {
        calculateDifference();
    });

    $("#searchBtn").on("click", function () {
        sendCall();
    });

    // AJAX form submission
    function sendCall() {
        let closingDate = $("#closingDate").val();

        if (!closingDate) {
            alert("Please select closing date");
            return;
        }
        let tbody = $("#closing_entry_table tbody");
        tbody.empty();
        $("#loading").show();

        $.get("/dailyClosingByDate?closingDate=" + closingDate)
            .done(function (response) {

                // ===== Fill main fields =====
                $("#cash").val(response.cash);
                $("#stock").val(response.stock);
                $("#profit").val(response.profit);
                $("#debit").val(response.debit);
                $("#credit").val(response.credit);
                $("#dailyClosingId").val(response.id);

                // ===== Difference =====
                let diff = response.difference;


                let qtyTotal = 0;
                let amountTotal = 0;

                $.each(response.dailyClosingEntries, function (index, entry) {
                    qtyTotal += Number(entry.qty || 0);
                    amountTotal += Number(entry.amount || 0);

                    let row = `
                    <tr>
                        <td>
                            ${index + 1}
                            <input type="hidden" name="dailyClosingEntries[${index}].id" value="${entry.id || ''}">
                        </td>

                      

                         <td>
        <input type="number"
               class="form-control"
               onfocus="this.select()"
               required
               style="font-weight:bold;"
               id="dailyClosingEntries${index}.qty"
               name="dailyClosingEntries[${index}].qty"
               value="${entry.qty ?? ''}">
    </td>
                        <td>
        <input type="number"
               class="form-control"
               onfocus="this.select()"
               required
               style="font-weight:bold;"
               id="dailyClosingEntries${index}.note"
               name="dailyClosingEntries[${index}].note"
               value="${entry.note ?? ''}">
    </td>

                        <td>
                            <input type="text" class="form-control amount" readonly="readonly"
                                   name="dailyClosingEntries[${index}].amount"
                                   value="${entry.amount}"
                                   style="font-weight:bold">
                        </td>
                    </tr>
                `;
                    tbody.append(row);
                });

                // ===== Footer totals =====
                $("#qtyTotal").text(qtyTotal.toFixed(2));
                $("#amountTotal").text(amountTotal.toFixed(2));

            })
            .fail(function () {
                alert("No data found for selected date");
                $("#closing_entry_table tbody").empty();
                $("#qtyTotal").text("0.00");
                $("#amountTotal").text("0.00");
            })
            .always(function () {
                // Hide loading GIF after AJAX finishes (success or error)
                $("#loading").hide();
            });
        calculateTotals();
        calculateDifference();
    }


    $("#voucherForm").submit(function (e) {
        e.preventDefault();

        // Prepare dailyClosing object
        var entries = [];

        $("#closing_entry_table tbody tr").each(function () {
            let row = $(this);
            let entry = {
                id: row.find("input[name$='.id']").val(),
                qty: parseFloat(row.find("input[name$='.qty']").val().replace(/,/g, '')) || 0,
                note: parseFloat(row.find("input[name$='.note']").val().replace(/,/g, '')) || 0,
                amount: parseFloat(row.find(".amount").val().replace(/,/g, '')) || 0
            };
            entries.push(entry);
        });

        var formData = {
            id: $("#dailyClosingId").val(),
            cash: parseFloat($("#cash").val().replace(/,/g, '')) || 0,
            stock: parseFloat($("#stock").val().replace(/,/g, '')) || 0,
            profit: parseFloat($("#profit").val().replace(/,/g, '')) || 0,
            debit: parseFloat($("#debit").val().replace(/,/g, '')) || 0,
            credit: parseFloat($("#credit").val().replace(/,/g, '')) || 0,
            closingDate: $("#closingDate").val(),
            dailyClosingEntries: entries
        };

        // POST JSON
        $.ajax({
            url: "/vouchers/dailyClosing",
            type: "POST",
            contentType: 'application/json',
            data: JSON.stringify(formData),
            dataType: "json",
            success: function (response) {
                $("#modal_notify .modal-title").text(response.success);
                $("#modal_notify").modal("show");
            },
            error: function (xhr) {
                $.alert({
                    title: 'Error',
                    content: xhr.responseText,
                    type: 'red'
                });
            },
            complete: function () {
                $("#submitForm").prop("disabled", false).text("Submit");
            }
        });
        location.reload();
    });


});
