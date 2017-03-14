$(document).ready(function () {
    TicketingSystem.init();
    TicketingSystem.registerEvents();
    Utility.hideLoading();

});
var User = function () {
    this.Name = "";
    this.Email = "";
    this.PhoneNumber = "";
    this.ID;
    this.EmployeeId = "";
    this.IsActive = "";
    this.Supervisor = "";
    this.IsExternalUser = "";
}

var TicketInputForm = function () {
    //this.isSelf = true;
    this.RequestedBy = {};
    //this.RequestedFor = new User();
    this.Title = "";
    this.Description = "";
    this.EmailsToNotify = "";
    this.Attachments = [];
}

var Attachment = function () {
    this.ID;
    this.FileName = "";
    this.FileUrl = "/";
    this.UploadedBy = new User();
    this.TicketId;
}
var TicketingSystem = {
    ticket: new TicketInputForm(),
    files: [],
    hostURL: "http://venus/api/",
    currentLoggedInUserObj : "",

    init: function () {
        var appWebUrl = _spPageContextInfo.webAbsoluteUrl;
        var hostUrl = _spPageContextInfo.siteAbsoluteUrl;

        context = new SP.ClientContext(appWebUrl);
        appContext = new SP.AppContextSite(context, hostUrl);
        TicketingSystem.currentLoggedInUserObj=Utility.getUserById( _spPageContextInfo.userId,appWebUrl);
        TicketingSystem.bindRequestedBy(TicketingSystem.currentLoggedInUserObj);        
    },
	bindRequestedBy:function(user){
		$('#requestedByName').val(user.Title).attr("disabled","disabled");
        $('#requestedByEmail').val(user.Email).attr("disabled","disabled");

	},
    bindPriority: function () {
        var ticket = new TicketInputForm();

        Utility.getData(TicketingSystem.hostURL + "Priority/GetAllPriorities/", "", "",
            function (data) {
                //console.log("Inside Success");

                if (data != null && data != "" ) {
                    //console.log("Inside Success 1");
                    ticket.Priority = data;
                    //console.log("Inside Success 2" + ticket.Priority);
                    $.each(ticket.Priority, function (index, value) {
                        //console.log("Inside Success 4" + value);
                        $('#ticketPriority').append('<option id=ticketPriority_' + value.ID + '>' + value.Title + '</option>');
                    })
                }
            },
        function (data) { console.log("Failed"); });
    },

    getCategoryId: function (categoryIdInFrontend) {
        var l = "ticketCategory_".length;
        return categoryIdInFrontend.substr(l, categoryIdInFrontend.length);
    },

    getSubCategoryId: function (subCategoryInFrontend) {
        var l = "ticketSubCategory_".length;
        return subCategoryInFrontend.substr(l, subCategoryInFrontend.length);
    },

    bindCategory: function () {
        var _this = this;
        Utility.getData(TicketingSystem.hostURL + "Category/GetAllCategories/", "", "",
            function (data) {
                if (data != null && data != "") {
                    _this.ticket.Category = data;
                    $.each(_this.ticket.Category, function (index, value) {
                        $('#ticketCategory').append('<option id=ticketCategory_' + value.ID + '>' + value.Title + '</option>');
                    })
                }
            },
            function (data) {
                console.log("Could not load Category. Please report.");
            })
    },

    bindSubCategory: function (category) {
        var _this = this;
        Utility.getData(TicketingSystem.hostURL + "SubCategory/GetAllSubCategories/", "", category,
            function (data) {
                if (data != null && data != "" ) {
                    _this.ticket.SubCategory = data.d;
                    $.each(_this.ticket.SubCategory, function (index, value) {
                        $('#ticketSubCategory').append('<option id=ticketSubCategory_' + value.ID + '>' + value.Title + '</option>');
                    })
                }
            },
            function (data) {

            })
    },

    registerEvents: function () {
        $("#btnUploadFiles").change(function () {
            var f = this.files;
            var filterFileType = ["exe", "ico", "ics"];
            $.each(f, function (index, file) {
                //file = new Attachment();                    
                if (TicketingSystem.files.filter(function (x) { return x.name == file.name; }).length) {
                    alert("A file with same name already exists.")
                } else {
                    var fileType = file.name.substr(file.name.lastIndexOf(".") + 1, file.name.length);
                    if (filterFileType.filter(function (x) {
                        return fileType == x
                    }).length) {
                        alert(fileType + " file type not allowed");
                    }
                    else {
                        var allowedFileSize = 5242880; // 5MB (1024*1024*5)
                        if (file.size > allowedFileSize) {
                            alert("Maximum allowed file size is 5 MB.")
                        }
                        else {
                            //TicketingSystem.files.push(file);
                            var newAttachment = new Attachment();
                            newAttachment.FileName = file.name;
                            newAttachment.FileUrl = file.url;
                            //TicketingSystem.files.push(new Attachment({ FileName: file.name, FileUrl: file.url }));
                            TicketingSystem.files.push(newAttachment);

                            var t = ' <div class="div-selected-user d-block " data-searchtype="PracticeLeader" data-displayname="' + file.name + '" data-loginname="' + file.name + '">' +
                                            '<span>' +
                                                '<span class="spn-selected-username">' + file.name + '</span>' +
                                                '<i class="fa fa-times spn-remove-selected-user" aria-hidden="true"></i>' +
                                            '</span>' +
                                        '</div>';

                            $("#attachedFiles").append(t);
                        }

                    }
                }


            });
            $(this).val("");

        });

        $("#attachedFiles").on("click", ".spn-remove-selected-user", function () {
            TicketingSystem.files.pop($(this));
            $(this).parent().parent().remove();
        });

        $(".radio-inline").change(function () {
            Validation.toggleRequestType();
        });

        $("#emailsToNotify").keypress(function (e) {
            if (e.keyCode == 13 || e.keyCode == 32) {
                $('#emailsToNotify').parent().parent().parent().siblings('.error').addClass('hidden');
                //Create a new tag with cross
                var newEmail = $('#emailsToNotify').val().trim();
                if (Validation.isValidEmail(newEmail)) {
                    var t = ' <div class="div-selected-user" data-searchtype="PracticeLeader" data-displayname="' + newEmail + '" data-loginname="' + newEmail + '">' +
                                    '<span>' +
                                        '<span class="spn-selected-username">' + newEmail + '</span>' +
                                        '<i class="fa fa-times spn-remove-selected-user" aria-hidden="true"></i>' +
                                        //'<span class="spn-remove-selected-user"></span>' +
                                    '</span>' +
                                '</div>';
                    $('#emailTags').append(t);
                    $('#emailsToNotify').val("");
                }
                else {
                    $('#emailsToNotify').parent().parent().parent().siblings('.error').removeClass('hidden');
                }
            }
        });

        $(".field-input").on("click", ".spn-remove-selected-user", function () {
            $(this).parent().parent().remove();
        });

        $("input[id=btnSubmit]").on("click", function () {
            TicketingSystem.submitForm();
        });

        $("#ticketCategory").change(function () {
            TicketingSystem.populateSubCategory();
        });
        $("textarea").attr('style', 'height:' + (this.scrollHeight) + 'px;overflow-y:hidden;').on('input', function () {
            this.style.height = 'auto';
            this.style.height = (this.scrollHeight) + 'px';
        }); 
    },

    populateSubCategory: function () { 
        var selected = $('#ticketCategory').find('option:selected').val();
        $('#ticketSubCategory').empty().append('<option id="ticketSubCategory_0">--Select subcategory--</option>');
        TicketingSystem.bindSubCategory(selected);
    },

    submitForm: function () {
        var isValid = Validation.validateForm();
        if(isValid)
        {
            TicketingSystem.submitFormDataToServer();
        }
    },

    submitFormDataToServer : function(){
        var ticket = new TicketInputForm();
        var x = new User();
        x.Name = $('#requestedByName').val();
        x.Email = $('#requestedByEmail').val();
        x.PhoneNumber = $('#requestedByPhone').val();
        ticket.RequestedBy = x;
        //ticket.RequestedFor = new User({ Name: $('#requestedForName').val(), Email: $('#requestedForEmail').val(), Phone: $('#requestedForPhone').val() });
        ticket.Title = $('#ticketTitle').val();
        ticket.Description = $('#ticketDescription').val();
        ticket.Attachments = TicketingSystem.files;

        //var e = ticket.EmailsToNotify;
        $('#emailTags').children().each(function () {
            //ticket.EmailsToNotify.push($(this).attr("data-displayname"));
            //e = e.concat($(this).attr("data-displayname") + ";");
            //ticket.EmailsToNotify.concat($(this).attr("data-displayname") + ";");
            ticket.EmailsToNotify = ticket.EmailsToNotify + $(this).attr("data-displayname") + ";";
        });
        //$('#attachedFiles').children().each(function () {
        //    ticket.Files.push($(this).attr("data-displayname"));
        //})
        //TicketingSystem.uploadFilesToServer();
        Utility.SubmitFormData("http://venus:8080/api/Ticket/CreateTicketWithBasicInfo/", "", ticket,
            function (data) {
                alert("Your ticket id is:" + data);
            },
            function (data) {
                alert("Some failure");
            },
            function (data) {
                alert("Error occured");
            });
    },

    uploadFilesToServer: function () {
        // Define the folder path for this example.
        var serverRelativeUrlToFolder = '/Documents';

        // Get test values from the file input and text input page controls.
        var fileInput = $('#btnUploadFiles');
        var serverUrl = _spPageContextInfo.webAbsoluteUrl;

        $.each(TicketingSystem.files, function (index, file) {
            var newName = file.name;
            // Initiate method calls using jQuery promises.
            // Get the local file as an array buffer.
            var getFile = getFileBuffer(file);
            getFile.done(function (arrayBuffer) {

                // Add the file to the SharePoint folder.
                var addFile = addFileToFolder(arrayBuffer);
                addFile.done(function (file, status, xhr) {

                    // Get the list item that corresponds to the uploaded file.
                    var getItem = getListItem(file.d.ListItemAllFields.__deferred.uri);
                    getItem.done(function (listItem, status, xhr) {

                        // Change the display name and title of the list item.
                        var changeItem = updateListItem(listItem.d.__metadata);
                        changeItem.done(function (data, status, xhr) {
                            alert('file uploaded and updated');
                        });
                        changeItem.fail(onError);
                    });
                    getItem.fail(onError);
                });
                addFile.fail(onError);
            });
            getFile.fail(onError);
        });







        // Get the local file as an array buffer.
        function getFileBuffer(file) {
            var deferred = jQuery.Deferred();
            var reader = new FileReader();
            reader.onloadend = function (e) {
                deferred.resolve(e.target.result);
            }
            reader.onerror = function (e) {
                deferred.reject(e.target.error);
            }
            //reader.readAsArrayBuffer(fileInput[0].files[0]);
            reader.readAsArrayBuffer(file);
            return deferred.promise();
        }

        // Add the file to the file collection in the Shared Documents folder.
        function addFileToFolder(arrayBuffer) {

            // Get the file name from the file input control on the page.
            var parts = fileInput[0].value.split('\\');
            var fileName = parts[parts.length - 1];

            // Construct the endpoint.
            var fileCollectionEndpoint = String.format(
                    "{0}/_api/web/getfolderbyserverrelativeurl('{1}')/files" +
                    "/add(overwrite=true, url='{2}')",
                    serverUrl, serverRelativeUrlToFolder, fileName);

            // Send the request and return the response.
            // This call returns the SharePoint file.
            return jQuery.ajax({
                url: fileCollectionEndpoint,
                type: "POST",
                data: arrayBuffer,
                processData: false,
                headers: {
                    "accept": "application/json;odata=verbose",
                    "X-RequestDigest": jQuery("#__REQUESTDIGEST").val(),
                    "content-length": arrayBuffer.byteLength
                }
            });
        }

        // Get the list item that corresponds to the file by calling the file's ListItemAllFields property.
        function getListItem(fileListItemUri) {

            // Send the request and return the response.
            return jQuery.ajax({
                url: fileListItemUri,
                type: "GET",
                headers: { "accept": "application/json;odata=verbose" }
            });
        }

        // Change the display name and title of the list item.
        function updateListItem(itemMetadata) {

            // Define the list item changes. Use the FileLeafRef property to change the display name. 
            // For simplicity, also use the name as the title. 
            // The example gets the list item type from the item's metadata, but you can also get it from the
            // ListItemEntityTypeFullName property of the list.
            var body = String.format("{{'__metadata':{{'type':'{0}'}},'FileLeafRef':'{1}','Title':'{2}'}}",
                itemMetadata.type, newName, newName);

            // Send the request and return the promise.
            // This call does not return response content from the server.
            return jQuery.ajax({
                url: itemMetadata.uri,
                type: "POST",
                data: body,
                headers: {
                    "X-RequestDigest": jQuery("#__REQUESTDIGEST").val(),
                    "content-type": "application/json;odata=verbose",
                    "content-length": body.length,
                    "IF-MATCH": itemMetadata.etag,
                    "X-HTTP-Method": "MERGE"
                }
            });
        }
    },


}

