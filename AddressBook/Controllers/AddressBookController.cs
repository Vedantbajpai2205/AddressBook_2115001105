using AutoMapper;
using BusinessLayer.Interface;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using System.Collections.Generic;
using System.Linq;

namespace AddressBook.Controllers
{
    [ApiController]
    [Route("api/addressbook")]
    public class AddressBookController : ControllerBase
    {
        private readonly IAddressBookBL _addressBookBL;
        private readonly IValidator<RequestModel> _validator;

        public AddressBookController(IAddressBookBL addressBookBL, IValidator<RequestModel> validator)
        {
            _addressBookBL = addressBookBL;
            _validator = validator;
        }

        // GET: Fetch all contacts
        [HttpGet]
        public ActionResult<ResponseModel<IEnumerable<AddressBookEntryModel>>> GetAll()
        {
            var contacts = _addressBookBL.GetAll();
            return Ok(new ResponseModel<IEnumerable<AddressBookEntryModel>>
            {
                Success = true,
                Message = "Contacts retrieved successfully.",
                Data = contacts
            });
        }

        // GET: Fetch contact by ID
        [HttpGet("get/{id}")]
        public ActionResult<ResponseModel<AddressBookEntryModel>> GetContactById(int id)
        {
            var contact = _addressBookBL.GetById(id);
            if (contact == null)
            {
                return NotFound(new ResponseModel<AddressBookEntryModel>
                {
                    Success = false,
                    Message = $"Contact with ID {id} not found.",
                    Data = null
                });
            }

            return Ok(new ResponseModel<AddressBookEntryModel>
            {
                Success = true,
                Message = "Contact retrieved successfully.",
                Data = contact
            });
        }

        // POST: Add new contact
        [HttpPost("add")]
        public ActionResult<ResponseModel<AddressBookEntryModel>> Add([FromBody] RequestModel dto)
        {
            var validationResult = _validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = "Validation failed.",
                    Data = validationResult.Errors.Select(e => e.ErrorMessage)
                });
            }

            var newContact = _addressBookBL.Add(dto);
            return CreatedAtAction(nameof(GetContactById), new { id = newContact.Id }, new ResponseModel<AddressBookEntryModel>
            {
                Success = true,
                Message = "Contact added successfully.",
                Data = newContact
            });
        }

        // PUT: Update contact
        [HttpPut("update/{id}")]
        public ActionResult<ResponseModel<AddressBookEntryModel>> Update(int id, [FromBody] RequestModel dto)
        {
            var validationResult = _validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = "Validation failed.",
                    Data = validationResult.Errors.Select(e => e.ErrorMessage)
                });
            }

            var updatedContact = _addressBookBL.Update(id, dto);
            if (updatedContact == null)
            {
                return NotFound(new ResponseModel<AddressBookEntryModel>
                {
                    Success = false,
                    Message = $"Contact with ID {id} not found.",
                    Data = null
                });
            }

            return Ok(new ResponseModel<AddressBookEntryModel>
            {
                Success = true,
                Message = "Contact updated successfully.",
                Data = updatedContact
            });
        }

        // DELETE: Delete contact
        [HttpDelete("delete/{id}")]
        public ActionResult<ResponseModel<string>> DeleteContact(int id)
        {
            var isDeleted = _addressBookBL.Delete(id);
            if (!isDeleted)
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = $"Contact with ID {id} not found.",
                    Data = null
                });
            }

            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "Contact deleted successfully.",
                Data = "Deleted"
            });
        }
    }
}