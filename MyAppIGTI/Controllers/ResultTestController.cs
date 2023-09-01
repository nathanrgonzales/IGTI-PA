using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyAppIGTI.AppVariable;
using MyAppIGTI.DBRepo;
using MyAppIGTI.Models;
using MyAppIGTI.Services;
using System.Drawing;
using System.Management.Automation;

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
        public IActionResult ShowSuccessResult(int id)
        {
            ResultTestModel resultTest = _IProfileTestRepo.GetResultTestbyProfile(id);

            string omainPath = _options.Value.MainPath;
            string otestFolder = resultTest.IdProfileTestModel.ToString("0000000000");
            string otestFile = "Result" + resultTest.IdProfileTestModel.ToString("0000000000") + ".txt";
            byte[] fileBytes = System.IO.File.ReadAllBytes(omainPath + "\\" + otestFolder + "\\" + otestFile);

            return File(fileBytes, "application/force-download", omainPath + "\\" + otestFolder + "\\" + otestFile);
        }

        public IActionResult ShowErrorResult(int id)
        {
            ResultTestModel resultTest = _IProfileTestRepo.GetResultTestbyProfile(id);

            string omainPath = _options.Value.MainPath;
            string otestFolder = resultTest.IdProfileTestModel.ToString("0000000000");
            string otestFile = "Error" + resultTest.IdProfileTestModel.ToString("0000000000") + ".txt";
            byte[] fileBytes = System.IO.File.ReadAllBytes(omainPath + "\\" + otestFolder + "\\" + otestFile);

            return File(fileBytes, "application/force-download", omainPath + "\\" + otestFolder + "\\" + otestFile);
        }

        public IActionResult StartResultTest(int id)
        {
            ResultTestModel resultTest = _IProfileTestRepo.GetResultTestbyProfile(id);
            return View(resultTest);
        }

        public IActionResult StartResultTestOk(int id)
        {
            ProfileTestModel profileTest = _IProfileTestRepo.GetProfileTest(id);
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

            bool enviou = SendResultInEmail(profileTest, resultTest);

            return RedirectToAction("Index");
        }

        internal bool SendResultInEmail(ProfileTestModel oProfileTest, ResultTestModel oResultTest)
        {            
            string omainPath = _options.Value.MainPath;
            string oTestFolder = oResultTest.IdProfileTestModel.ToString("0000000000");
            string oResultFile = "Result" + oResultTest.IdProfileTestModel.ToString("0000000000") + ".txt";

            string EmailTo  = oProfileTest.ListEmail;
            string Assunto = "Result of Profile = " + oProfileTest.Description;
            string Messagem = oResultTest.Status;
            string AttFile  = omainPath + "\\" + oTestFolder + "\\" + oResultFile;

            try
            {
                TesteEnvioEmail(EmailTo, Assunto, Messagem, AttFile).GetAwaiter();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task TesteEnvioEmail(string email, string assunto, string mensagem, string anexo)
        {
            try
            {
                AuthMessageSender oEngine = new AuthMessageSender();    
                await oEngine.SendEmailAsync(email, assunto, mensagem, anexo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

            ps.AddScript("dotnet build " + omainPath + "\\" + otestFolder + profileTest.ProjectName + " | Out-File \"Result"+ otestFolder + ".txt\"").Invoke();
            
            ps.AddScript("dotnet sonarscanner end /d:sonar.token=\"59bd70d6e28d37965f899481451f1e5df0ac27d5\"").Invoke();

            return true;
        }

    }
}
