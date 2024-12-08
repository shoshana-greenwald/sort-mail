using AutoMapper;
using Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public  class EmailDto
    {
        public EmailDto()
        {

        }

        public int mailID { get; set; }
        public string IdInOutLook { get; set; }
        public int toUserId { get; set; }
        public string fromName { get; set; }
        public string fromAddressMail { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public System.DateTime dateReceived { get; set; }
        public Nullable<int> subjectMail { get; set; }
        public string RelatedSentence { get; set; }

        public static EmailDto DalToDto(Email_tbl Email)
        {
            var config = new MapperConfiguration(cfg =>
                 cfg.CreateMap<Email_tbl, EmailDto>()
             );
            var mapper = new Mapper(config);
            return mapper.Map<EmailDto>(Email);

        }
        public Email_tbl DtoToDal()
        {
            var config = new MapperConfiguration(cfg =>
                     cfg.CreateMap<EmailDto, Email_tbl>()
                 );
            var mapper = new Mapper(config);
            return mapper.Map<Email_tbl>(this);
        }
    }
}
