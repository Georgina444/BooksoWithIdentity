using Bookso.Models;
using Bookso.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Bookso.DataAccess.Repository.IRepository;

namespace BooksoWeb.Areas.Admin.Controllers;

public class CategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryController(IUnitOfWork unitOfWork)    //This db has all the implementations of the ConnectionStrings and tables needed to retreive the data
    {   // populates our local dbObject with this implementation
        _unitOfWork = unitOfWork;
        // _db is used to retreive our categories
    }

    public IActionResult Index()
    {
        // var objCategoryList = _db.Categories.ToList();    //_db.Categories (retreives all the Categories from the Db)
        IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
        return View(objCategoryList);
    }

    //GET
    public IActionResult Create()
    {
        // var objCategoryList = _db.Categories.ToList();    //_db.Categories (retreives all the Categories from the Db)
        return View();
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]  //prevents CSRF(Cross-site request forgery) attacts by encrypting 
    public IActionResult Create(Category obj)   // we are receiving a Category object as a parameter
    {
        if (obj.Name == obj.DisplayOrder.ToString())
        {
            ModelState.AddModelError("name", "The display order cannot exactly match the Name!");
        }
        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Add(obj);  // creates the record inside the database | (obj is a category entity)
            _unitOfWork.Save();
            TempData["success"] = "Category created successfully";
            return RedirectToAction("Index");
        }
        return View(obj);
    }


    //GET
    public IActionResult Edit(int id) // based on the id we will retrieve the Category details and display them
    {

        // var categoryFromDb = _db.Categories.Find(id);
        var categoryFromDbFirst = _unitOfWork.Category.GetFirstOrDefault(u => u.CategoryID == id);
        //          var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.CategoryID == id);      

        if (categoryFromDbFirst == null)
        {
            return NotFound();
        }

        return View(categoryFromDbFirst);
    }

    //POST
    [HttpPost, ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Category obj)   // we are receiving a Category object as a parameter
    {
        if (obj.Name == obj.DisplayOrder.ToString())
        {
            ModelState.AddModelError("name", "The display order cannot exactly match the name!");
        }
        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Attach(obj);
            _unitOfWork.Category.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category updated successfully";
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    //GET
    public IActionResult Delete(int? id) // based on the id we will retrieve the Category details and display them
    {

        if (id == null || id == 0)
        {
            return NotFound();
        }
        var categoryFromDbFirst = _unitOfWork.Category.GetFirstOrDefault(u => u.CategoryID == id);

        if (categoryFromDbFirst == null)
        {
            return NotFound();
        }

        return View(categoryFromDbFirst);
    }

    //POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePOST(int? id)   // we are receiving a Category object as a parameter
    {
        var obj = _unitOfWork.Category.GetFirstOrDefault(u => u.CategoryID == id);

        if (obj == null)
        {
            return NotFound();
        }

        _unitOfWork.Category.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "Category deleted successfully";

        return RedirectToAction("Index");
    }
}


// 1.2.1 We need an implementation of the ApplicationDbContext (where the connection to DB is already made)
// 1.2.2 Add ctor (we get the implementation here)