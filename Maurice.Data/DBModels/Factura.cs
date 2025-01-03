using System.ComponentModel.DataAnnotations;

namespace Maurice.Data.DBModels
{
    public class Factura
    {
        public int Id { get; set; }
        [Required]
        public string Uuid { get; set; }
        [Required]
        public string Folio { get; set; } = string.Empty;
        [Required]
        public DateTime Fecha {  get; set; } 
        [Required]
        public string RfcEmisor {  get; set; } = string.Empty;
        [Required]
        public string NombreEmisor { get; set; } = string.Empty;
        [Required]
        public string RfcReceptor {  get; set; } = string.Empty;
        [Required]
        public decimal TotalImpuesto {  get; set; }
        [Required]
        public decimal Base { get; set; }
        [Required]
        public decimal Tasa {  get; set; }
        [Required]
        public decimal Importe {  get; set; }
        [Required]
        public string FileName {  get; set; } = string.Empty;
        [Required]
        public DateTime EntryDate {  get; set; } 
    }
}
