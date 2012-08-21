using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace CLRSurvey.Admin_MaintainEmployee
{
    public partial class Admin_MaintainEmployeeUserControl : UserControl
    {
        SqlCommand selectCommand = null;
        SqlConnection cn = new SqlConnection();
        DataTable dtEmployeeList = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter();

        protected void Page_Load(object sender, EventArgs e)
        {
            cn.ConnectionString = ConfigurationManager.AppSettings["clr_ConStr"]; // get appSettings for current appln configuretion
            selectCommand = cn.CreateCommand();

            if (!Page.IsPostBack)
            {
                try
                {
                    if (Request.QueryString["empid"] != null)
                    {
                        if (Page.Request.QueryString["DeptName"] == null)
                        {
                            trddlDepartment.Visible = false;
                        }
                        ImgbtnClear.Visible = false;
                        // get EmployeeId from QueryString  
                        string strEmpId = Request.QueryString["empid"];
                        
                        selectCommand.Parameters.Add(new SqlParameter("@EmployeeID", strEmpId)); //Parameters for stored proc
                        selectCommand.CommandType = CommandType.StoredProcedure;    //command through stored proc
                        selectCommand.CommandText = "SP_GetEmployee_LoadByEmpID";      // stored proc name                        
                        da.SelectCommand = selectCommand;
                        da.Fill(dtEmployeeList);
                        if (dtEmployeeList != null)
                        {
                            if (dtEmployeeList.Rows.Count > 0)
                            {
                                if (dtEmployeeList.Rows[0]["EmployeeID"] != null)
                                {
                                    ArrayList _arrayList = new ArrayList();
                                    SPWeb web = SPContext.Current.Web;
                                    web.AllowUnsafeUpdates = true;
                                    SPUser User = null;
                                    
                                    User = web.EnsureUser(dtEmployeeList.Rows[0]["EmployeeID"].ToString());
                                    User = web.SiteUsers.GetByID(User.ID);

                                    PickerEntity pen = new PickerEntity();
                                    pen.Key = User.LoginName;
                                    pen.IsResolved = true;
                                    _arrayList.Add(pen);
                                    ppleditEmployeeID.UpdateEntities(_arrayList);
                                    txtEmployeeID.Text = ppleditEmployeeID.Accounts.Count > 0 ? ppleditEmployeeID.Accounts[0].ToString() : "";
                                }
                                txtEmployeeRole.Text = dtEmployeeList.Rows[0]["RoleName"].ToString();                       
                            }
                        }
                    }
                    else if (Page.Request.QueryString["DeptName"] != null)
                    {
                        trddlDepartment.Visible = true;

                        ddlDepartment.DataSource = GetAllDept();  // Call GetAll Department Function
                        ddlDepartment.DataTextField = "Name";
                        ddlDepartment.DataValueField = "DeptID";
                        ddlDepartment.DataBind();
                        ddlDepartment.Items.Insert(0, new ListItem("-- Select --"));

                        ddlDepartment.SelectedIndex = ddlDepartment.Items.IndexOf(ddlDepartment.Items.FindByText(Page.Request.QueryString["DeptName"].ToString()));
                    }
                    else
                    {
                        trddlDepartment.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                    lblMsg.Text = ex.Message;
                    cn.Close();
                }
            }
        }

        protected DataTable GetAllDept()        // gets all Dept from Dept table
        {
            DataTable dtDepartment = new DataTable();
            try
            {
                selectCommand = cn.CreateCommand();
                selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                selectCommand.CommandText = "sp_Dept_GetAll"; // stored proc name

                da.SelectCommand = selectCommand;
                da.Fill(dtDepartment);  // dataAdaptor fills all dept to Datatable dtDepartment
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                lblMsg.Text = ex.Message;
            }

            return dtDepartment;
        }

        protected void ImgbtnSave_Click(object sender, ImageClickEventArgs e)
        {
            selectCommand = cn.CreateCommand();
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }

            try
            {
                // Adding & passing values to parameters   
                if (ppleditEmployeeID.Accounts.Count > 0)
                {
                    lblppleditEmployeeID.Visible = false;
                    SPWeb oSPWeb = SPContext.Current.Web;
                    txtEmployeeID.Text = ppleditEmployeeID.Accounts.Count > 0 ? ppleditEmployeeID.Accounts[0].ToString() : "";
                    SPUser user = oSPWeb.EnsureUser(txtEmployeeID.Text);
                    if (user != null)
                    {
                        selectCommand.Parameters.Add(new SqlParameter("@EmployeeID", user.LoginName)); //ID== LoginName
                        selectCommand.Parameters.Add(new SqlParameter("@EmployeeName", user.Name)); //Name== Name
                        txtEmployeeID.Text = ppleditEmployeeID.Accounts.Count > 0 ? ppleditEmployeeID.Accounts[0].ToString() : "";
                    }
                }
                else
                {
                    lblppleditEmployeeID.Visible = true;
                    return;
                }
                selectCommand.Parameters.Add(new SqlParameter("@RoleName", txtEmployeeRole.Text));

                if (Request.QueryString["empid"] != null)
                {
                    string strEmpId = Request.QueryString["empid"];
                    // Update the current edited row
                    selectCommand.Parameters.Add(new SqlParameter("@EmployeeIDOld", strEmpId));
                    selectCommand.CommandType = CommandType.StoredProcedure; //command through stored proc
                    selectCommand.CommandText = "SP_EmployeeUpdate"; // stored proc name
                    selectCommand.ExecuteNonQuery();
                }
                else
                {
                    DataTable dtTemp = new DataTable();
                    SqlDataAdapter da1 = new SqlDataAdapter();
                    // Insert new row to the Employee table
                    selectCommand.CommandType = CommandType.StoredProcedure; //command through stored proc
                    selectCommand.CommandText = "SP_EmployeeInsert"; // stored proc name
                    da1.SelectCommand = selectCommand;
                    selectCommand.ExecuteNonQuery();
                    da1.Fill(dtTemp);
                    if (dtTemp.Rows[0]["ErrorMSG"].ToString() == "Employee Already Exist")
                    {
                        lblMsg.Visible = true;
                        lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                        lblMsg.Text = "Employee Already Exist";
                        return;
                    }
                    else if (Page.Request.QueryString["DeptName"] != null)
                    {
                        selectCommand = null;
                        selectCommand = cn.CreateCommand();
                        selectCommand.Parameters.Add(new SqlParameter("@DeptID", ddlDepartment.SelectedValue));
                        selectCommand.Parameters.Add(new SqlParameter("@EmployeeID", txtEmployeeID.Text));
                        selectCommand.CommandType = CommandType.StoredProcedure; //command through stored proc
                        selectCommand.CommandText = "sp_EmployeeDept_Insert"; // stored proc name
                        selectCommand.ExecuteNonQuery();
                    }
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
            // Clears all the controls
            
            lblMsg.Visible = false;
            ppleditEmployeeID.Accounts.Clear();
            ppleditEmployeeID.Entities.Clear();
            ppleditEmployeeID.CommaSeparatedAccounts = null;
            txtEmployeeID.Text = "";
            
            txtEmployeeRole.Text = "";
        }
    }
}
