var ICDMReportExportToExcelUrl = BASEPATHURL + "/Report/ICDMReportExportToExcel";
var ICDMReportSearchUrl = "/Report/ICDMReportSearch";

$(document).ready(function () {
    BindDatePicker('');
    AddRemoveClass();
    var aDayBefore = new Date();
    aDayBefore.setDate(aDayBefore.getDate() - parseInt(jQuery('#hdnDaysLimit').val()));

    $('#FromDatePicker').data("DateTimePicker").date(aDayBefore);
    $('#ToDatePicker').data("DateTimePicker").date(new Date());

    $('#Status,#PendingWith').multiselect({
        includeSelectAllOption: true
    });

    $(document).on('click', '#btnSearch', function () {
        AddRemoveClass();
        Search();
    });

    $(document).on('click', '#btnICDMExportToExcel', function () {
        AddRemoveClass();
        ICDMReportExportToExcel();
    });
    Search();
});

function AddRemoveClass() {
    jQuery('#lblFromDateError').addClass('hidden');
    jQuery('#lblToDateError').addClass('hidden');
    jQuery('#lblToDateRangeError').addClass('hidden');
    jQuery('#FromDatePicker').removeClass('input-validation-error');
    jQuery('#ToDatePicker').removeClass('input-validation-error');
}

function SearchReportAjaxCall(url, postData) {
    AjaxCall({
        url: url,
        postData: JSON.stringify({ report: postData }), httpmethod: 'POST', calldatatype: 'HTML', contentType: 'application/json; charset=utf-8', sucesscallbackfunction: function (data) {
            jQuery('#tblReport').html(data);
            BindDataTable();
        }
    });
}

function filterCriteria(filterControlId) {
    var controlID = '#' + filterControlId;
    var filterValue = '';
    jQuery(controlID + " option:selected").each(function (i, e) {
        filterValue = filterValue + jQuery(e).text().trim() + ",";
    });
    return filterValue != '' ? filterValue.substring(0, filterValue.length - 1) : '';
}

function filterCriteriaByValue(filterControlId) {
    var controlID = '#' + filterControlId;
    var filterValue = '';
    jQuery(controlID + " option:selected").each(function (i, e) {
        filterValue = filterValue + jQuery(e).val().trim() + ",";
    });
    return filterValue != '' ? filterValue.substring(0, filterValue.length - 1) : '';
}

function Search() {
    if (ValidatePageControl()) {
        var postData = {
            fromDate: jQuery("#FromDate").val(),
            toDate: jQuery("#ToDate").val(),
            status: filterCriteriaByValue('Status'),
            pendingWith: filterCriteria('PendingWith')
        }

        SearchReportAjaxCall(ICDMReportSearchUrl, postData);
    }
}

function ICDMReportExportToExcel() {
    if (ValidatePageControl()) {
        var fromDate = jQuery("#FromDate").val();
        var toDate = jQuery("#ToDate").val();
        var status = filterCriteriaByValue("Status");
        var pendingWith = filterCriteria('PendingWith');
        window.open(ICDMReportExportToExcelUrl + "?fromDate=" + fromDate + "&toDate=" + toDate + "&status=" + status + "&pendingWith=" + pendingWith, '_self');
        // window.open(ICDMReportExportToExcelUrl + "?fromDate=" + fromDate + "&toDate=" + toDate + "&pendingWith=" + pendingWith, '_self');
    }
}

function ValidatePageControl() {
    var flag = true;
    if (jQuery("#FromDate").val() == '') {
        jQuery('#FromDatePicker').addClass('input-validation-error');
        jQuery('#lblFromDateError').removeClass('hidden');
        flag = false;
    }
    if (jQuery("#ToDate").val() == '') {
        jQuery('#ToDatePicker').addClass('input-validation-error');
        jQuery('#lblToDateError').removeClass('hidden');
        flag = false;
    }
    if (jQuery("#FromDate").val() != '' && jQuery("#ToDate").val() != '') {
        var job_start_date = jQuery("#FromDate").val(); // Oct 1, 2014
        var job_end_date = jQuery("#ToDate").val(); // Nov 1, 2014
        job_start_date = job_start_date.split('/');
        job_end_date = job_end_date.split('/');

        var new_start_date = new Date(job_start_date[2], job_start_date[1] - 1, job_start_date[0]);
        var new_end_date = new Date(job_end_date[2], job_end_date[1] - 1, job_end_date[0]);


        if (new_start_date > new_end_date) {
            jQuery('#ToDatePicker').addClass('input-validation-error');
            jQuery('#lblToDateRangeError').removeClass('hidden');
            flag = false;
        }
    }
    return flag;
}



$(document).ready(function () {

    BindDataTable();

});

function BindDataTable() {
    $("#ItemCodeReports").DataTable({
        "paging": true,
        "searching": false,
        "info": false,
        "order": [[1, "asc"]],
        "fixedHeader": true,
        "autoWidth": false
    });
}


