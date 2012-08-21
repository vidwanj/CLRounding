using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SharePoint.Utilities;

namespace CLRSurvey
{
    class CLRSurveyTimerJob: SPJobDefinition
    {
        public CLRSurveyTimerJob() : base()
        {
        }

        public CLRSurveyTimerJob(string jobName, SPService service, SPServer server, SPJobLockType targetType) : base(jobName, service, server, targetType)
        {
        }

        public CLRSurveyTimerJob(string jobName, SPWebApplication webApplication) : base(jobName, webApplication, null, SPJobLockType.ContentDatabase)
        {
            this.Title = "CLRSurvey Timer Job";
        }

        public override void Execute(Guid contentDbId)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPWebApplication webApplication = this.Parent as SPWebApplication;
                SPContentDatabase content = webApplication.ContentDatabases[contentDbId];

                //SPSite site = content.Sites["http://sonora8-lt/CLRSurvey"];
                //SPSite site = content.Sites["http://ftwssdevsql02:9000/CLRSurvey"];
                //SPSite site = content.Sites["http://team.txhealth.org/apps/clr"]; 

                using (SPSite site = content.Sites["http://team.txhealth.org/apps/clr"])
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList ListConfigItems = web.Lists["ConfigItems"];
                        SPListItemCollection itemsConStr = ListConfigItems.Items;
                        DataTable dtConStr = itemsConStr.GetDataTable();
                        DataRow[] drConStr = dtConStr.Select("Title='ConStr'");
                        string ConnectionString = drConStr[0]["KeyValue"].ToString();

                        DataRow[] drLastSentOn = dtConStr.Select("Title='TimerJobSettings'");
                        DateTime LastSentOn = Convert.ToDateTime(drLastSentOn[0]["LastSentOn"].ToString());

                        DataRow[] drEmailAddress = dtConStr.Select("Title='EMailAddress'");
                        string strEmailAddress = drEmailAddress[0]["EmailID"].ToString();
                        string strCCEmailAddress = drEmailAddress[0]["CCEMailID"].ToString();

                        string LastSentTime = LastSentOn.ToString("hh:mm tt");
                        string LastSentDate = LastSentOn.ToString("MM/dd/yyyy");

                        if (DateTime.Parse(LastSentDate) == DateTime.Now.Date)
                        {
                            if ((DateTime.Parse(DateTime.Now.ToString("hh:mm tt")) > DateTime.Parse("11:59 AM")) && (DateTime.Parse(LastSentTime) < DateTime.Parse("12:00 PM")))
                            {
                                //Send Mail at 12:00 PM
                                string DateForTitle = DateTime.Now.ToString("MM/dd/yyyy") + " 11:59 AM";
                                SendMail(ConnectionString, web, strEmailAddress, strCCEmailAddress, DateTime.Now, DateForTitle);
                                UpdateList(web);
                            }
                        }
                        else if (DateTime.Parse(LastSentDate) < DateTime.Now.Date)
                        {
                            if ((DateTime.Parse(DateTime.Now.ToString("hh:mm tt")) > DateTime.Parse("6:59 AM")) && (DateTime.Parse(LastSentTime) > DateTime.Parse("11:59 AM")))
                            {
                                //Send Mail at 07:00 AM
                                string DateForTitle = DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy") + " 11:59 PM";
                                SendMail(ConnectionString, web, strEmailAddress, strCCEmailAddress, DateTime.Now.AddDays(-1), DateForTitle);
                                UpdateList(web);
                            }
                        }
                    }
                }
            });
        }

        private void SendMail(string ConnectionString, SPWeb web, string EmailTo, string CCTo, DateTime DateForSP, string DateForTitle)
        {
            DataTable dtCLRStatusCountForEntity = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand selectCommand = null;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            selectCommand = cn.CreateCommand();
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            selectCommand.CommandType = CommandType.StoredProcedure;
            selectCommand.CommandText = "sp_GetCLRStatusCountForEntity2";
            selectCommand.Parameters.Add(new SqlParameter("@EntityID", "THD"));
            selectCommand.Parameters.Add(new SqlParameter("@TodaysDate", DateForSP.ToString("MM/dd/yyyy")));
            da.SelectCommand = selectCommand;
            da.Fill(dtCLRStatusCountForEntity);

            if (dtCLRStatusCountForEntity.Rows.Count > 0)
            {
                string strBody = ConvertDataTableToHtml(dtCLRStatusCountForEntity, DateForTitle);
                if (EmailTo != null && EmailTo.Trim().Length > 0)
                {
                    SPUtility.SendEmail(web, false, false, EmailTo, "CLR Status Of Entity THD For the Date :- " + DateForTitle + "", strBody, false);
                }
                if (CCTo != null && CCTo.Trim().Length > 0)
                {
                    SPUtility.SendEmail(web, false, false, CCTo, "CLR Status Of Entity THD For the Date :- " + DateForTitle + "", strBody, false);                
                }
            }

            if (cn.State == ConnectionState.Open)
            {
                cn.Close();   // close connection
            }
        }

        private string ConvertDataTableToHtml(DataTable targetTable, string DateForTitle)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("<table>\n<tr>\n<td>The CLR Details for the Date " + DateForTitle + " are as follows:-</td>\n</tr>\n</table></br>\n");

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

        private void UpdateList(SPWeb web)
        {
            SPList ListConfigItems = web.Lists["ConfigItems"];
            SPQuery query = new SPQuery();
            query.Query = String.Format("<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>TimerJobSettings</Value></Eq></Where>");
            SPListItemCollection listItems = ListConfigItems.GetItems(query);
            DataTable dtListItems = listItems.GetDataTable();      //List items into table                    

            if (dtListItems.Rows.Count > 0)
            {
                int intID = Convert.ToInt32(dtListItems.Rows[0]["ID"].ToString());
                // Update the List item by ID
                SPListItem itemToUpdate = ListConfigItems.GetItemById(intID);
                itemToUpdate["LastSentOn"] = DateTime.Now.ToString();
                itemToUpdate.Update();
            }
        }
    }
}
