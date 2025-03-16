using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using BuisnessLayer.Interface;
using Newtonsoft.Json;
using BuisnessLayer.Interface;
using ModelLayer.Model;

namespace AddressBook.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressBookController : ControllerBase
    {
        private readonly IAddressBookBL _addressBookBL;

        public AddressBookController(IAddressBookBL addressBookBL)
        {
            _addressBookBL = addressBookBL;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var contacts = _addressBookBL.GetAll();

            ResponseModel<IEnumerable<AddressBookEntryModel>> responseModel = new ResponseModel<IEnumerable<AddressBookEntryModel>>
            {
                Success = true,
                Message = "All contacts fetched successfully",
                Data = contacts
            };

            return Ok(responseModel);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var contact = _addressBookBL.GetById(id);

            if (contact == null)
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = $"Contact with ID {id} not found",
                    Data = null
                });
            }

            ResponseModel<AddressBookEntryModel> responseModel = new ResponseModel<AddressBookEntryModel>
            {
                Success = true,
                Message = $"Contact with ID {id} fetched successfully",
                Data = contact
            };

            return Ok(responseModel);
        }

        [HttpPost]
        public ActionResult<ResponseModel<AddressBookEntryModel>> Add([FromBody] RequestModel requestModel)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = "Validation failed.",
                    Data = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            var addedContact = _addressBookBL.Add(requestModel);

            return CreatedAtAction(nameof(GetById), new { id = addedContact.Id }, new ResponseModel<AddressBookEntryModel>
            {
                Success = true,
                Message = "Contact added successfully.",
                Data = addedContact
            });
        }



        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] AddressBookEntryModel addressBookModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedContact = _addressBookBL.Update(id, addressBookModel);

            if (updatedContact == null)
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = $"Contact with ID {id} not found",
                    Data = null
                });
            }

            ResponseModel<AddressBookEntryModel> responseModel = new ResponseModel<AddressBookEntryModel>
            {
                Success = true,
                Message = $"Contact with ID {id} updated successfully",
                Data = updatedContact
            };

            return Ok(responseModel);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var isDeleted = _addressBookBL.Delete(id);

            if (!isDeleted)
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = $"Contact with ID {id} not found",
                    Data = null
                });
            }

            ResponseModel<string> responseModel = new ResponseModel<string>
            {
                Success = true,
                Message = $"Contact with ID {id} deleted successfully",
                Data = "Deletion successful"
            };

            return Ok(responseModel);
        }
    }
}