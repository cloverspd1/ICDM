var uploadedFiles = [], uploadedFiles1 = [], uploadedFiles2 = [], uploadedFiles3 = [], uploadedFiles4 = [], uploadedFiles5 = [], uploadedFiles6 = [], uploadedFiles7 = [], uploadedFiles8 = [], uploadedFiles9 = [], uploadedFiles10 = [];
var hdnVenodrsId = '';
if ($('#hdnVendors').val() != undefined && $('#hdnVendors').val() != "") {
    hdnVenodrsId = $('#hdnVendors').val() + ",";
}
$(document).ready(function () {

    BindUserTagsForGrid("#tblVendors");

    BindAttachments();

    MultiSelectCalls('LUMMarketingDelegateUser', 'LUMMarketingDelegate', 'LUMMarketingDelegateName');
    MultiSelectCalls('SCMLUMDesignDelegateUser', 'SCMLUMDesignDelegate', 'SCMLUMDesignDelegateUser');
    MultiSelectCalls('SMSDelegateUser', 'SMSDelegate', 'SMSDelegateName');
    MultiSelectCalls('QADelegateUser', 'QADelegate', 'QADelegateName');
    MultiSelectCalls('FinalSMSDelegateUser', 'FinalSMSDelegate', 'FinalSMSDelegateName');
    MultiSelectCalls('CostingDelegate1User', 'CostingDelegate', 'CostingDelegateName');
    MultiSelectCalls('CostingDelegate2User', 'CostingDelegate2', 'CostingDelegate2Name');
    //  MultiSelectCalls('CostingDelegate2User1', 'CostingDelegateUser2', 'CostingDelegateUserName2');
    MultiSelectCalls('TDSDelegateUser', 'TDSDelegate', 'TDSDelegateName');

    $('#ItemPilotLotPreparation').multiselect({
        includeSelectAllOption: true,
        onChange: function (option, checked) {
            if ($('#ItemPilotLotPreparation').hasClass('multiselectrequired') && checked) {
                $('#ItemPilotLotPreparation.multiselectrequired').next('div.btn-group.input-validation-error').removeClass('input-validation-error');
                $('#ItemPilotLotPreparation.multiselectrequired').next('div.btn-group').next("span.field-validation-error").addClass("field-validation-valid");
                $('#ItemPilotLotPreparation.multiselectrequired').next('div.btn-group').next("span.field-validation-valid").removeClass("field-validation-error");
            }
        },
        onSelectAll: function (checked) {
            if ($('#ItemPilotLotPreparation').hasClass('multiselectrequired') && checked) {
                $('#ItemPilotLotPreparation.multiselectrequired').next('div.btn-group.input-validation-error').removeClass('input-validation-error');
                $('#ItemPilotLotPreparation.multiselectrequired').next('div.btn-group').next("span.field-validation-error").addClass("field-validation-valid");
                $('#ItemPilotLotPreparation.multiselectrequired').next('div.btn-group').next("span.field-validation-valid").removeClass("field-validation-error");
            }
        }
    });


    ShowHideDelegateSectionButtons('LUMMarketingDelegateUser', 'hdnIsLUMMarketingInchargeSectionActive');
    ShowHideDelegateSectionButtons('SCMLUMDesignDelegateUser', 'hdnIsSCMLUMDesignInchargeSectionActive');
    ShowHideDelegateSectionButtons('SMSDelegateUser', 'hdnIsSMSInchargeSectionActive');
    ShowHideDelegateSectionButtons('QADelegateUser', 'hdnIsQAInchargeSectionActive');
    ShowHideDelegateSectionButtons('FinalSMSDelegateUser', 'hdnIsFinalSMSInchargeSectionActive');
    ShowHideDelegateSectionButtons('CostingDelegate1User', 'hdnIsCostingInchargeSectionActive');
    //  ShowHideDelegateSectionButtons('CostingDelegate2User', 'hdnIsCostingInchargeSectionActive');
    ShowHideDelegateSectionButtons('TDSDelegateUser', 'hdnIsTDSInchargeSectionActive');

    if ($('#BISRegistrationApplicable').val() == "False") {
        $('.RNumber').hide();
        $('#RNumber').val('');
    }
    else if ($('#BISRegistrationApplicable').val() == "True") {
        $('.RNumber').show();
    }

    $("#BISRegistrationApplicableYes").on("change", function () {
        if ($(this).is(":checked")) {
            $('.RNumber').show();
            $('#BISRegistrationApplicable').val($(this).val());
        }
    }).change();

    $("#BISRegistrationApplicableNo").on("change", function () {
        if ($(this).is(":checked")) {
            $('.RNumber').hide();
            $('#RNumber').val('');
            $('#BISRegistrationApplicable').val($(this).val());
        }
    }).change();


    $("#JointInspectionDoneYes").on("change", function () {
        if ($(this).is(":checked")) {
            $('#IsJointInspectionDone').val($(this).val());
        }
    }).change();

    $("#JointInspectionDoneNo").on("change", function () {
        if ($(this).is(":checked")) {
            $('#IsJointInspectionDone').val($(this).val());
        }
    }).change();

    var selectedSection = $("#ItemPilotLotPreparation").attr("data-selected");
    if ($.trim(selectedSection) != "") {
        $('#ItemPilotLotPreparation').multiselect('select', selectedSection.split(","));
    }

    $("select#ItemPilotLotPreparation").off("change").on("change", function () {
        $("input[type='hidden'][name='PilotLotPreparation']").val($('select#ItemPilotLotPreparation').val());

        if ($("#ItemPilotLotPreparation").val() == null) {
            $('#PilotLotPreparation').val('');
        }
    }).change();

    $(".BuyMakeSelect").off("change").on("change", function () {
        var value = $(".BuyMakeSelect option:selected").text();
        if (value == '') {
            value = $(".BuyMakeSelect").val();
        }
        if (value == "Buy" || value == 'Select') {
            $('#spanBOM').hide();
            if ($("#BOMAttachment") != undefined && $("#BOMAttachment").val() != undefined) {
                $("#BOMAttachment").rules("remove", "required");
                $("input[type=text][id*=BOMAttachment]").removeClass('input-validation-error');
                $("input[type=text][id*=BOMAttachment]").next().addClass('field-validation-valid');
                $("input[type=text][id*=BOMAttachment]").next().removeClass('field-validation-error');
            }

        } else if (value == "Make") {
            $('#spanBOM').show();
            $("#BOMAttachment").rules("add", {
                required: true,
                messages: {
                    required: "BOM Attachment is required."
                }
            });
            if ($("#BOMAttachment").val() == undefined || $("#BOMAttachment").val() == '' || $("#BOMAttachment").val() == null) {
                $("input[type=text][id*=BOMAttachment]").addClass('input-validation-error');
                $("input[type=text][id*=BOMAttachment]").next().removeClass('field-validation-valid');
                $("input[type=text][id*=BOMAttachment]").next().addClass('field-validation-error');
            }
        }
    }).change();
    if ($('#Vendors').val() != '' && $('#Vendors').val() != undefined) {
        $('ul.token-input-list').removeClass('token-input-focused');
    }

    $("select#PilotLotPreparation").off("change").on("change", function () {
        var value = $("#PilotLotPreparation option:selected").val();
        if (value == "Yes") {
            $(".quantity").show();
            $("#Quantity").rules("add", {
                required: true,
                messages: {
                    required: "Quantity is required."
                }
            });
        } else {
            $(".quantity").hide();
            $("#Quantity").rules("remove", "required");
        }


    }).change();

    setTimeout(function () {
        $("#divLUMmmarketingDelegate").find("div.card-body").removeClass("collapse");
        $("#divLUMmmarketingDelegate").find("div.card-body").removeClass("in");
        $("#divLUMmmarketingDelegate").find("div.card-body").addClass("collapsed");
    }, 1001);

});

