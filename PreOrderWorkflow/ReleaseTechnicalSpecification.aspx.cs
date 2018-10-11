using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.IO;

namespace PreOrderWorkflow
{
    public partial class ReleaseTechnicalSpecification : System.Web.UI.Page
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
            objWorkFlow.WF_Status = "'Technical Specification Released Returned','Technical Specification Released'";
            objWorkFlow.UserId = Request.QueryString["u"];
            DataTable dt = objWorkFlow.GetWFBYForUser();
            if (dt.Rows.Count > 0)
            {
                gvData.DataSource = dt;
                gvData.DataBind();
                DivNoRecords.Visible = false;
                divData.Visible = true;
            }
            else
            {
                DivNoRecords.Visible = true;
                divData.Visible = false;
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string[] value = btn.CommandArgument.Split('&');
            int WFID = Convert.ToInt32(value[0]);
            Response.Redirect("CreateForm.aspx?WFID=" + WFID + "&Status=" + value[1]+ "&u=" + Request.QueryString["u"] + "&p=U");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            objWorkFlow = new WorkFlow();
            objWorkFlow.WFID = Convert.ToInt32(btn.CommandArgument);
            int res = objWorkFlow.Delete_PreOrderHistory();
            //if (res > 0)
            //{
                int res1 = objWorkFlow.Delete_PreOrder();
                if (res1 > 0)
                {
                    GetData();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert('Successfully Deleted');", true);
                }
           // }
        }

        protected void btnNewForm_Click(object sender, EventArgs e)
        {
            Response.Redirect("CreateForm.aspx?u=" + Request.QueryString["u"]);
        }
        protected void lnkViewWorkflow_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            Response.Redirect("ViewWorkflow.aspx?WFID=" + btn.CommandArgument + "&u=" + Request.QueryString["u"] + "&p=U");
        }
    }
}