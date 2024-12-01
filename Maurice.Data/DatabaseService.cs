using MySql.Data.MySqlClient;

namespace Maurice.Data.Services
{
    public class DatabaseService
    {
        private const string ConnectionString = "Server=localhost;Database=maurice;Uid=root;Pwd=yourpassword;";

        public bool SaveFactura(IDictionary<string, string> facturaData, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                using var connection = new MySqlConnection(ConnectionString);
                connection.Open();

                string query = @"
                    INSERT INTO facturas 
                    (file_name,folio, fecha, rfc_emisor, nombre_emisor, rfc_receptor, total_impuesto, base, tasa, importe) 
                    VALUES 
                    (@FileName, @Folio, @Fecha, @RfcEmisor, @NombreEmisor, @RfcReceptor, @TotalImpuesto, @Base, @Tasa, @Importe);";

                using var command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@FileName", facturaData.ContainsKey("FileName") ? facturaData["FileName"] : null);
                command.Parameters.AddWithValue("@Folio", facturaData.ContainsKey("Folio") ? facturaData["Folio"] : null);
                command.Parameters.AddWithValue("@Fecha", facturaData.ContainsKey("Fecha") ? facturaData["Fecha"] : null);
                command.Parameters.AddWithValue("@RfcEmisor", facturaData.ContainsKey("RFC Emisor") ? facturaData["RFC Emisor"] : null);
                command.Parameters.AddWithValue("@NombreEmisor", facturaData.ContainsKey("Nombre de Emisor") ? facturaData["Nombre de Emisor"] : null);
                command.Parameters.AddWithValue("@RfcReceptor", facturaData.ContainsKey("RFC Receptor") ? facturaData["RFC Receptor"] : null);
                command.Parameters.AddWithValue("@TotalImpuesto", facturaData.ContainsKey("Total Impuesto") ? facturaData["Total Impuesto"] : null);
                command.Parameters.AddWithValue("@Base", facturaData.ContainsKey("Base") ? facturaData["Base"] : null);
                command.Parameters.AddWithValue("@Tasa", facturaData.ContainsKey("Tasa") ? facturaData["Tasa"] : null);
                command.Parameters.AddWithValue("@Importe", facturaData.ContainsKey("Importe") ? facturaData["Importe"] : null);

                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                // Capture the error message
                errorMessage = $"An error occurred while saving to the database: {ex.Message}";
                return false;
            }
        }
    }
}