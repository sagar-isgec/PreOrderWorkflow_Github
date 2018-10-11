using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PreOrderWorkflow
{
    public class WorkFlow
    {
        public static string Con = ConfigurationManager.AppSettings["Connection"];
        //public static string ConTest = ConfigurationManager.AppSettings["ConnectionTest"]; ConnectionLive
        public static string Con129 = ConfigurationManager.AppSettings["ConnectionLive"];

        #region Properties
        public int WFID { get; set; }
        public int WFIDHistoryId { get; set; }
        public int Parent_WFID { get; set; }
        public int SLNO_WFID { get; set; }
        public string Project { get; set; }
        public string Element { get; set; }
        public string SpecificationNo { get; set; }
        public string Buyer { get; set; }
        public string Manager { get; set; }
        public string WF_Status { get; set; }
        public string UserId { get; set; }
        public string Supplier { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }

        public string EmailSubject { get; set; }
        public string Notes { get; set; }

        public string AttachmentId { get; set; }
        public string AthhFile { get; set; }
        public string IndexValue { get; set; }
        public string AttachmentHandle { get; set; }
        public string NotesHandle { get; set; }
        public string PurposeCode { get; set; }
        public string FileName { get; set; }
        public string LibraryCode { get; set; }
        public string AttachedBy { get; set; }
        public string RandomNo { get; set; }
        public string PMDLdocDesc { get; set; }
        // public DateTime TransactionDate { get; set; }
        public string TransactionDate { get; set; }
        public string BusinessObjectHandle { get; set; }
        public long DetailSerialCount { get; set; }
        public int PartItemCount { get; set; }
        public double PartItemWeight { get; set; }

        public string ProjectFrom { get; set; }
        public string ProjectTo { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }

        public string MultiPMDLdocDesc { get; set; }

        public string BuyerFrom { get; set; }
        public string BuyerTo { get; set; }
        public string PMDLFrom { get; set; }
        public string PMDLTo { get; set; }

        public string ItemReference { get; set; }

        #endregion

        #region Methods
        public WorkFlow(int a)
        { }
        public WorkFlow()
        { }
        public DataTable GetWFById()

        {
            return SqlHelper.ExecuteDataset(Con, CommandType.Text, @"select [WFID]
                                      ,[Parent_WFID]
                                      ,[Project]
                                      ,[Element]
                                      ,[SpecificationNo]
                                      ,[PMDLDocNo]
                                      ,(Select EmployeeName FROM HRM_Employees where CardNo=Buyer) as BuyerName
                                      ,[Buyer]
                                      ,(Select EmployeeName FROM HRM_Employees where CardNo=Manager) as ManagerName
                                      ,[Manager]
                                      ,[WF_Status]
                                      ,[UserId]
                                      ,[DateTime]
                                      ,[Supplier]
                                      ,[SupplierName]
                                      ,[SupplierCode]
                                      ,[EmailSubject]
                                      ,EmployeeName,RandomNo
                                       from [WF1_PreOrder] WF
                                INNER JOIN HRM_Employees E on E.CardNo=WF.UserId
                                WHERE WFID='" + WFID + "'").Tables[0]; //
        }

        public DataTable GetWFBY_Status()
        {
            string WFbyStatus = @"select [WFID],[Parent_WFID],[Project] ,[Element],[SpecificationNo]
              ,[PMDLDocNo],(Select EmployeeName FROM HRM_Employees where CardNo=Buyer) as BuyerName
              ,[Buyer],(Select EmployeeName FROM HRM_Employees where CardNo=Manager) as ManagerName
              ,[Manager],[WF_Status] ,[UserId],[DateTime],[Supplier],[SupplierName] ,EmployeeName
                from [WF1_PreOrder] WF INNER JOIN HRM_Employees E on E.CardNo=WF.UserId
                WHERE Wf_Status in('" + WF_Status + "')  and WF.Buyer='" + UserId + "' order by WFID desc";


            return SqlHelper.ExecuteDataset(Con, CommandType.Text, WFbyStatus).Tables[0];

            //return SqlHelper.ExecuteDataset(Con, CommandType.Text, @"select [WFID]
            //                          ,[Parent_WFID]
            //                          ,[Project]
            //                          ,[Element]
            //                          ,[SpecificationNo]
            //                          ,[PMDLDocNo]
            //                          ,(Select EmployeeName FROM HRM_Employees where CardNo=Buyer) as BuyerName
            //                          ,[Buyer]
            //                          ,[WF_Status]
            //                          ,[UserId]
            //                          ,[DateTime]
            //                          ,[Supplier]
            //                          ,[SupplierName]
            //                          ,EmployeeName
            //                           from [WF1_PreOrder] WF
            //                    INNER JOIN HRM_Employees E on E.CardNo=WF.UserId
            //                    WHERE Wf_Status in(" + WF_Status + ")  and WF.Buyer='" + UserId + "' order by WFID desc").Tables[0]; //
        }

        public DataTable PopulateDropdownList()
        {
            return SqlHelper.ExecuteDataset(Con, CommandType.Text, @"select distinct Project from WF1_PreOrder").Tables[0];
        }

        public DataTable PopulatePMDLDropdownList()
        {
            return SqlHelper.ExecuteDataset(Con, CommandType.Text, @"select distinct PMDLDocNo from WF1_PreOrder where PMDLDocNo not in ('')").Tables[0];
        }
        public DataTable PopulateBuyerDropdownList()
        {
            return SqlHelper.ExecuteDataset(Con, CommandType.Text, @"select distinct WF.Buyer,HRM.EmployeeName, HRM.CardNo from WF1_PreOrder WF join HRM_Employees HRM on WF.Buyer= HRM.CardNo order by HRM.CardNo asc").Tables[0];
        }
        public DataTable GetPreOrderData()
        {
            string WFbyStatus = @"select WF.WFID,WF.Parent_WFID,WF.Project ,WF.Element,WF.SpecificationNo
              ,WF.PMDLDocNo,(Select EmployeeName FROM HRM_Employees where CardNo=WF.Buyer) as BuyerName
              ,WF.Buyer,(Select EmployeeName FROM HRM_Employees where CardNo=WF.Manager) as ManagerName
              ,WF.Manager,WF.WF_Status ,WF.UserId,WH.DateTime, WF.DateTime as LatestStatusDateTime, WF.Supplier,WF.SupplierName ,E.EmployeeName
               from [WF1_PreOrder] WF 
               INNER JOIN HRM_Employees E on E.CardNo=WF.UserId 
               Inner join WF1_PreOrder_History WH on WH.WFID = WF.WFID
               where WH.WF_Status='Technical Specification Released' 
               order by WF.WFID desc";
            return SqlHelper.ExecuteDataset(Con, CommandType.Text, WFbyStatus).Tables[0];

        }

        public DataTable GetPreOrderData_byProject()
        {
            string WFbyStatus = @"select WF.WFID,WF.Parent_WFID,WF.Project ,WF.Element,WF.SpecificationNo
              ,WF.PMDLDocNo,(Select EmployeeName FROM HRM_Employees where CardNo=WF.Buyer) as BuyerName
              ,WF.Buyer,(Select EmployeeName FROM HRM_Employees where CardNo=WF.Manager) as ManagerName
              ,WF.Manager,WF.WF_Status ,WF.UserId,WH.DateTime,WF.DateTime as LatestStatusDateTime,WF.Supplier,WF.SupplierName ,E.EmployeeName
               from [WF1_PreOrder] WF 
               INNER JOIN HRM_Employees E on E.CardNo=WF.UserId 
               Inner join WF1_PreOrder_History WH on WH.WFID = WF.WFID
              where WH.WF_Status='Technical Specification Released' 
              and WF.Project between '" + ProjectFrom + "' and '" + ProjectTo + "' order by WF.WFID desc";
            return SqlHelper.ExecuteDataset(Con, CommandType.Text, WFbyStatus).Tables[0];
        }
        public DataTable GetPreOrderData_byDate()
        {
            string WFbyStatus = @"select WF.WFID,WF.Parent_WFID,WF.Project ,WF.Element,WF.SpecificationNo
              ,WF.PMDLDocNo,(Select EmployeeName FROM HRM_Employees where CardNo=WF.Buyer) as BuyerName
              ,WF.Buyer,(Select EmployeeName FROM HRM_Employees where CardNo=WF.Manager) as ManagerName
              ,WF.Manager,WF.WF_Status ,WF.UserId,WH.DateTime,WF.DateTime as LatestStatusDateTime,WF.Supplier,WF.SupplierName ,E.EmployeeName
               from [WF1_PreOrder] WF 
               INNER JOIN HRM_Employees E on E.CardNo=WF.UserId 
               Inner join WF1_PreOrder_History WH on WH.WFID = WF.WFID
              where WH.WF_Status='Technical Specification Released' 
              and replace(convert(varchar, WH.[DateTime], 111), '/','-') between '" + DateFrom + "' and '" + DateTo + "' order by WF.WFID desc";
            return SqlHelper.ExecuteDataset(Con, CommandType.Text, WFbyStatus).Tables[0];
        }
        //GetPreOrderData_byDate GetPreOrderData_byDate_Project
        public DataTable GetPreOrderData_byDate_Project()
        {
            string WFbyStatus = @"select WF.WFID,WF.Parent_WFID,WF.Project ,WF.Element,WF.SpecificationNo
              ,WF.PMDLDocNo,(Select EmployeeName FROM HRM_Employees where CardNo=WF.Buyer) as BuyerName
              ,WF.Buyer,(Select EmployeeName FROM HRM_Employees where CardNo=WF.Manager) as ManagerName
              ,WF.Manager,WF.WF_Status ,WF.UserId,WH.DateTime, WF.DateTime as LatestStatusDateTime,WF.Supplier,WF.SupplierName ,E.EmployeeName
               from [WF1_PreOrder] WF 
               INNER JOIN HRM_Employees E on E.CardNo=WF.UserId 
               Inner join WF1_PreOrder_History WH on WH.WFID = WF.WFID
              where WH.WF_Status='Technical Specification Released' 
              and replace(convert(varchar, WH.[DateTime], 111), '/','-') between '" + DateFrom + "' and '" + DateTo + "' and WH.Project between '" + ProjectFrom + "' and '" + ProjectTo + "' order by WF.WFID desc";
            return SqlHelper.ExecuteDataset(Con, CommandType.Text, WFbyStatus).Tables[0];
        }

        public DataTable GetFinalizedStatus()
        {
            string WFbyStatus = @"select [WFID],[Parent_WFID],[Project] ,[Element],[SpecificationNo]
              ,[PMDLDocNo],(Select EmployeeName FROM HRM_Employees where CardNo=Buyer) as BuyerName
              ,[Buyer],(Select EmployeeName FROM HRM_Employees where CardNo=Manager) as ManagerName
              ,[Manager],[WF_Status] ,[UserId],[DateTime],[Supplier],[SupplierName] ,EmployeeName
                from [WF1_PreOrder] WF INNER JOIN HRM_Employees E on E.CardNo=WF.UserId
                WHERE Wf_Status in('" + WF_Status + "')  order by WFID desc";


            return SqlHelper.ExecuteDataset(Con, CommandType.Text, WFbyStatus).Tables[0];


        }
        public DataTable GetWFBY_Status_Project()
        {
            string WFbyStatus = @"select [WFID],[Parent_WFID],[Project] ,[Element],[SpecificationNo]
              ,[PMDLDocNo],(Select EmployeeName FROM HRM_Employees where CardNo=Buyer) as BuyerName
              ,[Buyer],(Select EmployeeName FROM HRM_Employees where CardNo=Manager) as ManagerName
              ,[Manager],[WF_Status] ,[UserId],[DateTime],[Supplier],[SupplierName] ,EmployeeName
                from [WF1_PreOrder] WF INNER JOIN HRM_Employees E on E.CardNo=WF.UserId
                WHERE Wf_Status in('" + WF_Status + "')  and WF.Project='" + Project + "'order by WFID desc";


            return SqlHelper.ExecuteDataset(Con, CommandType.Text, WFbyStatus).Tables[0];

        }
        public DataTable GetWFBY_Status_Element()
        {
            string WFbyStatus = @"select [WFID],[Parent_WFID],[Project] ,[Element],[SpecificationNo]
              ,[PMDLDocNo],(Select EmployeeName FROM HRM_Employees where CardNo=Buyer) as BuyerName
              ,[Buyer],(Select EmployeeName FROM HRM_Employees where CardNo=Manager) as ManagerName
              ,[Manager],[WF_Status] ,[UserId],[DateTime],[Supplier],[SupplierName] ,EmployeeName
                from [WF1_PreOrder] WF INNER JOIN HRM_Employees E on E.CardNo=WF.UserId
                WHERE Wf_Status in('" + WF_Status + "')  and WF.Element='" + Element + "'order by WFID desc";


            return SqlHelper.ExecuteDataset(Con, CommandType.Text, WFbyStatus).Tables[0];

        }

        public DataTable GetWFBY_Status_PMDL()
        {
            string WFbyStatus = @"select [WFID],[Parent_WFID],[Project] ,[Element],[SpecificationNo]
              ,[PMDLDocNo],(Select EmployeeName FROM HRM_Employees where CardNo=Buyer) as BuyerName
              ,[Buyer],(Select EmployeeName FROM HRM_Employees where CardNo=Manager) as ManagerName
              ,[Manager],[WF_Status] ,[UserId],[DateTime],[Supplier],[SupplierName] ,EmployeeName
                from [WF1_PreOrder] WF INNER JOIN HRM_Employees E on E.CardNo=WF.UserId
                WHERE Wf_Status in('" + WF_Status + "')  and WF.PMDLDocNo='" + PMDLdocDesc + "'order by WFID desc";


            return SqlHelper.ExecuteDataset(Con, CommandType.Text, WFbyStatus).Tables[0];

        }
        public DataTable GetWFBY_Status_Spec()
        {
            string WFbyStatus = @"select [WFID],[Parent_WFID],[Project] ,[Element],[SpecificationNo]
              ,[PMDLDocNo],(Select EmployeeName FROM HRM_Employees where CardNo=Buyer) as BuyerName
              ,[Buyer],(Select EmployeeName FROM HRM_Employees where CardNo=Manager) as ManagerName
              ,[Manager],[WF_Status] ,[UserId],[DateTime],[Supplier],[SupplierName] ,EmployeeName
                from [WF1_PreOrder] WF INNER JOIN HRM_Employees E on E.CardNo=WF.UserId
                WHERE Wf_Status in('" + WF_Status + "')  and WF.SpecificationNo='" + SpecificationNo + "'order by WFID desc";


            return SqlHelper.ExecuteDataset(Con, CommandType.Text, WFbyStatus).Tables[0];

        }
        public DataTable GetWFBYForUser()
        {
            return SqlHelper.ExecuteDataset(Con, CommandType.Text, @"select [WFID]
                                      ,[Parent_WFID]
                                      ,[Project]
                                      ,[Element]
                                      ,[SpecificationNo]
                                      ,[PMDLDocNo]
                                      ,(Select EmployeeName FROM HRM_Employees where CardNo=Buyer) as BuyerName
                                      ,[Buyer]
                                      ,[WF_Status]
                                      ,[UserId]
                                      ,[DateTime]
                                      ,[Supplier]
                                      ,[SupplierName]
                                      ,EmployeeName
                                       from [WF1_PreOrder] WF
                                INNER JOIN HRM_Employees E on E.CardNo=WF.UserId
                                WHERE WF.UserId='" + UserId + "' and Parent_WFID=0 order by WFID desc").Tables[0]; //
        }
        public DataTable GetRaisedEnqiry()
        {
            return SqlHelper.ExecuteDataset(Con, CommandType.Text, @"select [WFID]
                                      ,[Parent_WFID]
                                      ,[Project]
                                      ,[Element]
                                      ,[SpecificationNo]
                                      ,[PMDLDocNo]
                                      ,(Select EmployeeName FROM HRM_Employees where CardNo=Buyer) as BuyerName
                                      ,[Buyer]
                                      ,(Select EmployeeName FROM HRM_Employees where CardNo=Manager) as ManagerName
                                      ,[Manager]
                                      ,[WF_Status]
                                      ,[UserId]
                                      ,[DateTime]
                                      ,[Supplier]
                                      ,[SupplierName]
                                      ,EmployeeName
                                       from [WF1_PreOrder] WF
                                INNER JOIN HRM_Employees E on E.CardNo=WF.UserId
                                WHERE WF.Buyer='" + UserId + "' and Parent_WFID='" + Parent_WFID + "' order by Parent_WFID desc").Tables[0]; //
        }
        public DataTable GetWFByParentId()
        {
            return SqlHelper.ExecuteDataset(Con, CommandType.Text, @"select [WFID]
                                      ,[Parent_WFID]
                                      ,[Project]
                                      ,[Element]
                                      ,[SpecificationNo]
                                      ,[PMDLDocNo]
                                      ,(Select EmployeeName FROM HRM_Employees where CardNo=Buyer) as BuyerName
                                      ,[Buyer]
                                      ,[WF_Status]
                                      ,[UserId]
                                      ,[DateTime]
                                      ,[Supplier]
                                      ,[SupplierName]
                                      ,EmployeeName
                                       from [WF1_PreOrder] WF
                                INNER JOIN HRM_Employees E on E.CardNo=WF.UserId
                                WHERE Parent_WFID='" + Parent_WFID + "'").Tables[0]; //
        }
        public DataTable GetHistory()
        {
            return SqlHelper.ExecuteDataset(Con, CommandType.Text, @"SELECT [WF_HistoryID]
                                      ,[WFID]
                                      ,[WFID_SlNo]
                                      ,[Parent_WFID]
                                      ,[Project]
                                      ,[Element]
                                      ,[SpecificationNo]
                                      ,[PMDLDocNo]
                                      ,[Buyer]
                                      ,(Select EmployeeName FROM HRM_Employees where CardNo=Buyer) as BuyerName
                                      ,[Manager]
                                      ,(Select EmployeeName FROM HRM_Employees where CardNo=Manager) as ManagerName
                                      ,[WF_Status]
                                      ,[UserId]
                                      ,[DateTime]
                                      ,[Supplier]
                                      ,[SupplierName]
                                      ,[Notes]
                                  FROM [WF1_PreOrder_History] WF
                                  INNER JOIN HRM_Employees E on E.CardNo=WF.UserId
                                WHERE  WFID='" + WFID + "'").Tables[0]; //
        }
        public DataTable InsertPreOrderData()
        {
            List<SqlParameter> sqlParamater = new List<SqlParameter>();
            sqlParamater.Add(new SqlParameter("@Parent_WFID", Parent_WFID));
            sqlParamater.Add(new SqlParameter("@Project", Project));
            sqlParamater.Add(new SqlParameter("@Element", Element));
            sqlParamater.Add(new SqlParameter("@SpecificationNo", SpecificationNo));
            sqlParamater.Add(new SqlParameter("@PMDLDocNo", PMDLdocDesc));
            sqlParamater.Add(new SqlParameter("@Buyer", Buyer));
            sqlParamater.Add(new SqlParameter("@Manager", Manager));
            sqlParamater.Add(new SqlParameter("@UserId", UserId));
            sqlParamater.Add(new SqlParameter("@WF_Status", WF_Status));


            //return SqlHelper.ExecuteDataset(Con, CommandType.StoredProcedure, "sp_Insert_PreOrder_Workflow", sqlParamater.ToArray()).Tables[0];[sp_Insert_PreOrder_Workflow_Test]
            return SqlHelper.ExecuteDataset(Con, CommandType.StoredProcedure, "sp_Insert_PreOrder_Workflow_Test", sqlParamater.ToArray()).Tables[0];
        }
        public void InsertMultiPMDL()
        {
            string sInsertPMDL = @" Insert into WF1_PreOrderPMDL values('" + WFID + "','" + MultiPMDLdocDesc + "')";
            SqlHelper.ExecuteNonQuery(Con, CommandType.Text, sInsertPMDL);
        }
        //InsertMultiPMDLtoDuplicateTable
        //public void InsertMultiPMDLtoDuplicateTable()
        //{
        //    string sInsertMultiPMDLtoDuplicateTable = @" Insert into tdmisg167200 values('" + WFID + "','" + MultiPMDLdocDesc + "',0,0)";
        //    SqlHelper.ExecuteNonQuery(Con129, CommandType.Text, sInsertMultiPMDLtoDuplicateTable);
        //}
        public DataTable InserPreOrderHistory()
        {
            List<SqlParameter> sqlParamater = new List<SqlParameter>();
            sqlParamater.Add(new SqlParameter("@WFID", WFID));
            sqlParamater.Add(new SqlParameter("@Parent_WFID", Parent_WFID));
            sqlParamater.Add(new SqlParameter("@Project", Project));
            sqlParamater.Add(new SqlParameter("@Element", Element));
            sqlParamater.Add(new SqlParameter("@SpecificationNo", SpecificationNo));
            sqlParamater.Add(new SqlParameter("@PMDLDocNo", PMDLdocDesc));
            sqlParamater.Add(new SqlParameter("@Buyer", Buyer));
            sqlParamater.Add(new SqlParameter("@Manager", Manager));
            sqlParamater.Add(new SqlParameter("@WF_Status", WF_Status));
            sqlParamater.Add(new SqlParameter("@UserId", UserId));
            sqlParamater.Add(new SqlParameter("@Supplier", Supplier));
            sqlParamater.Add(new SqlParameter("@SupplierName", SupplierName));
            sqlParamater.Add(new SqlParameter("@Notes", Notes));
            // return SqlHelper.ExecuteDataset(Con, CommandType.StoredProcedure, "sp_Insert_PreOrder_Workflow_History", sqlParamater.ToArray()).Tables[0];
            return SqlHelper.ExecuteDataset(Con, CommandType.StoredProcedure, "sp_Insert_PreOrder_Workflow_History_Test", sqlParamater.ToArray()).Tables[0];
        }
        public int UpdateWFPreOrder()
        {

            return SqlHelper.ExecuteNonQuery(Con, CommandType.Text, @"UPDATE [IJTPerks].[dbo].[WF1_PreOrder]
            SET [Project] = '" + Project + "',[Element] = '" + Element + "',[SpecificationNo] = '" + PMDLdocDesc + "',[PMDLDocNo] = '" + PMDLdocDesc + "',[Buyer] = '" + Buyer + @"'
                          ,[WF_Status] = '" + WF_Status + @"'                                                
                          ,[Supplier] = '" + Supplier + @"'
                          ,[Manager] = '" + Manager + @"'
                          WHERE [WFID] = '" + WFID + @"'");
            //
        }
        // NoteRefIndex
        public string NoteRefIndex(string WFID)
        {
            string sNoteIndex = @"Select ReceiptNo from  WF1_Preorder where  WFID = '" + WFID + "'";
            if (SqlHelper.ExecuteScalar(Con, CommandType.Text, sNoteIndex) != null)
            {
                return SqlHelper.ExecuteScalar(Con, CommandType.Text, sNoteIndex).ToString();
            }
            else
            {
                return "";
            }
        }
        public DataTable GetIndex()
        {
            //  string sGetIndex = @"select td132.t_tran from tdmisg132200 td132 join tdmisg131200 td131 on td132.t_tran =td131.t_tran where td131.t_type=4 and td131.t_dprj='" + Project + "' and td132.t_docn='" + PMDLdocDesc + "' and td132.t_tran not in ('') and td131.t_issu='001' ";
            string sGetIndex = @"select distinct td132.t_tran from tdmisg132200 td132 join tdmisg131200 td131 on td132.t_tran = td131.t_tran
   where td131.t_type in (2,3,4)  and td131.t_dprj = '" + Project + "' and td132.t_docn in ('" + PMDLdocDesc + @"')
   and td132.t_tran not in ('')  and td131.t_stat='5' and td131.t_isdt in (
   select Max(td131.t_isdt) from tdmisg132200 td132 join tdmisg131200 td131 on td132.t_tran = td131.t_tran
   where td131.t_type in (2,3,4) and td131.t_dprj = '" + Project + "' and td132.t_docn in ( '" + PMDLdocDesc + @"')
   and td132.t_tran not in ('') and td131.t_stat='5' and td132.t_revn in(select Max(td132.t_revn)from tdmisg132200 td132 join tdmisg131200 td131 on td132.t_tran = td131.t_tran
   where td131.t_type in (2,3,4) and td131.t_dprj = '" + Project + "' and td132.t_docn in ( '" + PMDLdocDesc + @"')
   and td132.t_tran not in ('') and td131.t_stat='5'))";
            if (SqlHelper.ExecuteScalar(Con129, CommandType.Text, sGetIndex) != null)
            {
                return SqlHelper.ExecuteDataset(Con129, CommandType.Text, sGetIndex).Tables[0];
            }
            else
            {
                DataTable dt = new DataTable();
                return dt;
            }
        }
        //public DataTable GetIndex()
        //{
        //    string sGetIndex = @"select (td132.t_tran+'_'+td132.t_docn+'_'+td132.t_revn)as Handle_Index from tdmisg132200 td132 join tdmisg131200 td131 on td132.t_tran =td131.t_tran where td131.t_type=4 and td131.t_dprj='" + Project + "' and td132.t_docn='" + PMDLdocDesc + "' and td132.t_tran not in ('') and td131.t_issu='001' ";
        //    return SqlHelper.ExecuteDataset(Con129, CommandType.Text, sGetIndex).Tables[0];
        //}
        public int UpdateStatusWFPreOrder_History()
        {
            return SqlHelper.ExecuteNonQuery(Con, CommandType.Text, @"UPDATE [IJTPerks].[dbo].[WF1_PreOrder_History]
            SET           [WF_Status] = '" + WF_Status + @"'
                          WHERE [WFID] = '" + WFID + @"' and [WFID_SlNo]='" + SLNO_WFID + "'");

            //'" + UserId + @"'     
        }
        public int UpdateWFPreOrder_History()
        {
            return SqlHelper.ExecuteNonQuery(Con, CommandType.Text, @"UPDATE [IJTPerks].[dbo].[WF1_PreOrder_History]
            SET [Project] = '" + Project + "',[Element] = '" + Element + "',[SpecificationNo] = '" + SpecificationNo + "',[PMDLDocNo] = '" + PMDLdocDesc + "',[Buyer] = '" + Buyer + @"'
                          ,[WF_Status] = '" + WF_Status + @"'
                          ,[Supplier] = '" + Supplier + @"'
                          ,[Manager] = '" + Manager + @"'
                          ,[Notes] = '" + Notes + @"'
                          WHERE [WFID] = '" + WFID + @"' and [WFID_SlNo]='" + SLNO_WFID + "'");

            //'" + UserId + @"'     
        }
        public int Delete_PreOrder()
        {
            return SqlHelper.ExecuteNonQuery(Con, CommandType.Text, "DELETE from Wf1_PreOrder WHERE WFID='" + WFID + "'");
        }
        public int Delete_PreOrderHistory()
        {
            return SqlHelper.ExecuteNonQuery(Con, CommandType.Text, "DELETE from WF1_PreOrder_History WHERE WFID='" + WFID + "'");
        }
        public int UpdateWF_Status()
        {
            return SqlHelper.ExecuteNonQuery(Con, CommandType.Text, "Update WF1_PreOrder set [WF_Status]='" + WF_Status + "',DateTime=getDate()  Where [WFID]='" + WFID + "'");
        }
        public int UpdateEnquiryRaised()
        {
            return SqlHelper.ExecuteNonQuery(Con, CommandType.Text, "Update WF1_PreOrder set [WF_Status]='" + WF_Status + "',Supplier='" + Supplier + "',SupplierName='" + SupplierName + "' ,EmailSubject='" + EmailSubject + "',SupplierCode='" + SupplierCode + "', DateTime=getDate(),RandomNo='" + RandomNo + "'  Where [WFID]='" + WFID + "'");
        }

        // change 24-08-2018 UpdateTechnoCommercialNegotiationCompleted
        public int UpdateTechnoCommercialNegotiationCompleted()
        {
            return SqlHelper.ExecuteNonQuery(Con, CommandType.Text, "Update WF1_PreOrder set [WF_Status]='" + WF_Status + "' ,DateTime=getDate() Where [WFID]='" + WFID + "'");
        }
        public DataTable GetWFID()
        {
            return SqlHelper.ExecuteDataset(Con, CommandType.Text, "SELECT [WFID] FROM [WF1_PreOrder] where [Parent_WFID]='" + Parent_WFID + "'").Tables[0];
        }
        public DataTable GetWFHID(int ID)
        {
            return SqlHelper.ExecuteDataset(Con, CommandType.Text, "Select  Max(WFID_SlNo) as WFSLNO from WF1_PreOrder_History WHERE WFID=" + ID + "").Tables[0];
        }
        // --------------  129 server-------------
        public DataTable GetProject()
        {
            return SqlHelper.ExecuteDataset(Con129, CommandType.Text, @"select ProjectCode,ProjectName from LN_ProjectMaster").Tables[0]; //
        }
        //PopulateDropdown
        public DataTable PopulateDropdown()
        {
            return SqlHelper.ExecuteDataset(Con, CommandType.Text, @"select Project,Element,Manager,PMDLDocNo,SpecificationNo from WF1_PreOrder").Tables[0];
        }
        public DataTable PopulateProjectDropdown()
        {
            return SqlHelper.ExecuteDataset(Con, CommandType.Text, @"select distinct Project from WF1_PreOrder where WF_Status = 'Commercial offer Finalized'").Tables[0];
        }
        public DataTable PopulateElementDropdown()
        {
            return SqlHelper.ExecuteDataset(Con, CommandType.Text, @"select distinct Element from WF1_PreOrder where WF_Status = 'Commercial offer Finalized'").Tables[0];
        }
        public DataTable PopulateSpecDropdown()
        {
            return SqlHelper.ExecuteDataset(Con, CommandType.Text, @"select distinct SpecificationNo from WF1_PreOrder where SpecificationNo not in ('') and  WF_Status = 'Commercial offer Finalized'").Tables[0];
        }
        public DataTable PopulatePMDLDropdown()
        {
            return SqlHelper.ExecuteDataset(Con, CommandType.Text, @"select distinct PMDLDocNo from WF1_PreOrder where PMDLDocNo not in ('') and  WF_Status = 'Commercial offer Finalized' ").Tables[0];
        }
        public DataTable GetProjectName(string Project)
        {
            return SqlHelper.ExecuteDataset(Con129, CommandType.Text, @"select ProjectCode,ProjectName from LN_ProjectMaster where ProjectCode='" + Project + "'").Tables[0]; //
        }
        public DataTable GetProjectElement(string Project)
        {
            return SqlHelper.ExecuteDataset(Con129, CommandType.Text, @"select Project,Element,ElementDescription from LN_ProjectElements where Project='" + Project + "'").Tables[0]; //
        }
        public DataTable GetProjectElementFDesc(string Project, string Element)
        {
            return SqlHelper.ExecuteDataset(Con129, CommandType.Text, @"select Element,ElementDescription from LN_ProjectElements where Project='" + Project + "' and Element='" + Element + "'").Tables[0]; //
        }
        public DataTable GetProjectSpecification(string Project, string Element)
        {
            return SqlHelper.ExecuteDataset(Con129, CommandType.Text, @"select Project,DocumentID,DocumentDescription from LN_PMDL where Project='" + Project + "' and Element ='" + Element + "'").Tables[0]; //
        }
        public DataTable GetProjectSpecificationDesc(string Project, string Element, string DocId)
        {
            return SqlHelper.ExecuteDataset(Con129, CommandType.Text, @"select DocumentID,DocumentDescription from LN_PMDL where Project='" + Project + "' and Element ='" + Element + "' and DocumentID='" + DocId + "'").Tables[0]; //
        }
        public DataTable GetProjectSpecificationMethod(string Doc)
        {
            return SqlHelper.ExecuteDataset(Con129, CommandType.Text, @"select Project,DocumentID,DocumentDescription from LN_PMDL where DocumentID Like '" + Doc + "%' or DocumentDescription Like '" + Doc + "%'").Tables[0]; //
        }
        public DataTable GetReleasedPMDLdoc(string ProjectCode, string Element)
        {
            string sPMDLdoc = @"select (DocumentID + '-' + DocumentDescription) as PMDLDoc, DocumentID from LN_PMDL where Project = '" + ProjectCode + "' and Element = '" + Element + "' and ActualReleaseDate > '2000-01-01'";
            return SqlHelper.ExecuteDataset(Con129, CommandType.Text, sPMDLdoc).Tables[0];

        }
        public DataTable GetWorkFlowPMDL()
        {
            string sWorkFlowPMDL = @"Select distinct PMDLDocNo from WF1_PreOrder where PMDLDocNo not in ('')";
            return SqlHelper.ExecuteDataset(Con, CommandType.Text, sWorkFlowPMDL).Tables[0];

        }
        public DataTable GetWorkFlowPMDL(string ProjectCode, string Element)
        {
            string sWorkFlowPMDL = @"Select distinct PMDLDocNo from WF1_PreOrder where PMDLDocNo not in ('') and Project like '%" + ProjectCode + "' and Element like '%" + Element + "' ";
            return SqlHelper.ExecuteDataset(Con, CommandType.Text, sWorkFlowPMDL).Tables[0];

        }
        public DataTable GetUnassignedPMDLdoc(string ProjectCode, string Element, string DocId)
        {
            string sUnassignedPMDLdoc = @"select (DocumentID + '-' + DocumentDescription) as PMDLDoc, DocumentID from LN_PMDL  where Project = '" + ProjectCode + "' and Element = '" + Element + "' and DocumentID not in ('" + DocId + "')";
            return SqlHelper.ExecuteDataset(Con129, CommandType.Text, sUnassignedPMDLdoc).Tables[0];
        }
        public DataTable GetPMDLdoc(string ProjectCode, string Element)
        {
            string sPMDLdoc = @"select (DocumentID + '-' + DocumentDescription) as PMDLDoc, DocumentID from LN_PMDL where Project = '" + ProjectCode + "' and Element = '" + Element + "'";
            return SqlHelper.ExecuteDataset(Con129, CommandType.Text, sPMDLdoc).Tables[0]; //
        }
        public DataTable GetSupplier(string supplierName)
        {
            string sSupplierId = @"select t_nama,t_bpid from ttccom100200 where ( UPPER(t_nama) Like '%" + supplierName.ToUpper() + "%') ";
            return SqlHelper.ExecuteDataset(Con129, CommandType.Text, sSupplierId).Tables[0];
        }
        //GetItemforSelectedproj_Element
        public DataTable GetItemforSelectedproj_Element()
        {
            //string sItem = @"select t_item from LN_ItemMaster where (LEN(t_item + '@') - LEN(REPLACE(t_item, ' ', '') + '@') )=9";
            string sItem = @"select distinct t_item from LN_IndentLines where t_cprj ='" + Project + "' and t_cspa='" + Element + "'";
            return SqlHelper.ExecuteDataset(Con129, CommandType.Text, sItem).Tables[0]; //
        }
        public DataTable GetItem()
        {
            //string sItem = @"select t_item from LN_ItemMaster where (LEN(t_item + '@') - LEN(REPLACE(t_item, ' ', '') + '@') )=9";
            string sItem = @"select distinct t_item from LN_ItemMaster where t_item like '         %'";
            return SqlHelper.ExecuteDataset(Con129, CommandType.Text, sItem).Tables[0]; //
        }
        public DataTable GetIndent(string indent, string projId)
        {
            string sPMDLdoc = @"select tdmisg134200.t_rqno,tdmisg134200.t_rqln from tdmisg134200 join ttdpur201200 on 
                tdmisg134200.t_rqno= ttdpur201200.t_rqno and tdmisg134200.t_cprj= '" + projId + "'  and ( UPPER(tdmisg134200.t_rqno) Like '%" + indent.ToUpper() + "%')";
            //@"select t_rqno, t_rqln from ttdpur201200  where t_cprj= '" + projId + "'  and ( UPPER(t_rqno) Like '%" + indent.ToUpper() + "%') ";
            return SqlHelper.ExecuteDataset(Con129, CommandType.Text, sPMDLdoc).Tables[0]; //
        }
        public DataTable GetIndent()
        {
            string sIndent = @"select (t_rqno + '-' +cast(t_pono as varchar) )  AS drpdwn,t_rqno from LN_IndentLines where t_cprj = '" + Project + "'and t_cspa = '" + Element + "'";
            return SqlHelper.ExecuteDataset(Con129, CommandType.Text, sIndent).Tables[0];
        }
        public DataTable GetHRM_Employees()
        {
            return SqlHelper.ExecuteDataset(Con, CommandType.Text, "select CardNo,EmployeeName  from HRM_Employees where ActiveState=1 order by EmployeeName asc").Tables[0];
        }
        public DataTable GetUserName(string UserCode)
        {
            return SqlHelper.ExecuteDataset(Con, CommandType.Text, "select CardNo,EmployeeName  from HRM_Employees where CardNo= '" + UserCode + "' order by EmployeeName asc").Tables[0];
        }
        public DataTable GetUser(string UserID)
        {
            return SqlHelper.ExecuteDataset(Con, CommandType.Text, "select CardNo,EmployeeName  from HRM_Employees where EmployeeName Like '" + UserID + "%' or CardNo Like '" + UserID + "%'").Tables[0];
        }
        public DataTable GetMAilID(string UserID)
        {
            return SqlHelper.ExecuteDataset(Con, CommandType.Text, "select CardNo,EmployeeName,EMailid  from HRM_Employees where CardNo= '" + UserID + "'").Tables[0];
        }
        public DataTable GetSupplierCode()
        {
            return SqlHelper.ExecuteDataset(Con129, CommandType.Text, "select( SupplierName +'('+ SupplierCode+')') as drpdwn,SupplierCode from LN_V_SupplierMaster").Tables[0];
        }
        public DataTable GetSupplier()
        {
            return SqlHelper.ExecuteDataset(Con129, CommandType.Text, "select SupplierCode,SupplierName,EMail from LN_V_SupplierMaster where UPPER(SupplierCode) Like '" + Supplier.ToUpper() + "%' or UPPER(SupplierName) Like '" + Supplier.ToUpper() + "%'").Tables[0];
        }
        public DataTable GetSupplierMailId(string SupplierId)
        {
            return SqlHelper.ExecuteDataset(Con129, CommandType.Text, "select SupplierCode,SupplierName,EMail from LN_V_SupplierMaster where SupplierCode = '" + SupplierId + "'").Tables[0];
        }
        public DataTable GetPath()
        {
            List<SqlParameter> sqlParamater = new List<SqlParameter>();
            //  sqlParamater.Add(new SqlParameter("@IndexValue", IndexValue));
            sqlParamater.Add(new SqlParameter("@AttachmentHandle", AttachmentHandle));
            return SqlHelper.ExecuteDataset(Con129, CommandType.StoredProcedure, "sp_GetAttachmentPath", sqlParamater.ToArray()).Tables[0];
        }
        public DataTable InsertAttachmentdata()
        {
            List<SqlParameter> sqlParamater = new List<SqlParameter>();
            sqlParamater.Add(new SqlParameter("@AttachmentHandle", AttachmentHandle));
            sqlParamater.Add(new SqlParameter("@IndexValue", IndexValue));
            sqlParamater.Add(new SqlParameter("@PurposeCode", PurposeCode));
            sqlParamater.Add(new SqlParameter("@FileName", FileName));
            sqlParamater.Add(new SqlParameter("@LibraryCode", LibraryCode));
            sqlParamater.Add(new SqlParameter("@AttachedBy", AttachedBy));
            return SqlHelper.ExecuteDataset(Con129, CommandType.StoredProcedure, "sp_InsertAttachment", sqlParamater.ToArray()).Tables[0];
        }
        public int UpdateBuyer_Manager()
        {
            return SqlHelper.ExecuteNonQuery(Con, CommandType.Text, "Update WF1_PreOrder set [Buyer]='" + Buyer + "',[Manager]='" + Manager + "',DateTime=getDate()  Where [WFID]='" + WFID + "'");
        }

        public DataTable GetNotesFromUSer()

        {
            string sUserNotes = @"Select Notes_RunningNo, NotesId, NN.NotesHandle,IndexValue,Title,Description,
                        NN.UserId, Created_Date, TableDescription,EMP.UserFullName as EmployeeName,
                        NN.SendEmailTo from Note_Notes NN
                        INNER JOIN Note_Handle NH ON NH.NotesHandle = NN.NotesHandle
                        INNER JOIN aspnet_users EMP ON EMP.LoginId= NN.USerId
                        Where NN.NotesHandle='" + NotesHandle + "' and NN.IndexValue='" + IndexValue + "' and ISNUMERIC(NN.UserId)!=1 order by Notes_RunningNo";
            return SqlHelper.ExecuteDataset(Con, CommandType.Text, sUserNotes).Tables[0];
        }

        public int GetSerialNumber()
        {
            string sGetSerialNumber = @" Select count(*) from ttpisg229200 where t_bohd = '" + BusinessObjectHandle + "' and t_indv = '" + IndexValue + "'";
            return (int)SqlHelper.ExecuteScalar(Con129, CommandType.Text, sGetSerialNumber);
        }
        public int GetDetailSerialNumber()
        {
            string sGetSerialNumber = @" Select count(*) from ttpisg230200 where t_bohd = '" + BusinessObjectHandle + "' and t_indv = '" + IndexValue + "' and t_srno = '" + SLNO_WFID + "'";
            return (int)SqlHelper.ExecuteScalar(Con129, CommandType.Text, sGetSerialNumber);
        }

        public int InsertPreOrderDatatoControlTower()
        {
            //TransactionDate
            string sInsertToControlTower = @" Insert into ttpisg229200 (t_trdt,t_bohd,t_indv,t_srno,t_stat,t_proj,t_elem,t_user,t_Refcntd, t_Refcntu) 
                                           values( '" + TransactionDate + "', '" + BusinessObjectHandle + "','" + IndexValue + "','" + SLNO_WFID + @"',
                                           '','" + Project + "','" + Element + "', '" + UserId + "',0,0  )";
            return SqlHelper.ExecuteNonQuery(Con129, CommandType.Text, sInsertToControlTower);
        }

        public void InsertPreOrderDatatoControlTowerChildTable()
        {

            string sInsertToControlTowerChildTable = @" Insert into ttpisg230200(t_trdt,t_bohd,t_indv,t_srno,t_dsno,t_stat,t_proj,
                                                        t_elem,t_dwno,t_pitc,t_wght,t_atcd,t_scup,t_acdt,t_acfh,
                                                       t_pper,t_lupd,t_Refcntd,t_Refcntu,t_numo,t_numq,t_numt,
                                                        t_numv,t_nutc)
                                           values( '" + TransactionDate + "', '" + BusinessObjectHandle + "','" + IndexValue + "','" + SLNO_WFID + @"',
                                            '" + DetailSerialCount + "','','" + Project + "','" + Element + "', '" + PMDLdocDesc + "','" + PartItemCount + @"',
                                            '" + PartItemWeight + "','','','','','','',0,0,0,0,0,0,0)";
             SqlHelper.ExecuteNonQuery(Con129, CommandType.Text, sInsertToControlTowerChildTable);

        }



        public DataTable GetPartItemCount_Weight()
        {
            string sPartItemCount_Weight = @" Select count(t_item) as PartItemCount, sum(t_wght) as PartItenWeight from tdmisg002200 where t_docn in ( '" + PMDLdocDesc + "')";
            return SqlHelper.ExecuteDataset(Con129, CommandType.Text, sPartItemCount_Weight).Tables[0];
        }

        public DataTable GetMultiPMDL(string ParentWFID)
        {
            string sMultiPMDL = @"Select distinct PMDLDocNo from WF1_PreOrderPMDL where WFID  = '" + ParentWFID + "'";
            return SqlHelper.ExecuteDataset(Con, CommandType.Text, sMultiPMDL).Tables[0];
        }

        public DataTable GetPreOrderDocumentTracking()
        {
            string sPreorderDocTracking = @"select PODT.[WFID],PODT.[Parent_WFID],WP.[Project],WP.[Element],WP.[SpecificationNo],
                                            WP.[SupplierCode],WP.[PMDLDocNo],WP.[SupplierName],WP.ReceiptNo,'' as ReceiptStatus,
                                            case when PODT.[Technical Specification Released] is null then ''
                                            else replace (convert(varchar, PODT.[Technical Specification Released], 105), '/','-')
                                            end
                                            as dt_tsr,
                                           case when PODT.[Created] is null then ''
                                            else replace (convert(varchar, PODT.[Created], 105), '/','-')
                                            end
                                            as dt_Created,
                                             case when PODT.[Enquiry in progress] is null then ''
                                             else  replace (convert(varchar, PODT.[Enquiry in progress], 105), '/','-')
                                             end as dt_eip,
                                             case when PODT.[Enquiry Raised] is null then ''
                                             else replace (convert(varchar, PODT.[Enquiry Raised], 105), '/','-')
                                             end as dt_er,
                                             case when PODT.[Technical offer Received]  is null then ''
                                             else replace (convert(varchar, PODT.[Technical offer Received] , 105), '/','-')
                                             end as dt_tor,
                                             PODT.[Commercial offer Received] as dt_cor,
                                            case when PODT.[Commercial offer Finalized] is null then ''
                                            else  replace (convert(varchar, PODT.[Commercial offer Finalized]  , 105), '/','-') end as dt_cof,
                                             WP.[WF_Status],(Select EmployeeName FROM HRM_Employees where CardNo =WP.Buyer)as BuyerName,
                                            (Select EmployeeName FROM HRM_Employees where CardNo =WP.Manager)as ManagerName
                                            from PivotPreOrderDocTracking_DateDetails PODT join WF1_PreOrder WP on PODT.WFID= WP.WFID and PODT.Parent_WFID= WP.Parent_WFID
                                            join HRM_Employees HRM on WP.UserId= HRM.CardNo 
                                            order by 
                                             case
                                             when PODT.[Parent_WFID] =0
                                             then  PODT.[WFID] else 
                                             PODT.[Parent_WFID]
                                             end,PODT.[WFID] ";
                            return SqlHelper.ExecuteDataset(Con, CommandType.Text, sPreorderDocTracking).Tables[0];
        }


        public DataTable GetPreOrderDocumentTracking_byProject_Buyer_Date_PMDL()
        {
            string Project_Buyer_Date_PMDL = @"select* from
                                            (
                                            select PODT.[WFID] as WFID, PODT.[Parent_WFID] as Parent_WFID, PODT.[Project] as Project, PODT.[Element], PODT.[SpecificationNo],
                                            WP.[SupplierCode], WP.ReceiptNo, '' as ReceiptStatus,
                                            case when PODT.[PMDLDocNo] is null then ''
                                            else PODT.[PMDLDocNo]
                                            end
                                            as [PMDLDocNo],
                                            WP.[SupplierName], PODT.[Buyer] as Buyer,
                                            case when PODT.[Technical Specification Released] is null then ''
                                            else replace(convert(varchar, PODT.[Technical Specification Released], 105), '/', '-')
                                            end
                                            as dt_tsr,
                                            case when PODT.[Created] is null then ''
                                            else replace(convert(varchar, PODT.[Created], 105), '/', '-')
                                            end
                                            as dt_Created,
                                             case when PODT.[Enquiry in progress] is null then ''
                                             else  replace(convert(varchar, PODT.[Enquiry in progress], 105), '/', '-')
                                             end as dt_eip,
                                             case when PODT.[Enquiry Raised] is null then ''
                                             else replace(convert(varchar, PODT.[Enquiry Raised], 105), '/', '-')
                                             end as dt_er,
                                             case when PODT.[Technical offer Received] is null then ''
                                             else replace(convert(varchar, PODT.[Technical offer Received], 105), '/', '-')
                                             end as dt_tor,
                                             PODT.[Commercial offer Received] as dt_cor,
                                            case when PODT.[Commercial offer Finalized] is null then ''
                                            else  replace(convert(varchar, PODT.[Commercial offer Finalized], 105), '/', '-') end as dt_cof,
                                             WP.[WF_Status], (Select EmployeeName FROM HRM_Employees where CardNo = PODT.Buyer)as BuyerName,
                                            (Select EmployeeName FROM HRM_Employees where CardNo = PODT.Manager)as ManagerName
                                            from PivotPreOrderDocTracking PODT join WF1_PreOrder WP on PODT.WFID = WP.WFID and PODT.Parent_WFID = WP.Parent_WFID
                                            join HRM_Employees HRM on WP.UserId = HRM.CardNo
                                           
                                              ) as x
                                            where
                                            x.dt_tsr between '" + DateFrom + "' and '" + DateTo + @"'
                                            and x.[Buyer]
                                            between '" + BuyerFrom + "' and '" + BuyerTo + @"' 
                                            and x.Project between '" + ProjectFrom + "'  and '" + ProjectTo + @"' 
                                            and x.[PMDLDocNo] between '" + PMDLFrom + "'  and '" + PMDLTo + @"'
                                            order by 
                                             case when x.Parent_WFID =0 then  x.WFID else 
                                            x.Parent_WFID end,x.[WFID] ";
            return SqlHelper.ExecuteDataset(Con, CommandType.Text, Project_Buyer_Date_PMDL).Tables[0];
        }

        public DataTable GetPreOrderDocumentTracking_byProject_Buyer_PMDL()
        {
            string Project_Buyer_Date_PMDL = @"select* from
                                            (
                                            select PODT.[WFID] as WFID, PODT.[Parent_WFID] as Parent_WFID, WP.[Project] as Project, WP.[Element], WP.[SpecificationNo],
                                            WP.[SupplierCode], WP.ReceiptNo, '' as ReceiptStatus,
                                            case when WP.[PMDLDocNo] is null then ''
                                            else WP.[PMDLDocNo]
                                            end
                                            as [PMDLDocNo],
                                            WP.[SupplierName], WP.[Buyer] as Buyer,
                                            case when PODT.[Technical Specification Released] is null then ''
                                            else replace(convert(varchar, PODT.[Technical Specification Released], 105), '/', '-')
                                            end
                                            as dt_tsr,
                                            case when PODT.[Created] is null then ''
                                            else replace(convert(varchar, PODT.[Created], 105), '/', '-')
                                            end
                                            as dt_Created,
                                             case when PODT.[Enquiry in progress] is null then ''
                                             else  replace(convert(varchar, PODT.[Enquiry in progress], 105), '/', '-')
                                             end as dt_eip,
                                             case when PODT.[Enquiry Raised] is null then ''
                                             else replace(convert(varchar, PODT.[Enquiry Raised], 105), '/', '-')
                                             end as dt_er,
                                             case when PODT.[Technical offer Received] is null then ''
                                             else replace(convert(varchar, PODT.[Technical offer Received], 105), '/', '-')
                                             end as dt_tor,
                                             PODT.[Commercial offer Received] as dt_cor,
                                            case when PODT.[Commercial offer Finalized] is null then ''
                                            else  replace(convert(varchar, PODT.[Commercial offer Finalized], 105), '/', '-') end as dt_cof,
                                             WP.[WF_Status], (Select EmployeeName FROM HRM_Employees where CardNo = WP.Buyer)as BuyerName,
                                            (Select EmployeeName FROM HRM_Employees where CardNo = WP.Manager)as ManagerName
                                            from PivotPreOrderDocTracking_DateDetails PODT join WF1_PreOrder WP on PODT.WFID = WP.WFID and PODT.Parent_WFID = WP.Parent_WFID
                                            join HRM_Employees HRM on WP.UserId = HRM.CardNo
                                           
                                              ) as x
                                            where
                                            x.[Buyer]
                                            between '" + BuyerFrom + "' and '" + BuyerTo + @"' 
                                            and x.Project between '" + ProjectFrom + "'  and '" + ProjectTo + @"' 
                                            and x.[PMDLDocNo] between '" + PMDLFrom + "'  and '" + PMDLTo + @"'
                                            order by 
                                             case when x.Parent_WFID =0 then  x.WFID else 
                                            x.Parent_WFID end,x.[WFID] ";
            return SqlHelper.ExecuteDataset(Con, CommandType.Text, Project_Buyer_Date_PMDL).Tables[0];
        }
        public DataTable GetReceiptNo()
        { 
             string sReceiptNo= @"select distinct ReceiptNo, PMDLDocNo from WF1_PreOrder where ReceiptNo is not null and ReceiptNo <> ''  and Parent_WFID = '" + WFID + @"'";
             return SqlHelper.ExecuteDataset(Con, CommandType.Text, sReceiptNo).Tables[0];
        }


        //public DataTable GetReceiptDetails(string sReceiptNo,string PMDLnum)
        //{
        //    string sReceiptDetails = @"Select t134.t_rcno,t134.t_revn,t134.t_date,t134.t_cprj,t134.t_nama as VendorName,
        //                                t134.t_item,tcb1.t_dsca,
        //                                t134.t_stat,
        //                                case 
        //                                when t134.t_stat='1' then 'Submitted'
        //                                when t134.t_stat='2' then'Document linking'
        //                                when t134.t_stat='3' then 'Under Evaluation'
        //                                when t134.t_stat='4' then'Comment Submitted'
        //                                when t134.t_stat='5' then 'Technically Cleared'
        //                                when t134.t_stat='6' then'Transmittal Issued'
        //                                when t134.t_stat='7' then 'Superseded'
        //                                when t134.t_stat='8' then'Closed'
        //                                end as [Doc_Status]
        //                                ,t134.t_user,tcc1.t_nama as Name, td3.t_rqno,td3.t_pono,td2.t_rdat
        //                                from tdmisg134200 t134
        //                                    join ttccom001200 tcc1 on t134.t_user = tcc1.t_emno
        //                                join ttdisg003200 td3 on td3.t_docn='" + PMDLnum + @"'
        //                                join ttcibd001200 tcb1 on t134.t_item =tcb1.t_item
        //                                join ttdpur200200 td2 on td2.t_rqno=td3.t_rqno
        //                                where t134.t_rcno ='" + sReceiptNo + "'";
        //    return SqlHelper.ExecuteDataset(Con129, CommandType.Text, sReceiptDetails).Tables[0];
        //}


        public DataTable GetReceiptDetails(string sReceiptNo)
        {
            List<SqlParameter> sqlParamater = new List<SqlParameter>();
            sqlParamater.Add(new SqlParameter("@ReceiptNo", sReceiptNo));
            return SqlHelper.ExecuteDataset(Con129, CommandType.StoredProcedure, "IDMSReceiptDetails_DocumentTracking", sqlParamater.ToArray()).Tables[0];
        }
        
        public string GetReceiptStatus(string sReceiptNo)
        {
            string sReceiptStatus = "";
            try
            {
                 sReceiptStatus = @"Select case 
                                        when t_stat='1' then 'Submitted'
                                        when t_stat='2' then'Document linking'
                                        when t_stat='3' then 'Under Evaluation'
                                        when t_stat='4' then'Comment Submitted'
                                        when t_stat='5' then 'Technically Cleared'
                                        when t_stat='6' then'Transmittal Issued'
                                        when t_stat='7' then 'Superseded'
                                        when t_stat='8' then'Closed'
                                        else ''
                                        end as [Doc_Status]
                                        from tdmisg134200 where t_rcno ='" + sReceiptNo + "'";

                var outputParam= SqlHelper.ExecuteScalar(Con129, CommandType.Text, sReceiptStatus).ToString();
                if (!Convert.IsDBNull(outputParam))
                return (outputParam).ToString();
               else return "";
            }
            catch (Exception ex)
            {
                string sErrorMessage = ex.Message;
                string sStackTrace = ex.StackTrace;
                string sErrorQuery=sReceiptStatus;
                return "";
            }
        }
        //public DataTable GetReceiptDetails(string sReceiptNo)   IDMSReceiptDetails_DocumentTracking
        //{
        //    string sReceiptDetails = @"Select t134.t_rcno,t134.t_revn,t134.t_date,t134.t_cprj,t134.t_nama as VendorName,
        //                                t134.t_item,tcb1.t_dsca,
        //                                t134.t_stat,
        //                                case 
        //                                when t134.t_stat='1' then 'Submitted'
        //                                when t134.t_stat='2' then'Document linking'
        //                                when t134.t_stat='3' then 'Under Evaluation'
        //                                when t134.t_stat='4' then'Comment Submitted'
        //                                when t134.t_stat='5' then 'Technically Cleared'
        //                                when t134.t_stat='6' then'Transmittal Issued'
        //                                when t134.t_stat='7' then 'Superseded'
        //                                when t134.t_stat='8' then'Closed'
        //                                end as [Doc_Status]
        //                                ,t134.t_user,tcc1.t_nama as Name, t134.t_rqno,t134.t_rqln
        //                                from tdmisg134200 t134
        //                                    join ttccom001200 tcc1 on t134.t_user = tcc1.t_emno
        //                                join ttcibd001200 tcb1 on t134.t_item =tcb1.t_item
        //                                where t134.t_rcno ='" + sReceiptNo + "'";
        //    return SqlHelper.ExecuteDataset(Con129, CommandType.Text, sReceiptDetails).Tables[0];
        //}




        //public DataTable GetRaisedEnquiryDate( )
        //{
        //    string sRaisedEnquiryDate = @"select Convert(varchar(20), t_rfqd,105) as RaisedDate,t_iref from tdmisg140200 where t_docn='" + PMDLdocDesc + "' and t_cprj= '" + Project + "' and t_orgn='ISG'";
        //    return SqlHelper.ExecuteDataset(Con129, CommandType.Text, sRaisedEnquiryDate).Tables[0];
        //}
        ////GetTechOfferReceiveDate
        //public DataTable GetTechOfferReceiveDate()
        //{
        //    string sTechOfferReceiveDate = @"select Convert(varchar(20), t_ofdt,105) as OfferReceiveDate,t_iref from tdmisg140200 where t_docn='" + PMDLdocDesc + "' and t_cprj= '" + Project + "' and t_orgn='ISG'";
        //    return SqlHelper.ExecuteDataset(Con129, CommandType.Text, sTechOfferReceiveDate).Tables[0];
        //}
        ////GetTechnoCommercialDate- Do not have to select technocommercial date only Itemref
        //public DataTable GetTechnoCommercialItemReference()
        //{
        //    string sTechOfferReceiveDate = @"select Convert(varchar(20), t_cmfd,105) as TechnoCommercialDate, t_iref from tdmisg140200 where t_docn='" + PMDLdocDesc + "' and t_cprj= '" + Project + "' and t_orgn='ISG'";
        //    return SqlHelper.ExecuteDataset(Con129, CommandType.Text, sTechOfferReceiveDate).Tables[0];
        //}
        //public void UpdateRaisedEnquiryDate(string CurrentDate)
        //{
        //    string sUpdateRaiseEnquiryDate = @"update tdmisg140200 set t_rfqd= '"+ CurrentDate + "'  where t_docn='" + PMDLdocDesc + "' and t_cprj= '" + Project + "' and t_orgn='ISG'";

        //    SqlHelper.ExecuteNonQuery(Con129, CommandType.Text, sUpdateRaiseEnquiryDate);
        //}
        ////UpdateOfferReceiveDate
        //public void UpdateOfferReceiveDate(string CurrentDate)
        //{
        //    string sUpdateOfferReceiveDate = @"update tdmisg140200 set t_ofdt= '" + CurrentDate + "'  where t_docn='" + PMDLdocDesc + "' and t_cprj= '" + Project + "' and t_orgn='ISG'";

        //    SqlHelper.ExecuteNonQuery(Con129, CommandType.Text, sUpdateOfferReceiveDate);
        //}

        ////UpdateTechnoCommercialDate
        //public void UpdateTechnoCommercialNegotiationDate(string CurrentDate)
        //{
        //    string sUpdateTechnoCommercialNegotiationDate = @"update tdmisg140200 set t_cmfd= '" + CurrentDate + "'  where t_docn='" + PMDLdocDesc + "'";

        //    SqlHelper.ExecuteNonQuery(Con129, CommandType.Text, sUpdateTechnoCommercialNegotiationDate);
        //}
        ////UpdateTechnoCommercialDate- Do not have to update TechnoCommercial Date
        ////public void UpdateTechnoCommercialDate(string CurrentDate)
        ////{
        ////    string sUpdateOfferReceiveDate = @"update tdmisg140200 set t_ofdt= '" + CurrentDate + "'  where t_docn='" + PMDLdocDesc + "'";

        ////    SqlHelper.ExecuteNonQuery(Con129, CommandType.Text, sUpdateOfferReceiveDate);
        ////}

        //public string GetItemRefType()
        //{
        //    string sGetItemRefType = @"select t_icls from ttpisg239200 where t_iref='" + ItemReference + "' and t_cprj= '" + Project + "' ";
        //    var outputParam= SqlHelper.ExecuteScalar(Con129, CommandType.Text, sGetItemRefType);
        //    if (!(outputParam ==null))
        //        return (outputParam).ToString();
        //    else return "";
        //}
        //public double GeTotalDrawingCount()
        //{
        //    string sTotalDrawing = @"select count(*) from tdmisg140200 where t_iref='" + ItemReference+ "' and t_cprj= '" + Project + "' and t_orgn='ISG'";
        //    var outputParam = (SqlHelper.ExecuteScalar(Con129, CommandType.Text, sTotalDrawing));
        //    if (!Convert.IsDBNull(outputParam))
        //        return Convert.ToDouble(outputParam);
        //    else return 0;
        //}

        //public double GeRFQraisedDrawingCount()
        //{
        //    string sRFQraisedDrawing = @"select count(*) from tdmisg140200 where t_iref='" + ItemReference + "' and t_cprj= '"+Project+"' and t_rfqd not in ('1970-01-01 00:00:00.000','1900-01-01 00:00:00.000') and t_orgn='ISG'";
        //    var outputParam=(SqlHelper.ExecuteScalar(Con129, CommandType.Text, sRFQraisedDrawing));
        //    if (!Convert.IsDBNull(outputParam))
        //        return Convert.ToDouble(outputParam);
        //    else return 0;
        //}
        //public double GetRFQRaisedDrawingWeight()
        //{
        //    string sRFQraisedDrawingWeight = @"select sum(t_dpwt) from tdmisg140200 where t_iref='" + ItemReference + "' and t_cprj= '" + Project + "' and t_rfqd not in ('1970-01-01 00:00:00.000','1900-01-01 00:00:00.000') and t_orgn='ISG'";
        //    var outputParam = (SqlHelper.ExecuteScalar(Con129, CommandType.Text, sRFQraisedDrawingWeight));
        //    if (!Convert.IsDBNull(outputParam))
        //        return Convert.ToDouble(outputParam);
        //    else return 0;
        //}
        //public double GetTechOfferReceivedDrawingWeight()
        //{
        //    string sGetTechOfferReceivedDrawingWeight = @"select sum(t_dpwt) from tdmisg140200 where t_iref='" + ItemReference + "' and t_cprj= '" + Project + "' and t_ofdt not in ('1970-01-01 00:00:00.000','1900-01-01 00:00:00.000') and t_orgn='ISG'";
        //    var outputParam = (SqlHelper.ExecuteScalar(Con129, CommandType.Text, sGetTechOfferReceivedDrawingWeight));
        //    if (!Convert.IsDBNull(outputParam))
        //        return Convert.ToDouble(outputParam);
        //    else return 0;
        //}
        //public double GetCuurentRFQ_OfferReceivedDrawingWeight( string PMDLDocNum)
        //{
        //    string sGetTechOfferReceivedDrawingWeight = @"select sum(t_dpwt) from tdmisg140200 where t_iref='" + ItemReference + "' and t_cprj= '" + Project + "' and t_ofdt not in ('1970-01-01 00:00:00.000','1900-01-01 00:00:00.000') and t_orgn='ISG' and t_docn in (" + PMDLDocNum + @") ";
        //    var outputParam = (SqlHelper.ExecuteScalar(Con129, CommandType.Text, sGetTechOfferReceivedDrawingWeight));
        //    if (!Convert.IsDBNull(outputParam))
        //        return Convert.ToDouble(outputParam);
        //    else return 0;
        //}
        //public double GetCuurentRFQComNegotiation_OfferReceivedDrawingWeight(string PMDLDocNum)
        //{
        //    string sGetTechOfferReceivedDrawingWeight = @"select sum(t_dpwt) from tdmisg140200 where t_iref='" + ItemReference + "' and t_cprj= '" + Project + "' and t_cmfd not in ('1970-01-01 00:00:00.000','1900-01-01 00:00:00.000') and t_orgn='ISG' and t_docn in (" + PMDLDocNum + @")  ";
        //    var outputParam = (SqlHelper.ExecuteScalar(Con129, CommandType.Text, sGetTechOfferReceivedDrawingWeight));
        //    if (!Convert.IsDBNull(outputParam))
        //        return Convert.ToDouble(outputParam);
        //    else return 0;
        //}
        //public double GetTechnoComNegotiationWeight( string PMDLDocNum)
        //{
        //    string sGetTechnoComNegotiationWeight = @"select sum(t_dpwt) from tdmisg140200 where t_iref='" + ItemReference + "' and t_cprj= '" + Project + "' and t_cmfd not in ('1970-01-01 00:00:00.000','1900-01-01 00:00:00.000') and t_orgn='ISG' and t_docn in (" + PMDLDocNum + @")  ";
        //    var outputParam = (SqlHelper.ExecuteScalar(Con129, CommandType.Text, sGetTechnoComNegotiationWeight));
        //    if (!Convert.IsDBNull(outputParam))
        //        return Convert.ToDouble(outputParam);
        //    else return 0;
        //}
        //public double GetTotalDrawingWeight()
        //{
        //    string sTotalDrawingWeight = @"select sum(t_pwgt) from ttpisg239200 where t_iref='" + ItemReference + "' and t_cprj= '" + Project + "'";
        //   var outputParam=SqlHelper.ExecuteScalar(Con129, CommandType.Text, sTotalDrawingWeight);
        //   if( !Convert.IsDBNull(outputParam))
        //        return Convert.ToDouble(outputParam);
        //    else return 0;
        //}
        ////GeTechOfferReceievedDrawingCount GetTotalDrawingWeight
        //public double GeTechOfferReceievedDrawingCount( string PMDLDocNo)
        //{
        //    string sTechOfferReceievedDrawingCount = @"select count(*) from tdmisg140200 where t_iref='" + ItemReference + "' and t_cprj= '" + Project + "' and t_ofdt not in ('1970-01-01 00:00:00.000','1900-01-01 00:00:00.000') and t_orgn='ISG' and t_docn in (" + PMDLDocNo + ")";
        //    var outputParam = (SqlHelper.ExecuteScalar(Con129, CommandType.Text, sTechOfferReceievedDrawingCount));
        //    if (!Convert.IsDBNull(outputParam))
        //        return Convert.ToDouble(outputParam);
        //    else return 0;
        //}

        //public double GetTechnoComNegotiationrawingCount( string PMDLDocNo)
        //{
        //    string sGetTechnoComNegotiationrawingCount = @"select count(*) from tdmisg140200 where t_iref='" + ItemReference + "' and t_cprj= '" + Project + "' and t_cmfd not in ('1970-01-01 00:00:00.000','1900-01-01 00:00:00.000') and t_orgn='ISG' and t_docn in (" + PMDLDocNo + ")";
        //    var outputParam = (SqlHelper.ExecuteScalar(Con129, CommandType.Text, sGetTechnoComNegotiationrawingCount));
        //    if (!Convert.IsDBNull(outputParam))
        //        return Convert.ToDouble(outputParam);
        //    else return 0;
        //}
        //public double GetTotalInquirySentCount()
        //{
        //    string sGetTotalInquirySentCount = @"select count(*) from tdmisg140200 where t_iref='" + ItemReference + "' and t_cprj= '" + Project + "' and t_orgn='ISG'";
        //    var outputParam = (SqlHelper.ExecuteScalar(Con129, CommandType.Text, sGetTotalInquirySentCount));
        //    if (!Convert.IsDBNull(outputParam))
        //        return Convert.ToDouble(outputParam);
        //    else return 0;

        //}
        ////GetTotalInquirySentCount
        //public void UpdateDrawingpercentage(double Drawingpercentage)
        //{
        //    string sUpdateDrawingpercentage = @"update ttpisg220200 set t_cpgv='" + Drawingpercentage + "' where t_sub1='" + ItemReference + "' and t_bohd='"+ BusinessObjectHandle + "' and t_cprj='"+Project+"' ";
        //    SqlHelper.ExecuteNonQuery(Con129, CommandType.Text, sUpdateDrawingpercentage);
        //}
        ////UpdateTechOfferReceivedDrawingpercentage
        //public void UpdateTechOfferReceivedDrawingpercentage(double Drawingpercentage)
        //{
        //    string sUpdateTechOfferReceivedDrawingpercentage = @"update ttpisg220200 set t_cpgv='" + Drawingpercentage + "' where t_sub1='" + ItemReference + "' and t_bohd='" + BusinessObjectHandle + "'  and t_cprj='" + Project + "' ";
        //    SqlHelper.ExecuteNonQuery(Con129, CommandType.Text, sUpdateTechOfferReceivedDrawingpercentage);
        //}
        ////GetTotalWeight
        //public double GetTotalWeight(string PMDLDoc)
        //{


        //    string sGetTotalInquirySentCount = @"select sum(a.t_wght)from tdmisg002200 a
        //                                         join tdmisg140200 b on a.t_docn = b.t_docn
        //                                         where b.t_docn in (" + PMDLDoc + @") and
        //                                        b.t_iref ='" + ItemReference + "' and b.t_cprj = '" + Project + "' and b.t_orgn='ISG'";
        //    var outputParam = (SqlHelper.ExecuteScalar(Con129, CommandType.Text, sGetTotalInquirySentCount));
        //    if (!Convert.IsDBNull(outputParam))
        //        return Convert.ToDouble(outputParam);
        //    else return 0;
        //}

        //public double GetTotalWeight()
        //{
        //    string sGetTotalInquirySentCount = @"select sum(t_pwgt) from ttpisg239200 where t_iref  ='" + ItemReference + "' and t_cprj= '" + Project + "' ";
        //    var outputParam = (SqlHelper.ExecuteScalar(Con129, CommandType.Text, sGetTotalInquirySentCount));
        //    if (!Convert.IsDBNull(outputParam))
        //        return Convert.ToDouble(outputParam);
        //    else return 0;
        //}

        ////UpdateTechnoCommercialpercentage
        //public void UpdateTechnoCommercialpercentage(double percentage)
        //{
        //    string sUpdateTechnoCommercialpercentage = @"update ttpisg220200 set t_cpgv='" + percentage + "' where t_sub1='" + ItemReference + "' and t_bohd='" + BusinessObjectHandle + "'  and t_cprj='" + Project + "' ";
        //    SqlHelper.ExecuteNonQuery(Con129, CommandType.Text, sUpdateTechnoCommercialpercentage);
        //}
        ////GeRFQraisedDrawingpercentage UpdateTechnicalClearancepercentage

      
        //public double GeRFQraisedDrawingpercentage()
        //{
        //    string sGeRFQraisedDrawingpercentage = @"select t_cpgv from ttpisg220200 where t_sub1='" + ItemReference + "' and t_bohd='CT_RFQRAISED'  and t_cprj='" + Project + "'";
        //    var outputParam = (SqlHelper.ExecuteScalar(Con129, CommandType.Text, sGeRFQraisedDrawingpercentage));
        //    if (!Convert.IsDBNull(outputParam))
        //        return Convert.ToDouble(outputParam);
        //    else return 0;
        //}
       

        ////GetTechnicalvettingpercentage GetTechOfferReceiveCount
        //public double GetTechnicalvettingpercentage()
        //{
        //    string sGetTechnicalvettingpercentage = @"Select t_cpgv from ttpisg220200 where t_sub1='" + ItemReference + "' and t_bohd='CT_PREORDERTECHCLEAR'  and t_cprj='" + Project + "'";
        //    var outputParam = (SqlHelper.ExecuteScalar(Con129, CommandType.Text, sGetTechnicalvettingpercentage));
        //    if (!Convert.IsDBNull(outputParam))
        //        return Convert.ToDouble(outputParam);
        //    else return 0;
        //}
        //public DateTime GetMinRFQDate()
        //{
        //    string sGetMinRFQDate = @"select Min(t_rfqd) from tdmisg140200 where t_iref='"+ItemReference+ "' and t_cprj= '" + Project + "' and  t_rfqd not in ('1970-01-01 00:00:00.000','1900-01-01 00:00:00.000') and t_orgn='ISG'";
        //    var outputParam =SqlHelper.ExecuteScalar(Con129, CommandType.Text, sGetMinRFQDate);
        //    if (!(outputParam is DBNull))
        //        return Convert.ToDateTime(outputParam);
        //    else return default(DateTime);

        //}
        //public DateTime GetMinOfferReceieveDate()
        //{
        //    string sGetMinOfferReceieveDate = @"select Min(t_ofdt) from tdmisg140200 where t_iref='" + ItemReference + "' and t_cprj= '" + Project + "' and  t_ofdt not in ('1970-01-01 00:00:00.000','1900-01-01 00:00:00.000') and t_orgn='ISG'";
        //    var outputParam = SqlHelper.ExecuteScalar(Con129, CommandType.Text, sGetMinOfferReceieveDate);
        //    if (!(outputParam is DBNull))
        //        return Convert.ToDateTime(outputParam);
        //    else return default(DateTime);

        //}
        //public DateTime GetMinTechnoCommecrialDate()
        //{
        //    string sGetMinTechnoCommecrialDate = @"select Min(t_cmfd) from tdmisg140200 where t_iref='" + ItemReference + "' and t_cprj= '" + Project + "' and  t_cmfd not in ('1970-01-01 00:00:00.000','1900-01-01 00:00:00.000') and t_orgn='ISG'";
        //    var outputParam = SqlHelper.ExecuteScalar(Con129, CommandType.Text, sGetMinTechnoCommecrialDate);
        //    if (!(outputParam is DBNull))
        //        return Convert.ToDateTime(outputParam);
        //    else return default(DateTime);

        //}
        //public void UpdateActualStartDate(string dt)
        //{
        //    string sUpdateActualStartDate = @"update ttpisg220200 set t_acsd=cast('" + dt + "' as DATETIME) where t_sub1='" + ItemReference + "' and t_bohd='" + BusinessObjectHandle + "'  and t_cprj='" + Project + "' ";
        //    SqlHelper.ExecuteNonQuery(Con129, CommandType.Text, sUpdateActualStartDate);
        //}
        //public void UpdateActualStartDate_tor(string dt)
        //{
        //    string sUpdateActualStartDate_tor = @"update ttpisg220200 set t_acsd=cast('" + dt + "' as DATETIME) where t_sub1='" + ItemReference + "' and t_bohd='CT_RFQOFFERECEIVED'  and t_cprj='" + Project + "' ";
        //    SqlHelper.ExecuteNonQuery(Con129, CommandType.Text, sUpdateActualStartDate_tor);
        //}
        //public int GetEnquiryRaisedCount(string PMDL)
        //{
        //    //need to ask if PMDLDocNo also to be added in where clause or not  Change 22/08/18 Sagar
        //    string sGetCount = @"select count(*) from WF1_PreOrder where WF_Status in('Enquiry Raised','Technical offer Received','Enquiry For Techno Commercial Negotiation Completed') and PMDLDocNo='" + PMDL + "'";
        //    return Convert.ToInt32(SqlHelper.ExecuteScalar(Con, CommandType.Text, sGetCount));
        //}
        ////GetTechOfferReceiveDateCount
        //public int GetTechOfferReceiveDateCount(string PMDL)
        //{
        //    //need to ask if PMDLDocNo also to be added in where clause or not  Change 22/08/18 Sagar
        //    string sGetCount = @"select count(*) from WF1_PreOrder where WF_Status in('Technical offer Received','Enquiry For Techno Commercial Negotiation Completed') and PMDLDocNo='" + PMDL + "'";
        //    return Convert.ToInt32(SqlHelper.ExecuteScalar(Con, CommandType.Text, sGetCount));
        //}
        //public void UpdateRFQCount(int nCount)
        //{
        //    string sUpdateRFQCount = @"update ttpisg220200 set t_numv='" + nCount + "' where t_sub1='" + ItemReference + "' and t_bohd='CT_RFQRAISED'  and t_cprj='" + Project + "'";
        //    SqlHelper.ExecuteNonQuery(Con129, CommandType.Text, sUpdateRFQCount);
        //}
        ////UpdateTechOfferReceiveCount
        //public void UpdateTechOfferReceiveCount(int nCount)
        //{
        //    string sUpdateTechOfferReceiveCount = @"update ttpisg220200 set t_numo='" + nCount + "' where t_sub1='" + ItemReference + "' and t_bohd='CT_RFQOFFERECEIVED'  and t_cprj='" + Project + "'";
        //    SqlHelper.ExecuteNonQuery(Con129, CommandType.Text, sUpdateTechOfferReceiveCount);
        //}
        ////UpdateTechnoCommercialCount
        //public void UpdateTechnoCommercialCount(double nCount)
        //{
        //    string sUpdateTechnoCommercialCount = @"update ttpisg220200 set t_nutc='" + nCount + "' where t_sub1='" + ItemReference + "' and  t_bohd='CT_RFQCOMMERCIALFINALISED'  and t_cprj='" + Project + "' ";
        //    SqlHelper.ExecuteNonQuery(Con129, CommandType.Text, sUpdateTechnoCommercialCount);
        //}
        //public DataTable GetPMDLFromItemRef()
        //{
        //    string sGetPMDLFromItemRef = @"select distinct t_docn from tdmisg140200 where t_iref='" + ItemReference + "'  and t_orgn='ISG'  and t_cprj='" + Project + "'";
        //    return SqlHelper.ExecuteDataset(Con129, CommandType.Text, sGetPMDLFromItemRef).Tables[0];

        //}
        //public DataTable GetPMDLFromItemRefForOfferReceived()
        //{
        //    string sGetPMDLFromItemRef = @"select distinct t_docn from tdmisg140200 where t_iref='" + ItemReference + "'  and t_orgn='ISG'  and t_cprj='" + Project + "' and t_ofdt not in ('1970-01-01 00:00:00.000','1900-01-01 00:00:00.000')";
        //    return SqlHelper.ExecuteDataset(Con129, CommandType.Text, sGetPMDLFromItemRef).Tables[0];

        //}
        //PMDLDocForTechnoCommNegotiation
        //public DataTable GetPMDLDocForTechnoCommNegotiation()
        //{
        //    string sGetPMDLFromItemRef = @"select distinct t_docn from tdmisg140200 where t_iref='" + ItemReference + "'  and t_orgn='ISG'  and t_cprj='" + Project + "' and t_cmfd not in ('1970-01-01 00:00:00.000','1900-01-01 00:00:00.000')";
        //    return SqlHelper.ExecuteDataset(Con129, CommandType.Text, sGetPMDLFromItemRef).Tables[0];

        //}
        //public int GetTotalChildRecordCount(string PMDLDoc)
        //{
        //    string sGetTotalChildRecordCount = @"select count(distinct WF1_PreOrder.WFId) from WF1_PreOrder inner join WF1_PreOrderPMDL
        //                                        on WF1_PreOrder.Parent_WFID = WF1_PreOrderPMDL.WFID
        //                                        and WF1_PreOrderPMDL.PMDLDocNo in(" + PMDLDoc + @")
        //                                        and WF1_PreOrder.Parent_WFID <> 0
        //                                        and WF1_PreOrder.WF_Status in('Enquiry Raised', 
        //                                       'Technical offer Received','Enquiry For Techno Commercial Negotiation Completed') and WF1_PreOrder.Parent_WFID='" + Parent_WFID + "'";
        //    //@"select count(*) from WF1_PreOrder_History where PMDLDocNo='"+ PMDLDoc + "'and Parent_WFID != '0'and WF_Status in ('Enquiry Raised','Technical offer Received')";
        //    return Convert.ToInt32(SqlHelper.ExecuteScalar(Con, CommandType.Text, sGetTotalChildRecordCount));
        //}
        //GetTotalChildRecordCount_AllOfferReceived
        //public int GetTotalChildRecordCount_AllOfferReceived(string PMDLDoc)
        //{
        //    string sGetTotalChildRecordCount_AllOfferReceived = @"select count(distinct WF1_PreOrder.WFId) from WF1_PreOrder inner join WF1_PreOrderPMDL
        //                                        on WF1_PreOrder.Parent_WFID = WF1_PreOrderPMDL.WFID
        //                                        and WF1_PreOrderPMDL.PMDLDocNo in(" + PMDLDoc + @")
        //                                        and WF1_PreOrder.Parent_WFID <> 0
        //                                        and WF1_PreOrder.WF_Status in( 
        //                                       'Technical offer Received','Enquiry For Techno Commercial Negotiation Completed') and WF1_PreOrder.Parent_WFID='" + Parent_WFID + "'";
        //    //@"select count(*) from WF1_PreOrder_History where PMDLDocNo='"+ PMDLDoc + "'and Parent_WFID != '0'and WF_Status in ('Enquiry Raised','Technical offer Received')";
        //    return Convert.ToInt32(SqlHelper.ExecuteScalar(Con, CommandType.Text, sGetTotalChildRecordCount_AllOfferReceived));
        //}
        //public int AllOfferReceieved_ChildRecordCount(string PMDLDoc)
        //{
        //     string sGetTotalChildRecordCount = @"select count(distinct WF1_PreOrder.WFId) from WF1_PreOrder inner join WF1_PreOrderPMDL
        //                                        on WF1_PreOrder.Parent_WFID = WF1_PreOrderPMDL.WFID
        //                                        and WF1_PreOrderPMDL.PMDLDocNo in(" + PMDLDoc + @")
        //                                        and WF1_PreOrder.Parent_WFID <> 0
        //                                      and WF1_PreOrder.WF_Status in( 'Technical offer Received','Enquiry For Techno Commercial Negotiation Completed')
        //                                      and WF1_PreOrder.Parent_WFID='" + Parent_WFID + "'";
        //    //@"select count(*) from WF1_PreOrder_History where PMDLDocNo='"+ PMDLDoc + "'and Parent_WFID != '0'and WF_Status in ('Enquiry Raised','Technical offer Received')";
        //    return Convert.ToInt32(SqlHelper.ExecuteScalar(Con, CommandType.Text, sGetTotalChildRecordCount));

        //}

        //public int TechnoComNegotiation_ChildRecordCount(string PMDLDoc)
        //{
        //   string sGetTotalChildRecordCount = @"select count(*) from tdmisg140200 where t_iref='" + ItemReference + "' and t_cprj= '" + Project + "'  and t_orgn='ISG' "; 
        //    //@"select count(*) from WF1_PreOrder_History where PMDLDocNo='"+ PMDLDoc + "'and Parent_WFID != '0'and WF_Status in ('Enquiry Raised','Technical offer Received')";
        //    return Convert.ToInt32(SqlHelper.ExecuteScalar(Con129, CommandType.Text, sGetTotalChildRecordCount));

        //}
        //GetQualifiedforTechnoCommercialCount
        //public double GetQualifiedforTechnoCommercialCount()
        //{
        //    string sGetQualifiedforTechnoCommercialCount = @"Select t_numt from ttpisg220200 where t_sub1='" + ItemReference + "' and t_bohd='CT_PREORDERTECHCLEAR'  and t_cprj='" + Project + "'";
        //    var outputParam=(SqlHelper.ExecuteScalar(Con129, CommandType.Text, sGetQualifiedforTechnoCommercialCount));
        //    if (!Convert.IsDBNull(outputParam))
        //        return Convert.ToDouble(outputParam);
        //    else return 0;
        //}
        //public int GetTechofferChildRecordCount(string PMDLDoc)
        //{
        //    string sGetTechofferChildRecordCount = @"select count(distinct WF1_PreOrder.WFID) from WF1_PreOrder inner join WF1_PreOrderPMDL
        //                                             on WF1_PreOrder.Parent_WFID = WF1_PreOrderPMDL.WFID
        //                                             and WF1_PreOrderPMDL.PMDLDocNo in(" + PMDLDoc + @")
        //                                             and WF1_PreOrder.Parent_WFID <> 0
        //                                             and WF1_PreOrder.WF_Status in ('Technical offer Received', 
        //                                             'Enquiry For Techno Commercial Negotiation Completed')  and WF1_PreOrder.Parent_WFID='" + Parent_WFID + "'";
        //   //@"select count(*) from WF1_PreOrder_History where PMDLDocNo='" + PMDLDoc + "'and Parent_WFID != '0' and WF_Status='Technical offer Received'";
        //    return Convert.ToInt32(SqlHelper.ExecuteScalar(Con, CommandType.Text, sGetTechofferChildRecordCount));
        //}

        //public int GetTechnoCommercialChildRecordCount(string PMDLDoc)
        //{
        //    string sGetTechnoCommercialChildRecordCount = @"select count(distinct WF1_PreOrder.WFID) from WF1_PreOrder inner join WF1_PreOrderPMDL
        //                                            on WF1_PreOrder.Parent_WFID = WF1_PreOrderPMDL.WFID
        //                                            and WF1_PreOrderPMDL.PMDLDocNo in(" + PMDLDoc + @")
        //                                            and WF1_PreOrder.Parent_WFID <> 0
        //                                            and WF1_PreOrder.WF_Status in ('Enquiry For Techno Commercial Negotiation Completed')  and WF1_PreOrder.Parent_WFID='" + Parent_WFID + "'";
        //    //@"select count(*) from WF1_PreOrder_History where PMDLDocNo='" + PMDLDoc + "'and Parent_WFID != '0' and WF_Status='Technical offer Received'";
        //    return Convert.ToInt32(SqlHelper.ExecuteScalar(Con, CommandType.Text, sGetTechnoCommercialChildRecordCount));
        //}

        //public string GetParenIDStatus(string WFID)
        //{
        //    string sStatus = @" select WF_Status from WF1_PreOrder where WFID=" + WFID + " and Parent_WFID='0'";
        //    return SqlHelper.ExecuteScalar(Con, CommandType.Text, sStatus).ToString();
        //}

        //public void UpdateAllTechOfferReceived()
        //{
        //    string sUpdateTechOfferReceivedDrawingpercentage = @"update ttpisg220200 set t_cpgv='100' where t_sub1='" + ItemReference + "' and t_bohd='CT_RFQOFFERECEIVED'  and t_cprj='" + Project + "'";
        //    SqlHelper.ExecuteNonQuery(Con129, CommandType.Text, sUpdateTechOfferReceivedDrawingpercentage);
        //}
        ////UpdateTechCommercialpercentage
        //public void UpdateTechCommercialNegotiaitionpercentage()
        //{
        //    string sUpdateTechCommercialNegotiaitionpercentage = @"update ttpisg220200 set t_cpgv='100' where t_sub1='" + ItemReference + "' and t_bohd='CT_RFQCOMMERCIALFINALISED'  and t_cprj='" + Project + "'";
        //    SqlHelper.ExecuteNonQuery(Con129, CommandType.Text, sUpdateTechCommercialNegotiaitionpercentage);
        //}

        //public void UpdateTechnicalClearancepercentage()
        //{
        //    string sUpdateTechnicalClearancepercentage = @"update ttpisg220200 set t_cpgv='100' where t_sub1='" + ItemReference + "' and t_bohd='CT_PREORDERTECHCLEAR'  and t_cprj='" + Project + "'";
        //    SqlHelper.ExecuteNonQuery(Con129, CommandType.Text, sUpdateTechnicalClearancepercentage);
        //}


        // Dump data to BAAN table

        public int GetWorkFlowID()
        {
            string sStatus = @"SELECT (ISNULL(MAX(WFID),0))as WfId FROM WF1_PreOrder";
            return Convert.ToInt32(SqlHelper.ExecuteScalar(Con, CommandType.Text, sStatus));
        }
        //public void InsertPreorderDataToBAN(int WorkFlowID)
        //{
           
        //    string InsertPreorderDataToBaan = @"INSERT INTO tdmisg168200 ([t_wfid],[t_pwfd],[t_cprj],[t_elem],[t_spec]
        //        ,[t_docn],[t_bpid],[t_mngr],[t_stat],[t_user],[t_date],[t_supp],[t_snam],[t_rdno],[t_supc],[t_rcno],[t_esub],t_Refcntd,t_Refcntu)
        //         VALUES
        //             (" + WorkFlowID + ",'"+Parent_WFID+"' , '" + Project + "','" + Element + @"',
        //                '" + SpecificationNo + "', '" + PMDLdocDesc + "', '" + Buyer + @"',
        //                '" + Manager + "', '" + WF_Status + "', '" + UserId + @"',
        //              '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "', 0,0,0,0,0,0,0,0)";
        //    SqlHelper.ExecuteNonQuery(Con129, CommandType.Text, InsertPreorderDataToBaan);

        //}
        //public int UpdateWF_StatusInBAANTable()
        //{
        //    return SqlHelper.ExecuteNonQuery(Con129, CommandType.Text, "Update tdmisg168200 set [t_stat]='" + WF_Status + "',t_date=getDate()  Where [t_wfid]='" + WFID + "'");
        //}

        //public DataTable InserPreOrderHistoryToBAAN()
        //{
        //    List<SqlParameter> sqlParamater = new List<SqlParameter>();
        //    sqlParamater.Add(new SqlParameter("@WFID", WFID));
        //    sqlParamater.Add(new SqlParameter("@Parent_WFID", Parent_WFID));
        //    sqlParamater.Add(new SqlParameter("@Project", Project));
        //    sqlParamater.Add(new SqlParameter("@Element", Element));
        //    sqlParamater.Add(new SqlParameter("@SpecificationNo", SpecificationNo));
        //    sqlParamater.Add(new SqlParameter("@PMDLDocNo", PMDLdocDesc));
        //    sqlParamater.Add(new SqlParameter("@Buyer", Buyer));
        //    sqlParamater.Add(new SqlParameter("@Manager", Manager));
        //    sqlParamater.Add(new SqlParameter("@WF_Status", WF_Status));
        //    sqlParamater.Add(new SqlParameter("@UserId", UserId));
        //    sqlParamater.Add(new SqlParameter("@Supplier", Supplier));
        //    sqlParamater.Add(new SqlParameter("@SupplierName", SupplierName));
        //    sqlParamater.Add(new SqlParameter("@Notes", Notes));
        //    // return SqlHelper.ExecuteDataset(Con, CommandType.StoredProcedure, "sp_Insert_PreOrder_Workflow_History", sqlParamater.ToArray()).Tables[0];
        //    return SqlHelper.ExecuteDataset(Con129, CommandType.StoredProcedure, "sp_Insert_PreOrderWorkflowHistory_DataToBAAN", sqlParamater.ToArray()).Tables[0];
        //}

        //public void UpdateTechnoCommercialDate( string DATE)

        //{
        //    string sUpdateTechnoCommercialDate = @"update ttpisg220200 set t_acsd=cast('" + DATE + "' as DATETIME) where t_sub1='" + ItemReference + "' and t_bohd='" + BusinessObjectHandle + "'  and t_cprj='" + Project + "' ";
        //    SqlHelper.ExecuteNonQuery(Con129, CommandType.Text, sUpdateTechnoCommercialDate);
        //}

        //public int UpdateStatusWFPreOrder_HistoryBaaN()
        //{
        //    return SqlHelper.ExecuteNonQuery(Con129, CommandType.Text, @"UPDATE tdmisg169200 SET  t_stat= '" + WF_Status + @"'
        //                  WHERE t_wfid = '" + WFID + @"' and t_slno='" + SLNO_WFID + "'");

        //}

        //public void InsertIntoItemReferencewiseProgressTable(double Weightpercentage, double Countpercentage)
        //{
        //    string sInsertIntoItemReferencewiseProgressTable = @"Insert into WF1_ItemReferencewiseProgress values('" + WFID + @"', 
        //    '" + Parent_WFID + "','" + BusinessObjectHandle + "','" + ItemReference + "' ,'" + Weightpercentage + @"',
        //     '" + Countpercentage + "','" + UserId + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','" + WF_Status + "','" + Project + "' )";
        //    SqlHelper.ExecuteNonQuery(Con, CommandType.Text, sInsertIntoItemReferencewiseProgressTable);
        //}
        //public DataTable GetPercentagebyCount_Weight()
        //{
        //    string sGetPercentagebyCount_Weight = @"select sum(a.ProgressByWeight) as WeightPercentage, sum(a.ProgressByCount) 
        //                                            as CountPercentage, a.Project,a.ItemReference  from WF1_ItemReferencewiseProgress a
        //                                            where a.project ='" + Project + @"'
        //                                            and a.ItemReference = '" + ItemReference + @"' 
        //                                            and a.Businnesshndl = '" + BusinessObjectHandle + @"'
        //                                            and a.WFID in(select distinct b.WFID from WF1_ItemReferencewiseProgress b
        //                                            where b.project = '" + Project + @"' and b.ItemReference = '" + ItemReference + @"'
        //                                            and b.Businnesshndl = '" + BusinessObjectHandle + @"' 
        //                                            and b.UpdatedDate = (select max(c.UpdatedDate) from WF1_ItemReferencewiseProgress c
        //                                            where c.parent_WFID =a.parent_WFID
        //                                            and c.project = '" + Project + @"'
        //                                            and c.ItemReference = '" + ItemReference + @"'
        //                                            and c.Businnesshndl = '" + BusinessObjectHandle + @"'
        //                                            ) )
        //                                           and a.Parent_WFID not in(select d.parent_WFID from WF1_ItemReferencewiseProgress d
        //                                           where d.UserAction = 'All Offer Received')
        //                                           group by a.ItemReference, a.Project
        //                                           union
        //                                            select convert(decimal(10, 2), sum(ProgressByWeight)) as WeightPercentage, 
        //                                            convert(decimal(10, 2), sum(ProgressByCount)) CountPercentage,
        //                                            Project, ItemReference  from  WF1_ItemReferencewiseProgress a
        //                                            where project ='" + Project + @"'
        //                                            and ItemReference = '" + ItemReference + @"' 
        //                                            and Businnesshndl = '" + BusinessObjectHandle + @"'
        //                                            and UserAction = 'All Offer Received'
        //                                            group by ItemReference,Project";
        //    DataTable dt = SqlHelper.ExecuteDataset(Con, CommandType.Text, sGetPercentagebyCount_Weight).Tables[0];

           
        //    return dt;




        //}

        //public DataTable GetPercentagebyCount_Weight_AllOfferReceievd()
        //{
        //    string sGetPercentagebyCount_Weight_AllOfferReceievd = @"select sum(b.ProgressByWeight) as WeightPercentage, sum(b.ProgressByCount) 
        //                                            as CountPercentage, b.Project,b.ItemReference from WF1_ItemReferencewiseProgress b
        //                                            where b.project = '" + Project + @"'
        //                                            and b.ItemReference ='" + ItemReference + @"' 
        //                                            and b.Businnesshndl = '" + BusinessObjectHandle + @"'
        //                                            and b.UpdatedDate in (select max(c.UpdatedDate) from WF1_ItemReferencewiseProgress c
        //                                            where c.project = '" + Project + @"'
        //                                            and c.ItemReference = '" + ItemReference + @"' 
        //                                            and c.Businnesshndl = '" + BusinessObjectHandle + @"' group by c.Parent_WFID)
        //                                            group by ItemReference,Project";
        //    DataTable dt = SqlHelper.ExecuteDataset(Con, CommandType.Text, sGetPercentagebyCount_Weight_AllOfferReceievd).Tables[0];


        //    return dt;
        //}


        //public DataTable GetPMDLbyWFID()
        //{
        //    string sGetPercentagebyCount_Weight = @"select PMDLDocNo from WF1_PreOrderPMDL where WFID ='" + WFID + @"' ";
        //    return SqlHelper.ExecuteDataset(Con, CommandType.Text, sGetPercentagebyCount_Weight).Tables[0];

        //}
        //public DataTable GetTechnoComNegotiationPercentagebyCount_Weight()
        //{
        //    string sGetPercentagebyCount_Weight = @"Select sum(ProgressByWeight) as WeightPercentage, sum(ProgressByCount) CountPercentage,
        //                                          Project, ItemReference from WF1_ItemReferencewiseProgress  where 
        //                                          Project ='" + Project + "' and ItemReference='" + ItemReference + @"' 
        //                                          and Businesshndl='CT_RFQCOMMERCIALFINALISED'order by ItemReference,Project ";
        //    return SqlHelper.ExecuteDataset(Con, CommandType.Text, sGetPercentagebyCount_Weight).Tables[0];

        //}

        //public int SelectAllOfferReceivedRFQCount()
        //{
        //    var outputParam= SqlHelper.ExecuteScalar(Con, CommandType.Text, @"select Count(*)  from WF1_PreOrder_History  where  
        //                                     WFID ='" + WFID + @"' and WF_Status='All Offer Received'");

        //    if (!Convert.IsDBNull(outputParam))
        //        return Convert.ToInt32(outputParam);
        //    else return 0;
        //}


        #endregion

    }
}