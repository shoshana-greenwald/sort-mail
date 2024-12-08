using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
using AutoMapper;

namespace Dto
{
    public class UserDto
    {
        public UserDto()
        {

        }

        public int userId { get; set; }
        public string mail { get; set; }
        public string userPassword { get; set; }


        public static UserDto DalToDto(User_tbl User)
        {
            if (User == null)
                return null;
            var config = new MapperConfiguration(cfg =>
                 cfg.CreateMap<User_tbl, UserDto>()
             );
            var mapper = new Mapper(config);
            return mapper.Map<UserDto>(User);

        }
        public User_tbl DtoToDal()
        {
            var config = new MapperConfiguration(cfg =>
                     cfg.CreateMap<UserDto, User_tbl>()
                 );
            var mapper = new Mapper(config);
            return mapper.Map<User_tbl>(this);
        }


    }
}
