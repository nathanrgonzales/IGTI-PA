using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyAppIGTI.DBRepo;
using MyAppIGTI.Models;
using System;

namespace MyAppIGTI.Controllers
{
    public class ResultTestController : Controller
    {
        private readonly IProfileTestRepo _IProfileTestRepo;

        public ResultTestController(IProfileTestRepo profileTestRepo)
        {
            _IProfileTestRepo = profileTestRepo;
        }

        public IActionResult Index()
        {
            List<ResultTestModel> listResultTest = _IProfileTestRepo.GetAllResultTest();
            return View(listResultTest);            
        }
        
        public IActionResult StartResultTest(int id)
        {
            ResultTestModel resultTest = _IProfileTestRepo.GetResultTestbyProfile(id);
            return View(resultTest);
        }

        public IActionResult StartResultTestOk(int id)
        {
            ResultTestModel resultTest = _IProfileTestRepo.GetResultTestbyProfile(id);
            if (resultTest.Id < 0)
            {
                resultTest.Status = "Running - started in " + DateTime.Now.ToString();
                resultTest.IdStatus = 1;
                _IProfileTestRepo.InsertResultTest(resultTest);
                return RedirectToAction("Index");
            }
            else
            {
                resultTest.Status = "Running - started in " + DateTime.Now.ToString();                
            }
            return RedirectToAction("Index");
        }
    }
}
