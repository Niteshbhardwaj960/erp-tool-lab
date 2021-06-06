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
    [Authorize(Roles = "MFG , Admin")]
    public class AgentCommRateController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public AgentCommRateController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }
        [HttpGet]
        public IActionResult AgentCommRate_Master()
        {
            AgentCommRate agentCommRate = new AgentCommRate();
            agentCommRate.Type = "Add";
            agentCommRate.ACCDropDown = ACClists();
            return View(agentCommRate);
        }
        public List<SelectListItem> ACClists()
        {
            var AccList = (from Acc in dbContext.Account_Masters.ToList()
                           select new SelectListItem()
                           {
                               Text = Acc.NAME,
                               Value = Acc.ID.ToString(),
                           }).ToList();

            AccList.Insert(0, new SelectListItem()
            {
                Text = "Select Account",
                Value = string.Empty,
                Selected = true
            });
            return AccList;
        }
        [HttpGet]
        public IActionResult AgentCommRateDetails()
        {
            List<AgentCommRate> agentCommRates = new List<AgentCommRate>();
            agentCommRates = dbContext.AgentCommRate.AsNoTracking().ToList();
            foreach (var item in agentCommRates)
            {
                if (item.QTY_AMOUNT_TAG == "Q")
                {
                    item.QTY_AMOUNT_TAG = "Qty";
                }
                else
                {
                    item.QTY_AMOUNT_TAG = "Amount";
                }
            }
            return View("AgentCommRateDetails", agentCommRates);
        }
        [HttpPost]
        public IActionResult AgentCommRate_Master(AgentCommRate agentCommRate)
        {
            var accname = dbContext.Account_Masters.Where(a => a.ID == agentCommRate.ACC_CODE).Select(aa => aa.NAME).FirstOrDefault();
            agentCommRate.INS_DATE = Helper.DateFormatDate(Convert.ToString(DateTime.Now));
            agentCommRate.INS_UID = userManager.GetUserName(HttpContext.User);
            agentCommRate.ACC_CODE = agentCommRate.ACC_CODE;
            agentCommRate.ACC_NAME = accname;
            dbContext.AgentCommRate.Add(agentCommRate);
            dbContext.SaveChanges();
            return RedirectToAction("AgentCommRateDetails");
        }
        [HttpGet]
        public IActionResult EditACR(int id)
        {
            AgentCommRate agentCommRate = new AgentCommRate();
            agentCommRate = dbContext.AgentCommRate.Where(r => r.ID == id).FirstOrDefault();
            agentCommRate.Type = "Edit";
            agentCommRate.ACCDropDown = ACClists();
            return View("AgentCommRate_Master", agentCommRate);
        }
        [HttpPost]
        public IActionResult SAVEACR(AgentCommRate agentCommRate)
        {
            var accname = dbContext.Account_Masters.Where(a => a.ID == agentCommRate.ACC_CODE).Select(aa => aa.NAME).FirstOrDefault();
            var result = dbContext.AgentCommRate.SingleOrDefault(b => b.ID == agentCommRate.ID);
            if (result != null)
            {
                result.UDT_DATE = DateTime.Now; Helper.DateFormatDate(Convert.ToString(DateTime.Now)); result.UDT_UID = userManager.GetUserName(HttpContext.User);
                result.ACC_CODE = agentCommRate.ACC_CODE;
                result.ACC_NAME = accname;
                result.COMM_RATE = agentCommRate.COMM_RATE;
                result.QTY_AMOUNT_TAG = agentCommRate.QTY_AMOUNT_TAG;
                dbContext.SaveChanges();
            }
            return RedirectToAction("AgentCommRateDetails");
        }
        [HttpGet]
        public IActionResult ActionACR(int id)
        {
            AgentCommRate agentCommRate = new AgentCommRate();
            agentCommRate = dbContext.AgentCommRate.Where(r => r.ID == id).FirstOrDefault();
            agentCommRate.Type = "Action";
            agentCommRate.ACCDropDown = ACClists();
            return View("AgentCommRate_Master", agentCommRate);
        }
        [HttpGet]
        public IActionResult DeleteACR(int ID)
        {
            var data = dbContext.AgentCommRate.Where(D => D.ID == ID).FirstOrDefault();
            dbContext.AgentCommRate.Remove(data);
            dbContext.SaveChanges();
            return RedirectToAction("AgentCommRateDetails");
        }
    }
}