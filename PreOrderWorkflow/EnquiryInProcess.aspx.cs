using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PreOrderWorkflow
{
    public partial class EnquiryInPrecess : System.Web.UI.Page
    {
        WorkFlow objWorkFlow;
        public bool IsStatusTechReleased { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["u"] != null)
                {
                    GetData();
                    // (LinkButton)btnAllOfferReceieved.Attributes.Add("onclick", "getMessage()");

                }
                else
                {

                }
            }
        }
        public bool GetVisible(object value)
        {
            if (value.ToString() == "Enquiry in progress")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool GetAllOfferReceiveVisible(object value)
        {
            if (value.ToString() == "Enquiry in progress")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void GetData()


        {
            objWorkFlow = new WorkFlow();
            objWorkFlow.WF_Status = "Enquiry in progress', 'Technical Specification Released','All Offer Received";
            objWorkFlow.UserId = Request.QueryString["u"];
            DataTable dt = objWorkFlow.GetWFBY_Status();
            //foreach (DataRow dr in dt.Rows)
            //{
            //    if (dr["WF_Status"].ToString() == "Enquiry in progress")
            //    {

            //       // gvData.Rows[].Visible = false;
            //       // IsStatusTechReleased = false;
            //    }
            //    else
            //    {
            //        gvData.Rows[13].Visible = true;
            //    }
            //}
            //foreach (DataRow row in dt.Rows)
            //{
            //    if (row["WF_Status"].ToString() == "Enquiry in progress")
            //    {
            //        btnReturn.Visible = false;
            //    }
            //    if (row["WF_Status"].ToString() == "Technical Specification Released")
            //    {
            //        btnClosed.Visible = false;
            //    }
            //}

            gvData.DataSource = dt;
            gvData.DataBind();

        }
        #region btn Raise Enquiry Click
        protected void btnRaiseEnquiry_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            objWorkFlow = new WorkFlow();
            objWorkFlow.WFID = Convert.ToInt32(btn.CommandArgument);
            objWorkFlow.WF_Status = "Enquiry in progress";
            objWorkFlow.UserId = Request.QueryString["u"];
            int res = objWorkFlow.UpdateWF_Status();
           // objWorkFlow.UpdateWF_StatusInBAANTable(); // Dump Preorder Data TO BAAN table change 25/08/2018 sagar 
            if (res > 0)
            {
                DataTable dt = objWorkFlow.GetWFById();
                if (dt.Rows.Count > 0)
                {
                    objWorkFlow.WF_Status = "Enquiry Created";
                    objWorkFlow.Parent_WFID = Convert.ToInt32(btn.CommandArgument);
                    objWorkFlow.Project = dt.Rows[0]["Project"].ToString();
                    objWorkFlow.Element = dt.Rows[0]["Element"].ToString();
                    objWorkFlow.PMDLdocDesc = dt.Rows[0]["PMDLDocNo"].ToString();
                    objWorkFlow.SpecificationNo = dt.Rows[0]["SpecificationNo"].ToString();
                    objWorkFlow.Buyer = dt.Rows[0]["Buyer"].ToString();
                    objWorkFlow.Manager = dt.Rows[0]["Manager"].ToString();
                    DataTable dtres = objWorkFlow.InsertPreOrderData();
                    int WorkFlowId = objWorkFlow.GetWorkFlowID();
                    objWorkFlow.Project = objWorkFlow.Project.Substring(0, 6);
                    objWorkFlow.Element = objWorkFlow.Element.Substring(0, 8);
                  //  objWorkFlow.InsertPreorderDataToBAN(WorkFlowId); // Dump Preorder Data TO BAAN table change 25/08/2018 sagar
                    if (dtres.Rows.Count > 0)
                    {
                        InsertPreHistory(Convert.ToInt32(btn.CommandArgument), "Enquiry in progress");
                        Response.Redirect("RaiseEnquiryForm.aspx?WFID=" + dtres.Rows[0][0].ToString() + "&Status=Enquiry Raised&u=" + Request.QueryString["u"] + "&WFPID=" + btn.CommandArgument);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert('Some technical issue');", true);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert('Some technical issue');", true);
            }
        }
        #endregion
        private void InsertPreHistory(int Id, string status)
        {
            objWorkFlow = new WorkFlow();
            objWorkFlow.WFID = Id;
            DataTable dt = objWorkFlow.GetWFById();
            objWorkFlow.Parent_WFID = Convert.ToInt32(dt.Rows[0]["Parent_WFID"]);
            objWorkFlow.Project = dt.Rows[0]["Project"].ToString();
            objWorkFlow.Element = dt.Rows[0]["Element"].ToString();
            objWorkFlow.SpecificationNo = dt.Rows[0]["SpecificationNo"].ToString();
            objWorkFlow.PMDLdocDesc = dt.Rows[0]["PMDLDocNo"].ToString();
            objWorkFlow.Buyer = dt.Rows[0]["Buyer"].ToString();
            objWorkFlow.UserId = Request.QueryString["u"];
            objWorkFlow.Manager = dt.Rows[0]["Manager"].ToString();
            objWorkFlow.WF_Status = status;
            objWorkFlow.Supplier = dt.Rows[0]["Supplier"].ToString();
            objWorkFlow.SupplierName = dt.Rows[0]["SupplierName"].ToString();
            objWorkFlow.InserPreOrderHistory();
            objWorkFlow.Notes = " ";
           // objWorkFlow.InserPreOrderHistoryToBAAN(); // Dump Preorder Data TO BAAN table change 25/08/2018 sagar
        }
        //protected void OnRowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    //Get the Command Name.
        //    string commandName = e.CommandName;

        //    //Get the Row Index.
        //    int rowIndex = Convert.ToInt32(e.CommandArgument);

        //    //Get the Row reference in which Button was clicked.
        //    GridViewRow row = gvData.Rows[rowIndex];
        //}
        protected void btnClosed_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            objWorkFlow = new WorkFlow();

            objWorkFlow.WFID = Convert.ToInt32(btn.CommandArgument);
            objWorkFlow.UserId = Request.QueryString["u"];
            objWorkFlow.WF_Status = "Commercial offer Finalized";
            string PMDLDocNo = "";
            int res = objWorkFlow.UpdateWF_Status();
            //  objWorkFlow.UpdateWF_StatusInBAANTable(); // Dump Preorder Data TO BAAN table change 25/08/2018 sagar 
            if (res > 0)
            {
                InsertPreHistory(Convert.ToInt32(btn.CommandArgument), "Commercial offer Finalized");

                GetData();
                // ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert('Commercial offer Finalized');", true);
                GridViewRow row = (btn.NamingContainer as GridViewRow);

                //Get the Row Index.
                int rowIndex = row.RowIndex;

                Label lProject = row.FindControl("Project") as Label;
                Label lElement = row.FindControl("Element") as Label;
                Label lPMDLDoc = row.FindControl("PMDLDocNo") as Label;
                string Project = "";
                string Element = "";
                string PMDLDoc = "";
                double percentageTechOfferReceivedDrawing_byWeight = 0;
                double percentageTechOfferReceivedDrawing_byCount = 0;



                if (lProject != null)
                {
                    Project = lProject.Text;
                }
                else
                { Project = ""; }
                if (lElement != null)
                {
                    Element = lElement.Text;
                }
                else
                { Element = ""; }
                if (lPMDLDoc != null)
                {
                    PMDLDoc = lPMDLDoc.Text;
                }
                else
                { PMDLDoc = ""; }
                // Convert.ToString(gvData.DataKeys[rowIndex].Values[2]
                //  string Project = (gvData.Rows[rowIndex].Cells[2].Text).ToString();
                //string Element = (gvData.Rows[rowIndex].Cells[3].Text).ToString();
                //string PMDLDoc =(gvData.Rows[rowIndex].Cells[5].Text).ToString();

                #region ERP Transaction Update for Control Tower

                //  Change - 26 June 2018 -- ERP Transaction Update for Control Tower
                // Insert data into ttpisg229200

                objWorkFlow.TransactionDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                objWorkFlow.BusinessObjectHandle = "CT_RFQCOMMERCIALFINALISED";
                objWorkFlow.IndexValue = objWorkFlow.WFID.ToString();
                //(Request.QueryString["WFID"]).ToString();
                int SerialNoCount = objWorkFlow.GetSerialNumber();
                SerialNoCount++;
                objWorkFlow.SLNO_WFID = SerialNoCount;
                string[] sProject = Project.Split('-');
                objWorkFlow.Project = sProject[0];
                string[] sElement = Element.Split('-');
                objWorkFlow.Element = sElement[0];
                objWorkFlow.UserId = Request.QueryString["u"];
                int nRecordInserted = objWorkFlow.InsertPreOrderDatatoControlTower();
                if (nRecordInserted > 0)
                {
                    objWorkFlow.WFID = Convert.ToInt32(btn.CommandArgument); 
                    DataTable dt2 = objWorkFlow.GetWFById();
                    //objWorkFlow.Parent_WFID = Convert.ToInt32(dt2.Rows[0]["Parent_WFID"]);
                    DataTable dtPMDL = objWorkFlow.GetMultiPMDL(objWorkFlow.WFID.ToString());
                    if (dtPMDL.Rows.Count > 0)
                    {
                        foreach (DataRow dr1 in dtPMDL.Rows)
                        {
                            long DetailSerialNoCount = objWorkFlow.GetDetailSerialNumber();
                            DetailSerialNoCount++;
                            objWorkFlow.DetailSerialCount = DetailSerialNoCount;
                            objWorkFlow.PMDLdocDesc = dr1["PMDLDocNo"].ToString();
                            //PMDLDoc.ToString();
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
                        }
                    }
                }
            }
        }
        //DataTable dtPMDL = objWorkFlow.GetMultiPMDL(Convert.ToInt32(btn.CommandArgument).ToString());
        //if (dtPMDL.Rows.Count > 0)
        //{
        //    foreach (DataRow dr in dtPMDL.Rows)
        //    {
        //        objWorkFlow.PMDLdocDesc = dr["PMDLDocNo"].ToString();
        //       // DataTable dataTable = objWorkFlow.GetRaisedEnquiryDate();
        //        //objWorkFlow.ItemReference = (string)dataTable.Rows[0]["t_iref"];
        //        // objWorkFlow.UpdateTechCommercialNegotiaitionpercentage();// Change in the logic date-14-09-2018 sagar
        //        objWorkFlow.WFID = Convert.ToInt32(btn.CommandArgument);
        //     //   DataTable dtPMDLbyWFID = objWorkFlow.GetPMDLbyWFID();
        //if (dtPMDLbyWFID.Rows.Count > 0)
        //{

        //    for (int i = 0; i < dtPMDLbyWFID.Rows.Count; i++)
        //    {

        //        if (i == 0)
        //        {
        //            PMDLDocNo = "'" + dtPMDLbyWFID.Rows[0]["PMDLDocNo"].ToString() + "'";
        //        }
        //        else
        //        {
        //            PMDLDocNo += ",'" + dtPMDLbyWFID.Rows[i]["PMDLDocNo"].ToString() + "'";
        //        }

        //    }
        // }
        //                    double nTechnoComNegotiationrawingCount = objWorkFlow.GetTechnoComNegotiationrawingCount(PMDLDocNo);
        //                    // double nTotalDrawingCount = objWorkFlow.GeTotalDrawingCount();
        //                    double nTotalDrawingCount=objWorkFlow.TechnoComNegotiation_ChildRecordCount(PMDLDocNo);
        //                    double nTechnoComNegotiationDrawing_Weight = objWorkFlow.GetTechnoComNegotiationWeight(PMDLDocNo);
        //                    double nTotalTechnoComNegotiation_Weight = objWorkFlow.GetTotalWeight();

        //                    if (nTotalDrawingCount != 0)
        //                    {
        //                        percentageTechOfferReceivedDrawing_byCount = Math.Round((nTechnoComNegotiationrawingCount / nTotalDrawingCount) * 100, 4);
        //                    }

        //                    if (nTotalTechnoComNegotiation_Weight != 0)
        //                    {
        //                        percentageTechOfferReceivedDrawing_byWeight = Math.Round((nTechnoComNegotiationDrawing_Weight / nTotalTechnoComNegotiation_Weight) * 100, 4);
        //                    }
        //                    objWorkFlow.BusinessObjectHandle = "CT_RFQCOMMERCIALFINALISED";
        //                    objWorkFlow.WF_Status = "Commercial offer Finalized";
        //                    objWorkFlow.Parent_WFID = objWorkFlow.WFID;
        //                    objWorkFlow.InsertIntoItemReferencewiseProgressTable(percentageTechOfferReceivedDrawing_byWeight, percentageTechOfferReceivedDrawing_byCount);
        //                    //DataTable dtPercentage = objWorkFlow.GetTechnoComNegotiationPercentagebyCount_Weight();
        //                    DataTable dtPercentage1 = objWorkFlow.GetPercentagebyCount_Weight();

        //                    objWorkFlow.BusinessObjectHandle = "CT_RFQOFFERECEIVED";
        //                    objWorkFlow.WF_Status = "Commercial offer Finalized";
        //                    objWorkFlow.Parent_WFID = objWorkFlow.WFID;
        //                    objWorkFlow.InsertIntoItemReferencewiseProgressTable(percentageTechOfferReceivedDrawing_byWeight, percentageTechOfferReceivedDrawing_byCount);
        //                    //DataTable dtPercentage = objWorkFlow.GetTechnoComNegotiationPercentagebyCount_Weight();
        //                    DataTable dtPercentage2 = objWorkFlow.GetPercentagebyCount_Weight();



        //                    double percentage_byCount_ComFinalized = 0;
        //                    double percentage_byWeight_ComFinalized = 0;
        //                    double percentage_byCount_OfferReceieved = 0;
        //                    double percentage_byWeight_OfferReceieved = 0;
        //                   string Itemref_Typ = objWorkFlow.GetItemRefType();

        //                    if (dtPercentage1.Rows.Count > 0)
        //                    {
        //                        foreach (DataRow dr1 in dtPercentage1.Rows)
        //                        {
        //                            objWorkFlow.Project = dr1["Project"].ToString();
        //                            objWorkFlow.ItemReference = dr1["ItemReference"].ToString();

        //                            //double percentage_byCount = Convert.ToDouble(dr1["CountPercentage"]);
        //                            //double percentage_byWeight = Convert.ToDouble(dr1["WeightPercentage"]);
        //                            percentage_byCount_ComFinalized += Convert.ToDouble(dr1["CountPercentage"]);
        //                            percentage_byWeight_ComFinalized += Convert.ToDouble(dr1["WeightPercentage"]);

        //                        }
        //                    }

        //                    if (dtPercentage2.Rows.Count > 0)
        //                    {
        //                        foreach (DataRow dr2 in dtPercentage2.Rows)
        //                        {
        //                            objWorkFlow.Project = dr2["Project"].ToString();
        //                            objWorkFlow.ItemReference = dr2["ItemReference"].ToString();

        //                            //double percentage_byCount = Convert.ToDouble(dr1["CountPercentage"]);
        //                            //double percentage_byWeight = Convert.ToDouble(dr1["WeightPercentage"]);
        //                            percentage_byCount_OfferReceieved += Convert.ToDouble(dr2["CountPercentage"]);
        //                            percentage_byWeight_OfferReceieved += Convert.ToDouble(dr2["WeightPercentage"]);

        //                        }
        //                    }


        //                    if (percentage_byCount_OfferReceieved < 100.00 && percentage_byWeight_ComFinalized < 100.00)
        //                    {
        //                        if ((percentage_byCount_OfferReceieved >= percentage_byWeight_OfferReceieved))
        //                        {

        //                            objWorkFlow.BusinessObjectHandle = "CT_RFQOFFERECEIVED";
        //                            objWorkFlow.UpdateTechOfferReceivedDrawingpercentage(percentage_byCount_OfferReceieved);
        //                        }
        //                        else
        //                        {
        //                            if (Itemref_Typ == "4")// when item reference typ=="Self Engineered"
        //                            {

        //                                objWorkFlow.BusinessObjectHandle = "CT_RFQOFFERECEIVED";
        //                                objWorkFlow.UpdateTechOfferReceivedDrawingpercentage(percentage_byWeight_OfferReceieved);
        //                            }
        //                            else
        //                            {

        //                                objWorkFlow.BusinessObjectHandle = "CT_RFQOFFERECEIVED";
        //                                objWorkFlow.UpdateTechOfferReceivedDrawingpercentage(percentage_byCount_OfferReceieved);
        //                            }
        //                        }
        //                    }

        //                    else
        //                    {
        //                        if (percentage_byCount_OfferReceieved >= 100)
        //                        {
        //                            objWorkFlow.BusinessObjectHandle = "CT_RFQOFFERECEIVED";
        //                            objWorkFlow.UpdateTechOfferReceivedDrawingpercentage(100);
        //                        }
        //                        else
        //                        {
        //                            objWorkFlow.BusinessObjectHandle = "CT_RFQOFFERECEIVED";
        //                            objWorkFlow.UpdateTechOfferReceivedDrawingpercentage(percentage_byCount_OfferReceieved);
        //                        }
        //                    }
        //                    if (percentage_byCount_ComFinalized < 100.00 && percentage_byWeight_ComFinalized < 100.00)
        //                            {
        //                                if ((percentage_byCount_ComFinalized >= percentage_byWeight_ComFinalized))
        //                                {
        //                                    objWorkFlow.BusinessObjectHandle = "CT_RFQCOMMERCIALFINALISED";
        //                                    objWorkFlow.UpdateTechOfferReceivedDrawingpercentage(percentage_byCount_ComFinalized);

        //                                }
        //                                else
        //                                {
        //                                    if (Itemref_Typ == "4")// when item reference typ=="Self Engineered"
        //                                    {
        //                                        objWorkFlow.BusinessObjectHandle = "CT_RFQCOMMERCIALFINALISED";
        //                                        objWorkFlow.UpdateTechOfferReceivedDrawingpercentage(percentage_byWeight_ComFinalized);

        //                                    }
        //                                    else
        //                                    {
        //                                        objWorkFlow.BusinessObjectHandle = "CT_RFQCOMMERCIALFINALISED";
        //                                        objWorkFlow.UpdateTechOfferReceivedDrawingpercentage(percentage_byCount_ComFinalized);

        //                                    }
        //                                }
        //                            }

        //                            else
        //                            {
        //                                if (percentage_byCount_ComFinalized >= 100)
        //                                {
        //                                    objWorkFlow.BusinessObjectHandle = "CT_RFQCOMMERCIALFINALISED";
        //                                    objWorkFlow.UpdateTechOfferReceivedDrawingpercentage(100);

        //                                }
        //                                else
        //                                {
        //                                    objWorkFlow.BusinessObjectHandle = "CT_RFQCOMMERCIALFINALISED";
        //                                    objWorkFlow.UpdateTechOfferReceivedDrawingpercentage(percentage_byCount_ComFinalized);

        //                                }
        //                            }





        //                    // }


        //                    //objWorkFlow.UpdateTechnicalClearancepercentage();
        //                    // }



        //                }
        //            }
        //            else
        //            {
        //            }
        //            #endregion
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert('Some technical issue');", true);
        //        }
        //    }
        //}
        protected void lnkViewWorkflow_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            Response.Redirect("ViewWorkflow.aspx?WFID=" + btn.CommandArgument + "&u=" + Request.QueryString["u"] + "&p=B");
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            Response.Redirect("RaisedEnquiry.aspx?WFPID=" + btn.CommandArgument + "&u=" + Request.QueryString["u"]);
        }
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            objWorkFlow = new WorkFlow();

            objWorkFlow.WFID = Convert.ToInt32(btn.CommandArgument);
            objWorkFlow.UserId = Request.QueryString["u"];
            objWorkFlow.WF_Status = "Technical Specification Released Returned";
            int res = objWorkFlow.UpdateWF_Status();
            // objWorkFlow.UpdateWF_StatusInBAANTable(); // Dump Preorder Data TO BAAN table change 25/08/2018 sagar 
            if (res > 0)
            {
                InsertPreHistory(Convert.ToInt32(btn.CommandArgument), "Technical Specification Released Returned");
                GetData();
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert('Successfully Return');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert('Some technical issue');", true);
            }
        }
    }
}
//        protected void btnAllOfferReceieved_Click(object sender, EventArgs e)
//        {

