namespace ASPNetCore6VoidLog.Controllers
{
    using ASPNetCore6VoidLog.Services;
    using ASPNetCore6VoidLog.Wrapper;
    using Microsoft.AspNetCore.Mvc;

    public class LottoController : Controller
    {
        private readonly ILottoService _lottoService;

        public LottoController(ILottoService lottoService)
        {
            _lottoService = lottoService;
        }

        public IActionResult Index()
        {
            var result = _lottoService.Lottoing(0, 10);
            return View(result);
        }
    }
}
