using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Validation.CustomModelsBinders
{
    public interface IPersonModelBinderProvider
    {
        IModelBinder GetBinder(ModelBinderProviderContext context);
    }
}