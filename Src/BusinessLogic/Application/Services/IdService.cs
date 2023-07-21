namespace Application.Services
{
    public class IdService : IIdService
    {
        public Guid GenerateNewId()
        {
            return Guid.NewGuid();
        }
    }
}