﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using System.IO;
using SW.Services.Authentication;
using SW.Services.Stamp;
using SW.Services.Cancelation;
using SW.Tools.Entities.Complementos;
using SW.Tools.Entities;
using SW.Tools.Services.Sign;
using SW.Tools.Services.Fiscal;
using SW.Tools.Cfdi;
using SW.Tools.Cfdi.Complementos.Pagos20;
using SW.Tools.Helpers;

namespace SW.ToolsUT
{
    [TestClass]
    public class UT_Tools_BuildInvoiceCFDI40
    {
        private string userStamp;
        private string passwordStamp;
        private string url;
        public UT_Tools_BuildInvoiceCFDI40()
        {
            userStamp = "ut@test.com";
            passwordStamp = "12345678a";
            url = "http://services.test.sw.com.mx";
        }

        [TestMethod]
        public void UT_StampInvoice()
        {
            Comprobante comprobante = new Comprobante();
            
            comprobante.SetComprobante("MXN", "I", "99", "PPD", "20000", "01");
            comprobante.SetConcepto(1, "84131500", "ZZ", "Prima neta", "1", "NO APLICA", 3592.83m,"02", 3592.83m);
            comprobante.SetConceptoImpuestoTraslado(0.160000m, "Tasa", "002", 3592.83m, 574.85m);
            comprobante.SetConcepto(1, "84131500", "ZZ", "Recargo por pago fraccionado", "1", "NO APLICA", 258.68m, "02", 258.68m);
            comprobante.SetConceptoImpuestoTraslado(0.160000m, "Tasa", "002", 258.68m, 41.38m);
            comprobante.SetConcepto(1, "84131500", "ZZ", "derecho de poliza", "1", "NO APLICA", 550.00m, "02", 550.00m);
            comprobante.SetConceptoImpuestoTraslado(0.160000m, "Tasa", "002", 550.00m, 88.00m);
            comprobante.SetEmisor("EKU9003173C9", "ESCUELA KEMPER URGATE", "601");
            comprobante.SetReceptor("URE180429TM6", "UNIVERSIDAD ROBOTICA ESPAÑOLA", "G01", "65000", "601");
            var invoice = comprobante.GetComprobante();
            var xmlInvoice = Serializer.SerializeDocument(invoice);
            xmlInvoice = SignInvoice(xmlInvoice);
            Stamp stamp = new Stamp(this.url, this.userStamp, this.passwordStamp);
            StampResponseV2 response = stamp.TimbrarV2(xmlInvoice);
            Assert.IsTrue(response.status == "success");
        }

        [TestMethod]
        public void UT_CFDI40_InformacionGlobal()
        {
            Comprobante comprobante = new Comprobante();

            comprobante.SetComprobante("MXN", "I", "99", "PPD", "20000", "01");
            comprobante.SetConcepto(1, "84131500", "ZZ", "derecho de poliza", "1", "NO APLICA", 550.00m, "02", 550.00m);
            comprobante.SetConceptoImpuestoTraslado(0.160000m, "Tasa", "002", 550.00m, 88.00m);
            comprobante.SetEmisor("EKU9003173C9", "ESCUELA KEMPER URGATE", "601");
            comprobante.SetReceptor("URE180429TM6", "UNIVERSIDAD ROBOTICA ESPAÑOLA", "G01", "65000", "601");
            comprobante.SetInformacionGlobal("01", "04", "2022");
            var invoice = comprobante.GetComprobante();
            var xmlInvoice = Serializer.SerializeDocument(invoice);
            xmlInvoice = SignInvoice(xmlInvoice);
            Stamp stamp = new Stamp(this.url, this.userStamp, this.passwordStamp);
            StampResponseV2 response = stamp.TimbrarV2(xmlInvoice);
            Assert.IsTrue(response.status == "success");
        }
        [TestMethod]
      
