﻿using ClientesCasa.DomainModel;
using ClientesCasa.Interfaces;
using ClientesCasa.Presenter;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NucleoBase.Core;
using System.Drawing;
using ClientesCasa.Clases;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace ClientesCasa.Views.Reportes
{
    public partial class frmEstadoCuenta : System.Web.UI.Page, IVIewEstadoCuenta
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            oPresenter = new EstadoCuenta_Presenter(this, new DBEstadoCuenta());

            if (Request[txtPeriodo.UniqueID] != null)
            {
                if (Request[txtPeriodo.UniqueID].Length > 0)
                {
                    txtPeriodo.Text = Request[txtPeriodo.UniqueID];
                }
            }
        }

        protected void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                sMatricula = string.Empty;

                if (eSearchObj != null)
                    eSearchObj(sender, e);
            }
            catch (Exception ex)
            {

            }
        }

        protected void gvClientes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        e.Row.Attributes.Add("title", "De clic aqui para selecciona un cliente");
                        e.Row.Attributes.Add("OnMouseOver", "On(this);");
                        e.Row.Attributes.Add("OnMouseOut", "Off(this);");
                        e.Row.Attributes["OnClick"] = Page.ClientScript.GetPostBackClientHyperlink(this.gvClientes, "Select$" + e.Row.RowIndex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void gvClientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)dtClientes;
                gvClientes.PageIndex = e.NewPageIndex;
                LLenaClientes(dt);
            }
            catch (Exception ex)
            {

            }
        }

        protected void gvClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string sNumCliente = string.Empty;
                string sRazonSocial = string.Empty;
                string sRFC = string.Empty;

                bool ban = false;
                foreach (GridViewRow row in gvClientes.Rows)
                {
                    if (row.RowIndex == gvClientes.SelectedIndex)
                    {
                        row.BackColor = ColorTranslator.FromHtml("#D9E1E4");
                        row.ToolTip = string.Empty;
                        sMatricula = gvClientes.Rows[row.RowIndex].Cells[3].Text.S();
                        sClaveContrato = gvClientes.Rows[row.RowIndex].Cells[2].Text.S();

                        //sNumCliente = gvClientes.Rows[row.RowIndex].Cells[0].Text.S();
                        sNombrecliente = gvClientes.Rows[row.RowIndex].Cells[1].Text.S();
                        //sRazonSocial = gvClientes.Rows[row.RowIndex].Cells[2].Text.S();
                        //sRFC = gvClientes.Rows[row.RowIndex].Cells[3].Text.S();
                        ban = true;
                    }
                    else
                    {
                        row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                        row.ToolTip = "Clic para seleccionar esta fila.";
                    }
                }

                if (ban)
                {
                    txtPeriodo.Text = string.Empty;
                    mpePeriodo.Show();
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                //Response.Clear();
                //Response.Buffer = true;
                //Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("content-disposition", "attachment;filename=EstadoCuenta_" + sMatricula + ".xls");
                //Response.Charset = "UTF-8";
                //Response.ContentEncoding = Encoding.Default;
                //this.EnableViewState = false;

                //StringWriter stringWrite = new StringWriter();
                //HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
                //pnlReporte.RenderControl(htmlWrite);
                //Response.Write(stringWrite.ToString());
                //Response.End();
                DataSet ds;
                string[] speriodo = txtPeriodo.Text.S().Split('/');

                if (speriodo.Length == 1)
                    speriodo = txtPeriodo.Text.S().Split('-');

                iMes = speriodo[1].S().I();
                iAnio = speriodo[0].S().I();

                if (eSearchEdoCuenta != null)
                    eSearchEdoCuenta(sender, e);
                
                ds = dsEdoCuenta;
                ds.Tables[0].TableName = "MXP";
                ds.Tables[1].TableName = "USD";

                lblNombreCliente.Text = sNombrecliente;
                lblMatricula.Text = sMatricula;
                lblContrato.Text = sClaveContrato;
                lblElaboro.Text = Utils.GetUserName;
                lblFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");

                
                string strPath = string.Empty;
                ReportDocument rd = new ReportDocument();
                strPath = Server.MapPath("RPT\\rptEstadoCuenta.rpt");
                strPath = strPath.Replace("\\Views\\Reportes", "");
                rd.Load(strPath, OpenReportMethod.OpenReportByDefault);
                

                //rd.SetParameterValue("Matricula", "XLT");
                rd.SetDataSource(ds.Tables[0]);


                rd.Subreports["rptSubRepEdoCuenta_USD.rpt"].SetDataSource(ds.Tables[1]);

                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "EstadoCuenta");
            }
            catch (Exception ex)
            {
                string strError = ex.S();
            }
        }

        protected void btnAceptarPeriodo_Click(object sender, EventArgs e)
        {
            try
            {
                string[] speriodo = txtPeriodo.Text.S().Split('/');

                if (speriodo.Length == 1)
                    speriodo = txtPeriodo.Text.S().Split('-');

                iMes = speriodo[1].S().I();
                iAnio = speriodo[0].S().I();

                if (eObjSelected != null)
                    eObjSelected(sender, e);


                lblNombreCliente.Text = sNombrecliente;
                lblMatricula.Text = sMatricula;
                lblContrato.Text = sClaveContrato;
                lblElaboro.Text = Utils.GetUserName;
                lblFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");


                if (eSearchTotales != null)
                    eSearchTotales(sender, e);

                if (dtTotal != null && dtTotal.Rows.Count > 0)
                {
                    lblRespSaldoAntMXP.Text = dtTotal.Rows[0]["SaldoAnterior"].S().D().ToString("c");
                    lblRespPagCreditMXP.Text = dtTotal.Rows[0]["PagosCreditos"].S().D().ToString("c");
                    lblRespNuevosCargosMXP.Text = dtTotal.Rows[0]["NuevosCargos"].S().D().ToString("c");
                    lblRespSaldoActMXP.Text = dtTotal.Rows[0]["SaldoActual"].S().D().ToString("c");

                    lblRespSaldoAntUSD.Text = dtTotal.Rows[1]["SaldoAnterior"].S().D().ToString("c");
                    lblRespPagCreditUSD.Text = dtTotal.Rows[1]["PagosCreditos"].S().D().ToString("c");
                    lblRespNuevosCargosUSD.Text = dtTotal.Rows[1]["NuevosCargos"].S().D().ToString("c");
                    lblRespSaldoActUSD.Text = dtTotal.Rows[1]["SaldoActual"].S().D().ToString("c");
                }

                pnlReporte.Visible = true;
                btnGenerar.Visible = true;

                mpePeriodo.Hide();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region METODOS
        public void LLenaClientes(DataTable dt)
        {
            try
            {
                gvClientes.DataSource = dt;
                gvClientes.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string ObtieneNombreMes(int iMes)
        {
            string sMes = string.Empty;
            switch (iMes)
            {
                case 1:
                    sMes = "ENERO";
                    break;
                case 2:
                    sMes = "FEBRERO";
                    break;
                case 3:
                    sMes = "MARZO";
                    break;
                case 4:
                    sMes = "ABRIL";
                    break;
                case 5:
                    sMes = "MAYO";
                    break;
                case 6:
                    sMes = "JUNIO";
                    break;
                case 7:
                    sMes = "JULIO";
                    break;
                case 8:
                    sMes = "AGOSTO";
                    break;
                case 9:
                    sMes = "SEPTIEMBRE";
                    break;
                case 10:
                    sMes = "OCTUBRE";
                    break;
                case 11:
                    sMes = "NOVIEMBRE";
                    break;
                case 12:
                    sMes = "DICIEMBRE";
                    break;
            }

            return sMes;
        }
        public void LlenaReporte(string sHTML)
        {
            try
            {
                pnlGastos.InnerHtml = sHTML;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region VARIABLES Y PROPIEDADES
        EstadoCuenta_Presenter oPresenter;
        public event EventHandler eNewObj;
        public event EventHandler eObjSelected;
        public event EventHandler eSaveObj;
        public event EventHandler eDeleteObj;
        public event EventHandler eSearchObj;
        public event EventHandler eSearchTotales;
        public event EventHandler eSearchEdoCuenta;

        public DataSet dsGastos
        {
            get { return (DataSet)ViewState["VdtGastos"]; }
            set { ViewState["VdtGastos"] = value; }
        }
        public object[] oArrFiltros
        {
            get
            {
                string sEstatus = string.Empty;
                string sClaveContrato = string.Empty;
                string sClaveCliente = string.Empty;
                string sMatricula = string.Empty;

                switch (ddlOpcBus.SelectedValue.S())
                {
                    case "0":
                    case "1":
                        sEstatus = ddlOpcBus.SelectedValue.S();
                        break;
                    case "2":
                        sEstatus = "2";
                        sClaveCliente = txtBusqueda.Text.S();
                        break;
                    case "3":
                        sEstatus = "2";
                        sClaveContrato = txtBusqueda.Text.S();
                        break;
                    case "4":
                        sEstatus = "2";
                        sMatricula = txtBusqueda.Text.S();
                        break;
                }

                return new object[]
                {

                    "@ClaveCliente", sClaveCliente,
                    "@ClaveContrato", sClaveContrato,
                    "@Estatus", sEstatus,
                    "@Matricula", sMatricula
                };
            }
        }

        public DataSet dsEdoCuenta
        {
            get { return (DataSet)ViewState["VEdoCuenta"]; }
            set { ViewState["VEdoCuenta"] = value; }
        }
        public DataTable dtClientes
        {
            get { return (DataTable)ViewState["VClientes"]; }
            set { ViewState["VClientes"] = value; }
        }
        public string sMatricula
        {
            get { return (string)ViewState["VMatricula"]; }
            set { ViewState["VMatricula"] = value; }
        }
        public string sClaveContrato
        {
            get { return (string)ViewState["VContrato"]; }
            set { ViewState["VContrato"] = value; }
        }
        public int iMes
        {
            get { return (int)ViewState["ViMes"]; }
            set { ViewState["ViMes"] = value; }
        }
        public int iAnio
        {
            get { return (int)ViewState["ViAnio"]; }
            set { ViewState["ViAnio"] = value; }
        }
        public DataTable dtTotal
        {
            get { return (DataTable)ViewState["VSdtTotal"]; }
            set { ViewState["VSdtTotal"] = value; }
        }
        public string sNombrecliente
        {
            get { return (string)ViewState["VSNombreCliente"]; }
            set { ViewState["VSNombreCliente"] = value; }
        }
        #endregion

        
    }
}