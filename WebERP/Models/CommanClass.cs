using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebERP.Data;

namespace WebERP.Models
{
    public class CommanClass
    {
        public readonly ApplicationDbContext dbContext;
        public List<SelectListItem> Brandlists()
        {
            var BrandList = (from Brand in dbContext.Brand_Master
                             select new SelectListItem()
                             {
                                 Text = Brand.NAME,
                                 Value = Brand.ID.ToString(),
                             }).ToList();

            BrandList.Insert(0, new SelectListItem()
            {
                Text = "Select Brand",
                Value = string.Empty,
                Selected = true
            });
            return BrandList;
        }
    }
}
