using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repositories.Data;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

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

        #region Data from Db and tables

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

        public async Task<IActionResult> Details(string tableName, string pkColumnName, string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = _datarepository.GetRow(tableName, pkColumnName, id);

            if (data is null)
            {
                return NotFound();
            }

            return View(data);
        }

        #endregion

        #region Edit GET/POST
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

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRecord([FromQuery] string tableName, string pkColumnName, string id)
        {
	        id ??= "-1";

            DataSet dataSet = _datarepository.GetRow(tableName, pkColumnName, id);

            Dictionary<string, string> tableValues = new Dictionary<string, string>();

            for (int i = 1; i < dataSet.Tables[0].Columns.Count; i++)
            {
                string name = dataSet.Tables[0].Columns[i].ColumnName;
                string value = Request.Form[name].ToString();

                tableValues.Add(name, value);
            }

            if (int.TryParse(id, out int idInt) && idInt == -1)
            {
	            await _datarepository.CreateAsync(tableValues, tableName);
            }
            else
            {
	            await _datarepository.UpdateAsync(tableName, pkColumnName, id, tableValues);
            }

            return RedirectToAction("Data", "Data", new { tableName = tableName });
        }
        #endregion

        public async Task<IActionResult> Create(string tableName, string pkColumnName, string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.TableName = tableName;
            DataSet dataSet = _datarepository.GetRow(tableName, pkColumnName, id);
            Dictionary<DataColumn, string> tableValues = new Dictionary<DataColumn, string>();

            for (int i = 1; i < dataSet.Tables[0].Columns.Count; i++)
            {
                DataColumn column = dataSet.Tables[0].Columns[i];
                string value = "";

                tableValues.Add(column, value);
            }

            return View(tableValues);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Dictionary<DataColumn, string> tableValues, string tableName)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            int index = 0;
            foreach (var item in tableValues)
            {
                if (index == 0)
                {
                    continue;
                }

                if (index == tableValues.Count - 1)
                {
                    break;
                }

                string name = item.Key.ColumnName;
                string value = Request.Form[name].ToString();

                values.Add(name, value);

                index++;
            }

            if (ModelState.IsValid)
            {
                await _datarepository.CreateAsync(values, tableName);
            }

            return RedirectToAction("Data", "Data", new { tableName = tableName });
        }

        #region Delete GET/POST
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
            return RedirectToAction("Data", "Data", new { tableName = tableName });
        }
        #endregion
    }
}
