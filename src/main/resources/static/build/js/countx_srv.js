function getFormData($form){
    var unindexed_array = $form.serializeArray();
    var indexed_array = {};

    $.map(unindexed_array, function(n, i){
        indexed_array[n['name']] = n['value'];
    });

    return indexed_array;
}

Number.prototype.toFixed = function(decimalPlaces) {
	return Number(Math.round(this + 'e' + decimalPlaces) + 'e-' + decimalPlaces);
}

$( document ).ready(function() {
	
	//instrument select2 dropdowns
	$(".select2_single").on("select2:close", function(){
		$(this).focus();
	});
	
	$('select#company option:not(:selected)').attr('disabled', true);
	$('select#itemStock\\.branch option:not(:selected)').attr('disabled', true);
	
	$("#voucherStatus").on("change", function(){
		if ($("#voucherStatus").val() == "M"){
			$("#itemStock\\.salesTax").val("0.00");
			$("#itemStock\\.furtherSalesTax").val("0.00");
			$("#itemStock\\.advanceIncomeTax").val("0.00");
		}
	});
	
	$(".sale_order_entry_add").on("click", function () {
		
		// un-instrument select2 dropdowns
		$(this).closest("tr").find(".select2_single").select2("destroy");
		
		var newRow = $(this).closest("tr").clone(true);
		
		//when you use clone, even the text is carried over. To remove it, do the following:
		newRow.children("td").children("input, span").each(function(index, element){
			$(element).val("");
			$(element).text("");
		});
		newRow.children("td").each(function(index, element){
			if ($(this).children().length < 1 ) {
				$(element).text("");
			}
			$(element).find(".itemCategory:first").val("");
			$(element).find(".itemDef:first").val("");
		});
	    
	    newRow.insertAfter($(this).closest("tr"));
		
		// again instrument select2 dropdowns
		$("#sale_order_entry_table .select2_single").select2({dropdownAutoWidth : true, width: '100%'});
		
		// update serial numbers
		$("#sale_order_entry_table span#serial_no").each(function(index, element){
			$(element).text(index + 1);
			
			$(element).closest("tr").find( "input[name*='.id']" ).attr("id", "itemStock.itemStockEntries" + index + ".id");
			$(element).closest("tr").find( "input[name*='.id']" ).attr("name", "itemStock.itemStockEntries[" + index + "].id");
			
			$(element).closest("tr").find( "select[name*='.itemCategory']" ).attr("id", "itemStock.itemStockEntries" + index + ".itemCategory");
			$(element).closest("tr").find( "select[name*='.itemCategory']" ).attr("name", "itemStock.itemStockEntries[" + index + "].itemCategory");
			
			$(element).closest("tr").find( "select[name*='.itemDef']" ).attr("id", "itemStock.itemStockEntries" + index + ".itemDef");
			$(element).closest("tr").find( "select[name*='.itemDef']" ).attr("name", "itemStock.itemStockEntries[" + index + "].itemDef");
			
			$(element).closest("tr").find( "input[name*='.itemQuantity']" ).attr("id", "itemStock.itemStockEntries" + index + ".itemQuantity");
			$(element).closest("tr").find( "input[name*='.itemQuantity']" ).attr("name", "itemStock.itemStockEntries[" + index + "].itemQuantity");
			
			$(element).closest("tr").find( "input[name*='.discountGain']" ).attr("id", "itemStock.itemStockEntries" + index + ".discountGain");
			$(element).closest("tr").find( "input[name*='.discountGain']" ).attr("name", "itemStock.itemStockEntries[" + index + "].discountGain");
			
			$(element).closest("tr").find( "input[name*='.unitWeight']" ).attr("id", "itemStock.itemStockEntries" + index + ".unitWeight");
			$(element).closest("tr").find( "input[name*='.unitWeight']" ).attr("name", "itemStock.itemStockEntries[" + index + "].unitWeight");
	    });
	});
	
	$(".sale_order_entry_delete").on("click", function () {
		
		if($(this).parents("tbody").find("tr").length > 1)
		{
			$(this).closest("tr").remove();
		}
		
		// update serial numbers
		$("#sale_order_entry_table span#serial_no").each(function(index, element){
			$(element).text(index + 1);
			
			$(element).closest("tr").find( "input[name*='.id']" ).attr("id", "itemStock.itemStockEntries" + index + ".id");
			$(element).closest("tr").find( "input[name*='.id']" ).attr("name", "itemStock.itemStockEntries[" + index + "].id");
			
			$(element).closest("tr").find( "select[name*='.itemCategory']" ).attr("id", "itemStock.itemStockEntries" + index + ".itemCategory");
			$(element).closest("tr").find( "select[name*='.itemCategory']" ).attr("name", "itemStock.itemStockEntries[" + index + "].itemCategory");
			
			$(element).closest("tr").find( "select[name*='.itemDef']" ).attr("id", "itemStock.itemStockEntries" + index + ".itemDef");
			$(element).closest("tr").find( "select[name*='.itemDef']" ).attr("name", "itemStock.itemStockEntries[" + index + "].itemDef");
			
			$(element).closest("tr").find( "input[name*='.itemQuantity']" ).attr("id", "itemStock.itemStockEntries" + index + ".itemQuantity");
			$(element).closest("tr").find( "input[name*='.itemQuantity']" ).attr("name", "itemStock.itemStockEntries[" + index + "].itemQuantity");
			
			$(element).closest("tr").find( "input[name*='.discountGain']" ).attr("id", "itemStock.itemStockEntries" + index + ".discountGain");
			$(element).closest("tr").find( "input[name*='.discountGain']" ).attr("name", "itemStock.itemStockEntries[" + index + "].discountGain");
			
			$(element).closest("tr").find( "input[name*='.unitWeight']" ).attr("id", "itemStock.itemStockEntries" + index + ".unitWeight");
			$(element).closest("tr").find( "input[name*='.unitWeight']" ).attr("name", "itemStock.itemStockEntries[" + index + "].unitWeight");
	    });
	});
	
	
	restrictVoucherDate();
	
	$("#financialYear").on("change", function(){
		restrictVoucherDate();
	});
	
	function restrictVoucherDate(){
		
		if (!$("#financialYear").val()){
			return;
		}
		
		$("#voucherDate").attr("min", $("#financialYear option:selected").attr("data-value").split("|")[0]);
		$("#voucherDate").attr("max", $("#financialYear option:selected").attr("data-value").split("|")[1]);
	}	
	
	// calculate total on load
	calculateAmountEntriesTotal();
	
	function calculateAmountEntriesTotal(){
		
		var debitTotal = parseFloat(0);
		var creditTotal = parseFloat(0);
		
		var formatter = new Intl.NumberFormat('ur-PK', { style: 'currency', currency: 'PKR', minimumFractionDigits: 2, maximumFractionDigits: 2})
		
		$("#voucher_entry_table tbody tr").each(function() {
	    	debitTotal = debitTotal + parseFloat($(this).find("#voucherEntry_debit").val() || 0);
	    	creditTotal = creditTotal + parseFloat($(this).find("#voucherEntry_credit").val() || 0);
	    });
		
		$("#voucher_entry_table span#debitTotal").text(formatter.format(debitTotal));
		$("#voucher_entry_table span#creditTotal").text(formatter.format(creditTotal));
		
		if ($("#voucher_entry_table span#debitTotal").text() == $("#voucher_entry_table span#creditTotal").text()){
			$("#voucher_entry_table span#debitTotal").css("color", "#73879C");
			$("#voucher_entry_table span#creditTotal").css("color", "#73879C");
    	}
		else{
			$("#voucher_entry_table span#debitTotal").css("color", "red");
			$("#voucher_entry_table span#creditTotal").css("color", "red");
		}
	}
	
	/*$("#sale_order_entry_table tbody tr .itemCategory").each(function() {
		loadItemCategoryDefs($(this));
    });*/
	
	$(".itemCategory").on("change", function(){
		loadItemCategoryDefs($(this));
	});
	
	function loadItemCategoryDefs(thisControl) {
		
		if (!$(thisControl).val() || $(thisControl).val() == 0){
			return;
		}
	    
	    $.get( "/receivables/category_item_defs?categoryId=" + $(thisControl).val(), function( data ) {
	    	$(thisControl).closest("tr").find(".itemDef").empty();
	    	$(thisControl).closest("tr").find(".itemDef").append("<option>&emsp;</option>");
	        
	        for (var i = 0, len = data.length; i < len; i++) {
	        	var option = "<option value = " + data[i].id + ">" + data[i].formattedCode + "&emsp;" + data[i].name +  "</option>";
	        	$(thisControl).closest("tr").find(".itemDef").append(option);
	    	}
	    });
	}
	
	$(".itemDef").on("change", function(){
		loadItemDefDetail($(this));
	});
	
	function loadItemDefDetail(thisControl) {
		
		if (!$(thisControl).val() || $(thisControl).val() == 0){
			return;
		}
		
		if (!$("#company").val() || !$("#branch").val() || !$("#voucherStatus").val()){
			return;
		}
	    
	    $.get( "/receivables/item_def_detail?itemDefId=" + $(thisControl).val()+"&companyId=" + $("#company").val()+"&branchId=" + $("#branch").val()+"&voucherStatusId=" + $("#voucherStatus").val(), function( data ) {
	    	$(thisControl).closest("tr").find(".availableQuantity").text(data.availableQuantity);
	    	$(thisControl).closest("tr").find(".standardPrice").text(data.standardRate);
	    	$(thisControl).closest("tr").find(".unitWeight").val(data.weight);
	    	$(thisControl).closest("tr").find(".feet").text(data.feet);
	    	$(thisControl).closest("tr").find(".pricingRule").val(data.pricingRule);
	    	calculateTotal();
	    });
	}
	
	calculateTotal();
	
	$("#sale_order_entry_table").on("input", "input", function () {
		calculateTotal();	
    });
	$("#tax_table").on("input", "input", function () {
		calculateTotal();	
    });
	
	function calculateTotal(){
		
		var amountTotal = parseFloat(0.00);
		var standardPrice = parseFloat(0.00);
		var discountGain = parseFloat(0.00);
		var percentageVal = parseFloat(0.00);
		
		var totalWeight = parseFloat(0.00);
		var totalFeet = parseFloat(0.00);
		
		var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 2, maximumFractionDigits: 2})
		
    	$("#sale_order_entry_table tbody tr").each(function() {
    		
    		standardPrice = parseFloat($(this).find(".standardPrice").text() || 0);
    		discountGain = parseFloat($(this).find(".discountGain").val() || 0);
    		//percentageVal = calcPercentage(standardPrice, discountGain)
    		
    		//$(this).find(".discountGainPrice").text((standardPrice + percentageVal).toFixed(2));
    		$(this).find(".totalWeight").text(($(this).find(".itemQuantity").val()*$(this).find(".unitWeight").val()).toFixed(2));
    		$(this).find(".totalFeet").text(($(this).find(".itemQuantity").val()*$(this).find(".feet").text()).toFixed(2));
    		
    		if ($(this).find(".pricingRule").val() == "QUANTITY"){
    			percentageVal = discountGain
    			$(this).find(".discountGainPrice").text((standardPrice + percentageVal).toFixed(4));
    			$(this).find(".amount").text(($(this).find(".discountGainPrice").text()*$(this).find(".itemQuantity").val()).toFixed(2));
	    	}
	    	if ($(this).find(".pricingRule").val() == "WEIGHT"){
	    		percentageVal = discountGain
    			$(this).find(".discountGainPrice").text((standardPrice + percentageVal).toFixed(4));
    			$(this).find(".amount").text(($(this).find(".discountGainPrice").text()*$(this).find(".totalWeight").text()).toFixed(2));
	    	}
	    	if ($(this).find(".pricingRule").val() == "FEET"){
	    		percentageVal = calcPercentage(standardPrice, discountGain)
    			$(this).find(".discountGainPrice").text((standardPrice + percentageVal).toFixed(4));
    			$(this).find(".amount").text(($(this).find(".discountGainPrice").text()*$(this).find(".totalFeet").text()).toFixed(2));
	    	}
	    	//$(this).find(".amount").text(formatter.format($(this).find(".amount").text()));
        	
	    	totalWeight = totalWeight + parseFloat($(this).find(".totalWeight").text() || 0);
	    	totalWeight = parseFloat((totalWeight).toFixed(2));
	    	
	    	totalFeet = totalFeet + parseFloat($(this).find(".totalFeet").text() || 0);
	    	totalFeet = parseFloat((totalFeet).toFixed(2));
	    	
			amountTotal = amountTotal + parseFloat($(this).find(".amount").text() || 0);
			amountTotal = parseFloat((amountTotal).toFixed(2));
	    });
		
		$("#sale_order_entry_table tfoot tr:eq(0) th:eq(1)").text(totalWeight);
		$("#sale_order_entry_table tfoot tr:eq(0) th:eq(3)").text(totalFeet);
		$("#sale_order_entry_table tfoot tr:eq(0) th:eq(4)").text(formatter.format(amountTotal));
		
		var unloadingCharges = parseFloat(($("#tax_table tbody #itemStock\\.unloadingRate").val() * totalWeight).toFixed(3));
		$("#tax_table tbody tr:eq(0) th:eq(2)").text(formatter.format(unloadingCharges));
		
		var netAmount = amountTotal;
		$("#tax_table tbody tr:eq(2) th:eq(2)").text(formatter.format(netAmount));
		
		var salesTaxAmount = parseFloat(calcPercentage(netAmount, $("#tax_table tbody #itemStock\\.salesTax").val()).toFixed(2));
		salesTaxAmount = parseFloat((salesTaxAmount).toFixed(2));
		$("#tax_table tbody tr:eq(3) th:eq(2)").text(formatter.format(salesTaxAmount));
		
		var furtherSalesTaxAmount = parseFloat(calcPercentage(netAmount, $("#tax_table tbody #itemStock\\.furtherSalesTax").val()).toFixed(2));
		furtherSalesTaxAmount = parseFloat((furtherSalesTaxAmount).toFixed(2));
		$("#tax_table tbody tr:eq(4) th:eq(2)").text(formatter.format(furtherSalesTaxAmount));
		
		var amountWithSalesTax = netAmount + salesTaxAmount + furtherSalesTaxAmount;
		amountWithSalesTax = parseFloat((amountWithSalesTax).toFixed(2));
		$("#tax_table tbody tr:eq(5) th:eq(2)").text(formatter.format(amountWithSalesTax));
		
		var advanceIncomeTaxAmount = parseFloat(calcPercentage(amountWithSalesTax, $("#tax_table tbody #itemStock\\.advanceIncomeTax").val()).toFixed(2));
		advanceIncomeTaxAmount = parseFloat((advanceIncomeTaxAmount).toFixed(2));
		$("#tax_table tbody tr:eq(5) th:eq(2)").text(formatter.format(advanceIncomeTaxAmount));
		$("#tax_table tbody tr:eq(6) th:eq(2)").text(formatter.format(amountWithSalesTax + advanceIncomeTaxAmount));
		
		$("#voucher_entry_table tbody tr:eq(0) td:eq(5) input").val(parseFloat((amountWithSalesTax + advanceIncomeTaxAmount).toFixed(2)));
		$("#voucher_entry_table tbody tr:eq(1) td:eq(4) input").val(amountTotal);
		$("#voucher_entry_table tbody tr:eq(2) td:eq(5) input").val(unloadingCharges);
		$("#voucher_entry_table tbody tr:eq(3) td:eq(4) input").val(unloadingCharges);
		$("#voucher_entry_table tbody tr:eq(4) td:eq(4) input").val(salesTaxAmount);
		$("#voucher_entry_table tbody tr:eq(5) td:eq(4) input").val(furtherSalesTaxAmount);
		$("#voucher_entry_table tbody tr:eq(6) td:eq(4) input").val(advanceIncomeTaxAmount);
		
		calculateAmountEntriesTotal();	
		
	}
	
	function calcPercentage(amount, percentage)
	{
		return ((amount * percentage) / 100).toFixed(4);
	}
	
	$(".unloadingRate").on("input", function () {
		$(".unloadingCharges").val(($(".unloadingRate").val() * $("#sale_order_entry_table tfoot tr:eq(0) th:eq(1)").text()).toFixed(2));
    });
	
	$("#calculateForm").on("click", function (event) {
		event.preventDefault();
	    $("#voucherForm").attr("action", $(location).attr("pathname")).submit();
    });
	$("#submitForm").on("click", function (event) {
		event.preventDefault();
	    $("#voucherForm").attr("action", "/sale_return_voucher/add_or_update_voucher").submit();
    });
    
})