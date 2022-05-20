using Microsoft.AspNetCore.Mvc;
using MyApp.DataAccessLayer;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models;
using MyApp.Models.ViewModels;

namespace MyApp.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            CategoryVM categoryVM = new CategoryVM();

            categoryVM.categories = _unitOfWork.Category.GetAll();
            return View(categoryVM.categories);
        }

//       /* [HttpGet]
//        public IActionResult Create()
//        {
//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Create(Category category)
//        {
//            if (ModelState.IsValid)
//            {
//                _unitOfWork.Category.Add(category);
                
//                TempData["Success"] = "Category Created Done!";
//                return RedirectToAction("Index");
//            }
//            return View(category);
//        }
//*/

        [HttpGet]
        public IActionResult CreateUpdate(int? id)
        {
            CategoryVM vm = new CategoryVM();
            if (id == null || id == 0)
            {
                return View(vm);
            }
            else
            {
               vm.Category = _unitOfWork.Category.GetT(x => x.Id == id);
                if (vm.Category == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(vm);

                }
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUpdate(CategoryVM vm)
        {

            if (ModelState.IsValid)
            {
                if(vm.Category.Id == 0)
                {
                    _unitOfWork.Category.Add(vm.Category);
                    TempData["Success"] = "Category Created Done!";

                }
                else
                {
                    _unitOfWork.Category.Update(vm.Category);
                    TempData["Success"] = "Category Updated Done!";

                }

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _unitOfWork.Category.GetT(x=>x.Id==id);
            if (category == null)
            {
                return NotFound();
            }
            

            return View(category);
        }


        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteData(int? id )
        {
            var category = _unitOfWork.Category.GetT(x=>x.Id==id);
            if(category==null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Delete(category);
            
                TempData["Success"] = "Category Deleted Done!";

            return RedirectToAction("Index");
        }
    }
}
