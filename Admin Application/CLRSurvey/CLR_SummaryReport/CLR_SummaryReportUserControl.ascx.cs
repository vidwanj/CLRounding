using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace CLRSurvey.CLR_SummaryReport
{
    public partial class CLR_SummaryReportUserControl : UserControl
    {
        int totalAverage = 0; //to store average of percentage

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) //check page is postback or not
            {
                dtcfromdate.SelectedDate = DateTime.Today;
                dtctodate.SelectedDate = DateTime.Today;
                fnBindddlentity();
                fnBindGridView();
            }
            else
            {

            }
        }

        protected void ddlentity_SelectedIndexChanged(object sender, EventArgs e)
        {
            fnBindddllocation();
            fnBindGridView();
        }

        protected void ddllocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            fnBindddldepartment();
            fnBindGridView();
        }

        protected void ddldepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            fnBindGridView();
        }

        protected void ImgbtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            if (dtcfromdate.IsDateEmpty)
            {
                Span1.Visible = true;
                Span1.InnerText = "* Please Select From Date";
            }
            else if (dtctodate.IsDateEmpty)
            {
                Span2.Visible = true;
                Span2.InnerText = "* Please Select To Date";
            }
            else if (dtcfromdate.SelectedDate > dtctodate.SelectedDate)
            {
                Span1.Visible = true;
                Span1.InnerText = "* From Date Can not be greater than To Date";
            }
            else
            {
                Span1.Visible = false;
                Span2.Visible = false;
                fnBindGridView();
            }
        }

        protected void linkbtnEntity_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                GridViewRow gvRow = (GridViewRow)btn.NamingContainer;
                int intRowIndex = gvRow.RowIndex;

                if (intRowIndex >= 0)
                {
                    ddlentity.SelectedIndex = ddlentity.Items.IndexOf(ddlentity.Items.FindByText(btn.Text));
                    fnBindGridView();
                    ddllocation.Items.Clear();
                    fnBindddllocation();
                }
                //tdBack.Controls.Add(new LiteralControl(" <a id=\"btnBack\" href=\"" + Page.Request.Url.ToString() + "\" runat=\"server\">< Back</a>"));
                tdBack.Controls.Add(new LiteralControl(" <a id=\"btnBack\" href=\"javascript:history.back(-1);\" runat=\"server\">< Back</a>"));
            }
            catch (Exception ex)
            {
                lblerrormsg.Text = ex.Message;
            }
        }

        protected void linkbtnLocation_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                GridViewRow gvRow = (GridViewRow)btn.NamingContainer;
                int intRowIndex = gvRow.RowIndex;
                if (intRowIndex >= 0)
                {
                    ddllocation.SelectedIndex = ddllocation.Items.IndexOf(ddllocation.Items.FindByText(btn.Text));
                    fnBindGridView();
                    ddldepartment.Items.Clear();
                    fnBindddldepartment();
                }
            }
            catch (Exception ex)
            {
                lblerrormsg.Text = ex.Message;
            }
            tdBack.Controls.Add(new LiteralControl(" <a id=\"btnBack\" href=\"javascript:history.back(-1);\" runat=\"server\">< Back</a>"));
        }

        protected void linkbtnDepartment_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                GridViewRow gvRow = (GridViewRow)btn.NamingContainer;
                int intRowIndex = gvRow.RowIndex;
                if (intRowIndex >= 0)
                {
                    ddldepartment.SelectedIndex = ddldepartment.Items.IndexOf(ddldepartment.Items.FindByText(btn.Text));
                    fnBindGridView();
                }
            }
            catch (Exception ex)
            {
                lblerrormsg.Text = ex.Message;
            }
            tdBack.Controls.Add(new LiteralControl(" <a id=\"btnBack\" href=\"javascript:history.back(-1);\" runat=\"server\">< Back</a>"));
        }

        protected void gdvCLRSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (ddlentity.SelectedIndex != 0) // check ddlentity is selected or not
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = true;
                if (ddllocation.SelectedIndex != 0) //check ddllocation is selected or not
                {
                    e.Row.Cells[1].Visible = false;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[4].Visible = true;

                    if (ddldepartment.SelectedIndex != 0) //check ddldepartment is select or not
                    {
                        e.Row.Cells[2].Visible = false;
                        e.Row.Cells[3].Visible = true;
                        e.Row.Cells[4].Visible = false;

                        //check date time controls are not empty and fromdate is less than todate
                        if (!dtcfromdate.IsDateEmpty && !dtctodate.IsDateEmpty && dtcfromdate.SelectedDate == dtctodate.SelectedDate)
                        {
                            DateTime fromdate = dtcfromdate.SelectedDate;
                            DateTime todate = dtctodate.SelectedDate;
                            TimeSpan spandiff = todate - fromdate; //calculate difference between two date
                            double datediff = spandiff.TotalDays;
                            //check date difference is 0 or not
                            if (datediff == 0)
                            {
                                if (e.Row.RowType == DataControlRowType.DataRow)
                                {
                                    LinkButton linkbtnRoomNo = (LinkButton)e.Row.FindControl("linkbtnRoomNo");
                                    linkbtnRoomNo.Visible = true;
                                    Label lblRoomNo = (Label)e.Row.FindControl("lblRoomNo");
                                    lblRoomNo.Visible = false;
                                    string roomDescr = lblRoomNo.Text;
                                    string strTodate = dtctodate.SelectedDate.ToString("MM-dd-yyyy");
                                    linkbtnRoomNo.Text = "<a style=\"cursor:hand\"  onclick=\"openDiaWindow('" + roomDescr + "','" + strTodate + "')\" >" + roomDescr + "</a>"; ;
                                }
                            }
                            else
                            {
                                if (e.Row.RowType == DataControlRowType.DataRow)
                                {
                                    LinkButton linkbtnRoomNo = (LinkButton)e.Row.FindControl("linkbtnRoomNo");
                                    linkbtnRoomNo.Visible = false;
                                    Label lblRoomNo = (Label)e.Row.FindControl("lblRoomNo");
                                    lblRoomNo.Visible = true;
                                }
                            }
                        }
                        else
                        {
                            if (e.Row.RowType == DataControlRowType.DataRow)
                            {
                                LinkButton linkbtnRoomNo = (LinkButton)e.Row.FindControl("linkbtnRoomNo");
                                linkbtnRoomNo.Visible = false;
                                Label lblRoomNo = (Label)e.Row.FindControl("lblRoomNo");
                                lblRoomNo.Visible = true;
                            }
                        }
                    }
                }
                else
                {
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = true;
                }
            }
            else
            {
                e.Row.Cells[0].Visible = true;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = true;
            }

            //Bind the total counted in fnLoadGridViewData to footer labels
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblEntityTotal = (Label)e.Row.FindControl("lblEntityTotal");
                Label lblLocationTotal = (Label)e.Row.FindControl("lblLocationTotal");
                Label lblDepartmentTotal = (Label)e.Row.FindControl("lblDepartmentTotal");
                Label lblRoomNoTotal = (Label)e.Row.FindControl("lblRoomNoTotal");

                lblEntityTotal.Text = totalAverage.ToString() + "%";
                lblLocationTotal.Text = totalAverage.ToString() + "%";
                lblDepartmentTotal.Text = totalAverage.ToString() + "%";
                lblRoomNoTotal.Text = totalAverage.ToString() + "%";
            }
        }

        //function to get connection and returns SqlConnection object.
        private SqlConnection fngetConnection()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection();
                sqlcon.ConnectionString = ConfigurationManager.AppSettings["clr_ConStr"];
                return sqlcon;
            }
            catch (Exception ex)
            {
                lblerrormsg.Text = ex.Message;
                return null;
            }
        }

        //function to Bind data to dropdown list ddlentity.
        protected void fnBindddlentity()
        {
            //function to bind data to ddlentity
            SqlConnection sqlcon = fngetConnection();

            try
            {
                SqlCommand sqlcmd = new SqlCommand("SP_GetAllEntity", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sqlda = new SqlDataAdapter();
                sqlda.SelectCommand = sqlcmd;
                DataSet sqlds = new DataSet();
                sqlda.Fill(sqlds);
                ddlentity.DataTextField = "EntityID";
                ddlentity.DataValueField = "EntityID";
                ddlentity.DataSource = sqlds;
                ddlentity.DataBind();
                ddlentity.Items.Insert(0, new ListItem("--Select--", "0"));
                ddllocation.Items.Insert(0, new ListItem("--Select--", "0"));
                ddldepartment.Items.Clear();
                ddldepartment.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception ex)
            {
                lblerrormsg.Text = "Error : " + ex.Message;
            }
        }

        //function to Bind data to dropdown list ddllocation.
        protected void fnBindddllocation()
        {
            //function to bind data to ddllocation
            SqlConnection sqlcon = fngetConnection();

            try
            {
                SqlCommand sqlcmd = new SqlCommand("SP_GetLocation_LoadByEntityID", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@EntityID", Convert.ToString(ddlentity.SelectedValue));
                SqlDataAdapter sqlda = new SqlDataAdapter();
                sqlda.SelectCommand = sqlcmd;
                DataSet sqlds = new DataSet();
                sqlda.Fill(sqlds);
                ddllocation.DataTextField = "LocatnAbbrvtnName";
                ddllocation.DataValueField = "LocatnID";
                ddllocation.DataSource = sqlds;
                ddllocation.DataBind();
                ddllocation.Items.Insert(0, new ListItem("--Select--", "0"));
                ddldepartment.Items.Clear();
                ddldepartment.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception ex)
            {
                lblerrormsg.Text = "Error : " + ex.Message;
            }
        }

        //function to Bind data to dropdown list ddldepartment.
        protected void fnBindddldepartment()
        {
            //function to bind data to ddldepartment
            SqlConnection sqlcon = fngetConnection();

            try
            {
                SqlCommand sqlcmd = new SqlCommand("SP_GetDept_LoadByLocationID", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@LocationID", Convert.ToString(ddllocation.SelectedValue));
                SqlDataAdapter sqlda = new SqlDataAdapter();
                sqlda.SelectCommand = sqlcmd;
                DataSet sqlds = new DataSet();
                sqlda.Fill(sqlds);

                // Binded dataset to control Dropdownlist and declared the data text field and data value field
                ddldepartment.DataTextField = "DeptAbbrvtnName";
                ddldepartment.DataValueField = "DeptID";
                ddldepartment.DataSource = sqlds;
                ddldepartment.DataBind();
                ddldepartment.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception ex)
            {
                // if an unhandled exception occurs it will be displayed on the web page
                lblerrormsg.Text = "Error : " + ex.Message;
            }
        }

        //function to Bind data to gridview gdvCLRSummary. 
        protected void fnBindGridView()
        {
            try
            {
                DataTable dtGridDataTable = fnLoadGridViewData();
                if (dtGridDataTable.Rows.Count != 0 || dtGridDataTable != null)
                {
                    gdvCLRSummary.DataSource = dtGridDataTable;
                    gdvCLRSummary.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblerrormsg.Text = ex.Message;
            }
        }

        //function to Get data from data source to bind gridview gdvCLRSummary and returns
        //DataTable. called in fnBindGridView function.
        protected DataTable fnLoadGridViewData()
        {
            //function to bind data to ddldepartment
            SqlConnection sqlcon = fngetConnection();
            totalAverage = 0;

            try
            {

                SqlCommand sqlcmd = new SqlCommand("SP_GetCLRSummary_LoadByFilter", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                if (ddlentity.SelectedIndex == 0)
                {
                    sqlcmd.Parameters.AddWithValue("@entityID", null);
                }
                else
                {
                    sqlcmd.Parameters.AddWithValue("@entityID", Convert.ToString(ddlentity.SelectedValue));
                }
                if (ddllocation.SelectedIndex == 0)
                {
                    sqlcmd.Parameters.AddWithValue("@locatnID", null);
                }
                else
                {
                    sqlcmd.Parameters.AddWithValue("@locatnID", Convert.ToString(ddllocation.SelectedValue));
                }
                if (ddldepartment.SelectedIndex == 0)
                {
                    sqlcmd.Parameters.AddWithValue("@deptID", null);
                }
                else
                {
                    sqlcmd.Parameters.AddWithValue("@deptID", Convert.ToString(ddldepartment.SelectedValue));
                }
                sqlcmd.Parameters.AddWithValue("@fromdate", Convert.ToString(dtcfromdate.SelectedDate));
                sqlcmd.Parameters.AddWithValue("@todate", Convert.ToString(dtctodate.SelectedDate));
                SqlDataAdapter sqlda = new SqlDataAdapter();
                sqlda.SelectCommand = sqlcmd;
                DataTable sqldt = new DataTable();
                sqlda.Fill(sqldt);

                //to calculate Entity above 92% and store in variable to bind the footer label in RowDataBound event

                int PercentCounter = 0;
                for (int i = 0; i < sqldt.Rows.Count; i++)
                {
                    if (Convert.ToInt32(sqldt.Rows[i]["percentcount"].ToString()) >= 92)
                    {
                        PercentCounter++;
                    }
                }

                if (Convert.ToInt32(sqldt.Rows.Count) > 0)
                {
                    totalAverage = PercentCounter * 100 / (Convert.ToInt32(sqldt.Rows.Count));
                }

                //returns resultant datatable.
                return sqldt;
            }
            catch (Exception ex)
            {
                // if an unhandled exception occurs it will be displayed on the web page
                lblerrormsg.Text = "Error : " + ex.Message;
                return null;
            }
        }

    }
}
