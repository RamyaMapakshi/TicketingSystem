<%@ Page Language="C#" Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<WebPartPages:AllowFraming ID="AllowFraming" runat="server" />

<html>
<head>
    <title></title>

    <script type="text/javascript" src="../Scripts/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="/_layouts/15/MicrosoftAjax.js"></script>
    <script type="text/javascript" src="/_layouts/15/sp.runtime.js"></script>
    <script type="text/javascript" src="/_layouts/15/sp.js"></script>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link href="http://fontawesome.io/assets/font-awesome/css/font-awesome.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <link href="/Content/Ticket.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.1.1.min.js"
        integrity="sha256-hVVnYaiADRTO2PzUGmuLJr8BLUSjGIZsDYGmIJLv2b8="
        crossorigin="anonymous"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <script src="/Scripts/NewTicket.js"></script>
    <script type="text/javascript">
        // Set the style of the client web part page to be consistent with the host web.
        (function () {
            'use strict';

            var hostUrl = '';
            if (document.URL.indexOf('?') != -1) {
                var params = document.URL.split('?')[1].split('&');
                for (var i = 0; i < params.length; i++) {
                    var p = decodeURIComponent(params[i]);
                    if (/^SPHostUrl=/i.test(p)) {
                        hostUrl = p.split('=')[1];
                        document.write('<link rel="stylesheet" href="' + hostUrl + '/_layouts/15/defaultcss.ashx" />');
                        break;
                    }
                }
            }
            if (hostUrl == '') {
                document.write('<link rel="stylesheet" href="/_layouts/15/1033/styles/themable/corev15.css" />');
            }
        })();
    </script>
