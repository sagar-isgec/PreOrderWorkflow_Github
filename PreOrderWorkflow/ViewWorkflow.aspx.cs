using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PreOrderWorkflow
{
    public partial class ViewWorkflow : System.Web.UI.Page
    {
        WorkFlow objWorkFlow;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["WFID"] != null)
                {
                    GetData();
                }
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
                txtProject.Text = dt.Rows[0]["Project"].ToString();
                txtElement.Text = dt.Rows[0]["Element"].ToString();
                txtSpecification.Text = dt.Rows[0]["SpecificationNo"].ToString();
                txtPMDLDoc.Text = dt.Rows[0]["PMDLDocNo"].ToString();
                txtBuyer.Text = dt.Rows[0]["BuyerName"].ToString() + "-" + dt.Rows[0]["Buyer"].ToString();
                txtManager.Text = dt.Rows[0]["ManagerName"].ToString() + "-" + dt.Rows[0]["Manager"].ToString();
                txtStatus.Text = dt.Rows[0]["WF_Status"].ToString();
                txtCreater.Text = dt.Rows[0]["EmployeeName"].ToString();
                txtDAte.Text = Convert.ToDateTime(dt.Rows[0]["DateTime"].ToString()).ToString("dd-MM-yyyy");
                hdfUser.Value = dt.Rows[0]["UserId"].ToString();
                hdfWFID.Value =(dt.Rows[0]["Parent_WFID"]).ToString();
            }
            if (dtGetNotes.Rows.Count > 0)
            {
                txtNotes.Text = dtGetNotes.Rows[0]["Notes"].ToString();
            }
        }

        protected void btnAttachment_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["WFID"] != null)
            {

              //  string url = "http://localhost/Attachment/Attachment.aspx?AthHandle=J_PREORDER_WORKFLOW" + "&Index=" + Request.QueryString["WFID"] + "&AttachedBy=" + Request.QueryString["u"]; //+ "&ed=a&RefHandle=TRANSMITTALLINES_200&RefIndex=BOi000532_JB0973-50270100-027-0001_00";
                    string url = "http://192.9.200.146/Attachment/Attachment.aspx?AthHandle=J_PREORDER_WORKFLOW" + "&Index=" + Request.QueryString["WFID"] + "&AttachedBy=" + Request.QueryString["u"]; //changes need to be reverted once testing is done sagar
                    string s = "window.open('" + url + "','abc','width=800,height=600,left=100,top=100,resizable=yes,scrollbars=yes');"; //, 'width=300,height=100,left=100,top=100,resizable=yes'
                    ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
        }
        protected void btnNotes_Click(object sender, EventArgs e)
        {
            string MailTo = string.Empty;
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
                if (Request.QueryString["p"] != null && Request.QueryString["p"] == "B")
                {
                    objWorkFlow = new WorkFlow();
                    DataTable dtMailTo = objWorkFlow.GetMAilID(hdfUser.Value);
                    MailTo = dtMailTo.Rows[0]["EMailid"].ToString();
                }
                if (Request.QueryString["p"] != null && Request.QueryString["p"] == "U")
                {
                    string[] Buyer = txtBuyer.Text.Split('-');
                    string[] manager = txtManager.Text.Split('-');
                    objWorkFlow = new WorkFlow();
                    DataTable dtMailBuyerTo = objWorkFlow.GetMAilID(Buyer[1]);
                    DataTable dtMailManagerTo = objWorkFlow.GetMAilID(Buyer[1]);
                    MailTo = dtMailBuyerTo.Rows[0]["EMailid"].ToString() +"," + dtMailManagerTo.Rows[0]["EMailid"].ToString();
                }
                string url = "http://192.9.200.146/Attachment/Notes.aspx?handle=J_PREORDER_WORKFLOW&Index=" + Request.QueryString["WFID"] + "&user=" + Request.QueryString["u"] + "&Em=" + MailTo  + "&Hd=" + Header + "&Tl=" + txtSpecification.Text;
                string s = "window.open('" + url + "','abc','width=1300,height=700,left=100,top=100,resizable=yes,scrollbars=yes');"; //, 'width=300,height=100,left=100,top=100,resizable=yes'
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
        }
    }
}