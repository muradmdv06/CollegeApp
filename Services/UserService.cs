using CollegeApp.Data.Repo;
using CollegeApp.Data;

using AutoMapper;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using CollegeApp.Models;

namespace CollegeApp.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly ICollegeRepository<User> _userRepository;
        public UserService(ICollegeRepository<User> userRepository, IMapper mapper) 
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<bool> CreateUserAsync(UserDTO dto)
        {
           

            ArgumentNullException.ThrowIfNull(dto,$"the argument {nameof(dto)} is null");

            var existingUser = await _userRepository.GetAsync(u => u.Username.Equals(dto.Username));

            if(existingUser != null)
            {
                throw new Exception("The username already taken");
            }

            User user = _mapper.Map<User>(dto);
            user.IsDeleted = false;
            user.CreatedDate=DateTime.Now;
            user.ModifiedDate=DateTime.Now;

            if (!string.IsNullOrEmpty(dto.Password))
            {
                var (passwordHash, salt) = CreatePasswordHashWithSalt(dto.Password);
                user.Password = passwordHash;
                user.PasswordSalt=salt;
            }

            await _userRepository.CreateAsync(user);

            return true;

            
            
        }
        public (string PasswordHash, string Salt) CreatePasswordHashWithSalt(string password)

        {
            //Create salt
            var salt = new byte[128 / 8];
            using(var rng=RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            //Create Password hash
            var hash=Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf:KeyDerivationPrf.HMACSHA256,
                iterationCount:1000,
                numBytesRequested:256 / 8
                ));
            return (hash,Convert.ToBase64String(salt));

        }
    }
}
