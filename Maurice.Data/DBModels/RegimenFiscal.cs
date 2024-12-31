using System.ComponentModel.DataAnnotations;

namespace Maurice.Data.DBModels
{
    public class RegimenFiscal
    {
        public int Id { get; set; }
        [Required]
        public string Codigo { get; set; } = string.Empty;
        [Required]
        public string Descripcion { get; set; } = string.Empty ;
    }
}
