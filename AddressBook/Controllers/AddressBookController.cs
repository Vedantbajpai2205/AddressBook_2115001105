using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model; // Include your models
using Microsoft.EntityFrameworkCore;
using System.Linq;
using RepositoryLayer.Context;
using Microsoft.Extensions.Logging;
using RepositoryLayer.Entity;
using NLog;
using AutoMapper;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace AddressBook.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressBookController : ControllerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IMapper _mapper;
        private readonly IValidator<AddressBookEntryModel> _validator;
        private static List<AddressBookEntity> _addressBookEntries = new List<AddressBookEntity>();
        private static int _idCounter = 1;

        public AddressBookController(IMapper mapper, IValidator<AddressBookEntryModel> validator)
        {
            _mapper = mapper;
            _validator = validator;
            _logger.Info("Logger has been integrated");
        }
        /// <summary>
        /// Get contacts 
        /// </summary>
        /// <returns></returns>
        // GET: api/addressbook
        [HttpGet]
        public IActionResult GetAllContacts()
        {
            var contacts = _mapper.Map<IEnumerable<AddressBookEntryModel>>(_addressBookEntries);
            return Ok(new ResponseModel<IEnumerable<AddressBookEntryModel>>
            {
                Success = true,
                Message = "Contacts fetched successfully.",
                Data = contacts
            });
        }
        /// <summary>
        /// Get contacts by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/addressbook/{id}
        [HttpGet("{id}")]
        public IActionResult GetContactById(int id)
        {
            var contact = _addressBookEntries.FirstOrDefault(e => e.Id == id);
            if (contact == null)
            {
                return NotFound(new ResponseModel<string> { Success = false, Message = "Contact not found." });
            }

            return Ok(new ResponseModel<AddressBookEntryModel>
            {
                Success = true,
                Message = "Contact fetched successfully.",
                Data = _mapper.Map<AddressBookEntryModel>(contact)
            });
        }
        /// <summary>
        /// Add Contacts
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        // POST: api/addressbook
        [HttpPost]
        public IActionResult AddContact([FromBody] AddressBookEntryModel request)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Validation failed.",
                    Data = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))
                });
            }

            var newContact = _mapper.Map<AddressBookEntity>(request);
            newContact.Id = _idCounter++;
            _addressBookEntries.Add(newContact);

            return CreatedAtAction(nameof(GetContactById), new { id = newContact.Id }, new ResponseModel<AddressBookEntryModel>
            {
                Success = true,
                Message = "Contact added successfully.",
                Data = _mapper.Map<AddressBookEntryModel>(newContact)
            });
        }
        /// <summary>
        /// Edit by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        // PUT: api/addressbook/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateContact(int id, [FromBody] AddressBookEntryModel request)
        {
            var existingContact = _addressBookEntries.FirstOrDefault(e => e.Id == id);
            if (existingContact == null)
            {
                return NotFound(new ResponseModel<string> { Success = false, Message = "Contact not found." });
            }

            _mapper.Map(request, existingContact);
            return Ok(new ResponseModel<AddressBookEntryModel>
            {
                Success = true,
                Message = "Contact updated successfully.",
                Data = _mapper.Map<AddressBookEntryModel>(existingContact)
            });
        }
        /// <summary>
        /// Delete by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/addressbook/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteContact(int id)
        {
            var contactToDelete = _addressBookEntries.FirstOrDefault(e => e.Id == id);
            if (contactToDelete == null)
            {
                return NotFound(new ResponseModel<string> { Success = false, Message = "Contact not found." });
            }

            _addressBookEntries.Remove(contactToDelete);
            return NoContent();
        }
    }
}
