using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PreOrderWorkflow
{
    public partial class CommercialofferFinal : System.Web.UI.Page
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
            objWorkFlow.WF_Status = "'Commercial offer Finalized'";
            objWorkFlow.UserId = Request.QueryString["u"];
            DataTable dt = objWorkFlow.GetWFBY_Status();
            gvData.DataSource = dt;
            gvData.DataBind();
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            Response.Redirect("RaisedEnquiry.aspx?WFPID=" + btn.CommandArgument + "&u=" + Request.QueryString["u"]);
        }
    }
    
}