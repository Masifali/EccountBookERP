Number.prototype.toFixed = function(decimalPlaces) {
	return Number(Math.round(this + 'e' + decimalPlaces) + 'e-' + decimalPlaces);
}

$( document ).ready(function() {

	$("#voucher_company").prop("selectedIndex", 1);
	$("#voucher_status").prop("selectedIndex", 1);
	$("#search_by_date").prop("selectedIndex", 2);
	$('#from_date').val(new Date().toISOString().substring(0, 10));
	$('#to_date').val(new Date().toISOString().substring(0, 10));
	
	loadCompanyBranches();
	
	$("#voucher_company").on("change", function () {
		loadCompanyBranches();
    });
	
	
	function loadCompanyBranches() {
		
		if (!$("#voucher_company").val() || $("#voucher_company").val() == 0){
			return;
		}
	    
		$.get( "/vouchers/company_branches?companyId=" + $("#voucher_company").val(), function( data ) {
    		$("#company_branch").empty();
    		$("#company_branch").append("<option value='0'>ALL</option>");
            
            for (var i = 0, len = data.length; i < len; i++) {
            	var option = "<option value = " + data[i].id + ">" + data[i].name +  "</option>";
            	$("#company_branch").append(option);
        	}
            
            $("#company_branch").prop("selectedIndex", 1);
	    });
	}
	
    // SUBMIT FORM
    $("#voucherFilterForm").submit(function(event) {
		// Prevent the form from submitting via the browser.
		event.preventDefault();
		getVoucherList();
	});
    
    function getFormattedDate(date) {
		var year = date.getFullYear();
		var month = (1 + date.getMonth()).toString();
		month = month.length > 1 ? month : '0' + month;
		var day = date.getDate().toString();
		day = day.length > 1 ? day : '0' + day;
		return day + '/' + month + '/' + year;
	}
    
    function getVoucherList(){
    	
    	// disable submit button 
    	$("#getVoucherList").attr("disabled", true);
    	
    	var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 2, maximumFractionDigits: 2});
    	
    	var table = $('#voucher_datatable').DataTable({
	        dom: 'Bfrtip',
	        deferRender: true,
	        scrollY: 500,
	        scroller: true,
	        destroy: true,
	        /*fixedHeader: true,*/
	        bPaginate: true,
	        select: true,
	        buttons: [
	            { extend: 'copyHtml5', footer: true },
	            { extend: 'excelHtml5', footer: true },
	            {
	                extend: 'pdfHtml5',
	                download: 'open',
	                title: 'SALE VOUCEHRS - LOADING' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
	                messageBottom: null,
	                footer: true,
	                customize: function(doc){
	                	doc.defaultStyle.fontSize = 8;
	                	doc.pageMargins = [10, 10, 10, 10];
	                	doc.styles.tableHeader.fontSize = 8;
	                	doc.styles.tableFooter.fontSize = 8;
	                	doc.defaultStyle.alignment = 'left';
	                    doc.styles.tableHeader.alignment = 'left';
	                    doc.styles.tableFooter.alignment = 'left';
	                }
	            },
	            { 
	            	extend: 'print',
	            	title: 'SALE VOUCEHRS - LOADING' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
	                messageBottom: null,
	                footer: true,
	                autoPrint: false,
	                exportOptions: {
	                    stripHtml: false,
	                    stripNewlines: false
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
	            },
	        ],
	        footerCallback: function ( row, data, start, end, display ) {
	            var api = this.api(), data;
	            
	            var colNumber = [7, 8, 9];
	            
	            // Remove the formatting to get integer data for summation
	            var intVal = function ( i ) {
	                return typeof i === 'string' ?
	                    i.replace(/[\$,]/g, '')*1 :
	                    typeof i === 'number' ?
	                        i.toFixed(2) : 0;
	            };
	            
	            for (i = 0; i < colNumber.length; i++) {
	                var colNo = colNumber[i];
	                var total = api
	                        .column(colNo)
	                        .data()
	                        .reduce(function (a, b) {
	                            return intVal(a) + intVal(b);
	                        }, 0);
	                $(api.column(colNo).footer()).html(formatter.format(parseFloat(total).toFixed(2)));
	            }
	        },
	    });
    	
    	// PREPARE FORM DATA    
    	var formData = {
    		company : {id: $("#voucherFilterForm #voucher_company").val(), name: ""},
    		branch : {id: $("#voucherFilterForm #company_branch").val(), name: ""},
    		voucherType : {id: $("#voucherFilterForm #voucher_type").val(), name: ""},
    		voucherStatus : {id: $("#voucherFilterForm #voucher_status").val(), name: ""},
    		voucherCode : $("#voucherFilterForm #voucher_code").val(),
    		inventoryVoucherType : $("#voucherFilterForm #inventory_voucher_type").val(),
    		voucherNarration : $("#voucherFilterForm #voucher_narration").val(),
    		searchByDate: $("#voucherFilterForm #search_by_date").val(),
    		fromDate : $("#voucherFilterForm #from_date").val(),
    		toDate : $("#voucherFilterForm #to_date").val(),
    		postedUnPosted : $("#voucherFilterForm #voucher_posted").val(),
    	}
    	
    	// DO POST
    	$.ajax({
			type : "POST",
			contentType : "application/json",
			url : $("#voucherFilterForm").attr("action"),
			data : JSON.stringify(formData),
			dataType : "json",
			success:function(data){
				// FILL TABLE ROWS	            
				
				table.clear().draw();
				
		        $.each(data.voucherDetailList, function (i, voucher) {

		    		var voucherViewLink;
		    		if (voucher.voucherCode.indexOf("STV") >= 0){
		    			voucherViewLink = "<a style='text-decoration: underline;' target=_blank href="+location.protocol + "//" + location.host+"/stock_transfer_vouchers/"+voucher.id+">"+voucher.voucherCode+"</a>";
		    		}
		    		if (voucher.voucherCode.indexOf("PJV") >= 0){
		    			voucherViewLink = "<a style='text-decoration: underline;' target=_blank href="+location.protocol + "//" + location.host+"/purchase_journal_vouchers/"+voucher.id+">"+voucher.voucherCode+"</a>";
		    		}
		    		else if (voucher.voucherCode.indexOf("PRV") >= 0){
		    			voucherViewLink = "<a style='text-decoration: underline;' target=_blank href="+location.protocol + "//" + location.host+"/purchase_return_vouchers/"+voucher.id+">"+voucher.voucherCode+"</a>";
		    		}
		    		else if (voucher.voucherCode.indexOf("SJV") >= 0){
		    			voucherViewLink = "<a style='text-decoration: underline;' target=_blank href="+location.protocol + "//" + location.host+"/sale_journal_vouchers/"+voucher.id+">"+voucher.voucherCode+"</a>";
		    		}
		    		else if (voucher.voucherCode.indexOf("SRV") >= 0){
		    			voucherViewLink = "<a style='text-decoration: underline;' target=_blank href="+location.protocol + "//" + location.host+"/sale_return_vouchers/"+voucher.id+">"+voucher.voucherCode+"</a>";
		    		}
		    		else{
		    			voucherViewLink = "<a style='text-decoration: underline;' target=_blank href="+location.protocol + "//" + location.host+"/vouchers/"+voucher.id+">"+voucher.voucherCode+"</a>";
		    		}
		    		
		        	table.row.add([
                        (i+1),
                        voucherViewLink,
                        getFormattedDate(new Date(voucher.voucherDate)),
                        getFormattedDate(new Date(voucher.createdAt)) + ", " + new Date(voucher.createdAt).toLocaleString().split(",")[1].trim(),
                        getFormattedDate(new Date(voucher.updatedAt)) + ", " + new Date(voucher.updatedAt).toLocaleString().split(",")[1].trim(),
                        /*formatter.format(voucher.voucherAmount),*/
                        voucher.formattedCode,
                        voucher.accountName,
                        voucher.voucherWeight,
                        voucher.voucherLoadingCharges,
                        voucher.loadingCharges,
                    ]);
		        });
		        
		        table.draw();
		        // enable submit button
		        $("#getVoucherList").attr("disabled", false);
			},
            complete:function(){  
            	// enable submit button 
		    	$("#getVoucherList").attr("disabled", false);
		    	/*$("#voucher_datatable").DataTable( {
		    		destroy: true,
	    	        initComplete: function () {
	    	            this.api().columns([1, 2, 3, 4, 5, 6, 7]).every( function () {
	    	                var column = this;
	    	                var select = $('<select><option value="">SHOW ALL</option></select>')
	    	                    .appendTo( $(column.footer()).empty() )
	    	                    .on( 'change', function () {
	    	                        var val = $.fn.dataTable.util.escapeRegex(
	    	                            $(this).val()
	    	                        );
	    	 
	    	                        column
	    	                            .search( val ? '^'+val+'$' : '', true, false )
	    	                            .draw();
	    	                    } );
	    	 
	    	                column.data().unique().sort().each( function ( d, j ) {
	    	                    select.append( '<option value="'+d+'">'+d+'</option>' )
	    	                } );
	    	            } );
	    	        }
	    	    } );
		    	$("#voucher_datatable tfoot tr").appendTo("#voucher_datatable thead");*/

            },
	    });
    	
    	
    	//highlight
		/*$('#voucher_datatable tbody').on( 'mouseenter', 'td', function () {
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
        });*/
    	
    }
    
    
})