using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PreOrderWorkflow
{
    public partial class RaiseEnquery : System.Web.UI.Page
    {
        WorkFlow objWorkFlow;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["u"] != null)
                {
                    GetData();
                }
                else
                {

                }
            }
        }

        private void GetData()
        {
            objWorkFlow = new WorkFlow();
            objWorkFlow.WF_Status = "'Technical Specification Released'";
            objWorkFlow.UserId = Request.QueryString["u"];
            DataTable dt = objWorkFlow.GetWFBY_Status();
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
                    objWorkFlow.PMDLdocDesc = dt.Rows[0]["PMDLDocNo"].ToString();
                    objWorkFlow.Buyer = dt.Rows[0]["Buyer"].ToString();
                    objWorkFlow.Manager= dt.Rows[0]["Manager"].ToString();
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
        protected void btnReturm_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            objWorkFlow = new WorkFlow();

            objWorkFlow.WFID = Convert.ToInt32(btn.CommandArgument);
            objWorkFlow.UserId = Request.QueryString["u"];
            objWorkFlow.WF_Status = "Technical Specification Released Returned";
            int res = objWorkFlow.UpdateWF_Status();
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
            objWorkFlow.Manager = dt.Rows[0]["Manager"].ToString();
            objWorkFlow.PMDLdocDesc = dt.Rows[0]["PMDLDocNo"].ToString();
            objWorkFlow.UserId = Request.QueryString["u"];
            objWorkFlow.WF_Status = status;
            objWorkFlow.Supplier = dt.Rows[0]["Supplier"].ToString();
            objWorkFlow.SupplierName = dt.Rows[0]["SupplierName"].ToString();
            objWorkFlow.InserPreOrderHistory();
        }
        protected void lnkViewWorkflow_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            Response.Redirect("ViewWorkflow.aspx?WFID=" + btn.CommandArgument + "&u=" + Request.QueryString["u"] + "&p=B");
        }
    }
}