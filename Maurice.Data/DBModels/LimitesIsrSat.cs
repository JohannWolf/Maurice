using System;
using System.ComponentModel.DataAnnotations;

namespace Maurice.Data.DBModels
{
    public class LimitesIsrSat
    {
        public int Id { get; set; }
        [Required]
        public decimal LimiteInferior { get; set; }
        [Required]
        public decimal LimiteSuperior { get; set; }
        [Required]
        public decimal CuotaFija { get; set; }
        [Required]
        public decimal PorcentajeSobreLimiteInferior { get; set; }
        [Required]
        public decimal? Periodo { get; set; }
        [Required]
        public RegimenFiscal Regimen { get; set; }
    }
}
