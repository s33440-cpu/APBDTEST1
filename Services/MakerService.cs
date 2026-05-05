using Test1.DTOs;
using Test1.Repositories;
namespace Test1.Service
{
    public class MakerService : IMakerService
    {
        private readonly IMakerRepository _repo;

        public MakerService(IMakerRepository repo)
        {
            _repo = repo;
        }

        public Task<MakerResponse?> GetMaker(int makerId)
            => _repo.GetMaker(makerId);

        public Task<int> CreateMaker(CreateMakerRequest request)
            => _repo.CreateMaker(request);
    }
}