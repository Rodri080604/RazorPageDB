using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WaRazorDB.Data;
using WaRazorDB.Models;

namespace WaRazorDB.Pages
{
    public class CreateModel : PageModel
    {
        private readonly WaRazorDB.Data.TareaDbContext _context;

        public CreateModel(WaRazorDB.Data.TareaDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Tarea Tarea { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            if (!Regex.IsMatch(Tarea.nombreTarea, @"^[\p{L}\p{N}]+(\s+[\p{L}\p{N}]+)+$"))
            {
                ModelState.AddModelError("Tarea.nombreTarea", "El nombre debe contener al menos dos palabras con letras y números, sin símbolos especiales.");
                return Page();
            }

            // Dividir el nombre en palabras
            var palabras = Tarea.nombreTarea.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (palabras.Length < 2)
            {
                ModelState.AddModelError("Tarea.nombreTarea", "El nombre debe contener al menos dos palabras.");
                return Page();
            }

            // Evitar letras repetidas muchas veces seguidas (por ejemplo, más de 3)
            if (Regex.IsMatch(Tarea.nombreTarea, @"([A-Za-z0-9])\1{2,}"))
            {
                ModelState.AddModelError("Tarea.nombreTarea", "No se permiten letras repetidas muchas veces consecutivas.");
                return Page();
            }


            // Validación de la fecha (no puede ser menor a la actual)
            if (Tarea.fechaVencimiento < DateTime.Today)
            {
                ModelState.AddModelError("Tarea.fechaVencimiento", "La fecha no puede ser menor a la actual.");
                return Page();
            }

            // Estado fijo en creación
            Tarea.estado = "Pendiente";



            _context.Tareas.Add(Tarea);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
