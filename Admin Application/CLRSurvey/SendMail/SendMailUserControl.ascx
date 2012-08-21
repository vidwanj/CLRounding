<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SendMailUserControl.ascx.cs" Inherits="CLRSurvey.SendMail.SendMailUserControl" %>
<div>
    <table align="center">
        <tr>
            <td>
                <asp:TextBox ID="txtbody" runat="server" Height="459px" TextMode="MultiLine" 
                    Width="573px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnSendMail" runat="server" Text="Send Mail" 
                    onclick="btnSendMail_Click" />
            </td>
        <tr>
            <td align="center">
                <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        </tr>
    </table>
</div>