using AutoMapper;
using BusinessLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text.Json;
using NLog;
using BusinessLayer.Interface;

namespace BusinessLayer.Service
{
    public class AddressBookBL : IAddressBookBL
    {
        private readonly IAddressBookRL _addressBookRL;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const string CacheKey = "AllContacts";

        public AddressBookBL(IAddressBookRL addressBookRL, ICacheService cacheService, IMapper mapper)
        {
            _addressBookRL = addressBookRL ?? throw new ArgumentNullException(nameof(addressBookRL));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public IEnumerable<AddressBookEntryModel> GetAll()
        {
            try
            {
                // Attempt to retrieve from cache
                var cacheData = _cacheService.GetCache(CacheKey);
                if (!string.IsNullOrEmpty(cacheData))
                {
                    Console.WriteLine("Cache Hit! Returning from Cache.");
                    return JsonSerializer.Deserialize<IEnumerable<AddressBookEntryModel>>(cacheData);
                }

                // Fetch from database
                var contacts = _addressBookRL.GetAll();
                if (contacts == null) return new List<AddressBookEntryModel>();

                var mappedContacts = _mapper.Map<IEnumerable<AddressBookEntryModel>>(contacts);

                // Store in cache
                var serializedData = JsonSerializer.Serialize(mappedContacts);
                _cacheService.SetCache(CacheKey, serializedData, 10);

                return mappedContacts;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error in GetAllContacts");
                return new List<AddressBookEntryModel>();
            }
        }

        public AddressBookEntryModel GetById(int id)
        {
            try
            {
                var contact = _addressBookRL.GetById(id);
                return contact != null ? _mapper.Map<AddressBookEntryModel>(contact) : null;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error retrieving contact with ID: {id}");
                return null;
            }
        }

        public AddressBookEntryModel Add(RequestModel contact)
        {
            if (contact == null) throw new ArgumentNullException(nameof(contact), "Contact data cannot be null.");

            try
            {
                var entity = _mapper.Map<AddressBookEntity>(contact);
                var newContact = _addressBookRL.Add(entity);

                _cacheService.RemoveCache(CacheKey); // Invalidate cache

                return _mapper.Map<AddressBookEntryModel>(newContact);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error adding new contact");
                return null;
            }
        }

        public AddressBookEntryModel Update(int id, RequestModel contact)
        {
            if (contact == null) throw new ArgumentNullException(nameof(contact), "Contact data cannot be null.");

            try
            {
                var entity = _mapper.Map<AddressBookEntity>(contact);
                var updatedContact = _addressBookRL.Update(id, entity);

                if (updatedContact == null)
                {
                    return null; // Contact not found
                }

                _cacheService.RemoveCache(CacheKey); // Invalidate cache

                return _mapper.Map<AddressBookEntryModel>(updatedContact);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error updating contact with ID: {id}");
                return null;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                bool isDeleted = _addressBookRL.Delete(id);
                if (isDeleted)
                {
                    _cacheService.RemoveCache(CacheKey);
                }
                return isDeleted;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error deleting contact with ID: {id}");
                return false;
            }
        }
    }
}