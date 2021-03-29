using FluentValidationApp.DTOs;
using FluentValidationApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace FluentValidationApp.Mapping
{
    public class EventDateProfile : Profile
    {
        public EventDateProfile()
        {
            CreateMap<EventDateDto, EventDate>()
                .ForMember(x => x.Date, opt => opt.MapFrom(x => new DateTime(x.Year, x.Month, x.Day)));
            //tersi dönüşüm için bu custom bi donusum oldugundan reversemap işe yaramaz. o yuzden kendımız yazıyoruz.
            CreateMap<EventDate, EventDateDto>()
                .ForMember(x => x.Year, opt => opt.MapFrom(x => x.Date.Year))
                .ForMember(x => x.Month, opt => opt.MapFrom(x => x.Date.Month))
                .ForMember(x => x.Day, opt => opt.MapFrom(x => x.Date.Day));
        }
    }
}
