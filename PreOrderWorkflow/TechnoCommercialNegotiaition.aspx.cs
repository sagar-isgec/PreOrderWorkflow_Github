using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PreOrderWorkflow
{
    public partial class TechnoCommercialNegotiaition : System.Web.UI.Page
    {
        WorkFlow objWorkFlow;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["Status"] == "Enquiry Qualify For Techno Commercial Negotiaition ")
                {
                    btnEnqTechCom.Text = "Techno Commercial Negotiation Completed";
                    hHeader.InnerHtml = "Complete Techno Commercial Negotiation";

                }

                txtProject.Text = Session["Project"].ToString();
                txtElement.Text= Session["Element"].ToString();
                txtSpecification.Text= Session["SpecificationNo"].ToString();
                txtPMDLDoc.Text= Session["PMDLDocNo"].ToString();
                txtBuyer.Text = Session["BuyerName"].ToString();
                txtManager.Text= Session["ManagerName"].ToString();
                txtSupplierEmail.Text= Session["SupplierEmail"].ToString();
                txtSupplierCode.Text= Session["SupplierCode"].ToString();
                txtSupplier.Text= Session["SupplierName"].ToString();
                hdfParentWFID.Value=Session["Parent_WFID"].ToString();
                lblWorkFlowID.Text= "WorkFlowId :"+(Request.QueryString["WFID"]).ToString();
                //if (ddlSupplier.Enabled)
                //{
                //    GetSupplier();
                //}
                //  GetData();
            }
        }
     

        protected void btnSendEnquiry_Click(object sender, EventArgs e)
        {
            WorkFlow objWorkFlow = new WorkFlow();
            objWorkFlow.WFID = Convert.ToInt32(Request.QueryString["WFID"]);
            DataTable dt1 = objWorkFlow.GetWFById();
            objWorkFlow.Parent_WFID = Convert.ToInt32(dt1.Rows[0]["Parent_WFID"]);
            objWorkFlow.WF_Status = "Enquiry For Techno Commercial Negotiation Completed";
            objWorkFlow.Project = dt1.Rows[0]["Project"].ToString().Substring(0, 6);
            //int res = objWorkFlow.UpdateEnquiryRaised();
            int res = objWorkFlow.UpdateTechnoCommercialNegotiationCompleted();
            objWorkFlow.UpdateWF_StatusInBAANTable();// Dump Preorder Data TO BAAN table change 25/08/2018 sagar
            if (res > 0)
            {
                InsertPreHistory(Convert.ToInt32(Request.QueryString["WFID"]), "Enquiry For Techno Commercial Negotiation Completed");
            }


            #region ERP Transaction Update for Control Tower

            //if (Request.QueryString["Status"] == "Enquiry Raised" || Request.QueryString["Status"] == "Technical offer Received")
            //    {
            objWorkFlow.TransactionDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            objWorkFlow.BusinessObjectHandle = "CT_RFQCOMMERCIALFINALISED";
            objWorkFlow.IndexValue = (Request.QueryString["WFID"]).ToString();
            int SerialNoCount = objWorkFlow.GetSerialNumber();
            SerialNoCount++;
            objWorkFlow.SLNO_WFID = SerialNoCount;
            string[] Project = txtProject.Text.Split('-');
            objWorkFlow.Project = Project[0];
            string[] Element = txtElement.Text.Split('-');
            objWorkFlow.Element = Element[0];
            objWorkFlow.UserId = Request.QueryString["u"];
            int nRecordInserted = objWorkFlow.InsertPreOrderDatatoControlTower();
            if (nRecordInserted > 0)
            {
                // for each PMDL doc insert a new record ( Sagar new change 11-July-2018)
                // 2 ways- comma seperated substring or select PMDLDoc for given WFID from WF1_PreOrderPMDL table 
                //and loop through each PMDL doc.
                int DetailSerialNoCount = 0;
                DataTable dtPMDL = objWorkFlow.GetMultiPMDL(hdfParentWFID.Value);
                if (dtPMDL.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPMDL.Rows)
                    {
                        DetailSerialNoCount = objWorkFlow.GetDetailSerialNumber();
                        DetailSerialNoCount++;
                        objWorkFlow.DetailSerialCount = DetailSerialNoCount;

                        objWorkFlow.PMDLdocDesc = dr["PMDLDocNo"].ToString();
                        DataTable dt = objWorkFlow.GetPartItemCount_Weight();
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["PartItemCount"].ToString() != "")
                            {
                                objWorkFlow.PartItemCount = (int)dt.Rows[0]["PartItemCount"];
                            }
                            else { objWorkFlow.PartItemCount = 0; }
                            if (dt.Rows[0]["PartItenWeight"].ToString() != "")
                            {
                                objWorkFlow.PartItemWeight = (double)dt.Rows[0]["PartItenWeight"];
                            }
                            else { objWorkFlow.PartItemWeight = 0; }
                        }
                        else
                        {
                            objWorkFlow.PartItemCount = 0;
                            objWorkFlow.PartItemWeight = 0;
                        }

                        objWorkFlow.InsertPreOrderDatatoControlTowerChildTable();

                    
                    //if (Request.QueryString["Status"] == "Technical offer Received")
                    //{
                    double nTechnoComNegotiationrawingCount = 0;
                    double nTotalDrawingCount = 0;
                    double nTotalChildRecordCount = 0;
                    double nTechnoCommNegotiationWeight = 0;
                    double nTotalWeight = 0;
                    double nQualifiedforTechnoCommercial = 0;
                    double nTechnoCommercialChildRecordCount = 0;
                    string PMDLDocs = "";
                    string PMDLDocuments = "";
                    string PMDLDocNo = "";
                    double percentageTechnoCommercial_byCount;
                    double percentageTechnoCommercial_byWeight;
                    DataTable dataTable1 = objWorkFlow.GetTechnoCommercialItemReference();
                    objWorkFlow.ItemReference = (string)dataTable1.Rows[0]["t_iref"];
                    string Itemref_Typ = objWorkFlow.GetItemRefType();

                    if (dataTable1.Rows[0]["TechnoCommercialDate"].ToString() == "01-01-1970" || dataTable1.Rows[0]["TechnoCommercialDate"].ToString() == "01-01-1900")
                    {
                        string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");
                        // objWorkFlow.UpdateTechnoCommercialDate(CurrentDate);
                        objWorkFlow.UpdateTechnoCommercialNegotiationDate(CurrentDate);
                    }
                    DataTable dtPMDLDoc = objWorkFlow.GetPMDLFromItemRef();
                    if (dtPMDLDoc.Rows.Count > 0)
                    {

                        for (int i = 0; i < dtPMDLDoc.Rows.Count; i++)
                        {

                            if (i == 0)
                            {
                                PMDLDocs = "'" + dtPMDLDoc.Rows[0]["t_docn"].ToString() + "'";
                            }
                            else
                            {
                                PMDLDocs += ",'" + dtPMDLDoc.Rows[i]["t_docn"].ToString() + "'";
                            }

                        }
                    }
                    DataTable dtPMDLDocForTechnoCommNegotiation = objWorkFlow.GetPMDLDocForTechnoCommNegotiation();
                    if (dtPMDLDocForTechnoCommNegotiation.Rows.Count > 0)
                    {

                        for (int i = 0; i < dtPMDLDocForTechnoCommNegotiation.Rows.Count; i++)
                        {

                            if (i == 0)
                            {
                                PMDLDocuments = "'" + dtPMDLDocForTechnoCommNegotiation.Rows[0]["t_docn"].ToString() + "'";
                            }
                            else
                            {
                                PMDLDocuments += ",'" + dtPMDLDocForTechnoCommNegotiation.Rows[i]["t_docn"].ToString() + "'";
                            }

                        }
                    }

                        int nWFID = objWorkFlow.WFID;
                        objWorkFlow.WFID = objWorkFlow.Parent_WFID;
                        DataTable dtStatus = objWorkFlow.GetWFById();
                       
                        string sCurrentWFID_Status = dtStatus.Rows[0]["WF_Status"].ToString();
                        DataTable dtPMDLbyWFID = objWorkFlow.GetPMDLbyWFID();
                        objWorkFlow.WFID = nWFID;
                        if (dtPMDLbyWFID.Rows.Count > 0)
                    {

                        for (int i = 0; i < dtPMDLbyWFID.Rows.Count; i++)
                        {

                            if (i == 0)
                            {
                                PMDLDocNo = "'" + dtPMDLbyWFID.Rows[0]["PMDLDocNo"].ToString() + "'";
                            }
                            else
                            {
                                PMDLDocNo += ",'" + dtPMDLbyWFID.Rows[i]["PMDLDocNo"].ToString() + "'";
                            }

                        }
                    }

                    nTechnoCommNegotiationWeight = objWorkFlow.GetTotalWeight(PMDLDocNo);
                    nTotalWeight = objWorkFlow.GetTotalWeight();
                    nTechnoComNegotiationrawingCount += objWorkFlow.GetTechnoComNegotiationrawingCount(PMDLDocNo);
                    nTotalDrawingCount = objWorkFlow.GeTotalDrawingCount();
                        // nQualifiedforTechnoCommercial += objWorkFlow.GetQualifiedforTechnoCommercialCount();
                        if (sCurrentWFID_Status == "All Offer Received")
                        {
                            nTotalChildRecordCount=objWorkFlow.GetTotalChildRecordCount_AllOfferReceived(PMDLDocNo);
                        }
                        else
                        {
                            nTotalChildRecordCount = objWorkFlow.GetTotalChildRecordCount(PMDLDocNo);
                        }
                    //3
                    nTechnoCommercialChildRecordCount += objWorkFlow.GetTechnoCommercialChildRecordCount(PMDLDocs);
                    //4
                    objWorkFlow.UpdateTechnoCommercialCount(nTechnoCommercialChildRecordCount);
                    double Technicalvettingpercentage = objWorkFlow.GetTechnicalvettingpercentage();
                    if (nTotalChildRecordCount != 0 && nTotalDrawingCount !=0)
                    {
                        percentageTechnoCommercial_byCount = Math.Round((nTechnoCommercialChildRecordCount / nTotalChildRecordCount) * (nTechnoComNegotiationrawingCount / nTotalDrawingCount) *100, 2);
                    }
                    else
                    {
                        percentageTechnoCommercial_byCount = 0;
                    }
                    //percentageTechnoCommercial_byWeight
                    if (nTotalWeight != 0 && nTotalDrawingCount != 0)
                    {
                        // percentageTechnoCommercial_byWeight = Math.Round((nTechnoCommNegotiationWeight / nTotalWeight) * (nTechnoComNegotiationrawingCount / nTotalDrawingCount)*100, 2);
                        percentageTechnoCommercial_byWeight = Math.Round((nTechnoCommNegotiationWeight / nTotalWeight) * (nTechnoCommercialChildRecordCount / nTotalChildRecordCount) * 100, 2);
                    }
                    else
                    {
                        percentageTechnoCommercial_byWeight = 0;
                    }
                    objWorkFlow.InsertIntoItemReferencewiseProgressTable(percentageTechnoCommercial_byWeight, percentageTechnoCommercial_byCount);
                        //DataTable dtPercentage = objWorkFlow.GetTechnoComNegotiationPercentagebyCount_Weight();
                        DataTable dtPercentage = new DataTable();
                        if (sCurrentWFID_Status == "All Offer Received")
                        {

                            dtPercentage= objWorkFlow.GetPercentagebyCount_Weight_AllOfferReceievd();
                        }
                        else
                        {
                             dtPercentage = objWorkFlow.GetPercentagebyCount_Weight();
                        }
                           
                        double percentage_byCount = 0;
                        double percentage_byWeight = 0;
                        if (dtPercentage.Rows.Count > 0)
                        {
                            foreach (DataRow dr2 in dtPercentage.Rows)
                            {
                                objWorkFlow.Project = dr2["Project"].ToString();
                                objWorkFlow.ItemReference = dr2["ItemReference"].ToString();
                                //double percentage_byCount = Convert.ToDouble(dr2["CountPercentage"]);
                                //double percentage_byWeight = Convert.ToDouble(dr2["WeightPercentage"]);
                                 percentage_byCount += Convert.ToDouble(dr2["CountPercentage"]);
                                 percentage_byWeight += Convert.ToDouble(dr2["WeightPercentage"]);
                            }
                        }
                                if (percentage_byCount < 100.00 && percentage_byWeight < 100.00)
                            {
                                if ((percentage_byCount >= percentage_byWeight))
                                {
                                    objWorkFlow.UpdateTechOfferReceivedDrawingpercentage(percentage_byCount);
                                }
                                else
                                {
                                    if (Itemref_Typ == "4")// when item reference typ=="Self Engineered"
                                    {
                                        objWorkFlow.UpdateTechOfferReceivedDrawingpercentage(percentage_byWeight);
                                    }
                                    else
                                    {
                                        objWorkFlow.UpdateTechOfferReceivedDrawingpercentage(percentage_byCount);
                                    }
                                }
                            }

                            else
                            {
                                if (percentage_byCount >= 100)
                                {
                                    objWorkFlow.UpdateTechOfferReceivedDrawingpercentage(100);
                                }
                                else
                                {
                                    objWorkFlow.UpdateTechOfferReceivedDrawingpercentage(percentage_byCount);
                                }
                            }
                    //    }
                    //}
                    //if (percentageTechnoCommercial_byCount>= 100.00 || percentageTechnoCommercial_byWeight>=100.00)
                    //{
                    //    objWorkFlow.UpdateTechOfferReceivedDrawingpercentage(100);
                    //}
                    //else if ((percentageTechnoCommercial_byCount >= percentageTechnoCommercial_byWeight))
                    //{
                    //    objWorkFlow.UpdateTechOfferReceivedDrawingpercentage(percentageTechnoCommercial_byCount);
                    //}
                    //else
                    //{
                    //    objWorkFlow.UpdateTechOfferReceivedDrawingpercentage(percentageTechnoCommercial_byWeight);
                    //}
                    //   objWorkFlow.UpdateTechnoCommercialpercentage(percentageTechnoCommercial_byCount);

                    DateTime MinTechnoCommecrialDate = objWorkFlow.GetMinTechnoCommecrialDate();
                    if (MinTechnoCommecrialDate != default(DateTime))
                    {
                        string OfferReceieveDate = MinTechnoCommecrialDate.ToString("yyyy-MM-dd hh:mm:ss");
                        objWorkFlow.UpdateTechnoCommercialDate(OfferReceieveDate);
                    }

                    }

                }
                
                else
                {
                    Response.Redirect("RaisedEnquiry.aspx?u=" + Request.QueryString["u"] + "&WFPID=" + hdfParentWFID.Value);
                }

                #endregion
               
                //if (Request.QueryString["Status"] == "Enquiry Raised")
                //{
                //    Response.Redirect("EnquiryInProcess.aspx?u=" + Request.QueryString["u"]);
                //}
                //if (Request.QueryString["Status"] == "Technical offer Received" || Request.QueryString["Status"] == "Commercial offer Received")
                //{
                //    Response.Redirect("RaisedEnquiry.aspx?u=" + Request.QueryString["u"] + "&WFPID=" + Request.QueryString["WFPID"]);
                //}
                //if (Request.QueryString["Status"] == "Isgec Comment Submitted")
                //{
                //    Response.Redirect("ReleaseComments.aspx?u=" + Request.QueryString["u"]);
                //}
                // ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert('" + Request.QueryString["Status"] + "');", true);
            }
            Response.Redirect("RaisedEnquiry.aspx?u=" + Request.QueryString["u"] + "&WFPID=" + hdfParentWFID.Value);
        }


       private void InsertPreHistory(int Id, string status)
        {
            objWorkFlow = new WorkFlow();
            objWorkFlow.WFID = Id;
            DataTable dt = objWorkFlow.GetWFById();
            objWorkFlow.Parent_WFID = Convert.ToInt32(dt.Rows[0]["Parent_WFID"]);
            objWorkFlow.Project = dt.Rows[0]["Project"].ToString();
            objWorkFlow.Element = dt.Rows[0]["Element"].ToString();
            objWorkFlow.SpecificationNo = dt.Rows[0]["SpecificationNo"].ToString() ?? " ";
            objWorkFlow.Buyer = dt.Rows[0]["Buyer"].ToString();
            objWorkFlow.UserId = Request.QueryString["u"];
            objWorkFlow.PMDLdocDesc = dt.Rows[0]["PMDLDocNo"].ToString();
            objWorkFlow.WF_Status = status;
            objWorkFlow.Supplier = dt.Rows[0]["Supplier"].ToString();
            objWorkFlow.SupplierName = dt.Rows[0]["SupplierName"].ToString();
            objWorkFlow.Manager= dt.Rows[0]["Manager"].ToString();
           
            // objWorkFlow.Notes = txtNotes.Text;
            DataTable dtWFHID = objWorkFlow.InserPreOrderHistory();
            objWorkFlow.Notes = " ";
            objWorkFlow.InserPreOrderHistoryToBAAN();// Dump Preorder Data TO BAAN table change 25/08/2018 sagar
        }
    }
 }
