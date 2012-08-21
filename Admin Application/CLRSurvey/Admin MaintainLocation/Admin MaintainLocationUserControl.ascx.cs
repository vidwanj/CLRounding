using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace CLRSurvey.Admin_MaintainLocation
{
    public partial class Admin_MaintainLocationUserControl : UserControl
    {
        SqlCommand selectCommand = null;
        SqlConnection cn = new SqlConnection();
        DataTable dtLocationList = new DataTable();
        DataTable dtEntityList = new DataTable();
        DataTable dtMaxLocatnID = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter();

        protected void Page_Load(object sender, EventArgs e)
        {
            cn.ConnectionString = ConfigurationManager.AppSettings["clr_ConStr"]; // get appSettings for current appln configuretion
            if (!Page.IsPostBack)
            {
                try
                {
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
                        ddlEntiyID.DataSource = dtEntityList;
                        ddlEntiyID.DataTextField = "EntityID";
                        ddlEntiyID.DataValueField = "EntityID";
                        ddlEntiyID.DataBind();
                        ddlEntiyID.Items.Insert(0, new ListItem("--Select--"));
                    }
                    if (Request.QueryString["locatnid"] != null)
                    {
                        ImgbtnClear.Visible = false;
                        // get LocationId from QueryString  
                        string strLocatnId = Request.QueryString["locatnid"];
                        selectCommand.Parameters.Add(new SqlParameter("@LocatnID", strLocatnId)); //Parameters for stored proc
                        selectCommand.CommandType = CommandType.StoredProcedure;    //command through stored proc
                        selectCommand.CommandText = "SP_GetLocation_LoadByLocatnID";      // stored proc name
                        selectCommand.ExecuteNonQuery();
                        da.SelectCommand = selectCommand;
                        da.Fill(dtLocationList);
                        ddlEntiyID.SelectedIndex = ddlEntiyID.Items.IndexOf(ddlEntiyID.Items.FindByText(dtLocationList.Rows[0]["EntityID"].ToString()));
                        txtLocationID.Text = dtLocationList.Rows[0]["LocatnID"].ToString(); // gets contained data in dataTable row into textBox
                        txtLocationName.Text = dtLocationList.Rows[0]["LocatnName"].ToString();
                        txtAbbreviation.Text = dtLocationList.Rows[0]["LocatnAbbrvtnName"].ToString();

                        //to select dropdownlist value by FindByValue
                        ddlActvnInactvn.SelectedIndex = ddlActvnInactvn.Items.IndexOf(ddlActvnInactvn.Items.FindByValue(dtLocationList.Rows[0]["ActvInactvInd"].ToString()));
                        txtSortOrder.Text = dtLocationList.Rows[0]["DefltSortOrdr"].ToString();

                    }
                    else if (Request.QueryString["EntityID"] != null)
                        {
                            txtLocationID.Text = (Convert.ToInt32(GetMaxLocatnID()) + 1).ToString();    
                            ddlEntiyID.SelectedIndex = ddlEntiyID.Items.IndexOf(ddlEntiyID.Items.FindByText(Request.QueryString["EntityID"].ToString()));
                        }
                   else
                    {
                        txtLocationID.Text = (Convert.ToInt32(GetMaxLocatnID()) + 1).ToString();                        
                    }
                }
                catch (Exception ex)
                {
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                    lblMsg.Text = ex.Message;
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                    {
                        cn.Close();
                    }
                }
            }
        }

        public string GetMaxLocatnID()
        {
            try
            {
                string maxlocatnID = null;
                selectCommand = cn.CreateCommand();
                selectCommand.CommandType = CommandType.StoredProcedure;
                selectCommand.CommandText = "SP_GetLocatnIDMaxCount";
                //selectCommand.ExecuteNonQuery();
                da.SelectCommand = selectCommand;
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }
                da.Fill(dtMaxLocatnID);
                maxlocatnID = dtMaxLocatnID.Rows[0]["MaxLocatnID"].ToString();
                if (maxlocatnID == null)
                {
                    maxlocatnID = "1";
                }
                return maxlocatnID;
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                lblMsg.Text = ex.Message;
                return null;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
        }

        protected void ImgbtnSave_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                selectCommand = cn.CreateCommand();
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }

                // Adding & passing values to parameters

                selectCommand.Parameters.Add(new SqlParameter("@EntityID", ddlEntiyID.SelectedValue));
                selectCommand.Parameters.Add(new SqlParameter("@LocatnName", txtLocationName.Text));
                selectCommand.Parameters.Add(new SqlParameter("@LocatnAbbrvtnName", txtAbbreviation.Text));
                selectCommand.Parameters.Add(new SqlParameter("@ActvInactvInd", ddlActvnInactvn.SelectedValue));
                selectCommand.Parameters.Add(new SqlParameter("@DefltSortOrdr", txtSortOrder.Text));

                if (Request.QueryString["locatnid"] != null)
                {
                    string strLocatnId = Request.QueryString["locatnid"];
                    // Update the current edited row                
                    selectCommand.Parameters.Add(new SqlParameter("@LocatnID", strLocatnId));
                    selectCommand.CommandType = CommandType.StoredProcedure; //command through stored proc
                    selectCommand.CommandText = "sp_Locatn_Update"; // stored proc name                
                    selectCommand.ExecuteNonQuery();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Update", "ClosePopup();", true);
                }
                else
                {
                    // Insert new row to the Location table 
                    selectCommand.Parameters.Add(new SqlParameter("@LocatnID", txtLocationID.Text));
                    selectCommand.CommandType = CommandType.StoredProcedure; //command through stored proc
                    selectCommand.CommandText = "sp_Locatn_Insert"; // stored proc name                
                    selectCommand.ExecuteNonQuery();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Create", "ClosePopup();", true);
                }
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                lblMsg.Text = ex.Message;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
        }

        protected void ImgbtnClear_Click(object sender, ImageClickEventArgs e)
        {
            lblMsg.Visible = false;
            txtLocationID.Text = (Convert.ToInt32(GetMaxLocatnID()) + 1).ToString();
            txtLocationName.Text = "";
            txtAbbreviation.Text = "";
            ddlActvnInactvn.SelectedIndex = 0;
            txtSortOrder.Text = "";
            if (Request.QueryString["EntityID"] == null)
            {
                ddlEntiyID.SelectedIndex = 0;
            }
        }
    }
}
