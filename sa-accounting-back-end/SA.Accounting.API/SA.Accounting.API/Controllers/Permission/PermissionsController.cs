using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SA.Accounting.Core.Abstractions.Consts;

namespace SA.Accounting.API.Controllers.Permission;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PermissionsController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        return Ok(Permissions.GetAllPermissions());
    }
}
