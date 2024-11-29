using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Maurice.Core.Services
{
    public class FileService
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

                var impuestos = comprobante.Element(XName.Get("Impuestos", "http://www.sat.gob.mx/cfd/4"));
                if (impuestos != null)
                {
                    var traslado = impuestos.Descendants(XName.Get("Traslado", "http://www.sat.gob.mx/cfd/4")).FirstOrDefault();
                    if (traslado != null)
                    {
                        result["Base"] = traslado.Attribute("Base")?.Value;
                        result["Tasa"] = traslado.Attribute("TasaOCuota")?.Value;
                        result["Importe"] = traslado.Attribute("Importe")?.Value;
                    }
                }
            }

            return result;
        }
    }
}