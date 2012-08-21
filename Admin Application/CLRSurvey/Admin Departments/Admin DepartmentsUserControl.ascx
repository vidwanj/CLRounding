<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Admin DepartmentsUserControl.ascx.cs"
    Inherits="CLRSurvey.Admin_Departments.Admin_DepartmentsUserControl" %>
<link href="../../../_layouts/CLRSurvey/main.css" rel="stylesheet" type="text/css" />
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
    .NoImageHeader
    {
        background-image: url(../../../_layouts/images/CLRSurvey/table-header-bg.png) !important;
    }
</style>
<script type="text/javascript" language="javascript">

    function OpenDialog(LocatnClientID) {
        var options;
        var LocatnIDvalue = document.getElementById(LocatnClientID).value;
        options = { url: "./MaintainDept.aspx?locid=" + LocatnIDvalue, width: 700, title: "Department Details" };
        options.dialogReturnValueCallback = Function.createDelegate(null, closeDialogCallback);
        SP.UI.ModalDialog.showModalDialog(options);
    }
    function OpenDepartmentWindowEditMode(strUrl) {
        var options;
        options = { url: strUrl, width: 700, title: "Department Details" };
        options.dialogReturnValueCallback = Function.createDelegate(null, closeDialogCallback);
        SP.UI.ModalDialog.showModalDialog(options);
    }
    function openAddRoomWindowNewMode(DeptName) {
        var options;
        options = { url: "./MaintainRoom.aspx?DeptName=" + DeptName, width: 700, title: "Add New Room" };
        options.dialogReturnValueCallback = Function.createDelegate(null, closeDialogCallback);
        SP.UI.ModalDialog.showModalDialog(options);
    }
    function openAddEmployeeWindowNewMode(DeptName) {
        var options;
        options = { url: "./MaintainEmployee.aspx?DeptName=" + DeptName, width: 600, title: "Add New Employee" };
        options.dialogReturnValueCallback = Function.createDelegate(null, closeDialogCallback);
        SP.UI.ModalDialog.showModalDialog(options);
    }

    function closeDialogCallback(result, target) {

        SP.UI.ModalDialog.RefreshPage(1);
    }
</script>
<script type="text/javascript" language="javascript">

    function changeArrowImg() {
        // debugger;
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
<div>
    <table width="750px" border="0" align="center" cellpadding="0" cellspacing="0" class="table-border"
        summary="Table for text fields">
        <tbody class="spacing">
            <tr>
                <td colspan="2" class="form-header">
                    Department Details
                </td>
            </tr>
            <tr>
                <td>
                    <table border="0" align="center" cellpadding="0" cellspacing="0" class="table-border">
                        <tbody class="spacing">
                            <tr>
                                <td colspan="2" class="form-sub-header">
                                    Department Filter
                                </td>
                            </tr>
                            <tr align="center">
                                <td align="right" style="width: 50%" class="sub-label-bg">
                                    Select Entity
                                </td>
                                <td align="left" class="text-field-bg">
                                    <asp:DropDownList ID="ddlEntity" runat="server" Width="150px" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlEntity_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr align="center">
                                <td align="right" style="width: 50%" class="sub-label-bg">
                                    Select Location
                                </td>
                                <td align="left" class="text-field-bg">
                                    <asp:DropDownList ID="ddlLocation" runat="server" Width="150px" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right" class="text-field-bg">
                    <img src="/CLRSurvey/_layouts/images/CALADD.GIF" style="text-align: center" alt="Add Image" />&nbsp;
                    <a style="cursor: hand" id="anchorAddnew" runat="server" >Add New Department</a><br />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <SharePoint:SPGridView ID="spGrdDeptList" runat="server" AllowFiltering="true" AllowSorting="true"
                        UseAccessibleHeader="true" FilteringIsCaseSensitive="false" CssClass="ms-listviewtable"
                        HeaderStyle-CssClass="ms-viewheadertr" FilterDataFields="DeptID,Name,DeptAbbrvtnName,ActvInactvInd"
                        FilteredDataSourcePropertyName="FilterExpression" FilteredDataSourcePropertyFormat="{1} = '{0}'"
                        EnableTheming="true" AutoGenerateColumns="false" OnSorting="spGrdDeptList_Sorting"
                        OnRowDataBound="spGrdDeptList_RowDataBound" AllowPaging="True" OnPageIndexChanging="OnViewPageChanging"
                        RowStyle-CssClass="GridRowStyle" HeaderStyle-ForeColor="White" AlternatingRowStyle-CssClass="GridAlternetRowStyle"
                        BorderWidth="1" BorderStyle="Solid" OnPreRender="spGrdDeptList_PreRender">
                        <Columns>
                            <asp:TemplateField Visible="false" HeaderText="Dept. ID" ItemStyle-Width="80px" HeaderStyle-Font-Bold="true"
                                HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Left" SortExpression="DeptID">
                                <ItemTemplate>
                                    <asp:Label ID="lblDeptID" runat="server" Text='<%# Bind("DeptID") %>' Visible="false"></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Department Name" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left"
                                HeaderStyle-Font-Bold="true" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left"
                                SortExpression="Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblDeptName" runat="server" Text='<%# Bind("Name") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Department Abbreviation Name" ItemStyle-Width="200px"
                                ItemStyle-HorizontalAlign="Left" HeaderStyle-Font-Bold="true" HeaderStyle-Width="200px"
                                HeaderStyle-HorizontalAlign="Left" SortExpression="DeptAbbrvtnName">
                                <ItemTemplate>
                                    <asp:Label ID="Label44" runat="server" Text='<%# Bind("DeptAbbrvtnName") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Is Active" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Font-Bold="true" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left"
                                SortExpression="ActvInactvInd">
                                <ItemTemplate>
                                    <asp:Label ID="lblActive" runat="server" Text='<%# Bind("ActvInactvInd") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-CssClass="NoImageHeader" HeaderStyle-Font-Bold="true" HeaderStyle-Width="150px"
                                HeaderStyle-HorizontalAlign="Left" SortExpression="DeptID">
                                <ItemTemplate>
                                    <asp:Label ID="lblAddRoom" runat="server" Text='Add Rooms'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-CssClass="NoImageHeader" HeaderStyle-Font-Bold="true" HeaderStyle-Width="150px"
                                HeaderStyle-HorizontalAlign="Left" SortExpression="DeptID">
                                <ItemTemplate>
                                    <asp:Label ID="lblAddEmployee" runat="server" Text='Add Employees'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </SharePoint:SPGridView>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right" class="text-field-bg">
                    <img src="/CLRSurvey/_layouts/images/CALADD.GIF" style="text-align: center" alt="Add Image" />&nbsp;
                    <a style="cursor: hand" id="anchorAddnew1" runat="server">Add New Department</a>
                </td>
            </tr>
            <tr>
                <td class="text-field-bg" align="center" colspan="2">
                    <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
                </td>
            </tr>
        </tbody>
    </table>
</div>
<div id="DivDialog" style="display: none; width: 500px">
    <br />
    <input type="button" value="Close" id="ButtonDialog" style="background-color: #FFFFFF" />
</div>
