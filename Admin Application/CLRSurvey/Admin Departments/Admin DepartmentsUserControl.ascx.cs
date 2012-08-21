using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections;
using System.ComponentModel;
using System.Web.UI.HtmlControls;
using Microsoft.SharePoint.Utilities;


namespace CLRSurvey.Admin_Departments
{
    public partial class Admin_DepartmentsUserControl : UserControl
    {
        private ObjectDataSource gridDS;
        SqlCommand selectCommand = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session["CLRAdminDeptFilterExpression"] != null || Session["CLRAdminDeptSortExpression"] != null || Session["CLRAdminDeptPageIndex"] != null) && IsPostBack)
            {
                if (!IsPostBack)
                {
                    ViewState["FilterExpression"] = Session["CLRAdminDeptFilterExpression"];
                }
            }
            else
            {
                if (!IsPostBack)
                {
                    Session["CLRAdminDeptFilterExpression"] = null;
                    Session["CLRAdminDeptSortExpression"] = null;
                    Session["CLRAdminDeptSortDirection"] = null;
                    Session["CLRAdminDeptPageIndex"] = null;
                    fnBindEntityDropDownList();

                    string ddlLocationClientID = ddlLocation.ClientID;
                    //Passing parameter to javascript through code behind.
                    System.IO.StringWriter sw = new System.IO.StringWriter();
                    sw.Write("OpenDialog('{0}')", ddlLocationClientID);
                    anchorAddnew.Attributes.Add("onclick", sw.ToString());
                    anchorAddnew1.Attributes.Add("onclick", sw.ToString());
                }
                else
                {
                    ViewState["FilterExpression"] = Session["CLRAdminDeptFilterExpression"];
                }
            }

            PopulateRequests();
            
        }

        protected void spGrdDeptList_PreRender(object sender, EventArgs e)
        {
            if (Session["CLRAdminDeptPageIndex"] != null)
            {
                spGrdDeptList.PageIndex = Convert.ToInt32(Session["CLRAdminDeptPageIndex"]);
            }
        }

        protected void PopulateRequests()
        {
            //Use a ObjectDataSource to bind to the data table
            gridDS = new ObjectDataSource();
            gridDS = (ObjectDataSource)this.FindControl("gridDS");
            if (gridDS == null)
            {
                gridDS = new ObjectDataSource();
                gridDS.ID = "gridDS";
            }

            //We select the method the data is pulled from
            gridDS.SelectMethod = "SelectData";
            gridDS.TypeName = this.GetType().AssemblyQualifiedName;
            gridDS.ObjectCreating += new ObjectDataSourceObjectEventHandler(gridDS_ObjectCreating);
            gridDS.Filtering += new ObjectDataSourceFilteringEventHandler(gridDS_Filtering);
            spGrdDeptList.PagerTemplate = null;
            gridDS.UpdateMethod = "MyEmptyUpdateMethod";
            gridDS.DeleteMethod = "MyEmptyDeleteMethod";


            this.Controls.Add(new LiteralControl("<style type='text/css'> "
            + ".ms-menuimagecell{text-align:center; background-image:url(../../../_layouts/images/CLRSurvey/table-header-bg.png);  background-repeat:repeat-x;   border:0px;margin-top:3px;} "
            + ".ms-selectedtitle{text-align:center; background-image:url(../../../_layouts/images/CLRSurvey/table-header-bg.png);  background-repeat:repeat-x;   height:35px;}"
            + ".ms-vb{border-bottom: 0px solid lightgray; font-size:11px !important; padding-top:10px !important; background-image:url(../../../_layouts/images/CLRSurvey/table-header-bg.png); background-repeat:repeat-x; background-position-x:0px; height:25px;  }"
            + ".ms-viewheadertr th {text-align:center; background-image:url(../../../_layouts/images/CLRSurvey/table-header-bg.png);background-repeat:repeat-x;  margin-top:3px;  padding-top:0px !important; border:0px !important;  }"
            + ".ms-viewheadertr .ms-vh2-gridview{height:38px;background-image:none;background-repeat:no-repeat;}"
            + ".ms-viewheadertr{ border-right:1px solid #004c80 !important; margin-right:0px !important; border-left:1px solid #004c80 !important; margin-left:0px !important; }"
            + ".ms-vb2 { border-bottom:1px lightgray solid  !important; vertical-align: middle !important;}</style>"));

            this.Controls.Add(gridDS);
            //Set the datasource of the grid to the instance of ObjectDataSource
            spGrdDeptList.DataSourceID = gridDS.ID;

            if (Session["CLRAdminDeptFilterExpression"] != null)
            {
                gridDS.FilterExpression = (string)ViewState["FilterExpression"];

            }

            //paging 
            spGrdDeptList.AllowPaging = true;
            spGrdDeptList.PageSize = 10;
            spGrdDeptList.PageIndexChanging += new GridViewPageEventHandler(OnViewPageChanging);
            spGrdDeptList.PagerTemplate = null;

            gridDS.DataBind();

        }

        protected void OnViewPageChanging(object sender, GridViewPageEventArgs e)
        {
            Session["CLRAdminDeptPageIndex"] = e.NewPageIndex;
            spGrdDeptList.PageIndex = e.NewPageIndex;
            Session["CLRAdminDeptFilterExpression"] = null;
            gridDS.DataBind();

        }

        public DataTable SelectData()
        {
            selectCommand = null;
            DataTable data = new DataTable();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.AppSettings["clr_ConStr"];
            try
            {
                // dataAdapter used to fill dataset dsQuestionList
                SqlDataAdapter da = new SqlDataAdapter();
                selectCommand = cn.CreateCommand();
                selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                selectCommand.CommandText = "sp_Dept_GetAll"; // stored proc name
                if (ddlEntity.SelectedIndex != 0)
                {
                    selectCommand.Parameters.Add(new SqlParameter("@EntityID", ddlEntity.SelectedValue));
                }
                if (ddlLocation.SelectedIndex != 0)
                {
                    selectCommand.Parameters.Add(new SqlParameter("@LocationID", ddlLocation.SelectedValue));
                }
                da.SelectCommand = selectCommand;
                da.Fill(data);        // fills dataset dsQuestionList thru datatAdapter

                foreach (DataRow dataRow in data.Rows)
                {
                    if (dataRow["ActvInactvInd"].ToString() == "y" || dataRow["ActvInactvInd"].ToString() == "Y")
                    {
                        dataRow["ActvInactvInd"] = "Yes";
                    }
                    else
                    {
                        dataRow["ActvInactvInd"] = "No";
                    }
                }

                if (ViewState["FilterExpression"] != null)
                {
                    DataTable dt = data;
                    string strlbl = ViewState["FilterExpression"].ToString();
                    DataRow[] foundRows;
                    foundRows = dt.Select(strlbl);
                    if (foundRows.Length == 0)
                    {
                        ViewState["FilterExpression"] = null;
                        Session["CLRAdminDeptFilterExpression"] = null;
                        gridDS.FilterExpression = "";
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                lblMsg.Text = ex.Message;
                return null;
            }
        }

        protected sealed override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);
            if (Context.Request.Form["__EVENTARGUMENT"] != null &&
                Context.Request.Form["__EVENTARGUMENT"].EndsWith("__ClearFilter__"))
            {
                // Clear FilterExpression
                ViewState.Remove("FilterExpression");
            }
        }

        protected void gridDS_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = this;
        }

        protected void spGrdDeptList_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (gridDS.FilterExpression == "") ViewState["FilterExpression"] = null;

            if (ViewState["FilterExpression"] != null)
            {
                gridDS.FilterExpression = (string)ViewState["FilterExpression"];
            }

            Session["CLRAdminDeptFilterExpression"] = ViewState["FilterExpression"];
            Session["CLRAdminDeptSortExpression"] = e.SortExpression;
            Session["CLRAdminDeptSortDirection"] = e.SortDirection;
        }

        private void gridDS_Filtering(object sender, ObjectDataSourceFilteringEventArgs e)
        {
            ViewState["FilterExpression"] = ((ObjectDataSourceView)sender).FilterExpression;
            Session["CLRAdminDeptFilterExpression"] = ViewState["FilterExpression"];
        }

        protected void spGrdDeptList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                foreach (TableCell cell in e.Row.Cells)
                {
                    LinkButton lbSort = (LinkButton)cell.Controls[0];
                    if (lbSort.CommandArgument == spGrdDeptList.SortExpression)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "MyKey", "<script language='javascript'  type='text/javascript'>changeArrowImg()</script>");
                    }
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblDeptName = (Label)e.Row.FindControl("lblDeptName");
                Label lblDeptId = (Label)e.Row.FindControl("lblDeptID");
                Label lblAddRoom = (Label)e.Row.FindControl("lblAddRoom");
                Label lblAddEmployee = (Label)e.Row.FindControl("lblAddEmployee");

                lblAddRoom.Text = "<a href='#' style=\"cursor:hand\"  onclick=\"openAddRoomWindowNewMode('" + lblDeptName.Text + "')\" >" + lblAddRoom.Text + "</a>";
                lblAddEmployee.Text = "<a href='#' style=\"cursor:hand\"  onclick=\"openAddEmployeeWindowNewMode('" + lblDeptName.Text + "')\" >" + lblAddEmployee.Text + "</a>";
                lblDeptName.Text = "<a href='#' style=\"cursor:hand\"  onclick=\"OpenDepartmentWindowEditMode('" + "MaintainDept.aspx?Did=" + lblDeptId.Text + "')\" >" + lblDeptName.Text + "</a>";
            }
        }

        protected void ddlEntity_SelectedIndexChanged(object sender, EventArgs e)
        {
            fnBindLocationDropDownList();
            PopulateRequests();
        }

        protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateRequests();
        }

        private void fnBindEntityDropDownList()
        {
            selectCommand = null;
            DataTable dtEntityList = new DataTable();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.AppSettings["clr_ConStr"];
            try
            {

                selectCommand = cn.CreateCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                selectCommand.CommandType = CommandType.StoredProcedure;
                selectCommand.CommandText = "SP_GetAllEntity";
                da.SelectCommand = selectCommand;
                da.Fill(dtEntityList);
                if (dtEntityList != null)
                {
                    ddlEntity.DataSource = dtEntityList;
                    ddlEntity.DataTextField = "EntityID";
                    ddlEntity.DataValueField = "EntityID";
                    ddlEntity.DataBind();
                    ddlEntity.Items.Insert(0, new ListItem("--Select--"));
                    
                    ddlLocation.Items.Insert(0, new ListItem("--Select--"));
                }
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                lblMsg.Text = ex.Message;
            }
        }

        private void fnBindLocationDropDownList()
        {
            selectCommand = null;
            DataTable dtLocationList = new DataTable();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.AppSettings["clr_ConStr"];
            try
            {
                selectCommand = cn.CreateCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                selectCommand.CommandType = CommandType.StoredProcedure;    //command thru stored proc
                selectCommand.CommandText = "SP_GetLocation_LoadByEntityID";      // stored proc name   
                selectCommand.Parameters.Add(new SqlParameter("@EntityID", ddlEntity.SelectedValue));
                da.SelectCommand = selectCommand;
                da.Fill(dtLocationList);
                if (dtLocationList != null)
                {
                    ddlLocation.Items.Clear();
                    ddlLocation.DataSource = dtLocationList;
                    ddlLocation.DataTextField = "LocatnName";
                    ddlLocation.DataValueField = "LocatnID";
                    ddlLocation.DataBind();
                    ddlLocation.Items.Insert(0, new ListItem("--Select--"));
                }
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                lblMsg.Text = ex.Message;
            }
        }

    }
}
