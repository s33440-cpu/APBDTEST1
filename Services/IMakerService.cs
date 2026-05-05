using Test1.DTOs;

namespace Test1.Service
{
    public interface IMakerService
    {
        Task<MakerResponse?> GetMaker(int makerId);
        Task<int> CreateMaker(CreateMakerRequest request);
    }
}