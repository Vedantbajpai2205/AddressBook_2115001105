﻿using ModelLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IAddressBookBL
    {
        IEnumerable<AddressBookEntryModel> GetAll();
        AddressBookEntryModel GetById(int id);
        AddressBookEntryModel Add(RequestModel contact);
        AddressBookEntryModel Update(int id, RequestModel contact);
        bool Delete(int id);
    }
}