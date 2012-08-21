using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.SharePoint;

namespace CLRSurvey.Admin_Rooms
{
    public partial class Admin_RoomsUserControl : UserControl
    {
        private ObjectDataSource gridDS;
        SqlCommand selectCommand = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session["CLRAdminRoomFilterExpression"] != null || Session["CLRAdminRoomSortExpression"] != null || Session["CLRAdminRoomPageIndex"] != null) && IsPostBack)
            {
                if (!IsPostBack)
                {
                    ViewState["FilterExpression"] = Session["CLRAdminRoomFilterExpression"];
                }
            }
            else
            {
                if (!IsPostBack)
                {
                    Session["CLRAdminRoomFilterExpression"] = null;
                    Session["CLRAdminRoomSortExpression"] = null;
                    Session["CLRAdminRoomSortDirection"] = null;
                    Session["CLRAdminRoomPageIndex"] = null;
                    fnBindEntityDropDownList();
                    string ddlDepartmentClientID = ddlDepartment.ClientID;
                    //Passing parameter to javascript through code behind.
                    System.IO.StringWriter sw = new System.IO.StringWriter();
                    sw.Write("OpenDialogAddNew('{0}')", ddlDepartmentClientID);
                    anchorAddnew.Attributes.Add("onclick", sw.ToString());
                    anchorAddnew1.Attributes.Add("onclick", sw.ToString());
                }
                else
                {
                    ViewState["FilterExpression"] = Session["CLRAdminRoomFilterExpression"];
                }
            }
            PopulateRequests();
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

        protected void spGrdRoomList_PreRender(object sender, EventArgs e)
        {
            if (Session["CLRAdminRoomPageIndex"] != null)
            {
                spGrdRoomList.PageIndex = Convert.ToInt32(Session["CLRAdminRoomPageIndex"]);
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
            spGrdRoomList.PagerTemplate = null;
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
            spGrdRoomList.DataSourceID = gridDS.ID;

            if (Session["CLRAdminRoomFilterExpression"] != null)
            {
                gridDS.FilterExpression = (string)ViewState["FilterExpression"];

            }

            //paging 
            spGrdRoomList.AllowPaging = true;
            spGrdRoomList.PageSize = 10;
            spGrdRoomList.PageIndexChanging += new GridViewPageEventHandler(OnViewPageChanging);

            spGrdRoomList.PagerTemplate = null;


            gridDS.DataBind();
        }

        protected void OnViewPageChanging(object sender, GridViewPageEventArgs e)
        {
            Session["CLRAdminRoomPageIndex"] = e.NewPageIndex;
            spGrdRoomList.PageIndex = e.NewPageIndex;
            Session["CLRAdminRoomFilterExpression"] = null;
            gridDS.DataBind();
        }

        public DataTable SelectData()
        {
            SqlCommand selectCommand = null;
            DataTable data = new DataTable();

            SqlConnection cn = new SqlConnection();

            cn.ConnectionString = ConfigurationManager.AppSettings["clr_ConStr"];
            try
            {
                // dataAdapter used to fill dataset dsQuestionList
                SqlDataAdapter da = new SqlDataAdapter();
                selectCommand = cn.CreateCommand();
                selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                selectCommand.CommandText = "sp_Room_GetIdDescr"; // stored proc name
                if (ddlEntity.SelectedIndex != 0)
                {
                    selectCommand.Parameters.Add(new SqlParameter("@EntityID", ddlEntity.SelectedValue));
                }
                if (ddlLocation.SelectedIndex != 0)
                {
                    selectCommand.Parameters.Add(new SqlParameter("@LocationID", ddlLocation.SelectedValue));
                }
                if (ddlDepartment.SelectedIndex != 0)
                {
                    selectCommand.Parameters.Add(new SqlParameter("@DepartmentID", ddlDepartment.SelectedValue));
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

                    if (dataRow["RoomTypeCode"].ToString() == "10")
                    {
                        dataRow["RoomTypeCode"] = "Private";
                    }

                    else if (dataRow["RoomTypeCode"].ToString() == "20")
                    {
                        dataRow["RoomTypeCode"] = "Semi-Private";
                    }
                    else
                    {
                        dataRow["RoomTypeCode"] = "Other";
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
                        Session["CLRAdminRoomFilterExpression"] = null;
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

        protected void gridDS_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = this;
        }

        protected void spGrdRoomList_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (gridDS.FilterExpression == "") ViewState["FilterExpression"] = null;

            if (ViewState["FilterExpression"] != null)
            {
                gridDS.FilterExpression = (string)ViewState["FilterExpression"];
            }

            Session["CLRAdminRoomFilterExpression"] = ViewState["FilterExpression"];
            Session["CLRAdminRoomSortExpression"] = e.SortExpression;
            Session["CLRAdminRoomSortDirection"] = e.SortDirection;
        }

        private void gridDS_Filtering(object sender, ObjectDataSourceFilteringEventArgs e)
        {
            ViewState["FilterExpression"] = ((ObjectDataSourceView)sender).FilterExpression;
            Session["CLRAdminRoomFilterExpression"] = ViewState["FilterExpression"];
        }

        protected void spGrdRoomList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                foreach (TableCell cell in e.Row.Cells)
                {
                    LinkButton lbSort = (LinkButton)cell.Controls[0];
                    if (lbSort.CommandArgument == spGrdRoomList.SortExpression)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "MyKey", "<script language='javascript'  type='text/javascript'>changeArrowImg()</script>");
                    }
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblRoomDescr = (Label)e.Row.Cells[1].Controls[1];
                Label lblRoomID = (Label)e.Row.Cells[0].Controls[1];

                lblRoomDescr.Text = "<a href=\"#\" style=\"cursor:hand\"  onclick=\"OpenDialog('" + "MaintainRoom.aspx?Rid=" + lblRoomID.Text + "')\" >" + lblRoomDescr.Text + "</a>";
            }
        }

        protected void ddlEntity_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["CLRAdminRoomFilterExpression"] = null;
            Session["CLRAdminRoomSortExpression"] = null;
            Session["CLRAdminRoomSortDirection"] = null;
            Session["CLRAdminRoomPageIndex"] = null;
            spGrdRoomList.PageIndex = 0;
            gridDS.FilterExpression = "";

            fnBindLocationDropDownList();
            ddlDepartment.Items.Clear();
            ddlDepartment.Items.Insert(0, new ListItem("--Select--"));
            PopulateRequests();
        }

        protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["FilterExpression"] = null;
            Session["CLRAdminRoomFilterExpression"] = null;
            gridDS.FilterExpression = "";

            fnBindDepartmentDropDownList();
            PopulateRequests();
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["FilterExpression"] = null;
            Session["CLRAdminRoomFilterExpression"] = null;
            gridDS.FilterExpression = "";

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
                    ddlDepartment.Items.Clear();
                    ddlDepartment.Items.Insert(0, new ListItem("--Select--"));
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

        private void fnBindDepartmentDropDownList()
        {
            selectCommand = null;
            DataTable dtDepartmentList = new DataTable();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.AppSettings["clr_ConStr"];
            try
            {
                selectCommand = cn.CreateCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                selectCommand.CommandType = CommandType.StoredProcedure;    //command thru stored proc
                selectCommand.CommandText = "SP_GetDept_LoadByLocationID";      // stored proc name   
                selectCommand.Parameters.Add(new SqlParameter("@LocationID", ddlLocation.SelectedValue));
                da.SelectCommand = selectCommand;
                da.Fill(dtDepartmentList);
                if (dtDepartmentList != null)
                {
                    ddlDepartment.Items.Clear();
                    ddlDepartment.DataSource = dtDepartmentList;
                    ddlDepartment.DataTextField = "Name";
                    ddlDepartment.DataValueField = "DeptID";
                    ddlDepartment.DataBind();
                    ddlDepartment.Items.Insert(0, new ListItem("--Select--"));
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
