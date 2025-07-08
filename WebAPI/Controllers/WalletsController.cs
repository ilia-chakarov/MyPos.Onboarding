using Microsoft.AspNetCore.Mvc;
using WebAPI.UnitOfWork;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public WalletsController(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        

    }
}
