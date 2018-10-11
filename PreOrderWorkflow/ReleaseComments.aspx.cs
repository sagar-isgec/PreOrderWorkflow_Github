using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PreOrderWorkflow
{
    public partial class Release_comments : System.Web.UI.Page
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
            objWorkFlow.WF_Status = "'Technical offer Received'";
            objWorkFlow.UserId = Request.QueryString["u"];
            DataTable dt = objWorkFlow.GetWFBY_Status();
            gvData.DataSource = dt;
            gvData.DataBind();
        }
        protected void btnReleseComment_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string[] values = btn.CommandArgument.Split('&');
            objWorkFlow = new WorkFlow();
            objWorkFlow.WFID = Convert.ToInt32(values[0]);
            Response.Redirect("RaiseEnquiryForm.aspx?WFID=" + objWorkFlow.WFID + "&Status=Isgec Comment Submitted&u=" + Request.QueryString["u"] + "&WFPID=" + values[1]);
        }
    }
}