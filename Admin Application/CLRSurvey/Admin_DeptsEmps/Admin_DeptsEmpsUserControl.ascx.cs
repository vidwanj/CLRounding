using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace CLRSurvey.Admin_DeptsEmps
{
    public partial class Admin_DeptsEmpsUserControl : UserControl
    {
        SqlCommand selectCommand = null;
        SqlDataAdapter da = null;
        SqlConnection cn = new SqlConnection();
        DataTable dtDepartment = new DataTable();
        DataTable dtEmpAll = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            cn.ConnectionString = ConfigurationManager.AppSettings["clr_ConStr"]; // connection string for clr_dev1 DB
            try
            {
                lblMsg.Visible = false;
                if (!IsPostBack)
                {
                    lstAvailable.Items.Clear();
                    lstMapped.Items.Clear();

                    fnBindEntityDropDownList();
                    fnBindDepartmentDropDownList();
                    fnBindEmployeeDropDownList();
                }
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                lblMsg.Text = strError;
            }  
        }

        protected void ddlEntity_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            fnBindLocationDropDownList();
            fnBindDepartmentDropDownList();
            ddlEmp.SelectedIndex = 0;
            lstAvailable.Items.Clear();
            lstMapped.Items.Clear();
        }

        protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            fnBindDepartmentDropDownList();
            ddlEmp.SelectedIndex = 0;
            lstAvailable.Items.Clear();
            lstMapped.Items.Clear();
        }

        protected void ddlDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlDept.SelectedIndex != 0)
                {
                    DataTable dtAllEmpTemp = GetAllEmployees();
                    DataTable dtMappedEmpTemp = GetEmployeeForDepartment();
                    if (dtAllEmpTemp != null && dtMappedEmpTemp != null)
                    {
                        //Remove Mapped Questions from List of All Questions
                        for (int i = 0; i < dtMappedEmpTemp.Rows.Count; i++)
                        {
                            for (int j = 0; j < dtAllEmpTemp.Rows.Count; j++)
                            {
                                if (dtAllEmpTemp.Rows[j]["EmployeeID"].ToString() == dtMappedEmpTemp.Rows[i]["EmployeeID"].ToString())
                                {
                                    dtAllEmpTemp.Rows.RemoveAt(j);
                                    dtAllEmpTemp.AcceptChanges();
                                }
                            }
                        }

                        dtAllEmpTemp.DefaultView.Sort = "EmployeeName";
                        dtMappedEmpTemp.DefaultView.Sort = "EmployeeName";
                    }

                    lblMsg.Visible = false;
                    ddlEmp.SelectedIndex = 0;
                    lblDeptEmpMsg.Text = "Saving Employees For Selected Department";
                    lblAvailable.Text = "Available Employees";
                    lblMapped.Text = "Mapped Employees";

                    lstAvailable.DataSource = dtAllEmpTemp; // dept DataTable for Available List of Dept for Selected Room
                    lstAvailable.DataTextField = "EmployeeName";
                    lstAvailable.DataValueField = "EmployeeID";
                    lstAvailable.DataBind();

                    lstMapped.DataSource = dtMappedEmpTemp;   // Selected row DataSource to Mapped List
                    lstMapped.DataTextField = "EmployeeName";
                    lstMapped.DataValueField = "EmployeeID";
                    lstMapped.DataBind();
                }
                else
                {
                    lstAvailable.Items.Clear();
                    lstMapped.Items.Clear();
                    lblDeptEmpMsg.Text = "";
                    lblMsg.Text = "";
                    lblAvailable.Text = "Available";
                    lblMapped.Text = "Mapped";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                lblMsg.Text = ex.Message;
            }
            
        }

        protected void ddlEmp_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlEmp.SelectedIndex != 0)
                {
                    DataTable dtAllDeptTemp = GetAllDept();
                    DataTable dtMappedDeptTemp = GetDepartmentForEmployee();

                    if (dtAllDeptTemp != null && dtMappedDeptTemp != null)
                    {
                        //Remove mapped Dept from all Departments
                        for (int i = 0; i < dtMappedDeptTemp.Rows.Count; i++)
                        {
                            for (int j = 0; j < dtAllDeptTemp.Rows.Count; j++)
                            {
                                if (dtAllDeptTemp.Rows[j]["DeptID"].ToString() == dtMappedDeptTemp.Rows[i]["DeptID"].ToString())
                                {
                                    dtAllDeptTemp.Rows.RemoveAt(j);
                                    dtAllDeptTemp.AcceptChanges();
                                }
                            }
                        }
                        dtMappedDeptTemp.DefaultView.Sort = "Name";
                        dtAllDeptTemp.DefaultView.Sort = "Name";
                    }                   

                    lblMsg.Visible = false;
                    ddlDept.SelectedIndex = 0;
                    lblDeptEmpMsg.Text = "Saving Departments For Selected Employee";
                    lblAvailable.Text = "Available Departments";
                    lblMapped.Text = "Mapped Department";

                    lstAvailable.DataSource = dtAllDeptTemp;
                    lstAvailable.DataTextField = "Name";
                    lstAvailable.DataValueField = "DeptID";
                    lstAvailable.DataBind();

                    lstMapped.DataSource = dtMappedDeptTemp;
                    lstMapped.DataTextField = "Name";
                    lstMapped.DataValueField = "DeptID";
                    lstMapped.DataBind();
                }
                else
                {
                    lstAvailable.Items.Clear();
                    lstMapped.Items.Clear();
                    lblDeptEmpMsg.Text = "";
                    lblMsg.Text = "";
                    lblAvailable.Text = "Available";
                    lblMapped.Text = "Mapped";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                lblMsg.Text = ex.Message;
            }
        }

        protected void ImgbtnSave_Click(object sender, ImageClickEventArgs e)
        {
            string strDeptID;
            string strEmployID;
            if (ddlDept.SelectedIndex != 0)    // if Dept selected
            {
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }
                try
                {
                    strDeptID = ddlDept.SelectedValue;
                    selectCommand = cn.CreateCommand();

                    selectCommand.Parameters.Add(new SqlParameter("@DeptID", strDeptID));
                    selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                    selectCommand.CommandText = "sp_EmployeeDept_Delete_ByDeptID";
                    selectCommand.ExecuteNonQuery();
                    for (int i = 0; i < lstMapped.Items.Count; i++)   // for the no. of items in Mapped Room list
                    {
                        strEmployID = lstMapped.Items[i].Value;
                        selectCommand = cn.CreateCommand();
                        selectCommand.Parameters.Add(new SqlParameter("@DeptID", strDeptID));
                        selectCommand.Parameters.Add(new SqlParameter("@EmployeeID", strEmployID));
                        selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                        selectCommand.CommandText = "sp_EmployeeDept_Insert"; // stored proc name
                        selectCommand.ExecuteNonQuery();
                    }
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.FromName("#33CC33");
                    lblMsg.Text = "Updated Successfully";
                }
                catch (Exception ex)
                {
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                    lblMsg.Text = ex.Message;
                }
            }

            else if (ddlEmp.SelectedIndex != 0) // if employee is selected 
            {
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }
                try
                {
                    strEmployID = ddlEmp.SelectedValue;
                    selectCommand = cn.CreateCommand();
                    selectCommand.Parameters.Add(new SqlParameter("@EmployeeID", strEmployID));
                    if (ddlEntity.SelectedIndex != 0)
                    {
                        selectCommand.Parameters.Add(new SqlParameter("@EntityID", ddlEntity.SelectedValue));
                    }
                    if (ddlLocation.SelectedIndex != 0)
                    {
                        selectCommand.Parameters.Add(new SqlParameter("@LocationID", ddlLocation.SelectedValue));
                    }
                    selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                    selectCommand.CommandText = "sp_EmployeeDept_Delete_ByEmpId"; // stored proc name
                    selectCommand.ExecuteNonQuery();

                    for (int i = 0; i < lstMapped.Items.Count; i++)   // for the no. of items in Mapped Room list
                    {
                        strDeptID = lstMapped.Items[i].Value;
                        selectCommand = cn.CreateCommand();
                        selectCommand.Parameters.Add(new SqlParameter("@DeptID", strDeptID));
                        selectCommand.Parameters.Add(new SqlParameter("@EmployeeID", strEmployID));
                        selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                        selectCommand.CommandText = "sp_EmployeeDept_Insert"; // stored proc name
                        selectCommand.ExecuteNonQuery();
                    }

                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.FromName("#33CC33");
                    lblMsg.Text = "Updated Successfully";
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
            else
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Items Not Selected";
            }
        }

        protected void ImgbtnAddItem_Click(object sender, ImageClickEventArgs e)
        {
            for (int i = 0; i < lstAvailable.Items.Count; i++)
            {
                if (lstAvailable.SelectedIndex > -1)
                {
                    string str_value = lstAvailable.SelectedItem.Value; //Gets the value of  items in list.
                    string str_text = lstAvailable.SelectedItem.Text;  // Gets the Text of items in the list.  
                    ListItem item = new ListItem(); //create a list item
                    item.Text = str_text;               //Assign the values to list item   
                    item.Value = str_value;
                    lstMapped.Items.Add(item); //Add the list item to the selected list of employees   
                    lstAvailable.Items.Remove(item); //Remove the details from employee list   
                }
            }
        }

        protected void ImgbtnRemoveItem_Click(object sender, ImageClickEventArgs e)
        {
            for (int i = lstMapped.Items.Count - 1; i >= 0; i--)
            {
                if (lstMapped.Items[i].Selected)
                {
                    lstAvailable.Items.Add(lstMapped.Items[i]);
                    lstAvailable.ClearSelection();
                    lstMapped.Items.Remove(lstMapped.Items[i]);
                }
            }
        }

        protected void ImgbtnAddAllItem_Click(object sender, ImageClickEventArgs e)
        {
            int i_count = lstAvailable.Items.Count;
            if (i_count != 0)
            {
                for (int i = 0; i < i_count; i++)
                {
                    ListItem item = new ListItem();
                    item.Text = lstAvailable.Items[i].Text;
                    item.Value = lstAvailable.Items[i].Value;
                    //Add the item to selected employee list
                    lstMapped.Items.Add(item);
                }
            }

            //clear employee list
            lstAvailable.Items.Clear();
        }

        protected void ImgbtnRemoveAllItem_Click(object sender, ImageClickEventArgs e)
        {
            int i_count = lstMapped.Items.Count;
            if (i_count != 0)
            {
                for (int i = 0; i < i_count; i++)
                {
                    ListItem item = new ListItem();
                    item.Text = lstMapped.Items[i].Text;
                    item.Value = lstMapped.Items[i].Value;
                    lstAvailable.Items.Add(item);
                }
            }

            lstMapped.Items.Clear();//clear the items  
        }

        private DataTable GetAllDept()
        {
            try
            {
                selectCommand = null;
                selectCommand = cn.CreateCommand();
                da = new SqlDataAdapter();
                selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                selectCommand.CommandText = "sp_Dept_GetAll"; // stored proc name
                if (ddlEntity.SelectedIndex != 0)
                {
                    selectCommand.Parameters.Add(new SqlParameter("@EntityID", ddlEntity.SelectedValue));
                }
                if (ddlLocation.SelectedIndex != 0)
                {
                    selectCommand.Parameters.Add(new SqlParameter("@LocationID", ddlLocation.SelectedValue));
                }
                da.SelectCommand = selectCommand;
                da.Fill(dtDepartment);        // fills dataset dsDepartment thru datatAdapter
                return dtDepartment;
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                lblMsg.Text = strError;
                return null;
            }
        }

        private DataTable GetAllEmployees()
        {
            try
            {
                selectCommand = null;
                da = new SqlDataAdapter();
                selectCommand = cn.CreateCommand();
                selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                selectCommand.CommandText = "sp_GetAllEmployees";
                da.SelectCommand = selectCommand;
                da.Fill(dtEmpAll);    // dataAdaptor fills all Room to DataTable dtRoom
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                lblMsg.Text = ex.Message;
            }
            return dtEmpAll;
        }

        private DataTable GetEmployeeForDepartment()
        {
            try
            {
                DataTable dtEmpID = new DataTable();
                selectCommand = null;
                da = new SqlDataAdapter();
                selectCommand = cn.CreateCommand();
                selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                selectCommand.Parameters.Add(new SqlParameter("@DeptId", ddlDept.SelectedValue));
                if (ddlEntity.SelectedIndex != 0)
                {
                    selectCommand.Parameters.Add(new SqlParameter("@EntityID", ddlEntity.SelectedValue));
                }
                if (ddlLocation.SelectedIndex != 0)
                {
                    selectCommand.Parameters.Add(new SqlParameter("@LocationID", ddlLocation.SelectedValue));
                }
                selectCommand.CommandText = "sp_EmployeeDept_GetEmpId";   // gets room info by DeptId
                da.SelectCommand = selectCommand;
                da.Fill(dtEmpID); // fills DataTable dtRoomID for selected Dept
                return dtEmpID;
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                lblMsg.Text = ex.Message;
                return null;
            }
        }

        private DataTable GetDepartmentForEmployee()
        {
            DataTable dtDepartment = new DataTable();
            try
            {
                selectCommand = cn.CreateCommand();
                selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                selectCommand.Parameters.Add( new SqlParameter("@EmployeeID", ddlEmp.SelectedValue));
                if (ddlEntity.SelectedIndex != 0)
                {
                    selectCommand.Parameters.Add(new SqlParameter("@EntityID",ddlEntity.SelectedValue));
                }
                if (ddlLocation.SelectedIndex != 0)
                {
                    selectCommand.Parameters.Add(new SqlParameter("@LocationID", ddlLocation.SelectedValue));
                }
                selectCommand.CommandText = "sp_EmployeeDept_GetDeptID";
                da.SelectCommand = selectCommand;
                da.Fill(dtDepartment);
                return dtDepartment;
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                lblMsg.Text = ex.Message;
                return null;
            }
        }

        private void fnBindEntityDropDownList()
        {
            selectCommand = null;
            DataTable dtEntityList = new DataTable();
            try
            {

                selectCommand = cn.CreateCommand();
                da = new SqlDataAdapter();
                selectCommand.CommandType = CommandType.StoredProcedure;
                selectCommand.CommandText = "SP_GetAllEntity";
                da.SelectCommand = selectCommand;
                da.Fill(dtEntityList);
                if (dtEntityList != null)
                {
                    ddlEntity.DataSource = dtEntityList;
                    ddlEntity.DataTextField = "EntityID";
                    ddlEntity.DataValueField = "EntityID";
                    ddlEntity.DataBind();
                    ddlEntity.Items.Insert(0, new ListItem("--Select--"));

                    ddlLocation.Items.Insert(0, new ListItem("--Select--"));
                }
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                lblMsg.Text = ex.Message;
            }
        }

        private void fnBindLocationDropDownList()
        {
            selectCommand = null;
            DataTable dtLocationList = new DataTable();
            try
            {
                selectCommand = cn.CreateCommand();
                da = new SqlDataAdapter();
                selectCommand.CommandType = CommandType.StoredProcedure;    //command thru stored proc
                selectCommand.CommandText = "SP_GetLocation_LoadByEntityID";      // stored proc name   
                selectCommand.Parameters.Add(new SqlParameter("@EntityID", ddlEntity.SelectedValue));
                da.SelectCommand = selectCommand;
                da.Fill(dtLocationList);
                if (dtLocationList != null)
                {
                    ddlLocation.Items.Clear();
                    ddlLocation.DataSource = dtLocationList;
                    ddlLocation.DataTextField = "LocatnName";
                    ddlLocation.DataValueField = "LocatnID";
                    ddlLocation.DataBind();
                    ddlLocation.Items.Insert(0, new ListItem("--Select--"));
                }
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                lblMsg.Text = ex.Message;
            }
        }

        private void fnBindDepartmentDropDownList()
        {
            DataTable dttemp = GetAllDept();
            if (dttemp != null)
            {
                ddlDept.DataSource = dttemp;  // Call GetAll Department Function
                ddlDept.DataTextField = "Name";
                ddlDept.DataValueField = "DeptID";
                ddlDept.DataBind();
                ddlDept.Items.Insert(0, new ListItem("-- Select --"));
            }
        }

        private void fnBindEmployeeDropDownList()
        {
            DataTable dttemp = GetAllEmployees();
            if (dttemp != null)
            {
                ddlEmp.DataSource = dttemp;  // Call GetAll Employee Function
                ddlEmp.DataTextField = "EmployeeName";
                ddlEmp.DataValueField = "EmployeeID";
                ddlEmp.DataBind();
                ddlEmp.Items.Insert(0, new ListItem("-- Select --"));
            }
        }
        
    }
}
