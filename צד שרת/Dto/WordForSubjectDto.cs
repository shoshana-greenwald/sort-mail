using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dal;
namespace Dto
{
    public class WordForSubjectDto
    {
        public WordForSubjectDto()
        {

        }

        public WordForSubjectDto( int sub, int word, double weight, bool common)
        {
            subjectId = sub;
            wordId = word;
            wordWeight = weight;
            isCommon = common;
        }
        public int id { get; set; }
        public int subjectId { get; set; }
        public int wordId { get; set; }
        public double wordWeight { get; set; }
        public bool isCommon { get; set; }


        public static WordForSubjectDto DalToDto(WordForSubject_tbl WordForSubject)
        {
            var config = new MapperConfiguration(cfg =>
                 cfg.CreateMap<WordForSubject_tbl, WordForSubjectDto>()
             );
            var mapper = new Mapper(config);
            return mapper.Map<WordForSubjectDto>(WordForSubject);

        }
        public WordForSubject_tbl DtoToDal()
        {
            var config = new MapperConfiguration(cfg =>
                     cfg.CreateMap<WordForSubjectDto, WordForSubject_tbl>()
                 );
            var mapper = new Mapper(config);
            return mapper.Map<WordForSubject_tbl>(this);
        }

    }
}