</head>
<body>
    <div class="container-full">

        <div class="supervisor-dashboard-pop-up-outer-container">
            <div class="supervisor-dashboard-pop-up-container">
                <div class="supervisor-dashboard-pop-up">
                    <div class="pop-up-title">
                    </div>
                    <div class="pup-up-body">
                        <div id="loadingGif" class="pop-container">
                            <img src="../Images/PageLoading.gif" id="imgPageLoading" />
                            <div>Please Wait...</div>
                        </div>
                    </div>
                    <div class="pop-up-footer">
                    </div>
                </div>
            </div>
        </div>
        <div class="container-fluid header">
            <h2>Create New Ticket</h2>

            <div class="form-container">
                <div class="group">
                    <div class="header-title">
                        <h3 class="header header-category">Requester Details</h3>
                    </div>
                    <div class="form-content">
                        <div class="row control form-group">
                            <div class="col-md-6 col-sm-6 col-xs-12 pad-left-right-0" id="requestedBy">
                                <h4 class="header header-category">Requested By</h4>
                                <div class="row control form-group">
                                    <div class="field-name">Name</div>
                                    <div class="field-input">
                                        <input type="text" class="form-control requested-by-input" id="requestedByName" />
                                        <div class="error hidden">Name cannot be empty.</div>
                                    </div>
                                </div>
                                <div class="row control form-group">
                                    <div class="field-name">Email</div>
                                    <div class="field-input">
                                        <input type="text" class="form-control requested-by-input" id="requestedByEmail" />
                                        <div class="error hidden">Please enter a valid email.</div>
                                    </div>
                                </div>
                                <div class="row control form-group">
                                    <div class="field-name">Phone</div>
                                    <div class="field-input">
                                        <input type="text" class="form-control requested-by-input" id="requestedByPhone" />
                                        <div class="error hidden">Please enter valid phone.</div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="group">
                    <div class="header-title">
                        <h3 class="header header-category">Ticket Description</h3>
                    </div>
                    <div class="form-content brief-desc">
                        <div class="row control form-group">
                            <div class="field-name">Brief Description</div>
                            <div class="field-input">
                                <input type="text" class="form-control" placeholder="Please provide brief comment about the ticket." id="ticketTitle" />
                                <div class="error hidden">Please enter brief description of the issue.</div>
                            </div>
                        </div>
                        <div class="row control form-group">
                            <div class="field-name">Describe the issue or request in detail</div>
                            <div class="field-input">
                                <textarea class="form-control" rows="4" id="ticketDescription"></textarea>
                                <div class="error hidden">Please enter complete description of the issue.</div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="group hidden">
                    <div class="header-title">
                        <h3 class="header header-category">Request Info</h3>
                    </div>
                    <div class="form-content request-info">
                        <div class="row control form-group">
                            <div class="field-name">Type of Request</div>
                            <div class="field-input">
                                <input type="text" class="form-control" placeholder="Type of request here" id="ticketType" />
                                <div class="error hidden">Please enter type of request.</div>
                            </div>
                        </div>
                        <div class="row control form-group">
                            <div class="col-md-6 col-sm-6 col-xs-12 pad-left-right-0">
                                <div class="field-name">Impact</div>
                                <div class="field-input">
                                    <input type="text" class="form-control" placeholder="Global" id="ticketImpact" />
                                    <div class="error hidden">Please enter impact of the request.</div>
                                </div>
                            </div>
                            <div class="col-md-6 col-sm-6 col-xs-12 pad-left-right-0">
                                <div class="field-name">Priority</div>
                                <div class="field-input ">
                                    <select class="form-control" id="ticketPriority">
                                        <option id="ticketPriority_0">--Select Priority--</option>
                                    </select>
                                    <div class="error hidden">Please select priority.</div>
                                </div>
                            </div>
                        </div>
                        <div class="row control form-group">
                            <div class="col-md-6 col-sm-6 col-xs-12 pad-left-right-0">
                                <div class="field-name">Category</div>
                                <div class="field-input">
                                    <select class="form-control" id="ticketCategory">
                                        <option id="ticketCategory_0">--Select Category--</option>
                                    </select>
                                    <div class="error hidden">Please select category.</div>
                                </div>
                            </div>
                            <div class="col-md-6 col-sm-6 col-xs-12 pad-left-right-0">
                                <div class="field-name">Sub Category</div>
                                <div class="field-input ">
                                    <select class="form-control" id="ticketSubCategory">
                                        <option id="ticketSubCategory_0">--Select subcategory--</option>
                                    </select>
                                    <div class="error hidden">Please select sub category.</div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="group">
                    <div class="header-title">
                        <h3 class="header header-category">Additional Info</h3>
                    </div>
                    <div class="form-content">
                        <div class="row control form-group">
                            <div class="col-md-12 col-sm-12 col-xs-12 pad-left-right-0">
                                <div class="field-name">Email(s) to notify</div>
                                <div class="field-input">

                                    <div class="email-input-div">
                                        <div id="divEmailTag" class="div-selected-users display-inlineBlock" contenteditable="false">

                                            <div class="email-tag" id="emailTags"></div>
                                            <div class="email-tag email-input">
                                                <input type="text" class="form-control" placeholder="Email" id="emailsToNotify" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="error hidden">Please enter correct email.</div>
                                </div>
                            </div>

                        </div>
                        <div class="row  control form-group">
                            <div class="col-md-12 col-sm-12 col-xs-12 pad-left-right-0" id="fileUpload">
                                <label for="btnUploadFiles" class="link-blue pointer" id="myFileSelectorLabel">
                                    <i class="fa fa-paperclip fa-rotate-270 fa-2x" aria-hidden="true"></i>
                                    Attach files
                                </label>
                                <input id="btnUploadFiles" multiple type="file" name="files" style="display: none" />
                                <div class="" id="attachedFiles"></div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="group">
                    <div class="row">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <input type="button" class="btn btn-default btn-lg btn-custom btn-cancel  pull-right btn-cancel" value="Cancel" />
                            <input type="button" class="btn btn-default btn-primary btn-lg btn-custom  pull-right btn-submit" value="Submit" id="btnSubmit" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