//            LinkButton btn = (LinkButton)sender;
//            objWorkFlow = new WorkFlow();
//            objWorkFlow.WFID = Convert.ToInt32(btn.CommandArgument);
//            objWorkFlow.UserId = Request.QueryString["u"];
//            objWorkFlow.WF_Status = "All Offer Received";
//            int res = objWorkFlow.UpdateWF_Status();
//           // objWorkFlow.UpdateWF_StatusInBAANTable(); // Dump Preorder Data TO BAAN table change 25/08/2018 sagar 
//            if (res > 0)
//            {
//                InsertPreHistory(Convert.ToInt32(btn.CommandArgument), "All Offer Received");
//               // GetData();
//                //ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert('Status Changed to All Offer Received');", true);
//            }

//            else
//            {
//                ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert('Some technical issue');", true);
//            }

//            DataTable dt12 = objWorkFlow.GetWFById();
//            if (dt12.Rows.Count > 0)
//            {
//                foreach (DataRow dr12 in dt12.Rows)
//                {
//                    objWorkFlow.Project = dr12["Project"].ToString().Substring(0,6);
//                }
//            }
//            string WFID = objWorkFlow.WFID.ToString();
//            string PMDLDocNo = "";
//            string PMDLDocs = "";
//            DataTable dtPMDL = objWorkFlow.GetMultiPMDL(WFID);
//            if (dtPMDL.Rows.Count > 0)
//            {
//                foreach (DataRow dr in dtPMDL.Rows)
//                {
//                    objWorkFlow.PMDLdocDesc = dr["PMDLDocNo"].ToString();
//                    //DataTable dataTable = objWorkFlow.GetRaisedEnquiryDate();
//                    //if (dataTable.Rows.Count > 0)
//                    //{
//                    //    objWorkFlow.ItemReference = (string)dataTable.Rows[0]["t_iref"];
//                    //    string Itemref_Typ = objWorkFlow.GetItemRefType();
//                    //    DataTable dtPMDLbyWFID = objWorkFlow.GetPMDLbyWFID();
//                    //    if (dtPMDLbyWFID.Rows.Count > 0)
//                    //    {

