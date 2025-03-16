using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface
{
    public interface IAddressBookRL
    {
        public AddressBookEntity Add(AddressBookEntity addressBookEntity);
        public AddressBookEntity Update(int id, AddressBookEntity addressBookEntity);
        public AddressBookEntity GetById(int id);
        public IEnumerable<AddressBookEntity> GetAll();
        public bool Delete(int id);
    }
}
