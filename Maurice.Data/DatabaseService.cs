using Google.Protobuf.WellKnownTypes;
using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;
using Maurice.Core.Models;

namespace Maurice.Data.Services
{
    public class DatabaseService
    {
        private const string DatabaseFileName = "maurice.db";

        private string GetConnectionString()
        {
            var dbPath = Path.Combine(AppContext.BaseDirectory, DatabaseFileName);
            return $"Data Source={dbPath}";
        }

        public void InitializeDatabase()
        {
            using var connection = new SqliteConnection(GetConnectionString());
            connection.Open();

            // Create the `facturas` table if it doesn't exist
            string createFacturasTableQuery = @"
                CREATE TABLE IF NOT EXISTS facturas (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    folio TEXT,
                    fecha TEXT,
                    rfc_emisor TEXT,
                    nombre_emisor TEXT,
                    rfc_receptor TEXT,
                    total_impuesto REAL,
                    base REAL,
                    tasa TEXT,
                    importe REAL,
                    filename TEXT,
                    entry_date TEXT
                );";

            // Create the `userData` table if it doesn't exist
            string createUserDataTableQuery = @"
                CREATE TABLE IF NOT EXISTS userData (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    rfc TEXT,
                    nombre TEXT,
                    codigo_postal TEXT,
                    regimen_fiscal TEXT
                );";

            // Create the `regimenFiscal` table if it doesn't exist
            string createRegimenFiscalTableQuery = @"
                CREATE TABLE IF NOT EXISTS regimenFiscal (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    code TEXT NOT NULL,
                    description TEXT NOT NULL
                );";

            using var command = new SqliteCommand(createFacturasTableQuery, connection);
            command.ExecuteNonQuery();

            command.CommandText = createUserDataTableQuery;
            command.ExecuteNonQuery();

            command.CommandText = createRegimenFiscalTableQuery;
            command.ExecuteNonQuery();
        }

        public bool SaveUserData(string rfc, string nombre, string codigoPostal, string regimenFiscal, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                using var connection = new SqliteConnection(GetConnectionString());
                connection.Open();

                string query = @"
                    INSERT INTO userData (rfc, nombre, codigo_postal, regimen_fiscal) 
                    VALUES (@Rfc, @Nombre, @CodigoPostal, @RegimenFiscal);";

                using var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Rfc", rfc);
                command.Parameters.AddWithValue("@Nombre", nombre);
                command.Parameters.AddWithValue("@CodigoPostal", codigoPostal);
                command.Parameters.AddWithValue("@RegimenFiscal", regimenFiscal);

                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = $"Error saving user data: {ex.Message}";
                return false;
            }
        }

        public bool SaveFactura(IDictionary<string, string> facturaData, string fileName, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                using var connection = new SqliteConnection(GetConnectionString());
                connection.Open();

                string insertQuery = @"
                    INSERT INTO facturas 
                    (folio, fecha, rfc_emisor, nombre_emisor, rfc_receptor, total_impuesto, base, tasa, importe, filename, entry_date) 
                    VALUES 
                    (@Folio, @Fecha, @RfcEmisor, @NombreEmisor, @RfcReceptor, @TotalImpuesto, @Base, @Tasa, @Importe, @FileName, @EntryDate);";

                using var command = new SqliteCommand(insertQuery, connection);

                command.Parameters.AddWithValue("@Folio", facturaData.ContainsKey("Folio") ? facturaData["Folio"] : DBNull.Value);
                command.Parameters.AddWithValue("@Fecha", facturaData.ContainsKey("Fecha") ? facturaData["Fecha"] : DBNull.Value);
                command.Parameters.AddWithValue("@RfcEmisor", facturaData.ContainsKey("RFC Emisor") ? facturaData["RFC Emisor"] : DBNull.Value);
                command.Parameters.AddWithValue("@NombreEmisor", facturaData.ContainsKey("Nombre de Emisor") ? facturaData["Nombre de Emisor"] : DBNull.Value);
                command.Parameters.AddWithValue("@RfcReceptor", facturaData.ContainsKey("RFC Receptor") ? facturaData["RFC Receptor"] : DBNull.Value);
                command.Parameters.AddWithValue("@TotalImpuesto", facturaData.ContainsKey("Total Impuesto") ? facturaData["Total Impuesto"] : DBNull.Value);
                command.Parameters.AddWithValue("@Base", facturaData.ContainsKey("Base") ? facturaData["Base"] : DBNull.Value);
                command.Parameters.AddWithValue("@Tasa", facturaData.ContainsKey("Tasa") ? facturaData["Tasa"] : DBNull.Value);
                command.Parameters.AddWithValue("@Importe", facturaData.ContainsKey("Importe") ? facturaData["Importe"] : DBNull.Value);
                command.Parameters.AddWithValue("@FileName", fileName);
                command.Parameters.AddWithValue("@EntryDate", DateTime.Now);

                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = $"Error saving factura: {ex.Message}";
                return false;
            }
        }

        public ObservableCollection<RegimenFiscalOption> GetRegimenFiscalOptions()
        {
            var options = new ObservableCollection<RegimenFiscalOption>();

            foreach (var (code, description) in new[] { ("601", "General de Ley Personas Morales"), //Add your options here
                                             ("603", "Personas Morales con Fines no Lucrativos"),
                                             ("605", "Sueldos y Salarios e Ingresos Asimilados a Salarios"),
                                             ("606", "Arrendamiento"),
                                             ("607", "Régimen de Enajenación o Adquisición de Bienes"),
                                             ("616", "Sin obligaciones fiscales"),
                                             ("625", "Régimen de las Actividades Empresariales con ingresos a través de Plataformas Tecnológicas"),
                                             ("626", "Régimen Simplificado de Confianza") })
            {
                options.Add(new RegimenFiscalOption { Code = code, Descripcion = description });
            }
            return options;
        }
        /*try
        {
            using var connection = new SqliteConnection(GetConnectionString());
            connection.Open();

            string query = "SELECT code, description FROM regimenFiscal";

            using var command = new SqliteCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var code = reader.GetString(0);
                var description = reader.GetString(1);
                options.Add((code, description));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving regimen fiscal options: {ex.Message}");
        }
            return options;
        }*/

        public bool SaveRegimenFiscal(string code, string description, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                using var connection = new SqliteConnection(GetConnectionString());
                connection.Open();

                string query = @"
                    INSERT INTO regimenFiscal (code, description) 
                    VALUES (@Code, @Description);";

                using var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Code", code);
                command.Parameters.AddWithValue("@Description", description);

                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = $"Error saving regimen fiscal: {ex.Message}";
                return false;
            }
        }
    }
}