//                    //        for (int i = 0; i < dtPMDLbyWFID.Rows.Count; i++)
//                    //        {

//                    //            if (i == 0)
//                    //            {
//                    //                PMDLDocNo = "'" + dtPMDLbyWFID.Rows[0]["PMDLDocNo"].ToString() + "'";
//                    //            }
//                    //            else
//                    //            {
//                    //                PMDLDocNo += ",'" + dtPMDLbyWFID.Rows[i]["PMDLDocNo"].ToString() + "'";
//                    //            }

//                    //        }
//                    //    }
//                        //  objWorkFlow.UpdateAllTechOfferReceived(); //earlier TechOfferReceieved percentage is to be set to 100 now logic change- 
//                        //change date - 14-09-2018

//                        #region	Progress Update of Offer Received Percentage on  All Offer Received button click

//            //            double nOfferReceiveDrawing_Count = 0;
//            //            double nOfferReceivedDrawing_Weight = 0;

//            //            double nTotalOfferReceived_Count = 0;
//            //            double nToatlOfferReceived_Weight = 0;

//            //            double percentageTechOfferReceivedDrawing_byCount = 0;
//            //            double percentageTechOfferReceivedDrawing_byWeight = 0;

//            //            nTotalOfferReceived_Count = objWorkFlow.GetTotalInquirySentCount();
//            //            nOfferReceiveDrawing_Count = objWorkFlow.GeTechOfferReceievedDrawingCount(PMDLDocNo);

