using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TestFeePayWebApp.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<FeeType> FeeTypes { get; set; }
        public DbSet<FeeStructure> feeStructures { get; set; }
        public DbSet<FeePayment> feePayments { get; set; }
        public DbSet<DueBalance> dueBalances { get; set; }
        public DbSet<Student> students { get; set; }
        public DbSet<Class> classes { get; set; }
        public DbSet<FeePaymentDetail> feePaymentDetails { get; set; }
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<Class>().HasData(new Class[]
            {
                new Class
                {
                    Id=1,
                    ClassName="ClassOne"
                },
                new Class
                {
                    Id=2,
                    ClassName="ClassTwo"
                },
                new Class
                {
                    Id=3,
                    ClassName="ClassThree"
                },
                new Class
                {
                    Id=4,
                    ClassName="ClassFour"
                }

            });

            modelBuilder.Entity<Student>().HasData(new Student[]
            {
                new Student
                {
                    Id=1,
                    Name="Hasan",
                    ClassId=1
                },
                new Student
                {
                    Id=2,
                    Name="Kamal",
                    ClassId=1
                },
                new Student
                {
                    Id=3,
                    Name="Zahidul",
                    ClassId=2
                },
                new Student
                {
                    Id=4,
                    Name="Joynal",
                    ClassId=2
                }

            });





            modelBuilder.Entity<FeeType>().HasData(new FeeType[]
            {
                new FeeType
                {
                    FeeTypeId=1,
                    TypeName="TusionFee"
                },
                new FeeType
                {
                    FeeTypeId=2,
                    TypeName="LibraryFee"
                },
                new FeeType
                {
                    FeeTypeId=3,
                    TypeName="TransfortFee"
                },
                new FeeType
                {
                    FeeTypeId=4,
                    TypeName="DevelopmentFee"
                }

            });

            modelBuilder.Entity<FeeStructure>().HasData(new FeeStructure[]
            {
                new FeeStructure
                {
                    FeeStructureId=1,
                    ClassId=1,
                    FeeTypeId=1,
                    Monthly=true,
                    Yearly=false,
                    FeeAmount=2000
                },
                new FeeStructure
                {
                    FeeStructureId=2,
                    ClassId=1,
                    FeeTypeId=2,
                    Monthly=true,
                    Yearly=false,
                    FeeAmount=400
                },
                new FeeStructure
                {
                    FeeStructureId=3,
                    ClassId=2,
                    FeeTypeId=1,
                    Monthly=true,
                    Yearly=false,
                    FeeAmount=2500
                },
                new FeeStructure
                {
                    FeeStructureId=4,
                    ClassId=2,
                    FeeTypeId=2,
                    Monthly=true,
                    Yearly=false,
                    FeeAmount=500
                },
                new FeeStructure
                {
                    FeeStructureId=5,
                    ClassId=3,
                    FeeTypeId=1,
                    Monthly=true,
                    Yearly=false,
                    FeeAmount=2500
                },
                new FeeStructure
                {
                    FeeStructureId=6,
                    ClassId=3,
                    FeeTypeId=2,
                    Monthly=true,
                    Yearly=false,
                    FeeAmount=500
                }


            });
            modelBuilder.Entity<FeePaymentDetail>().HasData(new FeePaymentDetail[]
            {
                new FeePaymentDetail
                {
                    FeePaymentDetailId = 1,
                    FeePaymentId = 1,
                    FeeTypeName = "TuitionFee",
                    FeeAmount = 2000
                },
                new FeePaymentDetail
                {
                    FeePaymentDetailId = 2,
                    FeePaymentId = 1,
                    FeeTypeName = "LibraryFee",
                    FeeAmount = 400
                },
                new FeePaymentDetail
                {
                    FeePaymentDetailId = 3,
                    FeePaymentId = 2,
                    FeeTypeName = "TuitionFee",
                    FeeAmount = 2000
                }, new FeePaymentDetail
                {
                    FeePaymentDetailId = 4,
                    FeePaymentId = 2,
                    FeeTypeName = "LibraryFee",
                    FeeAmount = 400
                },
                 new FeePaymentDetail
                {
                    FeePaymentDetailId = 5,
                    FeePaymentId = 3,
                    FeeTypeName = "TuitionFee",
                    FeeAmount = 2500
                }, new FeePaymentDetail
                {
                    FeePaymentDetailId = 6,
                    FeePaymentId = 3,
                    FeeTypeName = "LibraryFee",
                    FeeAmount = 500
                }



            });

            modelBuilder.Entity<FeePayment>().HasData(new FeePayment[]
            {
                new FeePayment
                {
                FeePaymentId = 1,
                StudentId = 1,
                Name="Hasan",
                TotalFeeAmount = 2400,
                Discount = 5,
                AmountAfterDiscount = 2280,
                PreviousDue = 0,
                TotalAmount = 2280,
                AmountPaid = 3000,
                AmountRemaining = -720,
                PaymentDate = DateTime.Parse("2024-02-14T10:42:22.787")
                },
                new FeePayment
                {
                FeePaymentId = 2,
                Name="Kamal",
                StudentId = 2,
                TotalFeeAmount = 2400,
                Discount = 5,
                AmountAfterDiscount = 2280,
                PreviousDue = 0,
                TotalAmount = 2280,
                AmountPaid = 500,
                AmountRemaining = 1780,
                PaymentDate = DateTime.Parse("2024-02-14T10:42:22.787")
                },
                 new FeePayment
                {
                FeePaymentId = 3,
                StudentId = 3,
                Name="Zahidul",
                TotalFeeAmount = 2400,
                Discount = 5,
                AmountAfterDiscount = 2280,
                PreviousDue = 0,
                TotalAmount = 2280,
                AmountPaid = 500,
                AmountRemaining = 1780,
                PaymentDate = DateTime.Parse("2024-02-14T10:42:22.787")
                }

            });

            base.OnModelCreating(modelBuilder);
        }

    }
}
