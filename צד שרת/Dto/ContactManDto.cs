using AutoMapper;
using Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public class ContactManDto
    {
        public ContactManDto()
        {

        }
        public int contactManId { get; set; }
        public string email { get; set; }
        public string describeContact { get; set; }

        public static ContactManDto DalToDto(ContactMan_tbl ContactMan)
        {
            var config = new MapperConfiguration(cfg =>
                 cfg.CreateMap<ContactMan_tbl, ContactManDto>()
             );
            var mapper = new Mapper(config);
            return mapper.Map<ContactManDto>(ContactMan);

        }
        public ContactMan_tbl DtoToDal()
        {
            var config = new MapperConfiguration(cfg =>
                     cfg.CreateMap<ContactManDto, ContactMan_tbl>()
                 );
            var mapper = new Mapper(config);
            return mapper.Map<ContactMan_tbl>(this);
        }
    }
}
