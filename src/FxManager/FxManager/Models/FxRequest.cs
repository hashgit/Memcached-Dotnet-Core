using System.ComponentModel.DataAnnotations;

namespace FxManager.Models
{
    public class FxRequest
    {
        [Required]
        public string BaseCurrency { get; set; }

        [Required]
        public string TargetCurrency { get; set; }
    }
}