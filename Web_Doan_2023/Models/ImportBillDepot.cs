using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Web_Doan_2023.Models
{
    public class ImportBillDepot// hóa đơn nhập kho
    {
        public int Id { get; set; }
        [StringLength(10)]
        public string? codeBill { get; set; }
        public decimal? Price { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UserCreate { get; set; }
        public string? UserUpdate { get; set; }
        public string? Status { get; set; }// trangj thais
        public bool IsAcceptance { get; set; }// chấp thuận
    }
}
