var GetUserListByRoleNameUrl = "/ICDMApprover/GetUserListByRoleName";
var ShowAllPendingRequestsUrl = "/ICDMApprover/ShowAllPendingRequests";
var GetChangeApproverListUrl = "/ICDMApprover/GetApproverReplacementList";

$(document).ready(function () {
    AddRemoveClass();

    $('select#RoleName').off("change").on('change', function () {
        AddRemoveClass();
        var roleName = $("#RoleName option:selected").val();
        onRoleNameChange(roleName);
    });

    $('#btnPendingRequest').off('click').on('click', function () {
        AddRemoveClass();
        ShowAllPendingRequest();
    });

    GetApproverReplacementList();

    $('#btnSearch').off('click').on('click', function () {
        GetApproverReplacementList();
    });
    //$('#btnReplaceApprover').off('click').on('click', function () {
    //    AddRemoveClass();
    //    SaveReplacedApprover();
    //});
});

function GetApproverReplacementList() {
    var roleName = $("#RoleName option:selected").val();
    var pendingWithUserName = $("#PendingWithWhom option:selected").val();
    var replaceByWhomUserName = $("#ReplaceByWhom option:selected").val();
    var jobstatus = $("#JobStatus option:selected").val();
    var data = {
        RoleName: roleName,
        PendingWithWhom: pendingWithUserName,
        ReplaceByWhom: replaceByWhomUserName,
        JobStatus: jobstatus
    }
    AjaxCall({
        url: GetChangeApproverListUrl, postData: JSON.stringify({ approverMaster: data }), httpmethod: 'POST', calldatatype: 'HTML', contentType: 'application/json; charset=utf-8',
        sucesscallbackfunction: function (result) {
            if (result != null) {
                $('#changeapproverfrommodel').html(result);
                BindReplacementDataTable();
            }
        }
    });
}

function AddRemoveClass() {
    jQuery('#lblRoleNameError').addClass('hidden');
    jQuery('#lblPendingWithWhomError').addClass('hidden');
    jQuery('#lblReplaceByWhomError').addClass('hidden');
    jQuery('#lblSameUserError').addClass('hidden');
    jQuery('#RoleName').removeClass('input-validation-error');
    jQuery('#PendingWithWhom').removeClass('input-validation-error');
    jQuery('#ReplaceByWhom').removeClass('input-validation-error');
}

function ValidatePageControl(isvalidatereplacer) {
    var flag = true;
    if (jQuery("#RoleName").val() == '') {
        jQuery('#RoleName').addClass('input-validation-error');
        jQuery('#lblRoleNameError').removeClass('hidden');
        flag = false;
    }
    if (jQuery("#PendingWithWhom").val() == '') {
        jQuery('#PendingWithWhom').addClass('input-validation-error');
        jQuery('#lblPendingWithWhomError').removeClass('hidden');
        flag = false;
    }
    if (isvalidatereplacer) {
        if (jQuery("#ReplaceByWhom").val() == '') {
            jQuery('#ReplaceByWhom').addClass('input-validation-error');
            jQuery('#lblReplaceByWhomError').removeClass('hidden');
            flag = false;
        }
        if (jQuery("#ReplaceByWhom").val() != '' && jQuery("#PendingWithWhom").val() != '') {
            if (jQuery("#ReplaceByWhom").val() == jQuery("#PendingWithWhom").val()) {
                jQuery('#ReplaceByWhom').addClass('input-validation-error');
                jQuery('#lblSameUserError').removeClass('hidden');
                flag = false;
            }
        }
    }
    return flag;
}

