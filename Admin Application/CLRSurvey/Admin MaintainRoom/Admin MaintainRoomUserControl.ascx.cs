using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Microsoft.SharePoint;

namespace CLRSurvey.Admin_MaintainRoom
{
    public partial class Admin_MaintainRoomUserControl : UserControl
    {
        SqlCommand selectCommand = null;
        SqlConnection cn = new SqlConnection();
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dtDept = new DataTable();
        DataTable dtRoom = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            txtRoomDescr.Attributes.Add("onKeyUp", "countCharacters()");
            cn.ConnectionString = ConfigurationManager.AppSettings["clr_ConStr"]; // get appSettings for current appln configuretion
            selectCommand = cn.CreateCommand();
            if (!Page.IsPostBack)
            {
                try
                {
                    selectCommand.CommandType = CommandType.StoredProcedure;    //command thru stored proc
                    selectCommand.CommandText = "sp_Dept_GetAll";      // stored proc name                      
                    da.SelectCommand = selectCommand;
                    da.Fill(dtDept);
                    ddlDept.DataSource = dtDept;
                    ddlDept.DataTextField = "Name";
                    ddlDept.DataValueField = "DeptID";
                    ddlDept.DataBind();
                    ddlDept.Items.Insert(0, new ListItem("--Select--"));
                    if (Request.QueryString["Rid"] != null)
                    {
                        ImgbtnClear.Visible = false;
                        int iRoomID = Convert.ToInt32(Request.QueryString["Rid"]);
                        selectCommand.Parameters.Add(new SqlParameter("@RoomID", iRoomID)); //Parameters for stored proc
                        selectCommand.CommandType = CommandType.StoredProcedure;    //command thru stored proc
                        selectCommand.CommandText = "sp_Room_GetByRoomID";      // stored proc name                      
                        da.SelectCommand = selectCommand;
                        da.Fill(dtRoom);
                        txtRoomDescr.Text = dtRoom.Rows[0]["RoomDescr"].ToString(); // gets contained data in dataTable row into textBox
                        txtComment.Text = dtRoom.Rows[0]["CommntText"].ToString();
                        ddlDept.SelectedValue = dtRoom.Rows[0]["DeptID"].ToString();
                        ddlActvnInactvn.SelectedValue = dtRoom.Rows[0]["ActvInactvInd"].ToString();
                        ddlRoomType.SelectedValue = dtRoom.Rows[0]["RoomTypeCode"].ToString();
                    }
                    else
                    {
                        if (Request.QueryString["DeptName"] != null)
                        {
                            ddlDept.SelectedIndex = ddlDept.Items.IndexOf(ddlDept.Items.FindByText(Request.QueryString["DeptName"].ToString()));
                        }
                        else if (Request.QueryString["DeptID"] != null)
                        {
                            ddlDept.SelectedIndex = ddlDept.Items.IndexOf(ddlDept.Items.FindByValue(Request.QueryString["DeptID"].ToString()));
                        }
                        ImgbtnClear.Visible = true;
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
        }

        protected void ImgbtnSave_Click(object sender, ImageClickEventArgs e)
        {
            try
            {                   
                string CurrentUser = SPContext.Current.Web.CurrentUser.Name;

                selectCommand = cn.CreateCommand();
                cn.Open();
                //command thru stored proc
                selectCommand.Parameters.Add(new SqlParameter("@CommntText", txtComment.Text)); // Comment Text param
                selectCommand.Parameters.Add(new SqlParameter("@DeptID", ddlDept.SelectedValue)); // DeptID From DropDown List
                selectCommand.Parameters.Add(new SqlParameter("@RoomDescr", txtRoomDescr.Text));
                selectCommand.Parameters.Add(new SqlParameter("@RoomTypeCode", ddlRoomType.SelectedValue));
                selectCommand.Parameters.Add(new SqlParameter("@ActvInactvInd", ddlActvnInactvn.SelectedValue));
                selectCommand.Parameters.Add(new SqlParameter("@UpdateDate", System.DateTime.Now.ToShortDateString()));
                selectCommand.Parameters.Add(new SqlParameter("@UpdateOpertr", CurrentUser));

                if (Request.QueryString["Rid"] != null)
                {
                    int iRID = Convert.ToInt32(Request.QueryString["Rid"]);
                    selectCommand.Parameters.Add(new SqlParameter("@RoomID", iRID));
                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandText = "sp_Room_Update";      // stored proc name
                    selectCommand.ExecuteNonQuery();
                    cn.Close();
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.FromName("#33CC33");
                    lblMsg.Text = "Updated Successfully";
                }
                else
                {
                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandText = "sp_Room_Insert";      // stored proc name
                    selectCommand.ExecuteNonQuery();
                    cn.Close();
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.FromName("#33CC33");
                    lblMsg.Text = "Added Successfully";

                }
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.FromName("#FF0000");
                lblMsg.Text = ex.Message;
            }

            Page.ClientScript.RegisterStartupScript(GetType(), "MyKey1", "<script language='javascript'  type='text/javascript'>window.frameElement.commitPopup();</script>");
        }

        protected void ImgbtnClear_Click(object sender, ImageClickEventArgs e)
        {
            txtRoomDescr.Text = "";
            txtComment.Text = "";
            ddlActvnInactvn.SelectedIndex = 0;
            ddlRoomType.SelectedIndex = 0;
            if (Request.QueryString["DeptName"] == null && Request.QueryString["DeptID"] == null)
            {
                ddlDept.SelectedIndex = 0;
            }
        }
    }
}