function ShowHideDelegateSectionButtons(eleId, hdnElementId) {
    var item = '#' + eleId;
    var isSectionActive = $('#' + hdnElementId).val();
    if (isSectionActive != undefined && isSectionActive != "disabled") {
        if ($(item).length > 0) {
            var selectedOptions = $(item + ' option:selected');
            ShowHideDelegateButton(selectedOptions);

            if (item == '#TDSDelegateUser' && selectedOptions.length > 0) {
                $('ul.header-nav > li > a:contains("Complete")').parent().hide();
            }
            else if (item == '#TDSDelegateUser' && selectedOptions.length <= 0) {
                $('ul.header-nav > li > a:contains("Complete")').parent().show();
            }
            if (item == '#QADelegateUser' && selectedOptions.length > 0) {
                $('ul.header-nav > li > a:contains("Approve")').parent().hide();
            }
            else if (item == '#QADelegateUser' && selectedOptions.length <= 0) {
                $('ul.header-nav > li > a:contains("Approve")').parent().show();
            }
        }
        else {
            $('ul.header-nav > li > a:contains("Delegate")').parent().hide();
            $('ul.header-nav > li > a:contains("Submit")').parent().show();
        }
    }
}

function MultiSelectCalls(id, hdnDelegateUser, hdnDelegateUserName) {
    var item = '#' + id;
    $(item).multiselect({
        onChange: function (option, checked) {

            // Get selected options.                      
            if ($('div[id$="Delegate"]').not('.collapse').children().find('.field-validation-error').length > 0) {
                ValidateDelegateControls('div[id$="Delegate"]');
            }
            else if (item == '#CostingDelegate1User' && $('div[id$="Delegate1"]').not('.collapse').children().find('.field-validation-error').length > 0) {
                ValidateDelegateControls('div[id$="Delegate1"]');
            }

            var selectedCostingDelegate1 = false;
            if (item == '#CostingDelegate1User') {
                $(item + ' option:selected').each(function () {
                    selectedCostingDelegate1 = true;
                });
            }

            var selectedCostingDelegate2 = false;
            if (item == '#CostingDelegate2User') {
                $(item + ' option:selected').each(function () {
                    selectedCostingDelegate2 = true;
                });
            }

            if (item == '#CostingDelegate1User') {
                if (selectedCostingDelegate1) {
                    $('#CostingDelegate2User').multiselect('disable');
                }
                else {
                    $('#CostingDelegate2User').multiselect('enable');
                }
            }
            else if (item == '#CostingDelegate2User') {
                if (selectedCostingDelegate2) {
                    $('#CostingDelegate1User').multiselect('disable');
                    if ($('#CostingDelegate2User').hasClass('multiselectrequired')) {
                        $('#CostingDelegate2User.multiselectrequired').next('div.btn-group.input-validation-error').removeClass('input-validation-error');
                        $('#CostingDelegate2User.multiselectrequired').next('div.btn-group').next("span.field-validation-error").addClass("field-validation-valid");
                        $('#CostingDelegate2User.multiselectrequired').next('div.btn-group').next("span.field-validation-valid").removeClass("field-validation-error");
                    }
                }
                else {
                    $('#CostingDelegate1User').multiselect('enable');
                }
            }
            var selectedOptions = $(item + ' option:selected');

            $("input[type='hidden'][name='" + hdnDelegateUser + "']").val($('select' + item).val());
            $("input[type='hidden'][name='" + hdnDelegateUserName + "']").val(GetMultiselectValue(selectedOptions));

            if (selectedOptions.length >= 1) {
                // Disable all other checkboxes.
                var nonSelectedOptions = $(item + ' option').filter(function () {
                    return !$(this).is(':selected');
                });

                nonSelectedOptions.each(function () {
                    var input = $('input[value="' + $(this).val() + '"]');
                    input.attr('disabled', 'disabled');
                    input.prop('disabled', true).closest('a').attr("tabindex", "-1").closest('li').addClass('disabled');
                    input.parent('li').addClass('disabled');
                });
            }
            else {
                // Enable all checkboxes.
                $(item + ' option').each(function () {
                    var input = $('input[value="' + $(this).val() + '"]');
                    input.prop('disabled', false);
                    input.parent('li').addClass('disabled');
                });
            }

            ShowHideDelegateButton(selectedOptions);

            var selectedTDS = false;
            if (item == '#TDSDelegateUser') {
                $(item + ' option:selected').each(function () {
                    selectedTDS = true;
                });
            }

            if (item == '#TDSDelegateUser' && selectedTDS) {
                $('ul.header-nav > li > a:contains("Complete")').parent().hide();
            }
            else if (item == '#TDSDelegateUser' && !selectedTDS) {
                $('ul.header-nav > li > a:contains("Complete")').parent().show();
            }

            var selected = false;
            if (item == '#QADelegateUser') {
                $(item + ' option:selected').each(function () {
                    selected = true;
                });
            }

            if (item == '#QADelegateUser' && selected) {
                $('ul.header-nav > li > a:contains("Approve")').parent().hide();
            }
            else if (item == '#QADelegateUser' && !selected) {
                $('ul.header-nav > li > a:contains("Approve")').parent().show();
            }
            if (item == '#CostingDelegate2User' && checked) {
                $('ul.header-nav > li > a:contains("Submit")').parent().show();
                $('ul.header-nav > li > a:contains("Delegate")').parent().hide();
            }
            else if (item == '#CostingDelegate2User' && !checked) {
                $('ul.header-nav > li > a:contains("Submit")').parent().show();
                $('ul.header-nav > li > a:contains("Delegate")').parent().hide();
            }
        }
    });
    if ($.trim($(item).attr("data-selected")) != "") {
        $(item).multiselect('select', $(item).attr("data-selected").split(","));
    }
}
function ShowHideDelegateButton(selectedOptions) {
    if (selectedOptions.length > 0) {
        $('ul.header-nav > li > a:contains("Submit")').parent().hide();
        $('ul.header-nav > li > a:contains("Delegate")').parent().show();

    } else {
        $('ul.header-nav > li > a:contains("Delegate")').parent().hide();
        $('ul.header-nav > li > a:contains("Submit")').parent().show();
    }
}
function BindAttachments() {
    if ($("#ReliabilityTestReportAttachment").length != 0) {
        BindFileUploadControl({
            ElementId: 'divReliabilityTestReportAttachment', Params: {}, Url: "UploadFile",
            AllowedExtensions: [],
            MultipleFiles: true,
            CallBack: "OnFileUploadedReliabilityTestReport"
        });
        uploadedFiles = BindFileList("ReliabilityTestReportAttachment", "divReliabilityTestReportAttachment");
    }
    if ($("#UploadArtworkAttachment").length != 0) {
        BindFileUploadControl({
            ElementId: 'divUploadArtworkAttachment', Params: {}, Url: "UploadFile",
            AllowedExtensions: [],
            MultipleFiles: true,
            CallBack: "OnFileArtworkAttachment"
        });
        uploadedFiles1 = BindFileList("UploadArtworkAttachment", "divUploadArtworkAttachment");
    }
    if ($("#BOMAttachment").length != 0) {
        BindFileUploadControl({
            ElementId: 'divBOMAttachment', Params: {}, Url: "UploadFile",
            AllowedExtensions: [],
            MultipleFiles: true,
            CallBack: "OnFileUploadeBOMAttachment"
        });
        uploadedFiles2 = BindFileList("BOMAttachment", "divBOMAttachment");
    }
    if ($("#ADSAttachment").length != 0) {
        BindFileUploadControl({
            ElementId: 'divADSAttachment', Params: {}, Url: "UploadFile",
            AllowedExtensions: [],
            MultipleFiles: true,
            CallBack: "OnFileUploadeADS"
        });
        uploadedFiles3 = BindFileList("ADSAttachment", "divADSAttachment");
    }

    if ($("#SpecificationSheetAttachment").length != 0) {
        BindFileUploadControl({
            ElementId: 'divSpecificationSheetAttachment', Params: {}, Url: "UploadFile",
            AllowedExtensions: [],
            MultipleFiles: false,
            maxFiles: 1,
            CallBack: "OnFileUploadedSpecificationSheet"
        });
        uploadedFiles4 = BindFileList("SpecificationSheetAttachment", "divSpecificationSheetAttachment");
    }

    if ($("#PackagingDataSheetAttachment").length != 0) {
        BindFileUploadControl({
            ElementId: 'divPackagingDataSheetAttachment', Params: {}, Url: "UploadFile",
            AllowedExtensions: [],
            MultipleFiles: true,
            CallBack: "OnFileUploadedPackagingDataSheet"
        });
        uploadedFiles5 = BindFileList("PackagingDataSheetAttachment", "divPackagingDataSheetAttachment");
    }
    if ($("#IESFileAttachment").length != 0) {
        BindFileUploadControl({
            ElementId: 'divIESFileAttachment', Params: {}, Url: "UploadFile",
            AllowedExtensions: [],
            MultipleFiles: true,
            CallBack: "OnFileIESFileAttachment"
        });
        uploadedFiles6 = BindFileList("IESFileAttachment", "divIESFileAttachment");
    }
    if ($("#LM79Attachment").length != 0) {
        BindFileUploadControl({
            ElementId: 'divLM79Attachment', Params: {}, Url: "UploadFile",
            AllowedExtensions: [],
            MultipleFiles: true,
            CallBack: "OnFileUploadeLM79MAttachment"
        });
        uploadedFiles7 = BindFileList("LM79Attachment", "divLM79Attachment");
    }
    if ($("#ProductDrawingAttachment").length != 0) {
        BindFileUploadControl({
            ElementId: 'divProductDrawingAttachment', Params: {}, Url: "UploadFile",
            AllowedExtensions: [],
            MultipleFiles: true,
            CallBack: "OnFileUploadeProductDrawing"
        });
        uploadedFiles8 = BindFileList("ProductDrawingAttachment", "divProductDrawingAttachment");
    }

    if ($("#TDSAttachment").length != 0) {
        BindFileUploadControl({
            ElementId: 'divTDSAttachment', Params: {}, Url: "UploadFile",
            AllowedExtensions: [],
            MultipleFiles: false,
            maxFiles: 1,
            CallBack: "OnFileUploadedTDSAttachment"
        });
        uploadedFiles9 = BindFileList("TDSAttachment", "divTDSAttachment");
    }

    if ($("#JointInspectionAttachment").length != 0) {
        BindFileUploadControl({
            ElementId: 'divJointInspectionAttachment', Params: {}, Url: "UploadFile",
            AllowedExtensions: [],
            MultipleFiles: true,
            CallBack: "OnFileUploadedJointInspection"
        });
        uploadedFiles10 = BindFileList("JointInspectionAttachment", "divJointInspectionAttachment");
    }
}