//            //            nOfferReceivedDrawing_Weight = objWorkFlow.GetCuurentRFQ_OfferReceivedDrawingWeight(PMDLDocNo);
//            //            //objWorkFlow.GetTechOfferReceivedDrawingWeight();
//            //            nToatlOfferReceived_Weight = objWorkFlow.GetTotalDrawingWeight();
//            //            //objWorkFlow.GetTotalDrawingWeight();

//            //            if (nTotalOfferReceived_Count != 0)
//            //            {
//            //                percentageTechOfferReceivedDrawing_byCount = Math.Round((nOfferReceiveDrawing_Count / nTotalOfferReceived_Count) * 100, 4);
//            //            }

//            //            if (nToatlOfferReceived_Weight != 0)
//            //            {
//            //                percentageTechOfferReceivedDrawing_byWeight = Math.Round((nOfferReceivedDrawing_Weight / nToatlOfferReceived_Weight) * 100, 4);
//            //            }
//            //            objWorkFlow.WF_Status = "All Offer Received";
//            //            objWorkFlow.BusinessObjectHandle = "CT_RFQOFFERECEIVED";
//            //            objWorkFlow.Parent_WFID= objWorkFlow.WFID;
//            //            double percentage_byCount = 0;
//            //            double percentage_byWeight = 0;

