<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Admin MaintainRoomUserControl.ascx.cs"
    Inherits="CLRSurvey.Admin_MaintainRoom.Admin_MaintainRoomUserControl" %>
<link href="../../../_layouts/CLRSurvey/main.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    function countCharacters() {
        var count = parseInt(document.getElementById('<%=txtRoomDescr.ClientID%>').value.length);
        document.getElementById('<%=lblCounter.ClientID%>').innerHTML = (50 - count) + " Characters Left";
    }
</script>
<div>
    <table border="0" align="center" cellpadding="0" cellspacing="0" class="table-border">
        <tr>
            <td colspan="2" class="form-header">
                Room Info
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
            <td align="left" class="label-bg">
                <asp:Label ID="lblRoomName" runat="server" Text="Room Description"></asp:Label>
            </td>
            <td class="text-field-bg">
                <asp:TextBox ID="txtRoomDescr" TabIndex="1" runat="server" Width="290px" MaxLength="50"
                    Font-Names="Arial, Helvetica, sans-serif" Font-Size="12px"></asp:TextBox>
                <span id="Span1" style="color: Red; vertical-align: top;" runat="server">*</span>&nbsp;
                <asp:Label ID="lblCounter" runat="server"></asp:Label>&nbsp;
                <asp:RequiredFieldValidator ID="ReqValidTxtQuestion" runat="server" ControlToValidate="txtRoomDescr"
                    ErrorMessage="Room Description is required"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="left" class="label-bg">
                <asp:Label ID="lblComment" runat="server" Text="Comment"></asp:Label>&nbsp;&nbsp;
            </td>
            <td class="text-field-bg">
                <asp:TextBox ID="txtComment" TabIndex="2" runat="server" Width="290px" MaxLength="500"
                    TextMode="MultiLine"></asp:TextBox>
                <span id="Span2" style="color: Red; vertical-align: top;" runat="server">*</span>&nbsp;
                <asp:RequiredFieldValidator ID="ReqValidTxtAbbrvtn" ControlToValidate="txtComment"
                    runat="server" ErrorMessage="Comment is required"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="left" class="label-bg">
                <asp:Label ID="lblDept" runat="server" Text="Department"></asp:Label>&nbsp;&nbsp;
            </td>
            <td class="text-field-bg">
                <asp:DropDownList ID="ddlDept" runat="server" Width="185px" TabIndex="3">
                </asp:DropDownList>
                <span id="Span4" style="color: Red; vertical-align: top;" runat="server">*</span>&nbsp;
                <asp:RequiredFieldValidator ID="ReqValidDdlActv" InitialValue="--Select--" ControlToValidate="ddlDept" runat="server"
                    ErrorMessage="Please select Department"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="left" class="label-bg">
                <asp:Label ID="lblActvn" runat="server" Text="Active / Inactive"></asp:Label>&nbsp;&nbsp;
            </td>
            <td class="text-field-bg">
                <asp:DropDownList ID="ddlActvnInactvn" runat="server" Width="80px"
                    TabIndex="4">
                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                </asp:DropDownList>
                <span id="Span3" style="color: Red; vertical-align: top;" runat="server">*</span>&nbsp;
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="ddlActvnInactvn"
                    runat="server" ErrorMessage="Please select Yes/No"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="left" class="label-bg">
                <asp:Label ID="lblRoomType" runat="server" Text="Room Type"></asp:Label>&nbsp;&nbsp;
            </td>
            <td class="text-field-bg">
                <asp:DropDownList ID="ddlRoomType" runat="server" Width="155px" TabIndex="5">
                    <asp:ListItem Text="Private" Value="10"></asp:ListItem>
                    <asp:ListItem Text="Semi-Private" Value="20"></asp:ListItem>
                    <asp:ListItem Text="Other" Value="999"></asp:ListItem>
                </asp:DropDownList>
                <span id="Span5" style="color: Red; vertical-align: top;" runat="server">*</span>&nbsp;
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="ddlRoomType"
                    runat="server" ErrorMessage="Please select Room Type"></asp:RequiredFieldValidator>
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
            <td colspan="2" align="center" class="text-field-bg">
                <asp:Label ID="lblMsg" runat="server" Visible="false" Font-Bold="true"></asp:Label>
            </td>
        </tr>
    </table>
</div>
