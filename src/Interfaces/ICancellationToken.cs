namespace Fleck2.Interfaces
{
    public interface ICancellationToken
    {
        void ThrowIfCancellationRequested();
    }
}
