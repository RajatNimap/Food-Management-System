using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private readonly IMapper  _mapper;
        public UserServices(IUnitOfWork _unit,IMapper mapper)
        {
            unitOfWork= _unit;
            _mapper=mapper; 
        }
        public async Task AddUser(UserModel user)
        {
           user.Password= BCrypt.Net.BCrypt.HashPassword(user.Password);  
           user.CreatedDate= DateTime.UtcNow;
           user.CreatedBy = user.Name;
           var UserEntity=_mapper.Map<User>(user);
           await unitOfWork.UserRepository.Add(UserEntity);  
           await unitOfWork.Commit();  

        }

        public async Task DeleteUser(int id)
        {
           var Data= await unitOfWork.UserRepository.GetById(id);
           unitOfWork.UserRepository.Delete(Data);
           await unitOfWork.Commit();  

        }
        public Task<IEnumerable<User>> GetAllUser()
        {
           return unitOfWork.UserRepository.GetAll();   
        }

        public Task<User> GetSingleUser(int Id)
        {
            return unitOfWork.UserRepository.GetById(Id);
        }

        public async Task UpdateUser(int id, UserModel  user)
        {
            var existingUser = await unitOfWork.UserRepository.GetById(id);
            if (existingUser == null)
                throw new KeyNotFoundException("User not found");

            _mapper.Map(user, existingUser);

            existingUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            existingUser.ModifiedDate=DateTime.UtcNow;
            existingUser.ModifiedBy = existingUser.Name;
            var rowsAffected = await unitOfWork.Commit();

        }
    }
}
