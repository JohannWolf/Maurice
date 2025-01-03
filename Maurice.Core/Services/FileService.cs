using System.Xml.Linq;

namespace Maurice.Core.Services
{
    public class FileService:IFileService
    {
        public IDictionary<string, string> ParseXml(string filePath)
        {
            var result = new Dictionary<string, string>();

            // Load the XML file
            var doc = XDocument.Load(filePath);

            // Extract data
            var comprobante = doc.Root;
            if (comprobante != null)
            {
                result["Folio"] = comprobante.Attribute("Folio")?.Value;
                result["Fecha"] = comprobante.Attribute("Fecha")?.Value;

                var complemento = comprobante.Element(XName.Get("Complemento", "http://www.sat.gob.mx/cfd/4")); 
                if (complemento != null)
                {
                    var timbreFiscalDigital = complemento.Element(XName.Get("TimbreFiscalDigital", "http://www.sat.gob.mx/TimbreFiscalDigital")); if (timbreFiscalDigital != null)
                    result["UUID"] = timbreFiscalDigital.Attribute("UUID")?.Value ?? "NA"; // Default value for UUID
                } 

                var emisor = comprobante.Element(XName.Get("Emisor", "http://www.sat.gob.mx/cfd/4"));
                if (emisor != null)
                {
                    result["RFC Emisor"] = emisor.Attribute("Rfc")?.Value;
                    result["Nombre de Emisor"] = emisor.Attribute("Nombre")?.Value;
                }

                var receptor = comprobante.Element(XName.Get("Receptor", "http://www.sat.gob.mx/cfd/4"));
                if (receptor != null)
                {
                    result["RFC Receptor"] = receptor.Attribute("Rfc")?.Value;
                }

                var concepto = comprobante.Descendants(XName.Get("Concepto", "http://www.sat.gob.mx/cfd/4")).FirstOrDefault();
                if (concepto != null)
                {
                    var value = concepto.Attribute("ClaveProdServ")?.Value;

                    if(value == "85121600")
                    {
                        result["Descripcion"] = "Honorarios medicos y gastos hospitalarios";
                    }
                }

                var impuestos = comprobante.Element(XName.Get("Impuestos", "http://www.sat.gob.mx/cfd/4"));
                if (impuestos != null)
                {
                    var traslado = impuestos.Descendants(XName.Get("Traslado", "http://www.sat.gob.mx/cfd/4")).FirstOrDefault();
                    if (traslado != null)
                    {
                        result["Base"] = traslado.Attribute("Base")?.Value ?? "0.00"; // Default value for Base
                        result["Tasa"] = traslado.Attribute("TasaOCuota")?.Value ?? "excento";
                        result["Importe"] = traslado.Attribute("Importe")?.Value ?? "0.00"; // Default value for Importe
                    }
                }
            }

            return result;
        }
    }
}