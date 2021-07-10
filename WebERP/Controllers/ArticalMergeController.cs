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
using WebERP.Helpers;
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

        [HttpGet]
        public IActionResult Edit_AM(int id)
        {
            Artical_Merge_HDR artical_Merge_HDR = new Artical_Merge_HDR();
            artical_Merge_HDR = dbContext.Artical_Merge_HDR.Where(am => am.ID == id).FirstOrDefault();
            ArticalMergeViewModel ArtMrgmodel = new ArticalMergeViewModel();
            ArtMrgmodel.GDWDropDown = GDWlists();
            ArtMrgmodel.ARTDropDown = Artlists();
            ArtMrgmodel.SIZEDropDown = Sizelists();
            ArtMrgmodel.ITEMDropDown = ITEMlists();
            ArtMrgmodel.artical_Merge_HDR = artical_Merge_HDR;

            ArtMrgmodel.artical_Merge_DTL = dbContext.Artical_Merge_DTL.Where(ad => ad.HDR_FK == id).ToList();
            foreach (var item in ArtMrgmodel.artical_Merge_DTL)
            {
                item.ARTICAL_NAME = dbContext.Artical_Master.Where(a => a.ID == item.ARTICAL_CODE).Select(a => a.NAME).FirstOrDefault();
                item.SIZE_NAME = dbContext.Size_Master.Where(a => a.ID == item.SIZE_CODE).Select(a => a.NAME).FirstOrDefault();
                item.ITEM_NAME = dbContext.Item_Master.Where(a => a.ID == item.ITEM_CODE).Select(a => a.NAME).FirstOrDefault();
                item.GDW_NAME = dbContext.Godown_Master.Where(a => a.ID == item.GDW_CODE).Select(a => a.NAME).FirstOrDefault();
            }
            ArtMrgmodel.Type = "Edit";
            return View("Edit_AM", ArtMrgmodel);


        }

        [HttpPost]
        public IActionResult Edit_AM(ArticalMergeViewModel articalMergeViewModel,int HDRID, decimal STK_QTY_IN, string doc_Date, string FinYear, string ddlITEM, string ddlART, string ddlSize, string ddlGDW)
        {
            if (ModelState.IsValid)
            {
                var ArtMgrHDR = dbContext.Artical_Merge_HDR.Where(hd => hd.ID == HDRID).FirstOrDefault();
                if (ArtMgrHDR != null)
                {
                    ArtMgrHDR.UDT_DATE = DateTime.Now;
                    ArtMgrHDR.UDT_UID = userManager.GetUserName(HttpContext.User);
                    ArtMgrHDR.DOC_DATE = Convert.ToDateTime(doc_Date);
                    ArtMgrHDR.DOC_FN_YEAR = FinYear;
                    ArtMgrHDR.GDW_CODE = Convert.ToInt32(ddlGDW);
                    ArtMgrHDR.ARTICAL_CODE = Convert.ToInt32(ddlART);
                    ArtMgrHDR.SIZE_CODE = Convert.ToInt32(ddlSize);
                    ArtMgrHDR.ITEM_CODE = Convert.ToInt32(ddlITEM);
                    ArtMgrHDR.STK_QTY_IN = STK_QTY_IN;
                    dbContext.SaveChanges();
                }

                var stkdetail = dbContext.StockDTL_Models.Where(s => s.Tran_Table_PK == HDRID && s.Tran_Table == "Artical Merge Entry HDR").FirstOrDefault();
                if (stkdetail != null)
                {
                    stkdetail.GDW_CODE = Convert.ToInt32(ddlGDW);
                    stkdetail.Artical_CODE = Convert.ToInt32(ddlART);
                    stkdetail.Size_Code = Convert.ToInt32(ddlSize);
                    stkdetail.Item_Code = Convert.ToInt32(ddlITEM);
                    stkdetail.Stk_Qty_IN = STK_QTY_IN;
                    dbContext.SaveChanges();
                }                

                foreach (var AMDetailModel in articalMergeViewModel.artical_Merge_DTL.ToList())
                {
                    var result = dbContext.Artical_Merge_DTL.SingleOrDefault(b => b.ID == AMDetailModel.ID);
                    if (result != null)
                    {
                        result.UDT_DATE = DateTime.Now;
                        result.UDT_UID = userManager.GetUserName(HttpContext.User);
                        result.STK_QTY_OUT = AMDetailModel.STK_QTY_OUT;
                        dbContext.SaveChanges();
                    }

                    var result2 = dbContext.StockDTL_Models.Where(s => s.Tran_Table_PK == AMDetailModel.ID && s.Tran_Table == "Artical Merge Entry DTL").FirstOrDefault();
                    if (result2 != null)
                    {
                        result2.Stk_Qty_OUT = AMDetailModel.STK_QTY_OUT;
                        dbContext.SaveChanges();
                    }
                }
                return RedirectToAction("Artical_Merge_Detail");
            }
            else
            {
                return View(articalMergeViewModel);
            }
        }

        [HttpGet]
        public IActionResult Action_AM(int id)
        {
            Artical_Merge_HDR artical_Merge_HDR = new Artical_Merge_HDR();
            artical_Merge_HDR = dbContext.Artical_Merge_HDR.Where(am => am.ID == id).FirstOrDefault();
            ArticalMergeViewModel ArtMrgmodel = new ArticalMergeViewModel();
            ArtMrgmodel.GDWDropDown = GDWlists();
            ArtMrgmodel.ARTDropDown = Artlists();
            ArtMrgmodel.SIZEDropDown = Sizelists();
            ArtMrgmodel.ITEMDropDown = ITEMlists();
            ArtMrgmodel.artical_Merge_HDR = artical_Merge_HDR;

            ArtMrgmodel.artical_Merge_DTL = dbContext.Artical_Merge_DTL.Where(ad => ad.HDR_FK == id).ToList();
            foreach (var item in ArtMrgmodel.artical_Merge_DTL)
            {
                item.ARTICAL_NAME = dbContext.Artical_Master.Where(a => a.ID == item.ARTICAL_CODE).Select(a => a.NAME).FirstOrDefault();
                item.SIZE_NAME = dbContext.Size_Master.Where(a => a.ID == item.SIZE_CODE).Select(a => a.NAME).FirstOrDefault();
                item.ITEM_NAME = dbContext.Item_Master.Where(a => a.ID == item.ITEM_CODE).Select(a => a.NAME).FirstOrDefault();
                item.GDW_NAME = dbContext.Godown_Master.Where(a => a.ID == item.GDW_CODE).Select(a => a.NAME).FirstOrDefault();
            }
            ArtMrgmodel.Type = "View";
            return View("Edit_AM", ArtMrgmodel);
        }

        [HttpGet]
        public IActionResult Delete_AM(int ID)
        {
            var HDRdata = dbContext.Artical_Merge_HDR.Where(D => D.ID == ID).FirstOrDefault();
            dbContext.Artical_Merge_HDR.Remove(HDRdata);
            dbContext.SaveChanges();

            var HDRSTkdata = dbContext.StockDTL_Models.Where(D => D.Tran_Table_PK == ID && D.Tran_Table == "Artical Merge Entry HDR").FirstOrDefault();
            dbContext.StockDTL_Models.Remove(HDRSTkdata);
            dbContext.SaveChanges();

            var data = dbContext.Artical_Merge_DTL.Where(D => D.HDR_FK == ID).ToList();
            foreach (var Datas in data)
            {
                dbContext.Artical_Merge_DTL.Remove(Datas);

                var stkdtldata = dbContext.StockDTL_Models.Where(dd => dd.Tran_Table_PK == Datas.ID && dd.Tran_Table == "Artical Merge Entry DTL").FirstOrDefault();
                dbContext.StockDTL_Models.Remove(stkdtldata);
            }            
            dbContext.SaveChanges();

            return RedirectToAction("Artical_Merge_Detail");
        }
    }
}