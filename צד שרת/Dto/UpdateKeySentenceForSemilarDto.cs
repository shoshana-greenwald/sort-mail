using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
using AutoMapper;

namespace Dto
{
    public class UpdateKeySentenceForSemilarDto
    {
        public UpdateKeySentenceForSemilarDto()
        {

        }
        public int id { get; set; }
        public int mailID { get; set; }
        public int subjectId { get; set; }
        public string word { get; set; }
        public double commonWeight { get; set; }
        public int commonAmount { get; set; }
        public double specialWeight { get; set; }
        public int specialAmount { get; set; }


        public static UpdateKeySentenceForSemilarDto DalToDto(UpdateKeySentenceForSemilar_tbl UpdateKeySentenceForSemilar)
        {
            var config = new MapperConfiguration(cfg =>
                 cfg.CreateMap<UpdateKeySentenceForSemilar_tbl, UpdateKeySentenceForSemilarDto>()
             );
            var mapper = new Mapper(config);
            return mapper.Map<UpdateKeySentenceForSemilarDto>(UpdateKeySentenceForSemilar);

        }
        public UpdateKeySentenceForSemilar_tbl DtoToDal()
        {
            var config = new MapperConfiguration(cfg =>
                     cfg.CreateMap<UpdateKeySentenceForSemilarDto, UpdateKeySentenceForSemilar_tbl>()
                 );
            var mapper = new Mapper(config);
            return mapper.Map<UpdateKeySentenceForSemilar_tbl>(this);
        }



    }
}
