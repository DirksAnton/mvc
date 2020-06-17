using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CL_LB1.Data;
using CL_LB1.Entities;

namespace WB_MVC
{
    public class IndexModel : PageModel
    {
        private readonly CL_LB1.Data.ApplicationDbContext _context;

        public IndexModel(CL_LB1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Dish> Dish { get;set; }

        public async Task OnGetAsync()
        {
            Dish = await _context.Dishes
                .Include(d => d.Group).ToListAsync();
        }
    }
}
