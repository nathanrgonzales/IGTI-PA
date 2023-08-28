using MyAppIGTI.Models;

namespace MyAppIGTI.DBRepo
{
    public interface IProfileTestRepo
    {
        List<ProfileTestModel> GetAllProfileTest();
        ProfileTestModel GetProfileTest(int id);
        ProfileTestModel InsertProfileTest(ProfileTestModel profileTest);
        ProfileTestModel UpdateProfileTest(ProfileTestModel profileTest);
        bool DeleteProfileTest(int id);
        List<ResultTestModel> GetAllResultTest();
        ResultTestModel GetResultTestbyProfile(int id);
        ResultTestModel InsertResultTest(ResultTestModel resultTest);
        ResultTestModel UpdateResultTest(ResultTestModel resultTest);
    }
}