var Utility = {
    getData: function (serviceURL, parameterName, parameterValue, success, failure) {

        $.ajax({
            url: serviceURL,
            method: "GET",
            //data: parameterName == "" ? "" : "{" + parameterName + ":'" + parameterValue + "'}",
            //data: JSON.stringify(parameterValue),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: success,
            failure: failure,
        });
    },
    getData: function (resourceUrl, success, failure) {
        $.ajax({
            url: resourceUrl,
            method: "GET",
            async: false,
            headers: { "Accept": "application/json; odata=verbose" },
            success: function (data) {
                success(data);
            },
            error: function (data) {
                failure(data);
            }
        });
    },
    showLoading: function () {
        $(".supervisor-dashboard-pop-up-outer-container").show();
    },
    hideLoading: function () {
        $(".supervisor-dashboard-pop-up-outer-container").hide();
    },
    SubmitFormData: function (serviceURL, parameterName, parameterValue, success, failure, error) {
        $.ajax({
            url: serviceURL,
            method: "POST",
            //data: parameterName == "" ? "" : "{'" + parameterName + "':" + JSON.stringify(parameterValue) + "}",
            //data: parameterName == "" ? "" : "{" + parameterName + ":'" + JSON.stringify(parameterValue) + "'}",
            data: JSON.stringify(parameterValue),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: success,
            failure: failure,
            error: error
        });
    },
    getUserById: function (userId, webUrl) {
        var user = null;
        try {
            var siteUrl = webUrl + "/_api/web/siteusers/getbyid(" + userId + ")";
            Utility.getData(siteUrl, function (data) {
                user = data.d;
            }, this.onQueryFailed);
        }
        catch (err) {
            //user = null;
        }
        finally {
            return user;
        }
    }

}

