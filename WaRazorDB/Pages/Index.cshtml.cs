using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WaRazorDB.Data;
using WaRazorDB.Models;

namespace WaRazorDB.Pages
{
    public class IndexModel : PageModel
    {
        private readonly WaRazorDB.Data.TareaDbContext _context;

        public IndexModel(WaRazorDB.Data.TareaDbContext context)
        {
            _context = context;
        }

        public IList<Tarea> Tarea { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Tarea = await _context.Tareas.ToListAsync();
        }
    }
}
