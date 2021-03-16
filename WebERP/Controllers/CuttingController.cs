using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebERP.Data;
using WebERP.Models;

namespace WebERP.Controllers
{
    public class CuttingController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public CuttingController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }
        [HttpGet]
        public IActionResult CuttingDetail()
        {
            List<V_CuttingDetail> cut = new List<V_CuttingDetail>();
            cut = dbContext.V_CuttingDetail.ToList();
            return View(cut);
        }
        [HttpGet]
        public IActionResult AddCutting()
        {
            Cutting_Order cut = new Cutting_Order();
            cut.EmpDropDown = Emplists();
            int DoC_No = dbContext.Cutting_Orders
                .Select(p => Convert.ToInt32(p.DOC_NO)).DefaultIfEmpty(0).Max();
            cut.ArticalDropDown = Articallists();
            cut.ContEmpDropDown = ContEmplists();
            cut.ItemDropDown = Itemlists();
            cut.ProcDropDown = Proclists();
            cut.SizeDropDown = Sizelists();
            cut.DOC_NO = DoC_No + 1;
            return View(cut);
        }

        [HttpPost]
        public IActionResult SaveCutting(Cutting_Order CuttingOrder)
        {
            if (ModelState.IsValid)
            {
                int DoC_No = dbContext.Cutting_Orders
                .Select(p => Convert.ToInt32(p.DOC_NO)).DefaultIfEmpty(0).Max();
                CuttingOrder.INS_DATE = DateTime.Now;
                CuttingOrder.INS_UID = userManager.GetUserName(HttpContext.User);
                CuttingOrder.DOC_NO = DoC_No + 1;
                dbContext.Cutting_Orders.Add(CuttingOrder);
                dbContext.SaveChanges();
                return RedirectToAction("CuttingDetail");
            }
            else
            {
                CuttingOrder.EmpDropDown = Emplists();
                CuttingOrder.ArticalDropDown = Articallists();
                CuttingOrder.ContEmpDropDown = ContEmplists();
                CuttingOrder.ItemDropDown = Itemlists();
                CuttingOrder.ProcDropDown = Proclists();
                CuttingOrder.SizeDropDown = Sizelists();
                return View("AddCutting", CuttingOrder);
            }
        }
        public List<SelectListItem> Emplists()
        {
            var EmpList = (from Emp in dbContext.Employee_Masters.Where(e => e.EMP_TYPE == "P").ToList()
                           select new SelectListItem()
                           {
                               Text = Emp.EMP_NAME,
                               Value = Convert.ToString(Emp.EMP_CODE),
                           }).ToList();

            EmpList.Insert(0, new SelectListItem()
            {
                Text = "Select Employee",
                Value = string.Empty,
                Selected = true
            });
            return EmpList;
        }
        public List<SelectListItem> ContEmplists()
        {
            var EmpList = (from Emp in dbContext.Employee_Masters.Where(e => e.EMP_TYPE == "C").ToList()
                           select new SelectListItem()
                           {
                               Text = Emp.EMP_NAME,
                               Value = Convert.ToString(Emp.EMP_CODE),
                           }).ToList();

            EmpList.Insert(0, new SelectListItem()
            {
                Text = "Select Cont Employee",
                Value = string.Empty,
                Selected = true
            });
            return EmpList;
        }
        public List<SelectListItem> Itemlists()
        {
            var ItemList = (from Item in dbContext.Item_Master.ToList()
                           select new SelectListItem()
                           {
                               Text = Item.NAME,
                               Value = Convert.ToString(Item.ID),
                           }).ToList();

            ItemList.Insert(0, new SelectListItem()
            {
                Text = "Select Item",
                Value = string.Empty,
                Selected = true
            });
            return ItemList;
        }
        public List<SelectListItem> Articallists()
        {
            var ArtList = (from Art in dbContext.Artical_Master.ToList()
                           select new SelectListItem()
                           {
                               Text = Art.NAME,
                               Value = Convert.ToString(Art.ID),
                           }).ToList();

            ArtList.Insert(0, new SelectListItem()
            {
                Text = "Select Artical",
                Value = string.Empty,
                Selected = true
            });
            return ArtList;
        }
        public List<SelectListItem> Sizelists()
        {
            var SizeList = (from Size in dbContext.Size_Master.ToList()
                           select new SelectListItem()
                           {
                               Text = Size.NAME,
                               Value = Convert.ToString(Size.ID),
                           }).ToList();

            SizeList.Insert(0, new SelectListItem()
            {
                Text = "Select Size",
                Value = string.Empty,
                Selected = true
            });
            return SizeList;
        }
        public List<SelectListItem> Proclists()
        {
            var ProcList = (from Proc in dbContext.Process_Master.ToList()
                           select new SelectListItem()
                           {
                               Text = Proc.NAME,
                               Value = Convert.ToString(Proc.ID),
                           }).ToList();

            ProcList.Insert(0, new SelectListItem()
            {
                Text = "Select Process",
                Value = string.Empty,
                Selected = true
            });
            return ProcList;
        }
    }
}