using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace Editor.Controllers
{
    public class DataController : Controller
    {
        private readonly IDataRepository _datarepository;
        private readonly DataContext _datacontext;

        public DataController(IDataRepository datarepository, DataContext datacontext)
        {
            _datarepository = datarepository;
            _datacontext = datacontext;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _datarepository.GetTablesAsync(_datacontext._connectionString));
        }

        public async Task<IActionResult> Data(string tableName)
        {
            var data = _datarepository.GetTableData(tableName);
            ViewBag.TableName = tableName;
            return View(data);
        }

        public async Task<IActionResult> Details(string tableName, string name, string columnType, int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = _datarepository.GetRow(tableName, name, columnType, id);

            if (data is null)
            {
                return NotFound();
            }

            return View(data);
        }

        public async Task<IActionResult> Edit(object id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string tableName, string name, string columnType, int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _datarepository.UpdateAsync(null);
                }

                catch (DbUpdateConcurrencyException)
                {
                    if (_datarepository.GetRow(tableName, name, columnType, id) is null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Delete(string tableName, string name, string columnType, int id)
        {
            ViewBag.TableName = tableName;

            if (id == null)
            {
                return NotFound();
            }
            
            var data = _datarepository.GetRow(tableName, name, columnType, id);

            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string tableName, string name, string columnType, int id)
        {
            await _datarepository.DeleteRowAsync(tableName, name, columnType, id);
            return RedirectToAction(nameof(Data));
        }
    }
}
