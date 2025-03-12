using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model; // Include your models
using Microsoft.EntityFrameworkCore;
using System.Linq;
using RepositoryLayer.Context;

namespace AddressBook.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressBookController : ControllerBase
    {

    }
}