function OnFileUploadedReliabilityTestReport(result) {
    uploadedFiles.push(result);
    $("#ReliabilityTestReportAttachment").val(JSON.stringify(uploadedFiles)).blur();
}
function divReliabilityTestReportAttachmentRemoveImage(ele) {
    RemoveAttachment(ele, uploadedFiles, "ReliabilityTestReportAttachment");
}
function OnFileArtworkAttachment(result) {
    uploadedFiles1.push(result);
    $("#UploadArtworkAttachment").val(JSON.stringify(uploadedFiles1)).blur();
}
function divUploadArtworkAttachmentRemoveImage(ele) {
    RemoveAttachment(ele, uploadedFiles1, "UploadArtworkAttachment");
}
function OnFileUploadeBOMAttachment(result) {
    uploadedFiles2.push(result);
    $("#BOMAttachment").val(JSON.stringify(uploadedFiles2)).blur();
}
function divBOMAttachmentRemoveImage(ele) {
    RemoveAttachment(ele, uploadedFiles2, "BOMAttachment");
}
function OnFileUploadeADS(result) {
    uploadedFiles3.push(result);
    $("#ADSAttachment").val(JSON.stringify(uploadedFiles3)).blur();
}
function divADSAttachmentRemoveImage(ele) {
    RemoveAttachment(ele, uploadedFiles3, "ADSAttachment");
}
function OnFileUploadedSpecificationSheet(result) {
    uploadedFiles4.push(result);
    $("#SpecificationSheetAttachment").val(JSON.stringify(uploadedFiles4)).blur();
}
function divSpecificationSheetAttachmentRemoveImage(ele) {
    RemoveAttachment(ele, uploadedFiles4, "SpecificationSheetAttachment");
}
function OnFileUploadedPackagingDataSheet(result) {
    uploadedFiles5.push(result);
    $("#PackagingDataSheetAttachment").val(JSON.stringify(uploadedFiles5)).blur();
}
function divPackagingDataSheetAttachmentRemoveImage(ele) {
    RemoveAttachment(ele, uploadedFiles5, "PackagingDataSheetAttachment");
}
function OnFileIESFileAttachment(result) {
    uploadedFiles6.push(result);
    $("#IESFileAttachment").val(JSON.stringify(uploadedFiles6)).blur();
}
function divIESFileAttachmentRemoveImage(ele) {
    RemoveAttachment(ele, uploadedFiles6, "IESFileAttachment");
}
function OnFileUploadeLM79MAttachment(result) {
    uploadedFiles7.push(result);
    $("#LM79Attachment").val(JSON.stringify(uploadedFiles7)).blur();
}
function divLM79AttachmentRemoveImage(ele) {
    RemoveAttachment(ele, uploadedFiles7, "LM79Attachment");
}
function OnFileUploadeProductDrawing(result) {
    uploadedFiles8.push(result);
    $("#ProductDrawingAttachment").val(JSON.stringify(uploadedFiles8)).blur();
}
function divProductDrawingAttachmentRemoveImage(ele) {
    RemoveAttachment(ele, uploadedFiles8, "ProductDrawingAttachment");
}
function OnFileUploadedTDSAttachment(result) {
    uploadedFiles9.push(result);
    $("#TDSAttachment").val(JSON.stringify(uploadedFiles9)).blur();
}
function divTDSAttachmentRemoveImage(ele) {
    RemoveAttachment(ele, uploadedFiles9, "TDSAttachment");
}
function OnFileUploadedJointInspection(result) {
    uploadedFiles10.push(result);
    $("#JointInspectionAttachment").val(JSON.stringify(uploadedFiles10)).blur();
}
function divJointInspectionAttachmentRemoveImage(ele) {
    RemoveAttachment(ele, uploadedFiles10, "JointInspectionAttachment");
}
function RemoveAttachment(ele, uploadFilesArray, attachmentId) {
    var Id = $(ele).attr("data-id");
    var li = $(ele).parents("li.qq-upload-success");
    var itemIdx = li.index();
    ConfirmationDailog({
        title: "Remove", message: "Are you sure to remove file?", id: Id, url: "/ItemcodeCreation/RemoveUploadFile", okCallback: function (id, data) {
            li.find(".qq-upload-status-text").remove();
            $('<span class="qq-upload-spinner"></span>').appendTo(li);
            li.removeClass("qq-upload-success");
            var idx = -1;
            var tmpList = [];
            $(uploadFilesArray).each(function (i, item) {
                if (idx == -1 && item.FileId == id) {
                    idx = i;
                    //if (item.Status == 0) {
                    item.Status = 2;
                    tmpList.push(item);
                    uploadFilesArray.splice(i, 1);
                    //uploadFilesArray.splice($.inArray(item, uploadFilesArray), 1);
                    //}
                } else {
                    tmpList.push(item);
                }
            });
            if (idx >= 0) {
                // uploadFilesArray = tmpList;
                li.remove();
                if (uploadFilesArray.length == 0) {
                    $("#" + attachmentId).val("");
                } else {
                    $("#" + attachmentId).val(JSON.stringify(uploadFilesArray)).blur();
                }
            }
        }
    });
}

