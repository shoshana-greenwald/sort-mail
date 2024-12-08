using AutoMapper;
using Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public class ContactForSubjectDto
    {
        public ContactForSubjectDto()
        {

        }
        public int id { get; set; }
        public int contactManID { get; set; }
        public int subjectId { get; set; }
        public double contactWeight { get; set; }


        public static ContactForSubjectDto DalToDto(ContactForSubject_tbl ContactForSubject)
        {
            var config = new MapperConfiguration(cfg =>
                 cfg.CreateMap<ContactForSubject_tbl, ContactForSubjectDto>()
             );
            var mapper = new Mapper(config);
            return mapper.Map<ContactForSubjectDto>(ContactForSubject);

        }
        public ContactForSubject_tbl DtoToDal()
        {
            var config = new MapperConfiguration(cfg =>
                     cfg.CreateMap<ContactForSubjectDto, ContactForSubject_tbl>()
                 );
            var mapper = new Mapper(config);
            return mapper.Map<ContactForSubject_tbl>(this);
        }



    }
}
