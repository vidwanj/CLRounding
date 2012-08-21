<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DeptRoomsUserControl.ascx.cs" Inherits="CLRSurvey.DeptRooms.DeptRoomsUserControl" %>
<div>
    <table width="730px">
        <tr>
            <td colspan="2" align="center">
                <h3>
                    Department Rooms</h3>
            </td>
            </tr>
        <tr>
            <td colspan="2" style="height: 30px">
            </td>
        </tr>
        <tr>
            <td align="center">
                Departments
            </td>
            <td align="center">Rooms</td>
        </tr>
        <tr>
            <td colspan="2" style="height: 5px">
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:ListBox ID="lstDept" runat="server" Width="231px" Height="151px" OnSelectedIndexChanged="lstDept_SelectedChange" AutoPostBack="true" >
                </asp:ListBox>
            </td>
            <td align="center">
                <asp:ListBox ID="lstRoom" runat="server" Width="231px" Height="151px" 
                    SelectionMode="Multiple"></asp:ListBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="height: 30px">
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
            <asp:Button ID="BtnSave" runat="server" Text="Save" Width="82px" 
                    onclick="BtnSave_Click" /></td>         
        </tr>
         <tr>
            <td colspan="2">
            </td></tr>
            <tr>
            <td colspan="2" align="center">
                <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
            </td>
            </tr>
    </table>
</div>