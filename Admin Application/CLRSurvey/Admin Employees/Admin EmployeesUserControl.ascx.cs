using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CLRSurvey.Admin_Employees
{
    public partial class Admin_EmployeesUserControl : UserControl
    {
        private ObjectDataSource gridDS;

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session["CLRAdminEmployeeFilterExpression"] != null || Session["CLRAdminEmployeeSortExpression"] != null || Session["CLRAdminEmployeePageIndex"] != null) && IsPostBack)
            {
                if (!IsPostBack)
                {
                    ViewState["FilterExpression"] = Session["CLRAdminEmployeeFilterExpression"];
                }
            }
            else
            {
                if (!IsPostBack)
                {
                    Session["CLRAdminEmployeeFilterExpression"] = null;
                    Session["CLRAdminEmployeeSortExpression"] = null;
                    Session["CLRAdminEmployeeSortDirection"] = null;
                    Session["CLRAdminEmployeePageIndex"] = null;
                }
                else
                {
                    ViewState["FilterExpression"] = Session["CLRAdminEmployeeFilterExpression"];
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

        protected void spGrdEmpList_PreRender(object sender, EventArgs e)
        {
            if (Session["CLRAdminEmployeePageIndex"] != null)
            {
                spGrdEmpList.PageIndex = Convert.ToInt32(Session["CLRAdminEmployeePageIndex"]);
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
            spGrdEmpList.PagerTemplate = null;
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
            spGrdEmpList.DataSourceID = gridDS.ID;

            if (Session["CLRAdminEmployeeFilterExpression"] != null)
            {
                gridDS.FilterExpression = (string)ViewState["FilterExpression"];

            }
            //paging 
            spGrdEmpList.AllowPaging = true;
            spGrdEmpList.PageSize = 10;
            spGrdEmpList.PageIndexChanging += new GridViewPageEventHandler(OnViewPageChanging);
            spGrdEmpList.PagerTemplate = null;

            gridDS.DataBind();
        }

        protected void OnViewPageChanging(object sender, GridViewPageEventArgs e)
        {
            //spGrdRoomList.PageIndex = e.NewPageIndex;
            //gridDS.DataBind();
            Session["CLRAdminEmployeePageIndex"] = e.NewPageIndex;
            spGrdEmpList.PageIndex = e.NewPageIndex;
            Session["CLRAdminEmployeeFilterExpression"] = null;
            gridDS.DataBind();
        }

        public DataSet SelectData()
        {

            SqlCommand selectCommand = null;
            DataSet data = new DataSet();


            SqlConnection cn = new SqlConnection();

            cn.ConnectionString = ConfigurationManager.AppSettings["clr_ConStr"];
            try
            {
                // dataAdapter used to fill dataset dsQuestionList
                SqlDataAdapter da = new SqlDataAdapter();
                selectCommand = cn.CreateCommand();
                selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                selectCommand.CommandText = "sp_GetAllEmployees"; // stored proc name

                da.SelectCommand = selectCommand;
                da.Fill(data);        // fills dataset dsQuestionList thru datatAdapter

                if (ViewState["FilterExpression"] != null)
                {
                    DataTable dt = data.Tables[0];
                    string strlbl = ViewState["FilterExpression"].ToString();
                    DataRow[] foundRows;
                    foundRows = dt.Select(strlbl);
                    if (foundRows.Length == 0)
                    {
                        ViewState["FilterExpression"] = null;
                        Session["CLRAdminEmployeeFilterExpression"] = null;
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

        protected void spGrdEmpList_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (gridDS.FilterExpression == "") ViewState["FilterExpression"] = null;

            if (ViewState["FilterExpression"] != null)
            {
                gridDS.FilterExpression = (string)ViewState["FilterExpression"];
            }

            Session["CLRAdminEmployeeFilterExpression"] = ViewState["FilterExpression"];
            Session["CLRAdminEmployeeSortExpression"] = e.SortExpression;
            Session["CLRAdminEmployeeSortDirection"] = e.SortDirection;
        }

        private void gridDS_Filtering(object sender, ObjectDataSourceFilteringEventArgs e)
        {
            ViewState["FilterExpression"] = ((ObjectDataSourceView)sender).FilterExpression;
            Session["CLRAdminEmployeeFilterExpression"] = ViewState["FilterExpression"];
        }

        protected void spGrdEmpList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                foreach (TableCell cell in e.Row.Cells)
                {
                    LinkButton lbSort = (LinkButton)cell.Controls[0];
                    if (lbSort.CommandArgument == spGrdEmpList.SortExpression)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "MyKey", "<script language='javascript'  type='text/javascript'>changeArrowImg()</script>");
                    }
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label EmployeeID = (Label)e.Row.FindControl("lblEmployeeID");
                Label EmployeeName = (Label)e.Row.FindControl("lblEmployeeName");
                EmployeeName.Text = "<a href=\"#\" style=\"cursor:hand\"  onclick=\"OpenDialog('" + "MaintainEmployee.aspx?Empid=" + System.Uri.EscapeDataString(EmployeeID.Text) + "')\" >" + EmployeeName.Text + "</a>";
            }
        }
    }
}
