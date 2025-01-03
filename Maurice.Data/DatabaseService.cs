using Maurice.Data.Context;
using Maurice.Data.DBModels;
using System;

namespace Maurice.Data.Services
{
    public class DatabaseService
    {
        public void InitializeDatabase()
        {
            using var context = new MauriceDbContext();
            context.Database.EnsureCreated();
        }

        public bool SaveUserData(string rfc, string nombre, string codigoPostal, List<RegimenFiscal> regimenFiscal, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                using var context = new MauriceDbContext();
                var userData = new User
                {
                    Rfc = rfc,
                    Nombre = nombre,
                    CodigoPostal = codigoPostal,
                    RegimenFiscal = regimenFiscal
                };

                context.User.Add(userData);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = $"Error al guardar datos: {ex.Message}";
                return false;
            }
        }

        public bool SaveFactura(IDictionary<string, string> facturaData, string fileName, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                using var context = new MauriceDbContext();

                var uuid = facturaData.TryGetValue("UUID", out string found)? found : null;


                // Check for duplicate UUID
                bool isDuplicate = CheckForDuplicates(uuid);

                if (!isDuplicate) {
                var factura = new Factura
                {
                    Uuid = uuid,
                    Folio = facturaData.TryGetValue("Folio", out string folio) ? folio : null,
                    Fecha = (DateTime)(facturaData.TryGetValue("Fecha", out var fechaStr) && DateTime.TryParse(fechaStr, out var fecha) ? fecha : (DateTime?)null),
                    RfcEmisor = facturaData.TryGetValue("RFC Emisor", out var rfcEmisor) ? rfcEmisor : null,
                    NombreEmisor = facturaData.TryGetValue("Nombre de Emisor", out var nombreEmisor) ? nombreEmisor : null,
                    RfcReceptor = facturaData.TryGetValue("RFC Receptor", out var rfcReceptor) ? rfcReceptor : null,
                    TotalImpuesto = decimal.TryParse(facturaData.TryGetValue("Total Impuesto", out var totalImpuestoStr) ? totalImpuestoStr : null, out decimal totalImpuesto) ? totalImpuesto : 0,
                    Base = decimal.TryParse(facturaData.TryGetValue("Base", out var baseStr) ? baseStr : null, out decimal baseValue) ? baseValue : 0,
                    Tasa = decimal.TryParse(facturaData.TryGetValue("Tasa", out var tasaStr) ? tasaStr : null, out decimal tasa) ? tasa : 0,
                    Importe = decimal.TryParse(facturaData.TryGetValue("Importe", out var importeStr) ? importeStr : null, out var importe) ? importe : 0,
                    FileName = fileName,
                    EntryDate = DateTime.Now
                };

                context.Facturas.Add(factura);
                context.SaveChanges();
                return true;
                }
                else
                {
                    errorMessage = "Error, la factura ya esta en el sistema.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Error al guardar factura: {ex.Message}";
                return false;
            }
        }

        public List<RegimenFiscal> GetRegimenFiscalOptions()
        {
            using var context = new MauriceDbContext();
            return context.RegimenFiscal.ToList();
        }

        public bool SaveRegimenFiscal(int code, string description, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                using var context = new MauriceDbContext();
                var regimen = new RegimenFiscal
                {
                    Codigo = code,
                    Descripcion = description
                };

                context.RegimenFiscal.Add(regimen);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = $"Error saving regimen fiscal: {ex.Message}";
                return false;
            }
        }

        public bool CheckForDuplicates(string id)
        {
            using var context = new MauriceDbContext();
            var fct = context.Facturas
                        .SingleOrDefault(f => f.Uuid == id);

            return fct != null;
        }
    }
}