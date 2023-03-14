using ArbisSalesManagers.Models;

namespace ArbisSalesManagers;

public class AutoMapperProfile : Profile {
  public AutoMapperProfile()
  {
    CreateMap<AmoCrmUser, User>().ForMember(dest => dest.Username, opt => opt.MapFrom(x => x.Name))
                                 .ForMember(dest => dest.is_active, opt => opt.MapFrom(x => x.rights.is_active));
  }
}