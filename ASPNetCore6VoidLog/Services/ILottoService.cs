namespace ASPNetCore6VoidLog.Services
{
    using ASPNetCore6VoidLog.ViewModels;

    public interface ILottoService
    {
        LottoViewModel Lottoing(int min, int max);
    }
}
