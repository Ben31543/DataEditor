using Microsoft.AspNetCore.Mvc;
using Models;
using Repositories.Data;
using Repositories.Interfaces;
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
            var data = new DataModel
            {
                TableData = await _datarepository.GetAllDataAsync(tableName)
            };

            return View(data);
        }
    }
}