function OnVendoraddSuccess(data, status, xhr) {
    if (data.IsSucceed) {
        $("#addeditVendorModel").modal('hide');
        AlertModal('Success', ParseMessage(data.Messages));
        AjaxCall({
            url: "/ItemCodeCreation/GetVendorGrid",
            httpmethod: "GET",
            sucesscallbackfunction: function (result) {
                $("#divVendor").html(result);
            }
        });
    } else {
        AlertModal('Error', ParseMessage(data.Messages));
    }
}

function VendorDelete(index) {
    ConfirmationDailog({
        title: "Delete Vendor",
        message: "Are you sure want to delete?",
        url: "/ItemCodeCreation/DeleteVendor?index=" + index,
        okCallback: function (id, data) {
            OnVendoraddSuccess(data);
        }
    });
}
function GetMultiselectValue(options) {
    var selected = '';
    options.each(function () {
        var label = ($(this).attr('label') !== undefined) ? $(this).attr('label') : $(this).text();
        selected += label + ",";
    });
    return selected.substr(0, selected.length - 1);
}
function ValidateDelegateControls(item) {
    $(item).not('.collapse').children().find(".field-validation-error").addClass("field-validation-valid");
    $(item).not('.collapse').children().find(".field-validation-valid").removeClass("field-validation-error");
    $(item).not('.collapse').children().find(".input-validation-error").removeClass("input-validation-error");
    $(item).not('.collapse').validate().settings.ignore = "*";
    $("form").not('.disabled').validate().resetForm();
}

