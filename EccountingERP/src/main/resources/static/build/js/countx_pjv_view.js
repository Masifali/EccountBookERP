$( document ).ready(function() {
	
	$('#purchase_journal_voucher_entry_table').DataTable( {
        dom: 'Bfrtip',
        buttons: [
        	{
                extend: 'print',
                text: 'PRINT',
                title: '',
                messageTop: $(".x_panel:eq(0)").html() + $(".x_panel:eq(1) .x_content:first").html() + "<br />",
                messageBottom: $("#po_footer").html(),
                footer: true,
                autoPrint: false,
                exportOptions: {
                    stripHtml: false,
                    stripNewlines: false,
                    columns: [ 0, 1, 2, 4, 5, 6, 7, 8, 9, 10, 11, 12],
                },
                customize: function (win) {
                    $(win.document.body).find('table').addClass('display').css('font-size', '12px');
                    $(win.document.body).css('background-color','WHITE');
                    $(win.document.body).css("color", "DIMGRAY");
                    /*$(win.document.body).find('h1').css('text-align', 'center');
                    $(win.document.body).find('div:first').css('text-align', 'center');*/
                    
                }
            },
            
            {
                extend: 'print',
                text: 'PRINT INVOICE',
                title: '',
                messageTop: $(".x_panel:eq(0)").html() + "<br />",
                messageBottom: $("#po_footer").html(),
                footer: true,
                autoPrint: false,
                exportOptions: {
                    stripHtml: false,
                    stripNewlines: false,
                    columns: [ 0, 1, 2, 4, 6, 7, 9, 12],
                },
                customize: function (win) {
                	$(win.document.body).find('table:eq(1) tbody tr td,th').addClass('display').css('padding', '1px 2px 1px 2px');
                    $(win.document.body).find('table:eq(1) tbody tr td,th').addClass('display').css('vertical-align', 'middle');
                    $(win.document.body).find('table').addClass('display').css('font-size', '14px');
                    $(win.document.body).css('background-color','WHITE');
                    $(win.document.body).css("color", "DIMGRAY");
                    /*$(win.document.body).find('h1').css('text-align', 'center');
                    $(win.document.body).find('div:first').css('text-align', 'center');*/
                    
                }
            }
        ],
        bPaginate: false,
        bFilter: false,
        bInfo: false,
    });
	
	
	$("#post_voucher").on("click", function (event) {
		// Prevent the form from submitting via the browser.
		event.preventDefault();
		// Will immediately show the confirmation popup
		$.confirm({
    	    title: "CONFIRMATION REQUIRED",
    	    content: "ARE YOU SURE YOU WANT TO "+$("#post_voucher").text()+"?",
    	    buttons: {
    	        confirm: function () {
    	        	postThisVoucher();
    	        },
    	        cancel: function () {
    	        },
    	    }
    	});
	});
	
	function postThisVoucher() {
		voucherId = $("#voucher_id").val();
	    $.get( "/purchase_journal_vouchers/post_voucher?voucherId="+voucherId, function(data) {
	    	location.reload();
	    });
	}
	
});