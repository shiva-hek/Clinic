namespace Domain.Models.Appointments.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IAppointmentRepository AppointmentRepository { get; }
        
        Task CommitAsync(CancellationToken cancellationToken = default);
        ValueTask RollBackAsync(CancellationToken cancellationToken = default);
    }
}