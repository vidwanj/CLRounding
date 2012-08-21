using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace CLRSurvey.CLRStatusSummary
{
    public partial class CLRStatusSummaryUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //txtfromdate.Text = DateTime.Now.Date.ToString("MM/dd/yyyy");
            //txttodate.Text = DateTime.Now.Date.ToString("MM/dd/yyyy");
            if (!Page.IsPostBack)
            {
                Bindddlentity();
            }
        }
       
        protected void ddlentity_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bindddllocation();
        }
        
        private SqlConnection getConnection()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.AppSettings["clr_ConStr"];           
            return con;
        }

        protected void Bindddlentity()
        {
            SqlConnection sqlcon = getConnection();

            try
            {
                // created sqlcommand object and passes stored procedure and sqlconnection object to sqlcommand object
                SqlCommand sqlcmd = new SqlCommand("SP_GetAllEntity", sqlcon);

                // assigned sqlcommandType object to storedprocedure
                sqlcmd.CommandType = CommandType.StoredProcedure;

                // created sqlDataAdapter object to establish path between database and dataset
                SqlDataAdapter sqlda = new SqlDataAdapter();

                // assigned sqlcommand object to sqlDataAdapter
                sqlda.SelectCommand = sqlcmd;

                // created new dataset to store data from database
                DataSet sqlds = new DataSet();

                // sqlDataAdapter fills data from database into DataSet
                sqlda.Fill(sqlds);

                // Binded dataset to control Dropdownlist and declared the data text field and data value field
                ddlentity.DataTextField = "EntityName";
                ddlentity.DataValueField = "EntityID";
                ddlentity.DataSource = sqlds;
                ddlentity.DataBind();
                ddlentity.Items.Insert(0, new ListItem("--Select--", "0"));
                ddllocation.Items.Insert(0, new ListItem("--Select--", "0"));
               
            }
            catch (Exception ex)
            {
                // if an unhandled exception occurs it will be displayed on the web page
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                string strError = ex.Message;
                lblMsg.Text = strError;
            }
        }

        protected void Bindddllocation()
        {
            SqlConnection sqlcon = getConnection();

            try
            {
                // created sqlcommand object and passes stored procedure and sqlconnection object to sqlcommand object
                SqlCommand sqlcmd = new SqlCommand("SP_GetLocation_LoadByEntityID", sqlcon);

                // assigned sqlcommandType object to storedprocedure
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@EntityID", Convert.ToString(ddlentity.SelectedValue));

                // created sqlDataAdapter object to establish path between database and dataset
                SqlDataAdapter sqlda = new SqlDataAdapter();

                // assigned sqlcommand object to sqlDataAdapter
                sqlda.SelectCommand = sqlcmd;

                // created new dataset to store data from database
                DataSet sqlds = new DataSet();

                // sqlDataAdapter fills data from database into DataSet
                sqlda.Fill(sqlds);

                // Binded dataset to control Dropdownlist and declared the data text field and data value field
                ddllocation.DataTextField = "LocatnName";
                ddllocation.DataValueField = "LocatnID";
                ddllocation.DataSource = sqlds;
                ddllocation.DataBind();
                ddllocation.Items.Insert(0, new ListItem("--Select--", "0"));
               
            }
            catch (Exception ex)
            {
                // if an unhandled exception occurs it will be displayed on the web page
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                string strError = ex.Message;
                lblMsg.Text = strError;
            }
        }
       
    }
}
