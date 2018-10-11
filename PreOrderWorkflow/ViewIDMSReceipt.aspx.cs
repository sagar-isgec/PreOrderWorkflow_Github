using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PreOrderWorkflow
{
    public partial class ViewIDMSReceipt : System.Web.UI.Page
    {
        WorkFlow objWorkFlow;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetData();
            }
        }
        /// <summary>
        /// Fetch all Preorder Data on Page load or when user do not select anyvalue from dropdown
        /// </summary>
        private void GetData()
        {
            objWorkFlow = new WorkFlow();
            objWorkFlow.WFID = Convert.ToInt32(Request.QueryString["WFID"]);
            DataTable dt = objWorkFlow.GetReceiptNo();
            if (dt.Rows.Count>0)
            { 
            if (dt.Rows[0]["ReceiptNo"].ToString() != "")
            {
                DataTable dtAllReceiptDetails = new DataTable();
                foreach(DataRow dr in dt.Rows)
                {
                    //loop thru receipt to get single row at a time and add that to a datatable i.e merge
                    // at last bind the merged datatable to gvData
                    string sReceiptNumber = dr["ReceiptNo"].ToString();
                   // string sPMDLDocNumber = dr["PMDLDocNo"].ToString();
                    //DataTable dtReceipt = objWorkFlow.GetReceiptDetails(sReceiptNumber, sPMDLDocNumber);
                    DataTable dtReceipt = objWorkFlow.GetReceiptDetails(sReceiptNumber);
 
                     dtAllReceiptDetails.Merge(dtReceipt);
                }
                    if (dtAllReceiptDetails.Rows.Count > 0)
                    {
                        gvData.Visible = true;
                        lblNoRecord.Visible = false;
                        gvData.DataSource = dtAllReceiptDetails;
                        // gvData.RowDataBound += new GridViewRowEventHandler(gvData_RowDataBound);
                        gvData.DataBind();
                    }
                    else
                    {
                        gvData.Visible = false;
                        lblNoRecord.Visible = true;
                        lblNoRecord.Text = "No Record Found for WFID -" + objWorkFlow.WFID + "!";
                    }
            }
            }
            else
            {
                gvData.Visible = false;
                lblNoRecord.Visible = true;
                lblNoRecord.Text = "No Record Found for WFID -" + objWorkFlow.WFID + "!";
            }
        }


        //protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        var dataRowView = e.Row.DataItem as DataRowView;
        //        DataRow row = dataRowView.Row;

        //        var Current = (row["Parent_WFID"] as double?) ?? null;
                
        //            if (row.ItemArray[1].ToString() == "0")
        //           e.Row.BackColor = System.Drawing.Color.Orange;
                
        //       // e.Row.Cells[1].BackColor = System.Drawing.Color.Orange;

        //          else
        //                //e.Row.Cells[1].BackColor = System.Drawing.Color.Yellow;
        //        e.Row.BackColor = System.Drawing.Color.Yellow;
        //    }
        //    }
        
        /// <summary>
        /// Change Gridview based on selected pagination
        /// </summary>

        protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvData.PageIndex = e.NewPageIndex;
        }

   
        /// <summary>
        /// Export Grid View data to an Export File 
        /// </summary>

        //protected void btnExport_Click(object sender, EventArgs e)
        //{
        //    Response.Clear();
        //    Response.AddHeader("content-disposition", "attachment;filename=Pre Order Details" + DateTime.Now.ToString("dd/MM/yy HH:mms") + ".xls");
        //    Response.ContentType = "File/Data.xls";
        //    StringWriter StringWriter = new System.IO.StringWriter();
        //    HtmlTextWriter HtmlTextWriter = new HtmlTextWriter(StringWriter);
        //    gvData.AllowPaging = false;
        //    this.GetDatabyDate_Project_Buyer_PMDL();
        //    gvData.RenderControl(HtmlTextWriter);
        //    Response.Write(StringWriter.ToString());
        //    Response.End();
        //}
        //public override void VerifyRenderingInServerForm(Control control)
        //{
        //    /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
        //       server control at run time. */
        //}
    
    }
}
