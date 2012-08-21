using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.Data;
//using System.Web.Services;
using System.Text;
using System.Configuration;

namespace CLRSurvey.QustnRespSummary
{
    public partial class QustnRespSummaryUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
              // frmDate.SelectedDate =Convert.ToDateTime(DateTime.Now.Date);
              // toDate.SelectedDate = Convert.ToDateTime(DateTime.Now.Date);
                fnBindddlentity();
                fnBindddlresponsetype();                       
            }
        }

        protected void ddlentity_SelectedIndexChanged(object sender, EventArgs e)
        {
            fnBindddllocation();
        }

        protected void ddllocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            fnBindddldepartment();
        }

        protected void ddldepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            fnBindddlroom();
        }

        private SqlConnection fngetConnection()
        {
            //function to get connection to database
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.AppSettings["clr_ConStr"]; // get appSettings for current appln configuretion
           
            return con;
        }

        protected void fnBindddlresponsetype()
        {
            //function to bind data to ddlresnonse
            SqlConnection sqlcon = fngetConnection();

            try
            {
                SqlCommand sqlcmd = new SqlCommand("SP_GetAllRespnsType", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sqlda = new SqlDataAdapter();
                sqlda.SelectCommand = sqlcmd;
                DataSet sqlds = new DataSet();
                sqlda.Fill(sqlds);
                ddlresponsetype.DataTextField = "RespnsTypeDescr";
                ddlresponsetype.DataValueField = "RespnsTypeCode";
                ddlresponsetype.DataSource = sqlds;
                ddlresponsetype.DataBind();
                ddlresponsetype.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception ex)
            {
                lblerrormsg.Visible = true;
                lblerrormsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                lblerrormsg.Text = "Error : " + ex.Message;
            }
        }

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
                ddlentity.DataTextField = "EntityName";
                ddlentity.DataValueField = "EntityID";
                ddlentity.DataSource = sqlds;
                ddlentity.DataBind();
                ddlentity.Items.Insert(0, new ListItem("--Select--", "0"));
                ddllocation.Items.Insert(0, new ListItem("--Select--", "0"));
                ddldepartment.Items.Clear();
                ddldepartment.Items.Insert(0, new ListItem("--Select--", "0"));
                ddlroom.Items.Clear();
                ddlroom.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception ex)
            {
                lblerrormsg.Visible = true;
                lblerrormsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                lblerrormsg.Text = "Error : " + ex.Message;
            }
        }

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
                ddllocation.DataTextField = "LocatnName";
                ddllocation.DataValueField = "LocatnID";
                ddllocation.DataSource = sqlds;
                ddllocation.DataBind();
                ddllocation.Items.Insert(0, new ListItem("--Select--", "0"));
                ddldepartment.Items.Clear();
                ddldepartment.Items.Insert(0, new ListItem("--Select--", "0"));
                ddlroom.Items.Clear();
                ddlroom.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception ex)
            {
                lblerrormsg.Visible = true;
                lblerrormsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                lblerrormsg.Text = "Error : " + ex.Message;
            }
        }

        protected void fnBindddldepartment()
        {
            //function to bind data to ddldepartment
            SqlConnection sqlcon = fngetConnection();

            try
            {
                // created sqlcommand object and passes stored procedure and sqlconnection object to sqlcommand object
                SqlCommand sqlcmd = new SqlCommand("SP_GetDept_LoadByLocationID", sqlcon);

                // assigned sqlcommandType object to storedprocedure
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@LocationID", Convert.ToString(ddllocation.SelectedValue));

                // created sqlDataAdapter object to establish path between database and dataset
                SqlDataAdapter sqlda = new SqlDataAdapter();

                // assigned sqlcommand object to sqlDataAdapter
                sqlda.SelectCommand = sqlcmd;

                // created new dataset to store data from database
                DataSet sqlds = new DataSet();

                // sqlDataAdapter fills data from database into DataSet
                sqlda.Fill(sqlds);

                // Binded dataset to control Dropdownlist and declared the data text field and data value field
                ddldepartment.DataTextField = "Name";
                ddldepartment.DataValueField = "DeptID";
                ddldepartment.DataSource = sqlds;
                ddldepartment.DataBind();
                ddldepartment.Items.Insert(0, new ListItem("--Select--", "0"));
                ddlroom.Items.Clear();
                ddlroom.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception ex)
            {
                // if an unhandled exception occurs it will be displayed on the web page
                lblerrormsg.Visible = true;
                lblerrormsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                lblerrormsg.Text = "Error : " + ex.Message;
            }
        }

        protected void fnBindddlroom()
        {
            //function to bind data to ddlroom
            SqlConnection sqlcon = fngetConnection();

            try
            {
                SqlCommand sqlcmd = new SqlCommand("sp_Room_GetByDeptId", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@DeptID", Convert.ToString(ddldepartment.SelectedValue));
                SqlDataAdapter sqlda = new SqlDataAdapter();
                sqlda.SelectCommand = sqlcmd;
                DataSet sqlds = new DataSet();
                sqlda.Fill(sqlds);
                ddlroom.DataTextField = "RoomID";
                ddlroom.DataValueField = "RoomID";
                ddlroom.DataSource = sqlds;
                ddlroom.DataBind();
                ddlroom.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception ex)
            {
                lblerrormsg.Visible = true;
                lblerrormsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                lblerrormsg.Text = "Error : " + ex.Message;
            }
        }        

    }
}
