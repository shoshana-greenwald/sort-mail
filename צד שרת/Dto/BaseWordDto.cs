using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
using AutoMapper;


namespace Dto
{
    public class BaseWordDto
    {

        public int id { get; set; }
        public string word { get; set; }
        public string @base { get; set; }


        public static BaseWordDto DalToDto(BaseWord_tbl BaseWord)
        {
            var config = new MapperConfiguration(cfg =>
                 cfg.CreateMap<BaseWord_tbl, BaseWordDto>()
             );
            var mapper = new Mapper(config);
            return mapper.Map<BaseWordDto>(BaseWord);

        }
        public BaseWord_tbl DtoToDal()
        {
            var config = new MapperConfiguration(cfg =>
                     cfg.CreateMap<BaseWordDto, BaseWord_tbl>()
                 );
            var mapper = new Mapper(config);
            return mapper.Map<BaseWord_tbl>(this);
        }
    }



}
