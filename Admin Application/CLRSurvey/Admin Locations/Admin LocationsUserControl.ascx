<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Admin LocationsUserControl.ascx.cs"
    Inherits="CLRSurvey.Admin_Locations.Admin_LocationsUserControl" %>
<link href="../../../_layouts/CLRSurvey/main.css" rel="stylesheet" type="text/css" />
<script src="../../../_layouts/CLRSurvey/jquery-1.4.1.js" type="text/javascript"></script>
<script type='text/javascript'>
    _spOriginalFormAction = document.forms[0].action;
    _spSuppressFormOnSubmitWrapper = true;   
</script>
<script type="text/javascript" language="javascript">
    function openAddLocationWindowNewMode(entityClientID) {
        var entityIDValue = document.getElementById(entityClientID).value;
        var options;
        options = { url: "./MaintainLocation.aspx?EntityID=" + entityIDValue, title: "Add New Location" };
        options.dialogReturnValueCallback = Function.createDelegate(null, closeDialogCallback);
        SP.UI.ModalDialog.showModalDialog(options);

    }

    function openAddDepartment(locationname, locationID) {
        var options;
        options = { url: "MaintainDept.aspx?locname=" + locationname + "&locID=" + locationID, width:700, title: "Add New Department" };
        options.dialogReturnValueCallback = Function.createDelegate(null, closeDialogCallback);
        SP.UI.ModalDialog.showModalDialog(options);

    }
    function openAddLocationWindowEditMode(lblLocatnID) {
        var options;
        options = { url: "./MaintainLocation.aspx?locatnid=" + lblLocatnID, title: "Update Location" };
        options.dialogReturnValueCallback = Function.createDelegate(null, closeDialogCallback);
        SP.UI.ModalDialog.showModalDialog(options);
    }


    function maintainAdminHome(lblLocatnID, lblEntityID) {
        var options;
        options = { url: "./MaintainHome.aspx?locatnid=" + lblLocatnID + "&EntityID=" + lblEntityID, title: "Administration for Location manager" };
        options.dialogReturnValueCallback = Function.createDelegate(null, closeDialogCallback);
        SP.UI.ModalDialog.showModalDialog(options);
    }

    function closeDialogCallback(result, target) {
        SP.UI.ModalDialog.RefreshPage(1);
    }

    function changeArrowImg() {
        var imgArr = document.getElementsByTagName("img");
        for (var i = 0; i < imgArr.length; i++) {

            var strsrc = imgArr[i].src;
            if (imgArr[i].alt == "" && strsrc.indexOf("/_layouts/images/sortup.gif") != -1) {
                imgArr[i].src = "../../../_layouts/images/CLRSurvey/sortup-m.gif";
            }
            if (imgArr[i].alt == "" && strsrc.indexOf("/_layouts/images/sortdown.gif") != -1) {
                imgArr[i].src = "../../../_layouts/images/CLRSurvey/sortdown-m.gif";
            }
        }
    }

</script>
<style type="text/css">
    .GridRowStyle
    {
        background-color: #eff8fb;
        border: 1px solid lightgray;
        vertical-align: middle;
        height: 25px;
        padding: 5px;
    }
    .GridAlternetRowStyle
    {
        background-color: #eff8fb;
        border: 1px solid lightgray;
        vertical-align: middle;
        height: 25px;
        padding: 5px;
    }
    .ImageStyle
    {
        background-image: :url(../../../_layouts/images/CLRSurvey/table-header-bg.png) !important;
    }
