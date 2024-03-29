﻿using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceForgeApi.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController: ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IRepositoryWrapper _repository;

        public UserController(IRepositoryWrapper repository)
        {
            _userRepository = repository.User;
            _repository = repository;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<UserGetRequest?> Get(int id)
        {   
            return await _userRepository.GetById(id);
        }
        [HttpGet]
        [Route("plain/{id}")]
        public async Task<UserGetRequest?> GetPlain(int id)
        {   
            return await _userRepository.GetById(id, true);
        }
        [HttpPost]
        public async Task<bool> Add(UserAddRequest user)
        {
            var userAdd = await _userRepository.Add(1,user);
            var userAddResult = userAdd is not null;

            if (userAddResult) {
                await _repository.Save();
            } else {
                _repository.DetachChanges();
            };
            return userAddResult;
        }
        [HttpPut]
        public async Task<bool> Update(UserUpdateRequest user)
        { 
            var userUpdate = await _userRepository.Update(user.Id, user);

            if (userUpdate) {
                await _repository.Save();
            } else {
                _repository.DetachChanges();
            };
            return userUpdate;
        }
        [HttpDelete]
        public async Task<bool> Delete(int id)
        {
            var userDelete = await _userRepository.Delete(id);

            if (userDelete) {
                await _repository.Save();
            } else {
                _repository.DetachChanges();
            };
            return userDelete;
        }
    }
}
