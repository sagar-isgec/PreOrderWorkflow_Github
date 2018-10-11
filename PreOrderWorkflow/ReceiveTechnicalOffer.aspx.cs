using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PreOrderWorkflow
{
    public partial class ReceiveTechnicalOffer : System.Web.UI.Page
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
            objWorkFlow.WF_Status = "'Enquiry Raised','Technical offer Received'";
            objWorkFlow.UserId = Request.QueryString["u"];
            DataTable dt = objWorkFlow.GetWFBY_Status();
            gvData.DataSource = dt;
            gvData.DataBind();
        }

        protected void btnReceiveTechOffer_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string[] values = btn.CommandArgument.Split('&');
            objWorkFlow = new WorkFlow();
            objWorkFlow.WFID = Convert.ToInt32(values[0]);
            if (values[2] == "Enquiry Raised")
            {
                Response.Redirect("RaiseEnquiryForm.aspx?WFID=" + objWorkFlow.WFID + "&Status=Technical offer Received&u=" + Request.QueryString["u"] + "&WFPID=" + values[1]);
            }
            if (values[2]== "Technical offer Received")
            {
                Response.Redirect("RaiseEnquiryForm.aspx?WFID=" + objWorkFlow.WFID + "&Status=Commercial offer Received&u=" + Request.QueryString["u"] + "&WFPID=" + values[1]);
            }
           
        }
    }
}