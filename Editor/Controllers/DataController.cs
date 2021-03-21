using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using Repositories.Data;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace Editor.Controllers
{
    public class DataController : Controller
    {
        private readonly IDataRepository _datarepository;
        private readonly DataContext _datacontext;
        private readonly ILogger<DataController> _logger;

        public DataController(IDataRepository datarepository, DataContext datacontext, ILogger<DataController> logger)
        {
            _datarepository = datarepository;
            _datacontext = datacontext;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _datarepository.GetTablesAsync(_datacontext.ConnectionString));
        }

        public IActionResult Data(string tableName)
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

            var data = _datarepository.GetRow(tableName, name, id);

            if (data is null)
            {
                return NotFound();
            }

            return View(data);
        }

        public async Task<IActionResult> Edit(string tableName, string pkColumnName, string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.TableName = tableName;
            DataSet model = _datarepository.GetRow(tableName, pkColumnName, id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRecord([FromQuery]string tableName, string columnName, string id)
        {
	        DataSet dataSet = _datarepository.GetRow(tableName, columnName, id);

	        //get columns from dataset
	        Dictionary<string, string> tableValues = new Dictionary<string, string>();
	        foreach (DataColumn column in dataSet.Tables[0].Columns)
	        {
		        string name = column.Caption;
		        string value = Request.Form[name].ToString();

		        tableValues.Add(name, value);
	        }

	        await _datarepository.UpdateAsync(tableName, tableValues);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        await _datarepository.UpdateAsync(tableName, new DataModel { });
            //    }

            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (_datarepository.GetRow(tableName, "name", id) is null)
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }

            //    return RedirectToAction(nameof(Index));
            //}

            //return View();
        }

        public async Task<IActionResult> Create(string tableName)
        {
            ViewBag.TableName = tableName;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DataModel dataModel, string tableName, List<object> values)
        {
            if (ModelState.IsValid)
            {
                _datarepository.Create(dataModel, tableName);
                return RedirectToAction(nameof(Index));
            }

            return View(dataModel);
        }

        public async Task<IActionResult> Delete(string tableName, string name, string columnType, int? id)
        {
            ViewBag.TableName = tableName;

            if (id == null)
            {
                return NotFound();
            }

            var data = _datarepository.GetRow(tableName, name, id);

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
            ViewBag.TableName = tableName;
            await _datarepository.DeleteRowAsync(tableName, name, columnType, id);
            return RedirectToAction("Data", "Data", new { tableName = tableName});
        }
    }
}
