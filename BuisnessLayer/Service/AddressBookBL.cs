using AutoMapper;
using BuisnessLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using Newtonsoft.Json;

namespace BusinessLayer.Services
{
    public class AddressBookBL : IAddressBookBL
    {
        private readonly IAddressBookRL _addressBookRL;
        private readonly IMapper _mapper;

        public AddressBookBL(IAddressBookRL addressBookRL, IMapper mapper)
        {
            _addressBookRL = addressBookRL;
            _mapper = mapper;
        }
        public AddressBookEntryModel Add(RequestModel requestModel)
        {
            var entity = _mapper.Map<AddressBookEntity>(requestModel);
            var addedEntity = _addressBookRL.Add(entity);


            return _mapper.Map<AddressBookEntryModel>(addedEntity);
        }





        public AddressBookEntryModel Update(int id, AddressBookEntryModel addressBookModel)
        {
            var addressBookEntity = _mapper.Map<AddressBookEntity>(addressBookModel);
            var updatedEntity = _addressBookRL.Update(id, addressBookEntity);
            addressBookEntity.Id = id;  // Ensure ID consistency
            return _mapper.Map<AddressBookEntryModel>(updatedEntity);
        }

        public AddressBookEntryModel GetById(int id)
        {
            var addressBookEntity = _addressBookRL.GetById(id);
            return _mapper.Map<AddressBookEntryModel>(addressBookEntity);
        }

        public IEnumerable<AddressBookEntryModel> GetAll()
        {
            var entities = _addressBookRL.GetAll();
            return _mapper.Map<IEnumerable<AddressBookEntryModel>>(entities);
        }

        public bool Delete(int id)
        {
            return _addressBookRL.Delete(id);
        }
    }
}