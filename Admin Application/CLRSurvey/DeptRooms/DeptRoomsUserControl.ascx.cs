using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.SharePoint;

namespace CLRSurvey.DeptRooms
{
    public partial class DeptRoomsUserControl : UserControl
    {
        DataSet dsDepartment = new DataSet();
        DataTable dtRoom = new DataTable();
        SqlCommand selectCommand = null;
        SqlDataAdapter da = new SqlDataAdapter();
        SqlConnection cn = new SqlConnection();  

        protected void Page_Load(object sender, EventArgs e)
        {
            cn.ConnectionString = ConfigurationManager.AppSettings["clr_ConStr"];
            try
            {
                // dataAdapter used to fill dataset dsDepartment

                if (!IsPostBack)
                {
                    selectCommand = cn.CreateCommand();
                    selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                    selectCommand.CommandText = "sp_Dept_GetAll"; // stored proc name

                    da.SelectCommand = selectCommand;
                    da.Fill(dsDepartment);        // fills dataset dsDepartment thru datatAdapter
                    // Bind the List Department
                    lstDept.DataSource = dsDepartment;
                    lstDept.DataTextField = "Name";
                    lstDept.DataValueField = "DeptID";
                    lstDept.DataBind();

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

        protected void lstDept_SelectedChange(object sender, EventArgs e)
        {
            try
            {
                selectCommand = cn.CreateCommand();
                selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                selectCommand.CommandText = "sp_Room_GetAll";
                da.SelectCommand = selectCommand;
                da.Fill(dtRoom);

                string strDptId = lstDept.SelectedValue;   // dept item selected value
                DataTable dtRoomID = new DataTable();
                selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                selectCommand.Parameters.Add(new SqlParameter("@DeptId", strDptId));
                selectCommand.CommandText = "sp_Room_GetByDeptId";   // gets room info by DeptId
                da.SelectCommand = selectCommand;
                da.Fill(dtRoomID);

                DataTable dtSelected = new DataTable();
                dtSelected.Columns.Add("RoomID", typeof(int));
                dtSelected.Columns.Add("RoomDescr", typeof(string));

                for (int i = 0; i < dtRoom.Rows.Count; i++)  // for list Employee all item 
                {
                    string strRoomId = (dtRoom.Rows[i]["RoomID"].ToString());
                    for (int j = 0; j < dtRoomID.Rows.Count; j++)  // for EmployeeId
                    {
                        string Rid = (dtRoomID.Rows[j]["RoomID"].ToString());   // get EmployeeID from DataTable Qid                   
                        if (strRoomId == Rid)      // If item value equals to EmployeeID
                        {
                            //lstEmp.Items[i].Selected = true;   // do item selected true
                            dtSelected.Rows.Add(Rid, dtRoom.Rows[i]["RoomDescr"]);
                        }
                    }
                }

                dtSelected.Merge(dtRoom);
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
                            if (dtSelected.Rows[k]["RoomID"].ToString() == dtSelected.Rows[cnt]["RoomID"].ToString())
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

                lstRoom.DataSource = dtSelected;
                lstRoom.DataTextField = "RoomDescr";
                lstRoom.DataValueField = "RoomID";
                lstRoom.DataBind();

                for (int i = 0; i < lstRoom.Items.Count; i++)  // for list Question all item 
                {
                    for (int j = 0; j < dtRoomID.Rows.Count; j++)  // for QuestionId
                    {
                        string strRid = (dtRoomID.Rows[j]["RoomID"].ToString());   // get QuestnId from DataTable Qid
                        if (lstRoom.Items[i].Value == strRid)      // If item value equals to QuestnId
                        {
                            lstRoom.Items[i].Selected = true;   // do item selected true

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
                lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                lblMsg.Visible = true;
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {            
            SPUser SpUserID = SPContext.Current.Web.CurrentUser;  // gets current user of site

            string strDeptID = lstDept.SelectedValue;

            for (int i = 0; i < lstRoom.Items.Count; i++)   // for the no. of items in room list
            {
                if (lstRoom.Items[i].Selected)    // if item is selected
                {
                    selectCommand = cn.CreateCommand();
                    cn.Open();
                    string strRoomID = lstRoom.Items[i].Value;
                    // Passing Params to SP update Room table
                    selectCommand.Parameters.Add(new SqlParameter("@DeptID", strDeptID));
                    selectCommand.Parameters.Add(new SqlParameter("@RoomID", strRoomID));
                    selectCommand.Parameters.Add(new SqlParameter("@UpdateOpertr", SpUserID.Name));
                    selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                    selectCommand.CommandText = "sp_Room_Update_DeptId"; // stored proc name
                    selectCommand.ExecuteNonQuery();
                    cn.Close();

                }
            }
            lblMsg.Text = "Added SUccessfully";
        }
    }
}
