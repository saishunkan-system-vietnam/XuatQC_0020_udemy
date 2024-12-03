using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json;
using Validation.Models;

namespace Validation.CustomModelsBinders
{
    public class PersonModelBider : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            Person person = new Person();
            try
            {
                var contentType = bindingContext.ActionContext.HttpContext.Request.Headers.ContentType;

                if (contentType != "application/json")
                {
                    throw new Exception("not valid format");
                }

                string json;
                using (var reader = new StreamReader(bindingContext.ActionContext.HttpContext.Request.Body, Encoding.UTF8))
                {
                    json = reader.ReadToEnd();

                    dynamic data = JObject.Parse(json);

                    //person = JsonSerializer.Deserialize<Person>(json);

                    person = data.ToObject<Person>(); 
                    person.PersonName =  data.FirstName + " " + data.LastName;
                }
            }
            catch (Exception)
            {
                throw;
            }

            //if (bindingContext.ValueProvider.GetValue("FirstName").Length > 0)
            //{
            //    person.PersonName = bindingContext.ValueProvider.GetValue("FirstName").FirstValue;
            //}
            //if (bindingContext.ValueProvider.GetValue("LastName").Length > 0)
            //{
            //    person.PersonName += " " + bindingContext.ValueProvider.GetValue("LastName").FirstValue;
            //}

            //return Task.FromResult(person);

            bindingContext.Result = ModelBindingResult.Success(person);
            return Task.CompletedTask;
        }
    }
}
