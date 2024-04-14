﻿using InvoiceForgeApi.DTO.Model;
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
            var userAdd = await _userRepository.Add(user);
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