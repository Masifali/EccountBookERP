

$( document ).ready(function() {
	var buttionName ="";
	//instrument select2 dropdowns
	$(".select2_single").on("select2:close", function(){
		$(this).focus();
	});
	

	$(".entry_add").on("click", function () {
		
		//console.log($(this).find("td.ed(1)").val());
		// un-instrument select2 dropdowns
		$(this).closest("tr").find(".select2_single").select2("destroy");
		console.log("vv "+$(this).closest("tr").find(".bank").val());
		var newRow = $(this).closest("tr").clone(true);
		//$(this).find("td.ed(1)").val();
		//when you use clone, even the text is carried over. To remove it, do the following:
		newRow.children("td").children("input, span").each(function(index, element){
			if(index<2)
			$(element).val("0");
			else
				$(element).val("");
			$(element).text("");
		});
		newRow.children("td").each(function(index, element){
		//	console.log("a  "+$(this).children().text());
			if ($(this).children().length < 1 ) {
				$(element).text("");
			}//.prop("selectedIndex", 1);
			$(element).find(".bank:first").val("");
			//(element).find(".itemCategory:first").prop("selectedIndex",4);
			//$(element).find(".itemDef:first").val("");
		});
	    
	    newRow.insertAfter($(this).closest("tr"));
		
		// again instrument select2 dropdowns
		$("#bank_name_entry_table .select2_single").select2({dropdownAutoWidth : true, width: '100%'});
		
		// update serial numbers
		$("#bank_name_entry_table span#serial_no").each(function(index, element){
			$(element).text(index + 1);
			
			$(element).closest("tr").find( "input[name*='.id']" ).attr("id", "bankAccountEntries" + index + ".id");
			$(element).closest("tr").find( "input[name*='.id']" ).attr("name", "bankAccountEntries[" + index + "].id");
			
			$(element).closest("tr").find( "select[name*='.bankName']" ).attr("id", "bankAccountEntries" + index + ".bankName");
			$(element).closest("tr").find( "select[name*='.bankName']" ).attr("name", "bankAccountEntries[" + index + "].bankName");
			
			$(element).closest("tr").find( "input[name*='.accountTitle']" ).attr("id", "bankAccountEntries" + index + ".accountTitle");
			$(element).closest("tr").find( "input[name*='.accountTitle']" ).attr("name", "bankAccountEntries[" + index + "].accountTitle");
			
			$(element).closest("tr").find( "input[name*='.branchCode']" ).attr("id", "bankAccountEntries" + index + ".branchCode");
			$(element).closest("tr").find( "input[name*='.branchCode']" ).attr("name", "bankAccountEntries[" + index + "].branchCode");
			
			$(element).closest("tr").find( "input[name*='.accountNumber']" ).attr("id", "bankAccountEntries" + index + ".accountNumber");
			$(element).closest("tr").find( "input[name*='.accountNumber']" ).attr("name", "bankAccountEntries[" + index + "].accountNumber");
			
			$(element).closest("tr").find( "input[name*='.address']" ).attr("id", "bankAccountEntries" + index + ".address");
			$(element).closest("tr").find( "input[name*='.address']" ).attr("name", "bankAccountEntries[" + index + "].address");
	 
	    });
		
		generateTabIndexing();
		
	});
	
	$(".entry_delete").on("click", function () {
		//event.preventDefault();
		if($(this).parents("tbody").find("tr").length > 1)
		{
			$(this).closest("tr").remove();
		}
		
		// update serial numbers
		$("#bank_name_entry_table span#serial_no").each(function(index, element){
			$(element).text(index + 1);
			
			$(element).closest("tr").find( "input[name*='.id']" ).attr("id", "bankAccountEntries" + index + ".id");
			$(element).closest("tr").find( "input[name*='.id']" ).attr("name", "bankAccountEntries[" + index + "].id");
			
			$(element).closest("tr").find( "select[name*='.bankName']" ).attr("id", "bankAccountEntries" + index + ".bankName");
			$(element).closest("tr").find( "select[name*='.bankName']" ).attr("name", "bankAccountEntries[" + index + "].bankName");
			
			$(element).closest("tr").find( "input[name*='.accountTitle']" ).attr("id", "bankAccountEntries" + index + ".accountTitle");
			$(element).closest("tr").find( "input[name*='.accountTitle']" ).attr("name", "bankAccountEntries[" + index + "].accountTitle");
			
			$(element).closest("tr").find( "input[name*='.branchCode']" ).attr("id", "bankAccountEntries" + index + ".branchCode");
			$(element).closest("tr").find( "input[name*='.branchCode']" ).attr("name", "bankAccountEntries[" + index + "].branchCode");
			
			$(element).closest("tr").find( "input[name*='.accountNumber']" ).attr("id", "bankAccountEntries" + index + ".accountNumber");
			$(element).closest("tr").find( "input[name*='.accountNumber']" ).attr("name", "bankAccountEntries[" + index + "].accountNumber");
			
			$(element).closest("tr").find( "input[name*='.address']" ).attr("id", "bankAccountEntries" + index + ".address");
			$(element).closest("tr").find( "input[name*='.address']" ).attr("name", "bankAccountEntries[" + index + "].address");
	     });
		
		generateTabIndexing();
		
	});
	$("#account\\.code").on("change", function(){
		console.log("Asif");
		//loadBankAccountByCode();
	});
	
	function loadBankAccountByCode() {
		
//		$('#bank_name_entry_table').DataTable().clear();
//    	$('#bank_name_entry_table').DataTable().destroy();
//		if (!$("#account\\.code") || $("#account\\.code").val() == 0){
//			return;
//		}
//	    
//	    $.get("/bank_account/by_code?code=" + $("#account\\.code").val(), function( data ) {
//	    
//	    	var bankAccountTable =  $("#bank_name_entry_table").DataTable({
//	    		destroy: true,
//	    		pageLength: 100,
//	    	});
//	    	bankAccountTable.clear().draw();
//	    	var voucherViewLink = "<a style='text-decoration: underline;' target=_blank href="+location.protocol + "//" + location.host+"/vouchers/"+">"+"</a>";
//	        $.each(data.bankAccountEntry, function (i, bankAccountEntry) {
//	        	 voucherViewLink = "<a style='text-decoration: underline;' target=_blank href="+location.protocol + "//" + location.host+"/vouchers/"+">"+"</a>";
//	        	bankAccountTable.row.add([
//                  i+1,
//                  '<td><input type="text"  id=' + bankAccountEntry.bankName.name  + '" class="form-control" value="' + bankAccountEntry.bankName.name + '"/></td>',
//                  '<td><input type="text"  id=' + bankAccountEntry.bankName.name  + '" class="form-control" value="' + bankAccountEntry.bankName.name + '"/></td>',
//                  '<td><input type="text"  id=' + bankAccountEntry.bankName.name  + '" class="form-control" value="' + bankAccountEntry.bankName.name + '"/></td>',
//                  bankAccountEntry.branchCode,
//                  bankAccountEntry.accountNumber,
//                 // bankAccountEntry.address,
//                 '<td> <button' '"class="btn btn-info entry_add"'  href="javascript:void(0)"'>Add'"</button></td>',
//                ]).draw();
//	        });
//	    });
	   // $("#loading").hide();
	 
	}
	
//	$("#submitForm").off().on("click", function (event) {
//		
//		$("#submitForm").addClass('disabled');
//	    setTimeout(function(){
//	    	$("#submitForm").removeClass('disabled');
//	    }, 5000);
//	    
//		// Prevent the form from submitting via the browser.
//		event.preventDefault();
//		event.stopImmediatePropagation();
//		// Will immediately show the confirmation popup
//		$.confirm({
//    	    title: "CONFIRMATION REQUIRED",
//    	    content: "ARE YOU SURE YOU WANT TO SUBMIT THIS PURCHASE ORDER",
//    	    buttons: {
//    	        confirm: function () {
//    	        	// SUBMIT FORM
//    	        	buttionName =$("#submitForm").text();
//    	        	userLog();
//    		    	$("#bankAccountForm").submit();
//    	        },
//    	        cancel: function () {
//    	        },
//    	    }
//    	});
//	});
//    
    // SUBMIT FORM
	$("#bankAccountForm").submit(function() {
		
		if (!$("#account\\.code").val() || $("#account\\.code").val() == 0){
			$.confirm({
			    title: "ENCOUNTERED AN ERROR!",
			    content: "PLEASE SELECT SUPPLIER ACCOUNT",
			    type: 'red',
			    typeAnimated: true,
			});
    		// Prevent the form from submitting via the browser.
    		return false;
    	}
		
		var itemEmpty = false;
		var itemAmountZero = false;
		var itemDuplicateExists = false;
		var itemDuplicate = "";
		var itemArray = [];
		var fromcompany =false;
		$("#bank_name_entry_table tbody tr").each(function() {
			
//			if (parseFloat($(this).find(".amount").text()) == 0 ){
//				itemAmountZero = true;
//	    	}
//			if($(this).find(".itemCategory").val() == 12 && $("#fromCompany").val() == 0)
//			{
//				fromcompany  =true;
//			}
//			if ($(this).find(".itemDef").val() && $.inArray($(this).find(".itemDef").val(), itemArray) >= 0){
//				itemDuplicateExists = true;
//				itemDuplicate = $(this).find(".itemDef option:selected").text();
//			}
//			
//			else{
//				itemArray.push($(this).find(".itemDef").val());
//			}
			
		});
		
		if (itemEmpty){
			$.confirm({
			    title: "ENCOUNTERED AN ERROR!",
			    content: "PLEASE SELECT ITEM CAT/DEF FOR EACH ROW",
			    type: 'red',
			    typeAnimated: true,
			});
    		// Prevent the form from submitting via the browser.
    		return false;
    	}
		
		if (itemDuplicateExists){
			$.confirm({
			    title: "ENCOUNTERED AN ERROR!",
			    content: "DUPLICATE ITEM EXISTS: " + itemDuplicate,
			    type: 'red',
			    typeAnimated: true,
			});
    		// Prevent the form from submitting via the browser.
    		return false;
    	}
		
		// prevent double submit
		if($("#bankAccountForm").data("submitted")){
			return false;
		}
	    else{
	    	$("#bankAccountForm").data("submitted", true);
	    }
	});
    
	generateTabIndexing();
	
	function generateTabIndexing() {
		var tabIndex = 0;
		$("#bank_name_entry_table tbody td").each(function (i) {
			if ($(this).find("select.select2_single").length){
				tabIndex = tabIndex + 1;
				$(this).find("select.select2_single:eq(0)").attr('tabindex', tabIndex);

			}
			else if ($(this).find("input:not(:hidden)").length){
				tabIndex = tabIndex + 1;
				$(this).find("input:not(:hidden):eq(0)").attr('tabindex', tabIndex);

			}
			else if ($(this).find("a").length){
				tabIndex = tabIndex + 1;
				$(this).find("a:eq(0)").attr('tabindex', tabIndex);

			}
		});
	}
	
	$("input[type=number]").on("focus", function() {
	    $(this).on("keydown", function(event) {
	        if (event.keyCode === 38 || event.keyCode === 40) {
	            event.preventDefault();
	        }
	     });
	 });
	
	$('input[type=number]').on('wheel', function(e){
	    return false;
	});
	
	$(document).keydown(function(e) {

	  // Set self as the current item in focus
	  var self = $(':focus'),
	      // Set the form by the current item in focus
	      form = self.parents('form:eq(0)'),
	      focusable;

	  // Array of Indexable/Tab-able items
	  //focusable = form.find('input').filter(':visible');
	  focusable = form.find(':input:enabled:not([readonly], input:hidden, button:hidden, textarea:hidden), textarea:enabled:not([readonly] textarea:hidden), a.entry_add')
      .not(function () {   // do not include inputs with hidden parents
          return $(this).parent().is(':hidden');
      });

	  function enterKey(){
	    if (e.which === 13 && !self.is('textarea,div[contenteditable=true]')) { // [Enter] key

	      // If not a regular hyperlink/button/textarea
	      if ($.inArray(self, focusable) && (!self.is('button'))){
	        // Then prevent the default [Enter] key behaviour from submitting the form
	        e.preventDefault();
	      } // Otherwise follow the link/button as by design, or put new line in textarea
	      
	      if(self.is('a.entry_add')){
	    	  self.click();
	    	  focusable = form.find(':input:enabled:not([readonly], input:hidden, button:hidden, textarea:hidden), textarea:enabled:not([readonly] textarea:hidden), a.entry_add')
	          .not(function () {   // do not include inputs with hidden parents
	              return $(this).parent().is(':hidden');
	          });
	      }
	    		  
	      // Focus on the next item (either previous or next depending on shift)
	      if (comingFromSelect2){
	    	  focusable.eq(focusable.index(self) + (e.shiftKey ? -1 : 0)).focus();
	    	  comingFromSelect2 = false;
	      }
	      else{
	    	  if (focusable.index(self) < 0){
	    		  return false;
	    	  }
	    	  focusable.eq(focusable.index(self) + (e.shiftKey ? -1 : 1)).focus();
	      }
	      //focusable.eq(focusable.index(self) + (e.shiftKey ? -1 : 1)).focus();

	      return false;
	    }
	  }
	  // We need to capture the [Shift] key and check the [Enter] key either way.
	  if (e.shiftKey) { enterKey() } else { enterKey() }
	});
    
	 function userLog() {
			
		 var userLogObj ={
				idNumber : $("#purchaseOrderId").val(),
				code: $("#purchaseOrderCode").val(),
				buttonClick: buttionName,
				
				windowName:$('h2').html(),
			}
		 
	    $.ajax({
			type : "POST",
			contentType : "application/json",
			url : "/viewSaleOrder",
			data : JSON.stringify(userLogObj),
			dataType : "json",
			success:function(data){
				successmessage = 'Data was succesfully captured';
				//location.reload();
	          //  $("#customerAccount\\.balanceLimit").text(successmessage);
	        },
	        error: function(data) {
	            successmessage = 'Error';
	          //  $("#customerAccount\\.balanceLimit").text(successmessage);
	        },
	    });
	}
		
});