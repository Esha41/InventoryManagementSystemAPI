using Ettad.Inventory.Service.Ammunitions;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ettad.Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AmmunitionsController : ApiControllerBase
    {
        private readonly IAmmunitionService _ammunitionService;

        public AmmunitionsController(IAmmunitionService ammunitionService)
        {
            _ammunitionService = ammunitionService;
        }
    }
}
