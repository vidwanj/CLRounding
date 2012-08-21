using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.DirectoryServices.AccountManagement;

namespace CLRRestService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CLRService" in code, svc and config file together.
    public class CLRService : ICLRService
    {
        string _ConnetctionString = ConfigurationManager.ConnectionStrings["CLRConStr"].ToString();

        string ICLRService.MassUpdateRoundingRoomStatus(CLRMassUpdateRoundingRoomEntity objCLRMassUpdateRoundingRoomEntity)
        {
         

            SqlConnection Conn = null;
            SqlTransaction myTran = null;
            try
            {
                Conn = new SqlConnection(_ConnetctionString);//Create Connection with db.
                Conn.Open();

                myTran = Conn.BeginTransaction();
                

                string strReturn = string.Empty;
                //all rooms
                string strMassUpdate = objCLRMassUpdateRoundingRoomEntity.MassUpdate;
                if (strMassUpdate.Length > 0)
                {
                    string[] SplitString = strMassUpdate.Split(';');
                    if (SplitString.Length > 0)
                    {
                        foreach (string strFields in SplitString)
                        {
                            //all fields
                            string[] strAllFields = strFields.Split(',');
                            if (strAllFields.Length == 3)
                            {

                                string  roomid = string.Empty;
                                string roundingdate = string.Empty;
                                string roomstatus = string.Empty;

                                CLRUpdateRoundingRoomEntity obj = new CLRUpdateRoundingRoomEntity();
                                obj.RoomID = strAllFields[0];
                                obj.CLRDateTime = strAllFields[1];
                                obj.RoomStatusCode = strAllFields[2];

                                strReturn = strReturn + LocalMarkStatusForRoom(obj, Conn, myTran);
                                strReturn = strReturn + ",";
                                    
                            }
                        }
                        strReturn = strReturn.Remove(strReturn.Length - 1);
                        myTran.Commit();
                        Conn.Close();
                    }

                }
                return strReturn;
            }
            catch (Exception ex)
            {
                try
                {
                    myTran.Rollback();
                }
                catch (Exception ex2)
                {
                    return ex2.Message; 
                }

                return ex.Message;               
            }
        }

        string ICLRService.InsertMarkStatusForRoom(CLRUpdateRoundingRoomEntity objCLRUpdateRoundingRoomEntity)
        {
            
            SqlConnection Conn = new SqlConnection(_ConnetctionString);//Create Connection with db.
            SqlCommand Cmd = new SqlCommand("sp_InsertMarkStatusForRoom", Conn);//Using SP from DB.
            int RowInserterd = 0;
            try
            {
                string UserName = ServiceSecurityContext.Current.PrimaryIdentity.Name;//Get Currtnt User Name.
                objCLRUpdateRoundingRoomEntity.UpdateOpertr = UserName;
                Conn.Open();
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.AddWithValue("@RoomID", Convert.ToInt32(objCLRUpdateRoundingRoomEntity.RoomID));
                Cmd.Parameters.AddWithValue("@Date", objCLRUpdateRoundingRoomEntity.CLRDateTime);
                Cmd.Parameters.AddWithValue("@EnteredByID", UserName);
                Cmd.Parameters.AddWithValue("@RoundingRoomStatus", objCLRUpdateRoundingRoomEntity.RoomStatusCode);
                Cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now.ToShortDateString());
                Cmd.Parameters.AddWithValue("@UpdateOpertr", objCLRUpdateRoundingRoomEntity.UpdateOpertr);
                //Passing parameters to sp.
                RowInserterd = Cmd.ExecuteNonQuery();// Get affected row index.  
                //Conn.Close();//Connection Closed.
                //Conn = null;
                //Cmd = null;
                return RowInserterd.ToString();//Returning Result.
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
 
           

        }

        string ICLRService.UpdateRoundingLogForRoom(CLRUpdateRoundingRoomEntity objCLRUpdateRoundingRoomEntity)
        {
            try
            {
                int RowInserterd = 0;
                string UserName = ServiceSecurityContext.Current.PrimaryIdentity.Name;//Get Currtnt User Name.
                objCLRUpdateRoundingRoomEntity.UpdateOpertr = UserName;
                SqlConnection Conn = new SqlConnection(_ConnetctionString);//Create Connection with db.
                Conn.Open();
                SqlCommand Cmd = new SqlCommand("sp_UpdateRoundingLogForRoom", Conn);//Using SP from DB.
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.AddWithValue("@EnteredByID", UserName);
                Cmd.Parameters.AddWithValue("@CLR_ID", Convert.ToInt32(objCLRUpdateRoundingRoomEntity.ClR_ID));
                Cmd.Parameters.AddWithValue("@CLRStatusCode", objCLRUpdateRoundingRoomEntity.CLRStatusCode);
                Cmd.Parameters.AddWithValue("@NurseNote", objCLRUpdateRoundingRoomEntity.NurseNote);                
                Cmd.Parameters.AddWithValue("@UpdateDate", Convert.ToDateTime(DateTime.Now.ToShortDateString()));
                Cmd.Parameters.AddWithValue("@UpdateOpertr", objCLRUpdateRoundingRoomEntity.UpdateOpertr);
                //Passing parameters to sp.


                RowInserterd = Cmd.ExecuteNonQuery();// Get affected row index.
                Conn.Close();//Connection Closed.
                return RowInserterd.ToString();//Returning Result.
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }     

        string ICLRService.UpdateQuestionResponseRoundingLogForRoom(CLRQuestionResponceEntities objCLRQuestionResponceEntities)
        {
            try
            {
                int RowInserterd = 0;
                string UserName = ServiceSecurityContext.Current.PrimaryIdentity.Name;//Get Currtnt User Name.
                objCLRQuestionResponceEntities.UpdateOpertr = UserName;
                SqlConnection Conn = new SqlConnection(_ConnetctionString);//Create Connection with db.
                Conn.Open();
                SqlCommand Cmd = new SqlCommand("sp_UpdateQuestionResponseRoundingLogForRoom", Conn);//Using SP from DB.
                Cmd.CommandType = CommandType.StoredProcedure;
                if (objCLRQuestionResponceEntities.QuestnRespnsID != "" && objCLRQuestionResponceEntities.QuestnRespnsID != "0")
                {
                    Cmd.Parameters.AddWithValue("@QuestnRespnsID", objCLRQuestionResponceEntities.QuestnRespnsID);
                }
                else
                {
                    Cmd.Parameters.AddWithValue("@QuestnRespnsID", 0);
                }
                Cmd.Parameters.AddWithValue("@QuestnID", objCLRQuestionResponceEntities.QuestnID);
                Cmd.Parameters.AddWithValue("@CLR_ID", Convert.ToInt32(objCLRQuestionResponceEntities.ClR_ID));
                Cmd.Parameters.AddWithValue("@RespnsText", objCLRQuestionResponceEntities.RespnsText);                
                Cmd.Parameters.AddWithValue("@CommntText", objCLRQuestionResponceEntities.CommntText);
                Cmd.Parameters.AddWithValue("@UpdateDate", Convert.ToDateTime(DateTime.Now.ToShortDateString()));
                Cmd.Parameters.AddWithValue("@UpdateOpertr", objCLRQuestionResponceEntities.UpdateOpertr);
                Cmd.Parameters.AddWithValue("@FollowupFlag", objCLRQuestionResponceEntities.FollowupFlag);
                //Passing parameters to sp.


                RowInserterd = Cmd.ExecuteNonQuery();// Get affected row index.
                Conn.Close();//Connection Closed.
                return RowInserterd.ToString();//Returning Result.
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        string ICLRService.UpdateFollowStatus(CLRUpdateFollowUp objCLRUpdateFollowUp)
        {
            try
            {
                int RowInserterd = 0;
                string UserName = ServiceSecurityContext.Current.PrimaryIdentity.Name;//Get Currtnt User Name.
                objCLRUpdateFollowUp.UpdateOpertr = UserName;
                SqlConnection Conn = new SqlConnection(_ConnetctionString);//Create Connection with db.
                Conn.Open();
                SqlCommand Cmd = new SqlCommand("sp_UpdateFollowupStatus", Conn);//Using SP from DB.
                Cmd.CommandType = CommandType.StoredProcedure;
                if (objCLRUpdateFollowUp.TicketID != "" && objCLRUpdateFollowUp.TicketID != "0")
                {
                    Cmd.Parameters.AddWithValue("@TicketID", objCLRUpdateFollowUp.TicketID);
                }
                else
                {
                    Cmd.Parameters.AddWithValue("@TicketID", 0);
                }                
                Cmd.Parameters.AddWithValue("@TicketTypeCode", objCLRUpdateFollowUp.TicketTypeCode);
                Cmd.Parameters.AddWithValue("@UdpateDate", Convert.ToDateTime(DateTime.Now.ToShortDateString()));
                Cmd.Parameters.AddWithValue("@UpdateOpertr", objCLRUpdateFollowUp.UpdateOpertr);
                //Passing parameters to sp.


                RowInserterd = Cmd.ExecuteNonQuery();// Get affected row index.
                Conn.Close();//Connection Closed.
                return RowInserterd.ToString();//Returning Result.
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        string ICLRService.UpdateFollowStatusV1(CLRUpdateFollowUpv1 objCLRUpdateFollowUpv1)
        {
            try
            {
                int RowInserterd = 0;
                string UserName = ServiceSecurityContext.Current.PrimaryIdentity.Name;//Get Currtnt User Name.
                objCLRUpdateFollowUpv1.UpdateOpertr = UserName;
                SqlConnection Conn = new SqlConnection(_ConnetctionString);//Create Connection with db.
                Conn.Open();
                SqlCommand Cmd = new SqlCommand("v1sp_UpdateFollowupStatus", Conn);//Using SP from DB.
                Cmd.CommandType = CommandType.StoredProcedure;
                if (objCLRUpdateFollowUpv1.TicketID != "")
                {
                    Cmd.Parameters.AddWithValue("@TicketID", objCLRUpdateFollowUpv1.TicketID);
                }
                else
                {
                    Cmd.Parameters.AddWithValue("@TicketID", 0);
                }

                Cmd.Parameters.AddWithValue("@ResltnText", objCLRUpdateFollowUpv1.ResltnText);
                
                Cmd.Parameters.AddWithValue("@TicketTypeCode", objCLRUpdateFollowUpv1.TicketTypeCode);
                Cmd.Parameters.AddWithValue("@UdpateDate", Convert.ToDateTime(DateTime.Now.ToShortDateString()));
                Cmd.Parameters.AddWithValue("@UpdateOpertr", objCLRUpdateFollowUpv1.UpdateOpertr);
                //Passing parameters to sp.


                RowInserterd = Cmd.ExecuteNonQuery();// Get affected row index.
                Conn.Close();//Connection Closed.
                return RowInserterd.ToString();//Returning Result.
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        string ICLRService.v1UpdateRoundingRoomStatus(version1CLRUpdateRoundingRoomEntity objversion1CLRUpdateRoundingRoomEntity)
        {
            //string format of persninroom is f,fr,p
            SqlConnection Conn = null;
            SqlTransaction myTran = null;
            try
            {
                Conn = new SqlConnection(_ConnetctionString);//Create Connection with db.
                Conn.Open();

                myTran = Conn.BeginTransaction();


                int RowInserterd = 0;
                string UserName = ServiceSecurityContext.Current.PrimaryIdentity.Name;//Get Currtnt User Name.
                //objversion1CLRUpdateRoundingRoomEntity.UpdateOpertr = UserName;               
                SqlCommand Cmd = new SqlCommand("sp_UpdateRoundingLogForRoom", Conn, myTran);//Using SP from DB.
                Cmd.CommandType = CommandType.StoredProcedure;
                //Passing parameters to sp.
                Cmd.Parameters.AddWithValue("@EnteredByID", UserName);
                Cmd.Parameters.AddWithValue("@CLR_ID", Convert.ToInt32(objversion1CLRUpdateRoundingRoomEntity.ClR_ID));
                Cmd.Parameters.AddWithValue("@CLRStatusCode", objversion1CLRUpdateRoundingRoomEntity.CLRStatusCode);
                Cmd.Parameters.AddWithValue("@NurseNote", objversion1CLRUpdateRoundingRoomEntity.NurseNote);
                Cmd.Parameters.AddWithValue("@UpdateDate", Convert.ToDateTime(DateTime.Now.ToShortDateString()));
                Cmd.Parameters.AddWithValue("@UpdateOpertr", UserName);
                LocalDeletFromPatientStateUsingClrID(Convert.ToInt32(objversion1CLRUpdateRoundingRoomEntity.ClR_ID), Conn, myTran);//Delete Existing record OF Clr_ID FROM PatientState.
                LocalDeleteFromPersonInPresenceUsingCLRID(Convert.ToInt32(objversion1CLRUpdateRoundingRoomEntity.ClR_ID), Conn, myTran);//Delete Existing record OF Clr_ID FROM PersnInPresnce.

                //To update PatientState.
                string strInputPatientStateString = string.Empty;
                strInputPatientStateString = objversion1CLRUpdateRoundingRoomEntity.PatientStateCode;
                if (strInputPatientStateString.Length > 0)
                {
                    string[] SplitPatientStatucCode = strInputPatientStateString.Split(',');
                    for (int i=0; i<SplitPatientStatucCode.Length;i++)
                    {
                        string strTempPatientStatusCode = string.Empty;
                        strTempPatientStatusCode = SplitPatientStatucCode[i];                       
                        if (strTempPatientStatusCode.Length > 0)
                        {
                            LocalInsertPatientStatus(objversion1CLRUpdateRoundingRoomEntity.ClR_ID, strTempPatientStatusCode, Conn, myTran);
                        }
                    }
                }
                //To uodate PatientState.
                string strInputPatientPresence = string.Empty;
                strInputPatientPresence = objversion1CLRUpdateRoundingRoomEntity.PersnInRoomCode;
                if (strInputPatientPresence.Length > 0)
                {
                    string[] SplitPersonPresenceString = strInputPatientPresence.Split(',');
                    for (int i = 0; i < SplitPersonPresenceString.Length; i++)
                    {
                        string strTempPersonPresenceCode=string.Empty;
                        strTempPersonPresenceCode=SplitPersonPresenceString[i];
                        if(strTempPersonPresenceCode.Length>0)
                        {
                            LocalInsertToPatientPresence(objversion1CLRUpdateRoundingRoomEntity.ClR_ID, strTempPersonPresenceCode, Conn, myTran);
                        }
                    }
                }

                RowInserterd = Cmd.ExecuteNonQuery();// Get affected row index.
                myTran.Commit();
                Conn.Close();//Connection Closed.
                return RowInserterd.ToString();//Returning Result.
            }
            catch (Exception ex)
            {
                try
                {
                    myTran.Rollback();
                }
                catch (Exception ex2)
                {
                    return ex2.Message;
                }

                return ex.Message;       
            }
        }

        string ICLRService.v1UpdateQuestionResponseRoundingLogForRoom(CLRQuestionResponceEntities objCLRQuestionResponceEntities)
        {
            try
            {
                int RowInserterd = 0;
                string UserName = ServiceSecurityContext.Current.PrimaryIdentity.Name;//Get Currtnt User Name.
                objCLRQuestionResponceEntities.UpdateOpertr = UserName;
                SqlConnection Conn = new SqlConnection(_ConnetctionString);//Create Connection with db.
                Conn.Open();
                SqlCommand Cmd = new SqlCommand("V1_sp_UpdateQuestionResponseRoundingLogForRoom", Conn);//Using SP from DB.
                Cmd.CommandType = CommandType.StoredProcedure;
                if (objCLRQuestionResponceEntities.QuestnRespnsID != "" && objCLRQuestionResponceEntities.QuestnRespnsID != "0")
                {
                    Cmd.Parameters.AddWithValue("@QuestnRespnsID", Int32.Parse(objCLRQuestionResponceEntities.QuestnRespnsID));
                }
                else
                {
                    Cmd.Parameters.AddWithValue("@QuestnRespnsID", 0);
                }
                Cmd.Parameters.AddWithValue("@QuestnID", Int32.Parse(objCLRQuestionResponceEntities.QuestnID));
                Cmd.Parameters.AddWithValue("@CLR_ID", Convert.ToInt32(objCLRQuestionResponceEntities.ClR_ID));
                Cmd.Parameters.AddWithValue("@RespnsText", objCLRQuestionResponceEntities.RespnsText);
                Cmd.Parameters.AddWithValue("@CommntText", objCLRQuestionResponceEntities.CommntText);
                Cmd.Parameters.AddWithValue("@UpdateDate", Convert.ToDateTime(DateTime.Now.ToShortDateString()));
                Cmd.Parameters.AddWithValue("@UpdateOpertr", objCLRQuestionResponceEntities.UpdateOpertr);
                Cmd.Parameters.AddWithValue("@FollowupFlag", Convert.ToInt32(objCLRQuestionResponceEntities.FollowupFlag));
                //Passing parameters to sp.


                RowInserterd = Cmd.ExecuteNonQuery();// Get affected row index.
                Conn.Close();//Connection Closed.
                return RowInserterd.ToString();//Returning Result.
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        string LocalMarkStatusForRoom(CLRUpdateRoundingRoomEntity objCLRUpdateRoundingRoomEntity, SqlConnection Conn, SqlTransaction myTran)
        {

            int RowInserterd = 0;

            SqlCommand Cmd = new SqlCommand("sp_InsertMarkStatusForRoom", Conn, myTran);//Using SP from DB.

            string UserName = ServiceSecurityContext.Current.PrimaryIdentity.Name;//Get Currtnt User Name.
            objCLRUpdateRoundingRoomEntity.UpdateOpertr = UserName;
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.Parameters.AddWithValue("@RoomID", Convert.ToInt32(objCLRUpdateRoundingRoomEntity.RoomID));
            Cmd.Parameters.AddWithValue("@Date", objCLRUpdateRoundingRoomEntity.CLRDateTime);
            Cmd.Parameters.AddWithValue("@EnteredByID", UserName);
            Cmd.Parameters.AddWithValue("@RoundingRoomStatus", objCLRUpdateRoundingRoomEntity.RoomStatusCode);
            Cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now.ToShortDateString());
            Cmd.Parameters.AddWithValue("@UpdateOpertr", objCLRUpdateRoundingRoomEntity.UpdateOpertr);
            //Passing parameters to sp.
            RowInserterd = Cmd.ExecuteNonQuery();// Get affected row index.  
            Conn.Close();//Connection Closed.
            //Conn = null;
            //Cmd = null;
            return RowInserterd.ToString();//Returning Result.



        }

        string LocalInsertToPatientPresence(string CLR_ID, string PersnInRoomCode, SqlConnection Conn, SqlTransaction myTran)
        {
            int RowInserterd = 0;
            int intTempCLR_ID = Int32.Parse(CLR_ID);
            SqlCommand Cmd = new SqlCommand("v1SpInsertToPatientPresence", Conn, myTran);//Using SP from DB.
            string UserName = ServiceSecurityContext.Current.PrimaryIdentity.Name;//Get Currtnt User Name.           
            Cmd.CommandType = CommandType.StoredProcedure;
            //Passing parameters to sp.
            Cmd.Parameters.AddWithValue("@ClR_ID", intTempCLR_ID);
            Cmd.Parameters.AddWithValue("@PersnInRoomCode", PersnInRoomCode);                       
            Cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now.ToShortDateString());
            Cmd.Parameters.AddWithValue("@UpdateOpertr", UserName);          
            RowInserterd = Cmd.ExecuteNonQuery();// Get affected row index.  
            //Conn.Close();//Connection Closed.
            //Conn = null;
            //Cmd = null;
            return RowInserterd.ToString();//Returning Result.
        }

        string LocalInsertPatientStatus(string CLR_ID, string PatientStatusCode, SqlConnection Conn, SqlTransaction myTran)
        {
            int RowInserterd = 0;

            SqlCommand Cmd = new SqlCommand("v1SpInsertToPatientState", Conn, myTran);//Using SP from DB.
            string UserName = ServiceSecurityContext.Current.PrimaryIdentity.Name;//Get Currtnt User Name.           
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.Parameters.AddWithValue("@ClR_ID", Convert.ToInt32(CLR_ID));
            Cmd.Parameters.AddWithValue("@PatientStateCode", PatientStatusCode);
            Cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now.ToShortDateString());
            Cmd.Parameters.AddWithValue("@UpdateOpertr", UserName);
            //Passing parameters to sp.
            RowInserterd = Cmd.ExecuteNonQuery();// Get affected row index.             
            //Conn = null;
            //Cmd = null;
            return RowInserterd.ToString();//Returning Result.
        }

        string LocalDeletFromPatientStateUsingClrID(int CLRID, SqlConnection Conn, SqlTransaction myTran)
        {
            try
            {
                int RowDeleted = 0;                             
                SqlCommand Cmd = new SqlCommand("V1_SP_DeleteFromPatientStateUsingCLR", Conn, myTran);//Using SP from DB.
                Cmd.CommandType = CommandType.StoredProcedure;
                //Passing parameters to sp.              
                Cmd.Parameters.AddWithValue("@CLR_ID",CLRID);
                RowDeleted = Cmd.ExecuteNonQuery();// Get affected row index.                
                return RowDeleted.ToString();//Returning Result.
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        string LocalDeleteFromPersonInPresenceUsingCLRID(int ClR_ID, SqlConnection Conn, SqlTransaction myTran)
        {
            try
            {
                int RowDeleted = 0;             
                SqlCommand Cmd = new SqlCommand("v1_SP_DeleteFromPersnInRoomUsingCLR_ID", Conn, myTran);//Using SP from DB.
                Cmd.CommandType = CommandType.StoredProcedure;
                //Passing parameters to sp.              
                Cmd.Parameters.AddWithValue("@CLR_ID", ClR_ID);
                RowDeleted = Cmd.ExecuteNonQuery();// Get affected row index.              
                return RowDeleted.ToString();//Returning Result.
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}



       //string ICLRService.InsertQuestionResponce(clrEntities objclrEntities)
       // {
       //     try
       //     {                
       //         int RowInserterd = 0;                
       //         string UserName = ServiceSecurityContext.Current.PrimaryIdentity.Name;//Get Currtnt User Name.
                
       //         SqlConnection Conn = new SqlConnection(_ConnetctionString);//Create Connection with db.
       //         Conn.Open();
       //         SqlCommand Cmd = new SqlCommand("sp_QuestnRespns_Insert", Conn);//Using SP from DB.
       //         Cmd.CommandType = CommandType.StoredProcedure;
       //         //Passing parameters to sp.
       //         Cmd.Parameters.AddWithValue("@ClR_ID", Convert.ToInt32(objclrEntities.ClR_ID));
       //         Cmd.Parameters.AddWithValue("@CommntText", objclrEntities.CommntText);
       //         Cmd.Parameters.AddWithValue("@EnteredByID", objclrEntities.EnteredByID);
       //         Cmd.Parameters.AddWithValue("@QuestnStatusCode", objclrEntities.QuestnStatusCode);
       //         Cmd.Parameters.AddWithValue("@RespnsDateTime", Convert.ToDateTime(objclrEntities.RespnsDateTime));
       //         Cmd.Parameters.AddWithValue("@RespnsText", objclrEntities.RespnsText);
       //         Cmd.Parameters.AddWithValue("@UpdateDate", objclrEntities.UpdateDate);
       //         Cmd.Parameters.AddWithValue("@UpdateOpertr", objclrEntities.UpdateOpertr);
       //         Cmd.Parameters.AddWithValue("@QuestnID", Convert.ToInt32(objclrEntities.QuestnID));
       //         RowInserterd = Cmd.ExecuteNonQuery();// Get affected row index.
       //        // Conn.Close();//Connection Closed.
       //         return RowInserterd.ToString();//Returning Result.
       //     }
       //     catch (Exception ex)
       //     {
       //         return ex.Message;
       //     }
       // }

       // string ICLRService.UpdateQuestionResponce(clrEntities objclrEntities)
       // {
       //     try
       //     {
       //         int Inserterd = 0;
       //         SqlConnection Conn = new SqlConnection(_ConnetctionString);//Create connection with db.
       //         Conn.Open();
       //         SqlCommand Cmd = new SqlCommand("sp_QuestnRespns_Update", Conn);//Call SP from db.
       //         Cmd.CommandType = CommandType.StoredProcedure;
       //         //Passing Parameters to the SP.
       //         Cmd.Parameters.AddWithValue("@QuestnRespnsID", Convert.ToInt32(objclrEntities.QuestnRespnsID));
       //         Cmd.Parameters.AddWithValue("@ClR_ID", Convert.ToInt32(objclrEntities.ClR_ID));
       //         Cmd.Parameters.AddWithValue("@CommntText", objclrEntities.CommntText);
       //         Cmd.Parameters.AddWithValue("@EnteredByID", objclrEntities.EnteredByID);
       //         Cmd.Parameters.AddWithValue("@QuestnStatusCode", objclrEntities.QuestnStatusCode);
       //         Cmd.Parameters.AddWithValue("@RespnsDateTime", Convert.ToDateTime(objclrEntities.RespnsDateTime));
       //         Cmd.Parameters.AddWithValue("@RespnsText", objclrEntities.RespnsText);
       //         Cmd.Parameters.AddWithValue("@UpdateDate", objclrEntities.UpdateDate);
       //         Cmd.Parameters.AddWithValue("@UpdateOpertr", objclrEntities.UpdateOpertr);
       //         Cmd.Parameters.AddWithValue("@QuestnID", Convert.ToInt32(objclrEntities.QuestnID));
       //         Inserterd = Cmd.ExecuteNonQuery();//Get Result count.
       //        // Conn.Close();//Connection closed.
       //         return Inserterd.ToString();//Return result.
       //     }
       //     catch (Exception ex)
       //     {
       //         return ex.Message;
       //     }
       // }

       // string ICLRService.InsertCLR(CLRtblEntities objCLRtblEntities)
       // {
       //     try
       //     {
       //         int Inserterd = 0;
       //         SqlConnection Conn = new SqlConnection(_ConnetctionString);//Create connection with db.
       //         Conn.Open();
       //         SqlCommand Cmd = new SqlCommand("sp_CLR_Insert", Conn);//Call SP from db.
       //         Cmd.CommandType = CommandType.StoredProcedure;
       //         //Passing Parameters to the SP.
       //         Cmd.Parameters.AddWithValue("@CLRDateTime", Convert.ToDateTime(objCLRtblEntities.CLRDateTime));
       //         Cmd.Parameters.AddWithValue("@CLRStatusCode", objCLRtblEntities.CLRStatusCode);
       //         Cmd.Parameters.AddWithValue("@CLRTypeID", objCLRtblEntities.CLRTypeID);
       //         Cmd.Parameters.AddWithValue("@CommntText", objCLRtblEntities.CommntText);
       //         Cmd.Parameters.AddWithValue("@EnteredByID", objCLRtblEntities.EnteredByID);
       //         Cmd.Parameters.AddWithValue("@MRN_EncntrDescr", objCLRtblEntities.MRN_EncntrDescr);
       //         Cmd.Parameters.AddWithValue("@RoomID", objCLRtblEntities.RoomID);
       //         Cmd.Parameters.AddWithValue("@UpdateDate", objCLRtblEntities.UpdateDate);
       //         Cmd.Parameters.AddWithValue("@UpdateOpertr", objCLRtblEntities.UpdateOpertr);
       //         Inserterd = Cmd.ExecuteNonQuery();//Get Result count.
       //        // Conn.Close();//Connection closed.
       //         return Inserterd.ToString();//Return result.
       //     }
       //     catch (Exception ex)
       //     {
       //         return ex.Message;
       //     }
       // }

       // string ICLRService.UpdateCLR(CLRtblEntities objCLRtblEntities) 
       // {
       //     try
       //     {
       //         int Inserterd = 0;
       //         SqlConnection Conn = new SqlConnection(_ConnetctionString);//Create connection with db.
       //         Conn.Open();
       //         SqlCommand Cmd = new SqlCommand("sp_CLR_Update", Conn);//Call SP from db.
       //         Cmd.CommandType = CommandType.StoredProcedure;
       //         //Passing Parameters to the SP.
       //         Cmd.Parameters.AddWithValue("@ClR_ID", Convert.ToInt32(objCLRtblEntities.ClR_ID));
       //         Cmd.Parameters.AddWithValue("@CLRDateTime", Convert.ToDateTime(objCLRtblEntities.CLRDateTime));
       //         Cmd.Parameters.AddWithValue("@CLRStatusCode", objCLRtblEntities.CLRStatusCode);
       //         Cmd.Parameters.AddWithValue("@CLRTypeID", objCLRtblEntities.CLRTypeID);
       //         Cmd.Parameters.AddWithValue("@CommntText", objCLRtblEntities.CommntText);
       //         Cmd.Parameters.AddWithValue("@EnteredByID", objCLRtblEntities.EnteredByID);
       //         Cmd.Parameters.AddWithValue("@MRN_EncntrDescr", objCLRtblEntities.MRN_EncntrDescr);
       //         Cmd.Parameters.AddWithValue("@RoomID", objCLRtblEntities.RoomID);
       //         Cmd.Parameters.AddWithValue("@UpdateDate", objCLRtblEntities.UpdateDate);
       //         Cmd.Parameters.AddWithValue("@UpdateOpertr", objCLRtblEntities.UpdateOpertr);
       //         Inserterd = Cmd.ExecuteNonQuery();//Get Result count.
       //        // Conn.Close();//Connection closed.
       //         return Inserterd.ToString();//Return result.
       //     }
       //     catch (Exception ex)
       //     {
       //         return ex.Message;
       //     }
       // }

       // string ICLRService.InsertCLRType(CLRTypetblEntities objCLRTypetblEntities)
       // {
       //     try
       //     {
       //         int RowInserterd = 0;
       //         string UserName = ServiceSecurityContext.Current.PrimaryIdentity.Name;//Get Currtnt User Name.

       //         SqlConnection Conn = new SqlConnection(_ConnetctionString);//Create Connection with db.
       //         Conn.Open();
       //         SqlCommand Cmd = new SqlCommand("sp_CLRType_Insert", Conn);//Using SP from DB.
       //         Cmd.CommandType = CommandType.StoredProcedure;
       //         //Passing parameters to sp.
       //         Cmd.Parameters.AddWithValue("@CLRTypeAbbrvtnName", objCLRTypetblEntities.CLRTypeAbbrvtnName);
       //         Cmd.Parameters.AddWithValue("@CLRTypeName", objCLRTypetblEntities.CLRTypeName);
       //         Cmd.Parameters.AddWithValue("@CommntText", objCLRTypetblEntities.CommntText);
       //         Cmd.Parameters.AddWithValue("@ActvInactvInd", objCLRTypetblEntities.ActvInactvInd);
       //         Cmd.Parameters.AddWithValue("@DefltSortOrdr", Convert.ToInt32(objCLRTypetblEntities.DefltSortOrdr));
       //         Cmd.Parameters.AddWithValue("@UpdateDate", Convert.ToDateTime(objCLRTypetblEntities.UpdateDate));
       //         Cmd.Parameters.AddWithValue("@UpdateOpertr", objCLRTypetblEntities.UpdateOpertr);

       //         RowInserterd = Cmd.ExecuteNonQuery();// Get affected row index.
       //        // Conn.Close();//Connection Closed.
       //         return RowInserterd.ToString();//Returning Result.
       //     }
       //     catch (Exception ex)
       //     {
       //         return ex.Message;
       //     }
       // }      

       // string ICLRService.InsertCLRTypeQuestion(CLRTypeQuestionEntities objCLRTypeQuestionEntities)
       // {
       //     try
       //     {
       //         int RowInserterd = 0;
       //         string UserName = ServiceSecurityContext.Current.PrimaryIdentity.Name;//Get Currtnt User Name.

       //         SqlConnection Conn = new SqlConnection(_ConnetctionString);//Create Connection with db.
       //         Conn.Open();
       //         SqlCommand Cmd = new SqlCommand("sp_CLRTypeQuestn_Insert", Conn);//Using SP from DB.
       //         Cmd.Parameters.AddWithValue("@CLRTypeID", objCLRTypeQuestionEntities.CLRTypeID);
       //         Cmd.Parameters.AddWithValue("@QuestnID", objCLRTypeQuestionEntities.QuestnID);
       //         Cmd.Parameters.AddWithValue("@UpdateDate", Convert.ToDateTime(objCLRTypeQuestionEntities.UpdateDate));
       //         Cmd.Parameters.AddWithValue("@UpdateOpertr", objCLRTypeQuestionEntities.UpdateOpertr);
       //         Cmd.CommandType = CommandType.StoredProcedure;
       //         //Passing parameters to sp.
                

       //         RowInserterd = Cmd.ExecuteNonQuery();// Get affected row index.
       //        // Conn.Close();//Connection Closed.
       //         return RowInserterd.ToString();//Returning Result.
       //     }
       //     catch (Exception ex)
       //     {
       //         return ex.Message;
       //     }
       // }

       // string ICLRService.InsertDept(CLRDepttblEntities objDepttblEntities)
       // {
       //     try
       //     {
       //         int RowInserterd = 0;
       //         string UserName = ServiceSecurityContext.Current.PrimaryIdentity.Name;//Get Currtnt User Name.

       //         SqlConnection Conn = new SqlConnection(_ConnetctionString);//Create Connection with db.
       //         Conn.Open();
       //         SqlCommand Cmd = new SqlCommand("sp_Dept_Insert", Conn);//Using SP from DB.
       //         Cmd.CommandType = CommandType.StoredProcedure;
       //         //Passing parameters to sp.
       //         Cmd.Parameters.AddWithValue("@DeptID", Convert.ToInt32(objDepttblEntities.DeptID));
       //         Cmd.Parameters.AddWithValue("@DeptAbbrvtnName", objDepttblEntities.DeptAbbrvtnName);
       //         Cmd.Parameters.AddWithValue("@LocatnID", objDepttblEntities.LocatnID);
       //         Cmd.Parameters.AddWithValue("@SvcLineID", Convert.ToInt32(objDepttblEntities.SvcLineID));
       //         Cmd.Parameters.AddWithValue("@ActvInactvInd", objDepttblEntities.ActvInactvInd);
       //         Cmd.Parameters.AddWithValue("@DefltSortOrdr", Convert.ToInt32(objDepttblEntities.DefltSortOrdr));
       //         Cmd.Parameters.AddWithValue("@UpdateDate", Convert.ToDateTime(objDepttblEntities.UpdateDate));
       //         Cmd.Parameters.AddWithValue("@UpdateOpertr", objDepttblEntities.UpdateOpertr);

       //         RowInserterd = Cmd.ExecuteNonQuery();// Get affected row index.
       //        // Conn.Close();//Connection Closed.
       //         return RowInserterd.ToString();//Returning Result.
       //     }
       //     catch (Exception ex)
       //     {
       //         return ex.Message;
       //     }
       // }

       // string ICLRService.InsertDeptContact(CLRDeptContactEntities objCLRDeptContactEntities)
       // {
       //     try
       //     {
       //         int RowInserterd = 0;
       //         string UserName = ServiceSecurityContext.Current.PrimaryIdentity.Name;//Get Currtnt User Name.

       //         SqlConnection Conn = new SqlConnection(_ConnetctionString);//Create Connection with db.
       //         Conn.Open();
       //         SqlCommand Cmd = new SqlCommand("sp_DeptContact_Insert", Conn);//Using SP from DB.
       //         Cmd.CommandType = CommandType.StoredProcedure;
       //         Cmd.Parameters.AddWithValue("@DeptContactID", Convert.ToInt32(objCLRDeptContactEntities.DeptContactID));
       //         Cmd.Parameters.AddWithValue("@DeptContactTypeCode", objCLRDeptContactEntities.DeptContactTypeCode);
       //         Cmd.Parameters.AddWithValue("@DeptID", Convert.ToInt32(objCLRDeptContactEntities.DeptID));
       //         Cmd.Parameters.AddWithValue("@EmpleID", objCLRDeptContactEntities.EmpleID);
       //         Cmd.Parameters.AddWithValue("@UpdateDate", Convert.ToDateTime(objCLRDeptContactEntities.UpdateDate));
       //         Cmd.Parameters.AddWithValue("@UpdateOpertr", objCLRDeptContactEntities.UpdateOpertr);
       //         //Passing parameters to sp.
              

       //         RowInserterd = Cmd.ExecuteNonQuery();// Get affected row index.
       //        // Conn.Close();//Connection Closed.
       //         return RowInserterd.ToString();//Returning Result.
       //     }
       //     catch (Exception ex)
       //     {
       //         return ex.Message;
       //     }
       // }

       // string ICLRService.InsertCLREntity(CLRtblEntitieValues objCLRtblEntitieValues)
       // {
       //     try
       //     {
       //         int RowInserterd = 0;
       //         string UserName = ServiceSecurityContext.Current.PrimaryIdentity.Name;//Get Currtnt User Name.

       //         SqlConnection Conn = new SqlConnection(_ConnetctionString);//Create Connection with db.
       //         Conn.Open();
       //         SqlCommand Cmd = new SqlCommand("sp_Entity_Insert", Conn);//Using SP from DB.
       //         Cmd.CommandType = CommandType.StoredProcedure;

       //         Cmd.Parameters.AddWithValue("@EntityID", objCLRtblEntitieValues.EntityID);
       //         Cmd.Parameters.AddWithValue("@EntityAbbrvtnName", objCLRtblEntitieValues.EntityAbbrvtnName);
       //         Cmd.Parameters.AddWithValue("@EntityName", objCLRtblEntitieValues.EntityName);
       //         Cmd.Parameters.AddWithValue("@CLRContactID", objCLRtblEntitieValues.CLRContactID);
       //         Cmd.Parameters.AddWithValue("@UpdateDate", Convert.ToDateTime(objCLRtblEntitieValues.UpdateDate));
       //         Cmd.Parameters.AddWithValue("@UpdateOpertr", objCLRtblEntitieValues.UpdateOpertr);
       //         //Passing parameters to sp.


       //         RowInserterd = Cmd.ExecuteNonQuery();// Get affected row index.
       //        // Conn.Close();//Connection Closed.
       //         return RowInserterd.ToString();//Returning Result.
       //     }
       //     catch (Exception ex)
       //     {
       //         return ex.Message;
       //     }
       // }

       // string ICLRService.InsertLocation(CLRtblLocatnEntities objCLRtblLocatnEntities)
       // {
       //     try
       //     {
       //         int RowInserterd = 0;
       //         string UserName = ServiceSecurityContext.Current.PrimaryIdentity.Name;//Get Currtnt User Name.

       //         SqlConnection Conn = new SqlConnection(_ConnetctionString);//Create Connection with db.
       //         Conn.Open();
       //         SqlCommand Cmd = new SqlCommand("sp_Locatn_Insert", Conn);//Using SP from DB.
       //         Cmd.CommandType = CommandType.StoredProcedure;
       //         Cmd.Parameters.AddWithValue("@LocatnID", objCLRtblLocatnEntities.LocatnID);
       //         Cmd.Parameters.AddWithValue("@EntityID", objCLRtblLocatnEntities.EntityID);
       //         Cmd.Parameters.AddWithValue("@LocatnAbbrvtnName", objCLRtblLocatnEntities.LocatnAbbrvtnName);
       //         Cmd.Parameters.AddWithValue("@LocatnName", objCLRtblLocatnEntities.LocatnName);
       //         Cmd.Parameters.AddWithValue("@ActvInactvInd", objCLRtblLocatnEntities.ActvInactvInd);
       //         Cmd.Parameters.AddWithValue("@DefltSortOrdr", Convert.ToInt32(objCLRtblLocatnEntities.DefltSortOrdr));
       //         Cmd.Parameters.AddWithValue("@UpdateDate", Convert.ToDateTime(objCLRtblLocatnEntities.UpdateDate));
       //         Cmd.Parameters.AddWithValue("@UpdateOpertr", objCLRtblLocatnEntities.UpdateOpertr);
       //         //Passing parameters to sp.


       //         RowInserterd = Cmd.ExecuteNonQuery();// Get affected row index.
       //        // Conn.Close();//Connection Closed.
       //         return RowInserterd.ToString();//Returning Result.
       //     }
       //     catch (Exception ex)
       //     {
       //         return ex.Message;
       //     }
       // }

       // string ICLRService.InsertCLRQuestions(CLRtblQuestnEntities objCLRtblQuestnEntities)
       // {
       //     try
       //     {
       //         int RowInserterd = 0;
       //         string UserName = ServiceSecurityContext.Current.PrimaryIdentity.Name;//Get Currtnt User Name.

       //         SqlConnection Conn = new SqlConnection(_ConnetctionString);//Create Connection with db.
       //         Conn.Open();
       //         SqlCommand Cmd = new SqlCommand("sp_Questn_Insert", Conn);//Using SP from DB.
       //         Cmd.CommandType = CommandType.StoredProcedure;
       //         Cmd.Parameters.AddWithValue("@CommntText", objCLRtblQuestnEntities.CommntText);
       //         Cmd.Parameters.AddWithValue("@QuestnText", objCLRtblQuestnEntities.QuestnText);
       //         Cmd.Parameters.AddWithValue("@QuestnShortText", objCLRtblQuestnEntities.QuestnShortText);
       //         Cmd.Parameters.AddWithValue("@TicketTypeCode", objCLRtblQuestnEntities.TicketTypeCode);
       //         Cmd.Parameters.AddWithValue("@RespnsTypeCode", objCLRtblQuestnEntities.RespnsTypeCode);
       //         Cmd.Parameters.AddWithValue("@ActvInactvInd", objCLRtblQuestnEntities.ActvInactvInd);
       //         Cmd.Parameters.AddWithValue("@UpdateDate", Convert.ToDateTime(objCLRtblQuestnEntities.UpdateDate));
       //         Cmd.Parameters.AddWithValue("@UpdateOpertr", objCLRtblQuestnEntities.UpdateOpertr);
       //         //Passing parameters to sp.


       //         RowInserterd = Cmd.ExecuteNonQuery();// Get affected row index.
       //        // Conn.Close();//Connection Closed.
       //         return RowInserterd.ToString();//Returning Result.
       //     }
       //     catch (Exception ex)
       //     {
       //         return ex.Message;
       //     }
       // }

       // string ICLRService.InsertRoom(CLRtblRoomEntities objCLRtblRoomEntities)
       // {
       //     try
       //     {
       //         int RowInserterd = 0;
       //         string UserName = ServiceSecurityContext.Current.PrimaryIdentity.Name;//Get Currtnt User Name.

       //         SqlConnection Conn = new SqlConnection(_ConnetctionString);//Create Connection with db.
       //         Conn.Open();
       //         SqlCommand Cmd = new SqlCommand("sp_Room_Insert", Conn);//Using SP from DB.
       //         Cmd.CommandType = CommandType.StoredProcedure;
       //         Cmd.Parameters.AddWithValue("@CommntText", objCLRtblRoomEntities.CommntText);
       //         Cmd.Parameters.AddWithValue("@DeptID", objCLRtblRoomEntities.DeptID);
       //         Cmd.Parameters.AddWithValue("@RoomDescr", objCLRtblRoomEntities.RoomDescr);
       //         Cmd.Parameters.AddWithValue("@RoomTypeCode", objCLRtblRoomEntities.RoomTypeCode);
       //         Cmd.Parameters.AddWithValue("@ActvInactvInd", objCLRtblRoomEntities.ActvInactvInd);
       //         Cmd.Parameters.AddWithValue("@UpdateDate", Convert.ToDateTime(objCLRtblRoomEntities.UpdateDate));
       //         Cmd.Parameters.AddWithValue("@UpdateOpertr", objCLRtblRoomEntities.UpdateOpertr);
       //         //Passing parameters to sp.


       //         RowInserterd = Cmd.ExecuteNonQuery();// Get affected row index.
       //        // Conn.Close();//Connection Closed.
       //         return RowInserterd.ToString();//Returning Result.
       //     }
       //     catch (Exception ex)
       //     {
       //         return ex.Message;
       //     }
       // }

       // string ICLRService.Insertsvcline(CLRtblSvcLineEntities objCLRtblSvcLineEntities)
       // {
       //     try
       //     {
       //         int RowInserterd = 0;
       //         string UserName = ServiceSecurityContext.Current.PrimaryIdentity.Name;//Get Currtnt User Name.

       //         SqlConnection Conn = new SqlConnection(_ConnetctionString);//Create Connection with db.
       //         Conn.Open();
       //         SqlCommand Cmd = new SqlCommand("sp_SvcLine_Insert", Conn);//Using SP from DB.
       //         Cmd.CommandType = CommandType.StoredProcedure;
       //         Cmd.Parameters.AddWithValue("@SvcLineAbbrvtnName", objCLRtblSvcLineEntities.SvcLineAbbrvtnName);
       //         Cmd.Parameters.AddWithValue("@SvcLineName", objCLRtblSvcLineEntities.SvcLineName);
       //         Cmd.Parameters.AddWithValue("@ActvInactvInd", objCLRtblSvcLineEntities.ActvInactvInd);
       //         Cmd.Parameters.AddWithValue("@UpdateDate", Convert.ToDateTime(objCLRtblSvcLineEntities.UpdateDate));
       //         Cmd.Parameters.AddWithValue("@UpdateOpertr", objCLRtblSvcLineEntities.UpdateOpertr);
       //         //Passing parameters to sp.


       //         RowInserterd = Cmd.ExecuteNonQuery();// Get affected row index.
       //        // Conn.Close();//Connection Closed.
       //         return RowInserterd.ToString();//Returning Result.
       //     }
       //     catch (Exception ex)
       //     {
       //         return ex.Message;
       //     }
       // }

       // string ICLRService.InsertTicket(CLRtblTicketEntities objCLRtblTicketEntities)
       // {
       //     try
       //     {
       //         int RowInserterd = 0;
       //         string UserName = ServiceSecurityContext.Current.PrimaryIdentity.Name;//Get Currtnt User Name.

       //         SqlConnection Conn = new SqlConnection(_ConnetctionString);//Create Connection with db.
       //         Conn.Open();
       //         SqlCommand Cmd = new SqlCommand("sp_Ticket_Insert", Conn);//Using SP from DB.
       //         Cmd.CommandType = CommandType.StoredProcedure;
       //         Cmd.Parameters.AddWithValue("@CommntText", objCLRtblTicketEntities.CommntText);
       //         Cmd.Parameters.AddWithValue("@DeptID", objCLRtblTicketEntities.DeptID);
       //         Cmd.Parameters.AddWithValue("@EnteredByID", objCLRtblTicketEntities.EnteredByID);
       //         Cmd.Parameters.AddWithValue("@IssueText", objCLRtblTicketEntities.IssueText);
       //         Cmd.Parameters.AddWithValue("@ResltnText", objCLRtblTicketEntities.ResltnText);
       //         Cmd.Parameters.AddWithValue("@ReslvdByID", objCLRtblTicketEntities.ReslvdByID);
       //         Cmd.Parameters.AddWithValue("@QuestnRespnsID", objCLRtblTicketEntities.QuestnRespnsID);
       //         Cmd.Parameters.AddWithValue("@TicketDateTime", Convert.ToDateTime(objCLRtblTicketEntities.TicketDateTime));
       //         Cmd.Parameters.AddWithValue("@TicketPriorityCode", objCLRtblTicketEntities.TicketPriorityCode);
       //         Cmd.Parameters.AddWithValue("@TicketStatusCode", objCLRtblTicketEntities.TicketStatusCode);
       //         Cmd.Parameters.AddWithValue("@TicketTypeCode", objCLRtblTicketEntities.TicketTypeCode);                
       //         Cmd.Parameters.AddWithValue("@UpdateDate", Convert.ToDateTime(objCLRtblTicketEntities.UdpateDate));
       //         Cmd.Parameters.AddWithValue("@UpdateOpertr", objCLRtblTicketEntities.UpdateOpertr);
       //         //Passing parameters to sp.


       //         RowInserterd = Cmd.ExecuteNonQuery();// Get affected row index.
       //         Conn.Close();//Connection Closed.
       //         return RowInserterd.ToString();//Returning Result.
       //     }
       //     catch (Exception ex)
       //     {
       //         return ex.Message;
       //     }
       // }

       // string ICLRService.InsertTicketAction(CLRtblTicketActnEntities objCLRtblTicketActnEntities)
       // {
       //     try
       //     {
       //         int RowInserterd = 0;
       //         string UserName = ServiceSecurityContext.Current.PrimaryIdentity.Name;//Get Currtnt User Name.

       //         SqlConnection Conn = new SqlConnection(_ConnetctionString);//Create Connection with db.
       //         Conn.Open();
       //         SqlCommand Cmd = new SqlCommand("sp_TicketActn_Insert", Conn);//Using SP from DB.
       //         Cmd.CommandType = CommandType.StoredProcedure;
       //         Cmd.Parameters.AddWithValue("@CommntText", objCLRtblTicketActnEntities.CommntText);
       //         Cmd.Parameters.AddWithValue("@DocmntFileName", objCLRtblTicketActnEntities.DocmntFileName);
       //         Cmd.Parameters.AddWithValue("@EnteredByID", objCLRtblTicketActnEntities.EnteredByID);
       //         Cmd.Parameters.AddWithValue("@TicketActnDateTime", Convert.ToDateTime(objCLRtblTicketActnEntities.TicketActnDateTime));
       //         Cmd.Parameters.AddWithValue("@TicketActnStatusCode", objCLRtblTicketActnEntities.TicketActnStatusCode);
       //         Cmd.Parameters.AddWithValue("@TicketActnTypeCode", objCLRtblTicketActnEntities.TicketActnTypeCode);
       //         Cmd.Parameters.AddWithValue("@TicketID", objCLRtblTicketActnEntities.TicketID);
       //         Cmd.Parameters.AddWithValue("@UpdateDate", Convert.ToDateTime(objCLRtblTicketActnEntities.UpdateDate));
       //         Cmd.Parameters.AddWithValue("@UpdateOpertr", objCLRtblTicketActnEntities.UpdateOpertr);
       //         //Passing parameters to sp.


       //         RowInserterd = Cmd.ExecuteNonQuery();// Get affected row index.
       //         Conn.Close();//Connection Closed.
       //         return RowInserterd.ToString();//Returning Result.
       //     }
       //     catch (Exception ex)
       //     {
       //         return ex.Message;
       //     }
       // }