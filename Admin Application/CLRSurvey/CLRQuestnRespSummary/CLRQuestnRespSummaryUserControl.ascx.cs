using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CLRSurvey.CLRQuestnRespSummary
{
    public partial class CLRQuestnRespSummaryUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string entiyID = null;
                string locationID = null;
                string departmentID = null;
                string roomDescr = null;
                string todate = null;
                if (Page.Request.QueryString["entity"] != null)
                {
                    entiyID = Page.Request.QueryString["entity"].ToString();
                }
                if (Page.Request.QueryString["location"] != null)
                {
                    locationID = Page.Request.QueryString["location"].ToString();
                }
                if (Page.Request.QueryString["dept"] != null)
                {
                    departmentID = Page.Request.QueryString["dept"].ToString();
                }
                if (Page.Request.QueryString["roomDescr"] != null)
                {
                    roomDescr = Page.Request.QueryString["roomDescr"].ToString();
                }
                if (Page.Request.QueryString["todate"] != null)
                {
                    todate = Page.Request.QueryString["todate"].ToString();
                }

                DataTable dtgridview = fnGetCLRQuestnData(entiyID, locationID, departmentID, roomDescr, todate);
                if (dtgridview != null)
                {
                    if (dtgridview.Rows.Count != 0)
                    {
                        gdvCLRQuestnSummary.DataSource = dtgridview;
                        gdvCLRQuestnSummary.DataBind();
                    }
                    else
                    {
                        lblerrormsg.Text = "There Are No Answers For This CLR.";
                    }
                }
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

        protected DataTable fnGetCLRQuestnData(string entiyID, string locationID, string departmentID, string roomDescr, string todate)
        {
            //function to bind data to ddldepartment
            SqlConnection sqlcon = fngetConnection();
            try
            {

                SqlCommand sqlcmd = new SqlCommand("SP_GetQuestnRespns_LoadByFilter", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@entityID", entiyID);
                sqlcmd.Parameters.AddWithValue("@locatnID", locationID);
                sqlcmd.Parameters.AddWithValue("@deptID", departmentID);
                sqlcmd.Parameters.AddWithValue("@roomDescr", roomDescr);
                sqlcmd.Parameters.AddWithValue("@date", todate);
                SqlDataAdapter sqlda = new SqlDataAdapter();
                sqlda.SelectCommand = sqlcmd;
                DataTable sqldt = new DataTable();
                sqlda.Fill(sqldt);
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