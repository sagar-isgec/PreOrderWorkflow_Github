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


    public partial class PreOrderDocumentTracking : System.Web.UI.Page
    {
        WorkFlow objWorkFlow;

        protected void Page_PreRender(object sender, EventArgs e)
        {

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetData();
                    GetProjects();
                    GetBuyer();
                    GetPMDL();

                    gvData.Visible = true;
                lblNoRecord.Visible = false;
                lblChild.Visible = true;
                lblParent.Visible = true;
            }
        }
        /// <summary>
        /// Fetch all Preorder Data on Page load or when user do not select anyvalue from dropdown
        /// </summary>
        private void GetData()
        {
            try
            {
                objWorkFlow = new WorkFlow();
                objWorkFlow.UserId = Request.QueryString["u"];
                DataTable dtAllReceiptStatus = new DataTable();
                DataTable dt = objWorkFlow.GetPreOrderDocumentTracking();
                foreach (DataRow dr in dt.Rows)
                {
                   if (dr["ReceiptNo"].ToString() != null)
                        {
                            if (dr["ReceiptNo"].ToString() != "")
                            {
                                string sReceiptNumber = dr["ReceiptNo"].ToString();
                                string ReceiptStatus = objWorkFlow.GetReceiptStatus(sReceiptNumber);
                                dr["ReceiptStatus"] = ReceiptStatus;
                            }
                        }
                }

                gvData.DataSource = dt;
                gvData.RowDataBound += new GridViewRowEventHandler(gvData_RowDataBound);
                gvData.DataBind();
                gvData.Columns[3].ItemStyle.Width = 500;
            }
            catch (Exception ex)
            {
                string sErrormsg=ex.Message;
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert('Some technical issue');", true);
            }
            //  gvData.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

        }


        protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataRowView = e.Row.DataItem as DataRowView;
                DataRow row = dataRowView.Row;

                var Current = (row["Parent_WFID"] as double?) ?? null;
                
                    if (row.ItemArray[1].ToString() == "0")
                   e.Row.BackColor = System.Drawing.Color.Orange;
                
               // e.Row.Cells[1].BackColor = System.Drawing.Color.Orange;

                  else
                        //e.Row.Cells[1].BackColor = System.Drawing.Color.Yellow;
                e.Row.BackColor = System.Drawing.Color.Yellow;
                if (e.Row.RowType == DataControlRowType.DataRow)

                {

                    if (e.Row.RowIndex == 0)
                        e.Row.Style.Add("height", "40px");
                }

            }
            }
        
        /// <summary>
        /// Fetch PreOrder Data based on the dropdown selection
        /// </summary>
       


        private void GetDatabyDate_Project_Buyer_PMDL()
        {
            objWorkFlow = new WorkFlow();
            DataTable dt = new DataTable();
            DataTable dtAllReceiptStatus = new DataTable();

            if (ddlProjectFrom.SelectedValue != "Select" && ddlProjectTo.SelectedValue != "Select")
            {
                objWorkFlow.ProjectFrom = ddlProjectFrom.SelectedValue;
                objWorkFlow.ProjectTo = ddlProjectTo.SelectedValue;
            }
            else
            {
                objWorkFlow.ProjectFrom = "";
                objWorkFlow.ProjectTo = "ZZZZZZ";
            }
            if (ddlBuyerFrom.SelectedValue != "Select" && ddlBuyerTo.SelectedValue != "Select")
            {
                objWorkFlow.BuyerFrom = ddlBuyerFrom.SelectedValue;
                objWorkFlow.BuyerTo = ddlBuyerTo.SelectedValue;
            }
            else
            {
                objWorkFlow.BuyerFrom = "00000";
                objWorkFlow.BuyerTo = "99999";
            }
            if (ddlPMDLDocFrom.SelectedValue != "Select" && ddlPMDLDocTo.SelectedValue != "Select")
            {
                if (ddlPMDLDocFrom.SelectedValue.Contains(",") || ddlPMDLDocTo.SelectedValue.Contains(","))
                {
                    if (ddlPMDLDocFrom.SelectedValue.Contains(","))
                    {
                        string[] PMDLFrom = ddlPMDLDocFrom.SelectedValue.Split('\'');
                        objWorkFlow.PMDLFrom = PMDLFrom[0];
                        }
                    else
                    {
                        objWorkFlow.PMDLFrom = ddlPMDLDocFrom.SelectedValue;
                    }
                    if (ddlPMDLDocFrom.SelectedValue.Contains(","))
                    {
                        string[] PMDLTo1 = ddlPMDLDocTo.SelectedValue.Split(',');
                        string[] PMDLTo= PMDLTo1[PMDLTo1.Length-1].Split('\'');
                        objWorkFlow.PMDLTo = PMDLTo[1];
                    }
                    else
                    {
                        objWorkFlow.PMDLTo = ddlPMDLDocFrom.SelectedValue;
                    }
                }
                else
                {
                    objWorkFlow.PMDLFrom = ddlPMDLDocFrom.SelectedValue;
                    objWorkFlow.PMDLTo = ddlPMDLDocTo.SelectedValue;
                }
            }
            else
            {
                objWorkFlow.PMDLFrom = "";
                objWorkFlow.PMDLTo = "ZZZZZZ";
            }
            if (txtDateFrom.Text != "" && txtDateTo.Text != "")
            {
                objWorkFlow.DateFrom = txtDateFrom.Text;
                objWorkFlow.DateTo = txtDateTo.Text;
                dt = objWorkFlow.GetPreOrderDocumentTracking_byProject_Buyer_Date_PMDL();
            }
            else
            {
                dt= objWorkFlow.GetPreOrderDocumentTracking_byProject_Buyer_PMDL();
            }
          
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ReceiptNo"].ToString() != null && dr["ReceiptNo"].ToString() != "")
                {
                    string sReceiptNumber = dr["ReceiptNo"].ToString();
                    string ReceiptStatus = objWorkFlow.GetReceiptStatus(sReceiptNumber);
                    dr["ReceiptStatus"] = ReceiptStatus;
                }
            }
            if (dt.Rows.Count > 0)
            {
                gvData.Visible = true;
                lblChild.Visible = true;
                lblParent.Visible = true;
                lblNoRecord.Visible = false;
                gvData.DataSource = dt;
                gvData.RowDataBound += new GridViewRowEventHandler(gvData_RowDataBound);
                gvData.DataBind();
            }
            else
            {
                gvData.Visible = false;
                lblChild.Visible = false;
                lblParent.Visible = false;
                lblNoRecord.Visible = true;
            }
        }
        /// <summary>
        /// Fetch all distinct Projects from Table WF1_PreOrder to populate Project DropDown
        /// </summary>
        private void GetProjects()
        {
            WorkFlow objWorkFlow = new WorkFlow();
            DataTable dt = objWorkFlow.PopulateDropdownList();
            ddlProjectFrom.DataSource = dt;
            ddlProjectFrom.DataValueField = "Project";
            ddlProjectFrom.DataTextField = "Project";
            ddlProjectFrom.DataBind();
            ddlProjectFrom.Items.Insert(0, "Select");

            ddlProjectTo.DataSource = dt;
            ddlProjectTo.DataValueField = "Project";
            ddlProjectTo.DataTextField = "Project";
            ddlProjectTo.DataBind();
            ddlProjectTo.Items.Insert(0, "Select");
        }

        private void GetBuyer()
        {
            WorkFlow objWorkFlow = new WorkFlow();
            DataTable dt = objWorkFlow.PopulateBuyerDropdownList();
            ddlBuyerFrom.DataSource = dt;
            ddlBuyerFrom.DataValueField = "Buyer";
            ddlBuyerFrom.DataTextField = "EmployeeName";
            ddlBuyerFrom.DataBind();
            ddlBuyerFrom.Items.Insert(0, "Select");

            ddlBuyerTo.DataSource = dt;
            ddlBuyerTo.DataValueField = "Buyer";
            ddlBuyerTo.DataTextField = "EmployeeName";
            ddlBuyerTo.DataBind();
            ddlBuyerTo.Items.Insert(0, "Select");
        }

        private void GetPMDL()
        {
            WorkFlow objWorkFlow = new WorkFlow();
            DataTable dt = objWorkFlow.PopulatePMDLDropdownList();
            ddlPMDLDocFrom.DataSource = dt;
            ddlPMDLDocFrom.DataValueField = "PMDLDocNo";
            ddlPMDLDocFrom.DataTextField = "PMDLDocNo";
            ddlPMDLDocFrom.DataBind();
            ddlPMDLDocFrom.Items.Insert(0, "Select");

            ddlPMDLDocTo.DataSource = dt;
            ddlPMDLDocTo.DataValueField = "PMDLDocNo";
            ddlPMDLDocTo.DataTextField = "PMDLDocNo";
            ddlPMDLDocTo.DataBind();
            ddlPMDLDocTo.Items.Insert(0, "Select");
        }
        /// <summary>
        /// Change Gridview based on selected pagination
        /// </summary>

        protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvData.PageIndex = e.NewPageIndex;
            GetDatabyDate_Project_Buyer_PMDL();
        }

        public bool GetVisible(object value)
        {
            if (value.ToString() == "0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Export Grid View data to an Export File 
        /// </summary>

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Pre Order Details" + DateTime.Now.ToString("dd/MM/yy HH:mms") + ".xls");
            Response.ContentType = "File/Data.xls";
            StringWriter StringWriter = new System.IO.StringWriter();
            HtmlTextWriter HtmlTextWriter = new HtmlTextWriter(StringWriter);
            gvData.AllowPaging = false;
            this.GetDatabyDate_Project_Buyer_PMDL();
            gvData.RenderControl(HtmlTextWriter);
            Response.Write(StringWriter.ToString());
            Response.End();
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }


        protected void btnGetRecord_Click(object sender, EventArgs e)
        {
            //GetDatabyDate_Project_Buyer();
            GetDatabyDate_Project_Buyer_PMDL();
        }

        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            ddlProjectTo.SelectedIndex = ddlProjectFrom.SelectedIndex;
        }

        protected void ddlBuyer_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlBuyerTo.SelectedIndex = ddlBuyerFrom.SelectedIndex;
        }
        protected void ddlPMDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlPMDLDocTo.SelectedIndex = ddlPMDLDocFrom.SelectedIndex;
        }

        protected void txtDateFrom_SelectedDateChange(object sender, EventArgs e)
        {
            txtDateTo.Text = txtDateFrom.Text;
        }

        protected void btnReceipt_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            Response.Redirect("ViewIDMSReceipt.aspx?WFID=" + btn.CommandArgument);
        }
    }
}
