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

        public IList<Tarea> Tarea { get; set; } = new List<Tarea>();



        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5; // Por defecto 5 tareas por página

        public int TotalPages { get; set; }
        public async Task OnGetAsync()
        {
            int totalTareas = await _context.Tareas.CountAsync();
            TotalPages = (int)Math.Ceiling(totalTareas / (double)PageSize);

            Tarea = await _context.Tareas
                .OrderBy(t => t.fechaVencimiento)
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
    }
}
