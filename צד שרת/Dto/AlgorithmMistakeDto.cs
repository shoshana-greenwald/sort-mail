using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dal;
namespace Dto
{
    public class AlgorithmMistakeDto
    {
        public AlgorithmMistakeDto()
        {

        }
        public int id { get; set; }
        public string word { get; set; }
        public int oldSubject { get; set; }
        public int newSubject { get; set; }
        public int AmountOfMistakes { get; set; }

        public static AlgorithmMistakeDto DalToDto(AlgorithmMistake_tbl AlgorithmMistake)
        {
            var config = new MapperConfiguration(cfg =>
                 cfg.CreateMap<AlgorithmMistake_tbl, AlgorithmMistakeDto>()
             );
            var mapper = new Mapper(config);
            return mapper.Map<AlgorithmMistakeDto>(AlgorithmMistake);

        }
        public AlgorithmMistake_tbl DtoToDal()
        {
            var config = new MapperConfiguration(cfg =>
                     cfg.CreateMap<AlgorithmMistakeDto, AlgorithmMistake_tbl>()
                 );
            var mapper = new Mapper(config);
            return mapper.Map<AlgorithmMistake_tbl>(this);
        }

    }
}
