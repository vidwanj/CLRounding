using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Microsoft.SharePoint;

namespace CLRSurvey.Admin_MaintainDept
{
    public partial class Admin_MaintainDeptUserControl : UserControl
    {
        SqlCommand selectCommand = null;
        SqlConnection cn = new SqlConnection();    
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dtSvcLine = new DataTable();
        DataTable dtLocation = new DataTable();
        DataTable dtDept = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            txtDeptname.Attributes.Add("onKeyUp", "countCharacters()");
            cn.ConnectionString = ConfigurationManager.AppSettings["clr_ConStr"]; // get appSettings for current appln configuretion
            selectCommand = cn.CreateCommand();
            if (!Page.IsPostBack)
            {
                try
                {
                    GetAllSvcLineIdName();
                    GetAllLocation();
                    if (Request.QueryString["Did"] != null)
                    {
                        ImgbtnClear.Visible = false;
                        int iDeptID = Convert.ToInt32(Request.QueryString["Did"]);
                        selectCommand.Parameters.Add(new SqlParameter("@DeptID", iDeptID)); //Parameters for stored proc
                        selectCommand.CommandType = CommandType.StoredProcedure;    //command thru stored proc
                        selectCommand.CommandText = "sp_Dept_ByDeptId";      // stored proc name                      
                        da.SelectCommand = selectCommand;
                        da.Fill(dtDept);
                        txtAbbreviation.Text = dtDept.Rows[0]["DeptAbbrvtnName"].ToString(); // gets contained data in dataTable row into textBox
                        txtDeptname.Text = dtDept.Rows[0]["Name"].ToString();
                        ddlLocation.SelectedValue = dtDept.Rows[0]["LocatnID"].ToString();
                        ddlSvcLine.SelectedValue = dtDept.Rows[0]["SvcLineID"].ToString();
                        if (dtDept.Rows[0]["ActvInactvInd"].ToString() == "Y" || dtDept.Rows[0]["ActvInactvInd"].ToString() == "y")
                        {
                            ddlActvnInactvn.SelectedValue = "Y";
                        }
                        else
                        {
                            ddlActvnInactvn.SelectedValue = "N";
                        }
                        txtDeptID.Text = dtDept.Rows[0]["DeptID"].ToString();
                    }
                    else if (Request.QueryString["locid"] != null)
                    {
                        txtDeptID.Text = GetNewDeptId();
                        ddlLocation.SelectedIndex = ddlLocation.Items.IndexOf(ddlLocation.Items.FindByValue(Request.QueryString["locid"].ToString()));
                    }
                    else
                    {
                        txtDeptID.Text = GetNewDeptId();
                        ImgbtnClear.Visible = true;                        
                    }
                }
                catch (Exception ex)
                {
                    string strError = ex.Message;
                    lblMsg.Text = strError;
                    lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                    lblMsg.Visible = true;                   
                }
            }
        }

        protected string GetNewDeptId()
        {
            string strDeptID = string.Empty;
            DataTable dtDeptid = new DataTable();
            try
            {
                selectCommand = cn.CreateCommand();
                selectCommand.CommandType = CommandType.StoredProcedure;    //command thru stored proc
                selectCommand.CommandText = "sp_Dept_GetAll";      // stored proc name                      
                da.SelectCommand = selectCommand;
                da.Fill(dtDeptid);

                int max = Convert.ToInt32(dtDeptid.Rows[0][0]); // assuming you want the third column (index 2)
                for (int i = 1; i < dtDeptid.Rows.Count; i++) 
                {
                    if (max < Convert.ToInt32(dtDeptid.Rows[i][0]))
                    {
                        max = Convert.ToInt32(dtDeptid.Rows[i][0]);
                    }
                }

                strDeptID = (max+1).ToString();
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
                lblMsg.Text = strError;
                lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                lblMsg.Visible = true;    
            }
            return strDeptID;
        }

