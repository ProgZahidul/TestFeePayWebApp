using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TestFeePayWebApp.Models
{
    public class FeeType
    {
        [Key]
        public int FeeTypeId { get; set; }
        public string TypeName { get; set; }

    }
    public class FeeStructure
    {
        [Key]
        public int FeeStructureId { get; set; }
        public int ClassId { get; set; }
        public bool Monthly { get; set; }
        public bool Yearly { get; set; }
        public int FeeTypeId { get; set; }
        public decimal FeeAmount { get; set; }

        [JsonIgnore]
        public FeePayment? FeePayment { get; set; }
        


        public virtual FeeType? FeeType { get; set; }
        public virtual Class? Class { get; set; }

    }

    public class FeePayment
    {
        [Key]
        public int FeePaymentId { get; set; }
        public int? StudentId { get; set; }
        public string? Name { get; set; }
        public decimal TotalFeeAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal AmountAfterDiscount { get; set; }
        public decimal PreviousDue { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal AmountRemaining { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public virtual List<FeePaymentDetail>? FeePaymentDetails { get; set; }
        public virtual Student? Student { get; set; }
        public IList<FeeStructure>? FeeStructure { get; set; }

    }

    public class DueBalance
    {
        [Key]
        public int BalanceId { get; set; }
        public int? StudentId { get; set; }
        public decimal? DueBalanceAmount { get; set; }
        public DateTime? LustUpdate { get; set; }
        //navigation
        public virtual Student? Student { get; set; }
    }
    public class FeePaymentDetail
    {
        [Key]
        public int FeePaymentDetailId { get; set; }
        public int FeePaymentId { get; set; }
        public string FeeTypeName { get; set; }
        public decimal FeeAmount { get; set; }

    }
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int ClassId { get; set; }

        public Class? Class { get; set; }

    }
    public class Class
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
    }
}
