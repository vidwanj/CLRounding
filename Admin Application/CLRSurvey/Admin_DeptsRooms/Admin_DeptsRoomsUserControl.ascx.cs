using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Microsoft.SharePoint;

namespace CLRSurvey.Admin_DeptsRooms
{
    public partial class Admin_DeptsRoomsUserControl : UserControl
    {
        DataTable dtDepartment = new DataTable();
        DataTable dtRoom = new DataTable();
        SqlCommand selectCommand = null;
        SqlDataAdapter da = null;
        SqlConnection cn = new SqlConnection();  

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
                     fnBindRoomDropDownList();
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
            fnBindLocationDropDownList();
            fnBindDepartmentDropDownList();
            ddlRoom.SelectedIndex = 0;
            lstAvailable.Items.Clear();
            lstMapped.Items.Clear();
            lblMsg.Text = "";
        }

        protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            fnBindDepartmentDropDownList();
            ddlRoom.SelectedIndex = 0;
            lstAvailable.Items.Clear();
            lstMapped.Items.Clear();
            lblMsg.Text = "";
        }

        protected void ddlDeptSelectedChange(object sender, EventArgs e)
        {
            lstAvailable.SelectionMode = ListSelectionMode.Multiple;
            ImgbtnAddAllItem.Visible = true;
            ImgbtnRemoveAllItem.Visible = true;
            lblMsg.Visible = false;
            ddlRoom.SelectedIndex = 0;
            lblDeptRoomMsg.Text = "Saving Rooms For Selected Department";   // msg for saving rooms
            
            lblAvailable.Text = "Available Rooms";  // labels text changed
            lblMapped.Text = "Mapped Rooms";
            string strDptId = ddlDept.SelectedValue;   // dept item selected value
            DataTable dtRoomID = new DataTable();
            try
            {
                selectCommand = null;
                da = new SqlDataAdapter();
                selectCommand = cn.CreateCommand();
                selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                selectCommand.Parameters.Add(new SqlParameter("@DeptId", strDptId));
                selectCommand.CommandText = "sp_Room_GetByDeptId";   // gets room info by DeptId
                da.SelectCommand = selectCommand;
                da.Fill(dtRoomID); // fills DataTable dtRoomID for selected Dept

                lstMapped.DataSource = dtRoomID;    // DataSource to listBox Mapped Rooms
                lstMapped.DataTextField = "RoomDescr";
                lstMapped.DataValueField = "RoomID";
                lstMapped.DataBind();

                DataTable dtRoomNotInDept = new DataTable();
                selectCommand = null;                
                da = new SqlDataAdapter();
                selectCommand = cn.CreateCommand();
                selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                
                selectCommand.CommandText = "sp_Room_NotInDept";   // gets room info by DeptId
                da.SelectCommand = selectCommand;
                da.Fill(dtRoomNotInDept);  // fills DataTable for rooms not in selected Dept

                lstAvailable.DataSource = dtRoomNotInDept;  // dataSource for List Available Rooms
                lstAvailable.DataTextField = "RoomDescr";
                lstAvailable.DataValueField = "RoomID";
                lstAvailable.DataBind();
            }
            catch(Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                lblMsg.Text = ex.Message;
            }
             
        }

        protected void ddlRoomSelectedChange(object sender, EventArgs e)
        {
            ImgbtnAddAllItem.Visible = false;
            ImgbtnRemoveAllItem.Visible = false;
            lstAvailable.SelectionMode = ListSelectionMode.Single;
            lblMsg.Visible = false;
            ddlDept.SelectedIndex = 0;
            lblDeptRoomMsg.Text = "Saving Department For Selected Room";
            lblAvailable.Text = "Available Departments";
            lblMapped.Text = "Mapped Department";
            int iRoomid = 0;
            if(ddlRoom.SelectedIndex > 0)
            {
                iRoomid = Convert.ToInt32(ddlRoom.SelectedValue);
            }
            DataTable dtDeptInRoom = new DataTable();
            try
            {
                selectCommand = null;
                da = new SqlDataAdapter();
                selectCommand = cn.CreateCommand();
                selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                selectCommand.Parameters.Add(new SqlParameter("@RoomId", iRoomid));
                selectCommand.CommandText = "sp_Room_GetDept";   // gets room info by DeptId
                da.SelectCommand = selectCommand;
                da.Fill(dtDeptInRoom);  // fills DataTable Dept for selected Room

                selectCommand = null;
                da = new SqlDataAdapter();
                selectCommand = cn.CreateCommand();
                selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                selectCommand.CommandText = "sp_Dept_GetAll"; // stored proc name

                da.SelectCommand = selectCommand;
                da.Fill(dtDepartment); // fills all dept into DataTable
                
                string strDeptInRoom = (dtDeptInRoom.Rows[0]["DeptID"].ToString());
                if (strDeptInRoom != (DBNull.Value).ToString())
                {
                    DataRow[] Result = dtDepartment.Select("DeptID='" + strDeptInRoom + "'"); // Select row from all dept for selected DeptId
                    DataTable dtTempListItems = new DataTable();
                    dtTempListItems = Result.CopyToDataTable();     // copy to Temp DataTable
                    
                    lstMapped.DataSource = dtTempListItems;   // Selected row DataSource to Mapped List
                    lstMapped.DataTextField = "Name";
                    lstMapped.DataValueField = "DeptID";
                    lstMapped.DataBind();
                }
                else
                {
                    lstMapped.Items.Clear();
                }

                for (int i = 0; i < dtDepartment.Rows.Count; i++)
                {   // Delete the selected row from allDept DataTable 
                    if ((dtDepartment.Rows[i]["DeptID"].ToString()) == strDeptInRoom)
                    {
                        dtDepartment.Rows[i].Delete();
                    }
                }

                lstAvailable.DataSource = dtDepartment; // dept DataTable for Available List of Dept for Selected Room
                lstAvailable.DataTextField = "Name";
                lstAvailable.DataValueField = "DeptID";
                lstAvailable.DataBind();
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
            string strRoomId;
            string strDeptId;

            string CurrentUser = SPContext.Current.Web.CurrentUser.Name;

            if (ddlDept.SelectedIndex != 0)    // if Dept selected
            {
                if (lstMapped.Items.Count == 0)
                {
                    strDeptId = ddlDept.SelectedValue;
                    try
                    {
                        selectCommand = cn.CreateCommand();
                        cn.Open();
                        // Passing Params to SP update Room table
                        selectCommand.Parameters.Add(new SqlParameter("@DeptID", strDeptId));

                        selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                        selectCommand.CommandText = "sp_Room_Update_DeleteDeptID"; // stored proc name
                        selectCommand.ExecuteNonQuery();
                        cn.Close();


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
                else
                {
                    strDeptId = ddlDept.SelectedValue;
                    try
                    {
                        selectCommand = cn.CreateCommand();
                        cn.Open();
                        // Passing Params to SP update Room table
                        selectCommand.Parameters.Add(new SqlParameter("@DeptID", strDeptId));

                        selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                        selectCommand.CommandText = "sp_Room_Update_DeleteDeptID"; // stored proc name
                        selectCommand.ExecuteNonQuery();
                        cn.Close();
                    }
                    catch (Exception ex)
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = ex.Message;
                    }

                    for (int i = 0; i < lstMapped.Items.Count; i++)   // for the no. of items in Mapped Room list
                    {
                        try
                        {

                            selectCommand = cn.CreateCommand();
                            cn.Open();
                            strRoomId = lstMapped.Items[i].Value;
                            // Passing Params to SP update Room table
                            selectCommand.Parameters.Add(new SqlParameter("@DeptID", strDeptId));
                            selectCommand.Parameters.Add(new SqlParameter("@RoomID", strRoomId));
                            selectCommand.Parameters.Add(new SqlParameter("@UpdateOpertr", CurrentUser));
                            selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                            selectCommand.CommandText = "sp_Room_Update_DeptId"; // stored proc name
                            selectCommand.ExecuteNonQuery();
                            cn.Close();

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
                }
            }
            else if (ddlRoom.SelectedIndex != 0)   // if Room is selected
            {
                if (lstMapped.Items.Count > 1)     // if list mapped Dept is empty or greater than 1
                {
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                    lblMsg.Text = "Single Department Required";
                }
                else if (lstMapped.Items.Count == 0)
                {
                    strRoomId = ddlRoom.SelectedValue;
                    //strDeptId = lstMapped.Items[0].Value;
                    try
                    {
                        selectCommand = cn.CreateCommand();
                        cn.Open();
                        // Passing Params to SP update Room table
                        selectCommand.Parameters.Add(new SqlParameter("@DeptID", DBNull.Value));
                        selectCommand.Parameters.Add(new SqlParameter("@RoomID", strRoomId));
                        selectCommand.Parameters.Add(new SqlParameter("@UpdateOpertr", CurrentUser));
                        selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                        selectCommand.CommandText = "sp_Room_Update_DeptId"; // stored proc name
                        selectCommand.ExecuteNonQuery();
                        cn.Close();

                        lblMsg.Visible = true;
                        lblMsg.ForeColor = System.Drawing.Color.FromName("#33CC33");
                        lblMsg.Text = "Updated Successfully";
                        lstMapped.Items.Clear();
                    }

                    catch (Exception ex)
                    {
                        lblMsg.Visible = true;
                        lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                        lblMsg.Text = ex.Message;
                    }
                }
                else
                {  // if mapped list items are not empty or greater than 1
                    strRoomId = ddlRoom.SelectedValue;
                    strDeptId = lstMapped.Items[0].Value;
                    try
                    {
                        selectCommand = cn.CreateCommand();
                        cn.Open();
                        // Passing Params to SP update Room table
                        selectCommand.Parameters.Add(new SqlParameter("@DeptID", DBNull.Value));
                        selectCommand.Parameters.Add(new SqlParameter("@RoomID", strRoomId));
                        selectCommand.Parameters.Add(new SqlParameter("@UpdateOpertr", CurrentUser));
                        selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                        selectCommand.CommandText = "sp_Room_Update_DeptId"; // stored proc name
                        selectCommand.ExecuteNonQuery();
                        cn.Close();

                    }
                    catch (Exception ex)
                    {
                        lblMsg.Visible = true;
                        lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                        lblMsg.Text = ex.Message;
                    }

                    try
                    {
                        selectCommand = cn.CreateCommand();
                        cn.Open();
                        // Passing Params to SP update Room table
                        selectCommand.Parameters.Add(new SqlParameter("@DeptID", strDeptId));
                        selectCommand.Parameters.Add(new SqlParameter("@RoomID", strRoomId));
                        selectCommand.Parameters.Add(new SqlParameter("@UpdateOpertr", CurrentUser));
                        selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                        selectCommand.CommandText = "sp_Room_Update_DeptId"; // stored proc name
                        selectCommand.ExecuteNonQuery();
                        cn.Close();

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
            }
            else
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Items not selected";
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
                    if (ddlRoom.SelectedIndex != 0 && lstMapped.Items.Count != 0)
                    {
                        lstAvailable.Items.Add(lstMapped.Items[0]);
                        lstAvailable.ClearSelection();
                        lstMapped.Items.Clear();//clear the items  
                    }
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

        private DataTable fnAllRooms()
        {
            try
            {
                selectCommand = null;
                da = new SqlDataAdapter();
                selectCommand = cn.CreateCommand();
                selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                selectCommand.CommandText = "sp_Room_GetAll";
                da.SelectCommand = selectCommand;
                da.Fill(dtRoom);    // dataAdaptor fills all Room to DataTable dtRoom
                return dtRoom;
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
            DataTable dtTemp = fnAllDept();
            if (dtTemp != null)
            {
                try
                {
                    ddlDept.DataSource = dtTemp; 
                    ddlDept.DataTextField = "Name";
                    ddlDept.DataValueField = "DeptID";
                    ddlDept.DataBind();
                    ddlDept.Items.Insert(0, new ListItem("-- Select --"));
                }
                catch (Exception ex)
                {
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                    lblMsg.Text = ex.Message;
                }
            }
        }

        private void fnBindRoomDropDownList()
        {
            DataTable dtTemp = fnAllRooms();
            if (dtTemp != null)
            {
                try
                {
                    ddlRoom.DataSource = dtTemp;
                    ddlRoom.DataTextField = "RoomDescr";
                    ddlRoom.DataValueField = "RoomID";
                    ddlRoom.DataBind();
                    ddlRoom.Items.Insert(0, new ListItem("-- Select --"));
                }
                catch (Exception ex)
                {
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                    lblMsg.Text = ex.Message;
                }
            }
        }

    }
}
