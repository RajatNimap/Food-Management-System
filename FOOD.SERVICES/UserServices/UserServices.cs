using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FOOD.DATA.Entites;
using FOOD.DATA.Infrastructure;
using FOOD.DATA.Repository.UserRepository;
using FOOD.MODEL.Model;

namespace FOOD.SERVICES.UserServices
{
    public class UserServices : IUserServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;

        public UserServices(IUnitOfWork _unit, IMapper mapper)
        {
            unitOfWork = _unit;
            _mapper = mapper;
        }

        public async Task<bool> AddUser(UserModel user)
        {
            try
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user.CreatedDate = DateTime.UtcNow;
                user.CreatedBy = "admin";
                user.ModifiedBy = "admin";

                var UserEntity = _mapper.Map<User>(user);
                await unitOfWork.UserRepository.Add(UserEntity);

                var rowaffected = await unitOfWork.Commit();
                return rowaffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while adding user", ex);
            }
        }

        public async Task<bool> DeleteUser(int id)
        {
            try
            {
                var Data = await unitOfWork.UserRepository.GetById(id);
                if (Data == null)
                    throw new KeyNotFoundException($"User with ID {id} not found");

                unitOfWork.UserRepository.Delete(Data);
                var rowaffect = await unitOfWork.Commit();

                return rowaffect > 0;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                
                throw new Exception($"Error occurred while deleting user with ID {id}", ex);
            }
        }

        public async Task<IEnumerable<User>> GetAllUser()
        {
            try
            {
                return await unitOfWork.UserRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving all users", ex);
            }
        }

        public async Task<User> GetSingleUser(int Id)
        {
            try
            {
                var user = await unitOfWork.UserRepository.GetById(Id);
                if (user == null)
                    throw new KeyNotFoundException($"User with ID {Id} not found");

                return user;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while retrieving user with ID {Id}", ex);
            }
        }

        public async Task<bool> UpdateUser(int id, UserModel user)
        {
            try
            {
                var existingUser = await unitOfWork.UserRepository.GetById(id);
                if (existingUser == null)
                    throw new KeyNotFoundException("User not found");

                _mapper.Map(user, existingUser);
                existingUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                existingUser.ModifiedDate = DateTime.UtcNow;
                existingUser.ModifiedBy = existingUser.Name;

                var rowsAffected = await unitOfWork.Commit();
                return rowsAffected > 0;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while updating user with ID {id}", ex);
            }
        }
    }
}