//            //            objWorkFlow.InsertIntoItemReferencewiseProgressTable(percentageTechOfferReceivedDrawing_byWeight, percentageTechOfferReceivedDrawing_byCount);
//            //            //DataTable dtPercentage = objWorkFlow.GetTechnoComNegotiationPercentagebyCount_Weight();
//            //            DataTable dtPercentage1 = objWorkFlow.GetPercentagebyCount_Weight();
//            //            if (dtPercentage1.Rows.Count > 0)
//            //            {
//            //                foreach (DataRow dr1 in dtPercentage1.Rows)
//            //                {

//            //                    objWorkFlow.Project = dr1["Project"].ToString();
//            //                    objWorkFlow.ItemReference = dr1["ItemReference"].ToString();
//            //                    //double percentage_byCount = Convert.ToDouble(dr1["CountPercentage"]);
//            //                    //double percentage_byWeight = Convert.ToDouble(dr1["WeightPercentage"]);
//            //                     percentage_byCount += Convert.ToDouble(dr1["CountPercentage"]);
//            //                     percentage_byWeight += Convert.ToDouble(dr1["WeightPercentage"]);
//            //                }
//            //            }

//            //                    if (percentage_byCount < 100.00 && percentage_byWeight < 100.00)
//            //                    {
//            //                        if ((percentage_byCount >= percentage_byWeight))
//            //                        {
//            //                            objWorkFlow.UpdateTechOfferReceivedDrawingpercentage(percentage_byCount);
//            //                        }
//            //                        else
//            //                        {
//            //                            if (Itemref_Typ == "4")// when item reference typ=="Self Engineered"
//            //                            {
//            //                                objWorkFlow.UpdateTechOfferReceivedDrawingpercentage(percentage_byWeight);
//            //                            }
//            //                            else
//            //                            {
//            //                                objWorkFlow.UpdateTechOfferReceivedDrawingpercentage(percentage_byCount);
//            //                            }
//            //                        }
//            //                    }

