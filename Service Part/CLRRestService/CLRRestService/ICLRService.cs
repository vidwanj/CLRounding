using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;


namespace CLRRestService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICLRService" in both code and config file together.
    [ServiceContract]
    public interface ICLRService
    {      

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Wrapped)]
        string UpdateRoundingLogForRoom(CLRUpdateRoundingRoomEntity objCLRUpdateRoundingRoomEntity);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Wrapped)]
        string InsertMarkStatusForRoom(CLRUpdateRoundingRoomEntity objCLRUpdateRoundingRoomEntity);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Wrapped)]
        string UpdateQuestionResponseRoundingLogForRoom(CLRQuestionResponceEntities objCLRQuestionResponceEntities);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Wrapped)]
        string UpdateFollowStatus(CLRUpdateFollowUp objCLRUpdateFollowUp);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Wrapped)]
        string UpdateFollowStatusV1(CLRUpdateFollowUpv1 objCLRUpdateFollowUpv1);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Wrapped)]
        string MassUpdateRoundingRoomStatus(CLRMassUpdateRoundingRoomEntity objCLRMassUpdateRoundingRoomEntity);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Wrapped)]
        string v1UpdateRoundingRoomStatus(version1CLRUpdateRoundingRoomEntity objversion1CLRUpdateRoundingRoomEntity);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Wrapped)]
        string v1UpdateQuestionResponseRoundingLogForRoom(CLRQuestionResponceEntities objCLRQuestionResponceEntities);
        
    }

    public class clrEntities
    {
        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public string UserPassword { get; set; }

        [DataMember]
        public string QuestnRespnsID { get; set; }
       
        [DataMember]
        public string ClR_ID { get; set; }

        [DataMember]
        public string CommntText { get; set; }

        [DataMember]
        public string EnteredByID { get; set; }

        [DataMember]
        public string QuestnStatusCode { get; set; }

        [DataMember]
        public string RespnsDateTime { get; set; }

        [DataMember]
        public string RespnsText { get; set; }

        [DataMember]
        public string UpdateDate { get; set; }

        [DataMember]
        public string UpdateOpertr { get; set; }

        [DataMember]
        public string QuestnID { get; set; }
    }

    public class CLRtblEntities
    {
        [DataMember]
        public string ClR_ID { get; set;}

        [DataMember]
        public string CLRDateTime { get; set; }

        [DataMember]
        public string CLRStatusCode { get; set; }

        [DataMember]
        public string CLRTypeID { get; set; }

        [DataMember]
        public string CommntText { get; set; }

        [DataMember]
        public string EnteredByID { get; set; }

        [DataMember]
        public string MRN_EncntrDescr { get; set; }

        [DataMember]
        public string NurseNote { get; set; }

        [DataMember]
        public string RoomID { get; set; }

        [DataMember]
        public string RoomStatusCode { get; set; }

        [DataMember]
        public string UpdateDate { get; set; }

        [DataMember]
        public string UpdateOpertr { get; set; }

    }

    public class CLRTypetblEntities
    {
        [DataMember]
        public string CLRTypeID { get; set; }

        [DataMember]
        public string CLRTypeAbbrvtnName { get; set; }

        [DataMember]
        public string CLRTypeName { get; set; }

        [DataMember]
        public string CommntText { get; set; }

        [DataMember]
        public string ActvInactvInd { get; set; }

        [DataMember]
        public string DefltSortOrdr { get; set; }

        [DataMember]
        public string UpdateDate { get; set; }

        [DataMember]
        public string UpdateOpertr { get; set; }
    }

    public class CLRDepttblEntities
    {
        [DataMember]
        public string DeptID { get; set; }

        [DataMember]
        public string DeptAbbrvtnName { get; set; }

        [DataMember]
        public string LocatnID { get; set; }

        [DataMember]
        public string SvcLineID { get; set; }

        [DataMember]
        public string ActvInactvInd { get; set; }

        [DataMember]
        public string DefltSortOrdr { get; set; }

        [DataMember]
        public string UpdateDate { get; set; }

        [DataMember]
        public string UpdateOpertr { get; set; }
    }

    public class CLRTypeQuestionEntities
    {
        [DataMember]
        public string CLRTypeID { get; set; }

        [DataMember]
        public string QuestnID { get; set; }

        [DataMember]
        public string UpdateDate { get; set; }

        [DataMember]
        public string UpdateOpertr { get; set; }
    }

    public class CLRDeptContactEntities
    {
        [DataMember]
        public string DeptContactID { get; set; }

        [DataMember]
        public string DeptContactTypeCode { get; set; }

        [DataMember]
        public string DeptID { get; set; }

        [DataMember]
        public string EmpleID { get; set; }

        [DataMember]
        public string UpdateDate { get; set; }

        [DataMember]
        public string UpdateOpertr { get; set; }
    }

    public class CLRtblEntitieValues
    {
        [DataMember]
        public string EntityID { get; set; }

        [DataMember]
        public string EntityAbbrvtnName { get; set; }

        [DataMember]
        public string EntityName { get; set; }

        [DataMember]
        public string CLRContactID { get; set; }

        [DataMember]
        public string UpdateDate { get; set; }

        [DataMember]
        public string UpdateOpertr { get; set; }
    }

    public class CLRtblLocatnEntities
    {
        [DataMember]
        public string LocatnID { get; set; }

        [DataMember]
        public string EntityID { get; set; }

        [DataMember]
        public string LocatnAbbrvtnName { get; set; }

        [DataMember]
        public string LocatnName { get; set; }

        [DataMember]
        public string ActvInactvInd { get; set; }

        [DataMember]
        public string DefltSortOrdr { get; set; }

        [DataMember]
        public string UpdateDate { get; set; }

        [DataMember]
        public string UpdateOpertr { get; set; }
    }

    public class CLRtblQuestnEntities
    {
        [DataMember]
        public string CommntText { get; set; }

        [DataMember]
        public string QuestnText { get; set; }

        [DataMember]
        public string QuestnShortText { get; set; }

        [DataMember]
        public string TicketTypeCode { get; set; }

        [DataMember]
        public string RespnsTypeCode { get; set; }

        [DataMember]
        public string ActvInactvInd { get; set; }

        [DataMember]
        public string UpdateDate { get; set; }

        [DataMember]
        public string UpdateOpertr { get; set; }
    }

    public class CLRtblRoomEntities
    {
        [DataMember]
        public string CommntText { get; set; }

        [DataMember]
        public string DeptID { get; set; }

        [DataMember]
        public string RoomDescr { get; set; }

        [DataMember]
        public string RoomTypeCode { get; set; }

        [DataMember]
        public string ActvInactvInd { get; set; }

        [DataMember]
        public string UpdateDate { get; set; }

        [DataMember]
        public string UpdateOpertr { get; set; }
    }

    public class CLRtblSvcLineEntities
    {
        [DataMember]
        public string SvcLineAbbrvtnName { get; set; }

        [DataMember]
        public string SvcLineName { get; set; }

        [DataMember]
        public string ActvInactvInd { get; set; }

        [DataMember]
        public string UpdateDate { get; set; }

        [DataMember]
        public string UpdateOpertr { get; set; }
    }

    public class CLRtblTicketEntities
    {
        [DataMember]
        public string CommntText { get; set; }
        [DataMember]
        public string DeptID { get; set; }
        [DataMember]
        public string EnteredByID { get; set; }
        [DataMember]
        public string IssueText { get; set; }
        [DataMember]
        public string ResltnText { get; set; }
        [DataMember]
        public string ReslvdByID { get; set; }
        [DataMember]
        public string QuestnRespnsID { get; set; }
        [DataMember]
        public string TicketDateTime { get; set; }
        [DataMember]
        public string TicketPriorityCode { get; set; }
        [DataMember]
        public string TicketStatusCode { get; set; }
        [DataMember]
        public string TicketTypeCode { get; set; }
        [DataMember]
        public string UdpateDate { get; set; }
        [DataMember]
        public string UpdateOpertr { get; set; }        
    }

    public class CLRtblTicketActnEntities
    {
        public string CommntText { get; set; }

        public string DocmntFileName { get; set; }

        public string EnteredByID { get; set; }

        public string TicketActnDateTime { get; set; }

        public string TicketActnStatusCode { get; set; }

        public string TicketActnTypeCode { get; set; }

        public string TicketID { get; set; }

        public string UpdateDate { get; set; }

        public string UpdateOpertr { get; set; }
    }

    public class CLRMassUpdateRoundingRoomEntity
    {
        [DataMember]
        public string MassUpdate { get; set; }
    }

    public class CLRUpdateRoundingRoomEntity
    {
        [DataMember]
        public string RoomID { get; set; }

        [DataMember]
        public string CLRDateTime { get; set; }

        [DataMember]
        public string RoomStatusCode { get; set; }

        [DataMember]
        public string ClR_ID { get; set; }

        [DataMember]
        public string CLRStatusCode { get; set; }

        [DataMember]
        public string NurseNote { get; set; }

        [DataMember]
        public string EnteredByID { get; set; }

        [DataMember]
        public string UpdateDate { get; set; }

        [DataMember]
        public string UpdateOpertr { get; set; }
    }

    public class CLRQuestionResponceEntities
    {
        [DataMember]
        public string QuestnRespnsID { get; set; }

        [DataMember]
        public string ClR_ID { get; set; }

        [DataMember]
        public string CommntText { get; set; }

        [DataMember]
        public string EnteredByID { get; set; }

        [DataMember]
        public string QuestnStatusCode { get; set; }

        [DataMember]
        public string RespnsDateTime { get; set; }

        [DataMember]
        public string RespnsText { get; set; }

        [DataMember]
        public string UpdateDate { get; set; }

        [DataMember]
        public string UpdateOpertr { get; set; }

        [DataMember]
        public string QuestnID { get; set; }

        [DataMember]
        public string FollowupFlag { get; set; }
    }

    public class CLRUpdateFollowUp
    {
        [DataMember]
        public string TicketID { get; set; }      

        [DataMember]
        public string TicketTypeCode { get; set; }

        [DataMember]
        public string UdpateDate { get; set; }

        [DataMember]
        public string UpdateOpertr { get; set; }
    }

    public class CLRUpdateFollowUpv1
    {
        [DataMember]
        public string TicketID { get; set; }

        [DataMember]
        public string ResltnText { get; set; }

        [DataMember]
        public string TicketTypeCode { get; set; }

        [DataMember]
        public string UdpateDate { get; set; }

        [DataMember]
        public string UpdateOpertr { get; set; }
    }

  

    public class version1CLRUpdateRoundingRoomEntity
    {
        [DataMember]
        public string RoomID { get; set; }

        [DataMember]
        public string CLRDateTime { get; set; }

        [DataMember]
        public string RoomStatusCode { get; set; }

        [DataMember]
        public string ClR_ID { get; set; }

        [DataMember]
        public string CLRStatusCode { get; set; }

        [DataMember]
        public string NurseNote { get; set; }

        [DataMember]
        public string EnteredByID { get; set; }

        [DataMember]
        public string UpdateDate { get; set; }

        [DataMember]
        public string UpdateOpertr { get; set; }

        [DataMember]
        public string PatientStateCode { get; set; }

        [DataMember]
        public string PersnInRoomCode { get; set; }
    }
}




 //[OperationContract]
 //       [WebInvoke(Method = "POST",
 //                   RequestFormat = WebMessageFormat.Json,
 //                   ResponseFormat = WebMessageFormat.Json,
 //                   BodyStyle = WebMessageBodyStyle.Wrapped)]
        
 //       string InsertQuestionResponce(clrEntities objclrEntities);

 //       [OperationContract]
 //       [WebInvoke(Method = "POST",
 //                   RequestFormat = WebMessageFormat.Json,
 //                   ResponseFormat = WebMessageFormat.Json,
 //                   BodyStyle = WebMessageBodyStyle.Wrapped)]
 //       string UpdateQuestionResponce(clrEntities objclrEntities);

 //       [OperationContract]
 //       [WebInvoke(Method = "POST",
 //                   RequestFormat = WebMessageFormat.Json,
 //                   ResponseFormat = WebMessageFormat.Json,
 //                   BodyStyle = WebMessageBodyStyle.Wrapped)]
 //       string InsertCLR(CLRtblEntities objCLRtblEntities);

 //       [OperationContract]
 //       [WebInvoke(Method = "POST",
 //                   RequestFormat = WebMessageFormat.Json,
 //                   ResponseFormat = WebMessageFormat.Json,
 //                   BodyStyle = WebMessageBodyStyle.Wrapped)]
 //       string UpdateCLR(CLRtblEntities objCLRtblEntities);

 //       [OperationContract]
 //       [WebInvoke(Method = "POST",
 //                   RequestFormat = WebMessageFormat.Json,
 //                   ResponseFormat = WebMessageFormat.Json,
 //                   BodyStyle = WebMessageBodyStyle.Wrapped)]
 //       string InsertCLRType(CLRTypetblEntities objCLRTypetblEntities);

 //       [OperationContract]
 //       [WebInvoke(Method = "POST",
 //                   RequestFormat = WebMessageFormat.Json,
 //                   ResponseFormat = WebMessageFormat.Json,
 //                   BodyStyle = WebMessageBodyStyle.Wrapped)]
 //       string InsertDept(CLRDepttblEntities objDepttblEntities);

 //       [OperationContract]
 //       [WebInvoke(Method = "POST",
 //                   RequestFormat = WebMessageFormat.Json,
 //                   ResponseFormat = WebMessageFormat.Json,
 //                   BodyStyle = WebMessageBodyStyle.Wrapped)]
 //       string InsertCLRTypeQuestion(CLRTypeQuestionEntities objCLRTypeQuestionEntities);

 //       [OperationContract]
 //       [WebInvoke(Method = "POST",
 //                   RequestFormat = WebMessageFormat.Json,
 //                   ResponseFormat = WebMessageFormat.Json,
 //                   BodyStyle = WebMessageBodyStyle.Wrapped)]
 //       string InsertDeptContact(CLRDeptContactEntities objCLRDeptContactEntities);

 //       [OperationContract]
 //       [WebInvoke(Method = "POST",
 //                   RequestFormat = WebMessageFormat.Json,
 //                   ResponseFormat = WebMessageFormat.Json,
 //                   BodyStyle = WebMessageBodyStyle.Wrapped)]
 //       string InsertCLREntity(CLRtblEntitieValues objCLRtblEntitieValues);

 //       [OperationContract]
 //       [WebInvoke(Method = "POST",
 //                   RequestFormat = WebMessageFormat.Json,
 //                   ResponseFormat = WebMessageFormat.Json,
 //                   BodyStyle = WebMessageBodyStyle.Wrapped)]
 //       string InsertLocation(CLRtblLocatnEntities objCLRtblLocatnEntities);

 //       [OperationContract]
 //       [WebInvoke(Method = "POST",
 //                   RequestFormat = WebMessageFormat.Json,
 //                   ResponseFormat = WebMessageFormat.Json,
 //                   BodyStyle = WebMessageBodyStyle.Wrapped)]
 //       string InsertCLRQuestions(CLRtblQuestnEntities objCLRtblQuestnEntities);

 //       [OperationContract]
 //       [WebInvoke(Method = "POST",
 //                   RequestFormat = WebMessageFormat.Json,
 //                   ResponseFormat = WebMessageFormat.Json,
 //                   BodyStyle = WebMessageBodyStyle.Wrapped)]
 //       string InsertRoom(CLRtblRoomEntities objCLRtblRoomEntities);

 //       [OperationContract]
 //       [WebInvoke(Method = "POST",
 //                   RequestFormat = WebMessageFormat.Json,
 //                   ResponseFormat = WebMessageFormat.Json,
 //                   BodyStyle = WebMessageBodyStyle.Wrapped)]
 //       string Insertsvcline(CLRtblSvcLineEntities objCLRtblSvcLineEntities);

 //       [OperationContract]
 //       [WebInvoke(Method = "POST",
 //                   RequestFormat = WebMessageFormat.Json,
 //                   ResponseFormat = WebMessageFormat.Json,
 //                   BodyStyle = WebMessageBodyStyle.Wrapped)]
 //       string InsertTicket(CLRtblTicketEntities objCLRtblTicketEntities);

 //       [OperationContract]
 //       [WebInvoke(Method = "POST",
 //                   RequestFormat = WebMessageFormat.Json,
 //                   ResponseFormat = WebMessageFormat.Json,
 //                   BodyStyle = WebMessageBodyStyle.Wrapped)]
 //       string InsertTicketAction(CLRtblTicketActnEntities objCLRtblTicketActnEntities);