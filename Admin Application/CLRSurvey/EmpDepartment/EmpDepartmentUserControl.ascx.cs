using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Microsoft.SharePoint;

namespace CLRSurvey.EmpDepartment
{
    public partial class EmpDepartmentUserControl : UserControl
    {
        DataTable dtDepartment = new DataTable();
       DataTable dtEmployee = new DataTable();        
        SqlCommand selectCommand = null;
        SqlDataAdapter da = new SqlDataAdapter();
        SqlConnection cn = new SqlConnection();  

        protected void Page_Load(object sender, EventArgs e)
        {
             cn.ConnectionString = ConfigurationManager.AppSettings["clr_ConStr"];
             try
             {
                 if (!IsPostBack)
                 {
                     selectCommand = cn.CreateCommand();
                     selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                     selectCommand.CommandText = "sp_Dept_GetAll"; // stored proc name

                     da.SelectCommand = selectCommand;
                     da.Fill(dtDepartment);        // fills dataset dsDepartment thru datatAdapter
                     lstDept.DataSource = dtDepartment;
                     lstDept.DataTextField = "Name";
                     lstDept.DataValueField = "DeptID";
                     lstDept.DataBind();
                 }
             }
             catch (Exception ex)
             {
                 string strError = ex.Message;
                 lblMsg.Text = strError;
             }            
        }

        protected void lstDept_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            lblMsg.Visible = false;
            selectCommand = cn.CreateCommand();
            selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
            selectCommand.CommandText = "sp_GetAllEmployees";
            da.SelectCommand = selectCommand;
            da.Fill(dtEmployee);
        

            // for selected true of question list
            DataTable dtEmpid = new DataTable();
            string strDptId = lstDept.SelectedValue;
            selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
            selectCommand.Parameters.Add(new SqlParameter("@Deptid", strDptId));
            selectCommand.CommandText = "sp_EmployeeDept_GetEmpId";
            da.SelectCommand = selectCommand;
            da.Fill(dtEmpid);
        
            DataTable dtSelected = new DataTable();
            dtSelected.Columns.Add("EmployeeID", typeof(string));
            dtSelected.Columns.Add("EmployeeName", typeof(string));


            for (int i = 0; i < dtEmployee.Rows.Count; i++)  // for list Employee all item 
            {
                string dtEmployeeId = (dtEmployee.Rows[i]["EmployeeID"].ToString());
                for (int j = 0; j < dtEmpid.Rows.Count; j++)  // for EmployeeId
                {
                    string Eid = (dtEmpid.Rows[j]["EmployeeID"].ToString());   // get EmployeeID from DataTable Qid                   
                    if (dtEmployeeId==Eid)      // If item value equals to EmployeeID
                    {
                        //lstEmp.Items[i].Selected = true;   // do item selected true
                        dtSelected.Rows.Add(Eid, dtEmployee.Rows[i]["EmployeeName"]);
                    }                    
                }
            }

            dtSelected.Merge(dtEmployee);
            dtSelected.GetChanges();

            for (int k = 0; k < dtSelected.Rows.Count; k++)
            {
                int cnt = k + 1;
                for (int c = 0; c < dtSelected.Rows.Count; )
                {
                    if (cnt == dtSelected.Rows.Count)
                        break;
                    else
                    {
                        if (dtSelected.Rows[k]["EmployeeID"].ToString() == dtSelected.Rows[cnt]["EmployeeID"].ToString())
                        {
                            dtSelected.Rows[cnt].Delete();
                            dtSelected.AcceptChanges();
                            //cnt--;
                        }
                        else
                        {
                            cnt++;
                        }
                    }
                }
                dtSelected.AcceptChanges();
            }

            lstEmp.DataSource = dtSelected;
            lstEmp.DataTextField = "EmployeeName";
            lstEmp.DataValueField = "EmployeeID";
            lstEmp.DataBind();

            for (int i = 0; i < lstEmp.Items.Count; i++)  // for list Question all item 
            {
                for (int j = 0; j < dtEmpid.Rows.Count; j++)  // for QuestionId
                {
                    string Qid = (dtEmpid.Rows[j]["EmployeeID"].ToString());   // get QuestnId from DataTable Qid
                    if (lstEmp.Items[i].Value == Qid)      // If item value equals to QuestnId
                    {
                        lstEmp.Items[i].Selected = true;   // do item selected true

                    }

                }
            }
        
        }        
         
  
        
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            string strDeptID = lstDept.SelectedValue;

            // First Delete The rows of selected Department         
            selectCommand = cn.CreateCommand();
            cn.Open();
            selectCommand.Parameters.Add(new SqlParameter("@DeptID", strDeptID));
            selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
            selectCommand.CommandText = "sp_EmployeeDept_Delete_ByDeptID";
            selectCommand.ExecuteNonQuery();
            cn.Close();

            for (int i = 0; i < lstEmp.Items.Count; i++)    // for list Employee all items
            {
                if (lstEmp.Items[i].Selected)      // if Employee List Item Selected
                {
                    string strEmpId = lstEmp.Items[i].Value;                    

                    // Insert new row to the table 
                    selectCommand = cn.CreateCommand();
                    cn.Open();
                    selectCommand.Parameters.Add(new SqlParameter("@DeptID", strDeptID));
                    selectCommand.Parameters.Add(new SqlParameter("@EmployeeID", strEmpId));
                    selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                    selectCommand.CommandText = "sp_EmployeeDept_Insert"; // stored proc name
                    selectCommand.ExecuteNonQuery();
                    cn.Close();

                }
            }
            lblMsg.Text = "Updated Successfully";
            lblMsg.Visible = true;
        }
    }
}