//            //                    else
//            //                    {
//            //                        if (percentage_byCount >= 100)
//            //                        {
//            //                            objWorkFlow.UpdateTechOfferReceivedDrawingpercentage(100);
//            //                        }
//            //                        else
//            //                        {
//            //                            objWorkFlow.UpdateTechOfferReceivedDrawingpercentage(percentage_byCount);
//            //                        }
//            //                    }
//            //                    #endregion

//            //                    #region Progress Update for Tecno Comm. Negotiation on All Offer Received Button Click

//            //                    DataTable dtPMDLDoc = objWorkFlow.GetPMDLFromItemRef();
//            //                    if (dtPMDLDoc.Rows.Count > 0)
//            //                    {

//            //                        for (int i = 0; i < dtPMDLDoc.Rows.Count; i++)
//            //                        {

//            //                            if (i == 0)
//            //                            {
//            //                                PMDLDocs = "'" + dtPMDLDoc.Rows[0]["t_docn"].ToString() + "'";
//            //                            }
//            //                            else
//            //                            {
//            //                                PMDLDocs += ",'" + dtPMDLDoc.Rows[i]["t_docn"].ToString() + "'";
//            //                            }

//            //                        }
//            //                    }


//            //                    double nTechnoComNegotiationDrawing_Count = 0;
//            //                    double nTechnoComNegotiationDrawing_Weight = 0;

