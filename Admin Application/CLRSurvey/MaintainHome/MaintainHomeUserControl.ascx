<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MaintainHomeUserControl.ascx.cs"
    Inherits="CLRSurvey.MaintainHome.MaintainHomeUserControl" %>
<link href="../../../_layouts/CLRSurvey/main.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" language="javascript">
    function openDepartments() {
        var options;
        options = { url: "./Departments_LocationManager.aspx", height: 600, width: 800, title: "Departments" };
        options.dialogReturnValueCallback = Function.createDelegate(null, closeDialogCallback);
        SP.UI.ModalDialog.showModalDialog(options);

    }

    function openDepartmentRooms() {
        var options;
        options = { url: "./DeptRooms_LocationManager.aspx", height: 600, width: 800, title: "Department Rooms" };
        options.dialogReturnValueCallback = Function.createDelegate(null, closeDialogCallback);
        SP.UI.ModalDialog.showModalDialog(options);

    }

    function openDepartmentQuestions() {
        var options;
        options = { url: "./DeptQuestions_LocationManager.aspx", title: "Department Questions" };
        options.dialogReturnValueCallback = Function.createDelegate(null, closeDialogCallback);
        SP.UI.ModalDialog.showModalDialog(options);

    }

    function openDepartmentEmployees(locationID) {
        var options;
        options = { url: "./DeptEmployees_LocationManager.aspx", title: "Department Employees" };
        options.dialogReturnValueCallback = Function.createDelegate(null, closeDialogCallback);
        SP.UI.ModalDialog.showModalDialog(options);

    }

    function openQuestionsResponse() {
        var options;
        options = { url: "QuestionsResponse_LocationManager.aspx", height: 600, width: 810, title: "Question Response Summary" };
        options.dialogReturnValueCallback = Function.createDelegate(null, closeDialogCallback);
        SP.UI.ModalDialog.showModalDialog(options);
    }

    function openCLRSummaryReport() {
        var options;
        options = { url: "ClrSummaryReport_LocationManager.aspx", height: 600, width: 800, title: "CLR Summary Report" };
        options.dialogReturnValueCallback = Function.createDelegate(null, closeDialogCallback);
        SP.UI.ModalDialog.showModalDialog(options);
    }

    function closeDialogCallback(result, target) {
        SP.UI.ModalDialog.RefreshPage(0);
    }
</script>
<div>
    <table width="600px" border="0" align="center" cellpadding="0" cellspacing="0" class="table-border">
        <tr>
            <td colspan="2" class="form-header">
                Administration for Location manager
            </td>
        </tr>
        <tr>
            <td align="center" class="text-field-bg">
                <asp:ImageButton ID="ImgbtnDepartment" runat="server" ImageUrl="~/_layouts/images/CLRSurvey/department.png"
                    OnClientClick="openDepartments(); return false" />
            </td>
            <td align="center" class="text-field-bg">
                <asp:ImageButton ID="ImgbtnDepartmentRooms" runat="server" ImageUrl="~/_layouts/images/CLRSurvey/dept-rooms.png"
                    OnClientClick="openDepartmentRooms(); return false" />
            </td>
        </tr>
        <tr>
            <td align="center" class="text-field-bg">
                <asp:ImageButton ID="ImgbtnDeptQuestions" runat="server" ImageUrl="~/_layouts/images/CLRSurvey/dept-questions.png"
                    OnClientClick="openDepartmentQuestions(); return false" />
            </td>
            <td align="center" class="text-field-bg">
                <asp:ImageButton ID="ImgbtnDeptEmployess" runat="server" ImageUrl="~/_layouts/images/CLRSurvey/dept-employees.png"
                    OnClientClick="openDepartmentEmployees(); return false" />
            </td>
        </tr>        
        <tr>
            <td colspan="2" align="left" class="form-sub-header">
                Reports
            </td>
        </tr>
        <tr>
            <td colspan="2" align="left" class="sub-label-bg">
                <a href="#" onclick="openQuestionsResponse();">Question Response</a>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="left" class="sub-label-bg">
                <a href="#" onclick="openCLRSummaryReport();">CLR Summary Report</a>
            </td>
        </tr>
    </table>
</div>
