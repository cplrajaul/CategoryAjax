using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CategoryAjax.Data;
using CategoryAjax.Models;
using CategoryAjax.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace CategoryAjax.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CategoryController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }



        public async Task<IActionResult> AddOrEdit(int id=0)
        {
            if (id==0)
            {
                return View( new CategoryViewModel());
            }
            else
            {
                var category = await _context.Categories.FirstOrDefaultAsync(e => e.CategoryId == id);
                var CategoryViewModel = new CategoryViewModel()
                {
                    Id = category.CategoryId,
                    CategoryName = category.CategoryName,
                    Description = category.Description,
                    ExistingImage = category.ProfilePicture,
                    CreatedBy = category.CreatedBy,
                    CreatedTime = category.CreatedTime,
                    ModifiedBy = category.ModifiedBy,
                    ModifiedTime = category.ModifiedTime


                };

                if (category == null)
                {
                    return NotFound();
                }
                return View(CategoryViewModel);
            }
            
        }


       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, CategoryViewModel model)
        {


            if (ModelState.IsValid)
            {


                if (id == 0)
                {
                    string uniqueFileName = ProcessUploadedFile(model);
                    Category category = new()
                    {
                        CategoryName = model.CategoryName,
                        Description = model.Description,
                        ProfilePicture = uniqueFileName,
                        CreatedBy = model.CreatedBy,
                        ModifiedBy = model.ModifiedBy,
                        CreatedTime = model.CreatedTime,
                        ModifiedTime = model.ModifiedTime
                    };
                    _context.Add(category);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    try
                    {
                        var category = await _context.Categories.FindAsync(model.Id);
                        category.CategoryName = model.CategoryName;
                        category.Description = model.Description;
                        category.CreatedBy = model.CreatedBy;
                        category.CreatedTime = model.CreatedTime;
                        category.ModifiedBy = model.ModifiedBy;
                        category.ModifiedTime = model.ModifiedTime;

                        if (model.CategoryPicture != null)
                        {
                            if (model.ExistingImage != null)
                            {
                                string filePath = Path.Combine(_env.WebRootPath, "Uploads", model.ExistingImage);
                                System.IO.File.Delete(filePath);
                            }

                            category.ProfilePicture = ProcessUploadedFile(model);
                        }
                        _context.Update(category);
                        await _context.SaveChangesAsync();


                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CategoryExists(model.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }


                return Json(new { isvalid = true, html = Helper.RederRazorViewToString(this, "_ViewAll", _context.Categories.ToList()) });
            }
            return Json(new { isvalid = false, html = Helper.RederRazorViewToString(this, "AddOrEdit", model) });
        }

          


            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var category = await _context.Categories.FindAsync(id);
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            return Json(new { html = Helper.RederRazorViewToString(this, "_ViewAll", _context.Categories.ToList()) });
        }

            


            private string ProcessUploadedFile(CategoryViewModel model)
            {
                string uniqueFileName = null;

                if (model.CategoryPicture != null)
                {
                    string uploadsFolder = Path.Combine(_env.WebRootPath, "Uploads");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.CategoryPicture.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                model.CategoryPicture.CopyTo(fileStream);
            }

                return uniqueFileName;
            }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
    } 

