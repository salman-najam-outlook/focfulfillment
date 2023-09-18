using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using LocalDropshipping.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocalDropshipping.Web.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly IProfilesService service;

        public ProfilesController(IProfilesService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(service.GetAllProfiles());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            return Ok(service.GetProfileById(id));
        }

        [HttpPost]
        public IActionResult Post(ProfilesDto profilesDto)
        {
            Profiles profile = profilesDto.ToEntity();
            service.Add(profile);
            return Ok();
        }

    }
}