function ShowAllPendingRequest() {
    $('#pendingrequestListmodel').html('');
    if (ValidatePageControl(false)) {
        var roleName = $("#RoleName option:selected").val();
        var pendingWithUserName = $("#PendingWithWhom option:selected").val();

        if (roleName != "" && roleName != undefined && pendingWithUserName != "" && pendingWithUserName != undefined) {
            var data = {
                roleName: roleName,
                approverName: pendingWithUserName
            }
            AjaxCall({
                url: ShowAllPendingRequestsUrl, postData: data, httpmethod: 'GET', calldatatype: 'HTML', contentType: 'application/json; charset=utf-8',
                sucesscallbackfunction: function (result) {
                    if (result != null) {
                        $('#pendingrequestListmodel').html(result);
                        BindDataTable();
                        jQuery('#pendingrequestmodel').modal('toggle');
                    }
                }

            });
        }
    }
}

function onRoleNameChange(RoleName) {
    //   if (RoleName != "" && RoleName != undefined) {

    var data = {
        roleName: RoleName
    }
    AjaxCall({
        url: GetUserListByRoleNameUrl, postData: data, httpmethod: 'GET', calldatatype: 'JSON', contentType: 'application/json; charset=utf-8',
        sucesscallbackfunction: function (result) {
            if (result != null) {
                SetOptionValuesinCombobox('PendingWithWhom', result.PendingWithWhomUserList);
                SetOptionValuesinCombobox('ReplaceByWhom', result.ReplaceByWhomUserList);
                SetOptionValuesinCombobox('PendingWithWhoms', result.PendingWithWhomUserList);
                SetOptionValuesinCombobox('ReplaceByWhoms', result.ReplaceByWhomUserList);
            }
        }

    });


}

function SetOptionValuesinCombobox(obj, result) {
    $("#" + obj).html("<option value=''>Select</option>");
    $(result).each(function (i, item) {
        var opt = $("<option/>");
        opt.text(item.Name);
        opt.attr("value", item.Value);
        if (item.Name == $("#" + obj).text()) {
            opt.attr("selected", 'selected');
        }
        opt.appendTo("#" + obj);
    });

    $("#" + obj).trigger("change");
    var selectedItem = $("#" + obj + " option:selected").val();
    if (selectedItem != '')
        $("#" + obj).val(selectedItem);
}


function BindReplacementDataTable() {
    $("#changeapprovertable").DataTable({
        "paging": true,
        "searching": false,
        "info": false,
        "order": [[0, "desc"]],
        "fixedHeader": true,
        "autoWidth": false,
        "iDisplayLength": 100,
        "columnDefs": [{
            "targets": [5], "orderable": false,  // set orderable for selected columns  
        }],
    });
}

function BindDataTable() {
    $("#pendingrequesttable").DataTable({
        "paging": true,
        "searching": false,
        "info": false,
        "order": [[1, "desc"]],
        "fixedHeader": true,
        "autoWidth": false,
        "iDisplayLength": 100
    });
}


function OnApproverReplacementSuccess(data, status, xhr) {
    if (data.IsSucceed) {
        $("#replacementapproverModel").modal('hide');
        AlertModal('Success', ParseMessage(data.Messages));
        $('select#RoleName').val('');
        $('select#PendingWithWhom').val('');
        $('select#JobStatus').val('');
        GetApproverReplacementList();
    } else {
        AlertModal('Error', ParseMessage(data.Messages));
    }
}

function ApproverReplacementDelete(id) {
    ConfirmationDailog({
        title: "Delete Approver Replacement",
        message: "Are you sure want to delete?",
        url: "/ICDMApprover/DeleteApproverReplacement?id=" + id,
        okCallback: function (id, data) {
            OnApproverReplacementSuccess(data);
        }
    });
}

function OnApproverReplacementModelSubmit(frmid) {
    var flag = true;
    if (jQuery("#ReplaceByWhoms").val() != '' && jQuery("#PendingWithWhoms").val() != '') {
        if (jQuery("#ReplaceByWhoms").val() == jQuery("#PendingWithWhoms").val()) {
            jQuery('#ReplaceByWhoms').addClass('input-validation-error');
            jQuery('#lblSameUserError').removeClass('hidden');
            flag = false;
        }
    }
    if (flag && $("#" + frmid).valid()) {
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