</style>
<div>
    <table width="750px" border="0" align="center" cellpadding="0" cellspacing="0" class="table-border">
        <tr>
            <td colspan="2" align="center" class="form-header">
                Location Details
            </td>
        </tr>
        <tr>
            <td>
                <table border="0" align="center" cellpadding="0" cellspacing="0" class="table-border">
                    <tbody class="spacing">
                        <tr>
                            <td colspan="2" class="form-sub-header">
                                Location Filter
                            </td>
                        </tr>
                        <tr align="center">
                            <td align="right" style="width:50%" class="sub-label-bg">
                                <asp:Label ID="lblEntityID" runat="server" Text="Select Entity"></asp:Label>
                            </td>
                            <td align="left" class="text-field-bg">
                                <asp:DropDownList ID="ddlEntityID" runat="server" AutoPostBack="true" Width="150px" TabIndex="1"
                                    Font-Size="Small" OnSelectedIndexChanged="ddlEntiyID_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right" class="text-field-bg">
                <img src="/_layouts/images/CALADD.GIF" alt="image" style="text-align: center" />&nbsp;<a
                   id="anchorAddnew" style="cursor: hand" runat="server">Add New Location</a>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="text-field-bg">
                <SharePoint:SPGridView ID="SPgrdLocation" runat="server" AllowFiltering="true" AllowSorting="true"
                    UseAccessibleHeader="true" FilteringIsCaseSensitive="false" CssClass="ms-listviewtable"
                    HeaderStyle-CssClass="ms-viewheadertr" FilterDataFields="LocatnID, EntityID, LocatnName, LocatnAbbrvtnName, ActvInactvInd, DefltSortOrdr"
                    FilteredDataSourcePropertyName="FilterExpression" FilteredDataSourcePropertyFormat="{1} = '{0}'"
                    EnableTheming="true" AutoGenerateColumns="false" OnSorting="SPgrdLocation_Sorting"
                    RowStyle-CssClass="GridRowStyle" AlternatingRowStyle-CssClass="GridAlternetRowStyle"
                    OnRowDataBound="SPgrdLocation_RowDataBound" AllowPaging="True" OnPageIndexChanging="OnViewPageChanging"
                    EmptyDataText="There are no items to show in Location." OnPreRender="SPgrdLocation_PreRender">
                    <AlternatingRowStyle CssClass="GridAlternetRowStyle" />
                    <HeaderStyle CssClass="ms-viewheadertr" />
                    <Columns>
                        <asp:TemplateField HeaderText="Location ID" Visible="false" SortExpression="LocatnID">
                            <ItemTemplate>
                                <asp:Label ID="lblLocatnID" runat="server" Text='<%# Bind("LocatnID") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Font-Bold="True" ForeColor="White" HorizontalAlign="Center" Width="100px" />
                            <ItemStyle Width="100px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Location Name" SortExpression="LocatnName">
                            <ItemTemplate>
                                <asp:Label ID="lblLocatnName" runat="server" Text='<%# Bind("LocatnName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Font-Bold="True" ForeColor="White" HorizontalAlign="Center" Width="200px" />
                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Abbreviation" SortExpression="LocatnAbbrvtnName">
                            <ItemTemplate>
                                <asp:Label ID="lblLocatnAbbrvtnName" runat="server" Text='<%# Bind("LocatnAbbrvtnName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Font-Bold="True" ForeColor="White" HorizontalAlign="Center" Width="150px" />
                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Entity ID" SortExpression="EntityID">
                            <ItemTemplate>
                                <asp:Label ID="lblEntityID" runat="server" Text='<%# Bind("EntityID") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Font-Bold="True" ForeColor="White" HorizontalAlign="Center" Width="100px" />
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Is Active" SortExpression="ActvInactvInd">
                            <ItemTemplate>
                                <asp:Label ID="lblIsActive" runat="server" Text='<%# Bind("ActvInactvInd") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Font-Bold="True" ForeColor="White" HorizontalAlign="Center" Width="100px" />
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sort Order" SortExpression="DefltSortOrdr">
                            <ItemTemplate>
                                <asp:Label ID="lblDefltSortOrdr" runat="server" Text='<%# Bind("DefltSortOrdr") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Font-Bold="True" ForeColor="White" HorizontalAlign="Center" Width="100px" />
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <ControlStyle />
                            <ItemTemplate>
                                <asp:Label ID="lblAddDepartment" runat="server" Text='Add Department'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Font-Bold="True" ForeColor="White" HorizontalAlign="Center" Width="200px"
                                CssClass="ImageStyle" />
                            <ItemStyle HorizontalAlign="Center" Width="200px" />
                        </asp:TemplateField>
                    </Columns>
                    <SelectedRowStyle CssClass="ms-selectednav" Font-Bold="True" />
                    <RowStyle CssClass="GridRowStyle" />
                </SharePoint:SPGridView>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right" class="text-field-bg">
                <img src="/_layouts/images/CALADD.GIF" alt="image" style="text-align: center" />&nbsp;<a
                   id="anchorAddNew1" style="cursor: hand" runat="server">Add New Location</a>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="text-field-bg" align="center">
                <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
            </td>
        </tr>
    </table>
</div>
