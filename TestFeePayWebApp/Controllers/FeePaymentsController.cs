using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestFeePayWebApp.Models;

namespace TestFeePayWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeePaymentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FeePaymentsController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetFeePayments()
        {
            try
            {
                var feePayments = await _context.feePayments

                    .Include(fp => fp.FeePaymentDetails)  // Include related FeePaymentDetails entities if needed
                    .ToListAsync();

                return Ok(feePayments);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Exception: {ex}");

                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


        // GET: api/FeePayments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeePaymentById(int id)
        {
            try
            {
                var feePayment = await _context.feePayments

                    .Include(fp => fp.FeePaymentDetails)  // Include related FeePaymentDetails entities if needed
                    .FirstOrDefaultAsync(fp => fp.FeePaymentId == id);

                if (feePayment == null)
                {
                    return NotFound($"FeePayment with ID {id} not found");
                }

                return Ok(feePayment);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Exception: {ex}");

                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


        // PUT: api/FeePayments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFeePayment(int id, FeePayment feePayment)
        {
            if (id != feePayment.FeePaymentId)
            {
                return BadRequest();
            }

            _context.Entry(feePayment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeePaymentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

       

        [HttpPost]
        public async Task<IActionResult> CreateFeePayment([FromBody] FeePayment feePayment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await AttachFeeStructureAsync(feePayment);
                    await CalculateFeePaymentFieldsAsync(feePayment);

                    UpdateDueBalance(feePayment);

                    _context.feePayments.Add(feePayment);
                    await _context.SaveChangesAsync();

                    // Save FeePaymentDetails
                    SaveFeePaymentDetails(feePayment);

                    transaction.Commit();

                    return Ok(feePayment);
                }
                catch (Exception ex)
                {
                    // Log the exception for debugging purposes
                    Console.WriteLine($"Exception: {ex}");

                    transaction.Rollback();
                    return StatusCode(500, $"Internal Server Error: {ex.Message}");
                }
            }
        }

        private async Task AttachFeeStructureAsync(FeePayment feePayment)
        {
            if (feePayment.FeeStructure != null && feePayment.FeeStructure.Any())
            {
                feePayment.FeeStructure = await _context.feeStructures
                    .Where(fs => feePayment.FeeStructure.Select(f => f.FeeStructureId).Contains(fs.FeeStructureId))
                    .ToListAsync();
            }
        }

        private async Task CalculateFeePaymentFieldsAsync(FeePayment feePayment)
        {
            var student = _context.students
                       .Where(ft => ft.Id == feePayment.StudentId)
                       .FirstOrDefault();

            var studentId = feePayment.StudentId;
            feePayment.Name = student?.Name;
            feePayment.TotalFeeAmount = feePayment.FeeStructure?.Sum(fs => fs.FeeAmount) ?? 0;
            feePayment.AmountAfterDiscount = feePayment.TotalFeeAmount - (feePayment.TotalFeeAmount * feePayment.Discount / 100);

            var previousDue = await _context.dueBalances
                .Where(b => b.StudentId == studentId)
                .Select(b => b.DueBalanceAmount)
                .FirstOrDefaultAsync();

            feePayment.PreviousDue = previousDue ?? 0;
            feePayment.TotalAmount = feePayment.AmountAfterDiscount + feePayment.PreviousDue;
            feePayment.AmountRemaining = feePayment.TotalAmount - feePayment.AmountPaid;
        }

        private void UpdateDueBalance(FeePayment feePayment)
        {
            var dueBalance = _context.dueBalances
                .Where(db => db.StudentId == feePayment.StudentId)
                .FirstOrDefault();

            if (dueBalance != null)
            {
                dueBalance.DueBalanceAmount = feePayment.AmountRemaining;
                dueBalance.LustUpdate = DateTime.Now; // Update LastUpdate timestamp
            }
            else
            {
                _context.dueBalances.Add(new DueBalance
                {
                    StudentId = feePayment.StudentId,
                    DueBalanceAmount = feePayment.AmountRemaining,
                    LustUpdate = DateTime.Now // Set LastUpdate timestamp for a new record
                });
            }
        }

        private void SaveFeePaymentDetails(FeePayment feePayment)
        {
            if (feePayment.FeeStructure != null && feePayment.FeeStructure.Any())
            {
                foreach (var feeStructure in feePayment.FeeStructure)
                {
                    // Fetch FeeType for FeeTypeName
                    var feeType = _context.FeeTypes
                        .Where(ft => ft.FeeTypeId == feeStructure.FeeTypeId)
                        .FirstOrDefault();

                    var feePaymentDetail = new FeePaymentDetail
                    {
                        FeePaymentId = feePayment.FeePaymentId,
                        FeeAmount = feeStructure.FeeAmount,
                        FeeTypeName = feeType?.TypeName // Set FeeTypeName based on FeeType
                    };

                    _context.feePaymentDetails.Add(feePaymentDetail);
                }

                _context.SaveChanges();
            }
        }





        // DELETE: api/FeePayments/5
        // DELETE: api/FeePayments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeePayment(int id)
        {
            var feePayment = await _context.feePayments
                .Include(fp => fp.FeeStructure) // Include related feeStructures
                .FirstOrDefaultAsync(fp => fp.FeePaymentId == id);

            if (feePayment == null)
            {
                return NotFound();
            }

            // Remove the reference to FeePayment in feeStructures
            foreach (var feeStructure in feePayment.FeeStructure)
            {
                feeStructure.FeePayment = null;
            }

            _context.feePayments.Remove(feePayment);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool FeePaymentExists(int id)
        {
            return _context.feePayments.Any(e => e.FeePaymentId == id);
        }
    }
}
