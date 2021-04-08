using AutoMapper;
using ReaderApp.Data.Domain;
using ReaderApp.Data.DTOs.File;
using ReaderApp.Data.DTOs.User;

namespace ReaderApp.Data
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateUserMappings();
            CreateFileMappings();
        }

        private void CreateUserMappings()
        {
            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>();
        }

        private void CreateFileMappings()
        {
            CreateMap<TextFile, FileDto>();
        }
    }
}
