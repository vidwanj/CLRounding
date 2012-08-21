<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Admin MaintainDeptUserControl.ascx.cs"
    Inherits="CLRSurvey.Admin_MaintainDept.Admin_MaintainDeptUserControl" %>
<link href="../../../_layouts/CLRSurvey/main.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    function countCharacters() {
        var count = parseInt(document.getElementById('<%=txtDeptname.ClientID%>').value.length);
        document.getElementById('<%=lblCounter.ClientID%>').innerHTML = (100 - count) + " Characters Left";
    }
</script>
<div>
    <table border="0" align="center" cellpadding="0" cellspacing="0" class="table-border">
        <tr>
            <td colspan="2" align="center" class="form-header">
                Department Info
            </td>
        </tr>
        <tr>
            <td class="label-bg" style="width:20%">
            </td>
            <td class="text-field-bg">
                <p class="info">
                    (Fields marked with <span class="required">*</span> are mandatory)</p>
            </td>
        </tr>
        <tr style="display:none">
            <td align="left" class="label-bg" style="width:25%">
                <asp:Label ID="lblDeptID" runat="server" Text="Department ID"></asp:Label>&nbsp;&nbsp;
            </td>
            <td class="text-field-bg">
                <asp:TextBox ID="txtDeptID" TabIndex="3" MaxLength="10" runat="server" Enabled="false"
                    Width="120px"></asp:TextBox>
                <span id="Span6" style="color: Red; vertical-align: top;" runat="server">*</span>&nbsp;
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtDeptID"
                    runat="server" ErrorMessage="Dept. ID is required"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="left" class="label-bg" style="width:25%">
                <asp:Label ID="lblDeptName" runat="server" Text="Department Name"></asp:Label>&nbsp;&nbsp;
            </td>
            <td class="text-field-bg">
                <asp:TextBox ID="txtDeptname" TabIndex="1" runat="server" Width="360px" MaxLength="100"></asp:TextBox>
                <span id="Span1" style="color: Red; vertical-align: top;" runat="server">*</span>&nbsp;
                <asp:RequiredFieldValidator ID="ReqValidTxtQuestion" runat="server" ControlToValidate="txtDeptname"
                    ErrorMessage="Department Name is required"></asp:RequiredFieldValidator>&nbsp;
                <asp:Label ID="lblCounter" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left" class="label-bg" style="width:25%">
                <asp:Label ID="lblAbbrvtn" runat="server" Text="Abbreviation"></asp:Label>&nbsp;&nbsp;
            </td>
            <td class="text-field-bg">
                <asp:TextBox ID="txtAbbreviation" TabIndex="2" MaxLength="50" runat="server" Width="185px"></asp:TextBox>
                <span id="Span2" style="color: Red; vertical-align: top;" runat="server">*</span>&nbsp;
                <asp:RequiredFieldValidator ID="ReqValidTxtAbbrvtn" ControlToValidate="txtAbbreviation"
                    runat="server" ErrorMessage="Abbreviation is required"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="left" class="label-bg" style="width:25%">
                <asp:Label ID="lblLocation" runat="server" Text="Location"></asp:Label>&nbsp;&nbsp;
            </td>
            <td class="text-field-bg">
                <asp:DropDownList ID="ddlLocation" runat="server" Width="130px" TabIndex="4">
                </asp:DropDownList>
                <span id="Span4" style="color: Red; vertical-align: top;" runat="server">*</span>&nbsp;
                <asp:RequiredFieldValidator ID="ReqValidDdlActv" InitialValue="--Select--" ControlToValidate="ddlLocation"
                    runat="server" ErrorMessage="Please select Location"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="left" class="label-bg" style="width:25%">
                <asp:Label ID="lblActvn" runat="server" Text="Active / Inactive"></asp:Label>&nbsp;&nbsp;
            </td>
            <td class="text-field-bg">
                <asp:DropDownList ID="ddlActvnInactvn" runat="server" Width="130px"
                    TabIndex="5">
                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                </asp:DropDownList>
                <span id="Span3" style="color: Red; vertical-align: top;" runat="server">*</span>&nbsp;
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" InitialValue="0" ControlToValidate="ddlActvnInactvn"
                    runat="server" ErrorMessage="Please select Yes/No"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="left" class="label-bg" style="width:25%">
                <asp:Label ID="lblSvcLine" runat="server" Text="Svc Line"></asp:Label>&nbsp;&nbsp;
            </td>
            <td class="text-field-bg">
                <asp:DropDownList ID="ddlSvcLine" runat="server" Width="185px" TabIndex="6">
                </asp:DropDownList>
                <span id="Span5" style="color: Red; vertical-align: top;" runat="server">*</span>&nbsp;
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" InitialValue="--Select--"
                    ControlToValidate="ddlSvcLine" runat="server" ErrorMessage="Please select Svc Line"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="text-field-bg" style="width:25%">
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
            <td align="center" colspan="2" class="text-field-bg">
                <asp:Label ID="lblMsg" runat="server" Visible="false" Font-Bold="true"></asp:Label>
            </td>
        </tr>
    </table>
</div>
