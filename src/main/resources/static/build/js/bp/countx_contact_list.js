Number.prototype.toFixed = function(decimalPlaces) {
	return Number(Math.round(this + 'e' + decimalPlaces) + 'e-' + decimalPlaces);
}
$(document).ready(function() {
	
	var buttionName ="";

	function getFormattedDate(date) {
		var year = date.getFullYear();
		var month = (1 + date.getMonth()).toString();
		month = month.length > 1 ? month : '0' + month;
		var day = date.getDate().toString();
		day = day.length > 1 ? day : '0' + day;
		return day + '/' + month + '/' + year;
	}
	
	function getSqlFormattedDate(date) {
		var year = date.split("/")[2];
		var month =date.split("/")[0];
		var day = date.split("/")[1];
		
		return day + '/' + month + '/' + year;
	}
	
	$("#city").on("change", function(){
		loadLinkedCities($(this));
	});
	
	function loadLinkedCities(thisControl) {
		
		if (!$(thisControl).val() || $(thisControl).val() == 0){
			return;
		}
		$("#linkedContact").empty();
		$("#linkedContact").append("<option>&emsp;</option>");
		//$(thisControl).closest("tr").find(".linkedcontanct").empty();
    	//$(thisControl).closest("tr").find(".linkedcontanct").append("<option>&emsp;</option>");
    	//console.log("Citiies " + $(thisControl).val());
	    $.get( "/bp/getContactByCity?city=" + $(thisControl).val(), function( data ) {
	        
	        for (var i = 0, len = data.length; i < len; i++) {
	        	var option = "<option value = " + data[i].id + ">" + data[i].firstName + "&emsp;" + data[i].lastName + "&emsp;" + data[i].companyName + "&emsp;" + data[i].city +  "</option>";
	        	$("#linkedContact").append(option);
	        	//$(thisControl).closest("tr").find(".linkedcontanct").append(option);
	    	}
	        
	        //$(thisControl).closest("tr").find(".itemDef").trigger("change.select2");
	        //$(thisControl).closest("tr").find(".itemDef").find("#inputhidden input.select2-input").trigger("input");
	    });
	}
	
	$("#selectAll").click(function(){
		$("#contact_report_maindiv").find('input[type=checkbox]').each(function () {
             // some staff
             this.checked = true;
        });
		/*$("#contact_report_datatable  tr").each(function(){
			  $(this).closest("tr").find(".checkBox").prop('checked',true);	
		});*/
	});
	$("#unSelectAll").click(function(){
		$("#contact_report_maindiv").find('input[type=checkbox]').each(function () {
            // some staff
            this.checked = false;
       });
		/*$("#contact_report_datatable  tr").each(function(index){
			 $(this).closest("tr").find(".checkBox").prop('checked', false);
		});*/
	});
	
	$('body').on('click','.send', function(){
		
		//var message = $(this).closest("tr").find("#message").val();
		//var contactId = $(this).closest("tr").find("#contactId").val();
		var contactId = $(this).attr("data-id");
		//console.log("Find Contact: " + contactId );
		var message = document.getElementById("message" + contactId).value;
		
		if(message.length<2)
		{
		  alert("Enter TEXT Minimum 2 char");
		  return false;
		}
		
		$.get("/bp/savedConversation?contactId="+contactId+"&conversation="+message,function(data)
		{
			$('#lastTalk'+ contactId).html(message);
			$("#message"+contactId).css('background-color', 'yellow');
			$("#message"+contactId).val("");
			
		});
	});
	
	$('body').on('click','.tdel', function(){
		alert('You click on tdel : ' + $(this).attr("data-id"));
		//return;
		//var message = $(this).closest("tr").find("#message").val();
		//var contactId = $(this).closest("tr").find("#contactId").val();
		var contactId = $(this).attr("data-id");
		//console.log("Find Contact: " + contactId );
		//var message = document.getElementById("message" + contactId).value;
		//$("textarea[id$='message'" + contactId + "]"); //document.getElementById("message" + contactId).val();
		//console.log("i m in send click event Contact: " + contactId + " Message: " + message);
		
		//var row = $(this).closest("tr");
		
		$.get("/bp/temporaryDeleteContact?contactId="+contactId, function(data)
		{
			alert('Temporary Deleted');
		});
	});
	
	$('body').on('click','.pdel', function(){
		alert('You click on pdel : ' + $(this).attr("data-id"));
		//return;
		//var message = $(this).closest("tr").find("#message").val();
		//var contactId = $(this).closest("tr").find("#contactId").val();
		var contactId = $(this).attr("data-id");
		
		$.get("/bp/permanentDeleteContact?contactId="+contactId,function(data)
		{
			$("#contact_card" + contactId).remove();
			
		});
	});
	
	$("#tdelall").click(function() {
		$("#startDeletion").show();
		var selectedContactList = [];
		var $list = $('#contact_report_maindiv input[id^="chk_"]');
		selectedContactList = [];
		var ControlCnt = $list.length;
		// Now loop through list of controls
		$list.each( function() {
			
		    var id = $(this).prop("id");      // get id
		    //console.log("Reading Loop " + id + " this value is " + $(this).is(':checkbox') );
		    var cbx = '';
		    //if ($(this).is(':checkbox')) {
		    if ($(this).prop('checked')) {
		    	contact = $(this).attr('data-id');
		    	selectedContactList.push(contact);
		        // Need to see if this control is checked
		    }
		    else { 
		        // Nope, not a checked control - so do something else
		    }
		});
		//console.log("Temp Delete All Selected Options Loop " + selectedContactList.length);

		if(selectedContactList.length === 0 ){
			alert("Please! Enter Select Contact For Temporary Deletion");
			$("#startDeletion").hide();
			return;
		}
		
		$.get("/bp/temporaryDeleteContact?contactId="+selectedContactList, function(data)
		{
			alert('Temporary Deleted');
			$("#startDeletion").hide();
		});

		//alert("Click on Temp All Delete");
	});
	
	$("#pdelall").click(function(){
		$("#startDeletion").show();
		var selectedContactList = [];
		var $list = $('#contact_report_maindiv input[id^="chk_"]');
		selectedContactList = [];
		var ControlCnt = $list.length;
		// Now loop through list of controls
		$list.each( function() {
			
		    var id = $(this).prop("id");      // get id
		    //console.log("Reading Loop " + id + " this value is " + $(this).is(':checkbox') );
		    var cbx = '';
		    //if ($(this).is(':checkbox')) {
		    if ($(this).prop('checked')) {
		    	contact = $(this).attr('data-id');
		    	selectedContactList.push(contact);
		        // Need to see if this control is checked
		    }
		    else { 
		        // Nope, not a checked control - so do something else
		    }
		});
		//console.log("Permanent Delete All Selected Options Loop " + selectedContactList.length);

		if(selectedContactList.length === 0 ){
			alert("Please! Select Contact For Temporary Deletion");
			$("#startDeletion").hide();
			return;
		}
		
		$.get("/bp/permanentDeleteContact?contactId="+selectedContactList, function(data)
		{
			alert('Permanent Deleted');
			$("#startDeletion").hide();
		});

		//alert("Click on Pemp All Delete");
	});
	
	$("#updateBusinessType").click(function(){
		//alert("Click on Update Business Type");
		$("#startDeletion").show();
		var selectedContactList = [];
		var busnissTypeList = [];
		var $list = $('#contact_report_maindiv input[id^="chk_"]');
		selectedContactList = [];
		var ControlCnt = $list.length;
		// Now loop through list of controls
		$list.each( function() {
			
		    var id = $(this).prop("id");      // get id
		    //console.log("Reading Loop " + id + " this value is " + $(this).is(':checkbox') );
		    var cbx = '';
		    //if ($(this).is(':checkbox')) {
		    if ($(this).prop('checked')) {
		    	contact = $(this).attr('data-id');
		    	selectedContactList.push({contactId:contact});
		        // Need to see if this control is checked
		    }
		    else { 
		        // Nope, not a checked control - so do something else
		    }
		});
		
		if(selectedContactList.length === 0 ){
			alert("Please! Select Contact For Business Type Updation");
			$("#startDeletion").hide();
			return;
		}
		
		var busType = $("#businessType").val();
		busnissTypeList.push({bsName:busType, ton:0, amount:0,itemCategory:{id:busType},});
		//console.log("busType:"+busType);
		//console.log("aaaa");
		var requestContacts = {
			
				requestContacts: selectedContactList,
				businessTypes:busnissTypeList,
				
        }
		console.log(requestContacts);
		$.ajax({
	         type : "POST",
            contentType : "application/json",
	         url:"/bp/addBusinessType",
	         dateType:"json",
	         data: JSON.stringify(requestContacts),
	        
	         success:function(data)
	         {
	        	 alert("Business Type Set Successfully.")
		          $("#startDeletion").hide(); 
	         }
		}); 

	});
	
	$("#updateCategory").click(function(){
		//alert("Click on Update Business Type");
		$("#startDeletion").show();
		var selectedContactList = [];
		var busnissTypeList = [];
		var $list = $('#contact_report_maindiv input[id^="chk_"]');
		selectedContactList = [];
		var ControlCnt = $list.length;
		// Now loop through list of controls
		$list.each( function() {
			
		    var id = $(this).prop("id");      // get id
		    //console.log("Reading Loop " + id + " this value is " + $(this).is(':checkbox') );
		    var cbx = '';
		    //if ($(this).is(':checkbox')) {
		    if ($(this).prop('checked')) {
		    	contact = $(this).attr('data-id');
		    	selectedContactList.push(contact);
		        // Need to see if this control is checked
		    }
		    else { 
		        // Nope, not a checked control - so do something else
		    }
		});
		
		if(selectedContactList.length === 0 ){
			alert("Please! Select Contact For Business Type Updation");
			$("#startDeletion").hide();
			return;
		}
		
		
		var requestContacts = {
			contactCategory:{id:$("#contactCategory").val(),name:""},
			contactId: selectedContactList,
        }
		console.log(requestContacts);
		$.ajax({
	         type : "POST",
            contentType : "application/json",
	         url:"/bp/updateContactCategory",
	         dateType:"json",
	         data: JSON.stringify(requestContacts),
	        
	         success:function(data)
	         {
	        	 alert("Contact Category Set Successfully.")
		          $("#startDeletion").hide(); 
	         }
		}); 

	});
	$("#sendingSms").click(function (){

		message = $("#mainmessage").val().trim();
		if(message === null || message.length <5 || message === ""){
			alert("Please! Enter SMS Text");
			$("#startSms").hide();
			$("#mainmessage").focus();
			return;
		}
		var formData = {
			companyName : $("#companyName").val(),
			lastName : $("#lastName").val(),
			firstName : $("#firstName").val(),
			designation : $("#designation").val(),
			mobileNumber : $("#mobileNumber").val(),
			linkedContact : $("#linkedContact").val(),
			city : $("#city").val(),
			smsText : $("#mainmessage").val(),
			businessType : $("#businessType").val(),
			contactCategory :{id: $("#contactCategory").val(),name:""},
		}
		$.confirm({
			title: "CONFIRMATION REQUIRED",
			content: "ARE YOU WANT TO SEND SMS TO SELECTED FILTER  ?",
			buttons: {
				confirm: function () {
					$("#startSms").show();
					$.ajax({
						type : "POST",
						contentType : "application/json",
						url:"/bp/sendSmscontactListByFilter",
						data : JSON.stringify(formData),
						dataType : "json",
						success:function(data)
						{
							$("#smsCost").text("Sms Cost:"+data.smsCost);
							$("#totalNo").text('====>'+" Total  No:"+data.totalNo);
							$("#totalNo").css('color',"green")
							$("#smsCost").css('color',"red")
							console.log("done Ijax Call");
							$("#startSms").hide();
						}
					});
				},
				cancel: function () {
					//console.log("Hello world!");
				},
			}
		});

	});
	$("#sendSms").click(function(){
		$("#startSms").show();
		var iii=0;
		var message ="";//$("#contact_report_datatable  tr:first").closest("tr").find("#message").val();
		var selectedContactList = [];

		message = $("#mainmessage").val();
		if(message === null || message.length === 0 || message === ""){
			alert("Please! Enter SMS Text");
			$("#startSms").hide();
			$("#mainmessage").focus();
			return;
		}
		
		var $list = $('#contact_report_maindiv input[id^="chk_"]');
		selectedContactList = [];
		var ControlCnt = $list.length;
		// Now loop through list of controls
		$list.each( function() {
			
		    var id = $(this).prop("id");      // get id
		  //  console.log("Reading Loop " + id + " this value is " + $(this).is(':checkbox') );
		    var cbx = '';
		    //if ($(this).is(':checkbox')) {
		    if ($(this).prop('checked')) {
		    	contact = $(this).attr('data-id');
		    	console.log("contact:"+contact);
		    	
		    	selectedContactList.push({contactId:contact});
		        // Need to see if this control is checked
		    }
		    else { 
		        // Nope, not a checked control - so do something else
		    }
		});
		if(selectedContactList.length === 0 ){
			alert("Please! Select Contact For SMS");
			$("#startSms").hide();
			return;
		}
	
         var formData = {
           requestContacts: selectedContactList,
           message:{text:message},
          
        }
        console.log(formData);
		 $.ajax({
			type : "POST",
			contentType : "application/json",
			url:"/bp/sendSmsToSelectedContact",
			data : JSON.stringify(formData),
			dataType : "json",
	      
	         success:function(data)
	         {
		          $("#smsCost").text("Sms Cost:"+data.smsCost);
		          $("#totalNo").text('====>'+" Total  No:"+data.totalNo);
		          $("#totalNo").css('color',"green")
		          $("#smsCost").css('color',"red")
		          console.log("done Ijax Call");
		          $("#startSms").hide();  
	         }
      }); 
		  
	});
	
	
	
	// SUBMIT FORM
    $("#contactListForm").submit(function(event) {
		// Prevent the form from submitting via the browser.
    	buttionName = $("#submitForm").text();
		event.preventDefault();
		generateContactListReportNew();
		//generateContactListReport();
	});
    
    function generateContactListReportNew(){
    	var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 2, maximumFractionDigits: 2});
    	$("#submitForm").attr("disabled", true);  // disable submit button 
    	$('#loading').show();  // show loading indicator
    	
    	$("#contact_report_maindiv").empty();
    	//$("#contact_report_datatable").DataTable().clear();
    	//$("#contact_report_datatable").DataTable().destroy();
    	
    	var formData = {
    		companyName : $("#companyName").val(),
    		lastName : $("#lastName").val(),
    		firstName : $("#firstName").val(),
    		designation : $("#designation").val(),
    		mobileNumber : $("#mobileNumber").val(),
    		linkedContact : $("#linkedContact").val(),
    		city : $("#city").val(),

			businessType : $("#businessType").val(),
			contactCategory :{id: $("#contactCategory").val(),name:""},
    	}
    	const videoGrids = document.getElementById("video-grids");
		//const videoGrid = document.createElement("div");
		console.log(formData);
		//console.log(videoGrids);
		
    	$.ajax({
			type : "POST",
			contentType : "application/json",
			url : $("#contactListForm").attr("action"),
			data : JSON.stringify(formData),
			dataType : "json",
			success:function(data){
				
				$.each(data,function (i,contact) {
					var ContactListed = "";
					var linkedContact = "";
				    var imageUrl = "";
				    var lastTalkDate ="";
				    var textarea = '<textarea rows="1" onblur="this.rows=1;" style="background-color: white; font-weight: bold; text-color: white;" id= "message' + contact.id + '" class ="form-control"></textarea>';
				    //var lastTalk = '<div id="lastTalk' + contact.id + '" class=" text-align: center;">' + contact.lastTalk + '</div> ';
				    var lastTalk = '' + contact.lastTalk + '';
				    var contactBusinessTypes = "";
				    //console.log("abc = " + textarea);
					 if(contact.conversition)
				     {
				       lastTalkDate = getSqlFormattedDate(new Date(contact.converDate).toLocaleString().split(",")[0]);
				       lastTalk =contact.lastTalk+"&emsp;"+"<a style='color:#009900;text:BOLD;'>" + lastTalkDate + "</b>"+"</a>";
				    //  console.log(lastTalkDate+" aa"+getSqlFormattedDate(new Date()));
				      var currentDate = new Date();
var date = currentDate.getDate();
var month = currentDate.getMonth()+1; 
var year = currentDate.getFullYear();
				       if(lastTalkDate===date+"/"+month+"/"+year)
				       {
					      textarea = '<textarea rows="1;" onblur="this.rows=1;" style="background-color: yellow;text:BOLD; text-color:white;" id= "message' + contact.id + '" class="form-control"></textarea>';
				        }
				       //smsParts[3]+":"+"<a style='color: #009900;text:BOLD;'>"+"<b>"+smsParts[4]+"</b>"+"</a>";
				     }
					imageUrl = contact.imageUrl;
					
					$.each(contact.likedContacts, function (j,linkedContactObj)
				     {
					     /*var firstname = "<a style='color: #009900;text:BOLD;'>" + "<b>" + linkedContactObj.firstName + "</b>" + "</a>";
					     var lastname = "<a style='color: #009900;text:BOLD;'>" + "<b>" + linkedContactObj.lastName + "</b>" + "</a>";
					     var companyName = "<a style='color: #ff1616;text:BOLD;'>" + "<b>" + linkedContactObj.companyName + "</b>" + "</a>";*/
						
						var firstname = "<b style='color: #009900; font-weight: bold;'>" + $.trim(linkedContactObj.firstName) + "</b>";
						var lastname ="<b style='color: #009900; font-weight: bold;'>" + $.trim(linkedContactObj.lastName) + "</b>";
						var companyName ="<b style='color: #ff1616; font-weight: bold;'>" + $.trim(linkedContactObj.companyName) + "</b>";
						//linkedContact = linkedContact + firstname + "&emsp;" + lastname + "&emsp;<br />" + companyName + "<br />";
						linkedContact = linkedContact + '<a style="text-decoration: underline; font-size: 12px;" target="_blank" href="' + location.protocol + '//' + location.host + '/bp/contact/' + linkedContactObj.id + '">' + firstname + "&nbsp;" + lastname + "</a><br />" + companyName + "<br />";
				     });
				     
					/*ContactListed += "<b>Mobile No. </b> 03221304445<br />";
					ContactListed += "<b>Mobile No. </b> 03221304445<br />";
					ContactListed += "<b>Mobile No. </b> 03221304445<br />";*/
					$.each(contact.contactNumbers, function (j,obj)
		    		{
						var num = obj.mobileNumber;
						//console.log(num);
						var a = num.split(" ");
						//console.log("Number " + obj.mobileNumber);
				    	ContactListed += "<b>" + obj.numberType + ":</b> <a target='_blank' href=https://wa.me/92"+a[a.length-1].trim()+">" + obj.mobileNumber + "</a><br />";
		    		});
					
					$.each(contact.busineesTypes, function (j,obj)
		    		{
					//	var num = obj.itemCategory.name;
					   if(obj.itemCategory!=null)
					   {
						contactBusinessTypes += obj.itemCategory.name + ' , ';
						console.log("B Type is " + obj.itemCategory.name);
						}
		    		});
					
					var mydiv = '<div class="contact_card" id="contact_card' + contact.id + '">';
					
					var tempBTN = ($("#temporaryContactDelete").val() === 'true' ? '<button type = "button" class = "btn btn-sm btn-warning tdel" id="tdel' + contact.id + '" data-id="' + contact.id + '">T-DEL</button>' : '');
					var perBTN = (($("#permanentContactDelete").val() === 'true' && contact.active == false) ? '<button type = "button" class = "btn btn-sm btn-danger pdel" id="pdel' + contact.id + '" data-id="' + contact.id + '">P-DEL</button>' : '');
					//console.log('ID IS ' + contact.id + ' My Rights ' + $("#permanentContactDelete").val() + ' My Active ' + contact.active);
					mydiv += '<div style="vertical-align: top;">';
					mydiv += '<img src="/img/' + imageUrl +'" onclick="zooMer(this.src);" style="cursor: pointer; width: 50px; height: 50px; float: left;  margin-right: 10px;" class="display_pic" />';
					mydiv += '<div style="margin: 5px;"><span id="serial_no">' + (i+1) + '</span>. <input type = "checkbox" id = "chk_' + contact.id + '" data-id = "' + contact.id + '" />&nbsp;<input type="hidden" id = "contactId" value = "' + contact.id + '" />&nbsp;';
					mydiv += '<a style="text-decoration: underline; font-size: 12px;" target="_blank" href="' + location.protocol + '//' + location.host + '/bp/contact/' + contact.id + '"><b>' + contact.firstName + '&nbsp;' + contact.lastName + '</b></a></div>';
					mydiv += '<div style="padding-top: 6px;">' + contact.companyName + '</div>';
					mydiv += '<div style="margin-left: 60px;background-color: white">' + ContactListed + '</div>';
					
					mydiv += '</div>';
					mydiv += '<div style="vertical-align: bottom; margin-top: 10px; margin-bottom: 10px;">';
					mydiv += '<div style="float: left;"><b>Linked Contacts:</b></div> <br /> <br />' + linkedContact;
					mydiv += '<div style="background-color: white; margin: 5px; padding: 8px;"><span style="color: #ff1616;">'+new Date(contact.converDate).toLocaleString()+' (Last Talked) </span><div style="float: right; margin: -6px; padding: 0;"> ' + tempBTN + ' ' + perBTN + '</div></div>';
					mydiv += '<div id="aditionalInfo' + contact.id + '" style="height: auto; min-height: 100% !important; width: 100% !important; word-wrap: break-word;">' + contact.additionalInfo +'</div>';
					mydiv += '<div id="lastTalk' + contact.id + '" style="height: auto; min-height: 100% !important; width: 100% !important; word-wrap: break-word;background-color: white">' + lastTalk +'</div>';
					mydiv += '<div><table cellpading="0" cellspacing="0" width="100%"><tbody><tr><td width="60%" style="padding: 5px 10px;">' + textarea +'</td><td width="25%" style="padding: 5px 10px;"><button type = "button" class = "btn btn-success send" data-id="' + contact.id + '">SAVED</button></td><td><button type = "button" class = "btn btn-success" style="background-color: ' + (contact.mstCustomer === true ? "white" : "yellow") + '; color: black;">C_A</button></td></tr></tbody></table></div>';
					mydiv += '</div>';
					
					mydiv += '<div style="padding-top: 6px;"><b>Business Type : </b><br />' + contactBusinessTypes + '</div>';
					mydiv += '<div style="padding-top: 6px;"><b>Contact : </b>' + (contact.contactCategory != null ? contact.contactCategory.name : "") + '</div>';
					
					mydiv += '</div>';
					//console.log(mydiv);
					
					$("#contact_report_maindiv").append(mydiv);
					
				});
				
				$("#submitForm").attr("disabled", false);  // enable submit button
		        $('#loading').hide();  // hide loading indicator
			},
            complete:function(){  
            	$("#submitForm").attr("disabled", false);  // enable submit button
		    	$('#loading').hide();  // hide loading indicator
            },
	    });
    }
        
    function generateContactListReport(){
    	
    	var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 2, maximumFractionDigits: 2});
    	
    	$("#submitForm").attr("disabled", true);  // disable submit button 
    	$('#loading').show();  // show loading indicator
    	
    	
    	$("#contact_report_datatable").DataTable().clear();
    	$("#contact_report_datatable").DataTable().destroy();
    	var table = $("#contact_report_datatable").DataTable({
	        dom: 'Bfrtip',

	        fixedHeader: true,
	        'bPaginate': false,
	        'scrollX': false,
	       buttons: [
	      
	        ],
	    	
	    });
    	
    	// PREPARE FORM DATA    
    	var formData = {
    		companyName : $("#companyName").val(),
    		lastName : $("#lastName").val(),
    		firstName : $("#firstName").val(),
    		designation : $("#designation").val(),
    		mobileNumber : $("#mobileNumber").val(),
    		linkedContact : $("#linkedContact").val(),
    		city : $("#city").val(),

    	}
    	console.log("aa000"+$("#businessType").val());
    	// DO POST
    	$.ajax({
			type : "POST",
			contentType : "application/json",
			url : $("#contactListForm").attr("action"),
			data : JSON.stringify(formData),
			dataType : "json",
			success:function(data){
				
				// FILL TABLE ROWS		
				$.each(data,function (i,contact) 
				{//console.log("asif");
				     var linkedContact = "";
				     
				     var imageUrl = "";
				     var lastTalkDate ="";
				     var textarea = '<div class="row"><div style="display: flex; justify-content: space-around; flex-direction: column; padding: 10px; 5px;"><textarea rows="3;" onblur="this.rows=1;" style="background-color: white; font-weight: bold; text-color:white;" id= "message" class ="form-control message"></textarea>';
				     var lastTalk ='<a id="lastTalk" class=" text-align: center;">'+contact.lastTalk+'</a> ';
				     if(contact.converDate.toLocaleString().length>0)
				     {
				       lastTalkDate = getSqlFormattedDate(new Date(contact.converDate).toLocaleString().split(",")[0]);
				       lastTalk =contact.lastTalk+"&emsp;"+lastTalkDate;
				       if(lastTalkDate===getFormattedDate(new Date()))
				       {
					      textarea = '<textarea  rows="1;"  onblur="this.rows=1;"  style="background-color: yellow;text:BOLD; text-color:white;" id= "message" class:form-control message></textarea>';
				        }
				       //smsParts[3]+":"+"<a style='color: #009900;text:BOLD;'>"+"<b>"+smsParts[4]+"</b>"+"</a>";
				     }
				     imageUrl = contact.imageUrl;
				    /* if(contact.defaultPic.length>0)
				        imageUrl = contact.defaultPic[0].pictureUrl;*/
				        
				    $.each(contact.likedContacts, function (j,linkedContactObj)
				     {
					     var firstname = "<a style='color: #009900;text:BOLD;'>" + "<b>" + linkedContactObj.firstName + "</b>" + "</a>";
					     var lastname = "<a style='color: #009900;text:BOLD;'>" + "<b>" + linkedContactObj.lastName + "</b>" + "</a>";
					     var companyName = "<a style='color: #ff1616;text:BOLD;'>" + "<b>" + linkedContactObj.companyName + "</b>" + "</a>";
					      linkedContact = linkedContact + firstname + "&emsp;" + lastname + "&emsp;" + companyName;
				     });
				    
				    
				    
				    
					// <textarea onfocus='this.rows=5;' onblur='this.rows=1;' rows='1;'  class='form-control ' required='required' th:field='${contact.companyName}'/></textarea>";
				    table.row.add([
					  '<td width="1%" style="width: 1%; min-width: 1%; max-width: 1%;"><span id="serial_no">' + i + '</span>' + '<input type="hidden" id = "contactId" value = ' + contact.id + '></td>',
					  '<td width="5%" style="width: 5%; min-width: 5%; max-width: 5%;"><img src=/img/' + imageUrl + ' onclick="zooMer(this.src);" style="cursor: pointer; width: 50px; height: 50px; float: left;" class="ImageIcon" /></td>',
					  '<td width="7%" style="width: 7%; min-width: 7%; max-width: 7%;">' + contact.firstName + "&emsp;" + contact.lastName + '</td>',           	
				 	  '<td width="8%" style="width: 8%; min-width: 8%; max-width: 8%;">' + contact.companyName +'</td>',
					  '<td width="8%" style="width: 8%; min-width: 8%; max-width: 8%;">' + linkedContact +'</td>',
					  '<td width="15%" style="width: 15%; min-width: 15%; max-width: 15%;">' + lastTalk + '</td>',
					  '<td width="10%" style="width: 10%; min-width: 10%; max-width: 10%;">' + textarea +'<br /><br /><button type = "button" class = "btn btn-success send">SAVED</button></div></div></td>',
					  '<td width="3%" style="width: 3%; min-width: 3%; max-width: 3%; text-align: center;"><input type = "checkbox" class = "form-control checkBox"><br /><br /><a class="btn btn-success" style="text-decoration: underline;" target="_blank" href="' + location.protocol + '//' + location.host + '/bp/contact/' + contact.id + '">EDIT</a></td>',
					]);
				    //console.log("Test Link", location.protocol,"//", location.host , "/bp/contact/", contact.id );
				});
				
		        table.draw();
		        
		       
		        $("#submitForm").attr("disabled", false);  // enable submit button
		        $('#loading').hide();  // hide loading indicator
            },
            complete:function(){  
            	$("#submitForm").attr("disabled", false);  // enable submit button
		    	$('#loading').hide();  // hide loading indicator
            },
	    });
    	
    }
    
    function getExcelBuilder(){
	    var xlsBuilder = {
			filename : 'SL_' + new Date().toLocaleString(),
			sheetName : 'sheet1',
			customize : function(xlsx) {
				var sheet = xlsx.xl.worksheets['sheet1.xml'];
				var downrows = 8;
				var clRow = $('row', sheet);
				var msg;
				// update Row
				clRow.each(function() {
					var attr = $(this).attr('r');
					var ind = parseInt(attr);
					ind = ind + downrows;
					$(this).attr("r", ind);
				});
	
				// Update row > c
				$('row c ', sheet).each(
						function() {
							var attr = $(this).attr('r');
							var pre = attr.substring(0, 1);
							var ind = parseInt(attr.substring(
									1, attr.length));
							ind = ind + downrows;
							$(this).attr("r", pre + ind);
						});
	
				function Addrow(index, data) {
	
					msg = '<row xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" r="'
							+ index + '">';
					for (var i = 0; i < data.length; i++) {
						var key = data[i].k;
						var value = data[i].v;
						msg += '<c t="inlineStr" r="' + key
								+ index + '">';
						msg += '<is>';
						msg += '<t>' + value + '</t>';
						msg += '</is>';
						msg += '</c>';
					}
					msg += '</row>';
					return msg;
				}
				var r1 = Addrow(1, [ {
					k : 'A',
					v : 'SALES REPORT' + ' ('
							+ getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim()
							+ ')'
				} ]);
				var r2 = Addrow(2, [ {
					k : 'A',
					v : 'COMPANY :'
				}, {
					k : 'B',
					v : $("#companyId option:selected").text()
				} ]);
				var r3 = Addrow(3, [ {
					k : 'A',
					v : 'BRANCH :'
				}, {
					k : 'B',
					v : $("#branchId option:selected").text()
				} ]);
				var r4 = Addrow(4, [ {
					k : 'A',
					v : 'VOUCHER STATUS :'
				}, {
					k : 'B',
					v : $("#voucherStatusId option:selected").text()
				} ]);
				var r5 = Addrow(5, [ {
					k : 'A',
					v : 'FINANCIAL YEAR :'
				}, {
					k : 'B',
					v : $("#financialYearId option:selected").text() + " (" + getFormattedDate(new Date($("#fromDate").val())) + " - " + getFormattedDate(new Date($("#toDate").val())) + ")"
				} ]);
				var r6 = Addrow(6, [ {
					k : 'A',
					v : 'ITEM :'
				}, {
					k : 'B',
					v : $("#itemDefId option:selected").text()
				} ]);
				var r7 = Addrow(7, [ {
					k : 'A',
					v : 'ACCOUNT :'
				}, {
					k : 'B',
					v : $("#accountCode option:selected").text()
				} ]);
	
				sheet.childNodes[0].childNodes[1].innerHTML = r1
						+ r2
						+ r3
						+ r4
						+ r5
						+ r6
						+ r7
						+ sheet.childNodes[0].childNodes[1].innerHTML;
			},
		/*
		 * exportOptions: { columns: [0, 1, 2, 3] }
		 */
		}
	    return xlsBuilder;
    }
    
    function userLog() {
		
		 var userLogObj ={
				idNumber : "",
				code: "",
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
			//	location.reload();
	          //  $("#customerAccount\\.balanceLimit").text(successmessage);
	        },
	        error: function(data) {
	            successmessage = 'Error';
	          //  $("#customerAccount\\.balanceLimit").text(successmessage);
	        },
	    });
	}
});