        public void UT_CFDI40_AcuentaTerceros()
        {
            Comprobante comprobante = new Comprobante();

            comprobante.SetComprobante("MXN", "I", "99", "PPD", "20000", "01");
            comprobante.SetConcepto(1, "84131500", "ZZ", "derecho de poliza", "1", "NO APLICA", 550.00m, "02", 550.00m);
            comprobante.SetConceptoImpuestoTraslado(0.160000m, "Tasa", "002", 550.00m, 88.00m);
            comprobante.SetEmisor("EKU9003173C9", "ESCUELA KEMPER URGATE", "601");
            comprobante.SetAcuentaTerceros("JUFA7608212V6", "ADRIANA JUAREZ FERNANDEZ", "601", "29133");
            comprobante.SetReceptor("URE180429TM6", "UNIVERSIDAD ROBOTICA ESPAÑOLA", "G01", "65000", "601");
            var invoice = comprobante.GetComprobante();
            var xmlInvoice = Serializer.SerializeDocument(invoice);
            xmlInvoice = SignInvoice(xmlInvoice);
            Stamp stamp = new Stamp(this.url, this.userStamp, this.passwordStamp);
            StampResponseV2 response = stamp.TimbrarV2(xmlInvoice);
            Assert.IsTrue(response.status == "success");
        }
        [TestMethod]
        public void UT_CFDI40_CFDIRelacionados()
        {
            Comprobante comprobante = new Comprobante();

            comprobante.SetComprobante("MXN", "I", "99", "PPD", "20000", "01");
            comprobante.SetConcepto(1, "84131500", "ZZ", "Prima neta", "1", "NO APLICA", 3592.83m, "02", 3592.83m);
            comprobante.SetConceptoImpuestoTraslado(0.160000m, "Tasa", "002", 3592.83m, 574.85m);
            comprobante.SetConcepto(1, "84131500", "ZZ", "Recargo por pago fraccionado", "1", "NO APLICA", 258.68m, "02", 258.68m);
            comprobante.SetConceptoImpuestoTraslado(0.160000m, "Tasa", "002", 258.68m, 41.38m);
            comprobante.SetConcepto(1, "84131500", "ZZ", "derecho de poliza", "1", "NO APLICA", 550.00m, "02", 550.00m);
            comprobante.SetConceptoImpuestoTraslado(0.160000m, "Tasa", "002", 550.00m, 88.00m);
            comprobante.SetEmisor("EKU9003173C9", "ESCUELA KEMPER URGATE", "601");
            string[] lista = new string[2];
            lista[0] = "0aded095-b84d-4364-8f8e-59c3f650e530";
            lista[1] = "2da2a676-f424-4898-a190-79253fdf5f7a";
            comprobante.SetCFDIRelacionado("03", lista);
            comprobante.SetReceptor("URE180429TM6", "UNIVERSIDAD ROBOTICA ESPAÑOLA", "G01", "65000", "601");
            var invoice = comprobante.GetComprobante();
            var xmlInvoice = Serializer.SerializeDocument(invoice);
            xmlInvoice = SignInvoice(xmlInvoice);
            Stamp stamp = new Stamp(this.url, this.userStamp, this.passwordStamp);
            StampResponseV2 response = stamp.TimbrarV2(xmlInvoice);
            Assert.IsTrue(response.status == "success");
        }
        private string SignInvoice(string xmlInvoice)
        {
            byte[] bytesCer = File.ReadAllBytes(@"Resources\CSD_Pruebas_CFDI_EKU9003173C9.cer");
            byte[] bytesKey = File.ReadAllBytes(@"Resources\CSD_Pruebas_CFDI_EKU9003173C9.key");
            string password = "12345678a";
            var pfx = Sign.CrearPFX(bytesCer, bytesKey, password);
            var xmlResult = Sign.SellarCFDIv40(pfx, password, xmlInvoice);
            return xmlResult;
        }
        [TestMethod]
        public void DeserailizeXml()
        {
            var xml = Fiscal.RemoverCaracteresInvalidosXml(Encoding.UTF8.GetString(File.ReadAllBytes(@"Resources\cfdi33Invoice.xml")));
            var xmlInvoicess = Tools.Helpers.Serializer.DeserealizeDocument<Comprobante>(xml);
        }
    }
}
