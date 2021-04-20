using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebERP.Data;
using WebERP.Models;

namespace WebERP.Controllers
{
    [Authorize(Roles = "Ledger , Admin")]
    public class LedgerController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public LedgerController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }
        [HttpGet]
        public IActionResult Ledger()
        {
            LedgerViewModel ledgerViewModel = new LedgerViewModel();
            //ledgerViewModel.v_LEDGERs = dbContext.V_LEDGER.AsNoTracking().ToList();
            ledgerViewModel.ddlAcc = ACClists();
            ledgerViewModel.FromDate = new DateTime(DateTime.Now.Year, 4, 1);
            ledgerViewModel.To_Date = new DateTime(DateTime.Now.Year + 1, 3, 31);
            return View(ledgerViewModel);
        }
        [HttpPost]
        public IActionResult Filter(LedgerViewModel ledgerViewModel, string ddlACC)
        {
            ledgerViewModel.v_LEDGERs = dbContext.V_LEDGER.AsNoTracking().Where(x => x.ACC_CODE.ToString() == ddlACC && Convert.ToDateTime(x.DOC_DATE) >= ledgerViewModel.FromDate && Convert.ToDateTime(x.DOC_DATE) <= ledgerViewModel.To_Date).ToList();
            ledgerViewModel.ddlAcc = ACClists();
            ledgerViewModel.Acc_Codes = ddlACC;
            ledgerViewModel.type = "Filter";
            decimal opnball = OpeningAmount(dbContext.V_LEDGER.AsNoTracking().Where(x => x.ACC_CODE.ToString() == ddlACC && Convert.ToDateTime(x.DOC_DATE) < ledgerViewModel.FromDate).ToList());
            char chr = Convert.ToChar("-");
            if (opnball < 1)
            {
                ledgerViewModel.CR_Amounts = opnball.ToString().TrimStart(chr);
                ledgerViewModel.DR_Amounts = "0";
            }
            else
            {
                ledgerViewModel.CR_Amounts = "0";
                ledgerViewModel.DR_Amounts = opnball.ToString();
            }
            return View("Ledger", ledgerViewModel);
        }
        public decimal OpeningAmount(List<V_LEDGER> v_LEDGERs)
        {
            decimal OpnAmnt = 0;
            decimal CrAmnt = 0;
            decimal DrnAmnt = 0;
            foreach (var v_LEDGER in v_LEDGERs.ToList())
            {
                DrnAmnt = DrnAmnt + v_LEDGER.DR_AMOUNT;
                CrAmnt = CrAmnt + v_LEDGER.CR_AMOUNT;
            }
            OpnAmnt = DrnAmnt - CrAmnt;

            return OpnAmnt;
        }
        public List<SelectListItem> ACClists()
        {
            var AccList = (from ACC in dbContext.V_LEDGER.AsNoTracking().GroupBy(x => x.ACC_NAME).Select(x => x.First()).ToList()
                           select new SelectListItem()
                           {
                               Text = ACC.ACC_NAME,
                               Value = Convert.ToString(ACC.ACC_CODE),
                           }).ToList();
            return AccList;
        }
        public string GetFinYear()
        {
            string FinYear = "";
            DateTime date = DateTime.Now;
            if ((date.Month) == 1 || (date.Month) == 2 || (date.Month) == 3)
            {
                FinYear = (date.Year - 1) + "" + date.Year;
            }
            else
            {
                FinYear = date.Year + "" + (date.Year + 1);
            }
            return FinYear;
        }

        [HttpPost]
        public IActionResult Export(LedgerViewModel ledgerViewModel , string ddlACC)
        {
            var ComData = dbContext.V_LEDGER.AsNoTracking().Where(x => x.ACC_CODE.ToString() == ddlACC && Convert.ToDateTime(x.DOC_DATE) >= ledgerViewModel.FromDate && Convert.ToDateTime(x.DOC_DATE) <= ledgerViewModel.To_Date).ToList();
         

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Ledger");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Account Name";
                worksheet.Cell(currentRow, 2).Value = "DOC Fin Year";
                worksheet.Cell(currentRow, 3).Value = "DOC No.";
                worksheet.Cell(currentRow, 4).Value = "Doc Date";
                worksheet.Cell(currentRow, 5).Value = "Doc Type";
                worksheet.Cell(currentRow, 6).Value = "Remarks";
                worksheet.Cell(currentRow, 7).Value = "Dr Amount";
                worksheet.Cell(currentRow, 8).Value = "Cr Amount";

                foreach (var Data in ComData)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = Data.ACC_NAME;
                    worksheet.Cell(currentRow, 2).Value = Data.DOC_FN_YEAR;
                    worksheet.Cell(currentRow, 3).Value = Data.DOC_NO;
                    worksheet.Cell(currentRow, 4).Value = Data.DOC_DATE;
                    worksheet.Cell(currentRow, 5).Value = Data.DOC_TYPE;
                    worksheet.Cell(currentRow, 6).Value = Data.REMARKS;
                    worksheet.Cell(currentRow, 7).Value = Data.DR_AMOUNT;
                    worksheet.Cell(currentRow, 8).Value = Data.CR_AMOUNT;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Ledger.xlsx");
                }
            }
        }
    }
}