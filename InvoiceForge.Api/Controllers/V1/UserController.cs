using InvoiceForgeApi.Abl.user;
using InvoiceForgeApi.Helpers;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceForgeApi.Controllers
{
    [Route("api/user")]
    public class UserController: BaseController
    {
        public UserController(IRepositoryWrapper repository): base(repository) {}
        [HttpGet]
        [Route("{id}")]
        public async Task<CustomResponse<UserGetRequest?>> Get(int id)
        {   
            var task = await _repository.User.GetById(id);
            return CreateRepsonse(task);
        }
        [HttpGet]
        [Route("plain/{id}")]
        public async Task<CustomResponse<UserGetRequest?>> GetPlain(int id)
        {   
            var task = await _repository.User.GetById(id, true);
            return CreateRepsonse(task);
        }
        [HttpPost]
        public async Task<CustomResponse<bool>> Add(UserAddRequest user)
        {
            var abl = new AddUserAbl(_repository);
            var result = await abl.Resolve(user);
            return CreateRepsonse(result);
        }
        [HttpPut]
        public async Task<CustomResponse<bool>> Update(UserUpdateRequest user)
        { 
            var abl = new UpdateUserAbl(_repository);
            var result = await abl.Resolve(user);
            return CreateRepsonse(result);
        }
        [HttpDelete]
        public async Task<CustomResponse<bool>> Delete(int id)
        {
            var abl = new DeleteUserAbl(_repository);
            var result = await abl.Resolve(id);
            return CreateRepsonse(result);
        }
    }
}