var Validation = {
    validateForm: function () {
        $(".error").addClass("hidden");
        var isValid = true;
        var phoneRegex = /^[0-9]{4}[-][0-9]{3}[-][0-9]{4}$/

       
        $('#requestedBy .form-control').each(function () {
            if ($(this).val() == "") {
                isValid = false;
                $(this).siblings(".error").removeClass("hidden");
            }
        });
        if (!Validation.isValidEmail($('#requestedByEmail').val().trim())) {
            isValid = false;
            $('#requestedByEmail').siblings(".error").removeClass("hidden");
        }      

        $('.brief-desc .form-control').each(function () {
            if ($(this).val() == "") {
                isValid = false;
                $(this).siblings(".error").removeClass("hidden");
            }
        });
        

        var emailsToNotify = $('#emailsToNotify').val().split(" ");
        emailsToNotify.forEach(function (email) {

            if (email != "" || email != null) {
                //var regexEmail = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
                if (Validation.isValidEmail(email)) {
                    $('#emailsToNotify').siblings('.error').removeClass("hidden");
                    isValid = false;
                    return false;
                }
            }

        });

        return isValid;
    },

    isValidEmail: function (email) {
        var regexEmail = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return regexEmail.test(email);
    },

    toggleRequestType: function () {
        //var requestType = $(".field-input input[name='request-type']:checked").val
        $('#requestedFor').toggleClass("hidden");
    }
}



  

    // Display error messages. 
    function onError(error) {
        alert(error.responseText);
    }

