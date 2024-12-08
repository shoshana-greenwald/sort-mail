using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
using AutoMapper;

namespace Dto
{
    public class UpdateKeySentenceDto
    {
        public UpdateKeySentenceDto()
        {

        }
        public int id { get; set; }
        public int mailID { get; set; }
        public int subjectId { get; set; }
        public string word { get; set; }
        public double wordWeight { get; set; }
        public bool isCommon { get; set; }

        public static UpdateKeySentenceDto DalToDto(UpdateKeySentence_tbl UpdateKeySentence)
        {
            var config = new MapperConfiguration(cfg =>
                 cfg.CreateMap<UpdateKeySentence_tbl, UpdateKeySentenceDto>()
             );
            var mapper = new Mapper(config);
            return mapper.Map<UpdateKeySentenceDto>(UpdateKeySentence);

        }
        public UpdateKeySentence_tbl DtoToDal()
        {
            var config = new MapperConfiguration(cfg =>
                     cfg.CreateMap<UpdateKeySentenceDto, UpdateKeySentence_tbl>()
                 );
            var mapper = new Mapper(config);
            return mapper.Map<UpdateKeySentence_tbl>(this);
        }


    }
}
