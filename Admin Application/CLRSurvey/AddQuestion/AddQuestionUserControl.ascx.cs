using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Microsoft.SharePoint;

namespace CLRSurvey.AddQuestion
{
    public partial class AddQuestionUserControl : UserControl
    {
        SqlCommand selectCommand = null;
        SqlConnection cn = new SqlConnection();
        DataTable dtQuestList = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter();

        protected void Page_Load(object sender, EventArgs e)
        {
            txtQuestion.Attributes.Add("onKeyUp", "countCharacters()");
            cn.ConnectionString = ConfigurationManager.AppSettings["clr_ConStr"]; // get appSettings for current appln configuretion
            selectCommand = cn.CreateCommand();
            if (!Page.IsPostBack)
            {
                try
                {
                    if (Request.QueryString["qid"] != null)
                    {
                        ImgbtnClear.Visible = false;
                        // get QuestionId from QueryString   
                        int iQuestId = Convert.ToInt32(Request.QueryString["qid"]);
                        selectCommand.Parameters.Add(new SqlParameter("@Qid", iQuestId)); //Parameters for stored proc
                        selectCommand.CommandType = CommandType.StoredProcedure;    //command thru stored proc
                        selectCommand.CommandText = "SP_GetQuestion_LoadById";     // stored proc name                        
                        da.SelectCommand = selectCommand;
                        da.Fill(dtQuestList);                                       //fill dataTable thru DataAdapter
                        if (dtQuestList != null && dtQuestList.Rows.Count > 0)
                        {
                            txtQuestion.Text = dtQuestList.Rows[0]["QuestnText"].ToString(); // gets contained data in dataTable row into textBox
                            txtAbbreviation.Text = dtQuestList.Rows[0]["QuestnShortText"].ToString();
                            txtComment.Text = dtQuestList.Rows[0]["CommntText"].ToString();
                            ddlActvnInactvn.SelectedValue = dtQuestList.Rows[0]["ActvInactvInd"].ToString();
                            ddlResponseCode.SelectedValue = dtQuestList.Rows[0]["RespnsTypeCode"].ToString();
                            txtTicktCode.Text = dtQuestList.Rows[0]["TicketTypeCode"].ToString();                          
                        }
                    }
                    else
                    {
                        string strDdlValue = ddlActvnInactvn.SelectedValue;
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
                // Adding & passing values to parameters
                selectCommand.Parameters.Add(new SqlParameter("@Question", txtQuestion.Text.Trim()));
                selectCommand.Parameters.Add(new SqlParameter("@Abbreviation", txtAbbreviation.Text.Trim()));
                selectCommand.Parameters.Add(new SqlParameter("@Comment", txtComment.Text.Trim()));
                selectCommand.Parameters.Add(new SqlParameter("@ActvnInactvn", ddlActvnInactvn.SelectedValue));
                selectCommand.Parameters.Add(new SqlParameter("@ResponseCode", ddlResponseCode.SelectedValue));
                selectCommand.Parameters.Add(new SqlParameter("@TicktCode", txtTicktCode.Text.Trim()));
                selectCommand.Parameters.Add(new SqlParameter("@UpdateDate", DateTime.Now.ToShortDateString()));
                selectCommand.Parameters.Add(new SqlParameter("@UpdateOpertr", CurrentUser));

                if (Request.QueryString["qid"] != null)
                {
                    int iQid = Convert.ToInt32(Request.QueryString["qid"]);
                    // Update the current edited row
                    selectCommand.Parameters.Add(new SqlParameter("@QuestId", iQid));
                    selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                    selectCommand.CommandText = "sp_Questn_Update"; // stored proc name
                    selectCommand.ExecuteNonQuery();
                    lblMsg.Text = "Successfully Updated";
                    lblMsg.ForeColor = System.Drawing.Color.FromName("#33CC33");
                    lblMsg.Visible = true;
                }
                else
                {
                    // Insert new row to the Question table
                    selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc
                    selectCommand.CommandText = "SP_InsertQuestion"; // stored proc name
                    selectCommand.ExecuteNonQuery();
                    lblMsg.Text = "Added Successfully";
                    lblMsg.ForeColor = System.Drawing.Color.FromName("#33CC33");
                    lblMsg.Visible = true;
                }
                Page.ClientScript.RegisterStartupScript(GetType(), "MyScript", "ClosePopup();", true);
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
            txtQuestion.Text = "";
            txtAbbreviation.Text = "";
            txtComment.Text = "";
            txtTicktCode.Text = "";
        }

    }
}