//            //                    double nTotalTechnoComNegotiation_Count = 0;
//            //                    double nTotalTechnoComNegotiation_Weight = 0;

//            //                    double percentageTechnoComNegotiation_byCount = 0;
//            //                    double percentageTechnoComNegotiatio_byWeight = 0;

//            //                   // nTotalTechnoComNegotiation_Count = objWorkFlow.GetTotalChildRecordCount(PMDLDocNo);
//            //                    nTotalTechnoComNegotiation_Count = objWorkFlow.AllOfferReceieved_ChildRecordCount(PMDLDocNo);
//            //                    //3
//            //                    nTechnoComNegotiationDrawing_Count = objWorkFlow.GetTechnoCommercialChildRecordCount(PMDLDocNo);
//            //                   //objWorkFlow.GetCuurentRFQ_OfferReceivedDrawingWeight
//            //                   nTechnoComNegotiationDrawing_Weight = objWorkFlow.GetCuurentRFQComNegotiation_OfferReceivedDrawingWeight(PMDLDocNo);
//            //                    nTotalTechnoComNegotiation_Weight = objWorkFlow.GetTotalDrawingWeight();

//            //                    if (nTotalTechnoComNegotiation_Count != 0 && nTotalOfferReceived_Count != 0)
//            //                    {
//            //                        percentageTechnoComNegotiation_byCount = Math.Round((nTechnoComNegotiationDrawing_Count / nTotalTechnoComNegotiation_Count)
//            //                                                               * (nOfferReceiveDrawing_Count / nTotalOfferReceived_Count) * 100, 4);
//            //                    }
//            //                    if (nToatlOfferReceived_Weight != 0 && nTotalTechnoComNegotiation_Weight != 0)
//            //                    {
//            //                // percentageTechnoComNegotiatio_byWeight = Math.Round((nTechnoComNegotiationDrawing_Weight / nTotalTechnoComNegotiation_Weight) * (nOfferReceivedDrawing_Weight / nToatlOfferReceived_Weight) * 100, 4);
//            //                percentageTechnoComNegotiatio_byWeight = Math.Round((nTechnoComNegotiationDrawing_Weight / nTotalTechnoComNegotiation_Weight) * (nTechnoComNegotiationDrawing_Count / nTotalTechnoComNegotiation_Count) * 100, 4);
//            //            }
//            //                      objWorkFlow.Parent_WFID = objWorkFlow.WFID;
//            //                     objWorkFlow.BusinessObjectHandle = "CT_RFQCOMMERCIALFINALISED";
//            //                    objWorkFlow.WF_Status = "All Offer Received";

