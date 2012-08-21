using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Net;
using System.Net.Mail;
using System.Data;
using Microsoft.SharePoint;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Microsoft.SharePoint.Utilities;

namespace CLRSurvey.SendMail
{
    public partial class SendMailUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtbody.Text = ConvertDataTableToHtml(fngetdata());
            }
        }

        protected void btnSendMail_Click(object sender, EventArgs e)
        {
            //fngetdata();
            try
            {
                //MailMessage mail = new MailMessage();
                //mail.From = new MailAddress("ID");
                ////mail.To.Add("dhanunjay@hotmail.com");
                //mail.To.Add("ID");
                ////to send mail on textbox value use this code.

                //mail.IsBodyHtml = true;
                //mail.Subject = "CLR Status Of Entity PHD For the Date :-" + DateTime.Now.ToString("g");
                //mail.Body = ConvertDataTableToHtml(fngetdata());
                //SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                //smtp.Credentials = new System.Net.NetworkCredential("ID", "Pass"); smtp.EnableSsl = true;
                //ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; }; 
                //smtp.Send(mail);
                //lblMsg.Text = "Email Send Successfully.";                

                SPWeb web = SPContext.Current.Web;

                SPList lstConStr = web.Lists["ConfigItems"];
                SPListItemCollection itemsConStr = lstConStr.Items;
                DataTable dtConStr = itemsConStr.GetDataTable();

                DataRow[] drLastSentOn = dtConStr.Select("Title='TimerJobSettings'");
                DateTime LastSentOn = Convert.ToDateTime(drLastSentOn[0]["LastSentOn"].ToString());

                string LastSentTime = LastSentOn.ToString("hh:mm tt");
                string LastSentDate = LastSentOn.ToString("MM/dd/yyyy");

                string DateForTitle = DateTime.Now.ToString("MM/dd/yyyy") + "11:59 AM";
                DateTime dtxaaaa = DateTime.Parse(DateForTitle);

                if (DateTime.Parse(LastSentDate) == DateTime.Now.Date)
                {
                    if ((DateTime.Parse(DateTime.Now.ToString("hh:mm tt")) > DateTime.Parse("12:19 AM")) && (DateTime.Parse(LastSentTime) < DateTime.Parse("12:20 AM")))
                    {
                        //Send Mail at 12:00 PM
                        
                    }
                }
                else if (DateTime.Parse(LastSentDate) < DateTime.Now.Date)
                {
                    if ((DateTime.Parse(DateTime.Now.ToString("hh:mm tt")) > DateTime.Parse("12:09 AM")) && (DateTime.Parse(LastSentTime) > DateTime.Parse("12:19 AM")))
                    {
                        //Send Mail at 07:00 AM
                        DateTime dt1 = DateTime.Now.AddDays(-1);
                    }
                }


                //if (DateTime.Parse(LastSentDate) == DateTime.Now.Date)
                //{
                //    if ((DateTime.Parse(DateTime.Now.ToString("hh:mm tt")) > DateTime.Parse("11:59 AM")) && (DateTime.Parse(LastSentTime) < DateTime.Parse("12:00 PM")))
                //    {

                //    }
                //}
                //else if (DateTime.Parse(LastSentDate) < DateTime.Now.Date)
                //{
                //    if ((DateTime.Parse(DateTime.Now.ToString("hh:mm tt")) > DateTime.Parse("06:59 AM")) && (DateTime.Parse(LastSentTime) > DateTime.Parse("11:59 AM")))
                //    {

                //    }
                //}

                DateTime dt = DateTime.Now.AddDays(-1);

                string strBody = txtbody.Text;

                //SPUtility.SendEmail(web, false, false, "jayvidwans@texashealth.org", "CLR Status Of Entity PHD For the Date :- " + DateTime.Now.ToString("g") + "", strBody, false);
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        private DataTable fngetdata()
        {
            SqlConnection cn = new SqlConnection();
            DataTable dtCLRStatusCountForEntity = new DataTable();
            try
            {

                SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand selectCommand = null;


                SPWeb web = SPContext.Current.Web;
                SPList lstConStr = web.Lists["ConfigItems"];
                SPListItemCollection itemsConStr = lstConStr.Items;
                DataTable dtConStr = itemsConStr.GetDataTable();
                DataRow[] drConStr = dtConStr.Select("Title='ConStr'");
                string ConnectionString = drConStr[0]["KeyValue"].ToString();

                cn.ConnectionString = ConnectionString;    // connection string
                selectCommand = cn.CreateCommand();
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }
                selectCommand.CommandType = CommandType.StoredProcedure;
                selectCommand.CommandText = "sp_GetCLRStatusCountForEntity2";      // stored proc name
                selectCommand.Parameters.Add(new SqlParameter("@EntityID", "THD"));
                selectCommand.Parameters.Add(new SqlParameter("@TodaysDate", "8/1/2012"));
                da.SelectCommand = selectCommand;
                da.Fill(dtCLRStatusCountForEntity);
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();   // close connection
                }
            }
            return dtCLRStatusCountForEntity;
        }

        private string ConvertDataTableToHtml(DataTable targetTable)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("<table>\n<tr>\n<td>The CLR Details for the Date " + DateTime.Now.ToString("g") + " are as follows:-</td>\n</tr>\n</table></br>\n");

            sb.Append("<table style=\"border-collapse:collapse; text-align:center;\">\n");
            sb.Append("<tr align=\"center\">\n");
            //first append the column names.
            foreach (DataColumn column in targetTable.Columns)
            {
                sb.Append("<td style=\"background-color:darkblue;color:white;border-color:#5c87b2; border-style:solid; border-width:thin; padding: 5px;\"><B>");
                sb.Append(column.ColumnName.ToString());
                sb.Append("</B></td>\n");
            }

            sb.Append("</tr>\n");
            int Num = 1;
            // next, the column values.
            foreach (DataRow row in targetTable.Rows)
            {
                if (Num % 2 != 0)
                {
                    sb.Append("<tr align=\"center\">\n");
                }
                else
                {
                    sb.Append("<tr align=\"center\" style =\"background-color:#CCCCCC;\">\n");
                }


                foreach (DataColumn column in targetTable.Columns)
                {
                    sb.Append("<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding: 5px;\">\n");
                    if (row[column] != null)
                    {
                        if (row[column].ToString().Trim().Length > 0)
                        {
                            sb.Append(row[column].ToString());
                        }
                        else
                        {
                            sb.Append("0");
                        }
                    }
                    else
                    {
                        sb.Append("0");
                    }
                    sb.Append("</td>\n");
                }

                sb.Append("</tr>\n");
                Num += 1;
            }
            sb.Append("</table>");

            return sb.ToString();
        }
    }
}
