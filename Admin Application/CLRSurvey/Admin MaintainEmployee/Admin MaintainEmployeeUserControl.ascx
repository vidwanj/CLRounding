<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Admin MaintainEmployeeUserControl.ascx.cs"
    Inherits="CLRSurvey.Admin_MaintainEmployee.Admin_MaintainEmployeeUserControl" %>
<link href="../../../_layouts/CLRSurvey/main.css" rel="stylesheet" type="text/css" />
<style type="text/css">
    .ms-inputuserfield
    {
        border: 1px solid gray !important;
    }
</style>
<script language="javascript" type="text/javascript">
    function updateSuccess() {
        alert("Employee Updated Successfully...!");
        window.location = "./Employees.aspx";
    }
    function createSuccess() {
        alert("Employee Created Successfully...!");
        window.location = "./Employees.aspx";
    }
</script>
<div>
    <table border="0" align="center" cellpadding="0" cellspacing="0" class="table-border">
        <tr>
            <td colspan="2" class="form-header">
                Employee Info
            </td>
        </tr>
        <tr>
            <td class="label-bg">
            </td>
            <td class="text-field-bg">
                <p class="info">
                    (Fields marked with <span class="required">*</span> are mandatory)</p>
            </td>
        </tr>
        <tr id="trddlDepartment" runat="server">
            <td align="left" class="label-bg">
                <asp:Label ID="lblDepartment" runat="server" Text="Department"></asp:Label>&nbsp;&nbsp;
            </td>
            <td class="text-field-bg">
                <asp:DropDownList ID="ddlDepartment" runat="server" Width="150px">
                </asp:DropDownList>
                <span id="Span4" style="color: Red; vertical-align: top;" runat="server">*</span>&nbsp;
                <asp:RequiredFieldValidator ID="rfvddlDepartment" ControlToValidate="ddlDepartment"
                    InitialValue="--Select--" runat="server" ErrorMessage="Select Department"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="left" class="label-bg">
                <asp:Label ID="lblEmployeeID" runat="server" Text="Employee ID"></asp:Label>&nbsp;&nbsp;
            </td>
            <td class="text-field-bg">
                <SharePoint:PeopleEditor ID="ppleditEmployeeID" RemoveText="Remove" EnableViewState="true"
                    value="ppleditEmployeeID" NoMatchesText="&lt;No Matching Names>" MoreItemsText="More Names..."
                    PreferContentEditableDiv="true" ShowDataValidationErrorBorder="false" EEAfterCallbackClientScript=""
                    inValidate="ture" ShowEntityDisplayTextInTextBox="false" excludeFromSpellCheck="true"
                    Style="border-collapse: collapse;" CssClass="ms-usereditor" runat="server" IsValid="true"
                    AllowEmpty="true" Height="18px" Width="245px" MultiSelect="false" 
                    TabIndex="1"/>
                <asp:TextBox ID="txtEmployeeID" runat="server" Visible="false"></asp:TextBox>
                <span id="Span2" runat="server" style="color: Red; vertical-align: top;">*</span>
                <asp:Label ID="lblppleditEmployeeID" runat="server" ForeColor="Red" Text=" Employee ID Required"
                    Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left" class="label-bg">
                <asp:Label ID="lblEmployeeRole" runat="server" Text="Employee Role"></asp:Label>&nbsp;&nbsp;
            </td>
            <td class="text-field-bg">
                <asp:TextBox ID="txtEmployeeRole" MaxLength="100" TabIndex="3" Style="border: 1px solid black;"
                    runat="server" Width="240px"></asp:TextBox>
                <span id="Span3" style="color: Red; vertical-align: top;" runat="server">*</span>&nbsp;
                <asp:RequiredFieldValidator ID="ReqValidTxtEmployeeRole" ControlToValidate="txtEmployeeRole"
                    runat="server" ErrorMessage="Role is required"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="text-field-bg">
            </td>
            <td class="text-field-bg">
                &nbsp;&nbsp
                <asp:ImageButton ID="ImgbtnSave" runat="server" 
                    ImageUrl="~/_layouts/images/CLRSurvey/save.png" onclick="ImgbtnSave_Click" />
                &nbsp;&nbsp
                <asp:ImageButton ID="ImgbtnClear" runat="server" 
                    ImageUrl="~/_layouts/images/CLRSurvey/clear.png" 
                    onclick="ImgbtnClear_Click" CausesValidation="False" />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="text-field-bg" align="center">
                <asp:Label ID="lblMsg" runat="server" Visible="false" Font-Bold="true"></asp:Label>
            </td>
        </tr>
    </table>
</div>
