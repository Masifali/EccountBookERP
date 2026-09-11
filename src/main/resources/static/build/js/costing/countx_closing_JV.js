Number.prototype.toFixed = function(decimalPlaces) {
	return Number(Math.round(this + "e" + decimalPlaces) + "e-" + decimalPlaces);
};

$(document).ready(function() {

	$("#generateVoucher").on("click", function () {
		alert("Show Generated Voucher");
		window.open("https://support.wwf.org.uk/earth_hour/index.php?type=individual", '_blank');
	});

	function getFormattedDate(date) {
		var year = date.getFullYear();
		var month = (1 + date.getMonth()).toString();
		month = month.length > 1 ? month : "0" + month;
		var day = date.getDate().toString();
		day = day.length > 1 ? day : "0" + day;
		return day + "/" + month + "/" + year;
	}

	function getFormattedDate2(date) {
		var year = date.getFullYear();
		var month = (1 + date.getMonth()).toString();
		month = month.length > 1 ? month : "0" + month;
		var day = date.getDate().toString();
		day = day.length > 1 ? day : "0" + day;
		return year + "-" + month + "-" + day ;
	}
	
	function getFormattedDateISO(date) {		
		return [date.getFullYear(), ("0" + (date.getMonth() + 1)).slice(-2), ("0" + date.getDate()).slice(-2)].join("-");
	}
	
	//on load
	restrictFromAndToDate();
	
	loadCompanyBranches();
	
	$("#companyId").on("change", function () {
		loadCompanyBranches();
    });
	
	function loadCompanyBranches() {
		
		if (!$("#companyId").val() || $("#companyId").val() == 0){
			return;
		}
	    
		$.get( "/vouchers/company_branches?companyId=" + $("#companyId").val(), function( data ) {
    		$("#branchId").empty();
    		/*$("#branchId").append("<option value='0'>ALL</option>");*/
            
            for (var i = 0, len = data.length; i < len; i++) {
            	var option = "<option value = " + data[i].id + ">" + data[i].name +  "</option>";
            	$("#branchId").append(option);
        	}
	    });
	}
	
	loadFinancialMonths();
	
	var monthNames = ["JANUARY", "FEBRUARY", "MARCH", "APRIL", "MAY", "JUNE", "JULY", "AUGUST", "SEPTEMBER", "OCTOBER", "NOVEMBER", "DECEMBER"];
	
	$("#financialYearId").on("change", function () {
		loadFinancialMonths();
    });
	
	function loadFinancialMonths() {
		
		if (!$("#financialYearId").val() || $("#financialYearId").val() == 0){
			return;
		}
	    
		$.get( "/costing/financial_months?financialYearId=" + $("#financialYearId").val(), function( data ) {
			$("#financialMonthId").empty();
    		for (var i = 0, len = data.financialYearMonths.length; i < len; i++) {
    			var date = new Date(data.financialYearMonths[i].monthYear);
    			var lastDayDate = new Date(date.getFullYear(), date.getMonth() + 1, 0);
            	var option = "<option value = " + data.financialYearMonths[i].id + " data-value=" + getFormattedDate(date) + "|" + getFormattedDate(lastDayDate) + ">" + monthNames[date.getMonth()] + "&emsp;(" + getFormattedDate(date) + " - " + getFormattedDate(lastDayDate) + ")</option>";
            	$("#financialMonthId").append(option);
        	}
	    });
	}
	
	$("#financialYearId").on("change", function () {
		restrictFromAndToDate();
    });
	
	function restrictFromAndToDate(){
		
		if (!$("#financialYearId").val()){
			return;
		}
		
		$("#fromDate").attr("min", $("#financialYearId option:selected").attr("data-value").split("|")[0]);
		$("#fromDate").attr("max", $("#financialYearId option:selected").attr("data-value").split("|")[1]);
		$("#fromDate").val($("#financialYearId option:selected").attr("data-value").split("|")[0]);
		
		$("#toDate").attr("min", $("#financialYearId option:selected").attr("data-value").split("|")[0]);
		$("#toDate").attr("max", $("#financialYearId option:selected").attr("data-value").split("|")[1]);
		$("#toDate").val($("#financialYearId option:selected").attr("data-value").split("|")[1]);
		
	}

	// calculate total on load
	calculateTotal();

	$(".voucherEntry_debit, .voucherEntry_credit").on("input", function () {
		calculateTotal();
	});

	function calculateTotal(){

		var debitTotal = parseFloat(0);
		var creditTotal = parseFloat(0);

		var formatter = new Intl.NumberFormat('ur-PK', { style: 'currency', currency: 'PKR', minimumFractionDigits: 2, maximumFractionDigits: 2})

		$("#voucher_entry_table tr").each(function() {
			if ($(this).find("td").length) {

				if ($(this).find(".voucherEntry_debit").val() != "" && parseFloat($(this).find(".voucherEntry_debit").val()) != 0) {
					$(this).find(".voucherEntry_credit").prop("readonly", true);
				}
				else{
					$(this).find(".voucherEntry_credit").prop("readonly", false);
				}

				if ($(this).find(".voucherEntry_credit").val() != "" && parseFloat($(this).find(".voucherEntry_credit").val()) != 0) {
					$(this).find(".voucherEntry_debit").prop("readonly", true);
				}
				else{
					$(this).find(".voucherEntry_debit").prop("readonly", false);
				}

				debitTotal = debitTotal + parseFloat($(this).find(".voucherEntry_debit").val() || 0);
				creditTotal = creditTotal + parseFloat($(this).find(".voucherEntry_credit").val() || 0);
			}
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
		setRowColor();
	}

	function setRowColor()
	{
		$("#voucher_entry_table tbody tr").each(function(index,element){
			console.log($(this).closest("tr").find(".entryStatus").val());
			if($(this).closest("tr").find(".entryStatus").val()==="true")
			{
				$(this).closest("tr").find(".voucherEntry_debit").css("background-color", "#4dff4d");
				$(this).closest("tr").find(".voucherEntry_credit").css("background-color", "#4dff4d");
				$(this).closest("tr").find(".voucherEntry_debit").prop("readonly", true);
				$(this).closest("tr").find(".select2_single").attr("readonly", true);
				$(this).closest("tr").find(".voucherEntry_credit").prop("readonly", true);
			}
		});
	}

	$("#submitForm").off().on("click", function (event) {

		// prevent double-click on submit
		/*if($("#submitForm").data('clicked')){
			return false;
		}

	    else{
	    	$("#submitForm").data('clicked', true);
	    }*/

		if($("#submitForm").prop("disabled")){
			return false;
		}

		$("#submitForm").prop("disabled", true);
		setTimeout(function(){
			$("#submitForm").prop("disabled", false);
		}, 20000);

		// Prevent the form from submitting via the browser.
		event.preventDefault();
		event.stopImmediatePropagation();
		// Will immediately show the confirmation popup
		$.confirm({
			title: "CONFIRMATION REQUIRED",
			content: "ARE YOU SURE YOU WANT TO SUBMIT THIS VOUCHER",
			buttons: {
				confirm: function () {
					// SUBMIT FORM
					$("#voucherForm").submit();
				},
				cancel: function () {
				}
			}
		});
	});

	// SUBMIT FORM
	$("#voucherForm").submit(function() {

		if ($("#voucherCode").val().length == 0){
			$.confirm({
				title: "ENCOUNTERED AN ERROR!",
				content: "PLEASE SELECT ALL PARAMETERS TO GENERATE VOUCHER CODE",
				type: "red",
				typeAnimated: true
			});
			// Prevent the form from submitting via the browser.
			return false;
		}

		// if ($("#voucherDate").val().length == 0 || !($("#voucherDate").val() >= $("#voucherDate").attr("min") && $("#voucherDate").val() <= $("#voucherDate").attr("max"))){
		// 	$.confirm({
		// 		title: "ENCOUNTERED AN ERROR!",
		// 		content: "VOUCHER DATE MUST LIES IN RANGE OF FINANCIAL YEAR",
		// 		type: "red",
		// 		typeAnimated: true
		// 	});
		// 	// Prevent the form from submitting via the browser.
		// 	return false;
		// }

		var branchEmpty = false;
		$("#voucher_entry_table tbody tr").each(function() {
			if (!$(this).find(".voucherEntry_branch").val() || $(this).find(".voucherEntry_branch").val() == 0){
				branchEmpty = true;
			}
		});

		if (branchEmpty){
			$.confirm({
				title: "ENCOUNTERED AN ERROR!",
				content: "PLEASE SELECT BRANCH FOR EACH VOUCHER ENTRY",
				type: "red",
				typeAnimated: true
			});
			// Prevent the form from submitting via the browser.
			return false;
		}

		var accountEmpty = false;
		$("#voucher_entry_table tbody tr").each(function() {
			if (!$(this).find(".voucherEntry_account").val() || $(this).find(".voucherEntry_account").val() == 0){
				accountEmpty = true;
			}
		});

		if (accountEmpty){
			$.confirm({
				title: "ENCOUNTERED AN ERROR!",
				content: "PLEASE SELECT ACCOUNT FOR EACH VOUCHER ENTRY",
				type: "red",
				typeAnimated: true
			});
			// Prevent the form from submitting via the browser.
			return false;
		}

		if ($("#voucher_entry_table span#debitTotal").text() !== $("#voucher_entry_table span#creditTotal").text()){
			$.confirm({
				title: "ENCOUNTERED AN ERROR!",
				content: "DEBIT AMOUNT DOES NOT MATCH CREDIT AMOUNT",
				type: "red",
				typeAnimated: true
			});
			// Prevent the form from submitting via the browser.
			return false;
		}


		if (($("#voucherForm #voucherType\\.id").val() == "OJV" || $("#voucherForm #voucherType\\.id").val() == "BRV" || $("#voucherForm #voucherType\\.id").val() == "CRV") && (!$("#voucherForm #voucher_id").val() || $("#voucherForm #voucher_id").val() === "0")){

			if ($("#voucherForm #voucher_entry_table tbody tr:first").find(".voucherEntry_credit").val() != "" && parseFloat($("#voucherForm #voucher_entry_table tbody tr:first").find(".voucherEntry_credit").val()) != 0){
				$.confirm({
					title: "ENCOUNTERED AN ERROR!",
					content: $("#voucherForm #voucherType\\.id").val() + " FIRST ENTRY MUST BE DEBIT ENTRY",
					type: "red",
					typeAnimated: true
				});
				// Prevent the form from submitting via the browser.
				return false;
			}

			var debit_count = 0;
			$("#voucherForm #voucher_entry_table tbody tr").each(function() {
				if ($(this).find(".voucherEntry_debit").val() != "" && parseFloat($(this).find(".voucherEntry_debit").val()) != 0) {
					debit_count = debit_count + 1;
				}
			});
			if (debit_count > 1){
				$.confirm({
					title: "ENCOUNTERED AN ERROR!",
					content: $("#voucherForm #voucherType\\.id").val() + " CANNOT HAVE MORE THAN ONE DEBIT ENTRY",
					type: "red",
					typeAnimated: true
				});
				// Prevent the form from submitting via the browser.
				return false;
			}
		}
		if (($("#voucherForm #voucherType\\.id").val() == "BPV" || $("#voucherForm #voucherType\\.id").val() == "CPV") && (!$("#voucherForm #voucher_id").val() || $("#voucherForm #voucher_id").val() === "0")){

			if ($("#voucherForm #voucher_entry_table tbody tr:first").find(".voucherEntry_debit").val() != "" && parseFloat($("#voucherForm #voucher_entry_table tbody tr:first").find(".voucherEntry_debit").val()) != 0){
				$.confirm({
					title: "ENCOUNTERED AN ERROR!",
					content: $("#voucherForm #voucherType\\.id").val() + " FIRST ENTRY MUST BE CREDIT ENTRY",
					type: "red",
					typeAnimated: true
				});
				// Prevent the form from submitting via the browser.
				return false;
			}

			var credit_count = 0;
			$("#voucherForm #voucher_entry_table tbody tr").each(function() {
				if ($(this).find(".voucherEntry_credit").val() != "" && parseFloat($(this).find(".voucherEntry_credit").val()) != 0) {
					credit_count = credit_count + 1;
				}
			});
			if (credit_count > 1){
				$.confirm({
					title: "ENCOUNTERED AN ERROR!",
					content: $("#voucherForm #voucherType\\.id").val() + " CANNOT HAVE MORE THAN ONE CREDIT ENTRY",
					type: "red",
					typeAnimated: true
				});
				// Prevent the form from submitting via the browser.
				return false;
			}
		}

		// prevent double submit
		if($("#voucherForm").data("submitted")){
			return false;
		}
		else{
			$("#voucherForm").data("submitted", true);
		}
	});

});