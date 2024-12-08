using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dal;


namespace Dto
{
    public class SubjectDto
    {
        public SubjectDto()
        {

        }

        public int subjectId { get; set; }
        public string subjectName { get; set; }
        public Nullable<int> parentID { get; set; }
        public int userId { get; set; }
        public string color { get; set; }


        public static SubjectDto DalToDto(Subject_tbl subject)
        {
            var config = new MapperConfiguration(cfg =>
                 cfg.CreateMap<Subject_tbl, SubjectDto>()
             );
            var mapper = new Mapper(config);
            return mapper.Map<SubjectDto>(subject);

        }
        public Subject_tbl DtoToDal()
        {
            var config = new MapperConfiguration(cfg =>
                     cfg.CreateMap<SubjectDto, Subject_tbl>()
                 );
            var mapper = new Mapper(config);
            return mapper.Map<Subject_tbl>(this);
        }
    }
}
