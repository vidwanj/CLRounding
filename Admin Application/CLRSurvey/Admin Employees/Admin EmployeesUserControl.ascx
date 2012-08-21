<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Admin EmployeesUserControl.ascx.cs" Inherits="CLRSurvey.Admin_Employees.Admin_EmployeesUserControl" %>
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

</script>

<script type="text/javascript" language="javascript">


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
        //options.html = htmlElement;
        options.url = strURL;
        options.title = 'Employee Details';
        options.width = 600;        
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
            Employee Details
        </td>
    </tr>
    <tr><td colspan="2" align="right" class="text-field-bg"><img src="/_layouts/images/CALADD.GIF" style="text-align:center"/>&nbsp;<a style="cursor:hand"  onclick="OpenDialog('MaintainEmployee.aspx')" >Add New Employee</a><br /></td></tr>
    <tr>
       <td colspan="2" align="center" class="text-field-bg">               
         <SharePoint:SPGridView ID="spGrdEmpList" runat="server" AllowFiltering="true" AllowSorting="true"
            UseAccessibleHeader="true" FilteringIsCaseSensitive="false" CssClass="ms-listviewtable"
            HeaderStyle-CssClass="ms-viewheadertr" FilterDataFields="EmployeeID,EmployeeName, RoleName"
            FilteredDataSourcePropertyName="FilterExpression" FilteredDataSourcePropertyFormat="{1} = '{0}'"
            EnableTheming="true" AutoGenerateColumns="false" 
            RowStyle-CssClass="GridRowStyle" 
            AlternatingRowStyle-CssClass="GridAlternetRowStyle" HeaderStyle-ForeColor="White"
            OnSorting="spGrdEmpList_Sorting" OnRowDataBound="spGrdEmpList_RowDataBound" 
            AllowPaging="True" OnPageIndexChanging="OnViewPageChanging" 
            onprerender="spGrdEmpList_PreRender" BorderWidth="1" BorderStyle="Solid">
            <AlternatingRowStyle CssClass="GridAlternetRowStyle" />
            <HeaderStyle CssClass="ms-viewheadertr" />
            <Columns>
                <asp:TemplateField HeaderText="Employee Name" SortExpression="EmployeeName">
                    <ItemTemplate>
                        <asp:Label ID="lblEmployeeName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Left" Width="200px" />
                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Employee ID" SortExpression="EmployeeID" Visible="true">
                    <ItemTemplate>
                        <asp:Label ID="lblEmployeeID" runat="server" Text='<%# Bind("EmployeeID") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Left" Width="80px" />
                    <ItemStyle Width="80px" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Employee Role" SortExpression="RoleName">
                    <ItemTemplate>
                        <asp:Label ID="lblEmpRoleName" runat="server" Text='<%# Bind("RoleName") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Left" Width="200px" />
                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                </asp:TemplateField>
                
            </Columns>
            <SelectedRowStyle CssClass="ms-selectednav" Font-Bold="True" />
        </SharePoint:SPGridView>

        </td>
    </tr>
     <tr><td colspan="2" align="right" class="text-field-bg"><img src="/_layouts/images/CALADD.GIF" style="text-align:center"/>&nbsp;<a style="cursor:hand"  onclick="OpenDialog('MaintainEmployee.aspx')" >Add New Employee</a><br /></td></tr>
     
     <tr>
        <td colspan="2" align="center" class="text-field-bg">
            <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
        </td>
    </tr>
</table>
</div>
