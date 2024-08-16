using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Price_lists_CRM.Models;

namespace Price_lists_CRM.Controllers
{
    public class PricelistController : Controller
    {
        private readonly ApplicationContext _context;

        public PricelistController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Pricelist
        public async Task<IActionResult> Index()
        {
              return _context.Pricelists != null ? 
                          View(await _context.Pricelists.ToListAsync()) :
                          Problem("Entity set 'ApplicationContext.Pricelists'  is null.");
        }

        // GET: Pricelist/Details/1
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pricelists == null)
            {
                return NotFound();
            }

            // include columns
            PricelistModel pricelist = await _context.Pricelists
                .Include(p => p.Columns)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pricelist == null)
            {
                return NotFound();
            }

            return View(pricelist);
        }

        // GET: Pricelist/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pricelist/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PricelistModel pricelist)
        {
            foreach (ColumnModel column in pricelist.Columns)
            {
                column.Value = column.Value ?? string.Empty;
                column.PricelistId = pricelist.Id;
                column.Pricelist = pricelist;
            }

            _context.Add(pricelist);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Pricelist/Edit/1
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pricelists == null)
            {
                return NotFound();
            }

            // include columns
            PricelistModel pricelist = await _context.Pricelists
                .Include(p => p.Columns) 
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pricelist == null)
            {
                return NotFound();
            }

            return View(pricelist);
        }

        // POST: Pricelist/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PricelistModel pricelist)
        {
            // get existing pricelists & columns
            PricelistModel existingPricelist = await _context.Pricelists
                .Include(p => p.Columns)
                .FirstOrDefaultAsync(p => p.Id == pricelist.Id);

            if (existingPricelist == null)
            {
                return NotFound();
            }

            // add & update columns
            List<int> existingColumnIds = existingPricelist.Columns.Select(c => c.Id).ToList();

            foreach (ColumnModel column in pricelist.Columns)
            {
                // if column exist, update it
                if (existingColumnIds.Contains(column.Id))
                {
                    ColumnModel existingColumn = existingPricelist.Columns
                        .First(c => c.Id == column.Id);
                    existingColumn.Title = column.Title;
                    existingColumn.Value = string.IsNullOrEmpty(column.Value) ? existingColumn.Value : column.Value;
                    existingColumn.Type = column.Type;
                }
                else
                {
                    // if column is new - create it & add
                    ColumnModel newColumn = new ColumnModel
                    {
                        Title = column.Title,
                        Value = string.IsNullOrEmpty(column.Value) ? string.Empty : column.Value,
                        Type = column.Type,
                        PricelistId = pricelist.Id
                    };
                    existingPricelist.Columns.Add(newColumn);
                }
            }

            // delete columns, that were on form
            List<int> columnIdsToKeep = pricelist.Columns.Select(c => c.Id).ToList();
            List<ColumnModel> columnsToRemove = existingPricelist.Columns
                .Where(c => !columnIdsToKeep.Contains(c.Id))
                .ToList();

            foreach (ColumnModel column in columnsToRemove)
            {
                _context.Columns.Remove(column);
            }

            // update pl if it has been changed
            _context.Update(existingPricelist);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = pricelist.Id });
        }

        // GET: Pricelist/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pricelists == null)
            {
                return NotFound();
            }

            PricelistModel pricelist = await _context.Pricelists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pricelist == null)
            {
                return NotFound();
            }

            return View(pricelist);
        }

        // POST: Pricelist/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pricelists == null)
            {
                return Problem("Entity set 'ApplicationContext.Pricelists'  is null.");
            }
            PricelistModel pricelist = await _context.Pricelists.FindAsync(id);
            if (pricelist != null)
            {
                _context.Pricelists.Remove(pricelist);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PricelistModelExists(int id)
        {
          return (_context.Pricelists?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
