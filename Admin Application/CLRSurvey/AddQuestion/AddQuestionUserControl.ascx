<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddQuestionUserControl.ascx.cs"
    Inherits="CLRSurvey.AddQuestion.AddQuestionUserControl" %>
<link href="../../../_layouts/CLRSurvey/main.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    function countCharacters() {
        var count = parseInt(document.getElementById('<%=txtQuestion.ClientID%>').value.length);
        document.getElementById('<%=lblCounter.ClientID%>').innerHTML = (120 - count) + " Characters Left";
    }

    function ClosePopup() {
        window.frameElement.commitPopup();
    }
</script>
<div>
    <table border="0" align="center" cellpadding="0" cellspacing="0" class="table-border">
        <tr>
            <td colspan="2" class="form-header">
                Survey Question
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
        <tr>
            <td class="label-bg">
                <asp:Label ID="lblQuestion" runat="server" Text="Question"></asp:Label>
            </td>
            <td class="text-field-bg">
                <asp:TextBox ID="txtQuestion" TabIndex="1" runat="server" Width="360px" MaxLength="120"></asp:TextBox>
                <span class="required">*</span>&nbsp;
                <asp:Label ID="lblCounter" runat="server"></asp:Label>&nbsp;
                <asp:RequiredFieldValidator ID="ReqValidTxtQuestion" runat="server" ControlToValidate="txtQuestion"
                    ErrorMessage="Question is required"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="label-bg">
                <asp:Label ID="lblAbbrvtn" runat="server" Text="Abbreviation"></asp:Label>&nbsp;&nbsp;
            </td>
            <td class="text-field-bg">
                <asp:TextBox ID="txtAbbreviation" TabIndex="2" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                <span class="required">*</span>&nbsp;
                <asp:RequiredFieldValidator ID="ReqValidTxtAbbrvtn" ControlToValidate="txtAbbreviation"
                    runat="server" ErrorMessage="Abbreviation is required"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="label-bg">
                <asp:Label ID="lblComment" runat="server" Text="Comment"></asp:Label>&nbsp;&nbsp;
            </td>
            <td class="text-field-bg">
                <asp:TextBox ID="txtComment" TabIndex="3" MaxLength="2000" runat="server" Width="360px"
                    Height="50px" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="label-bg">
                <asp:Label ID="lblActvn" runat="server" Text="Active / Inactive"></asp:Label>&nbsp;&nbsp;
            </td>
            <td class="text-field-bg">
                <asp:DropDownList ID="ddlActvnInactvn" runat="server" Width="80px"
                    TabIndex="4">
                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                </asp:DropDownList>
                <span class="required">*</span>&nbsp;
                <asp:RequiredFieldValidator ID="ReqValidDdlActv" ControlToValidate="ddlActvnInactvn"
                    runat="server" ErrorMessage="Please select Yes/No"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="label-bg">
                <asp:Label ID="lblTcktId" runat="server" Text="Ticket Type Code"></asp:Label>&nbsp;&nbsp;
            </td>
            <td class="text-field-bg">
                <asp:TextBox ID="txtTicktCode" runat="server" MaxLength="5" TabIndex="5" Width="105px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="label-bg">
                <asp:Label ID="lblResponseCode" runat="server" Text="Response Type Code"></asp:Label>&nbsp;&nbsp;
            </td>
            <td class="text-field-bg">
                <asp:DropDownList ID="ddlResponseCode" runat="server" Width="110px"
                    TabIndex="6">
                    <asp:ListItem Text="Acknowledge" Value="10"></asp:ListItem>
                    <asp:ListItem Text="Yes/No" Value="20"></asp:ListItem>
                </asp:DropDownList>
                <span class="required">*</span>
                &nbsp;
                <asp:RequiredFieldValidator ID="ReqValidTxtRespn" ControlToValidate="ddlResponseCode"
                    runat="server" ErrorMessage="Response Type Code is required "></asp:RequiredFieldValidator>
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
