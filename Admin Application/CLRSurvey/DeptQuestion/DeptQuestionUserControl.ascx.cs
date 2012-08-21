using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.SharePoint;

namespace CLRSurvey.DeptQuestion
{
    public partial class DeptQuestionUserControl : UserControl
    {
            DataTable dtDepartment = new DataTable();
            DataTable dtQuestion = new DataTable();
            SqlCommand selectCommand = null;
            SqlDataAdapter da = null;
            SqlConnection cn = new SqlConnection();
           
        protected void Page_Load(object sender, EventArgs e)
        {
            cn.ConnectionString = ConfigurationManager.AppSettings["clr_ConStr"];
            try
            {
                if (!IsPostBack)
                {
                    lstAvailable.Items.Clear();
                    lstMapped.Items.Clear();

                    fnBindEntityDropDownList();
                    fnBindDepartmentDropDownList();
                    fnBindQuestionDropDownList();
                }
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                lblMsg.Text = ex.Message;
            }

        }

        protected void ddlEntity_SelectedIndexChanged(object sender, EventArgs e)
        {
            fnBindLocationDropDownList();
            fnBindDepartmentDropDownList();
            ddlQuestion.SelectedIndex = 0;
            lstAvailable.Items.Clear();
            lstMapped.Items.Clear();
            lblMsg.Text = "";
        }

        protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            fnBindDepartmentDropDownList();
            ddlQuestion.SelectedIndex = 0;
            lstAvailable.Items.Clear();
            lstMapped.Items.Clear();
            lblMsg.Text = "";
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlDepartment.SelectedIndex != 0)
                {
                    lblMsg.Text = "";
                    DataTable dtAllQuestnTemp = fnAllQuestion();
                    DataTable dtMappedQuestnTemp = fnGetQuetionForDepartment();

                    if (dtAllQuestnTemp != null && dtMappedQuestnTemp != null)
                    {
                        //Remove Mapped Questions from List of All Questions
                        for (int i = 0; i < dtMappedQuestnTemp.Rows.Count; i++)
                        {
                            for (int j = 0; j < dtAllQuestnTemp.Rows.Count; j++)
                            {
                                if (dtAllQuestnTemp.Rows[j]["QuestnID"].ToString() == dtMappedQuestnTemp.Rows[i]["QuestnID"].ToString())
                                {
                                    dtAllQuestnTemp.Rows.RemoveAt(j);
                                    dtAllQuestnTemp.AcceptChanges();
                                }
                            }
                        }

                        dtAllQuestnTemp.DefaultView.Sort = "QuestnText";
                        dtMappedQuestnTemp.DefaultView.Sort = "QuestnText";
                    }

                    lblDeptQuestionMsg.Text = "Saving Questions For Selected Department";
                    lblAvailable.Text = "Available Questions";
                    lblMapped.Text = "Mapped Questions";
                    ddlQuestion.SelectedIndex = 0;

                    lstAvailable.DataSource = dtAllQuestnTemp;
                    lstAvailable.DataTextField = "QuestnText";
                    lstAvailable.DataValueField = "QuestnID";
                    lstAvailable.DataBind();

                    lstMapped.DataSource = dtMappedQuestnTemp;
                    lstMapped.DataTextField = "QuestnText";
                    lstMapped.DataValueField = "QuestnID";
                    lstMapped.DataBind();
                }
                else
                {
                    lstAvailable.Items.Clear();
                    lstMapped.Items.Clear();
                    lblDeptQuestionMsg.Text = "";
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

        protected void ddlQuestion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlQuestion.SelectedIndex != 0)
                {
                    lblMsg.Text = "";
                    DataTable dtAllDeptTemp = fnAllDept();
                    DataTable dtMappedDeptTemp = fnGetDepartmentForQuestion();

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

                    lblDeptQuestionMsg.Text = "Saving Department For Selected Question";
                    lblAvailable.Text = "Available Department";
                    lblMapped.Text = "Mapped Department";

                    ddlDepartment.SelectedIndex = 0;

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
                    lblDeptQuestionMsg.Text = "";
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
            try
            {                
                SPUser SpUserID = SPContext.Current.Web.CurrentUser;  // gets current user of site

                if (ddlDepartment.SelectedIndex != 0)
                {
                    string strDeptID = ddlDepartment.SelectedValue;

                    // First Delete The rows of selected Department            

                    selectCommand = cn.CreateCommand();
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                    }
                    selectCommand.Parameters.Add(new SqlParameter("@DeptID", strDeptID));
                    selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                    selectCommand.CommandText = "sp_QuestnDept_DeleteByDeptId";
                    selectCommand.ExecuteNonQuery();

                    for (int i = 0; i < lstMapped.Items.Count; i++)    // for list Question all items
                    {
                        string strQuestnID = lstMapped.Items[i].Value;
                        // Insert new row to the table 
                        selectCommand = cn.CreateCommand();
                        selectCommand.Parameters.Add(new SqlParameter("@DeptID", strDeptID));
                        selectCommand.Parameters.Add(new SqlParameter("@EffctvDate", DateTime.Now.ToShortDateString()));
                        selectCommand.Parameters.Add(new SqlParameter("@ExpirdDate", DateTime.Now.ToShortDateString()));
                        selectCommand.Parameters.Add(new SqlParameter("@QuestnID", strQuestnID));
                        selectCommand.Parameters.Add(new SqlParameter("@UdpateDate", DateTime.Now.ToShortDateString()));
                        selectCommand.Parameters.Add(new SqlParameter("@UpdateOpertr", SpUserID.Name));
                        selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                        selectCommand.CommandText = "sp_QuestnDept_Insert"; // stored proc name
                        selectCommand.ExecuteNonQuery();
                    }
                }
                else if (ddlQuestion.SelectedIndex != 0)
                {
                    string strQuestnID = ddlQuestion.SelectedValue;

                    // First Delete The rows of selected Department
                    selectCommand = cn.CreateCommand();
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                    }
                    selectCommand.Parameters.Add(new SqlParameter("@QuestnID", strQuestnID));
                    if (ddlEntity.SelectedIndex != 0)
                    {
                        selectCommand.Parameters.Add(new SqlParameter("@EntityID", ddlEntity.SelectedValue));
                    }
                    if (ddlLocation.SelectedIndex != 0)
                    {
                        selectCommand.Parameters.Add(new SqlParameter("@LocationID", ddlLocation.SelectedValue));
                    }
                    selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                    selectCommand.CommandText = "sp_QuestnDept_DeleteByQuestnID";
                    selectCommand.ExecuteNonQuery();

                    for (int i = 0; i < lstMapped.Items.Count; i++)    // for list Question all items
                    {
                        string strDeptID = lstMapped.Items[i].Value;
                        // Insert new row to the table 
                        selectCommand = cn.CreateCommand();
                        selectCommand.Parameters.Add(new SqlParameter("@DeptID", strDeptID));
                        selectCommand.Parameters.Add(new SqlParameter("@EffctvDate", DateTime.Now.ToShortDateString()));
                        selectCommand.Parameters.Add(new SqlParameter("@ExpirdDate", DateTime.Now.ToShortDateString()));
                        selectCommand.Parameters.Add(new SqlParameter("@QuestnID", strQuestnID));
                        selectCommand.Parameters.Add(new SqlParameter("@UdpateDate", DateTime.Now.ToShortDateString()));
                        selectCommand.Parameters.Add(new SqlParameter("@UpdateOpertr", SpUserID.Name));
                        selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                        selectCommand.CommandText = "sp_QuestnDept_Insert"; // stored proc name
                        selectCommand.ExecuteNonQuery();
                    }
                }

