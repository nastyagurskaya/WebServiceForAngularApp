
using AutoMapper;
using WebServiceForAngular.DAL.Models;

namespace WebServiceForAngular.ViewModels.Mappings
{
    public class ViewModelToEntityMappingProfile : Profile
    {
        public ViewModelToEntityMappingProfile()
        {
            CreateMap<RegistrationViewModel, AppUser>().ForMember(au => au.UserName, map => map.MapFrom(vm => vm.UserName));
            CreateMap<CheckPostViewModel, CheckListPost>();
            CreateMap<PostViewModel, Post>();
            CreateMap<CheckItemViewModel, CheckItem>();
            CreateMap<UserPostViewModel, UserPost>();
        }
    }
}
