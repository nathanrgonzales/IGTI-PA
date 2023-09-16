using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAppIGTI.Data;
using MyAppIGTI.Models;

namespace MyAppIGTI.DBRepo
{
    public class ProfileTestRepo : IProfileTestRepo
    {
        private readonly DBMyAppContext _dbMyAppContext;
        public ProfileTestRepo(DBMyAppContext dbMyAppContext)
        {
            _dbMyAppContext = dbMyAppContext;
        }

        public List<ProfileTestModel> GetAllProfileTest()
        {
            return _dbMyAppContext.TabProfileTest.ToList();
        }

        public ProfileTestModel GetProfileTest(int id)
        {
            return _dbMyAppContext.TabProfileTest.FirstOrDefault(x => x.Id == id);
        }

        public ProfileTestModel InsertProfileTest(ProfileTestModel profileTest)
        {
            _dbMyAppContext.TabProfileTest.Add(profileTest);
            _dbMyAppContext.SaveChanges();
            return profileTest;
        }
        public ProfileTestModel UpdateProfileTest(ProfileTestModel profileTest)
        {
            ProfileTestModel oProfileTestModelDB = GetProfileTest(profileTest.Id);

            if (oProfileTestModelDB == null) throw new System.Exception("Update error");

            oProfileTestModelDB.Description = profileTest.Description;
            oProfileTestModelDB.RepoType = profileTest.RepoType;
            oProfileTestModelDB.RepoLink = profileTest.RepoLink;
            oProfileTestModelDB.BuildType = profileTest.BuildType;
            oProfileTestModelDB.ProjectName = profileTest.ProjectName;
            oProfileTestModelDB.ListEmail = profileTest.ListEmail;

            _dbMyAppContext.TabProfileTest.Update(oProfileTestModelDB);
            _dbMyAppContext.SaveChanges();
            return profileTest;
        }

        public bool DeleteProfileTest(int id)
        {
            ProfileTestModel oProfileTestModelDB = GetProfileTest(id);

            if (oProfileTestModelDB == null) throw new System.Exception("Insert error");

            _dbMyAppContext.TabProfileTest.Remove(oProfileTestModelDB);
            _dbMyAppContext.SaveChanges();

            return true;
        }

        public List<ResultTestModel> GetAllResultTest()
        {
            List<ProfileTestModel> oListProfileTestDB = _dbMyAppContext.TabProfileTest.ToList();
            List<ResultTestModel> oListResultTestDB    = _dbMyAppContext.TabResultTest.ToList();
            List<ResultTestModel> oResult = new List<ResultTestModel>();
            
            foreach (ProfileTestModel oProfileTest in oListProfileTestDB)
            {
                ResultTestModel oResultTest = new ResultTestModel();
                oResultTest.Id = oListResultTestDB.FirstOrDefault(x => x.IdProfileTestModel == oProfileTest.Id)?.Id ?? -1;
                oResultTest.IdProfileTestModel = oProfileTest.Id;
                oResultTest.Description = oProfileTest.Description;                
                oResultTest.Status = oListResultTestDB.FirstOrDefault(x => x.IdProfileTestModel == oProfileTest.Id)?.Status ?? "";
                oResultTest.IdStatus = oListResultTestDB.FirstOrDefault(x => x.IdProfileTestModel == oProfileTest.Id)?.IdStatus ?? 0;
                oResultTest.Result = oListResultTestDB.FirstOrDefault(x => x.IdProfileTestModel == oProfileTest.Id)?.Result ?? "";

                oResult.Add(oResultTest);
            }
            return oResult;
        }

        public ResultTestModel GetResultTestbyProfile(int id)
        {
            ProfileTestModel oProfileTestDB = _dbMyAppContext.TabProfileTest.FirstOrDefault(x => x.Id == id);
            
            if (oProfileTestDB == null) throw new System.Exception("GetResultTestbyProfile error");

            ResultTestModel oResultTest = _dbMyAppContext.TabResultTest.FirstOrDefault(x => x.IdProfileTestModel == id);

            if (oResultTest == null)
            {
                oResultTest = new ResultTestModel();
                oResultTest.Id = -1;
                oResultTest.IdProfileTestModel = oProfileTestDB.Id;
                oResultTest.Description = oProfileTestDB.Description;
                oResultTest.Status = "";
                oResultTest.IdStatus = 0;
                oResultTest.Result = "";
            }

            return oResultTest;
        }

        public ResultTestModel GetResultTestbyID(int id)
        {
            ResultTestModel oResultTestModelDB = _dbMyAppContext.TabResultTest.FirstOrDefault(x => x.Id == id);

            if (oResultTestModelDB == null) throw new System.Exception("GetResultTestbyProfile error");

            return oResultTestModelDB;
        }

        public ResultTestModel InsertResultTest(ResultTestModel resultTest)
        {
            if (resultTest.Id < 0)
                resultTest.Id = 0;

            _dbMyAppContext.TabResultTest.Add(resultTest);
            _dbMyAppContext.SaveChanges();
            return resultTest;
        }

        public ResultTestModel UpdateResultTest(ResultTestModel resultTest)
        {
            ResultTestModel oResultTestModelDB = GetResultTestbyID(resultTest.Id);

            if (oResultTestModelDB == null) throw new System.Exception("Update error");

            oResultTestModelDB.Status = resultTest.Status;

            _dbMyAppContext.TabResultTest.Update(oResultTestModelDB);
            _dbMyAppContext.SaveChanges();
            return resultTest;
        }
    }
}
