using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TestFeePayWebApp.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "classes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeeTypes",
                columns: table => new
                {
                    FeeTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeTypes", x => x.FeeTypeId);
                });

            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_students_classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dueBalances",
                columns: table => new
                {
                    BalanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: true),
                    DueBalanceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LustUpdate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dueBalances", x => x.BalanceId);
                    table.ForeignKey(
                        name: "FK_dueBalances_students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "feePayments",
                columns: table => new
                {
                    FeePaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalFeeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountAfterDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PreviousDue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountRemaining = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feePayments", x => x.FeePaymentId);
                    table.ForeignKey(
                        name: "FK_feePayments_students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "feePaymentDetails",
                columns: table => new
                {
                    FeePaymentDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeePaymentId = table.Column<int>(type: "int", nullable: false),
                    FeeTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FeeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feePaymentDetails", x => x.FeePaymentDetailId);
                    table.ForeignKey(
                        name: "FK_feePaymentDetails_feePayments_FeePaymentId",
                        column: x => x.FeePaymentId,
                        principalTable: "feePayments",
                        principalColumn: "FeePaymentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "feeStructures",
                columns: table => new
                {
                    FeeStructureId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    Monthly = table.Column<bool>(type: "bit", nullable: false),
                    Yearly = table.Column<bool>(type: "bit", nullable: false),
                    FeeTypeId = table.Column<int>(type: "int", nullable: false),
                    FeeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FeePaymentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feeStructures", x => x.FeeStructureId);
                    table.ForeignKey(
                        name: "FK_feeStructures_FeeTypes_FeeTypeId",
                        column: x => x.FeeTypeId,
                        principalTable: "FeeTypes",
                        principalColumn: "FeeTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_feeStructures_classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_feeStructures_feePayments_FeePaymentId",
                        column: x => x.FeePaymentId,
                        principalTable: "feePayments",
                        principalColumn: "FeePaymentId");
                });

            migrationBuilder.InsertData(
                table: "FeeTypes",
                columns: new[] { "FeeTypeId", "TypeName" },
                values: new object[,]
                {
                    { 1, "TusionFee" },
                    { 2, "LibraryFee" },
                    { 3, "TransfortFee" },
                    { 4, "DevelopmentFee" }
                });

            migrationBuilder.InsertData(
                table: "classes",
                columns: new[] { "Id", "ClassName" },
                values: new object[,]
                {
                    { 1, "ClassOne" },
                    { 2, "ClassTwo" },
                    { 3, "ClassThree" },
                    { 4, "ClassFour" }
                });

            migrationBuilder.InsertData(
                table: "feeStructures",
                columns: new[] { "FeeStructureId", "ClassId", "FeeAmount", "FeePaymentId", "FeeTypeId", "Monthly", "Yearly" },
                values: new object[,]
                {
                    { 1, 1, 2000m, null, 1, true, false },
                    { 2, 1, 400m, null, 2, true, false },
                    { 3, 2, 2500m, null, 1, true, false },
                    { 4, 2, 500m, null, 2, true, false },
                    { 5, 3, 2500m, null, 1, true, false },
                    { 6, 3, 500m, null, 2, true, false }
                });

            migrationBuilder.InsertData(
                table: "students",
                columns: new[] { "Id", "ClassId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Hasan" },
                    { 2, 1, "Kamal" },
                    { 3, 2, "Zahidul" },
                    { 4, 2, "Joynal" }
                });

            migrationBuilder.InsertData(
                table: "feePayments",
                columns: new[] { "FeePaymentId", "AmountAfterDiscount", "AmountPaid", "AmountRemaining", "Discount", "Name", "PaymentDate", "PreviousDue", "StudentId", "TotalAmount", "TotalFeeAmount" },
                values: new object[,]
                {
                    { 1, 2280m, 3000m, -720m, 5m, "Hasan", new DateTime(2024, 2, 14, 10, 42, 22, 787, DateTimeKind.Unspecified), 0m, 1, 2280m, 2400m },
                    { 2, 2280m, 500m, 1780m, 5m, "Kamal", new DateTime(2024, 2, 14, 10, 42, 22, 787, DateTimeKind.Unspecified), 0m, 2, 2280m, 2400m },
                    { 3, 2280m, 500m, 1780m, 5m, "Zahidul", new DateTime(2024, 2, 14, 10, 42, 22, 787, DateTimeKind.Unspecified), 0m, 3, 2280m, 2400m }
                });

            migrationBuilder.InsertData(
                table: "feePaymentDetails",
                columns: new[] { "FeePaymentDetailId", "FeeAmount", "FeePaymentId", "FeeTypeName" },
                values: new object[,]
                {
                    { 1, 2000m, 1, "TuitionFee" },
                    { 2, 400m, 1, "LibraryFee" },
                    { 3, 2000m, 2, "TuitionFee" },
                    { 4, 400m, 2, "LibraryFee" },
                    { 5, 2500m, 3, "TuitionFee" },
                    { 6, 500m, 3, "LibraryFee" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_dueBalances_StudentId",
                table: "dueBalances",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_feePaymentDetails_FeePaymentId",
                table: "feePaymentDetails",
                column: "FeePaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_feePayments_StudentId",
                table: "feePayments",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_feeStructures_ClassId",
                table: "feeStructures",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_feeStructures_FeePaymentId",
                table: "feeStructures",
                column: "FeePaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_feeStructures_FeeTypeId",
                table: "feeStructures",
                column: "FeeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_students_ClassId",
                table: "students",
                column: "ClassId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dueBalances");

            migrationBuilder.DropTable(
                name: "feePaymentDetails");

            migrationBuilder.DropTable(
                name: "feeStructures");

            migrationBuilder.DropTable(
                name: "FeeTypes");

            migrationBuilder.DropTable(
                name: "feePayments");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "classes");
        }
    }
}
