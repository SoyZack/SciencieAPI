using AutoMapper;
using SciencieAPI.Dtos.Cientificos;
using SciencieAPI.Dtos.Descubrimientos;
using SciencieAPI.Dtos.Especialidades;
using SciencieAPI.Dtos.Investigaciones;
using SciencieAPI.Models;
using System;

namespace SciencieAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeo de DateTime a DateOnly (Para los POST/PUT)
            CreateMap<DateTime, DateOnly>().ConvertUsing(dt => DateOnly.FromDateTime(dt));
            CreateMap<DateTime?, DateOnly?>().ConvertUsing(dt => dt.HasValue ? DateOnly.FromDateTime(dt.Value) : null);

            // Mapeo de DateOnly a DateTime (Para los GET)
            CreateMap<DateOnly, DateTime>().ConvertUsing(d => d.ToDateTime(TimeOnly.MinValue));
            CreateMap<DateOnly?, DateTime?>().ConvertUsing(d => d.HasValue ? d.Value.ToDateTime(TimeOnly.MinValue) : null);



            // Mapeos para Científicos (Tus mapeos actuales se quedan igual)
            CreateMap<Cientifico, CientificoDto>()
                .ForMember(dest => dest.Especialidades, opt => opt.MapFrom(src => src.Especialidades));

            CreateMap<Cientifico, CientificoDetalleDto>()
                .ForMember(dest => dest.Especialidades, opt => opt.MapFrom(src => src.Especialidades))
                .ForMember(dest => dest.Descubrimientos, opt => opt.MapFrom(src => src.Descubrimientos))
                .ForMember(dest => dest.Investigaciones, opt => opt.MapFrom(src => src.Investigaciones));

            // Los DTOs usan "FechaNaciemiento" en el JSON; el modelo usa "FechaNacimiento".
            CreateMap<CrearCientificoDto, Cientifico>()
                .ForMember(dest => dest.FechaNacimiento, opt => opt.MapFrom(src => src.FechaNaciemiento));
            CreateMap<ActualizarCientificoDto, Cientifico>()
                .ForMember(dest => dest.FechaNacimiento, opt => opt.MapFrom(src => src.FechaNaciemiento));

            // Mapeos para Especialidades
            CreateMap<Especialidad, EspecialidadDto>().ReverseMap();
            CreateMap<CrearEspecialidadDto, Especialidad>();

            // Mapeos para Descubrimientos
            CreateMap<Descubrimiento, DescubrimientosDto>().ReverseMap();
            CreateMap<CrearDescubrimientoDto, Descubrimiento>();
            CreateMap<ActualizarDescubrimientoDto, Descubrimiento>();

            // Mapeos para Investigaciones
            CreateMap<Investigacion, InvestigacionDto>().ReverseMap();
            CreateMap<CrearInvestigacionDto, Investigacion>();
            CreateMap<ActualizarInvestigacionDto, Investigacion>();
        }
    }
}