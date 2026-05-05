using Test1.DTOs;

namespace Test1.Repositories
{
    public interface IMakerRepository
    {
        Task<MakerResponse?> GetMaker(int makerId);
        Task<int> CreateMaker(CreateMakerRequest request);
    }
}