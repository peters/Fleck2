namespace Fleck.Interfaces
{
    public interface ICancellationToken
    {
        void ThrowIfCancellationRequested();
    }
}
