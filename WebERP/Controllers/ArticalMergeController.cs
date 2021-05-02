using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebERP.Data;
using WebERP.Models;

namespace WebERP.Controllers
{
    [Authorize(Roles = "ArticalMerge , Admin")]
    public class ArticalMergeController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public ArticalMergeController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }
        [HttpGet]
        public IActionResult Artical_Merge_Detail()
        {
            List<Artical_Merge_HDR> artical_Merge_HDR = new List<Artical_Merge_HDR>();
            artical_Merge_HDR = dbContext.Artical_Merge_HDR.ToList();
            foreach (var MergList in artical_Merge_HDR)
            {
                MergList.ARTICAL_NAME = dbContext.Artical_Master.Where(a => a.ID == MergList.ARTICAL_CODE).Select(aa => aa.NAME).FirstOrDefault();
                MergList.GDW_NAME = dbContext.Godown_Master.Where(a => a.ID == MergList.GDW_CODE).Select(aa => aa.NAME).FirstOrDefault();
                MergList.ITEM_NAME = dbContext.Item_Master.Where(a => a.ID == MergList.ITEM_CODE).Select(aa => aa.NAME).FirstOrDefault();
                MergList.SIZE_NAME = dbContext.Size_Master.Where(a => a.ID == MergList.SIZE_CODE).Select(aa => aa.NAME).FirstOrDefault();
            }
            return View("Artical_Merge_Detail", artical_Merge_HDR);
        }
        [HttpGet]
        public IActionResult Artical_Merge_Master()
        {
            Artical_Merge_HDR artical_Merge_HDR = new Artical_Merge_HDR();
            ArticalMergeViewModel articalMergeViewModel = new ArticalMergeViewModel();
            articalMergeViewModel.v_STK_DTL = dbContext.V_RM_DTL.AsNoTracking().ToList();
            articalMergeViewModel.GDWDropDown = GDWlists();
            articalMergeViewModel.ARTDropDown = Artlists();
            articalMergeViewModel.SIZEDropDown = Sizelists();
            articalMergeViewModel.ITEMDropDown = ITEMlists();
            artical_Merge_HDR.DOC_DATE = DateTime.Now;
            artical_Merge_HDR.DOC_FN_YEAR = GetFinYear();
            articalMergeViewModel.artical_Merge_HDR = artical_Merge_HDR;
            return View(articalMergeViewModel);
        }
        [HttpPost]
        public IActionResult Artical_Merge_Master(ArticalMergeViewModel articalMergeViewModel, string ddlITEM, string ddlART, string ddlSize, string ddlGDW)
        {
            if (ModelState.IsValid)
            {
                List<V_RM_DTL> li = new List<V_RM_DTL>();
                List<V_RM_DTL> lli = new List<V_RM_DTL>();
                Artical_Merge_HDR artical_Merge_HDR = new Artical_Merge_HDR();
                ArticalMergeViewModel ArtMrgmodel = new ArticalMergeViewModel();
                ArtMrgmodel.GDWDropDown = GDWlists();
                ArtMrgmodel.ARTDropDown = Artlists();
                ArtMrgmodel.SIZEDropDown = Sizelists();
                ArtMrgmodel.ITEMDropDown = ITEMlists();
                artical_Merge_HDR.DOC_DATE = articalMergeViewModel.artical_Merge_HDR.DOC_DATE;
                artical_Merge_HDR.DOC_FN_YEAR = articalMergeViewModel.artical_Merge_HDR.DOC_FN_YEAR;
                //artical_Merge_HDR.GDW_CODE = ddlGDW;
                //artical_Merge_HDR.ARTICAL_CODE = ddlART;
                //artical_Merge_HDR.SIZE_CODE = ddlSize;
                //artical_Merge_HDR.ITEM_CODE = ddlITEM;
                artical_Merge_HDR.GDW_NAME = ddlGDW;
                artical_Merge_HDR.ARTICAL_NAME = ddlART;
                artical_Merge_HDR.SIZE_NAME = ddlSize;
                artical_Merge_HDR.ITEM_NAME = ddlITEM;
                artical_Merge_HDR.STK_QTY_IN = articalMergeViewModel.artical_Merge_HDR.STK_QTY_IN;
                ArtMrgmodel.artical_Merge_HDR = artical_Merge_HDR;

                foreach (var order in articalMergeViewModel.v_STK_DTL)
                {
                    if (order.CHK == true)
                    {
                        li.Add(order);
                    }
                }
                ModelState.Clear();
                ArtMrgmodel.v_STK_DTL = li.ToList();
                return View("Artical_Merge", ArtMrgmodel);
            }
            else
            {
                Artical_Merge_HDR artical_Merge_HDR = new Artical_Merge_HDR();
                ArticalMergeViewModel articalMergeViewModelss = new ArticalMergeViewModel();
                articalMergeViewModelss.v_STK_DTL = dbContext.V_RM_DTL.AsNoTracking().ToList();
                articalMergeViewModelss.GDWDropDown = GDWlists();
                articalMergeViewModelss.ARTDropDown = Artlists();
                articalMergeViewModelss.SIZEDropDown = Sizelists();
                articalMergeViewModelss.ITEMDropDown = ITEMlists();
                artical_Merge_HDR.DOC_DATE = DateTime.Now;
                artical_Merge_HDR.DOC_FN_YEAR = GetFinYear();
                articalMergeViewModelss.artical_Merge_HDR = artical_Merge_HDR;
                return View(articalMergeViewModelss);
            }

        }

        [HttpPost]
        public IActionResult Artical_Merge(ArticalMergeViewModel articalMergeViewModel, decimal STK_QTY_IN, string doc_Date, string FinYear, string ddlITEM, string ddlART, string ddlSize, string ddlGDW)
        {
            Artical_Merge_HDR artical_Merge_HDR = new Artical_Merge_HDR();
            StockDTL_Model StkDTLHDR = new StockDTL_Model();
            List<Artical_Merge_DTL> artical_Merge_List = new List<Artical_Merge_DTL>();
            List<StockDTL_Model> StkDTL = new List<StockDTL_Model>();
            int Doc_Number = dbContext.Artical_Merge_HDR
                .Where(x => x.DOC_FN_YEAR == FinYear)
                .Select(p => Convert.ToInt32(p.DOC_NO)).DefaultIfEmpty(0).Max();
            artical_Merge_HDR.DOC_NO = Doc_Number + 1;
            artical_Merge_HDR.DOC_DATE = Convert.ToDateTime(doc_Date);
            artical_Merge_HDR.DOC_FN_YEAR = FinYear;
            artical_Merge_HDR.GDW_CODE = Convert.ToInt32(ddlGDW);
            artical_Merge_HDR.ARTICAL_CODE = Convert.ToInt32(ddlART);
            artical_Merge_HDR.SIZE_CODE = Convert.ToInt32(ddlSize);
            artical_Merge_HDR.ITEM_CODE = Convert.ToInt32(ddlITEM);
            artical_Merge_HDR.STK_QTY_IN = STK_QTY_IN;
            artical_Merge_HDR.INS_DATE = DateTime.Now;
            artical_Merge_HDR.INS_UID = userManager.GetUserName(HttpContext.User);
            dbContext.Artical_Merge_HDR.Add(artical_Merge_HDR);
            dbContext.SaveChanges();
            var ID = artical_Merge_HDR.ID;

            StkDTLHDR.INS_DATE = DateTime.Now;
            StkDTLHDR.INS_UID = userManager.GetUserName(HttpContext.User);
            StkDTLHDR.COMP_CODE = 0;
            StkDTLHDR.Tran_Table = "Artical Merge Entry HDR";
            StkDTLHDR.Tran_Table_PK = ID;
            StkDTLHDR.GDW_CODE = Convert.ToInt32(ddlGDW);
            StkDTLHDR.Item_Code = Convert.ToInt32(ddlITEM);
            StkDTLHDR.Artical_CODE = Convert.ToInt32(ddlART);
            StkDTLHDR.Size_Code = Convert.ToInt32(ddlSize);
            StkDTLHDR.Stk_Qty_IN = STK_QTY_IN;
            dbContext.StockDTL_Models.Add(StkDTLHDR);
            dbContext.SaveChanges();

            foreach (var order in articalMergeViewModel.v_STK_DTL)
            {
                artical_Merge_List.Add(new Artical_Merge_DTL()
                {
                    HDR_FK = ID,
                    INS_DATE = DateTime.Now,
                    INS_UID = userManager.GetUserName(HttpContext.User),
                    GDW_CODE = order.GDW_CODE,
                    ITEM_CODE = order.ITEM_CODE,
                    ARTICAL_CODE = order.ARTICAL_CODE,
                    SIZE_CODE = order.SIZE_CODE,
                    STK_QTY_OUT = order.Issue_Qty,
                });             

            }
            foreach (var item in artical_Merge_List)
            {
                dbContext.Artical_Merge_DTL.Add(item);
                dbContext.SaveChanges();
                StkDTL.Add(new StockDTL_Model()
                {
                    INS_DATE = DateTime.Now,
                    INS_UID = userManager.GetUserName(HttpContext.User),
                    COMP_CODE = 0,
                    Tran_Table = "Artical Merge Entry DTL",
                    Tran_Table_PK = item.ID,
                    GDW_CODE = Convert.ToInt32(item.GDW_CODE),
                    Item_Code = Convert.ToInt32(item.ITEM_CODE),
                    Artical_CODE = Convert.ToInt32(item.ARTICAL_CODE),
                    Size_Code = Convert.ToInt32(item.SIZE_CODE),
                    Stk_Qty_OUT = Convert.ToInt32(item.STK_QTY_OUT),
                });
            }
            foreach (var item in StkDTL)
            {
                dbContext.StockDTL_Models.Add(item);
                dbContext.SaveChanges();

            }

            //articalMergeViewModel.v_STK_DTL = li.ToList();
            return RedirectToAction("Artical_Merge_Detail");
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

        public List<SelectListItem> GDWlists()
        {
            var GDWList = (from GDW in dbContext.Godown_Master.AsNoTracking().ToList()
                           select new SelectListItem()
                           {
                               Text = GDW.NAME,
                               Value = Convert.ToString(GDW.ID),
                           }).ToList();

            return GDWList;
        }
        public List<SelectListItem> ITEMlists()
        {
            var ITEMList = (from Item in dbContext.Item_Master.AsNoTracking().ToList()
                            select new SelectListItem()
                            {
                                Text = Item.NAME,
                                Value = Convert.ToString(Item.ID),
                            }).ToList();

            return ITEMList;
        }
        public List<SelectListItem> Artlists()
        {
            var ARTList = (from ART in dbContext.Artical_Master.AsNoTracking().ToList()
                           select new SelectListItem()
                           {
                               Text = ART.NAME,
                               Value = Convert.ToString(ART.ID),
                           }).ToList();

            return ARTList;
        }
        public List<SelectListItem> Sizelists()
        {
            var SIZEList = (from SIZE in dbContext.Size_Master.AsNoTracking().ToList()
                            select new SelectListItem()
                            {
                                Text = SIZE.NAME,
                                Value = Convert.ToString(SIZE.ID),
                            }).ToList();

            return SIZEList;
        }
    }
}