//            //                    objWorkFlow.InsertIntoItemReferencewiseProgressTable(percentageTechnoComNegotiatio_byWeight, percentageTechnoComNegotiation_byCount);
//            //                    //DataTable dtPercentage = objWorkFlow.GetTechnoComNegotiationPercentagebyCount_Weight();
//            //                    DataTable dtPercentage = objWorkFlow.GetPercentagebyCount_Weight();
//            //            double percentage_byCount1 = 0;
//            //            double percentage_byWeight1 = 0;

//            //            if (dtPercentage.Rows.Count > 0)
//            //            {
//            //                foreach (DataRow dr2 in dtPercentage.Rows)
//            //                {
//            //                    objWorkFlow.Project = dr2["Project"].ToString();
//            //                    objWorkFlow.ItemReference = dr2["ItemReference"].ToString();
//            //                    //double percentage_byCount1 = Convert.ToDouble(dr2["CountPercentage"]);
//            //                    //double percentage_byWeight1 = Convert.ToDouble(dr2["WeightPercentage"]);
//            //                     percentage_byCount1 += Convert.ToDouble(dr2["CountPercentage"]);
//            //                     percentage_byWeight1 += Convert.ToDouble(dr2["WeightPercentage"]);
//            //                }
//            //            }

//            //                            if (percentage_byCount1 < 100.00 && percentage_byWeight1 < 100.00)
//            //                            {
//            //                                if ((percentage_byCount1 >= percentage_byWeight1))
//            //                                {
//            //                                    objWorkFlow.UpdateTechOfferReceivedDrawingpercentage(percentage_byCount1);
//            //                                }
//            //                                else
//            //                                {
//            //                                    if (Itemref_Typ == "4")// when item reference typ=="Self Engineered"
//            //                                    {
//            //                                        objWorkFlow.UpdateTechOfferReceivedDrawingpercentage(percentage_byWeight1);
//            //                                    }
//            //                                    else
//            //                                    {
//            //                                        objWorkFlow.UpdateTechOfferReceivedDrawingpercentage(percentage_byCount1);
//            //                                    }
//            //                                }
//            //                            }

//            //                            else
//            //                            {
//            //                                if (percentage_byCount1 >= 100)
//            //                                {
//            //                                    objWorkFlow.UpdateTechOfferReceivedDrawingpercentage(100);
//            //                                }
//            //                                else
//            //                                {
//            //                                    objWorkFlow.UpdateTechOfferReceivedDrawingpercentage(percentage_byCount1);
//            //                                }
//            //                            }
//            //                    //    }
//            //                    //}
//            //                    #endregion

#endregion