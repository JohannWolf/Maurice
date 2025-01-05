using Maurice.Data.DBModels;

namespace Maurice.Data.Services
{
    public interface IDatabaseService
    {
        void InitializeDatabase();
        bool SaveUserData(string rfc, string nombre, string codigoPostal, List<RegimenFiscal> regimenFiscal, out string errorMessage);
        bool SaveFactura(IDictionary<string, string> facturaData, string fileName, out string errorMessage);
        bool CheckForDuplicates(string uuid);
    }
}