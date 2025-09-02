using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WaRazorDB.Data;
using WaRazorDB.Models;

namespace WaRazorDB.Pages
{
    public class EditModel : PageModel
    {
        private readonly WaRazorDB.Data.TareaDbContext _context;

        public EditModel(WaRazorDB.Data.TareaDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Tarea Tarea { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarea =  await _context.Tareas.FirstOrDefaultAsync(m => m.Id == id);
            if (tarea == null)
            {
                return NotFound();
            }
            Tarea = tarea;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Validación de palabras mínimas y solo letras/números (incluyendo acentos)
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

            // Evitar letras repetidas muchas veces seguidas
            if (Regex.IsMatch(Tarea.nombreTarea, @"([A-Za-z0-9])\1{2,}"))
            {
                ModelState.AddModelError("Tarea.nombreTarea", "No se permiten letras repetidas muchas veces consecutivas.");
                return Page();
            }

            // Validación de la fecha
            if (Tarea.fechaVencimiento < DateTime.Today)
            {
                ModelState.AddModelError("Tarea.fechaVencimiento", "La fecha no puede ser menor a la actual.");
                return Page();
            }

            _context.Attach(Tarea).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TareaExists(Tarea.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TareaExists(int id)
        {
            return _context.Tareas.Any(e => e.Id == id);
        }
    }
}
