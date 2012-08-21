<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuestionListUserControl.ascx.cs" Inherits="CLRSurvey.QuestionList.QuestionListUserControl" %>
<link href="../../../_layouts/CLRSurvey/main.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" language="javascript">
    function openAddQuestionWindowNewMode() {
        var options;
        options = { url: "./AddQuestion.aspx?", width: 750, title: "Add New Question" };
        options.dialogReturnValueCallback = Function.createDelegate(null, closeDialogCallback);
        SP.UI.ModalDialog.showModalDialog(options);
    }

    function openAddQuestionWindowEditMode(QuestnID) {
        var options;
        options = { url: "./AddQuestion.aspx?qid=" + QuestnID, width: 750, title: "Update Question" };
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
</style>
<div>
    <table align="center" width="80%" border="0" cellpadding="0" cellspacing="0" class="table-border">
        <tr>
            <td colspan="2" class="form-header">
                Question Bank
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right" class="text-field-bg">
                <img src="/_layouts/images/CALADD.GIF" alt="image" style="text-align: center" />&nbsp;<a
                    href="#" onclick="openAddQuestionWindowNewMode();">Add New Question</a>
            </td>
        </tr>
        <tr>
            <td class="text-field-bg">
                <SharePoint:SPGridView ID="SPgrdQuestion" runat="server" AllowFiltering="true" AllowSorting="true"
                    UseAccessibleHeader="true" FilteringIsCaseSensitive="false" CssClass="ms-listviewtable"
                    HeaderStyle-CssClass="ms-viewheadertr" FilterDataFields="QuestnID, QuestnText, QuestnShortText, CommntText, RespnsTypeCode, ActvInactvInd"
                    FilteredDataSourcePropertyName="FilterExpression" FilteredDataSourcePropertyFormat="{1} = '{0}'"
                    EnableTheming="true" AutoGenerateColumns="false" OnSorting="SPgrdQuestion_Sorting"
                    RowStyle-CssClass="GridRowStyle" AlternatingRowStyle-CssClass="GridAlternetRowStyle"
                    OnRowDataBound="SPgrdQuestion_RowDataBound" AllowPaging="True" OnPageIndexChanging="OnViewPageChanging"
                    EmptyDataText="There are no items to show in Question Bank." OnPreRender="SPgrdQuestion_PreRender">
                    <AlternatingRowStyle CssClass="GridAlternetRowStyle" />
                    <HeaderStyle CssClass="ms-viewheadertr" />
                    <Columns>
                        <asp:TemplateField HeaderText="ID" Visible="false" SortExpression="QuestnID">
                            <ItemTemplate>
                                <asp:Label ID="lblQuestnID" runat="server" Text='<%# Bind("QuestnID") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Font-Bold="True" ForeColor="White" HorizontalAlign="Center" Width="30px" />
                            <ItemStyle Width="30px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Question" SortExpression="QuestnText">
                            <ItemTemplate>
                                <asp:Label ID="lblQuestnText" runat="server" Text='<%# Bind("QuestnText") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Font-Bold="True" ForeColor="White" HorizontalAlign="Center" Width="200px" />
                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Abbreviation" SortExpression="QuestnShortText">
                            <ItemTemplate>
                                <asp:Label ID="lblQuestnShortText" runat="server" Text='<%# Bind("QuestnShortText") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Font-Bold="True" ForeColor="White" HorizontalAlign="Center" Width="200px" />
                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Comment" SortExpression="CommntText">
                            <ItemTemplate>
                                <asp:Label ID="lblComment" runat="server" Text='<%# Bind("CommntText") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Font-Bold="True" ForeColor="White" HorizontalAlign="Center" Width="150px" />
                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Response Type Code" SortExpression="RespnsTypeCode">
                            <ItemTemplate>
                                <asp:Label ID="lblResponseTypeCode" runat="server" Text='<%# Bind("RespnsTypeCode") %>'></asp:Label>
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
                    </Columns>
                    <SelectedRowStyle CssClass="ms-selectednav" Font-Bold="True" />
                    <RowStyle CssClass="GridRowStyle" />
                </SharePoint:SPGridView>
            </td>
        </tr>        
        <tr>
            <td colspan="2" align="right" class="text-field-bg">
                <img src="/_layouts/images/CALADD.GIF" alt="image" style="text-align: center" />&nbsp;<a
                    href="#" onclick="openAddQuestionWindowNewMode();">Add New Question</a>
            </td>
        </tr>        
        <tr class="text-field-bg">
            <td colspan="2" align="center">
                <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
            </td>
        </tr>
    </table>
</div>