                lblMsg.Text = "Updated Successfully";
                lblMsg.Visible = true;
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                lblMsg.Text = strError;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
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

        private DataTable fnAllDept()
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
                    selectCommand.Parameters.Add(new SqlParameter("@EntityID",ddlEntity.SelectedValue));
                }
                if (ddlLocation.SelectedIndex != 0)
                {
                    selectCommand.Parameters.Add(new SqlParameter("@LocationID",ddlLocation.SelectedValue));
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

        private DataTable fnAllQuestion()
        {
            try
            {
                selectCommand = null;
                selectCommand = cn.CreateCommand();
                da = new SqlDataAdapter();
                selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                selectCommand.CommandText = "SP_GetAllQuestion_ByActive";
                da.SelectCommand = selectCommand;
                da.Fill(dtQuestion);
                return dtQuestion;
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

        private DataTable fnGetQuetionForDepartment()
        {
            try
            {
                selectCommand = null;
                selectCommand = cn.CreateCommand();
                DataTable dtQid = new DataTable();
                da = new SqlDataAdapter();
                selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                selectCommand.Parameters.Add(new SqlParameter("@Deptid", ddlDepartment.SelectedValue));
                selectCommand.CommandText = "sp_QuestnDept_GetQid";
                da.SelectCommand = selectCommand;
                da.Fill(dtQid);
                return dtQid;
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

        private DataTable fnGetDepartmentForQuestion()
        {
            try
            {
                selectCommand = null;
                selectCommand = cn.CreateCommand();
                DataTable dtDept = new DataTable();
                da = new SqlDataAdapter();
                selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                selectCommand.Parameters.Add(new SqlParameter("@QuestnID", ddlQuestion.SelectedValue));
                if (ddlEntity.SelectedIndex != 0)
                {
                    selectCommand.Parameters.Add(new SqlParameter("@EntityID", ddlEntity.SelectedValue));
                }
                if (ddlLocation.SelectedIndex != 0)
                {
                    selectCommand.Parameters.Add(new SqlParameter("@LocationID", ddlLocation.SelectedValue));
                }

                selectCommand.CommandText = "sp_QuestnDept_GetDeptID";
                da.SelectCommand = selectCommand;
                da.Fill(dtDept);
                return dtDept;
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
            DataTable dtDeptTemp = fnAllDept();
            if (dtDeptTemp != null)
            {
                ddlDepartment.Items.Clear();
                ddlDepartment.DataSource = dtDeptTemp;
                ddlDepartment.DataTextField = "Name";
                ddlDepartment.DataValueField = "DeptID";
                ddlDepartment.DataBind();
                ddlDepartment.Items.Insert(0, new ListItem("-- Select --"));
            }
        }

        private void fnBindQuestionDropDownList()
        {
            DataTable dtQuestnTemp = fnAllQuestion();
            if (dtQuestnTemp != null)
            {
                ddlQuestion.DataSource = dtQuestnTemp;
                ddlQuestion.DataTextField = "QuestnText";
                ddlQuestion.DataValueField = "QuestnID";
                ddlQuestion.DataBind();
                ddlQuestion.Items.Insert(0, new ListItem("-- Select --"));
            }
        }
    }
}
