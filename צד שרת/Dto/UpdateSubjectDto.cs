using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
using AutoMapper;

namespace Dto
{
    public class UpdateSubjectDto
    {
        public UpdateSubjectDto()
        {

        }
        public int id { get; set; }
        public int mailID { get; set; }
        public string word { get; set; }
        public int amount { get; set; }

        public static UpdateSubjectDto DalToDto(UpdateSubject_tbl UpdateSubject)
        {
            var config = new MapperConfiguration(cfg =>
                 cfg.CreateMap<UpdateSubject_tbl, UpdateSubjectDto>()
             );
            var mapper = new Mapper(config);
            return mapper.Map<UpdateSubjectDto>(UpdateSubject);

        }
        public UpdateSubject_tbl DtoToDal()
        {
            var config = new MapperConfiguration(cfg =>
                     cfg.CreateMap<UpdateSubjectDto, UpdateSubject_tbl>()
                 );
            var mapper = new Mapper(config);
            return mapper.Map<UpdateSubject_tbl>(this);
        }


    }
}
