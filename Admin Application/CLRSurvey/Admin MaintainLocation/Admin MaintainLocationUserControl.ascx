<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Admin MaintainLocationUserControl.ascx.cs"
    Inherits="CLRSurvey.Admin_MaintainLocation.Admin_MaintainLocationUserControl" %>
<link href="../../../_layouts/CLRSurvey/main.css" rel="stylesheet" type="text/css" />
<script language="javascript" type="text/javascript">
    function ClosePopup() {
        window.frameElement.commitPopup();
    }
</script>
<div>
    <table border="0" align="center" cellpadding="0" cellspacing="0" class="table-border">
        <tr>
            <td colspan="2" class="form-header">
                Location Info
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
        <tr style="display:none">
            <td align="left" class="label-bg">
                <asp:Label ID="lblLocationID" runat="server" Text="Location ID"></asp:Label>
            </td>
            <td class="text-field-bg">
                <asp:TextBox ID="txtLocationID" MaxLength="10" runat="server" ReadOnly="True" Enabled="False"
                    Width="100px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left" class="label-bg">
                <asp:Label ID="lblEntiyID" runat="server" Text="Entity ID"></asp:Label>
            </td>
            <td class="text-field-bg">
                <asp:DropDownList ID="ddlEntiyID" runat="server" Width="105px" TabIndex="1"
                    Font-Size="Small">
                </asp:DropDownList>
                <span id="Span3" style="color: Red; vertical-align: top;" runat="server">* </span>
                <asp:RequiredFieldValidator ID="ReqValidEntityID" ControlToValidate="ddlEntiyID"
                    runat="server" InitialValue="--Select--" ErrorMessage="Entity ID is required "></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="left" class="label-bg">
                <asp:Label ID="lblLocationName" runat="server" Text="Location Name"></asp:Label>
            </td>
            <td class="text-field-bg">
                <asp:TextBox ID="txtLocationName" TabIndex="2" runat="server" Width="150px" MaxLength="50"></asp:TextBox>
                <span id="Span5" style="color: Red; vertical-align: top;" runat="server">*</span>
                <asp:RequiredFieldValidator ID="ReqValidLocationName" ControlToValidate="txtLocationName"
                    runat="server" ErrorMessage="Location Name is required"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="left" class="label-bg">
                <asp:Label ID="lblAbbrvtn" runat="server" Text="Abbreviation"></asp:Label>
            </td>
            <td class="text-field-bg">
                <asp:TextBox ID="txtAbbreviation" TabIndex="3" runat="server" Width="150px" MaxLength="20"></asp:TextBox>
                <span id="Span2" style="color: Red; vertical-align: top;" runat="server">*</span>
                <asp:RequiredFieldValidator ID="ReqValidTxtAbbrvtn" ControlToValidate="txtAbbreviation"
                    runat="server" ErrorMessage="Abbreviation is required"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="left" class="label-bg">
                <asp:Label ID="lblActvn" runat="server" Text="Active / Inactive"></asp:Label>
            </td>
            <td class="text-field-bg">
                <asp:DropDownList ID="ddlActvnInactvn" runat="server" Width="105px"
                    TabIndex="4" Font-Size="Small">
                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                </asp:DropDownList>
                <span id="Span4" style="color: Red; vertical-align: top;" runat="server">*</span>
                <asp:RequiredFieldValidator ID="ReqValidDdlActv" ControlToValidate="ddlActvnInactvn"
                    runat="server" InitialValue="0" ErrorMessage="Activation Status is Required"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="left" class="label-bg">
                <asp:Label ID="lblSortOrder" runat="server" Text="Sort Order"></asp:Label>
            </td>
            <td class="text-field-bg">
                <asp:TextBox ID="txtSortOrder" TabIndex="5" runat="server" Width="150px"></asp:TextBox>
                <span id="Span6" style="color: Red; vertical-align: top;" runat="server">*</span>&nbsp;
                <asp:RequiredFieldValidator ID="vldrtxtSortOrder" Display="Dynamic" ControlToValidate="txtSortOrder"
                    runat="server" ErrorMessage="Sort Order Is Required"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="vldr1txtSortOrder" Display="Dynamic" ControlToValidate="txtSortOrder"
                    ValidationExpression="^\d+$" runat="server" ErrorMessage="Sort Order Should Be Integer"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td class="text-field-bg">
            </td>
            <td class="text-field-bg">
                &nbsp;&nbsp
                <asp:ImageButton ID="ImgbtnSave" runat="server" ImageUrl="~/_layouts/images/CLRSurvey/save.png"
                    OnClick="ImgbtnSave_Click" />
                &nbsp;&nbsp
                <asp:ImageButton ID="ImgbtnClear" runat="server" ImageUrl="~/_layouts/images/CLRSurvey/clear.png"
                    OnClick="ImgbtnClear_Click" CausesValidation="False" />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="text-field-bg" align="center">
                <asp:Label ID="lblMsg" runat="server" Visible="false" Font-Bold="true"></asp:Label>
            </td>
        </tr>
    </table>
</div>
