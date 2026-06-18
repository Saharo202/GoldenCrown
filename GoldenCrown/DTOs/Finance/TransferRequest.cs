using GoldenCrown.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GoldenCrown.DTOs.Finance
{
    public class TransferRequest
    {
        public string ReceiverLogin { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
    }
}
