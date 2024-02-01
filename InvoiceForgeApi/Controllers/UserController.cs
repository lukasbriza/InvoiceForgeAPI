using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Handlers;
using InvoiceForgeApi.Interfaces;
using InvoiceForgeApi.Model;
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
        public async Task<ResponseHandler<User?>> Get(int id)
        {
            //validate model
            
            var User = await _userRepository.GetById(id);
            var response = User.BuildResponse(Response);
            return response;
        }
        [HttpPost]
        public async Task<ResponseHandler<bool>> Add(UserAddDTO user)
        {
            RequestHandler<bool> UserAdd = await _userRepository.Add(user);
            

            ResponseHandler<bool> UserResponse = new(Response, UserAdd);
            
            return UserResponse;
        }

    }
}
