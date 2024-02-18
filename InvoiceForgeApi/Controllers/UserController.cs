using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceForgeApi.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController: ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<UserGetRequest?> Get(int id)
        {   
            return await _userRepository.GetById(id);
        }

        [HttpPost]
        public async Task<bool> Add(UserAddRequest user)
        {
            return await _userRepository.Add(user);

        }

        [HttpPut]
        public async Task<bool> Update(UserUpdateRequest user)
        { 
            return await _userRepository.Update(user);
        }

        [HttpDelete]
        public async Task<bool> Delete(int id)
        {
            return await _userRepository.Delete(id);
        }
    }
}
