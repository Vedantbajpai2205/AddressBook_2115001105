using AutoMapper;
using RepositoryLayer.Entity;
using ModelLayer.Model;

namespace AddressBook.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AddressBookEntity, AddressBookEntryModel>().ReverseMap();
        }
    }
}
