using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.Management;
using MyAppIGTI.DBRepo;
using MyAppIGTI.Models;
using MyAppIGTI.AppVariable; 
using System;
using System.Management.Automation;
using Humanizer;
using System.IO;

namespace MyAppIGTI.Controllers
{
    public class ResultTestController : Controller
    {
        private readonly IProfileTestRepo _IProfileTestRepo;
        private readonly IOptions<AppVariables> _options;

        public ResultTestController(IProfileTestRepo profileTestRepo, IOptions<AppVariables> options)
        {
            _IProfileTestRepo = profileTestRepo;
            _options = options;
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


            if (RunningPipelineAsync(resultTest))
            {
                resultTest.Status = "Last result in " + DateTime.Now.ToString();
                resultTest.IdStatus = 1; 
            }
            else
            {
                resultTest.Status = "Error in - " + DateTime.Now.ToString();
                resultTest.IdStatus = 2;
            }

            if (resultTest.Id < 0)
            {
                _IProfileTestRepo.InsertResultTest(resultTest);                
            }
            else
            {
                _IProfileTestRepo.UpdateResultTest(resultTest);
            }

            return RedirectToAction("Index");
        }

        internal bool RunningPipelineAsync(ResultTestModel oResultTest)
        {
            ProfileTestModel profileTest = _IProfileTestRepo.GetProfileTest(oResultTest.IdProfileTestModel);
            string omainPath = _options.Value.MainPath;
            string otestFolder = oResultTest.IdProfileTestModel.ToString("0000000000");

            if (!Directory.Exists(omainPath + "\\" + otestFolder))
            {
                try
                {
                    Directory.CreateDirectory(omainPath + "\\" + otestFolder);
                }
                catch
                {
                    throw new Exception("Cannot create the main folder");
                }
            }
            else 
            {
                try
                {
                    using var psDel = PowerShell.Create();
                    psDel.AddScript("cd " + omainPath).Invoke();
                    psDel.AddScript("Remove-Item " + omainPath + "\\" + otestFolder + "\\* -Recurse -Force").Invoke();
                    Thread.Sleep(2000);
                }
                finally 
                {
                    Directory.CreateDirectory(omainPath + "\\" + otestFolder);
                }
            }

            using var ps = PowerShell.Create();
            ps.AddScript("cd " + omainPath + "\\" + otestFolder).Invoke();
            
            ps.AddScript("git clone " + profileTest.RepoLink).Invoke();

            ps.AddScript("dotnet sonarscanner begin /o:\"igti-pa\" /k:\"igti-pa_igti-pa\" /d:sonar.host.url=\"https://sonarcloud.io\" /d:sonar.token=\"59bd70d6e28d37965f899481451f1e5df0ac27d5\" ").Invoke();

            var results = ps.AddScript("dotnet build C:\\SonarTest\\0000000001\\IGTI-PA\\MyAppIGTI.sln | Out-File \"Result"+ otestFolder + ".txt\"").Invoke();
            
            ps.AddScript("dotnet sonarscanner end /d:sonar.token=\"59bd70d6e28d37965f899481451f1e5df0ac27d5\"").Invoke();

            return true;
        }

    }
}
