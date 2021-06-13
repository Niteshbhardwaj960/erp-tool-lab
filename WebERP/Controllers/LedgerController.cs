using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Reporting;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebERP.Data;
using WebERP.Models;
using WebERP.Helpers;

namespace WebERP.Controllers
{
    [Authorize(Roles = "Ledger , Admin")]
    public class LedgerController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;
        private readonly IHostingEnvironment _IHostingEnvironment;

        public LedgerController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            IHostingEnvironment IHostingEnvironment)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
            this._IHostingEnvironment = IHostingEnvironment;
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
            LedgerViewModel ledgers = new LedgerViewModel();
            ledgers.v_LEDGERs = dbContext.V_LEDGER.AsNoTracking().Where(x => x.ACC_CODE.ToString() == ddlACC && Convert.ToDateTime(x.DOC_DATE) >= ledgerViewModel.FromDate && Convert.ToDateTime(x.DOC_DATE) <= ledgerViewModel.To_Date).ToList();
            ledgers.ddlAcc = ACClists();
            ledgers.Acc_Codes = ddlACC;
            ledgers.type = "Filter";
            decimal opnball = OpeningAmount(dbContext.V_LEDGER.AsNoTracking().Where(x => x.ACC_CODE.ToString() == ddlACC && Convert.ToDateTime(x.DOC_DATE) < ledgerViewModel.FromDate).ToList());
            char chr = Convert.ToChar("-");
            if (opnball < 1)
            {
                ledgers.CR_Amounts = Math.Abs(opnball);
                ledgers.DR_Amounts = 0;
            }
            else
            {
                ledgers.CR_Amounts = 0;
                ledgers.DR_Amounts = opnball;
            }
            decimal dramount = 0;
            decimal cramount = 0;
            foreach (var data in ledgers.v_LEDGERs.OrderBy(a => a.DOC_DATE).ToList())
            {
                decimal bal = 0;
                dramount = dramount + data.DR_AMOUNT;
                cramount = cramount + data.CR_AMOUNT;
                bal = (opnball + dramount - cramount);
                data.BAL_AMOUNT = Math.Abs(bal);
                data.DOC_DATE = Helper.DateFormat(data.DOC_DATE);
            }

            return View("Ledger", ledgers);
        }
        //public JsonResult Filter(string fromdate, string todate, string ddlACC)
        //{
        //    var fildata = dbContext.V_LEDGER.AsNoTracking().Where(x => x.ACC_CODE.ToString() == ddlACC && Convert.ToDateTime(x.DOC_DATE) >= Convert.ToDateTime(fromdate) && Convert.ToDateTime(x.DOC_DATE) <= Convert.ToDateTime(todate)).ToList();

        //    return Json(fildata);
        //}
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
            DateTime date = Helper.DateFormatDate(Convert.ToString(DateTime.Now));
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


        [HttpGet]
        public IActionResult Export(LedgerViewModel ledgerViewModel, string ddlACC, string CRAMNT, string DRAMNT)
        {
            var ComData = dbContext.V_LEDGER.AsNoTracking().Where(x => x.ACC_CODE.ToString() == ddlACC && Convert.ToDateTime(x.DOC_DATE) >= ledgerViewModel.FromDate && Convert.ToDateTime(x.DOC_DATE) <= ledgerViewModel.To_Date).ToList();

            var accname = ComData.Select(a => a.ACC_NAME).FirstOrDefault();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Ledger");

                var currentRow = 6;
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 1).Value = "Account Name";
                worksheet.Cell(1, 2).Value = accname;
                worksheet.Cell(1, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                worksheet.Cell(2, 1).Style.Font.Bold = true;
                worksheet.Cell(2, 1).Value = "From Date";
                worksheet.Cell(2, 2).Value = ledgerViewModel.FromDate;
                worksheet.Cell(2, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                worksheet.Cell(3, 1).Style.Font.Bold = true;
                worksheet.Cell(3, 1).Value = "To Date";
                worksheet.Cell(3, 2).Value = ledgerViewModel.To_Date;
                worksheet.Cell(3, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                worksheet.Cell(5, 5).Style.Font.Bold = true;
                worksheet.Cell(5, 5).Value = "Opening Balance";
                worksheet.Cell(5, 6).Value = DRAMNT;
                worksheet.Cell(5, 7).Value = CRAMNT;
                worksheet.Row(currentRow).Style.Font.Bold = true;


                worksheet.Cell(currentRow, 1).Style.Fill.BackgroundColor = XLColor.Pistachio;
                worksheet.Cell(currentRow, 2).Style.Fill.BackgroundColor = XLColor.Pistachio;
                worksheet.Cell(currentRow, 3).Style.Fill.BackgroundColor = XLColor.Pistachio;
                worksheet.Cell(currentRow, 4).Style.Fill.BackgroundColor = XLColor.Pistachio;
                worksheet.Cell(currentRow, 5).Style.Fill.BackgroundColor = XLColor.Pistachio;
                worksheet.Cell(currentRow, 6).Style.Fill.BackgroundColor = XLColor.Pistachio;
                worksheet.Cell(currentRow, 7).Style.Fill.BackgroundColor = XLColor.Pistachio;

                worksheet.Cell(currentRow, 1).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(currentRow, 2).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(currentRow, 3).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(currentRow, 4).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(currentRow, 5).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(currentRow, 6).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(currentRow, 7).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                worksheet.Cell(currentRow, 1).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(currentRow, 2).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(currentRow, 3).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(currentRow, 4).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(currentRow, 5).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(currentRow, 6).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(currentRow, 7).Style.Border.TopBorder = XLBorderStyleValues.Thin;

                worksheet.Cell(currentRow, 1).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(currentRow, 2).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(currentRow, 3).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(currentRow, 4).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(currentRow, 5).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(currentRow, 6).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(currentRow, 7).Style.Border.LeftBorder = XLBorderStyleValues.Thin;

                worksheet.Cell(currentRow, 1).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(currentRow, 2).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(currentRow, 3).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(currentRow, 4).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(currentRow, 5).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(currentRow, 6).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(currentRow, 7).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                worksheet.Cell(currentRow, 1).Value = "DOC Fin Year";
                worksheet.Cell(currentRow, 2).Value = "DOC No.";
                worksheet.Cell(currentRow, 3).Value = "Doc Date";
                worksheet.Cell(currentRow, 4).Value = "Doc Type";
                worksheet.Cell(currentRow, 5).Value = "Remarks";
                worksheet.Cell(currentRow, 6).Value = "Dr Amount";
                worksheet.Cell(currentRow, 7).Value = "Cr Amount";

                var currentcell = 1;

                foreach (var Data in ComData)
                {
                    currentRow++;

                    for (var i = 1; i <= 7; i++)
                    {
                        worksheet.Cell(currentRow, i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(currentRow, i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(currentRow, i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(currentRow, i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }
                    worksheet.Cell(currentRow, 1).Value = Data.DOC_FN_YEAR;
                    worksheet.Cell(currentRow, 2).Value = Data.DOC_NO;
                    worksheet.Cell(currentRow, 3).Value = Data.DOC_DATE;
                    worksheet.Cell(currentRow, 4).Value = Data.DOC_TYPE;
                    worksheet.Cell(currentRow, 5).Value = Data.REMARKS;
                    worksheet.Cell(currentRow, 6).Value = Data.DR_AMOUNT;
                    worksheet.Cell(currentRow, 7).Value = Data.CR_AMOUNT;
                    currentcell++;
                }

                worksheet.ColumnWidth = 15;

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

        [HttpPost]
        public IActionResult Print(LedgerViewModel ledgerViewModel, string ddlACC, string CRAMNT, string DRAMNT)
        {
            DataTable dt = new DataTable();

            var Comm_Name = dbContext.Companies.FirstOrDefault();

            var dtList = ledgerViewModel.v_LEDGERs.ToList();
            var accname = dbContext.Account_Masters.Where(aa => aa.ID.ToString() == ddlACC).Select(a => a.NAME).FirstOrDefault();
            dt = ListtoDatataable.ToDataTable(dtList);

            string mimetype = "";
            int extension = 1;
            var path = $"{this._IHostingEnvironment.WebRootPath}\\Reports\\RptLedger1.rdlc";

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("Comm_Name", Comm_Name.NAME);
            parameters.Add("Comm_Add", Comm_Name.ADD1 + " " + Comm_Name.ADD2);
            parameters.Add("Comm_Ph", Comm_Name.PH_NO);
            parameters.Add("CRAMNT", CRAMNT);
            parameters.Add("DRAMNT", DRAMNT);
            parameters.Add("fromdate", Helper.DateFormat(Convert.ToString(ledgerViewModel.FromDate)));
            parameters.Add("todate", Helper.DateFormat(Convert.ToString(ledgerViewModel.To_Date)));
            parameters.Add("ACC_Name", accname);

            LocalReport localReport = new LocalReport(path);

            localReport.AddDataSource("DataSet1", dt);

            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimetype);

            return File(result.MainStream, "application/pdf", "LedgerReport.pdf");
        }

    }
}