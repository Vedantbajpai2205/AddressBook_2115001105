using ModelLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IAddressBookRL
    {
        IEnumerable<AddressBookEntity> GetAll();
        AddressBookEntity GetById(int id);
        AddressBookEntity Add(AddressBookEntity contact);
        AddressBookEntity Update(int id, AddressBookEntity contact);
        bool Delete(int id);
    }
}