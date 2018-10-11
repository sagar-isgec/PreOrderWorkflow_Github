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
    public partial class PreOrderProjDetails : System.Web.UI.Page
    {
        WorkFlow objWorkFlow;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                    GetData();
                    GetProjects();
                    gvData.Visible = true;
            }
        }
        private void GetData()
        {
            objWorkFlow = new WorkFlow();
            objWorkFlow.UserId = Request.QueryString["u"];
            DataTable dt = objWorkFlow.GetPreOrderData();
            gvData.DataSource = dt;
            gvData.DataBind();
        }
        private void GetDatabyDate_Project()
        {
            if (ddlProjectFrom.SelectedValue != "Select" && ddlProjectTo.SelectedValue != "Select" && txtDateFrom.Text == "" && txtDateTo.Text == "")
            {
                objWorkFlow = new WorkFlow();
                objWorkFlow.ProjectFrom = ddlProjectFrom.SelectedValue;
                objWorkFlow.ProjectTo = ddlProjectTo.SelectedValue;
                DataTable dtbyProj = objWorkFlow.GetPreOrderData_byProject();
                gvData.DataSource = dtbyProj;
                gvData.DataBind();
            }
            else if (ddlProjectFrom.SelectedValue == "Select" && ddlProjectTo.SelectedValue == "Select" && txtDateFrom.Text != "" && txtDateTo.Text != "")
            {
                objWorkFlow = new WorkFlow();
                objWorkFlow.DateFrom = txtDateFrom.Text;
                objWorkFlow.DateTo = txtDateTo.Text;
                DataTable dtbyProj = objWorkFlow.GetPreOrderData_byDate();
                gvData.DataSource = dtbyProj;
                gvData.DataBind();
            }

            else if (ddlProjectFrom.SelectedValue != "Select" && ddlProjectTo.SelectedValue != "Select" && txtDateFrom.Text != "" && txtDateTo.Text != "")
            {
                objWorkFlow = new WorkFlow();
                objWorkFlow.DateFrom = txtDateFrom.Text;
                objWorkFlow.DateTo = txtDateTo.Text;
                objWorkFlow.ProjectFrom = ddlProjectFrom.SelectedValue;
                objWorkFlow.ProjectTo = ddlProjectTo.SelectedValue;
                DataTable dtbyProj = objWorkFlow.GetPreOrderData_byDate_Project();
                gvData.DataSource = dtbyProj;
                gvData.DataBind();
            }

            else
            {
                GetData();
            }

        }
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

        protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvData.PageIndex = e.NewPageIndex;
            GetDatabyDate_Project();
          //  GetData();
        }

        //protected void ddlProjectFrom_SelectedIndexChanged(object sender, EventArgs e)
        //{

           
        //}

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Pre Order Details" + DateTime.Now.ToString("dd/MM/yy HH:mms") + ".xls");
            Response.ContentType = "File/Data.xls";
            StringWriter StringWriter = new System.IO.StringWriter();
            HtmlTextWriter HtmlTextWriter = new HtmlTextWriter(StringWriter);
            gvData.AllowPaging = false;
            this.GetDatabyDate_Project();
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
            GetDatabyDate_Project();
        }
    }
}
