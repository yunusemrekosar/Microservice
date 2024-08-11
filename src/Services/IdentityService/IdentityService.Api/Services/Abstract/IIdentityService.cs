using IdentityService.Api.Models;
using Microsoft.AspNetCore.Identity.Data;

namespace IdentityService.Api.Services.Abstract
{
    public interface IIdentityService
    {
        Task<LoginResponseModel> Login(LoginRequestModel requestModel);
    }
}
