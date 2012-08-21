using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace CLRSurvey
{
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1), WebService(Namespace = "http://tempuri.org/")]
    [System.Web.Script.Services.ScriptService]
    class ChartService : System.Web.Services.WebService
    {
        SqlCommand selectCommand = null;
        SqlConnection cn = new SqlConnection();

        [WebMethod]
        public string LoadChart(string entity, string frmDate, string toDate, string LocVal)
        {
            StringBuilder chartXaml = new StringBuilder();
            //Random objRandom = new Random();
            string strEntityName = "";
            int StatusCount;
            object temp;
            int variable;
            DataTable dtTemp = new DataTable();
            dtTemp = GetChartData(entity, frmDate, toDate, LocVal);

            // Append chart XAML data
            chartXaml.Append("<vc:Chart xmlns:vc=\"clr-namespace:Visifire.Charts;assembly=SLVisifire.Charts\" Width=\"700\" Height=\"500\" BorderThickness=\"0\" Theme=\"Theme1\" BorderBrush=\"Gray\" AnimatedUpdate=\"true\" CornerRadius=\"7\" ShadowEnabled=\"true\" Padding=\"4,4,4,10\" DataPointWidth='10' >");
            chartXaml.Append("<vc:Chart.AxesY>");
            //chartXaml.Append("<vc:Axis Prefix=\"$\"></vc:Axis>");
            chartXaml.Append("<vc:Axis Title=\"CLR Counts\" LineThickness=\"0\">");
            chartXaml.Append("<vc:Axis.Grids>");
            chartXaml.Append("<vc:ChartGrid Interval=\"0.2\" LineThickness=\"0\" InterlacedColor=\"#0fdddddd\">");
            chartXaml.Append("  </vc:ChartGrid>");
            chartXaml.Append("</vc:Axis.Grids>");
            chartXaml.Append("</vc:Axis>");

            chartXaml.Append("</vc:Chart.AxesY>");
            chartXaml.Append("<vc:Chart.Titles>");
            chartXaml.Append("<vc:Title Text=\"CLR Chart\" FontSize=\"14\"/>");
            chartXaml.Append("</vc:Chart.Titles>");
            chartXaml.Append("<vc:Chart.Legends>");
            chartXaml.Append("<vc:Legend BorderColor=\"#dbf2f2\" BorderThickness=\"0.5\" CornerRadius=\"2\">");
            chartXaml.Append("<vc:Legend.Background>");
            chartXaml.Append("<LinearGradientBrush EndPoint=\"1,1\" StartPoint=\"0,1\">");
            chartXaml.Append("<GradientStop Color=\"#f9f8f8\" Offset=\"0.1\"/>");
            chartXaml.Append("<GradientStop Color=\"#fcfefe\" Offset=\"0.4\"/>");
            chartXaml.Append("<GradientStop Color=\"#f1fafa\" Offset=\"1\"/>");
            chartXaml.Append("</LinearGradientBrush>");
            chartXaml.Append("</vc:Legend.Background>");
            chartXaml.Append(" </vc:Legend>");
            chartXaml.Append("</vc:Chart.Legends>");

            chartXaml.Append("<vc:Chart.Series>");

            //for incomplete status
            chartXaml.Append("<vc:DataSeries  LegendText=\"Incomplete\" RenderAs=\"StackedColumn\">");
            chartXaml.Append("<vc:DataSeries.DataPoints>");
            if (entity == "0" && LocVal == "0")
            {
                if (dtTemp.Rows.Count > 0)
                {
                    for (int i = 0; i < dtTemp.Rows.Count; i++)
                    {
                        if (DBNull.Value == (temp = dtTemp.Rows[i]["CLRStatusCode"]))
                        {
                            variable = 0;
                        }

                        else
                        {
                            variable = Convert.ToInt32(dtTemp.Rows[i]["CLRStatusCode"].ToString());
                        }

                        strEntityName = dtTemp.Rows[i]["EntityName"].ToString().Replace("&", "And").Replace("'", " ");
                        StatusCount = Convert.ToInt32(dtTemp.Rows[i]["CLRStatusWiseCount"].ToString());
                        if (variable == 10)
                        {
                            chartXaml.Append("<vc:DataPoint AxisXLabel='" + strEntityName + "' YValue='" + StatusCount + "'/> " + " \n");
                        }


                    }
                }
            }
            else if (entity != "0" && LocVal == "0")
            {
                if (dtTemp.Rows.Count > 0)
                {
                    for (int i = 0; i < dtTemp.Rows.Count; i++)
                    {
                        if (DBNull.Value == (temp = dtTemp.Rows[i]["CLRStatusCode"]))
                        {
                            variable = 0;
                        }

                        else
                        {
                            variable = Convert.ToInt32(dtTemp.Rows[i]["CLRStatusCode"].ToString());
                        }
                        strEntityName = dtTemp.Rows[i]["LocatnName"].ToString().Replace("&", "And").Replace("'", " ");
                        StatusCount = Convert.ToInt32(dtTemp.Rows[i]["CLRStatusWiseCount"].ToString());
                        if (variable == 10)
                        {
                            chartXaml.Append("<vc:DataPoint AxisXLabel='" + strEntityName + "' YValue='" + StatusCount + "'/> " + " \n");
                        }

                    }

                }
            }
            else if (LocVal != "0" && entity != "0")
            {
                if (dtTemp.Rows.Count > 0)
                {
                    for (int i = 0; i < dtTemp.Rows.Count; i++)
                    {
                        if (DBNull.Value == (temp = dtTemp.Rows[i]["CLRStatusCode"]))
                        {
                            variable = 0;
                        }

                        else
                        {
                            variable = Convert.ToInt32(dtTemp.Rows[i]["CLRStatusCode"].ToString());
                        }
                        //StatusCode = Convert.ToInt32(dtTemp.Rows[i]["CLRStatusCode"].ToString());
                        strEntityName = dtTemp.Rows[i]["DeptAbbrvtnName"].ToString().Replace("&", "And").Replace("'", " ");
                        StatusCount = Convert.ToInt32(dtTemp.Rows[i]["CLRStatusWiseCount"].ToString());
                        if (variable == 10)
                        {
                            chartXaml.Append("<vc:DataPoint AxisXLabel='" + strEntityName + "' YValue='" + StatusCount + "'/> " + " \n");
                        }

                    }
                }
            }
            chartXaml.Append("</vc:DataSeries.DataPoints>");
            chartXaml.Append("</vc:DataSeries>");

            // for Complete status
            chartXaml.Append("<vc:DataSeries  LegendText=\"Complete\" RenderAs=\"StackedColumn\">");
            chartXaml.Append("<vc:DataSeries.DataPoints>");
            if (entity == "0" && LocVal == "0")
            {
                if (dtTemp.Rows.Count > 0)
                {
                    for (int i = 0; i < dtTemp.Rows.Count; i++)
                    {
                        if (DBNull.Value == (temp = dtTemp.Rows[i]["CLRStatusCode"]))
                        {
                            variable = 0;
                        }

                        else
                        {
                            variable = Convert.ToInt32(dtTemp.Rows[i]["CLRStatusCode"].ToString());
                        }
                        // StatusCode = Convert.ToInt32(dtTemp.Rows[i]["CLRStatusCode"].ToString());
                        // strEntityName = dtTemp.Rows[i]["EntityName"].ToString().Replace("&", "And").Replace("'", " ");
                        StatusCount = Convert.ToInt32(dtTemp.Rows[i]["CLRStatusWiseCount"].ToString());
                        if (variable == 20)
                        {
                            chartXaml.Append("<vc:DataPoint YValue='" + StatusCount + "'/> " + " \n");
                        }

                    }
                }
            }
            else if (entity != "0" && LocVal == "0")
            {
                if (dtTemp.Rows.Count > 0)
                {
                    for (int i = 0; i < dtTemp.Rows.Count; i++)
                    {
                        if (DBNull.Value == (temp = dtTemp.Rows[i]["CLRStatusCode"]))
                        {
                            variable = 0;
                        }

                        else
                        {
                            variable = Convert.ToInt32(dtTemp.Rows[i]["CLRStatusCode"].ToString());
                        }
                        //StatusCode = Convert.ToInt32(dtTemp.Rows[i]["CLRStatusCode"].ToString());
                        // strEntityName = dtTemp.Rows[i]["LocatnName"].ToString().Replace("&", "And").Replace("'", " ");
                        StatusCount = Convert.ToInt32(dtTemp.Rows[i]["CLRStatusWiseCount"].ToString());
                        if (variable == 20)
                        {
                            chartXaml.Append("<vc:DataPoint YValue='" + StatusCount + "'/> " + " \n");
                        }

                    }

                }
            }
            else if (LocVal != "0" && entity != "0")
            {
                if (dtTemp.Rows.Count > 0)
                {
                    for (int i = 0; i < dtTemp.Rows.Count; i++)
                    {
                        if (DBNull.Value == (temp = dtTemp.Rows[i]["CLRStatusCode"]))
                        {
                            variable = 0;
                        }

                        else
                        {
                            variable = Convert.ToInt32(dtTemp.Rows[i]["CLRStatusCode"].ToString());
                        }
                        //StatusCode = Convert.ToInt32(dtTemp.Rows[i]["CLRStatusCode"].ToString());
                        // strEntityName = dtTemp.Rows[i]["DeptAbbrvtnName"].ToString().Replace("&", "And").Replace("'", " ");
                        StatusCount = Convert.ToInt32(dtTemp.Rows[i]["CLRStatusWiseCount"].ToString());
                        if (variable == 20)
                        {
                            chartXaml.Append("<vc:DataPoint YValue='" + StatusCount + "'/> " + " \n");
                        }

                    }
                }
            }
            chartXaml.Append("</vc:DataSeries.DataPoints>");
            chartXaml.Append("</vc:DataSeries>");

            //for Cancelled status
            chartXaml.Append("<vc:DataSeries  LegendText=\"Cancelled\" RenderAs=\"StackedColumn\">");
            chartXaml.Append("<vc:DataSeries.DataPoints>");
            if (entity == "0" && LocVal == "0")
            {
                if (dtTemp.Rows.Count > 0)
                {
                    for (int i = 0; i < dtTemp.Rows.Count; i++)
                    {
                        if (DBNull.Value == (temp = dtTemp.Rows[i]["CLRStatusCode"]))
                        {
                            variable = 0;
                        }

                        else
                        {
                            variable = Convert.ToInt32(dtTemp.Rows[i]["CLRStatusCode"].ToString());
                        }
                        // StatusCode = Convert.ToInt32(dtTemp.Rows[i]["CLRStatusCode"].ToString());
                        //strEntityName = dtTemp.Rows[i]["EntityName"].ToString().Replace("&", "And").Replace("'", " ");
                        StatusCount = Convert.ToInt32(dtTemp.Rows[i]["CLRStatusWiseCount"].ToString());
                        if (variable == 30)
                        {
                            chartXaml.Append("<vc:DataPoint YValue='" + StatusCount + "'/> " + " \n");
                        }

                    }
                }
            }
            else if (entity != "0" && LocVal == "0")
            {
                if (dtTemp.Rows.Count > 0)
                {
                    for (int i = 0; i < dtTemp.Rows.Count; i++)
                    {
                        if (DBNull.Value == (temp = dtTemp.Rows[i]["CLRStatusCode"]))
                        {
                            variable = 0;
                        }

                        else
                        {
                            variable = Convert.ToInt32(dtTemp.Rows[i]["CLRStatusCode"].ToString());
                        }
                        //StatusCode = Convert.ToInt32(dtTemp.Rows[i]["CLRStatusCode"].ToString());
                        //strEntityName = dtTemp.Rows[i]["LocatnName"].ToString().Replace("&", "And").Replace("'", " ");
                        StatusCount = Convert.ToInt32(dtTemp.Rows[i]["CLRStatusWiseCount"].ToString());
                        if (variable == 30)
                        {
                            chartXaml.Append("<vc:DataPoint YValue='" + StatusCount + "'/> " + " \n");
                        }

                    }

                }
            }
            else if (LocVal != "0" && entity != "0")
            {
                if (dtTemp.Rows.Count > 0)
                {
                    for (int i = 0; i < dtTemp.Rows.Count; i++)
                    {
                        if (DBNull.Value == (temp = dtTemp.Rows[i]["CLRStatusCode"]))
                        {
                            variable = 0;
                        }

                        else
                        {
                            variable = Convert.ToInt32(dtTemp.Rows[i]["CLRStatusCode"].ToString());
                        }
                        //StatusCode = Convert.ToInt32(dtTemp.Rows[i]["CLRStatusCode"].ToString());
                        // strEntityName = dtTemp.Rows[i]["DeptAbbrvtnName"].ToString().Replace("&", "And").Replace("'", " ");
                        StatusCount = Convert.ToInt32(dtTemp.Rows[i]["CLRStatusWiseCount"].ToString());
                        if (variable == 30)
                        {
                            chartXaml.Append("<vc:DataPoint YValue='" + StatusCount + "'/> " + " \n");
                        }

                    }
                }
            }
            chartXaml.Append("</vc:DataSeries.DataPoints>");
            chartXaml.Append("</vc:DataSeries>");

            //for Not Started status
            chartXaml.Append("<vc:DataSeries  LegendText=\"Not Started\" RenderAs=\"StackedColumn\">");
            chartXaml.Append("<vc:DataSeries.DataPoints>");
            if (entity == "0" && LocVal == "0")
            {
                if (dtTemp.Rows.Count > 0)
                {
                    for (int i = 0; i < dtTemp.Rows.Count; i++)
                    {
                        StatusCount = Convert.ToInt32(dtTemp.Rows[i]["CLRStatusWiseCount"].ToString());
                        if (DBNull.Value == (temp = dtTemp.Rows[i]["CLRStatusCode"]))
                        {
                            chartXaml.Append("<vc:DataPoint YValue='" + StatusCount + "'/> " + " \n");
                            variable = 0;
                        }

                    }
                }
            }
            else if (entity != "0" && LocVal == "0")
            {
                if (dtTemp.Rows.Count > 0)
                {
                    for (int i = 0; i < dtTemp.Rows.Count; i++)
                    {
                        StatusCount = Convert.ToInt32(dtTemp.Rows[i]["CLRStatusWiseCount"].ToString());
                        if (DBNull.Value == (temp = dtTemp.Rows[i]["CLRStatusCode"]))
                        {
                            chartXaml.Append("<vc:DataPoint YValue='" + StatusCount + "'/> " + " \n");
                            variable = 0;
                        }
                    }

                }
            }
            else if (LocVal != "0" && entity != "0")
            {
                if (dtTemp.Rows.Count > 0)
                {
                    for (int i = 0; i < dtTemp.Rows.Count; i++)
                    {
                        StatusCount = Convert.ToInt32(dtTemp.Rows[i]["CLRStatusWiseCount"].ToString());
                        if (DBNull.Value == (temp = dtTemp.Rows[i]["CLRStatusCode"]))
                        {
                            chartXaml.Append("<vc:DataPoint YValue='" + StatusCount + "'/> " + " \n");
                            variable = 0;
                        }
                    }
                }
            }
            chartXaml.Append("</vc:DataSeries.DataPoints>");
            chartXaml.Append("</vc:DataSeries>");

            chartXaml.Append("</vc:Chart.Series>");
            chartXaml.Append("</vc:Chart>");
            // Write object to an HTTP response stream
            return chartXaml.ToString();
        }

        protected DataTable GetChartData(string entity, string frmDate, string toDate, string LocVal)
        {
            try
            {
                cn.ConnectionString = ConfigurationManager.AppSettings["clr_ConStr"];

                selectCommand = cn.CreateCommand();
                selectCommand.CommandText = "SP_CLR_StatuswiseSummary"; // stored proc name
                cn.Open();
                if (entity != "0")
                {
                    selectCommand.Parameters.Add(new SqlParameter("@entityid", entity));
                }
                else
                {
                    selectCommand.Parameters.Add(new SqlParameter("@entityid", null));
                }
                if (LocVal != "0")
                {
                    selectCommand.Parameters.Add(new SqlParameter("@LocatnId", LocVal));
                }
                else
                {
                    selectCommand.Parameters.Add(new SqlParameter("@LocatnId", null));
                }
                DateTime dtTemp = Convert.ToDateTime(frmDate);
                selectCommand.Parameters.Add(new SqlParameter("@fromdate", Convert.ToDateTime(frmDate)));
                selectCommand.Parameters.Add(new SqlParameter("@todate", Convert.ToDateTime(toDate)));
                selectCommand.CommandType = CommandType.StoredProcedure; //command thru stored proc           
                selectCommand.ExecuteNonQuery();
                cn.Close();
                SqlDataAdapter sqlda = new SqlDataAdapter();
                sqlda.SelectCommand = selectCommand;
                DataTable dtStatusSummary = new DataTable();

                // sqlDataAdapter fills data from database into DataSet
                sqlda.Fill(dtStatusSummary);
                return dtStatusSummary;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        // Mubin's web service method
        [WebMethod]
        public string LoadQuestionResponceChart(string entityID, string locatnID, string deptID, string roomID, string responstype, string fromdate, string enddate)
        {
            //function to filter data and bind to visifire chart 
            SqlConnection sqlcon = new SqlConnection();
            sqlcon.ConnectionString = ConfigurationManager.AppSettings["clr_ConStr"];

            SqlCommand sqlcmd = new SqlCommand("SP_GetRespns_LoadByFilter", sqlcon);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            if (entityID != "0")
            {
                sqlcmd.Parameters.AddWithValue("@entityID", entityID);
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@entityID", null);
            }
            if (locatnID != "0")
            {
                sqlcmd.Parameters.AddWithValue("@locatnID", locatnID);
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@locatnID", null);
            }
            if (deptID != "0")
            {
                sqlcmd.Parameters.AddWithValue("@deptID", deptID);
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@deptID", null);
            }
            if (roomID != "0")
            {
                sqlcmd.Parameters.AddWithValue("@roomID", Convert.ToInt32(roomID));
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@roomID", null);
            }
            if (responstype != "0")
            {
                sqlcmd.Parameters.AddWithValue("@querespntype", responstype);
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@querespntype", null);
            }

            sqlcmd.Parameters.AddWithValue("@fromdate", DateTime.Parse(fromdate));

            sqlcmd.Parameters.AddWithValue("@todate", DateTime.Parse(enddate));


            SqlDataAdapter sqlda = new SqlDataAdapter();
            sqlda.SelectCommand = sqlcmd;
            DataTable sqldt = new DataTable();
            sqlda.Fill(sqldt);

            StringBuilder chartXml = new StringBuilder();
            // Append chart XML data            
            chartXml.Append("<vc:Chart xmlns:vc=\"clr-namespace:Visifire.Charts;assembly=SLVisifire.Charts\" Width=\"700\" Height=\"500\" Theme=\"Theme1\" DataPointWidth='10' >");

            chartXml.Append("<vc:Chart.AxesY>");
            chartXml.Append("<vc:Axis Title=\"Response Count\"></vc:Axis>");
            chartXml.Append("</vc:Chart.AxesY>");
            chartXml.Append("<vc:Chart.AxesX>");
            chartXml.Append("<vc:Axis Title=\"Question\"></vc:Axis>");
            chartXml.Append("</vc:Chart.AxesX>");

            chartXml.Append("<vc:Chart.Titles>");
            chartXml.Append("<vc:Title Text=\"Question Response Summary\" FontSize=\"14\"/>");
            chartXml.Append("</vc:Chart.Titles>");

            chartXml.Append("<vc:Chart.Series>");
            chartXml.Append("<vc:DataSeries RenderAs=\"Column\" >");
            chartXml.Append("<vc:DataSeries.DataPoints>");

            foreach (DataRow dataRow in sqldt.Rows)
                chartXml.Append("<vc:DataPoint AxisXLabel=\"" + dataRow["QuestnShortText"].ToString() + "\" YValue=\"" + dataRow["CountRespons"].ToString() + "\"/>");

            chartXml.Append("</vc:DataSeries.DataPoints>");
            chartXml.Append("</vc:DataSeries>");
            chartXml.Append("</vc:Chart.Series>");
            chartXml.Append("</vc:Chart>");
            return chartXml.ToString();
        }
        // Sujits webMethod
        [WebMethod]
        public string ClrResponseLoadChart(string FromDate, string ToDate)
        {
            DataTable dtStackedColumnData = new DataTable();
            StringBuilder chartXaml = new StringBuilder();
            Random objRandom = new Random();
            string strQuestnShortText = string.Empty;
            int IssueCount = 0;
            string strTestquestionDescription = string.Empty;
            string strTicketStatus = string.Empty;
            DataSet dsQuestionStatusChartData = new DataSet();
            DataSet dsQuestionRequestedData = new DataSet();
            DateTime ParamFromDate = DateTime.Parse(FromDate);
            DateTime ParamToDate = DateTime.Parse(ToDate);
            dsQuestionStatusChartData = GetChartDataForQuestionWiserReport(ParamFromDate, ParamToDate);
            // Append chart XAML data
            chartXaml.Append("<vc:Chart xmlns:vc=\"clr-namespace:Visifire.Charts;assembly=SLVisifire.Charts\" Width=\"700\" Height=\"500\" Theme=\"Theme1\" DataPointWidth='10' >");
            chartXaml.Append("<vc:Chart.AxesY>");
            //chartXaml.Append("<vc:Axis Prefix=\"$\"></vc:Axis>");
            chartXaml.Append("</vc:Chart.AxesY>");
            chartXaml.Append("<vc:Chart.Titles>");
            chartXaml.Append("<vc:Title Text=\"Followup Chart\" FontSize=\"14\"/>");
            chartXaml.Append("</vc:Chart.Titles>");
            chartXaml.Append("<vc:Chart.Series>");
            if (dsQuestionStatusChartData != null)
            {
                chartXaml.Append("<vc:DataSeries LegendText=\"InProcess\" ShowInLegend=\"true\" RenderAs=\"StackedColumn\" ShadowEnabled=\"true\" LabelEnabled=\"false\"> ");
                chartXaml.Append("<vc:DataSeries.DataPoints> ");
                if (dsQuestionStatusChartData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsQuestionStatusChartData.Tables[0].Rows.Count; i++)
                    {
                        strQuestnShortText = dsQuestionStatusChartData.Tables[0].Rows[i]["QuestnShortText"].ToString();

                        DataRow[] ResultData = dsQuestionStatusChartData.Tables[1].Select("TicketStatusCode='10' and QuestnShortText='" + strQuestnShortText + "'");
                        if (ResultData.Length > 0)
                        {
                            dtStackedColumnData = ResultData.CopyToDataTable();
                            strTicketStatus = dtStackedColumnData.Rows[0]["TicketStatusDescr"].ToString();
                            IssueCount = int.Parse(dtStackedColumnData.Rows[0]["issues"].ToString());
                        }
                        else
                        {
                            strTicketStatus = "";
                            IssueCount = 0;
                        }
                        chartXaml.Append("<vc:DataPoint AxisXLabel='" + strQuestnShortText + "' LabelText='" + strTicketStatus + "' YValue='" + IssueCount + "'/>");
                    }
                }
                chartXaml.Append("</vc:DataSeries.DataPoints> ");
                chartXaml.Append("</vc:DataSeries> ");
                chartXaml.Append("<vc:DataSeries LegendText=\"Completed\"  ShowInLegend=\"true\" RenderAs=\"StackedColumn\" ShadowEnabled=\"true\" LabelEnabled=\"false\"> ");
                chartXaml.Append("<vc:DataSeries.DataPoints> ");
                if (dsQuestionStatusChartData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsQuestionStatusChartData.Tables[0].Rows.Count; i++)
                    {
                        strQuestnShortText = dsQuestionStatusChartData.Tables[0].Rows[i]["QuestnShortText"].ToString();

                        DataRow[] ResultData = dsQuestionStatusChartData.Tables[1].Select("TicketStatusCode='20' and QuestnShortText='" + strQuestnShortText + "'");
                        if (ResultData.Length > 0)
                        {
                            dtStackedColumnData = ResultData.CopyToDataTable();
                            strTicketStatus = dtStackedColumnData.Rows[0]["TicketStatusDescr"].ToString();
                            IssueCount = int.Parse(dtStackedColumnData.Rows[0]["issues"].ToString());
                        }
                        else
                        {
                            strTicketStatus = "";
                            IssueCount = 0;
                        }
                        chartXaml.Append("<vc:DataPoint AxisXLabel='" + strQuestnShortText + "' LabelText='" + strTicketStatus + "' YValue='" + IssueCount + "'/>");
                    }
                }
                chartXaml.Append("</vc:DataSeries.DataPoints> ");
                chartXaml.Append("</vc:DataSeries> ");
                chartXaml.Append("</vc:Chart.Series>");
                chartXaml.Append("</vc:Chart>");
            }
            return chartXaml.ToString();
        }

        public DataSet GetChartDataForQuestionWiserReport(DateTime FromDate, DateTime ToDate)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.AppSettings["clr_ConStr"];
            SqlCommand cmd = new SqlCommand("sp_GetQuestionWiseIssueCount", con);
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromDate", FromDate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);
                SqlDataAdapter DataAdp = new SqlDataAdapter(cmd);
                DataSet dsChartData = new DataSet();
                DataAdp.Fill(dsChartData);
                return dsChartData;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                con.Close();
                cmd.Dispose();
            }
        }
    }
}