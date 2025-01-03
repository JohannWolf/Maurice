
using System.ComponentModel.DataAnnotations;

namespace Maurice.Data.DBModels
{
    public class User
    {
        public int Id {  get; set; }
        [Required]
        public string Rfc {  get; set; } = string.Empty;
        [Required]
        public string Nombre { get; set; } = string.Empty;
        [Required]
        public string CodigoPostal {  get; set; } = string.Empty;
        [Required]
        public List<RegimenFiscal> RegimenFiscal {  get; set; }
    }
}
