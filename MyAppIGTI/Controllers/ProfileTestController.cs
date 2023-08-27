using Microsoft.AspNetCore.Mvc;
using MyAppIGTI.DBRepo;
using MyAppIGTI.Models;

namespace MyAppIGTI.Controllers
{
    public class ProfileTestController : Controller
    {
        private readonly IProfileTestRepo _IProfileTestRepo;
        public ProfileTestController(IProfileTestRepo profileTestRepo)
        {
            _IProfileTestRepo = profileTestRepo;
        }

        public IActionResult Index()
        {
            List<ProfileTestModel> listProfileTest = _IProfileTestRepo.GetAllProfileTest();
            return View(listProfileTest);
        }

        public IActionResult AddProfileTest()
        {
            return View();
        }

        public IActionResult EditProfileTest(int id)
        {
            ProfileTestModel profileTest = _IProfileTestRepo.GetProfileTest(id);
            return View(profileTest);
        }

        public IActionResult DeleteProfileTest(int id)
        {
            ProfileTestModel profileTest = _IProfileTestRepo.GetProfileTest(id);
            return View(profileTest);
        }

        [HttpPost]
        public IActionResult AddProfileTest(ProfileTestModel profileTest)
        {
            _IProfileTestRepo.InsertProfileTest(profileTest);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditProfileTest(ProfileTestModel profileTest)
        {
            _IProfileTestRepo.UpdateProfileTest(profileTest);
            return RedirectToAction("Index");
        }
                
        public IActionResult DeleteProfileTestOk(int id)
        {
            _IProfileTestRepo.DeleteProfileTest(id);
            return RedirectToAction("Index");
        }
    }
}
