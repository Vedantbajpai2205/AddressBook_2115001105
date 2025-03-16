using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace BuisnessLayer.Interface
{
    public interface IAddressBookBL
    {
        public AddressBookEntryModel Add(RequestModel requestModel);
        public AddressBookEntryModel Update(int id, AddressBookEntryModel addressBookModel);
        public AddressBookEntryModel GetById(int id);
        public IEnumerable<AddressBookEntryModel> GetAll();
        public bool Delete(int id);
    }
}
