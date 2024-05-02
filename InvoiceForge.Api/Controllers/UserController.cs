using InvoiceForgeApi.Abl.user;
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
        public async Task<UserGetRequest?> Get(int id)
        {   
            return await _repository.User.GetById(id);
        }
        [HttpGet]
        [Route("plain/{id}")]
        public async Task<UserGetRequest?> GetPlain(int id)
        {   
            return await _repository.User.GetById(id, true);
        }
        [HttpPost]
        public async Task<bool> Add(UserAddRequest user)
        {
            var abl = new AddUserAbl(_repository);
            var result = await abl.Resolve(user);
            return result;
        }
        [HttpPut]
        public async Task<bool> Update(UserUpdateRequest user)
        { 
            var abl = new UpdateUserAbl(_repository);
            var result = await abl.Resolve(user);
            return result;
        }
        [HttpDelete]
        public async Task<bool> Delete(int id)
        {
            var abl = new DeleteUserAbl(_repository);
            var result = await abl.Resolve(id);
            return result;
        }
    }
}