function OnVendorModelSubmit(frmid) {
    if ($('#VendorName').val() == "") {
        $("#" + frmid).find("ul.token-input-list").addClass('input-validation-error');
        var validationmesage = $("#" + frmid).find("ul.token-input-list").next().attr('data-val-required');
        $("#" + frmid).find("ul.token-input-list").next().next().removeClass('field-validation-valid').addClass('field-validation-error');
        $("#" + frmid).find("ul.token-input-list").next().next().attr('data-valmsg-replace', 'false');
        $("#" + frmid).find("ul.token-input-list").next().next().html(validationmesage);
    }
    else {
        if ($("#" + frmid).validate()) {
            ShowWaitDialog();
            jQuery.ajax({
                type: "GET",
                url: BASEPATHURL + "/Master/GetTocken",
                global: true,
                contentType: "application/x-www-form-urlencoded;charset=UTF-8",
                async: true,
                cache: false,
                success: function (result) {
                    securityToken = result;
                    $("#" + frmid).submit();

                },
                error: function (xhr, textStatus, errorThrown) {
                    HideWaitDialog();
                    onAjaxError(xhr);
                }

            });
        }
    }
}
function VendorAdded(ele, id, text, items) {
    ShowWaitDialog();
    $(ele).val(decodeURIComponent(text));
    if (items != undefined) {
        var item = JSON.parse(items);
        hdnVenodrsId = hdnVenodrsId + item.ID + ",";
        $('#hdnVendors').val(hdnVenodrsId);
        $('#Vendors').val(hdnVenodrsId);
    }
    HideWaitDialog();
}

function VendorRemoved(ele) {
    //$(ele).tokenInput("clear");
    hdnVenodrsId = "";
    $('#hdnVendors').val($(ele).val());
    $('#Vendors').val($(ele).val());
}

