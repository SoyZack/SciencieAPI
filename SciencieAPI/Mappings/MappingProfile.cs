using AutoMapper;
using SciencieAPI.Dtos.Cientificos;
using SciencieAPI.Dtos.Descubrimientos;
using SciencieAPI.Dtos.Especialidades;
using SciencieAPI.Dtos.Investigaciones;
using SciencieAPI.Models;

namespace SciencieAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeos para Científicos
            // Se mapean las especialidades a través de la tabla intermedia CientificoEspecialidad.
            CreateMap<Cientifico, CientificoDto>()
                .ForMember(dest => dest.Especialidades, opt => opt.MapFrom(src => src.CientificoEspecialidades.Select(ce => ce.Especialidad)));

            CreateMap<Cientifico, CientificoDetalleDto>()
                .ForMember(dest => dest.Especialidades, opt => opt.MapFrom(src => src.CientificoEspecialidades.Select(ce => ce.Especialidad)))
                .ForMember(dest => dest.Descubrimientos, opt => opt.MapFrom(src => src.Descubrimientos))
                .ForMember(dest => dest.Investigaciones, opt => opt.MapFrom(src => src.Investigaciones));

            // Los DTOs usan "FechaNaciemiento" en el JSON; el modelo usa "FechaNacimiento".
            // AutoMapper copia por nombre, así que se mapea explícitamente para mantener el contrato.
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
