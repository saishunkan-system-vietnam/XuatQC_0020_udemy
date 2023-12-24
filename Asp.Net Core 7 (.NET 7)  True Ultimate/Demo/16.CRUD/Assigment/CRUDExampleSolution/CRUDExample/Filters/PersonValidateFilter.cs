using CRUDExample.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using ServiceContracts.DTO;

namespace CRUDExample.Filters
{
    public class PersonValidateFilter : IAsyncActionFilter
    {
        private readonly ILogger<PersonValidateFilter> _logger;

        public PersonValidateFilter(ILogger<PersonValidateFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Controller is PersonsController personController)
            {
                var actionName = context.RouteData.Values["action"];
                if (actionName.ToString() == "Create")
                {
                    var personAddRequest = context.ActionArguments["personAddRequest"] as PersonAddRequest;

                    if (personAddRequest != null)
                    {
                        //re-validate the model object after updating the date
                        personController.ModelState.Clear();
                        personController.TryValidateModel(personAddRequest);

                        if (!personController.ModelState.IsValid)
                        {
                           
                            var errorList = personController.ModelState.Values.SelectMany(v => v.Errors).ToList();
                            personController.ViewBag.Errors = errorList.Select(e => e.ErrorMessage).ToList();
                            
                            foreach (var error in errorList)
                            {
                                _logger.LogWarning("please check the values input {type} {errorMessage}", error.GetType(), error.ErrorMessage);
                            }

                            personController.GetAllCountries();
                            context.Result = personController.View(nameof(personController.Create));
                        }
                        else
                        {
                            await next();
                        }
                    }
                    else
                    {
                        await next();
                    }
                }
                else if (actionName.ToString() == "Edit")
                {
                    var personUpdateRequest = context.ActionArguments["personUpdateRequest"] as PersonUpdateRequest;

                    if (personUpdateRequest != null)
                    {
                        //re-validate the model object after updating the date
                        personController.ModelState.Clear();
                        personController.TryValidateModel(personUpdateRequest);

                        if (!personController.ModelState.IsValid)
                        {
                            var errorList = personController.ModelState.Values.SelectMany(v => v.Errors).ToList();
                            personController.ViewBag.Errors = errorList.Select(e => e.ErrorMessage).ToList();

                            foreach (var error in errorList)
                            {
                                _logger.LogWarning("please check the values input {type} {errorMessage}", error.GetType(), error.ErrorMessage);
                            }

                            personController.GetAllCountries();
                            context.Result = personController.View(nameof(personController.Edit), personUpdateRequest);
                        }
                        else
                        {
                            await next();
                        }
                    }
                    else
                    {
                        await next();
                    }
                }
            }
        }
    }
}
