using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PreOrderWorkflow
{
    public partial class Notes : System.Web.UI.Page
    {
        //   public static string cs = ConfigurationManager.AppSettings["ConnectionTest"];ConnectionLive
        public static string csLive = ConfigurationManager.AppSettings["ConnectionLive"];
        public static string csERPLive = ConfigurationManager.AppSettings["ConnectionERPLive"];
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
               
                try
                {
                    WorkFlow objWorkFlow = new WorkFlow(1);
                    string note_handle = (Request.QueryString["handle"]).ToString();
                    objWorkFlow.NotesHandle = note_handle;
                    string Indexvalue = (Request.QueryString["Index"]).ToString();
                    objWorkFlow.IndexValue = Indexvalue;
                    DataTable dt = objWorkFlow.GetNotesFromUSer();
                    if (dt.Rows.Count > 0)
                    {
                        rptNotes.Visible = true;
                        rptNotes.DataSource = dt;
                        rptNotes.DataBind();

                    }
                }
                catch (Exception ex)
                {
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", ex.Message, true);
                    throw ex;

                }
            }
           
           
           
        }

        protected void btnAttach_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in rptNotes.Items)
            {
                if (((CheckBox)item.FindControl("Checkbox1")).Checked)
                {
                    using (SqlConnection con = new SqlConnection(csLive))   // change cs to csLive everywhere inside sql connection ....this is just for testing sagar

                    {
                        int nRecord = 0;
                        Label sNoteId = (Label)item.FindControl("sNoteId");
                        string sNotes = (sNoteId.Text).ToString();
                        //and t_hndl = 'J_PREORDER_WORKFLOW'
                        SqlConnection conLive = new SqlConnection(csLive);
                        string sNoteToAttach = @"select * from ttcisg132200 where t_indx='"+ sNotes + "'";
                        ////SqlCommand cmd = new SqlCommand(sNoteToAttach, con);
                        // cmd.CommandType = CommandType.Text;
                        //string dResult = cmd.ExecuteDataset().ToString();
                        string revision ="_"+Request.QueryString["Revision"]+"_1";
                        DataTable dt= Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(conLive, CommandType.Text, sNoteToAttach).Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            //string sDocumentId = dt.Rows[0]["t_dcid"].ToString();
                            string sFileName = dt.Rows[0]["t_fnam"].ToString();
                            string sLibCode = dt.Rows[0]["t_lbcd"].ToString();
                            string sattachedby = dt.Rows[0]["t_atby"].ToString();
                            //  DateTime sattachedon = DateTime(dt.Rows[0]["t_aton"]);



                           // //string sCount = @"select MAX(CONVERT(int,t_drid)) from ttcisg132200";
                            nRecord = new Random(Guid.NewGuid().GetHashCode()).Next();
                            string sDocumentId = "AAA" + nRecord;
                           // string sCount = @"SELECT (ISNULL(MAX(t_rnum),0) + 1)as RunningNo FROM ttcisg131200 where t_acti='Y'";
                            //string count =(Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(conLive, CommandType.Text, sCount)).ToString();
                            //nRecord = Convert.ToInt32(count);
                            //// nRecord = nRecord + 1
                            //string UpdateCount = @"Update ttcisg131200 set t_rnum='" + nRecord + "' where t_acti='Y'";
                            //SqlCommand cmdUpdate = new SqlCommand(UpdateCount, con);
                            //if (con.State == ConnectionState.Closed)
                            //{
                            //    con.Open();
                            //}
                            //cmdUpdate.ExecuteNonQuery();

                            string InsertIDMSAttachment = @"Insert into ttcisg132200 (t_atby,t_aton,t_prcd,t_drid,t_dcid,t_fnam,t_lbcd,t_hndl,t_indx,t_Refcntd,t_Refcntu) 
                                            values('" + sattachedby + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','Dmisg134_1_vendor','" + nRecord + "','" + sDocumentId + "','" + sFileName + "','" + sLibCode + "','IDMSRECEIPTS_200','" + Request.QueryString["RecNo"] + revision + "','','')";

                            SqlCommand cmd = new SqlCommand(InsertIDMSAttachment, con);
                            
                            cmd.CommandType = CommandType.Text;
                            if (con.State == ConnectionState.Closed)
                            {
                                con.Open();
                            }
                            cmd.ExecuteNonQuery();

                            con.Close();
                        }
                    }
                   

                }
            }
           Session["ReceiptNo"] = Request.QueryString["RecNo"];
            Session["item"] = Request.QueryString["Item"];
            Session["IndentNo"] = Request.QueryString["Indent"];
            // Response.Redirect(Request.UrlReferrer.ToString()); //To redirect to previous page Request.QueryString["Index"]
            string sRedirect = "GeneratePreOrderReceipt.aspx?u=" + Request.QueryString["AttachedBy"] + "&WFID=" + Request.QueryString["Index"] + "";
            Response.Redirect(sRedirect);
        }
    }
}