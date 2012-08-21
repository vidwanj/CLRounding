<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Admin RoomsUserControl.ascx.cs"
    Inherits="CLRSurvey.Admin_Rooms.Admin_RoomsUserControl" %>
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
</style>
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
    function OpenDialog(strURL) {

        //create html
        var htmlElement = document.createElement('h3');

        var dialogDiv = document.getElementById('DivDialog');
        if (dialogDiv != null) {
            dialogDiv.setAttribute('style', 'block');
            var textNode = document.createTextNode('Make code not war!');
            htmlElement.appendChild(textNode);
            htmlElement.appendChild(dialogDiv);
        }

        var options = SP.UI.$create_DialogOptions();
        options.url = strURL;
        options.title = 'Update Room Details';
        options.width = 700;       
        options.dialogReturnValueCallback = Function.createDelegate(null, closeDialogCallback);
        SP.UI.ModalDialog.showModalDialog(options);
    }
    function OpenDialogAddNew(ddlDepartmentClientID) {
        var options;
        var ddlDepartmentValue = document.getElementById(ddlDepartmentClientID).value;
        options = { url: "./MaintainRoom.aspx?DeptID=" + ddlDepartmentValue, width: 700, title: "Add New Room" };
        options.dialogReturnValueCallback = Function.createDelegate(null, closeDialogCallback);
        SP.UI.ModalDialog.showModalDialog(options);
    }
    function closeDialogCallback(result, target) {
        SP.UI.ModalDialog.RefreshPage(1);
    }
</script>
<div>
    <table width="650px" border="0" align="center" cellpadding="0" cellspacing="0" class="table-border">
        <tr>
            <td colspan="2" class="form-header">
                Room Details
            </td>
        </tr>
        <tr>
            <td>
                <table border="0" align="center" cellpadding="0" cellspacing="0" class="table-border">
                    <tbody class="spacing">
                        <tr>
                            <td colspan="2" class="form-sub-header">
                                Room Filter
                            </td>
                        </tr>
                        <tr align="center">
                            <td align="right" class="sub-label-bg">
                                Select Entity
                            </td>
                            <td align="left" class="text-field-bg">
                                <asp:DropDownList ID="ddlEntity" runat="server" AutoPostBack="true" Width="150px"
                                    OnSelectedIndexChanged="ddlEntity_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr align="center">
                            <td align="right" class="sub-label-bg">
                                Select Location 
                            </td>
                            <td align="left" class="text-field-bg">
                                <asp:DropDownList ID="ddlLocation" runat="server" AutoPostBack="true" Width="150px"
                                    OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr align="center">
                            <td align="right" class="sub-label-bg">
                                Select Department 
                            </td>
                            <td align="left" class="text-field-bg">
                                <asp:DropDownList ID="ddlDepartment" runat="server" AutoPostBack="true" Width="150px"
                                    OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right" class="text-field-bg">
                <img src="/_layouts/images/CALADD.GIF" style="text-align: center" alt="Add Image" />&nbsp;<a
                    style="cursor: hand" id="anchorAddnew" runat="server" >Add New Room</a><br />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center" class="text-field-bg">
                <SharePoint:SPGridView ID="spGrdRoomList" runat="server" AllowFiltering="true" AllowSorting="true"
                    UseAccessibleHeader="true" FilteringIsCaseSensitive="false" CssClass="ms-listviewtable"
                    HeaderStyle-CssClass="ms-viewheadertr" FilterDataFields="RoomID,RoomDescr, RoomTypeCode,ActvInactvInd"
                    FilteredDataSourcePropertyName="FilterExpression" FilteredDataSourcePropertyFormat="{1} = '{0}'"
                    EnableTheming="true" AutoGenerateColumns="false" RowStyle-CssClass="GridRowStyle"
                    AlternatingRowStyle-CssClass="GridAlternetRowStyle" HeaderStyle-ForeColor="White"
                    OnSorting="spGrdRoomList_Sorting" OnRowDataBound="spGrdRoomList_RowDataBound"
                    AllowPaging="True" OnPageIndexChanging="OnViewPageChanging" OnPreRender="spGrdRoomList_PreRender">
                    <AlternatingRowStyle CssClass="GridAlternetRowStyle" />
                    <HeaderStyle CssClass="ms-viewheadertr" />
                    <Columns>
                        <asp:TemplateField HeaderText="Room ID" SortExpression="RoomID" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="Label33" runat="server" Text='<%# Bind("RoomID") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Font-Bold="True" HorizontalAlign="Left" Width="80px" />
                            <ItemStyle Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Room Description" SortExpression="RoomDescr">
                            <ItemTemplate>
                                <asp:Label ID="Label44" runat="server" Text='<%# Bind("RoomDescr") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Font-Bold="True" HorizontalAlign="Left" Width="150px" />
                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Room Type" SortExpression="RoomTypeCode">
                            <ItemTemplate>
                                <asp:Label ID="Label55" runat="server" Text='<%# Bind("RoomTypeCode") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Font-Bold="True" HorizontalAlign="Left" Width="150px" />
                            <ItemStyle HorizontalAlign="Center" Width="150px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Is Active" SortExpression="ActvInactvInd">
                            <ItemTemplate>
                                <asp:Label ID="lblActive" runat="server" Text='<%# Bind("ActvInactvInd") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Font-Bold="True" HorizontalAlign="Left" Width="100px" />
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:TemplateField>
                    </Columns>
                    <SelectedRowStyle CssClass="ms-selectednav" Font-Bold="True" />
                </SharePoint:SPGridView>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right" class="text-field-bg">
                <img src="/_layouts/images/CALADD.GIF" style="text-align: center" />&nbsp;<a style="cursor: hand"
                    id="anchorAddnew1" runat="server">Add New Room</a>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center" class="text-field-bg">
                <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
            </td>
        </tr>
    </table>
</div>
