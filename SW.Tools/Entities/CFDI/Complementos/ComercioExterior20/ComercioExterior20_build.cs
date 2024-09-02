using SW.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SW.Tools.Cfdi.Complementos.ComercioExterior20
{
    public partial class ComercioExterior
    {
        private Comprobante invoice;

        public ComercioExterior(Comprobante comprobanteInvoice)
        {
            this.versionField = "2.0";
            this.invoice = comprobanteInvoice;
        }

        public Comprobante GetInvoice()
        {
            if (invoice == null)
                throw new Exception("Se debe proporcionar un Comprobante para contener el complemento de Comercio Exterior");
            if (invoice.Exportacion == "01" || invoice.Exportacion == "03")
                return invoice;
            invoice.InformacionGlobal = null;
            decimal totalUsd = 0;
            var mercanciasList = this.Mercancias?.ToList();
            mercanciasList.ForEach(mercancia => {
                totalUsd += mercancia.ValorDolares;
            });
            this.TotalUSD = Math.Round(totalUsd, 2);
            var xmlComercioExterior = SerializerCE.SerializeDocument(this);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlComercioExterior);
            invoice.SetComplemento(doc.DocumentElement);
            return invoice;
        }

        public void SetComprobante(Comprobante comprobanteInvoice)
        {
            this.invoice = comprobanteInvoice;
        }

        public void SetComercioExterior(string claveDePedimento, int certificadoOrigen, decimal tipoCambioUSD,
        string motivoTraslado, string numCertificadoOrigen, string numeroExportadorConfiable,
        string incoterm, string observaciones)
        {
            this.ClaveDePedimento = claveDePedimento;
            this.CertificadoOrigen = certificadoOrigen;
            this.TipoCambioUSD = tipoCambioUSD;
            this.MotivoTraslado = motivoTraslado;
            this.MotivoTrasladoSpecified = !String.IsNullOrEmpty(motivoTraslado);
            this.NumCertificadoOrigen = numCertificadoOrigen;
            this.NumeroExportadorConfiable = numeroExportadorConfiable;
            this.Incoterm = incoterm;
            this.IncotermSpecified = !String.IsNullOrEmpty(incoterm);
            this.Observaciones = observaciones;
        }


        public void SetEmisor(string curp, string calle, string numeroExterior, string numeroInterior, string colonia,
            string localidad, string referencia, string municipio, string estado, string pais, string codigoPostal)
        {
            if (this.Emisor == null)
                this.Emisor = new ComercioExteriorEmisor();
            Emisor.Curp = curp;
            Emisor.Domicilio = new ComercioExteriorEmisorDomicilio()
            {
                Calle = calle,
                NumeroExterior = numeroExterior,
                NumeroInterior = numeroInterior,
                Colonia = colonia,
                ColoniaSpecified = !String.IsNullOrEmpty(colonia),
                Localidad = localidad,
                LocalidadSpecified = !String.IsNullOrEmpty(localidad),
                Referencia = referencia,
                Municipio = municipio,
                MunicipioSpecified = !String.IsNullOrEmpty(municipio),
                Estado = estado,
                Pais = pais,
                CodigoPostal = codigoPostal
            };
        }

        public void SetPropietario(string numRegIdTrib, string residenciaFiscal)
        {
            if (this.Propietario == null)
                this.Propietario = new ComercioExteriorPropietario[0];
            List<ComercioExteriorPropietario> propietariosList = Propietario.ToList();
            ComercioExteriorPropietario propietario = new ComercioExteriorPropietario
            {
                NumRegIdTrib = numRegIdTrib,
                ResidenciaFiscal = residenciaFiscal
            };
            propietariosList.Add(propietario);
            this.Propietario = propietariosList.ToArray();
        }

        public void SetReceptor(string numRegIdTrib, string calle, string numeroExterior, string numeroInterior, string colonia,
            string localidad, string referencia, string municipio, string estado, string pais, string codigoPostal)
        {
            if (this.Receptor == null)
                this.Receptor = new ComercioExteriorReceptor();
            Receptor.NumRegIdTrib = numRegIdTrib;
            Receptor.Domicilio = new ComercioExteriorReceptorDomicilio()
            {
                Calle = calle,
                NumeroExterior = numeroExterior,
                NumeroInterior = numeroInterior,
                Colonia = colonia,
                Localidad = localidad,
                Referencia = referencia,
                Municipio = municipio,
                Estado = estado,
                Pais = pais,
                CodigoPostal = codigoPostal
            };
        }

        public void SetDestinatario(string numRegIdTrib, string nombre)
        {
            if (this.Destinatario == null)
                this.Destinatario = new ComercioExteriorDestinatario[0];
            List<ComercioExteriorDestinatario> destinatariosList = Destinatario.ToList();

            ComercioExteriorDestinatario destinatario = new ComercioExteriorDestinatario
            {
                NumRegIdTrib = numRegIdTrib,
                Nombre = nombre
            };
            destinatariosList.Add(destinatario);
            this.Destinatario = destinatariosList.ToArray();
        }

        public void SetDomicilioDestinatario(string calle, string numeroExterior, string numeroInterior, string colonia,
            string localidad, string referencia, string municipio, string estado, string pais, string codigoPostal)
        {
            ComercioExteriorDestinatario lastDestinatario = Destinatario.Last()
                ?? throw new NullReferenceException("No hay destinatarios para establecer domicilio");
            if (lastDestinatario.Domicilio == null)
                lastDestinatario.Domicilio = new ComercioExteriorDestinatarioDomicilio[0];
            List<ComercioExteriorDestinatarioDomicilio> domiciliosList = lastDestinatario.Domicilio.ToList();
            ComercioExteriorDestinatarioDomicilio domicilio = new ComercioExteriorDestinatarioDomicilio
            {
                Calle = calle,
                NumeroExterior = numeroExterior,
                NumeroInterior = numeroInterior,
                Colonia = colonia,
                Localidad = localidad,
                Referencia = referencia,
                Municipio = municipio,
                Estado = estado,
                Pais = pais,
                CodigoPostal = codigoPostal
            };
            domiciliosList.Add(domicilio);
            lastDestinatario.Domicilio = domiciliosList.ToArray();
        }

        public void SetMercancia(string noIdentificacion, decimal valorDolares, string fraccionArancelaria = null,
            decimal? cantidadAduana = 0, string unidadAduana = null, decimal? valorUnitarioAduana = 0)
        {
            if (this.Mercancias == null)
                this.Mercancias = new ComercioExteriorMercancia[0];
            List<ComercioExteriorMercancia> mercanciasList = Mercancias.ToList();
            ComercioExteriorMercancia mercancia = new ComercioExteriorMercancia
            {
                NoIdentificacion = noIdentificacion,
                CantidadAduana = cantidadAduana ?? 0,
                CantidadAduanaSpecified = cantidadAduana > 0,
                ValorUnitarioAduana = valorUnitarioAduana ?? 0,
                ValorUnitarioAduanaSpecified = valorUnitarioAduana > 0,
                ValorDolares = valorDolares,
                UnidadAduana = unidadAduana,
                UnidadAduanaSpecified = !String.IsNullOrEmpty(unidadAduana),
                FraccionArancelaria = fraccionArancelaria,
                FraccionArancelariaSpecified = !String.IsNullOrEmpty(fraccionArancelaria)
            };
            mercanciasList.Add(mercancia);
            this.Mercancias = mercanciasList.ToArray();
        }

        public void SetDescripcionEspecificaMercancia(string marca, string modelo, string subModelo, string numeroSerie)
        {
            ComercioExteriorMercancia lastMercancia = Mercancias.Last()
                ?? throw new NullReferenceException("No hay mercancias para establecer descripcion especifica");
            if (lastMercancia.DescripcionesEspecificas == null)
                lastMercancia.DescripcionesEspecificas = new ComercioExteriorMercanciaDescripcionesEspecificas[0];
            List<ComercioExteriorMercanciaDescripcionesEspecificas> descripcionesList = lastMercancia.DescripcionesEspecificas.ToList();
            ComercioExteriorMercanciaDescripcionesEspecificas descripcionEspecifica = new ComercioExteriorMercanciaDescripcionesEspecificas
            {
                Marca = marca,
                Modelo = modelo,
                SubModelo = subModelo,
                NumeroSerie = numeroSerie
            };
            descripcionesList.Add(descripcionEspecifica);
            lastMercancia.DescripcionesEspecificas = descripcionesList.ToArray();
        }
    }
}
