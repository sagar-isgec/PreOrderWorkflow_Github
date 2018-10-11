using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PreOrderWorkflow
{
    public partial class TechnicalClearance : System.Web.UI.Page
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
            objWorkFlow.WF_Status = "'Isgec Comment Submitted'";
            objWorkFlow.UserId = Request.QueryString["u"];
            DataTable dt = objWorkFlow.GetWFBY_Status();
            gvData.DataSource = dt;
            gvData.DataBind();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {

            LinkButton btn = (LinkButton)sender;
            string[] Value = btn.CommandArgument.Split('&');
            objWorkFlow = new WorkFlow();
            objWorkFlow.WFID = Convert.ToInt32(Value[0]);
            objWorkFlow.UserId = Request.QueryString["u"];
            objWorkFlow.WF_Status = "Technically cleared";
            int res = objWorkFlow.UpdateWF_Status();
            if (res > 0)
            {
                InsertPreHistory(objWorkFlow.WFID, "Technically cleared");
                DataTable dtMailTo = objWorkFlow.GetMAilID(Value[1]);
                string MailTo = dtMailTo.Rows[0]["EMailid"].ToString();
                SendMail(MailTo);
                GetData();
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert('Technically cleared');", true);
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

        public void SendMail(string MailTo)
        {
            try
            {

                // objWorkFlow = new WorkFlow();
                //  objWorkFlow.UserId = Request.QueryString["AttachedBy"];
                // DataTable dtUserMail = ;
                MailMessage mM = new MailMessage();
                mM.From = new MailAddress("baansupport@isgec.co.in");
                // mM.To.Add(txtTo.Text.Trim());
                // string[] MailTo = txtTo.Text.Split(';');
                // foreach (string Mailid in MailTo)
                // {
                //     mM.To.Add(new MailAddress(Mailid));
                //  }
                mM.To.Add(MailTo); //MailTo
                mM.Subject = "Technical Cleared";
                mM.Body = "Test"; //txtNotes.Text;
                mM.IsBodyHtml = true;
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

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert('Some technical issue Mail not sent');", true);
            }
            //}
        }
    }
}