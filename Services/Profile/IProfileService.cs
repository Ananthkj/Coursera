using Coursera.Areas.Instructor.Models;

namespace Coursera.Services.Profile
{
    public interface IProfileService
    {
        Task<MyProfileModel> GetProfile(int UserId);

        Task<List<MyProfileModel>> GetInstructorDetails();
    }
}
