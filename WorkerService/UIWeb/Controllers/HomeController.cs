using System;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UIWeb.Models;

namespace UIWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly DataContext _dataContext;

        public HomeController(
            DataContext dataContext
            )
        {
            _dataContext = dataContext;
        }

        public IActionResult Index()
        {
            TempData["ErrorDetail"] = null;
            try
            {
                var result = _dataContext.CheckApp;
                var newData = result.Select(DTO).ToList();
                return View(newData);
            }
            catch (Exception ex)
            {
                TempData["ErrorDetail"] = ex.Message;
                return View("Index");
            }
        }

        public async Task<IActionResult> Details(int? Id)
        {
            if (Id == null)
            {
                TempData["ErrorDetail"] = "Uygulama bulunamadı!";
                return RedirectToAction(nameof(Index));
            }

            var data = await _dataContext.CheckApp
                .FirstOrDefaultAsync(m => m.Id == Id);
            if (data == null)
            {
                TempData["ErrorDetail"] = "Uygulama bulunamadı!";
                return RedirectToAction(nameof(Index));
            }
            return View(data);
        }

        private CheckAppViewModel DTO(CheckApp data)
        {
            if (data == null) return null;
            return new CheckAppViewModel { AppName = data.AppName, AppUrl = data.AppUrl, Id = data.Id, Interval = data.Interval };
        }

        [HttpGet]
        public IActionResult CreateUpdate(int? Id)
        {
            TempData["ErrorDetail"] = null;
            try
            {
                if (Id != null && Id > 0)
                {
                    var result = _dataContext.CheckApp.FirstOrDefault(x => x.Id == Id);
                    if (result == null)
                    {
                        TempData["ErrorDetail"] = "Uygulama bulunamadı!";
                        return RedirectToAction(nameof(Index));
                    }
                    return View(DTO(result));
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorDetail"] = ex.Message;
                return View("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUpdate(CheckAppViewModel model)
        {
            TempData["ErrorDetail"] = null;
            if (!ModelState.IsValid)
                return View(model);
            try
            {
                bool isUpdate = false;
                if (model.Id != null && model.Id > 0)
                    isUpdate = true;

                CheckApp data = new CheckApp();

                if (isUpdate)
                {
                    data = _dataContext.CheckApp.FirstOrDefault(x => x.Id == model.Id);
                    if (data == null)
                    {
                        TempData["ErrorDetail"] = "Uygulama bulunamadı!";
                        return RedirectToAction(nameof(Index));
                    }
                }

                data.AppName = model.AppName;
                data.AppUrl = model.AppUrl;
                data.Interval = model.Interval;

                if (!isUpdate)
                    _dataContext.Add(data);
                _dataContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorDetail"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null)
            {
                TempData["ErrorDetail"] = "Uygulama bulunamadı!";
                return RedirectToAction(nameof(Index));
            }

            var result = await _dataContext.CheckApp
                .FirstOrDefaultAsync(m => m.Id == Id);
            if (result == null)
            {
                TempData["ErrorDetail"] = "Uygulama bulunamadı!";
                return RedirectToAction(nameof(Index));
            }

            return View(DTO(result));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int Id)
        {
            var data = await _dataContext.CheckApp.FindAsync(Id);
            if (data == null)
            {
                TempData["ErrorDetail"] = "Uygulama bulunamadı!";
                return RedirectToAction(nameof(Index));
            }
            _dataContext.CheckApp.Remove(data);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
