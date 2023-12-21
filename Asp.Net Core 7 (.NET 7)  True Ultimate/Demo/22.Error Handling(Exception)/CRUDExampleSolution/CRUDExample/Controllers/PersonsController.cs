using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDExample.Controllers
{
    [Route("persons")]
    public class PersonsController : Controller
    {
        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;

        public PersonsController(IPersonService personService, ICountriesService countriesService)
        {
            _personService = personService;
            _countriesService = countriesService;
        }

        [Route("index")]
        [Route("/")]
        public async Task<IActionResult> Index(string searchBy, string? searchValue,
            string sortBy = nameof(PersonResponse.PersonName), SortedOrderOptions sortOrder = SortedOrderOptions.ASC)
        {
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                {nameof(PersonResponse.PersonName), "Person Name" },
                {nameof(PersonResponse.Email), "Email" },
                {nameof(PersonResponse.DateOfBirth), "Date of birth" },
                {nameof(PersonResponse.Gender), "Gender" },
                {nameof(PersonResponse.CountryID), "Country" },
                {nameof(PersonResponse.Address), "Address" },
                {nameof(PersonResponse.Age), "Age" },
            };

            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearhValue = searchValue;
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder;
            ViewBag.NextOrder = sortOrder == SortedOrderOptions.ASC ? SortedOrderOptions.DESC : SortedOrderOptions.ASC;

            // Search
            List<PersonResponse> persons = _personService.GetFilteredPersons(searchBy, searchValue);

            // Sort
            List<PersonResponse> personsSorted = _personService.GetSortedPersons(persons, sortBy, sortOrder);
            return View(personsSorted);
        }

        [Route("create")]
        [HttpGet]
        public IActionResult Create()
        {
            GetAllCountries();

            return View();
        }

        [Route("create")]
        [HttpPost]
        public IActionResult Create(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                GetAllCountries();
                return View();
            }

            // call the service to add person to the persons list
            PersonResponse personResponse = _personService.AddPerson(personAddRequest);

            // redirect to the list page
            return RedirectToAction("Index", "persons");
        }

        [HttpGet]
        [Route("edit/{personID}")]
        public IActionResult Edit(Guid personID)
        {
            PersonResponse? personResponse = _personService.GetPersonById(personID);

            if (personResponse == null)
            {
                return RedirectToAction("Index", "persons");
            }

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();

            GetAllCountries();

            return View(personUpdateRequest);
        }


        [HttpPost]
        [Route("edit/{personID}")]
        public IActionResult Edit(PersonUpdateRequest personUpdateRequest)
        {
            PersonResponse? personResponse = _personService.GetPersonById(personUpdateRequest.PersonID);

            if (personResponse == null)
            {
                return RedirectToAction("Index", "Persons");
            }

            if (ModelState.IsValid)
            {
                // for test exception
                personUpdateRequest.PersonID = Guid.NewGuid();
                PersonResponse updatePersonResponse = _personService.UpdatePerson(personUpdateRequest);
                return RedirectToAction("Index", "Persons");
            }

            return View();
        }


        [HttpGet]
        [Route("[action]/{personID}")]
        public IActionResult Delete(Guid personID)
        {
            PersonResponse? personResponse = _personService.GetPersonById(personID);

            if (personResponse == null)
            {
                return RedirectToAction("Index", "Persons");
            }

            if (ModelState.IsValid)
            {
                _personService.DetletePerson(personID);
                return RedirectToAction("Index", "Persons");
            }

            return View();
        }

        [Route("person-pdf")]
        public IActionResult PersonPDF()
        {
            List<PersonResponse> persons = _personService.GetAllPersons();


            return new ViewAsPdf("PersonPDF", persons, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins()
                {
                    Top = 20,
                    Bottom = 20,
                    Left = 20,
                    Right = 20
                },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }

        private List<CountryResponse> GetAllCountries()
        {
            List<CountryResponse> countries = _countriesService.GetAllCountries();

            ViewBag.Countries = countries.Select(temp => new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() });
            return countries;
        }
    }
}
