using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PreOrderWorkflow
{
    public partial class GeneratePreOrderReceipt : System.Web.UI.Page
    {
        //  public static string cs = ConfigurationManager.AppSettings["ConnectionTest"];ConnectionLive
        public static string csLive = ConfigurationManager.AppSettings["ConnectionLive"];
        public static string csERPLive = ConfigurationManager.AppSettings["Connection"];
        //protected void Page_PreRender(object sender, EventArgs e)
        //{
        //    if (Session["IndentNo"] != null)
        //    { //ddlIndent.SelectedValue = Session["IndentNo"].ToString();

        //        ddlIndent.Items.Add(Session["IndentNo"].ToString());
        //    }
        //    if (Session["item"] != null)
        //    { //ddlItem.SelectedValue = Session["item"].ToString();
        //        ddlItem.Enabled = true;
        //        ddlItem.Items.Add(Session["item"].ToString());
        //    }
        //}
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session != null)
                {
                    txtProject.Text = Session["Project"].ToString();
                    txtElement.Text = Session["Element"].ToString();
                    txtSpecification.Text = Session["SpecificationNo"].ToString();
                    txtPMDLDoc.Text = Session["PMDLDocNo"].ToString();
                    txtBuyer.Text = Session["BuyerName"].ToString();
                    txtManager.Text = Session["ManagerName"].ToString();
                    txtSupplierEmail.Text = Session["SupplierEmail"].ToString();
                    if (Session["SupplierName"].ToString() != "" && Session["SupplierName"].ToString() != null)
                    {
                        txtSupplier.Text = Session["SupplierName"].ToString();
                        //+ "-" + Session["SupplierCode"].ToString();
                    }
                    if (Session["SupplierCode"].ToString() != "" && Session["SupplierCode"].ToString() != null)
                    {
                        txtSupplierCode.Text = Session["SupplierCode"].ToString();
                        //+ "-" + Session["SupplierCode"].ToString();
                    }
                                       //Session["BuyerId"] = hdfBuyerId.Value;
                    //Session["RandomNo"] = hdfRandomNo.Value;
                }
                using (SqlConnection con = new SqlConnection(csERPLive))
                {
                    string RecieptNumber = @"Select ReceiptNo from  WF1_Preorder where  WFID=" + Request.QueryString["WFID"] + "";
                    SqlConnection conERPLive = new SqlConnection(csERPLive); // comment out this line after Testing sagar
                    SqlCommand cmdRecieptNumber = new SqlCommand(RecieptNumber, conERPLive); // changeconlive to con sagar
                    cmdRecieptNumber.CommandType = CommandType.Text;
                    conERPLive.Open(); // for test use only ....comment later sagar
                    string RecieptNo = cmdRecieptNumber.ExecuteScalar().ToString();

                    // DataTable dtRecieptRecord = SqlHelper.ExecuteDataset(con, CommandType.Text, RecieptNumber).Tables[0];

                    conERPLive.Close();
                    if (RecieptNo == "")
                    {
                        btnIdmsReceipt.Enabled = true;
                        btnAttachment.Enabled = true;
                        btnIdmsReceipt.Text = "Generate Receipt";
                    }
                    else
                    {
                        using (SqlConnection conLive = new SqlConnection(csLive))
                        {
                            string RecieptRecord = @"Select t_item,t_rqno, t_stat,t_revn from tdmisg134200 where t_rcno = '" + RecieptNo + "' order by t_revn desc";
                            //SqlCommand cmdRecieptStatus = new SqlCommand(RecieptStatus, conLive); // changeconlive to con sagar
                            //cmdRecieptNumber.CommandType = CommandType.Text;
                            if (conLive.State != ConnectionState.Open)
                            {
                                conLive.Open();
                            }
                            //int ReceiptStatus = (int)cmdRecieptStatus.ExecuteScalar();
                            DataTable dtRecieptRecord = SqlHelper.ExecuteDataset(conLive, CommandType.Text, RecieptRecord).Tables[0];
                            conLive.Close();
                            if (dtRecieptRecord.Rows.Count > 0)
                            {
                                Session["item"] = dtRecieptRecord.Rows[0]["t_item"].ToString();
                                Session["IndentNo"] = dtRecieptRecord.Rows[0]["t_rqno"].ToString();


                                if ((int)dtRecieptRecord.Rows[0]["t_stat"] == 4)
                                {
                                    btnIdmsReceipt.Width = 250;
                                    btnIdmsReceipt.Text = "Revise Receipt No - " + RecieptNo + "";
                                    btnIdmsReceipt.Enabled = true;
                                    btnAttachment.Enabled = true;
                                    lblNotify.Visible = false;

                                    // string UpdateRECStatus = @"update tdmisg134200 set t_stat = 7 where t_rcno = '" + RecieptNo + "' and t_revn = '" + dtRecieptRecord.Rows[0]["t_revn"].ToString() + "' ";
                                    // SqlCommand cmd = new SqlCommand(UpdateRECStatus, conLive);
                                    // cmd.CommandType = CommandType.Text;
                                    //if (conLive.State != ConnectionState.Open)
                                    // {
                                    //  conLive.Open();
                                    // }
                                    // cmd.ExecuteNonQuery();

                                }
                                else
                                {
                                    btnIdmsReceipt.Text = "Generate Receipt";
                                    btnIdmsReceipt.Enabled = false;
                                    btnAttachment.Enabled = false;
                                    lblNotify.Visible = true;
                                    lblNotify.Text = " Receipt:- '" + RecieptNo + "' has been already generated for WFID '" + Request.QueryString["WFPID"] + "'";
                                }
                            }
                            else
                            {
                                btnIdmsReceipt.Text = "Generate Receipt";
                                btnIdmsReceipt.Enabled = false;
                                btnAttachment.Enabled = false;
                                lblNotify.Visible = true;
                                lblNotify.Text = " Receipt:- '" + RecieptNo + "' has been already generated for WFID '" + Request.QueryString["WFPID"] + "'";
                            }
                        }
                    }
                    if (Session["IndentNo"] != null)
                    {
                        // ddlIndent.SelectedValue = Session["IndentNo"].ToString();
                        ddlIndent.Items.Add(Session["IndentNo"].ToString());
                        ddlIndent.Enabled = false;
                    }
                    else
                    {
                        GetIndent();
                    }
                    if (Session["item"] != null)
                    {

                        ddlItem.Items.Add(Session["item"].ToString());
                        // ddlItem.SelectedValue = Session["item"].ToString();
                        ddlItem.Enabled = false;
                    }
                    else { GetItem(); }
                }
            }

            //}

        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod()]
        public static string[] GetSupplier(string prefixText, int count)
        {
            WorkFlow objWorkFlow = new WorkFlow();
            objWorkFlow.Supplier = prefixText;
            DataTable dt = objWorkFlow.GetSupplier();
            List<string> lst = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                string Supplier = dr["SupplierName"].ToString() + "-" + dr["SupplierCode"].ToString();
                lst.Add(Supplier);
            }
            return lst.ToArray();


        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod()]
        public static string[] GetItemMethod(string prefixText, int count)
        {

            //string sElement = (string)HttpContext.Current.Session["Element"];
            WorkFlow objWorkFlow = new WorkFlow();
            DataTable dt = objWorkFlow.GetItem();
            List<string> lst = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                string item = dr["t_item"].ToString();
                lst.Add(item);
            }
            return lst.ToArray();
        }

        private void GetItem()
        {
            WorkFlow objWorkFlow = new WorkFlow();

            // WorkFlow objWorkFlow = new WorkFlow();
            string[] Project = txtProject.Text.Split('-');
            objWorkFlow.Project = Project[0];
            string[] Element = txtElement.Text.Split('-');
            objWorkFlow.Element = Element[0];
            DataTable dt = objWorkFlow.GetItemforSelectedproj_Element();
            if (dt.Rows.Count > 0 && dt.Rows[0]["t_item"].ToString() != "")
            {
                ddlItem.Enabled = true;
                ddlItem.DataSource = dt;
                ddlItem.DataValueField = "t_item";
                ddlItem.DataTextField = "t_item";
                ddlItem.DataBind();
                ddlItem.Items.Insert(0, "Select");
            }
            else
            {
                DataTable dtall = objWorkFlow.GetItem();
                if (dtall.Rows.Count > 0 && dtall.Rows[0]["t_item"].ToString() != "")
                {
                    ddlItem.Enabled = true;
                    ddlItem.DataSource = dtall;
                    ddlItem.DataValueField = "t_item";
                    ddlItem.DataTextField = "t_item";
                    ddlItem.DataBind();
                    ddlItem.Items.Insert(0, "Select");
                }
                else
                {
                    ddlItem.Enabled = false;
                }

            }
        }
        private void GetIndent()
        {
            WorkFlow objWorkFlow = new WorkFlow();
            string[] Project = txtProject.Text.Split('-');
            objWorkFlow.Project = Project[0];
            string[] Element = txtElement.Text.Split('-');
            objWorkFlow.Element = Element[0];
            DataTable dt = objWorkFlow.GetIndent();
            if (dt.Rows.Count > 0)
            {
                ddlIndent.DataSource = dt;
                ddlIndent.DataValueField = "t_rqno";
                ddlIndent.DataTextField = "drpdwn";
                ddlIndent.DataBind();
                ddlIndent.Items.Insert(0, "Select");
            }
            else
            {

            }
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod()]
        public static string[] GetIndentMethod(string prefixText, int count)
        {
            string sProj = HttpContext.Current.Session["Project"].ToString();
            int indexstart = sProj.IndexOf('-');
            string ProjectId = sProj.Substring(0, indexstart);
            //txtProject.Text.ToString();
            //HttpContext.Current.Session["Project"].ToString();
            //string sProj = (string)HttpContext.Current.Session["ProjName"];
            //string sElement = (string)HttpContext.Current.Session["Element"];
            WorkFlow objWorkFlow = new WorkFlow();
            DataTable dt = objWorkFlow.GetIndent(prefixText, ProjectId);
            List<string> lst = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                string indent = dr["t_rqno"].ToString() + " ( " + dr["t_rqln"].ToString() + ")";

                lst.Add(indent);
            }
            return lst.ToArray();
        }
        protected void ddlItem_SelectedIndexChange(object sender, EventArgs e)
        {
            Session["item"] = ddlItem.SelectedValue;
            if (ddlItem.SelectedValue != "Select")

            {
                lblNotify.Visible = false;
            }

        }
        protected void ddlIndent_SelectedIndexChange(object sender, EventArgs e)
        {
            Session["IndentNo"] = ddlIndent.SelectedValue;
        }

        protected void btnAttachment_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["WFPID"] != null && ddlItem.SelectedValue != "Select")
            {
                string sRECNo = "";
                string revision = "00";
                //  string sRECNo = GetNextRecNo();
                if (!btnIdmsReceipt.Text.Contains("-"))
                {
                    sRECNo = "REC" + GetNextRecNo();
                }
                else
                {
                    string[] btntext = btnIdmsReceipt.Text.Split('-');
                    sRECNo = btntext[1].Trim();

                    //SqlCommand cmdRecieptStatus = new SqlCommand(RecieptStatus, conLive); // changeconlive to con sagar
                    //cmdRecieptNumber.CommandType = CommandType.Text;

                }
                string RecieptRecord = @"Select count(*) as count from tdmisg134200 where t_rcno = '" + sRECNo + "'";
                using (SqlConnection conLive = new SqlConnection(csLive))
                {
                    if (conLive.State != ConnectionState.Open)
                    {
                        conLive.Open();
                    }
                    //int ReceiptStatus = (int)cmdRecieptStatus.ExecuteScalar();
                    DataTable dtRecieptRecord = SqlHelper.ExecuteDataset(conLive, CommandType.Text, RecieptRecord).Tables[0];
                    if(dtRecieptRecord.Rows.Count>0)
                    {
                      string Count = dtRecieptRecord.Rows[0]["count"].ToString();
                      revision= Count.ToString().PadLeft(2, '0');

                    }
                    //} "Notes.aspx?handle=J_PREORDER_WORKFLOW&Index=" + Request.QueryString["WFID"] + "&AttachedBy=" + Request.QueryString["u"] + "&RecNo=" + sRECNo + "&Item=" + ddlItem.SelectedValue + "&Indent=" + ddlIndent.SelectedValue + "&Revision=" + revision;
                    string sAttachOffer = "Notes.aspx?handle=J_PREORDER_WORKFLOW&Index=" + Request.QueryString["WFID"] + "&AttachedBy=" + Request.QueryString["u"] + "&RecNo=" + sRECNo + "&Item=" + ddlItem.SelectedValue + "&Indent=" + ddlIndent.SelectedValue + "&Revision=" + revision;
                    Response.Redirect(sAttachOffer);
                    // //IDMSRECEIPTS_200 workflow handle= J_PREORDER_WORKFLOW ---- IDMSRECEIPTS_200 JOOMLA_NOTES Notes1126 237
                    // string url = "Notes.aspx?handle=J_PREORDER_WORKFLOW&Index=" + Request.QueryString["WFPID"] + "&AttachedBy=" + Request.QueryString["u"] +"&RecNo=" + sRECNo;
                    //// string url = "http://192.9.200.146/Attachment/Attachment.aspx?AthHandle=JOOMLA_NOTES" + "&Index=" + "Notes1124" + "&AttachedBy=" +"9583" + "&ed=n";
                    // //string url = "http://192.9.200.146/Attachment/Attachment.aspx?AthHandle=J_PREORDER_WORKFLOW" + "&Index=" + Request.QueryString["WFPID"] + "&AttachedBy=" + Request.QueryString["u"] + "&ed=n";
                    // string s = "window.open('" + url + "','abc','width=800,height=600,left=100,top=100,resizable=yes,scrollbars=yes');"; //, 'width=300,height=100,left=100,top=100,resizable=yes'
                    // ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                }
            }

            else
            {
                lblNotify.Visible = true;
                lblNotify.Text = "Please Select Item No. before clicking on Attach Offer!";
            }
        }

        public static string GetNextRecNo()
        {
            string NextNo = "";
            using (SqlConnection con = new SqlConnection(csLive))
            {

                string RECno = @"select t_ffno from ttcmcs050200 where t_nrgr='VEN' and t_seri='REC'";
                SqlCommand cmd = new SqlCommand(RECno, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                NextNo = cmd.ExecuteScalar().ToString();
                // private int tmp = (Convert.ToInt32(NextNo) + 1);
            }
            int tmp = (Convert.ToInt32(NextNo) + 1);
            using (SqlConnection con = new SqlConnection(csLive))
            {
                string UpdateRECno = @"update ttcmcs050200 set t_ffno = "
            + tmp + " where t_nrgr='VEN' and t_seri='REC'";
                SqlCommand cmd = new SqlCommand(UpdateRECno, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                cmd.ExecuteNonQuery();

            }
            string NextNumber = NextNo.PadLeft(6, '0');
            return NextNumber;
            // private SqlCommand Cmd = Con.CreateCommand();
            //private int tmp = (Convert.ToInt32(NextNo) + 1);


        }

        protected void btnIdmsReceipt_Click(object sender, EventArgs e)
        {
            btnIdmsReceipt.Enabled = false;
            btnAttachment.Enabled = false;
            if (btnIdmsReceipt.Text.Contains("-"))
            {
                string[] btntext = btnIdmsReceipt.Text.Split('-');
                string RECEIPTNumber = btntext[1].Trim();
                Session["ReceiptNo"] = RECEIPTNumber;
                using (SqlConnection conLive = new SqlConnection(csLive))   // change csLive to csLive everywhere inside sql connection ....this is just for testing sagar

                {
                    string RevisionNo = @"select count(*) from tdmisg134200 where t_rcno ='" + RECEIPTNumber + "'";
                    SqlCommand cmdDivision = new SqlCommand(RevisionNo, conLive); // changeconlive to con sagar
                    cmdDivision.CommandType = CommandType.Text;
                    if (conLive.State != ConnectionState.Open)
                    {
                        conLive.Open();
                    }
                    int RevisionCount = (int)cmdDivision.ExecuteScalar();
                    conLive.Close();
                    Session["RevisionNo"] = RevisionCount.ToString().PadLeft(2, '0');
                    string RecieptRecord = @"Select t_item,t_rqno, t_stat,t_revn from tdmisg134200 where t_rcno = '" + RECEIPTNumber + "' order by t_revn desc";
                    //SqlCommand cmdRecieptStatus = new SqlCommand(RecieptStatus, conLive); // changeconlive to con sagar
                    //cmdRecieptNumber.CommandType = CommandType.Text;
                    if (conLive.State != ConnectionState.Open)
                    {
                        conLive.Open();
                    }
                    //int ReceiptStatus = (int)cmdRecieptStatus.ExecuteScalar();
                    DataTable dtRecieptRecord = SqlHelper.ExecuteDataset(conLive, CommandType.Text, RecieptRecord).Tables[0];

                    string UpdateRECStatus = @"update tdmisg134200 set t_stat = 7 where t_rcno = '" + RECEIPTNumber + "' and t_revn = '" + dtRecieptRecord.Rows[0]["t_revn"].ToString() + "' ";
                    SqlCommand cmd = new SqlCommand(UpdateRECStatus, conLive);
                    cmd.CommandType = CommandType.Text;
                    if (conLive.State != ConnectionState.Open)
                    {
                        conLive.Open();
                    }
                    cmd.ExecuteNonQuery();
                    conLive.Close();


                }
            }
                if (ddlItem.SelectedValue != null && ddlItem.SelectedValue != "" && ddlItem.SelectedValue != "Select")
                {
                    int nRecord = 0;
                    string SupplierName = "";
                    string SupplierCode = "";
                    string IndentNo = "";
                    string LineNo = "";
                    string RECNumber = "";
                    string RevisionCount = "00";
                    if (Session["ReceiptNo"] != null)
                    {
                        RECNumber = Session["ReceiptNo"].ToString();
                    }
                    else
                    {
                        RECNumber = "REC" + GetNextRecNo();
                    }
                    if (Session["RevisionNo"] != null)
                    {
                        RevisionCount = (string)Session["RevisionNo"];
                    }
                    else
                    {
                        RevisionCount = "00";
                    }
                    // String sSpace=new String(' ', 9);
                    string itemNo = ddlItem.SelectedValue;
                    // string item = sSpace + itemNo;
                    //Session["item"] = item;
                    string user = Request.QueryString["u"];
                    string Project = txtProject.Text.ToString();
                    int indexProjstart = Project.IndexOf('-');
                    string ProjCode = Project.Substring(0, indexProjstart);
                ////if (txtSupplier.Text != "")
                ////{

                ////    if (txtSupplier.Text.ToString().Contains("-"))
                ////    {
                ////        string[] supplier = txtSupplier.Text.Split('-');

                ////        if (supplier[1] != null && Regex.IsMatch(supplier[1], @"[^ a - zA - Z0 - 9]"))
                ////        {
                ////            SupplierName = supplier[0];
                ////            SupplierCode = supplier[1];
                ////        }
                ////        else
                ////        {
                ////            SupplierName = txtSupplier.Text;
                ////            SupplierCode = "";
                ////        }
                ////    }
                ////    else
                ////    {
                ////        if (txtSupplier.Text.ToString().Contains("SUP"))
                ////        {
                ////            SupplierCode = txtSupplier.Text;
                ////            SupplierName = "";
                ////        }
                ////        else
                ////        {
                ////            SupplierName = txtSupplier.Text;
                ////            SupplierCode = "";
                ////        }
                ////    }
                ////}
                    SupplierName = txtSupplier.Text;
                    SupplierCode = txtSupplierCode.Text;
                    string IndentNoLineNo = ddlIndent.SelectedValue;
                    if (IndentNoLineNo != null && IndentNoLineNo != "")
                    {
                        if (IndentNoLineNo.Contains("-"))
                        {
                            string[] Indent_Line = ddlIndent.SelectedValue.Split('-');

                            //    int indexstart = IndentNoLineNo.IndexOf('(');
                            //int indexEnd = IndentNoLineNo.IndexOf(')');
                            IndentNo = Indent_Line[0];
                            //IndentNoLineNo.Substring(0, indexstart);
                            LineNo = Indent_Line[1];
                            //IndentNoLineNo.Substring(indexstart + 1, indexEnd - (indexstart + 1));
                        }
                    }
                    else
                    {

                    }
                    //Session["IndentNo"] = IndentNoLineNo;
                    using (SqlConnection con = new SqlConnection(csLive))   // change csLive to csLive everywhere inside sql connection ....this is just for testing sagar

                    {
                        con.Open();
                        string DivisionQuery = @"select EnterpriseUnit from LN_ProjectMaster where ProjectCode='" + ProjCode + "'";
                        SqlConnection conTest = new SqlConnection(csLive); // comment out this line after Testing sagar
                        SqlCommand cmdDivision = new SqlCommand(DivisionQuery, conTest); // changeconlive to con sagar
                        cmdDivision.CommandType = CommandType.Text;
                        conTest.Open(); // for test use only ....comment later sagar
                        string DivisionName = cmdDivision.ExecuteScalar().ToString();
                        conTest.Close(); // for test use only .... need to comment out later sagar

                        string TotalRecordForReceipt = @"select count(*) from tdmisg134200 where t_rcno='" + RECNumber + "'";
                        SqlCommand cmdRecordForReceipt = new SqlCommand(TotalRecordForReceipt, con);
                        cmdRecordForReceipt.CommandType = CommandType.Text;
                        string dResult = cmdRecordForReceipt.ExecuteScalar().ToString();
                        nRecord = Convert.ToInt32(dResult);
                        nRecord = nRecord + 1;
                        string InsertParentRecord = @"Insert into tdmisg134200(t_atch,t_stat,t_sent_1,t_rece_1,t_suer,t_sdat,t_appr,
                                        t_adat,t_subm_1,t_orno,t_pono,t_trno,t_docn,t_revn,t_eunt,t_rqno,t_rqln,
                                        t_rcno,t_cprj,t_item,t_bpid,t_user,t_nama,t_date,t_sent_2,t_sent_3,t_sent_4,t_sent_5,
                                        t_sent_6,t_sent_7,t_rece_2,t_rece_3,t_rece_4,t_rece_5,t_rece_6,t_rece_7,t_subm_2,
                                        t_subm_3,t_subm_4,t_subm_5,t_subm_6,t_subm_7,t_Refcntd,t_Refcntu)  
                                        values('W',1,2,2,'','','','',2,'',0,'','CRI',
                                       '" + RevisionCount + "', '" + DivisionName + "','" + IndentNo + "','" + LineNo + "','"
                                             + RECNumber + "','" + ProjCode + "','" + itemNo + "','" + SupplierCode + "','" + user
                                             + "',left('" + SupplierName + "',30),'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")
                                             + "','','','','','','','','','','','','','','','','','','','','')";

                    // ";  values('','','','','',0,'',2,'" + RevisionCount + "','" + RECNumber + "','" + nRecord + "','','')";
                    string InsertChildRecord = @"Insert into tdmisg135200 (t_docn,t_revi,t_dsca,t_idoc,t_irev,
                                            t_date,t_remk,t_proc,t_revn,t_rcno,t_srno,t_Refcntd,t_Refcntu) 
                                            values('', '', '', '', '', 0, '', 2, '" + RevisionCount + "', '" + RECNumber + "', 1, '', '')";
                        SqlCommand cmd = new SqlCommand(InsertParentRecord, con);
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();

                        SqlCommand CmdChild = new SqlCommand(InsertChildRecord, con);
                        CmdChild.CommandType = CommandType.Text;
                        CmdChild.ExecuteNonQuery();
                        con.Close();
                        string PreorderReceiptInsertion = @"Update  WF1_Preorder set ReceiptNo= '" + RECNumber + "' where  WFID=" + Request.QueryString["WFID"] + "";
                    //// Dump Preorder Data TO BAAN table change 25/08/2018 sagar 
                    //string PreorderReceiptInsertionToBAAN = @"update tdmisg168200 set t_rcno = '" + RECNumber + "' where  t_wfid =" + Request.QueryString["WFPID"] + " ";
                    //if (con.State != ConnectionState.Open)
                    //{
                    //    con.Open();
                    //}
                    //SqlCommand CmdInsertReceiptTOBAAN = new SqlCommand(PreorderReceiptInsertionToBAAN, con);
                    //CmdInsertReceiptTOBAAN.CommandType = CommandType.Text;
                    //CmdInsertReceiptTOBAAN.ExecuteNonQuery();
                    //con.Close();
                    SqlConnection conERPLive = new SqlConnection(csERPLive); // comment out this line after Testing sagar
                        SqlCommand cmdPreorderReceipt = new SqlCommand(PreorderReceiptInsertion, conERPLive); // changeconlive to con sagar
                        cmdPreorderReceipt.CommandType = CommandType.Text;
                        conERPLive.Open(); // for test use only ....comment later sagar
                        cmdPreorderReceipt.ExecuteNonQuery();
                        conERPLive.Close();
                       // string InsertPMDLrecord = @" insert into tdmisg167200 values(" + Request.QueryString["WFPID"] + ",)";
                        lblNotify.Visible = true;
                        lblNotify.ForeColor = System.Drawing.Color.Green;
                        lblNotify.Text = " Receipt:- '" + RECNumber + "' has been generated for WFID '" + Request.QueryString["WFPID"] + "'";
                        // write the actual value for mFileName, ReceiptNo, RevisionNo below
                        string sFilename = "IDMS_Receipt_No. (" + RECNumber + ")_" + DateTime.Now.ToString("dd-MM-yyyy HH-mm") + ".xml";
                    //System.IO.StreamWriter oTW = new System.IO.StreamWriter(@"E:\Temp\" + sFilename); //for local debugging
                    System.IO.StreamWriter oTW = new System.IO.StreamWriter(@"C:\inetpub\wwwroot\App_Temp\TABill\s\" + sFilename);
                        oTW.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                        oTW.WriteLine("<ERPFunctions>");
                        oTW.WriteLine("  <Function dll=\"odmisgdll0100\" fun=\"dmisgdll0100.forward.receipt.to.departments\" >" + RECNumber + "," + 0 + ",</Function>");
                        oTW.WriteLine("</ERPFunctions>");
                        oTW.Close();
                    }
                }
                else
                {
                    lblNotify.Visible = true;
                    lblNotify.ForeColor = System.Drawing.Color.Red;
                    lblNotify.Text = "Please select Item from Dropdown and then click on Generate IDMS Receipt button!";
                }
            }
        }

    }


           
  

      

  

     
       

