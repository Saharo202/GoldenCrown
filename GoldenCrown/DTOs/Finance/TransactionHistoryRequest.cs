using System.ComponentModel.DataAnnotations;

namespace GoldenCrown.DTOs.Finance
{
    public class TransactionHistoryRequest
    {
        [Required(ErrorMessage = "Поле token обязательно")]
        public string Token {  get; set; }
        
        public DateTime? From { get; set; }

        public DateTime? To {  get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Значение limit должно быть не менее 1")]
        public int Limit { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Значение offset не может быть отрицательным")]
        public int Offset { get; set; }
    }
}
