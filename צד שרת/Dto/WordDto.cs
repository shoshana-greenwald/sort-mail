using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dal;
namespace Dto
{
    public class WordDto
    {
        public WordDto()
        {

        }
        public int wordId { get; set; }
        public string wordName { get; set; }

        public static WordDto DalToDto(Word_tbl Word)
        {
            var config = new MapperConfiguration(cfg =>
                 cfg.CreateMap<Word_tbl, WordDto>()
             );
            var mapper = new Mapper(config);
            return mapper.Map<WordDto>(Word);

        }
        public Word_tbl DtoToDal()
        {
            var config = new MapperConfiguration(cfg =>
                     cfg.CreateMap<WordDto, Word_tbl>()
                 );
            var mapper = new Mapper(config);
            return mapper.Map<Word_tbl>(this);
        }
    }
}
