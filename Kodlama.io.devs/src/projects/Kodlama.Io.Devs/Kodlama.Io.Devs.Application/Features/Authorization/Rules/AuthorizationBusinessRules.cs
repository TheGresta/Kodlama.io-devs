﻿using Core.CrossCuttingConcerns.Exceptions;
using Core.Security.Dtos;
using Core.Security.Entities;
using Core.Security.Hashing;
using Kodlama.Io.Devs.Application.Services.Repositories;

namespace Kodlama.Io.Devs.Application.Features.Authorization.Rules
{
    public class AuthorizationBusinessRules
    {
        private readonly IUserRepository _userRepository;
        private readonly AuthorizationBusinessRulesMessages _message;

        public AuthorizationBusinessRules(IUserRepository userRepository, AuthorizationBusinessRulesMessages message)
        {
            _userRepository = userRepository;
            _message = message;
        }

        public async Task UserShouldBeExistWhenLogin(string mail)
        {
            User? user = await _userRepository.GetAsync(u => u.Email.ToLower() == mail.ToLower());
            if (user == null) throw new BusinessException(_message.UserDoesNotExist);
        }

        public async Task UserShouldNotExistsWhenRegister(string mail)
        {
            User? user = await _userRepository.GetAsync(u => u.Email.ToLower() == mail.ToLower());
            if (user != null) throw new BusinessException(_message.UserAlreadyExists);
        }

        public void VerifyPassword(UserForLoginDto userForLoginDto, User user)
        {
            if (HashingHelper.VerifyPasswordHash(userForLoginDto.Password, user.PasswordHash, user.PasswordSalt) == false)
                throw new BusinessException(_message.WrongPassword);
        }
    }
}
