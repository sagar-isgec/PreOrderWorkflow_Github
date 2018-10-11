using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PreOrderWorkflow
{
    public partial class ChangeProjectStatus : System.Web.UI.Page
    {
        WorkFlow objWorkFlow;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["u"] != null)
                {
                    GetData();
                    GetProject();
                    GetElement();
                    GetPMDL();
                    GetSpecification();
                    ddlProject.Visible = true;
                    ddlElement.Visible = true;
                    ddlPMDL.Visible = true;
                    ddlSpecification.Visible = true;
                    gvData.Visible = true;
                }
                else
                {
                    ddlProject.Visible = false;
                    ddlElement.Visible = false;
                    ddlPMDL.Visible = false;
                    ddlSpecification.Visible = false;
                    gvData.Visible = false;
                }
            }
        }
        private void GetData()
        {
            objWorkFlow = new WorkFlow();
            objWorkFlow.WF_Status = "Commercial offer Finalized";
            objWorkFlow.UserId = Request.QueryString["u"];
            DataTable dt = objWorkFlow.GetFinalizedStatus();
            gvData.DataSource = dt;
            gvData.DataBind();
           
        }
        protected void btnRaiseEnquiry_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            objWorkFlow = new WorkFlow();
            objWorkFlow.WFID = Convert.ToInt32(btn.CommandArgument);
            objWorkFlow.WF_Status = "Enquiry in progress";
            objWorkFlow.UserId = Request.QueryString["u"];
            int res = objWorkFlow.UpdateWF_Status();
            if (res > 0)
            {
                DataTable dt = objWorkFlow.GetWFById();
                if (dt.Rows.Count > 0)
                {
                    objWorkFlow.WF_Status = "Enquiry Created";
                    objWorkFlow.Parent_WFID = Convert.ToInt32(btn.CommandArgument);
                    objWorkFlow.Project = dt.Rows[0]["Project"].ToString();
                    objWorkFlow.Element = dt.Rows[0]["Element"].ToString();
                    objWorkFlow.SpecificationNo = dt.Rows[0]["SpecificationNo"].ToString();
                    objWorkFlow.Buyer = dt.Rows[0]["Buyer"].ToString();
                    DataTable dtres = objWorkFlow.InsertPreOrderData();
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
        private void InsertPreHistory(int Id, string status)
        {
            objWorkFlow = new WorkFlow();
            objWorkFlow.WFID = Id;
            DataTable dt = objWorkFlow.GetWFById();
            objWorkFlow.Parent_WFID = Convert.ToInt32(dt.Rows[0]["Parent_WFID"]);
            objWorkFlow.Project = dt.Rows[0]["Project"].ToString();
            objWorkFlow.Element = dt.Rows[0]["Element"].ToString();
            objWorkFlow.SpecificationNo = dt.Rows[0]["SpecificationNo"].ToString();
            objWorkFlow.Buyer = dt.Rows[0]["Buyer"].ToString();
            objWorkFlow.UserId = Request.QueryString["u"];
            objWorkFlow.WF_Status = status;
            objWorkFlow.Supplier = dt.Rows[0]["Supplier"].ToString();
            objWorkFlow.SupplierName = dt.Rows[0]["SupplierName"].ToString();
            objWorkFlow.InserPreOrderHistory();
        }
        protected void btnClosed_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            objWorkFlow = new WorkFlow();

            objWorkFlow.WFID = Convert.ToInt32(btn.CommandArgument);
            objWorkFlow.UserId = Request.QueryString["u"];
            objWorkFlow.WF_Status = "Commercial offer Finalized";
            int res = objWorkFlow.UpdateWF_Status();
            if (res > 0)
            {
                InsertPreHistory(Convert.ToInt32(btn.CommandArgument), "Commercial offer Finalized");
                GetData();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert('Some technical issue');", true);
            }
        }
        protected void lnkViewWorkflow_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            Response.Redirect("ViewWorkflow.aspx?WFID=" + btn.CommandArgument+"&u="+Request.QueryString["u"]+"&p=B");
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            Response.Redirect("RaisedEnquiry.aspx?WFPID=" + btn.CommandArgument+"&u="+Request.QueryString["u"]);
        }

        private void GetProject()
        {
            WorkFlow objWorkFlow = new WorkFlow();
            DataTable dtProj = objWorkFlow.PopulateProjectDropdown();
            ddlProject.DataSource = dtProj;
            ddlProject.DataValueField = "Project";
            ddlProject.DataTextField = "Project";
            ddlProject.DataBind();
            ddlProject.Items.Insert(0, "Select Project");
        }

        private void GetElement()
        {
            WorkFlow objWorkFlow = new WorkFlow();
            DataTable dtElement = objWorkFlow.PopulateElementDropdown();
            ddlElement.DataSource = dtElement;
            ddlElement.DataValueField = "Element";
            ddlElement.DataTextField = "Element";
            ddlElement.DataBind();
            ddlElement.Items.Insert(0, "Select Element");
        }
        private void GetPMDL()
        {
            WorkFlow objWorkFlow = new WorkFlow();
            DataTable dtPMDL = objWorkFlow.PopulatePMDLDropdown();
            ddlPMDL.DataSource = dtPMDL;
            ddlPMDL.DataValueField = "PMDLDocNo";
            ddlPMDL.DataTextField = "PMDLDocNo";
            ddlPMDL.DataBind();
            ddlPMDL.Items.Insert(0, "Select PMDLDocNo");
        }
        private void GetSpecification()
        {
            WorkFlow objWorkFlow = new WorkFlow();
            DataTable dtSpec = objWorkFlow.PopulateSpecDropdown();
            ddlSpecification.DataSource = dtSpec;
            ddlSpecification.DataValueField = "SpecificationNo";
            ddlSpecification.DataTextField = "SpecificationNo";
            ddlSpecification.DataBind();
            ddlSpecification.Items.Insert(0, "Select SpecificationNo");
        }
        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            objWorkFlow = new WorkFlow();
            //if (ddlProject.SelectedValue.Contains("-"))
            //{
            //    string[] ProjCode = ddlProject.SelectedValue.Split('-');

            //    objWorkFlow.Project = ProjCode[0];
            //}
            //else
            //{
                objWorkFlow.Project = ddlProject.SelectedValue;
          //  }
            
            ddlElement.SelectedValue = "Select Element";
            ddlPMDL.SelectedValue = "Select PMDLDocNo";
            ddlSpecification.SelectedValue = "Select SpecificationNo";

            objWorkFlow.WF_Status = "Commercial offer Finalized";
            objWorkFlow.UserId = Request.QueryString["u"];
            if (objWorkFlow.Project != "Select Project")
            {
                DataTable dtbyProj = objWorkFlow.GetWFBY_Status_Project();
                //if (dtbyProj.Rows[0]["Project"].ToString() != "")
                //{
                    gvData.DataSource = dtbyProj;
                    gvData.DataBind();
                //}
            }
            else
            {
                GetData();
            }

            // }
        }

        protected void ddlElement_SelectedIndexChanged(object sender, EventArgs e)
        {
            objWorkFlow = new WorkFlow();
            //if (ddlElement.SelectedValue.Contains("-"))
            //{
            //    string[] Element = ddlElement.SelectedValue.Split('-');
            //    objWorkFlow.Element = Element[0];
            //}
            //else
            //{
                objWorkFlow.Element = ddlElement.SelectedValue;
           // }
            ddlProject.SelectedValue = "Select Project";
            ddlPMDL.SelectedValue = "Select PMDLDocNo";
            ddlSpecification.SelectedValue = "Select SpecificationNo";

            objWorkFlow.WF_Status = "Commercial offer Finalized";
            objWorkFlow.UserId = Request.QueryString["u"];
            if (ddlElement.SelectedValue != "Select Element")
            {
                DataTable dtbyElement = objWorkFlow.GetWFBY_Status_Element();
                //if (dtbyElement.Rows[0]["Element"].ToString() != "")
                //{
                    gvData.DataSource = dtbyElement;
                    gvData.DataBind();
                //}
            }
            else
            {
                GetData();
            }

            // }
        }

        protected void ddlPMDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            objWorkFlow = new WorkFlow();
          //  string[] PMDL = ddlPMDL.SelectedValue.Split('-');
            objWorkFlow.PMDLdocDesc = ddlPMDL.SelectedValue;
            ddlProject.SelectedValue = "Select Project";
            ddlElement.SelectedValue = "Select Element";
            ddlSpecification.SelectedValue = "Select SpecificationNo";

            objWorkFlow.WF_Status = "Commercial offer Finalized";
            objWorkFlow.UserId = Request.QueryString["u"];
            if (ddlPMDL.SelectedValue != "Select PMDLDocNo")
            {
                DataTable dtbyPMDL = objWorkFlow.GetWFBY_Status_PMDL();
                //if (dtbyPMDL.Rows[0]["PMDLDocNo"].ToString() != "")
                //{
                    gvData.DataSource = dtbyPMDL;
                    gvData.DataBind();
                //}
            }
            else
            {
                GetData();
            }

            // }
        }

        protected void ddlSpecification_SelectedIndexChanged(object sender, EventArgs e)
        {
            objWorkFlow = new WorkFlow();
            //objWorkFlow.SpecificationNo = ddlProject.SelectedValue;
            //string[] Specification = ddlSpecification.SelectedValue.Split('-');
            objWorkFlow.SpecificationNo = ddlSpecification.SelectedValue;
            ddlProject.SelectedValue = "Select Project";
            ddlElement.SelectedValue = "Select Element";
            ddlPMDL.SelectedValue = "Select PMDLDocNo";
           

            objWorkFlow.WF_Status = "Commercial offer Finalized";
            objWorkFlow.UserId = Request.QueryString["u"];
            if (ddlSpecification.SelectedValue != "Select SpecificationNo")
            {
                DataTable dtbySpec = objWorkFlow.GetWFBY_Status_Spec();
                //if (dtbySpec.Rows[0]["SpecificationNo"].ToString() != "")
                //{
                    gvData.DataSource = dtbySpec;
                    gvData.DataBind();
                //}
            }
            else
            {
                GetData();
            }

            // }
        }

        protected void btnReactivate_Click(object sender, EventArgs e)
        {
            // objWorkFlow.WF_Status = "Enquiry in Progress";
            LinkButton btn = (LinkButton)sender;
            objWorkFlow = new WorkFlow();
            objWorkFlow.WFID = Convert.ToInt32(btn.CommandArgument);
            objWorkFlow.WF_Status = "Enquiry in progress";
            objWorkFlow.UserId = Request.QueryString["u"];
            int res = objWorkFlow.UpdateWF_Status();
           if (res > 0)
            {
                DataTable dt = objWorkFlow.GetWFById();
                if (dt.Rows.Count > 0)
                {
                    objWorkFlow.WF_Status = "Enquiry Created";
                    objWorkFlow.Parent_WFID = Convert.ToInt32(btn.CommandArgument);
                    objWorkFlow.Project = dt.Rows[0]["Project"].ToString();
                    objWorkFlow.Element = dt.Rows[0]["Element"].ToString();
                    objWorkFlow.SpecificationNo = dt.Rows[0]["SpecificationNo"].ToString();
                    objWorkFlow.Buyer = dt.Rows[0]["Buyer"].ToString();
                    DataTable dtres = objWorkFlow.InsertPreOrderData();
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


    }
}
