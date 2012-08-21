using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.SharePoint;

namespace CLRSurvey.QuestionList
{
    public partial class QuestionListUserControl : UserControl
    {
        private ObjectDataSource gridDS;

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session["CLRQuestionFilterExpression"] != null || Session["CLRQuestionSortExpression"] != null || Session["CLRQuestionPageIndex"] != null) && IsPostBack)
            {
                if (!IsPostBack)
                {
                    ViewState["FilterExpression"] = Session["CLRQuestionFilterExpression"];
                }
            }
            else
            {
                if (!IsPostBack)
                {
                    Session["CLRQuestionFilterExpression"] = null;
                    Session["CLRQuestionSortExpression"] = null;
                    Session["CLRQuestionSortDirection"] = null;
                    Session["CLRQuestionPageIndex"] = null;
                }
                else
                {
                    ViewState["FilterExpression"] = Session["CLRQuestionFilterExpression"];
                }
            }
            PopulateRequests();
        }

        protected void SPgrdQuestion_PreRender(object sender, EventArgs e)
        {
            if (Session["CLRQuestionPageIndex"] != null)
            {
                SPgrdQuestion.PageIndex = Convert.ToInt32(Session["CLRQuestionPageIndex"]);
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
            SPgrdQuestion.PagerTemplate = null;
            gridDS.UpdateMethod = "MyEmptyUpdateMethod";
            gridDS.DeleteMethod = "MyEmptyDeleteMethod";
            this.Controls.Add(gridDS);

            //Set the datasource of the grid to the instance of ObjectDataSource
            SPgrdQuestion.DataSourceID = gridDS.ID;

            if (Session["CLRQuestionFilterExpression"] != null)
            {
                gridDS.FilterExpression = (string)ViewState["FilterExpression"];
            }

            //paging 
            SPgrdQuestion.AllowPaging = true;
            SPgrdQuestion.PageSize = 15;
            SPgrdQuestion.PageIndexChanging += new GridViewPageEventHandler(OnViewPageChanging);
            SPgrdQuestion.PagerTemplate = null;

            gridDS.DataBind();

            this.Controls.Add(new LiteralControl("<style type='text/css'> "
            + ".ms-menuimagecell{text-align:center; background-image:url(../../../_layouts/images/CLRSurvey/table-header-bg.png);  background-repeat:repeat-x;   border:0px;margin-top:3px;} "
            + ".ms-selectedtitle{text-align:center; background-image:url(../../../_layouts/images/CLRSurvey/table-header-bg.png);  background-repeat:repeat-x;   height:35px;}"
            + ".ms-vb{border-bottom: 0px solid lightgray; font-size:11px !important; padding-top:10px !important; background-image:url(../../../_layouts/images/CLRSurvey/table-header-bg.png); background-repeat:repeat-x; background-position-x:0px; height:25px;  }"
            + ".ms-viewheadertr th {text-align:center; background-image:url(../../../_layouts/images/CLRSurvey/table-header-bg.png);background-repeat:repeat-x;  margin-top:3px;  padding-top:0px !important; border:0px !important;  }"
            + ".ms-viewheadertr .ms-vh2-gridview{height:38px;background-image:none;background-repeat:no-repeat;}"
            + ".ms-viewheadertr{ border-right:1px solid #004c80 !important; margin-right:0px !important; border-left:1px solid #004c80 !important; margin-left:0px !important; }"
            + ".ms-vb2 { border-bottom:1px lightgray solid  !important; vertical-align: middle !important;}</style>"));

            this.Controls.Add(gridDS);
        }

        protected void OnViewPageChanging(object sender, GridViewPageEventArgs e)
        {
            Session["CLRQuestionPageIndex"] = e.NewPageIndex;
            SPgrdQuestion.PageIndex = e.NewPageIndex;
            Session["CLRQuestionFilterExpression"] = null;
            gridDS.DataBind();
        }

        public DataTable SelectData()
        {
            DataTable dtQuestionList = new DataTable();
            SqlCommand selectCommand = null;

            SqlConnection cn = new SqlConnection();

            cn.ConnectionString = ConfigurationManager.AppSettings["clr_ConStr"]; // get appSettings for current appln configuretion

            try
            {
                // dataAdapter used to fill dataset dsQuestionList
                SqlDataAdapter da = new SqlDataAdapter();
                selectCommand = cn.CreateCommand();
                selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                selectCommand.CommandText = "SP_GetAllQuestion"; // stored proc name

                da.SelectCommand = selectCommand;
                da.Fill(dtQuestionList);        // fills dataset dsQuestionList thru datatAdapter

                foreach (DataRow dataRow in dtQuestionList.Rows)
                {
                    if (dataRow["ActvInactvInd"].ToString() == "y" || dataRow["ActvInactvInd"].ToString() == "Y")
                    {
                        dataRow["ActvInactvInd"] = "Yes";
                    }
                    else
                    {
                        dataRow["ActvInactvInd"] = "No";
                    }

                    if (dataRow["RespnsTypeCode"].ToString() == "10")
                    {
                        dataRow["RespnsTypeCode"] = "Acknowledge";
                    }
                    else
                    {
                        dataRow["RespnsTypeCode"] = "Yes\\No";
                    }
                }

                if (ViewState["FilterExpression"] != null)
                {
                    DataTable dt = dtQuestionList;
                    string strlbl = ViewState["FilterExpression"].ToString();
                    DataRow[] foundRows;
                    foundRows = dt.Select(strlbl);
                    if (foundRows.Length == 0)
                    {
                        ViewState["FilterExpression"] = null;
                        Session["CLRQuestionFilterExpression"] = null;
                        gridDS.FilterExpression = "";
                    }
                }

                return dtQuestionList;
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

        protected void SPgrdQuestion_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (gridDS.FilterExpression == "") ViewState["FilterExpression"] = null;

            if (ViewState["FilterExpression"] != null)
            {
                gridDS.FilterExpression = (string)ViewState["FilterExpression"];
            }

            Session["CLRQuestionFilterExpression"] = ViewState["FilterExpression"];
            Session["CLRQuestionSortExpression"] = e.SortExpression;
            Session["CLRQuestionSortDirection"] = e.SortDirection;
        }

        private void gridDS_Filtering(object sender, ObjectDataSourceFilteringEventArgs e)
        {
            ViewState["FilterExpression"] = ((ObjectDataSourceView)sender).FilterExpression;
            Session["CLRQuestionFilterExpression"] = ViewState["FilterExpression"];
        }

        protected void SPgrdQuestion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblQuestnID = (Label)e.Row.FindControl("lblQuestnID");
                Label lblQuestnText = (Label)e.Row.FindControl("lblQuestnText");
                lblQuestnText.Text = "<a href='#' style=\"cursor:hand\"  onclick=\"openAddQuestionWindowEditMode('" + lblQuestnID.Text + "')\" >" + lblQuestnText.Text + "</a>"; ;
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                foreach (TableCell cell in e.Row.Cells)
                {
                    LinkButton lbSort = (LinkButton)cell.Controls[0];
                    if (lbSort.CommandArgument == SPgrdQuestion.SortExpression)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "MyKey", "<script language='javascript'  type='text/javascript'>changeArrowImg()</script>");
                    }
                }
            }
        }

    }
}
