$(document).ready(function() {
	
	//on load
	restrictFromAndToDate();
	//loadCompanyBranches();
	
	$("#companyId").on("change", function () {
		loadCompanyBranches();
    });
	
	function loadCompanyBranches() {
		
		if (!$("#companyId").val() || $("#companyId").val() == 0){
			return;
		}
	    
		$.get( "/vouchers/company_branches?companyId=" + $("#companyId").val(), function( data ) {
    		$("#branchId").empty();
    		$("#branchId").append("<option value='0'>ALL</option>");
            
            for (var i = 0, len = data.length; i < len; i++) {
            	var option = "<option value = " + data[i].id + ">" + data[i].name +  "</option>";
            	$("#branchId").append(option);
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
	
	$("#show_sms_modal").on("click", function(){
		if (!$("#accountCode").val() || $("#accountCode").val() == 0){
			$.confirm({
			    title: "ENCOUNTERED AN ERROR!",
			    content: "PLEASE SELECT AN ACCOUNT",
			    type: 'red',
			    typeAnimated: true,
			});
    		return false;
		}	
		$("#sendSMSModal h4.modal-title").text($("#accountCode option:selected").text());
		//$("#sendSMSModal #recipient_number").val($("#accountCode option:selected").attr("data-mobile"));
		
		$("#recipient_number").val($("#accountCode option:selected").attr("data-mobile").split(",")[0].replace("+92", "0").trim());
		
		$("#recipient_numbers").empty();
		$.each($("#accountCode option:selected").attr("data-mobile").split(","), function(i, item) {
			$("#recipient_numbers").append($("<option>").text(item.replace("+92", "0").trim()));
		});
		
		var message_val;
		if ($("#gl_report_datatable tbody tr:eq(-1) td:eq(2)").text().trim().indexOf("SJV") !== -1){
			message_val = $("#sendSMSModal #message_val2").val();
			message_val = message_val.replace("PRE_DATE", $("#gl_report_datatable tbody tr:eq(-2) td:eq(1)").text().split("\n")[0].trim());
			message_val = message_val.replace("PRE_BAL", $("#gl_report_datatable tbody tr:eq(-2) td:eq(-1)").text().split(".")[0].trim());
			message_val = message_val.replace("DATE", $("#gl_report_datatable tbody tr:eq(-1) td:eq(1)").text().split("\n")[0].trim());
			message_val = message_val.replace("AMOUNT", $("#gl_report_datatable tbody tr:eq(-1) td:eq(-3)").text().split(".")[0].trim());
			message_val = message_val.replace("BALANCE", $("#gl_report_datatable tbody tr:eq(-1) td:eq(-1)").text().split(".")[0].trim());
		}
		else{
			message_val = $("#sendSMSModal #message_val1").val();
			message_val = message_val.replace("PRE_DATE", $("#gl_report_datatable tbody tr:eq(-2) td:eq(1)").text().split("\n")[0].trim());
			message_val = message_val.replace("PRE_BAL", $("#gl_report_datatable tbody tr:eq(-2) td:eq(-1)").text().split(".")[0].trim());
			message_val = message_val.replace("DATE", $("#gl_report_datatable tbody tr:eq(-1) td:eq(1)").text().split("\n")[0].trim());
			message_val = message_val.replace("AMOUNT", $("#gl_report_datatable tbody tr:eq(-1) td:eq(-2)").text().split(".")[0].trim());
			message_val = message_val.replace("BALANCE", $("#gl_report_datatable tbody tr:eq(-1) td:eq(-1)").text().split(".")[0].trim());
		}
		
		$("#sendSMSModal #message_text").val(message_val);
		
		/*$.get( "/receivables/account_limit_and_balance?accountCode=" + $("#customerAccount\\.code").val(), function( data ) {
			$("input#customerAccount\\.balanceLimit").val(data.accountBalanceLimit);
			$("span#previousBalance").text(formatter.format(data.accountBalance));
	    	calculateTotal();
	    });*/
	});
	
	$("#send_sms").on("click", function(){
		
		$.ajax({
			   url: "http://192.168.1.50:8080/SMSGateway/SMSSender.jsp?mobile_no="+$("#recipient_number").val()+"&message="+$("#message_text").val()+"&owner_number=03166300000&partyName="+$("#accountCode option:selected").text().substring(14).trim()+"&userName="+$("#user_name").val(),
			   headers: {"Access-Control-Allow-Origin": "*", "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept"},
			   type: 'GET',
			   dataType: 'jsonp',
			   crossDomain: true,
			   success: function(data){
				   $.confirm({
					    title: "SUCCESS!",
					    content: "MESSAGE DELIVERED",
					    type: 'green',
					    typeAnimated: true,
					});
			   },
			   complete: function(){
				   $("#sendSMSModal").modal("toggle");
				   $.confirm({
					    title: "SUCCESS!",
					    content: "MESSAGE DELIVERED",
					    type: 'green',
					    typeAnimated: true,
					});
			   }
		});
			
		/*$.get("http://192.168.1.50:8080/SMSGateway/SMSSender.jsp?mobile_no=03000332488&message="+$("#sendSMSModal #message_text").text()+"&owner_number=03004076533&partyName=saif&userName=saif", function(data) {
	    	$.each(data.module, function(i, record){
	        	var option = "<option data-id = " + record.id + " value = " + record.name + ">" + record.name + "</option>";
	        	$("#province").append(option);
	        });
	    });*/
	});
	
	function getFormattedDate(date) {
		var year = date.getFullYear();
		var month = (1 + date.getMonth()).toString();
		month = month.length > 1 ? month : '0' + month;
		var day = date.getDate().toString();
		day = day.length > 1 ? day : '0' + day;
		return day + '/' + month + '/' + year;
	}
	
	// SUBMIT FORM
    $("#glReportForm").submit(function(event) {
		// Prevent the form from submitting via the browser.
		event.preventDefault();
		generateGLReport();
	});
        
    function generateGLReport(){
    	
    	$("#submitGLForm").attr("disabled", true);  // disable submit button 
    	$('#loading').show();  // show loading indicator
    	
    	var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 2, maximumFractionDigits: 2});
    	
    	if (!$("#companyId").val() || !$("#voucherStatusId").val() || !$("#financialYearId").val() || !$("#fromDate").val() || !$("#toDate").val()){
			alert("PLEASE SELECT ALL THE FIELDS")
    		return;
		}
    	
    	$('#gl_report_datatable').DataTable().clear();
    	$('#gl_report_datatable').DataTable().destroy();
    	var table = $('#gl_report_datatable').DataTable({
	        dom: 'Bfrtip',
	        'bPaginate': false,
	        destroy: true,
	        buttons: [
	            {
	                extend: 'copy',
	                text: 'COPY',
	                title: null,
	                footer: true,
	                exportOptions: {
	                	columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
	                },
	            },
	            {
	                extend: 'excelHtml5',
	                title: 'SUPPLIER STATUS REPORT' + ' (' + (new Date().toLocaleString()) + ')',
	                messageTop: null,
	                messageBottom: null,
	                footer: true,
	            	exportOptions: {
	                    stripNewlines: false,
	                    columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
	                },
	            },
	            {
	                extend: 'pdfHtml5',
	                download: 'open',
	                title: 'SUPPLIER STATUS REPORT' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
	                 messageTop: 'COMPANY: ' + $("#companyId option:selected").text()
	                + '  ' + 'BRANCH: ' + $("#branchId option:selected").text()
	                + '  ' + 'ITEM: ' + $("#itemDefId option:selected").text()
	                + '   ' + 'ACCOUNT: ' + $("#accountCode option:selected").text()
	                + ' ' + 'FINANCIAL YEAR: ' + $("#financialYearId option:selected").text() + " (" + getFormattedDate(new Date($("#fromDate").val())) + " - " + getFormattedDate(new Date($("#toDate").val())) + ")"
	               ,
	                messageBottom: null,
	                footer: true,
	                exportOptions: {
	                	columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
	                    stripNewlines: false,
	                },
	                customize: function(doc){
	                	doc.defaultStyle.fontSize = 6;
	                	doc.pageMargins = [10, 10, 10, 10];
	                	doc.styles.tableHeader.fontSize = 6;
	                	doc.styles.tableFooter.fontSize = 6;
	                	doc.defaultStyle.alignment = 'left';
	                    doc.styles.tableHeader.alignment = 'left';
	                    doc.styles.tableFooter.alignment = 'left';
	                }
	            },
	            {
	                extend: 'print',
	                text: 'VIEW',
	                title: 'SUPPLIER STATUS REPORT' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
	                messageTop: null,
	                messageBottom: null,
	                footer: true,
	                autoPrint: false,
	                exportOptions: {
	                    stripHtml: false,
	                    stripNewlines: false,
	                    columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
	                },
	                customize: function (win) {
	                    $(win.document.body).find('table').addClass('display').css('font-size', '12px');
	                    $(win.document.body).find('tr:nth-child(odd) td').each(function(index){
	                        $(this).css('background-color','WHITESMOKE');
	                    });
	                    $(win.document.body).find('tr:nth-child(even) td').each(function(index){
	                        $(this).css('background-color','WHITE');
	                    });
	                    
	                    $(win.document.body).css('background-color','WHITE');
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
	        "footerCallback": function ( row, data, start, end, display ) {
	            var api = this.api(), data;
	            
	            var colNumber = [3, 4, 5];
	            
	            // Remove the formatting to get integer data for summation
	            var intVal = function ( i ) {
	            	if (typeof i === 'string'){
	            		return i.replace(/[\$,]/g, '')*1;
	            	}
	            	else if (typeof i === 'number'){
	            		return parseFloat(i);
	            	}
	            	else{
	            		return parseFloat(0);
	            	}
	            };
	            
	            for (i = 0; i < colNumber.length; i++) {
	                var colNo = colNumber[i];
	                var total = api
	                        .column(colNo)
	                        .data()
	                        .reduce(function (a, b) {
	                            return intVal(a) + intVal(b);
	                        }, 0);
	                //$(api.column(colNo).footer()).html(parseFloat(total).toFixed(2));
	                $(api.column(colNo).footer()).html(formatter.format(parseFloat(total).toFixed(2)));
	            }
	        }
	    });
    	
    	// PREPARE FORM DATA    
    	var formData = {
    		companyId : $("#companyId").val(),
    		branchId : $("#branchId").val(),
    		voucherStatusId : $("#voucherStatusId").val(),
    		financialYearId : $("#financialYearId").val(),
    		fromDate : $("#fromDate").val(),
    		toDate : $("#toDate").val(),
    	}
    	
    	// DO POST
    	$.ajax({
			type : "POST",
			contentType : "application/json",
			url : $("#glReportForm").attr("action"),
			data : JSON.stringify(formData),
			dataType : "json",
			success:function(data){
				
				// FILL TABLE ROWS				
		        $.each(data, function (i, gLEntry) {
		        	
		    		table.row.add([
                        (i+1),
                        gLEntry.accountFormattedCode,
                        gLEntry.accountName,
                        formatter.format(gLEntry.accountBalance),
                        formatter.format(gLEntry.accountLimit),
                        formatter.format(gLEntry.accountBalanceDiff),
                        gLEntry.accountContactPerson,
                        gLEntry.accountContactMobile,
                        gLEntry.accountContactPhone,
                        gLEntry.accountContactAddress,
                        gLEntry.accountContactCity,
                    ]);
		        });
		        
		        table.draw();
		        $("#submitGLForm").attr("disabled", false);  // enable submit button
		        $('#loading').hide();  // hide loading indicator
            },
            complete:function(){  
            	$("#submitGLForm").attr("disabled", false);  // enable submit button
		    	$('#loading').hide();  // hide loading indicator
            },
	    });
    	
    	//highlight
		$('#gl_report_datatable tbody').on( 'mouseenter', 'td', function () {
			if (table instanceof $.fn.dataTable.Api) {
				table.rows().eq(0).each(function (index) {
	    			$(table.row(index).nodes()).removeClass('highlight');
	    		});
	    		$(table.cells().nodes()).removeClass('highlight');
	    		
	    		var rowIdx = table.cell(this).index().row;
	            var colIdx = table.cell(this).index().column;
	            
	            $(table.row(rowIdx).nodes()).addClass('highlight');
	            $(table.column(colIdx).nodes()).addClass('highlight');
			}
        });
    }
	
});