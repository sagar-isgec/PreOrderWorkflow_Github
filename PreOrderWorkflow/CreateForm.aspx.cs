using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.Services;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Web.UI.WebControls;
namespace PreOrderWorkflow
{
    public partial class CreateForm : System.Web.UI.Page
    {
        WorkFlow objWorkFlow;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["WFID"] != null && Request.QueryString["Status"] == "Technical Specification Released Returned")
                {
                    GetData();
                    btnSave.Text = "ReSubmit";
                    btnNotes.Visible = true;
                }
                if (Request.QueryString["WFID"] != null && Request.QueryString["Status"] == "Created")
                {
                    GetData();
                    btnSave.Text = "Release";
                }
                
                GetProject();
                GetProjectElement();
                if (Request.QueryString["WFID"] != null)
                {
                    GetPMDL();
                    btnSave.Text = "Release";
                }
                GetBuyer();
                GetManager();
                chkbx1.Visible = false;
                chkbx2.Visible = false;
                chkbx3.Visible = false;
               // lblPMDL.Text= (ddlPMDL.GetSelectedIndices().Count()).ToString();
                // ddlPMDL.SelectionMode = ListSelectionMode.Multiple;
                // BindYourControl();
            }
        }

        private void GetData()
        {
            objWorkFlow = new WorkFlow();
            objWorkFlow.WFID = Convert.ToInt32(Request.QueryString["WFID"]);
            DataTable dt = objWorkFlow.GetWFById();
            DataTable dtGetNotes = objWorkFlow.GetHistory();
            if (dt.Rows.Count > 0)
            {
                string[] Project = dt.Rows[0]["Project"].ToString().Split('-');
                string[] Element = dt.Rows[0]["Element"].ToString().Split('-');
                ddlProject.SelectedValue = Project[0];
                lblProjectName.Text = Project[1];
                ddlElement.Text = Element[0];
                lblProjectElementName.Text = Element[1];
                if (dt.Rows[0]["PMDLDocNo"].ToString().Contains(","))
                {
                    string[] valuesArray = dt.Rows[0]["PMDLDocNo"].ToString().Split(',');
                    var list = valuesArray.ToList();
                    foreach (ListItem i in ddlPMDL.Items)
                    {
                        i.Selected = list.Contains(i.Value);
                    }
                }
                else
                {
                    ddlPMDL.SelectedValue = dt.Rows[0]["PMDLDocNo"].ToString();
                }
                ddlBuyer.SelectedValue = dt.Rows[0]["Buyer"].ToString();
                lblBuyerName.Text = dt.Rows[0]["BuyerName"].ToString();
                ddlManager.SelectedValue = dt.Rows[0]["Manager"].ToString();
                lblManagername.Text = dt.Rows[0]["ManagerName"].ToString();
                // ddlSpecification.SelectedValue= dt.Rows[0]["SpecificationNo"].ToString();
                txtSpecification.Text = dt.Rows[0]["SpecificationNo"].ToString();
                //  txtPMDLdoc.Text = dt.Rows[0]["PMDLDocNo"].ToString();
                // txtSpecification.Text = dt.Rows[0]["SpecificationNo"].ToString();
                //txtBuyer.Text = dt.Rows[0]["BuyerName"].ToString() + "-" + dt.Rows[0]["Buyer"].ToString();
                // hdfStatus.Value = dt.Rows[0]["WF_Status"].ToString();
                // hdfWFID.Value = dt.Rows[0]["WFID"].ToString();
                // txtManager.Text = dt.Rows[0]["ManagerName"].ToString() + "-" + dt.Rows[0]["Manager"].ToString();
            }
            if (dtGetNotes.Rows.Count > 0)
            {
                txtNotes.Text = dtGetNotes.Rows[0]["Notes"].ToString();
            }
        }
        private void GetProject()
        {
            objWorkFlow = new WorkFlow();
            DataTable dt = objWorkFlow.GetProject();
            ddlProject.DataSource = dt;
            ddlProject.DataValueField = "ProjectCode";
            ddlProject.DataTextField = "ProjectCode";
            ddlProject.DataBind();
            ddlProject.Items.Insert(0, "Select");
        }
        private void GetProjectElement()
        {
            objWorkFlow = new WorkFlow();
            DataTable dt = objWorkFlow.GetProjectElement(ddlProject.SelectedValue);
            ddlElement.DataSource = dt;
            ddlElement.DataValueField = "Element";
            ddlElement.DataTextField = "Element";
            ddlElement.DataBind();
            ddlElement.Items.Insert(0, "Select");
        }
        //private void GetSpecification()
        //{
        //    objWorkFlow = new WorkFlow();
        //    DataTable dt = objWorkFlow.GetProjectSpecification(ddlProject.SelectedValue,ddlElement.SelectedValue);
        //    if (dt.Rows.Count > 0)
        //    {
        //        ddlSpecification.DataSource = dt;
        //        ddlSpecification.DataValueField = "DocumentID";
        //        ddlSpecification.DataTextField = "DocumentID";
        //        ddlSpecification.DataBind();
        //        ddlSpecification.Items.Insert(0, "Select");
        //       // txtSpecification.Visible = false;
        //        ddlSpecification.Visible = true;
        //        lblSpecDesc.Visible = true;
        //    }
        //    else
        //    {
        //        //txtSpecification.Visible = true;
        //        //  ddlSpecification.Visible = false;
        //        lblSpecDesc.Visible = false;
        //    }
        //}
        private void GetPMDL()
        {
            objWorkFlow = new WorkFlow();
            DataTable dt = objWorkFlow.GetPMDLdoc(ddlProject.SelectedValue, ddlElement.SelectedValue);
            if (dt.Rows.Count > 0)
            {
                ddlPMDL.DataSource = dt;
                ddlPMDL.DataValueField = "DocumentID";
                ddlPMDL.DataTextField = "PMDLDoc";
                ddlPMDL.DataBind();
                //  ddlPMDL.Items.Insert(0, "Select");
                //txtSpecification.Visible = false;
                ddlPMDL.Enabled = true;
                ddlPMDL.Visible = true;
                lblPMDL.Visible = false;
                chkbx1.Visible = true;
                chkbx2.Visible = true;
                chkbx3.Visible = true;
                chkbx1.Checked = true;
            }
            else
            {
                //txtSpecification.Visible = true;
                //  ddlSpecification.Visible = false;
                lblPMDL.Visible = true;
                ddlPMDL.Enabled = false;
                lblPMDL.Text = "No PMDL Doc for selected Project & Element";
                chkbx1.Visible = false;
                chkbx2.Visible = false;
                chkbx3.Visible = false;

                //lblPMDL.Visible = false;
            }
        }

        private void GetBuyer()
        {
            objWorkFlow = new WorkFlow();
            DataTable dt = objWorkFlow.GetHRM_Employees();
            ddlBuyer.DataSource = dt;
            ddlBuyer.DataValueField = "CardNo";
            ddlBuyer.DataTextField = "EmployeeName";
            ddlBuyer.DataBind();
            ddlBuyer.Items.Insert(0, "Select");
            
        }
        private void GetManager()
        {
            objWorkFlow = new WorkFlow();
            DataTable dt = objWorkFlow.GetHRM_Employees();
            ddlManager.DataSource = dt;
            ddlManager.DataValueField = "CardNo";
            ddlManager.DataTextField = "EmployeeName";
            ddlManager.DataBind();
            ddlManager.Items.Insert(0, "Select");

        }
        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            objWorkFlow = new WorkFlow();
            DataTable dt = objWorkFlow.GetProjectName(ddlProject.SelectedValue);
            lblProjectName.Text = dt.Rows[0][1].ToString();
            GetProjectElement();
            lblProjectElementName.Text = "";
            lblPMDL.Text = ""; 
            // GetSpecification();
            // txtSpecification.Text = "";
        }
        protected void ddlElement_SelectedIndexChanged(object sender, EventArgs e)
        {
            objWorkFlow = new WorkFlow();
            DataTable dt = objWorkFlow.GetProjectElementFDesc(ddlProject.SelectedValue, ddlElement.SelectedValue);
            lblProjectElementName.Text = dt.Rows[0][1].ToString();
            Session["ProjName"] = ddlProject.SelectedValue;
            Session["Element"] = ddlElement.SelectedValue;
            GetPMDL();


        }

        //protected void ddlSpecification_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    objWorkFlow = new WorkFlow();
        //    DataTable dt = objWorkFlow.GetProjectSpecificationDesc(ddlProject.SelectedValue,ddlElement.SelectedValue, ddlSpecification.SelectedValue);
        //    lblSpecDesc.Text = dt.Rows[0]["DocumentDescription"].ToString();
        //}

        //ddlPMDL_SelectedIndexChanged
        //protected void ddlPMDL_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    objWorkFlow = new WorkFlow();
        //    foreach (ListItem listItem in ddlPMDL.Items)
        //    {
        //        if (listItem.Selected)
        //        {
        //            ddlPMDL.SelectedValue += listItem.Value + ",";
        //            ddlPMDL.SelectedItem.Text = listItem.Text;
        //        }
        //    }
        //    //DataTable dt = objWorkFlow.GetProjectSpecificationDesc(ddlProject.SelectedValue, ddlElement.SelectedValue, ddlPMDL.SelectedValue);
        //    //lblPMDL.Text = dt.Rows[0]["DocumentDescription"].ToString();
        //}
        protected void ddlBuyer_SelectedIndexChanged(object sender, EventArgs e)
        {
            objWorkFlow = new WorkFlow();
            DataTable dt = objWorkFlow.GetUserName(ddlBuyer.SelectedValue);
            lblBuyerName.Text = dt.Rows[0]["CardNo"].ToString();//CardNo
        }
        protected void ddlManager_SelectedIndexChanged(object sender, EventArgs e)
        {
            objWorkFlow = new WorkFlow();
            DataTable dt = objWorkFlow.GetUserName(ddlManager.SelectedValue);
            lblManagername.Text = dt.Rows[0]["CardNo"].ToString();
        }

        //[WebMethod]
        //public static string[] GetSpecificationMethod(string prefixText, int count)
        //{
        //    WorkFlow objWorkFlow = new WorkFlow();
        //    DataTable dt = objWorkFlow.GetProjectSpecificationMethod(prefixText);
        //    List<string> lst = new List<string>();
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        string Specification = dr["DocumentID"].ToString() + " (" + dr["DocumentDescription"].ToString() + ")";
        //        lst.Add(Specification);
        //    }
        //    return lst.ToArray();
        //}

        //GetPMDLdocMethod
        // [WebMethod]
        //[System.Web.Services.WebMethod(EnableSession = true)]
        //[System.Web.Script.Services.ScriptMethod()]
        //public static string[] GetPMDLdocMethod(string prefixText,  int count)
        //{
        //   // HttpContext.Current.Session;
        //    //Page.Session  session = new System.Web.SessionState.HttpSessionState();
        //    string sProj = (string)HttpContext.Current.Session["ProjName"];
        //    string sElement= (string)HttpContext.Current.Session["Element"];
        //    WorkFlow objWorkFlow = new WorkFlow();
        //    DataTable dt = objWorkFlow.GetPMDLdoc(prefixText,sProj,sElement);
        //    List<string> lst = new List<string>();
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        string Specification = dr["DocumentID"].ToString() + " (" + dr["DocumentDescription"].ToString() + ")";
        //        lst.Add(Specification);
        //    }
        //    return lst.ToArray();
        //}


        [WebMethod]
        public static string[] GetUSer(string prefixText, int count)
        {
            WorkFlow objWorkFlow = new WorkFlow();
            DataTable dt = objWorkFlow.GetUser(prefixText);
            List<string> lst = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                string UserId = dr["EmployeeName"].ToString() + "-" + dr["CardNo"].ToString();
                lst.Add(UserId);
            }
            return lst.ToArray();
        }
        protected void btnAttachment_Click(object sender, EventArgs e)
        {
            try
            {
                string sRefIndex = "";
                string sRefHandle = "";
                string sPMDLDocNumber = "";

                if (Request.QueryString["WFID"] != null)
                {
                    WorkFlow objectWorkFlow = new WorkFlow();
                    objectWorkFlow.Project = ddlProject.SelectedValue;
                    //objectWorkFlow.PMDLdocDesc = ddlPMDL.SelectedValue;
                    //if (ddlPMDL.Items.Count > 0)
                    //{
                    //    for (int i = 0; i < ddlPMDL.Items.Count; i++)
                    //    {
                    //        if (ddlPMDL.Items[i].Selected)
                    //        {
                    //            objWorkFlow.PMDLdocDesc += ddlPMDL.Items[i] + ",";
                    //        }
                    //    }
                    //}
                    if (ddlPMDL.Items.Count > 0)
                    {
                        for (int i = 0; i < ddlPMDL.Items.Count; i++)
                        {
                            if (ddlPMDL.Items[i].Selected)
                            {
                                //objWorkFlow.PMDLdocDesc += ddlPMDL.Items[i] + ",";
                                sPMDLDocNumber += ddlPMDL.Items[i].Value + "','";
                            }
                        }
                        if (sPMDLDocNumber.Contains(","))
                        {
                            objectWorkFlow.PMDLdocDesc = sPMDLDocNumber.Substring(0, sPMDLDocNumber.Length - 3);
                        }
                        else
                        {
                            objectWorkFlow.PMDLdocDesc = sPMDLDocNumber.ToString();
                        }
                        // objWorkFlow.PMDLdocDesc=String.Join("','", objWorkFlow.PMDLdocDesc);
                    }
                    else
                    {
                        objectWorkFlow.PMDLdocDesc = "";
                    }
                    if (objectWorkFlow.PMDLdocDesc != "")
                    {
                        string sTransaction = "";
                        DataTable dtRefIndex = objectWorkFlow.GetIndex();
                        if (dtRefIndex.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtRefIndex.Rows)
                            {
                                sTransaction += dr["t_tran"].ToString() + "','";
                            }
                            if (sTransaction.Contains(","))
                            {
                                sRefIndex = sTransaction.Substring(0, sTransaction.Length - 3);
                            }
                            sRefHandle = "TRANSMITTALLINES_200";
                        }
                    }
                    else
                    {
                       
                    }
                    if (sRefIndex != null && sRefIndex != "")
                    {

                        string url = "http://192.9.200.146/Attachment/Attachment.aspx?AthHandle=J_PREORDER_WORKFLOW" + "&Index=" + Request.QueryString["WFID"] + "&AttachedBy=" + Request.QueryString["u"] + "&RefHandle=" + sRefHandle + "&RefIndex=" + sRefIndex + "&ed=a";

                        string s = "window.open('" + url + "','abc','width=1300,height=700,left=100,top=100,resizable=yes,scrollbars=yes');"; //," + hdfWFID.Value + " 'width=300,height=100,left=100,top=100,resizable=yes'
                        ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);

                    }
                    else
                    { 
                        // if (dtRefIndex != null)

                        string url = "http://192.9.200.146/Attachment/Attachment.aspx?AthHandle=J_PREORDER_WORKFLOW" + "&Index=" + Request.QueryString["WFID"] + "&AttachedBy=" + Request.QueryString["u"] + "&ed=a";
                    string s = "window.open('" + url + "','abc','width=800,height=600,left=100,top=100,resizable=yes,scrollbars=yes');"; //, 'width=300,height=100,left=100,top=100,resizable=yes'
                    ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                    }
                }
                else
                {
                    if (ddlProject.SelectedValue != "Select" && ddlElement.SelectedValue != "Select" && ddlBuyer.SelectedValue != "")
                    {
                        //string[] Buyer = txtBuyer.Text.Split('-');
                        //string[] Manager = txtManager.Text.Split('-');
                        objWorkFlow = new WorkFlow();
                        objWorkFlow.Parent_WFID = 0;
                        objWorkFlow.Project = ddlProject.SelectedValue + "-" + lblProjectName.Text;
                        objWorkFlow.Element = ddlElement.SelectedValue + "-" + lblProjectElementName.Text;
                        objWorkFlow.SpecificationNo = txtSpecification.Text;
                        //if (ddlPMDL.SelectedValue != "" || ddlPMDL.SelectedValue != null)
                        //{
                        //    objWorkFlow.PMDLdocDesc = ddlPMDL.SelectedValue;
                        //}
                        //else
                        //{
                        //    objWorkFlow.PMDLdocDesc = "";

                        if (ddlPMDL.Items.Count > 0)
                        {
                            for (int i = 0; i < ddlPMDL.Items.Count; i++)
                            {
                                if (ddlPMDL.Items[i].Selected)
                                {
                                    //objWorkFlow.PMDLdocDesc += ddlPMDL.Items[i] + ",";
                                    sPMDLDocNumber += ddlPMDL.Items[i].Value + "','";
                                }
                            }
                            if (sPMDLDocNumber.Contains(","))
                            {
                                objWorkFlow.PMDLdocDesc = sPMDLDocNumber.Substring(0, sPMDLDocNumber.Length - 3);
                            }
                            else
                            {
                                objWorkFlow.PMDLdocDesc = sPMDLDocNumber.ToString();
                            }
                            // objWorkFlow.PMDLdocDesc=String.Join("','", objWorkFlow.PMDLdocDesc);
                        }
                        objWorkFlow.Buyer = ddlBuyer.SelectedValue;
                        objWorkFlow.Manager = ddlManager.SelectedValue;
                        objWorkFlow.UserId = Request.QueryString["u"];
                        objWorkFlow.WF_Status = "Created";

                        DataTable dtres = objWorkFlow.InsertPreOrderData();
                        // Dump Data into BAAN table Change-25-08-2018 sagar
                         
                        int sWFID = objWorkFlow.GetWorkFlowID();
                        Session["WFID"] = sWFID;
                        objWorkFlow.Project = objWorkFlow.Project.Substring(0, 6);
                        objWorkFlow.Element = objWorkFlow.Element.Substring(0, 8);
                        // Write a function to insert record in dmisg168 
                       // objWorkFlow.InsertPreorderDataToBAN(sWFID);
                        objWorkFlow.Project = ddlProject.SelectedValue + "-" + lblProjectName.Text;
                        objWorkFlow.Element = ddlElement.SelectedValue + "-" + lblProjectElementName.Text;
                        hdfWFID.Value = dtres.Rows[0][0].ToString();
                        if (dtres.Rows.Count > 0)
                        {
                            //Insert In History
                            objWorkFlow.WFID = Convert.ToInt32(dtres.Rows[0][0]);
                            objWorkFlow.Supplier = "";
                            objWorkFlow.SupplierName = "";
                            string sPMDLDocNo = "";
                            objWorkFlow.Notes = txtNotes.Text;
                            DataTable dtWFHID = objWorkFlow.InserPreOrderHistory();
                          //  objWorkFlow.InserPreOrderHistoryToBAAN(); // Dump Preorder Data TO BAAN table change 25/08/2018 sagar
                            string IndexValue = dtWFHID.Rows[0][0].ToString() + "_" + dtWFHID.Rows[0][1].ToString();
                            hdfHistoryID.Value = dtWFHID.Rows[0][1].ToString();
                            //  divAlert.Visible = true;
                            btnSave.Text = "Release";
                            //  Response.Redirect("ReleaseTechnicalSpecification.aspx?u=" + Request.QueryString["u"]);
                            WorkFlow objectWorkFlow = new WorkFlow();
                            objectWorkFlow.Project = ddlProject.SelectedValue;
                           //bjectWorkFlow.PMDLdocDesc = ddlPMDL.SelectedValue;
                            if (ddlPMDL.Items.Count > 0)
                            {
                                for (int i = 0; i < ddlPMDL.Items.Count; i++)
                                {
                                    if (ddlPMDL.Items[i].Selected)
                                    {
                                        //objWorkFlow.PMDLdocDesc += ddlPMDL.Items[i] + ",";
                                        sPMDLDocNo += ddlPMDL.Items[i].Value + "','";
                                    }
                                }
                                if (sPMDLDocNumber.Contains(","))
                                {
                                    objectWorkFlow.PMDLdocDesc += sPMDLDocNo.Substring(0, sPMDLDocNo.Length - 3);
                                }
                                else
                                {
                                    objectWorkFlow.PMDLdocDesc = sPMDLDocNo.ToString();
                                }
                            }
                            else
                            {
                                objectWorkFlow.PMDLdocDesc = "";
                            }
                            if (objectWorkFlow.PMDLdocDesc != "")
                            {
                                string sTransaction = "";
                                DataTable dtRefIndex = objectWorkFlow.GetIndex();
                                if (dtRefIndex.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtRefIndex.Rows)
                                    {
                                        sTransaction += dr["t_tran"].ToString() + "','";
                                    }
                                    if (sTransaction.Contains(","))
                                    {
                                        sRefIndex = sTransaction.Substring(0, sTransaction.Length - 3);
                                    }
                                    sRefHandle = "TRANSMITTALLINES_200";
                                }
                            }
                            else
                            {

                            }
                            //sRefIndex = objectWorkFlow.GetIndex();
                            //sRefHandle = "TRANSMITTALLINES_200";

                            if (sRefIndex != null && sRefIndex != "")
                            {

                                string url = "http://192.9.200.146/Attachment/Attachment.aspx?AthHandle=J_PREORDER_WORKFLOW" + "&Index=" + hdfWFID.Value + "&AttachedBy=" + Request.QueryString["u"] + "&RefHandle=" + sRefHandle + "&RefIndex=" + sRefIndex + "&ed=a";

                                string s = "window.open('" + url + "','abc','width=1300,height=700,left=100,top=100,resizable=yes,scrollbars=yes');"; //," + hdfWFID.Value + " 'width=300,height=100,left=100,top=100,resizable=yes'
                                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);

                            }
                            else
                            {
                                /// Open Attachment Page http://localhost/Attachment/Attachment.aspx    http://localhost:6595/Attachment.aspx
                                // string url = "http://localhost/Attachment/Attachment.aspx?AthHandle=J_PREORDER_WORKFLOW" + "&Index="+ hdfWFID.Value + "&AttachedBy=" + Request.QueryString["u"] + "&ed=a&RefHandle=TRANSMITTALLINES_200&RefIndex=BOi000532_JB0973-50270100-027-0001_00";
                                string url = "http://192.9.200.146/Attachment/Attachment.aspx?AthHandle=J_PREORDER_WORKFLOW" + "&Index=" + hdfWFID.Value + "&AttachedBy=" + Request.QueryString["u"] + "&ed=a";
                                string s = "window.open('" + url + "','abc','width=800,height=600,left=100,top=100,resizable=yes,scrollbars=yes');"; //, 'width=300,height=100,left=100,top=100,resizable=yes'
                                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                            }


                            //SAve Attachment
                            // if (dtWFHID.Rows.Count > 0)
                            // {
                            // string IndexValue = dtWFHID.Rows[0][0].ToString() + "_" + dtWFHID.Rows[0][1].ToString();
                            // UploadAttachments(IndexValue);
                            //  }

                            //Mail Send
                            //DataTable dtMailTo = objWorkFlow.GetMAilID(Buyer[1]);
                            // string MailTo = dtMailTo.Rows[0]["EMailid"].ToString();
                            // SendMail(MailTo);
                            //  ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert('Successfully Insert');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert('Some technical issue');", true);
                        }

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert('Please Fill All information');", true);
                    }
                }
            }

            //try
            //{
            //    //string sRefIndex = string.Empty;
            //    //List<string> results = new List<string>();
            //    WorkFlow objectWorkFlow = new WorkFlow();
            //    objectWorkFlow.Project = ddlProject.SelectedValue;
            //    objectWorkFlow.PMDLdocDesc = ddlPMDL.SelectedValue;
            //    string sRefIndex = objectWorkFlow.GetIndex();
            //    //DataTable dtRefIndex = objectWorkFlow.GetIndex();
            //    //foreach (DataRow dr in dtRefIndex.Rows)
            //    //{
            //    //    //string sRefIndex= dr["Handle_Index"].ToString();
            //    //   results.Add(dr["Handle_Index"].ToString());
            //    //}

            //    //sRefIndex = string.Join(",", results);
            //    string sRefHandle = "TRANSMITTALLINES_200";

            //    if (sRefIndex != null && sRefIndex != "")
            //   // if (dtRefIndex != null)
            //    {
            //        if (Request.QueryString["WFID"] != null)
            //        {
            //            string url = "http://localhost/Attachment/Attachment.aspx?AthHandle=J_PREORDER_WORKFLOW" + "&Index=" + Request.QueryString["WFID"] + "&AttachedBy=" + Request.QueryString["u"] + "&RefHandle=" + sRefHandle + "&RefIndex=" + sRefIndex + "&ed=a";

            //            string s = "window.open('" + url + "','abc','width=1300,height=700,left=100,top=100,resizable=yes,scrollbars=yes');"; //," + hdfWFID.Value + " 'width=300,height=100,left=100,top=100,resizable=yes'
            //            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            //        }
            //        else
            //        {
            //            string url = "http://localhost/Attachment/Attachment.aspx?AthHandle=J_PREORDER_WORKFLOW" + "&Index=" + hdfWFID.Value + "&AttachedBy=" + Request.QueryString["u"] + "&RefHandle=" + sRefHandle + "&RefIndex=" + sRefIndex + "&ed=a";

            //            string s = "window.open('" + url + "','abc','width=1300,height=700,left=100,top=100,resizable=yes,scrollbars=yes');"; //," + hdfWFID.Value + " 'width=300,height=100,left=100,top=100,resizable=yes'
            //            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            //        }
            //    }

            //    else if (Request.QueryString["WFID"] != null)
            //    {
            //        //string url = "http://localhost/Attachment/Attachment.aspx?AthHandle=J_PREORDER_WORKFLOW" + "&Index=" + Request.QueryString["WFID"] + "&AttachedBy=" + Request.QueryString["u"] + "&ed=a";
            //        string url = "http://192.9.200.146/Attachment/Attachment.aspx?AthHandle=J_PREORDER_WORKFLOW" + "&Index=" + Request.QueryString["WFID"] + "&AttachedBy=" + Request.QueryString["u"] + "&ed=a";//  Change url to point to server Sagar
            //        string s = "window.open('" + url + "','abc','width=800,height=600,left=100,top=100,resizable=yes,scrollbars=yes');"; //, 'width=300,height=100,left=100,top=100,resizable=yes'
            //        ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            //    }
            //    else
            //    {
            //        if (ddlProject.SelectedValue != "Select" && ddlElement.SelectedValue != "Select" && ddlBuyer.SelectedValue != "")
            //        {
            //            //string[] Buyer = txtBuyer.Text.Split('-');
            //            //string[] Manager = txtManager.Text.Split('-');
            //            objWorkFlow = new WorkFlow();
            //            objWorkFlow.Parent_WFID = 0;
            //            objWorkFlow.Project = ddlProject.SelectedValue + "-" + lblProjectName.Text;
            //            objWorkFlow.Element = ddlElement.SelectedValue + "-" + lblProjectElementName.Text;
            //            objWorkFlow.SpecificationNo = txtSpecification.Text;
            //            if (ddlPMDL.SelectedValue != "" || ddlPMDL.SelectedValue != null)
            //            {
            //                objWorkFlow.PMDLdocDesc = ddlPMDL.SelectedValue;
            //            }
            //            else
            //            {
            //                objWorkFlow.PMDLdocDesc = "";
            //            }
            //            objWorkFlow.Buyer = ddlBuyer.SelectedValue;
            //            objWorkFlow.Manager = ddlManager.SelectedValue;
            //            objWorkFlow.UserId = Request.QueryString["u"];
            //            objWorkFlow.WF_Status = "Created";

            //            DataTable dtres = objWorkFlow.InsertPreOrderData();
            //            hdfWFID.Value = dtres.Rows[0][0].ToString();
            //            if (dtres.Rows.Count > 0)
            //            {
            //                //Insert In History
            //                objWorkFlow.WFID = Convert.ToInt32(dtres.Rows[0][0]);
            //                objWorkFlow.Supplier = "";
            //                objWorkFlow.SupplierName = "";
            //                objWorkFlow.Notes = txtNotes.Text;
            //                DataTable dtWFHID = objWorkFlow.InserPreOrderHistory();
            //                string IndexValue = dtWFHID.Rows[0][0].ToString() + "_" + dtWFHID.Rows[0][1].ToString();
            //                hdfHistoryID.Value = dtWFHID.Rows[0][1].ToString();
            //                //  divAlert.Visible = true;
            //                btnSave.Text = "Release";
            //                //  Response.Redirect("ReleaseTechnicalSpecification.aspx?u=" + Request.QueryString["u"]);

            //                /// Open Attachment Page http://localhost/Attachment/Attachment.aspx    http://localhost:6595/Attachment.aspx
            //               // string url = "http://localhost/Attachment/Attachment.aspx?AthHandle=J_PREORDER_WORKFLOW" + "&Index="+ hdfWFID.Value + "&AttachedBy=" + Request.QueryString["u"] + "&ed=a&RefHandle=TRANSMITTALLINES_200&RefIndex=BOi000532_JB0973-50270100-027-0001_00";
            //                  string url = "http://192.9.200.146/Attachment/Attachment.aspx?AthHandle=J_PREORDER_WORKFLOW" + "&Index=" + hdfWFID.Value + "&AttachedBy=" + Request.QueryString["u"] + "&ed=a";
            //                string s = "window.open('" + url + "','abc','width=800,height=600,left=100,top=100,resizable=yes,scrollbars=yes');"; //, 'width=300,height=100,left=100,top=100,resizable=yes'
            //                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);


            //                //SAve Attachment
            //                // if (dtWFHID.Rows.Count > 0)
            //                // {
            //                // string IndexValue = dtWFHID.Rows[0][0].ToString() + "_" + dtWFHID.Rows[0][1].ToString();
            //                // UploadAttachments(IndexValue);
            //                //  }

            //                //Mail Send
            //                //DataTable dtMailTo = objWorkFlow.GetMAilID(Buyer[1]);
            //                // string MailTo = dtMailTo.Rows[0]["EMailid"].ToString();
            //                // SendMail(MailTo);
            //                //  ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert('Successfully Insert');", true);
            //            }
            //            else
            //            {
            //                ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert('Some technical issue');", true);
            //            }

            //        }
            //        else
            //        {
            //            ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert('Please Fill All information');", true);
            //        }
            //    }
            //}
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert('Some technical issue');", true);
            }
        }
        protected void btnRelease_Click(object sender, EventArgs e)
        {
            try
            {
                string sPMDLDocNumber = "";
                if (ddlProject.SelectedValue != "Select" && ddlElement.SelectedValue != "Select" && ddlBuyer.SelectedValue != "" && ddlManager.SelectedValue != "")
                {
                    //string[] Buyer = txtBuyer.Text.Split('-'); && ddlPMDL.SelectedValue != ""
                    //string[] pmdlDoc = txtPMDLdoc.Text.Split('(');
                    objWorkFlow = new WorkFlow();
                    objWorkFlow.Parent_WFID = 0;
                    //objWorkFlow.Project = ddlProject.SelectedValue + "-" + lblProjectName.Text;
                    //objWorkFlow.Element = ddlElement.SelectedValue + "-" + lblProjectElementName.Text;
                    objWorkFlow.Project = ddlProject.SelectedValue + "-" + lblProjectName.Text; ;
                    objWorkFlow.Element = ddlElement.SelectedValue + "-" + lblProjectElementName.Text;
                    objWorkFlow.SpecificationNo = txtSpecification.Text;
                    objWorkFlow.Buyer = ddlBuyer.SelectedValue;
                    objWorkFlow.Manager = ddlManager.SelectedValue;
                    //if (ddlPMDL.Items.Count > 0)
                    //{
                    //    for (int i = 0; i < ddlPMDL.Items.Count; i++)
                    //    {
                    //        if (ddlPMDL.Items[i].Selected)
                    //        {
                    //            objWorkFlow.PMDLdocDesc += ddlPMDL.Items[i] + ",";
                    //            //Insert into a seperate table against WFID and revision Number
                    //            objWorkFlow.WFID = Request.QueryString["WFID"] != null ? Convert.ToInt32(Request.QueryString["WFID"]) : Convert.ToInt32(hdfWFID.Value);
                    //            objWorkFlow.MultiPMDLdocDesc = ddlPMDL.Items[i].ToString();
                    //            objWorkFlow.InsertMultiPMDL();
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    objWorkFlow.PMDLdocDesc = "";
                    //}
                    //if (ddlPMDL.SelectedValue != "" || ddlPMDL.SelectedValue != null)
                    //{
                    //    objWorkFlow.PMDLdocDesc = ddlPMDL.SelectedValue;
                    //}
                    //else
                    //{
                    //    objWorkFlow.PMDLdocDesc = "";
                    //}
                    objWorkFlow.UserId = Request.QueryString["u"];
                    objWorkFlow.WF_Status = "Technical Specification Released";

                    if (btnSave.Text == "Release" && Request.QueryString["WFID"]==null)
                    {
                        //if (ddlPMDL.Items.Count > 0)
                        //{
                        //    for (int i = 0; i < ddlPMDL.Items.Count; i++)
                        //    {
                        //        if (ddlPMDL.Items[i].Selected)
                        //        {
                        //            objWorkFlow.PMDLdocDesc += ddlPMDL.Items[i] + ",";
                        //        }
                        //    }
                        //}
                        if (ddlPMDL.Items.Count > 0)
                        {
                            for (int i = 0; i < ddlPMDL.Items.Count; i++)
                            {
                                if (ddlPMDL.Items[i].Selected)
                                {
                                    //objWorkFlow.PMDLdocDesc += ddlPMDL.Items[i] + ",";
                                    sPMDLDocNumber += ddlPMDL.Items[i].Value + ",";
                                }
                            }
                            if (sPMDLDocNumber.Contains(","))
                            {
                                objWorkFlow.PMDLdocDesc = sPMDLDocNumber.Substring(0, sPMDLDocNumber.Length - 1);
                            }
                            else
                            {
                                objWorkFlow.PMDLdocDesc = sPMDLDocNumber.ToString();
                            }
                            // objWorkFlow.PMDLdocDesc=String.Join("','", objWorkFlow.PMDLdocDesc);
                        }
                        else
                        {
                            objWorkFlow.PMDLdocDesc = "";
                        }
                        //DataTable dtres = objWorkFlow.InsertPreOrderData();
                        // Dump Data into BAAN table Change-25-08-2018 sagar
                        int sWFID = 0;
                        if (Session["WFID"] != null)
                        {
                            objWorkFlow.WFID = Convert.ToInt16(Session["WFID"]);
                        }
                        else
                        {
                          //  sWFID = objWorkFlow.GetWorkFlowID();
                        }


                        objWorkFlow.Project = objWorkFlow.Project.Substring(0, 6);
                        objWorkFlow.Element = objWorkFlow.Element.Substring(0, 8);
                        // Write a function to insert record in dmisg168 
                        //  objWorkFlow.InsertPreorderDataToBAN(sWFID);
                        int res1 = 0;
                        if (Session["WFID"] != null)
                        {
                            res1 = objWorkFlow.UpdateWF_Status();
                        }
                        else
                        {
                            objWorkFlow.Buyer = ddlBuyer.SelectedValue;
                            objWorkFlow.Manager = ddlManager.SelectedValue;
                            objWorkFlow.UserId = Request.QueryString["u"];
                            objWorkFlow.WF_Status = "Technical Specification Released";

                            DataTable dtres = objWorkFlow.InsertPreOrderData();
                            objWorkFlow.WFID = objWorkFlow.GetWorkFlowID();
                            res1 = objWorkFlow.UpdateWF_Status();
                        }
                        if (res1 > 0)
                        {
                            InsertPreHistory(objWorkFlow.WFID, "Technical Specification Released");
                        }
                            objWorkFlow.Project = ddlProject.SelectedValue + "-" + lblProjectName.Text;
                        objWorkFlow.Element = ddlElement.SelectedValue + "-" + lblProjectElementName.Text;
                        // if (dtres.Rows.Count > 0)
                        if (res1 > 0)
                        {
                            //Insert In History
                            //if (objWorkFlow.WFID == null)
                            //{
                            //    objWorkFlow.WFID = Convert.ToInt32(dtres.Rows[0][0]);
                            //}
                            if (ddlPMDL.Items.Count > 0)
                            {
                                for (int i = 0; i < ddlPMDL.Items.Count; i++)
                                {
                                    if (ddlPMDL.Items[i].Selected)
                                    {
                                        objWorkFlow.MultiPMDLdocDesc = ddlPMDL.Items[i].Value.ToString();
                                        objWorkFlow.InsertMultiPMDL();
                                       // objWorkFlow.InsertMultiPMDLtoDuplicateTable();
                                    }
                                }
                            }
                            else
                            {
                              
                            }
                            //objWorkFlow.Supplier = "";
                            //objWorkFlow.SupplierName = "";
                            //objWorkFlow.Notes = txtNotes.Text;
                            //DataTable dtWFHID = objWorkFlow.InserPreOrderHistory();
                            //objWorkFlow.Project = objWorkFlow.Project.Substring(0, 6);
                            //objWorkFlow.Element = objWorkFlow.Element.Substring(0, 8);
                            //objWorkFlow.InserPreOrderHistoryToBAAN(); // Dump Preorder Data TO BAAN table change 25/08/2018 sagar
                            //string IndexValue = dtWFHID.Rows[0][0].ToString() + "_" + dtWFHID.Rows[0][1].ToString();

                            //divAlert.Visible = true;
                            //Response.Redirect("ReleaseTechnicalSpecification.aspx?u=" + Request.QueryString["u"]);
                        }
                    }
                    if (btnSave.Text == "Release" && Request.QueryString["WFID"] != null)
                    {
                        objWorkFlow = new WorkFlow();
                        objWorkFlow.WFID = Request.QueryString["WFID"] != null ? Convert.ToInt32(Request.QueryString["WFID"]) : Convert.ToInt32(hdfWFID.Value);
                        DataTable dtSlHistory = objWorkFlow.GetWFHID(Convert.ToInt32(objWorkFlow.WFID));
                        objWorkFlow.SLNO_WFID = Convert.ToInt32(dtSlHistory.Rows[0][0]);
                        objWorkFlow.WF_Status = "Technical Specification Released";
                        objWorkFlow.UpdateWF_Status();
                     //   objWorkFlow.UpdateWF_StatusInBAANTable(); // Dump Preorder Data TO BAAN table change 25/08/2018 sagar
                        objWorkFlow.UpdateStatusWFPreOrder_History();
                      //  objWorkFlow.UpdateStatusWFPreOrder_HistoryBaaN();
                        // Response.Redirect("ReleaseTechnicalSpecification.aspx?u=" + Request.QueryString["u"]);
                    }
                    if (btnSave.Text == "ReSubmit")
                    {
                       // objWorkFlow = new WorkFlow();
                        objWorkFlow.WFID = Convert.ToInt32(Request.QueryString["WFID"]);                     
                        int res = objWorkFlow.UpdateWFPreOrder();
                      //  objWorkFlow.UpdateWF_StatusInBAANTable(); // Dump Preorder Data TO BAAN table change 25/08/2018 sagar 
                        if (res > 0)
                        {
                            // DataTable dtWFHID = objWorkFlow.GetWFHID(Convert.ToInt32(Request.QueryString["WFID"]));
                            // int res1 = objWorkFlow.UpdateWFPreOrder_History();
                            string IndexValue = InsertPreHistory(Convert.ToInt32(Request.QueryString["WFID"]), "Technical Specification Released");

                            //SAve Attachment
                            // UploadAttachments(IndexValue);

                            divAlert.Visible = true;
                        }
                        else
                        {

                        }
                    }

                    //   Mail Send
                    DataTable dtBuyer = objWorkFlow.GetMAilID(ddlBuyer.SelectedValue);
                    DataTable dtManager = objWorkFlow.GetMAilID(ddlManager.SelectedValue);
                    string BuyerMail = dtBuyer.Rows[0]["EMailid"].ToString();
                    string ManagerMail = dtManager.Rows[0]["EMailid"].ToString();
                    string MailTo = BuyerMail+","+ ManagerMail;
                    SendMail(MailTo);

                    Response.Redirect("ReleaseTechnicalSpecification.aspx?u=" + Request.QueryString["u"]);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert('Please Fill All information');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert('Some technical issue');", true);
            }
        }
        private string InsertPreHistory(int Id, string status)
        {
            objWorkFlow = new WorkFlow();
            objWorkFlow.WFID = Id;
            DataTable dt = objWorkFlow.GetWFById();
            objWorkFlow.Parent_WFID = 0;
            objWorkFlow.Project = dt.Rows[0]["Project"].ToString();
            objWorkFlow.Element = dt.Rows[0]["Element"].ToString();
            objWorkFlow.PMDLdocDesc = dt.Rows[0]["PMDLDocNo"].ToString();
            objWorkFlow.Buyer = dt.Rows[0]["Buyer"].ToString();
            objWorkFlow.Buyer = dt.Rows[0]["Manager"].ToString();
            objWorkFlow.UserId = Request.QueryString["u"];
            objWorkFlow.WF_Status = status;
            objWorkFlow.Supplier = dt.Rows[0]["Supplier"].ToString();
            objWorkFlow.SupplierName = dt.Rows[0]["SupplierName"].ToString();
            objWorkFlow.Notes = txtNotes.Text;
            objWorkFlow.SpecificationNo = dt.Rows[0]["SpecificationNo"].ToString();
            DataTable dtWFHID = objWorkFlow.InserPreOrderHistory();
            objWorkFlow.Project = objWorkFlow.Project.Substring(0, 6);
            objWorkFlow.Element = objWorkFlow.Element.Substring(0, 8);
           
            //  objWorkFlow.InserPreOrderHistoryToBAAN(); // Dump Preorder Data TO BAAN table change 25/08/2018 sagar
            string IndexValue = dtWFHID.Rows[0][0].ToString() + "_" + dtWFHID.Rows[0][1].ToString();
            return IndexValue;
        }

        #region Attachment and mail
        //protected void UploadAttachments(string IndexValue)
        //{
        //    if (fileUpload.HasFile)
        //    {
        //        objWorkFlow = new WorkFlow();
        //        // objWorkFlow.IndexValue = Request.QueryString["Index"];
        //        objWorkFlow.AttachmentHandle = "J_PREORDER_WORKFLOW";
        //        DataTable dt = objWorkFlow.GetPath();
        //        if (dt.Rows.Count > 0)
        //        {
        //            //string ServerPath = "\\\\" + dt.Rows[0]["ServerName"].ToString() + "\\" + dt.Rows[0]["Path"].ToString() + "\\";  //dt.Rows[0]["Path"].ToString() + "\\";//      // Server.MapPath("~/Files/");//
        //            string ServerPath = "D:\\" + dt.Rows[0]["t_path"].ToString() + "\\";  //dt.Rows[0]["Path"].ToString() + "\\";//      // Server.MapPath("~/Files/");//


        //            int filecount = 0;
        //            filecount = fileUpload.PostedFiles.Count();
        //            if (filecount > 0)
        //            {
        //                foreach (HttpPostedFile PostedFile in fileUpload.PostedFiles)
        //                {
        //                    string fileName = Path.GetFileNameWithoutExtension(PostedFile.FileName);
        //                    string fileExtension = Path.GetExtension(PostedFile.FileName);
        //                    try
        //                    {
        //                        objWorkFlow = new WorkFlow();
        //                        objWorkFlow.AttachmentHandle = "J_PREORDER_WORKFLOW";
        //                        objWorkFlow.IndexValue = IndexValue;
        //                        objWorkFlow.PurposeCode = "PreOrderWorkFlow";// Request.QueryString["PurposeCode"];
        //                        objWorkFlow.AttachedBy = Request.QueryString["u"];
        //                        objWorkFlow.FileName = fileName + fileExtension;
        //                        objWorkFlow.LibraryCode = dt.Rows[0]["LibCode"].ToString();
        //                        // DataTable dtFile = objWorkFlow.GetFileName();
        //                        //  if (dtFile.Rows.Count == 0)
        //                        //  {
        //                        DataTable dtDocID = objWorkFlow.InsertAttachmentdata();
        //                        if (dtDocID.Rows[0][0].ToString() != "0")
        //                        {
        //                            try
        //                            {
        //                                fileUpload.SaveAs(ServerPath + dtDocID.Rows[0][0]);
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                            }
        //                            // ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert('Successfully Uploaded');", true);
        //                        }
        //                        else
        //                        {
        //                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert('Attachment Handle does not exist');", true);
        //                        }
        //                        //  }
        //                        //  else
        //                        //  {
        //                        //    ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert('This file name already exist please change your file name');", true);
        //                        // }
        //                    }
        //                    catch (System.Exception ex)
        //                    {
        //                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert('" + ex.Message + "');", true);
        //                    }
        //                }
        //            }
        //            else
        //            {

        //            }
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert('Path does not exist');", true);
        //        }
        //    }
        //    else
        //    {

        //    }
        //    // }
        //    // else
        //    // {
        //    //     ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert('Data not found Properly');", true);
        //    // }
        //}
        #endregion

        public void SendMail(string MailTo)
        {
            try
            {
                // if (fileUpload.HasFile)
                // {
                objWorkFlow = new WorkFlow();
                DataTable dtMailTo = objWorkFlow.GetMAilID(Request.QueryString["u"]);
                string UserMailId = dtMailTo.Rows[0]["EMailid"].ToString();
                MailMessage mM = new MailMessage();
                mM.From = new MailAddress(UserMailId);
                // mM.To.Add(txtTo.Text.Trim());
                // string[] MailTo = txtTo.Text.Split(';');
                // foreach (string Mailid in MailTo)
                // {
                //     mM.To.Add(new MailAddress(Mailid));
                //  }
                if (MailTo.Contains(","))
                {
                    string[] Recipient = MailTo.Split(',');
                    mM.To.Add(Recipient[0]);
                    mM.To.Add(Recipient[1]);
                }//MailTo
                else
                {
                    mM.To.Add(MailTo);
                }
                mM.To.Add(UserMailId); //MailTo
                mM.Subject = "Technical Specification Released" + "-" + txtSpecification.Text ;
                //  foreach (HttpPostedFile PostedFile in fileUpload.PostedFiles)
                // {
                //     string fileName = Path.GetFileName(PostedFile.FileName);
                //     Attachment myAttachment = new Attachment(fileUpload.FileContent, fileName);
                //     mM.Attachments.Add(myAttachment);
                //  }
                mM.IsBodyHtml = true;
                mM.Body = txtNotes.Text;
                mM.Body =mM.Body.ToString().Replace("\n", "<br />");
                mM.Body += "<br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />This mail has been triggered to draw your attention on the respective ERP/Joomla module. Please login to respective module to see further details and file attachments";
                SmtpClient sC = new SmtpClient("192.9.200.214", 25);
                //   sC.Host = "192.9.200.214"; //"smtp-mail.outlook.com"// smtp.gmail.com
                //   sC.Port = 25; //587
                sC.DeliveryMethod = SmtpDeliveryMethod.Network;
                sC.UseDefaultCredentials = false;
                sC.Credentials = new NetworkCredential("baansupport@isgec.co.in", "isgec");
                //sC.Credentials = new NetworkCredential("adskvaultadmin", "isgec@123");
                sC.EnableSsl = false;  // true
                sC.Timeout = 10000000;
                sC.Send(mM);
                //  }
                //   else
                //   {

                //  }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert('Some technical issue Mail not sent');", true);
            }
            //}
        }

        protected void btnNotes_Click(object sender, EventArgs e)
        {
            string MailTo = string.Empty;
            string sRefIndex = "";
            string sRefHandle = "";
            WorkFlow objectWorkFlow = new WorkFlow();
            objectWorkFlow.Project = ddlProject.SelectedValue;
            if (ddlPMDL.Items.Count > 0)
            {
                for (int i = 0; i < ddlPMDL.Items.Count; i++)
                {
                    if (ddlPMDL.Items[i].Selected)
                    {
                        objWorkFlow.PMDLdocDesc += ddlPMDL.Items[i] + ",";

                    }
                }
            }
            else
            {
                objWorkFlow.PMDLdocDesc = "";
            }
            //  objectWorkFlow.PMDLdocDesc = ddlPMDL.SelectedValue;
            string sTransaction = "";
            DataTable dtRefIndex = objectWorkFlow.GetIndex();
            if (dtRefIndex.Rows.Count > 0)
            {
                foreach (DataRow dr in dtRefIndex.Rows)
                {
                    sTransaction += dr["t_tran"].ToString() + "','";
                }
                if (sTransaction.Contains(","))
                {
                    sRefIndex = sTransaction.Substring(0, sTransaction.Length - 3);
                }
                sRefHandle = "TRANSMITTALLINES_200";
            }
            //   string url = "";

            if (Request.QueryString["WFID"] != null)
            {
                //Header
                string Header = string.Empty;
                if (txtSpecification.Text.Contains("("))
                {
                    string[] Heading = txtSpecification.Text.Split('(');
                    Header = Heading[0];
                }
                else
                {
                    Header = txtSpecification.Text;
                }
                // Header = ddlPMDL.SelectedValue.ToString()+"-"+lblPMDL.Text;
                if (Request.QueryString["p"] != null && Request.QueryString["p"] == "U")
                {
                   // string[] Buyer = txtBuyer.Text.Split('-');
                    objWorkFlow = new WorkFlow();
                    //DataTable dtMailTo = objWorkFlow.GetMAilID(ddlBuyer.SelectedValue);
                    //MailTo = dtMailTo.Rows[0]["EMailid"].ToString();
                    DataTable dtBuyerMail = objWorkFlow.GetMAilID(ddlBuyer.SelectedValue);
                    DataTable dtManagerMail = objWorkFlow.GetMAilID(ddlManager.SelectedValue);
                   string MailBuyerTo = dtBuyerMail.Rows[0]["EMailid"].ToString();
                   string MailManagerTo= dtManagerMail.Rows[0]["EMailid"].ToString();
                    MailTo = MailBuyerTo + ";" + MailManagerTo;

                }
                if (sRefIndex != null && sRefIndex != "")
                {
                      //  string url = "http://localhost/Attachment/Notes.aspx?handle=J_PREORDER_WORKFLOW"+"&Index="+hdfWFID.Value+"&user="+Request.QueryString["u"]+"&Em=" +MailTo+ "&Hd="+Header+"&Tl="+txtSpecification.Text+"&RefHandle="+sRefHandle+"&RefIndex="+sRefIndex;
                    string url = "http://192.9.200.146/Attachment/Notes.aspx?handle=J_PREORDER_WORKFLOW" + "&Index=" + hdfWFID.Value + "&user=" + Request.QueryString["u"] + "&Em=" + MailTo + "&Hd=" + Header + "&Tl=" + txtSpecification.Text + "&RefHandle=" + sRefHandle + "&RefIndex=" + sRefIndex;

                    string s = "window.open('" + url + "','abc','width=1300,height=700,left=100,top=100,resizable=yes,scrollbars=yes');"; //," + hdfWFID.Value + " 'width=300,height=100,left=100,top=100,resizable=yes'
                        ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                }
                else
                { 
               // string url = "http://localhost/Attachment/Notes.aspx?handle=J_PREORDER_WORKFLOW&index=235&AttachedBy=3194&ed=a&RefHandle=TRANSMITTALLINES_200&RefIndex=BOi000532_JB0973-50270100-027-0001_00&Em=&Tl=&Hd=";
                 //   "http://localhost/Attachment/Notes.aspx?AthHandle=J_PREORDER_WORKFLOW" + "&Index=251&AttachedBy=" + Request.QueryString["u"] + "&ed=a&RefHandle=TRANSMITTALLINES_200&RefIndex=BOi000532_JB0973-50270100-027-0001_00";
                string url = "http://192.9.200.146/Attachment/Notes.aspx?handle=J_PREORDER_WORKFLOW&Index=" + Request.QueryString["WFID"] + "&user=" + Request.QueryString["u"] + "&Em=" + MailTo + "&Hd=" + Header + "&Tl=" + txtSpecification.Text; //  change pass optional parameter ref handle and ref index  sagar
                string s = "window.open('" + url + "','abc','width=1300,height=700,left=100,top=100,resizable=yes,scrollbars=yes');"; //," + hdfWFID.Value + " 'width=300,height=100,left=100,top=100,resizable=yes'
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                }
            }
        }
        protected void ckhbx1_CheckedChanged(object sender, EventArgs e)
        {
            chkbx2.Checked = false;
            chkbx3.Checked = false;
            GetPMDL();
        }

        protected void ckhbx2_CheckedChanged(object sender, EventArgs e)
        {
            chkbx1.Checked = false;
            chkbx3.Checked = false;
            objWorkFlow = new WorkFlow();
            DataTable dt = objWorkFlow.GetReleasedPMDLdoc(ddlProject.SelectedValue, ddlElement.SelectedValue);
            if (dt.Rows.Count > 0)
            {
                ddlPMDL.DataSource = dt;
                ddlPMDL.DataValueField = "DocumentID";
                ddlPMDL.DataTextField = "PMDLDoc";
                ddlPMDL.DataBind();
            }
        }
        protected void ckhbx3_CheckedChanged(object sender, EventArgs e)
        {
            chkbx1.Checked = false;
            chkbx2.Checked = false;
            string PMDLString = "";
            objWorkFlow = new WorkFlow();
           // DataTable dt1 = objWorkFlow.GetWorkFlowPMDL();
            DataTable dt1 = objWorkFlow.GetWorkFlowPMDL(ddlProject.SelectedValue, ddlElement.SelectedValue);
            var result = new string[dt1.Rows.Count];
            for (var i = 0; i < dt1.Rows.Count; i++)
            {
                if (dt1.Rows[i].ItemArray[0].ToString().Trim().Contains(","))
                {
                    string[] PMDL = dt1.Rows[i].ItemArray[0].ToString().Trim().Split(',');
                    result[i] = String.Join("','", PMDL);
                }
                else
                {
                    result[i] = dt1.Rows[i].ItemArray[0].ToString().Trim();
                }
            }
            if (dt1.Rows.Count == 1)
            {
                PMDLString = String.Join("''", result);
            }
            else
            {
                 PMDLString = String.Join("','", result);
            }
            DataTable dt = objWorkFlow.GetUnassignedPMDLdoc(ddlProject.SelectedValue, ddlElement.SelectedValue, PMDLString);
            if (dt.Rows.Count > 0)
            {
                ddlPMDL.DataSource = dt;
                ddlPMDL.DataValueField = "DocumentID";
                ddlPMDL.DataTextField = "PMDLDoc";
                ddlPMDL.DataBind();
            }
            }
        
       
    }
}