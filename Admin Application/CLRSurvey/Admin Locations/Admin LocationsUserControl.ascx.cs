using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CLRSurvey.Admin_Locations
{
    public partial class Admin_LocationsUserControl : UserControl
    {
        SqlCommand selectCommand = null;
        SqlConnection cn = new SqlConnection();
        DataTable dtEntityList = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter();
        
        private ObjectDataSource gridDS;

        protected void Page_Load(object sender, EventArgs e)
        {
            

           if ((Session["CLRLocationFilterExpression"] != null || Session["CLRLocationSortExpression"] != null || Session["CLRLocationPageIndex"] != null) && IsPostBack)
            {
                if (!IsPostBack)
                {
                    ViewState["FilterExpression"] = Session["CLRLocationFilterExpression"];
                }
            }
            else
            {
                if (!IsPostBack)
                {
                    Session["CLRLocationFilterExpression"] = null;
                    Session["CLRLocationSortExpression"] = null;
                    Session["CLRLocationSortDirection"] = null;
                    Session["CLRLocationPageIndex"] = null;
                    getEntityList();

                    string EntityClientID = ddlEntityID.ClientID;
                    //Passing parameter to javascript through code behind.
                    System.IO.StringWriter sw = new System.IO.StringWriter();
                    sw.Write("openAddLocationWindowNewMode('{0}')", EntityClientID);
                    anchorAddnew.Attributes.Add("onclick", sw.ToString());
                    anchorAddNew1.Attributes.Add("onclick", sw.ToString());
                }
                else
                {
                    ViewState["FilterExpression"] = Session["CLRLocationFilterExpression"];
                    
                }
            }

           
           PopulateRequests();
            
        }

        protected void SPgrdLocation_PreRender(object sender, EventArgs e)
        {
            if (Session["CLRLocationPageIndex"] != null)
            {
                SPgrdLocation.PageIndex = Convert.ToInt32(Session["CLRLocationPageIndex"]);
            }
        }

        protected void getEntityList()
        {
            try
            {
                cn.ConnectionString = ConfigurationManager.AppSettings["clr_ConStr"];
                selectCommand = cn.CreateCommand();
                selectCommand.CommandType = CommandType.StoredProcedure;
                selectCommand.CommandText = "SP_GetAllEntity";
                da.SelectCommand = selectCommand;
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }
                da.Fill(dtEntityList);
                if (dtEntityList != null)
                {
                    ddlEntityID.DataSource = dtEntityList;
                    ddlEntityID.DataTextField = "EntityID";
                    ddlEntityID.DataValueField = "EntityID";
                    ddlEntityID.DataBind();
                    ddlEntityID.Items.Insert(0, new ListItem("--Select--"));
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
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
            SPgrdLocation.PagerTemplate = null;
            gridDS.UpdateMethod = "MyEmptyUpdateMethod";
            gridDS.DeleteMethod = "MyEmptyDeleteMethod";
           // this.Controls.Add(gridDS);

            //Set the datasource of the grid to the instance of ObjectDataSource
            SPgrdLocation.DataSourceID = gridDS.ID;

            if (Session["CLRLocationFilterExpression"] != null)
            {
                gridDS.FilterExpression = (string)ViewState["FilterExpression"];
            }

            //paging 
            SPgrdLocation.AllowPaging = true;
            SPgrdLocation.PageSize = 10; //15
            SPgrdLocation.PageIndexChanging += new GridViewPageEventHandler(OnViewPageChanging);
            SPgrdLocation.PagerTemplate = null;
            
            this.Controls.Add(new LiteralControl("<style type='text/css'> "
            + ".ms-menuimagecell{text-align:center; background-image:url(../../../_layouts/images/CLRSurvey/table-header-bg.png);  background-repeat:repeat-x;   border:0px;margin-top:3px;} "
            + ".ms-selectedtitle{text-align:center; background-image:url(../../../_layouts/images/CLRSurvey/table-header-bg.png);  background-repeat:repeat-x;   height:35px;}"
            + ".ms-vb{border-bottom: 0px solid lightgray; font-size:11px !important; padding-top:10px !important; background-image:url(../../../_layouts/images/CLRSurvey/table-header-bg.png); background-repeat:repeat-x; background-position-x:0px; height:25px;  }"
            + ".ms-viewheadertr th {text-align:center; background-image:url(../../../_layouts/images/CLRSurvey/table-header-bg.png);background-repeat:repeat-x;  margin-top:3px;  padding-top:0px !important; border:0px !important;  }"
            + ".ms-viewheadertr .ms-vh2-gridview{height:38px;background-image:none;background-repeat:no-repeat;}"
            + ".ms-viewheadertr{ border-right:1px solid #004c80 !important; margin-right:0px !important; border-left:1px solid #004c80 !important; margin-left:0px !important; }"
            + ".ms-vb2 { border-bottom:1px lightgray solid  !important; vertical-align: middle !important;}</style>"));
             

            this.Controls.Add(gridDS);
            gridDS.DataBind();
           
        }

        protected void OnViewPageChanging(object sender, GridViewPageEventArgs e)
        {
            Session["CLRLocationPageIndex"] = e.NewPageIndex;
            SPgrdLocation.PageIndex = e.NewPageIndex;
            Session["CLRLocationFilterExpression"] = null;
            gridDS.DataBind();
        }

        public DataTable SelectData()
        {
            DataTable dtEmployeeList = new DataTable();
            SqlCommand selectCommand = null;
            SqlConnection cn = new SqlConnection();

            cn.ConnectionString = ConfigurationManager.AppSettings["clr_ConStr"];
            try
            {
                // dataAdapter used to fill dataset dsLocationList
                SqlDataAdapter da = new SqlDataAdapter();
                selectCommand = cn.CreateCommand();

                if (ddlEntityID.SelectedIndex > 0)
                {
                    selectCommand.Parameters.Add(new SqlParameter("@EntityID", ddlEntityID.SelectedValue));
                }
                selectCommand.CommandType = CommandType.StoredProcedure; //command through stored proc
                selectCommand.CommandText = "SP_GetAllLocation"; // stored proc name

                da.SelectCommand = selectCommand;
                da.Fill(dtEmployeeList);        // fills dataset dsLocationList through datatAdapter

                if (ViewState["FilterExpression"] != null)
                {
                    DataTable dt = dtEmployeeList;
                    string strlbl = ViewState["FilterExpression"].ToString();
                    DataRow[] foundRows;
                    foundRows = dt.Select(strlbl);
                    if (foundRows.Length == 0)
                    {
                        ViewState["FilterExpression"] = null;
                        Session["CLRLocationFilterExpression"] = null;
                        gridDS.FilterExpression = "";
                    }
                }

                foreach (DataRow dataRow in dtEmployeeList.Rows)
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
                               
               return dtEmployeeList;
            }
            catch (Exception ex)
            {
                string strErrorMsg = ex.Message;
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                lblMsg.Text = strErrorMsg;
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

        protected void SPgrdLocation_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (gridDS.FilterExpression == "") ViewState["FilterExpression"] = null;

            if (ViewState["FilterExpression"] != null)
            {
                gridDS.FilterExpression = (string)ViewState["FilterExpression"];
            }

            Session["CLRLocationFilterExpression"] = ViewState["FilterExpression"];
            Session["CLRLocationSortExpression"] = e.SortExpression;
            Session["CLRLocationSortDirection"] = e.SortDirection;
        }

        private void gridDS_Filtering(object sender, ObjectDataSourceFilteringEventArgs e)
        {
            ViewState["FilterExpression"] = ((ObjectDataSourceView)sender).FilterExpression;
            Session["CLRLocationFilterExpression"] = ViewState["FilterExpression"];
        }

        protected void SPgrdLocation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
           if (e.Row.RowType == DataControlRowType.Header)
            {
               foreach (TableCell cell in e.Row.Cells)
                {
                    if (cell.Controls.Count > 0)
                    {
                        LinkButton lbSort = (LinkButton)cell.Controls[0];
                        if (lbSort.CommandArgument == SPgrdLocation.SortExpression)
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "MyKey", "<script language='javascript'  type='text/javascript'>changeArrowImg()</script>");
                        }
                    }
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblLocatnID = (Label)e.Row.FindControl("lblLocatnID");
                Label lblLocatnName = (Label)e.Row.FindControl("lblLocatnName");
                Label lblAddDepartment = (Label)e.Row.FindControl("lblAddDepartment");
                Label lblManageDepartment = (Label)e.Row.FindControl("lblManageDepartment");                

                lblAddDepartment.Text = "<a href='#' style=\"cursor:hand\"  onclick=\"openAddDepartment('" + lblLocatnName.Text + "','" + lblLocatnID.Text + "')\" >" + lblAddDepartment.Text + "</a>";

                lblLocatnName.Text = "<a href='#' style=\"cursor:hand\"  onclick=\"openAddLocationWindowEditMode('" + lblLocatnID.Text + "')\" >" + lblLocatnName.Text + "</a>";
            }
           
        }

        protected void ddlEntiyID_SelectedIndexChanged(object sender, EventArgs e)
        {
           PopulateRequests();
        }

    }
}
