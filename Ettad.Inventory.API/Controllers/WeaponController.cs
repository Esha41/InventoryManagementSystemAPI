using Ettad.CrossCutting.Common.Security;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Inventory.Service.Common.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Ettad.Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WeaponController : ApiControllerBase
    {
        private readonly Ettad.EntityFramework.DataBaseContext.ApplicationDbContext _context;
        private readonly AutoMapper.IMapper _mapper;

        public WeaponController(
            Ettad.EntityFramework.DataBaseContext.ApplicationDbContext context,
            AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Ammunition.View", "Permissions.Ammunition.Page")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var weapons = await _context.Weapons
                    .Where(w => !w.IsDeleted)
                    .Include(w => w.Hcc)
                    .ToListAsync();

                var dtos = _mapper.Map<List<BaseItemDto>>(weapons);
                var result = APIOperationResponse<List<BaseItemDto>>.Success(dtos);
                return ProcessResponse(result);
            }
            catch (Exception ex)
            {
                var result = APIOperationResponse<List<BaseItemDto>>.Fail(
                    Ettad.ResponseHandler.Consts.ResponseType.InternalServerError, 
                    $"An error occurred: {ex.Message}");
                return ProcessResponse(result);
            }
        }
    }
}