        // method to get all SVC Line Name & ID
        protected void GetAllSvcLineIdName()
        {
            selectCommand.CommandType = CommandType.StoredProcedure;    //command thru stored proc
            selectCommand.CommandText = "sp_SvcLine_GetIdName";      // stored proc name                      
            da.SelectCommand = selectCommand;
            da.Fill(dtSvcLine);
            ddlSvcLine.DataSource = dtSvcLine;
            ddlSvcLine.DataTextField = "SvcLineName";
            ddlSvcLine.DataValueField = "SvcLineID";
            ddlSvcLine.DataBind();
            ddlSvcLine.Items.Insert(0, new ListItem("--Select--"));
        }

        // Method to get All Location Name & ID
        protected void GetAllLocation()
        {
            selectCommand.CommandType = CommandType.StoredProcedure;    //command thru stored proc
            selectCommand.CommandText = "SP_GetAllLocation";      // stored proc name                      
            da.SelectCommand = selectCommand;
            da.Fill(dtLocation);
            ddlLocation.DataSource = dtLocation;
            ddlLocation.DataTextField = "LocatnName";
            ddlLocation.DataValueField = "LocatnID";
            ddlLocation.DataBind();
            ddlLocation.Items.Insert(0, new ListItem("--Select--"));
        }

        protected void ImgbtnSave_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string CurrentUser = SPContext.Current.Web.CurrentUser.Name;

                selectCommand = cn.CreateCommand();
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }
                //command thru stored proc, parameters list
                selectCommand.Parameters.Add(new SqlParameter("@DeptAbbrvtnName", txtAbbreviation.Text)); //Abbreviation Text Param
                selectCommand.Parameters.Add(new SqlParameter("@LocatnID", ddlLocation.SelectedValue)); // Location Id Param
                selectCommand.Parameters.Add(new SqlParameter("@Name", txtDeptname.Text));   // Dept name Text
                selectCommand.Parameters.Add(new SqlParameter("@SvcLineID", ddlSvcLine.SelectedValue)); // Svc Line Id
                selectCommand.Parameters.Add(new SqlParameter("@ActvInactvInd", ddlActvnInactvn.SelectedValue)); // Active Inactive Yes/No
                selectCommand.Parameters.Add(new SqlParameter("@DefltSortOrdr", DBNull.Value));  // Default Sort Order
                selectCommand.Parameters.Add(new SqlParameter("@UpdateDate", System.DateTime.Now.ToShortDateString()));  // Current Date
                selectCommand.Parameters.Add(new SqlParameter("@UpdateOpertr", CurrentUser));  // Current UserName


                if (Request.QueryString["Did"] != null)
                {
                    int iDeptid = Convert.ToInt32(Request.QueryString["Did"]);
                    selectCommand.Parameters.Add(new SqlParameter("@DeptID", iDeptid));
                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandText = "sp_Dept_Update";      // stored proc name
                    selectCommand.ExecuteNonQuery();

                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.FromName("#33CC33");
                    lblMsg.Text = "Updated Successfully";
                }
                else
                {
                    selectCommand.Parameters.Add(new SqlParameter("@DeptID", txtDeptID.Text));
                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandText = "sp_Dept_Insert";      // stored proc name
                    selectCommand.ExecuteNonQuery();
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.FromName("#33CC33");
                    lblMsg.Text = "Added Successfully";
                }

                Page.ClientScript.RegisterStartupScript(GetType(), "MyKey1", "<script language='javascript'  type='text/javascript'>window.frameElement.commitPopup();</script>");
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
            txtDeptname.Text = "";
            txtAbbreviation.Text = "";
            txtDeptID.Text = GetNewDeptId();
            ddlActvnInactvn.SelectedIndex = 0;
            ddlSvcLine.SelectedIndex = 0;
            if (Request.QueryString["locid"] == null)
            {
                ddlLocation.SelectedIndex = 0;
            }
        }
    }
}
