using App.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace App.Services.Filters
{
    public class NotFoundFilter<T, TId> : Attribute, IAsyncActionFilter where T : class where TId : struct
    {
        private readonly IGenericRepository<T, TId> _genericRepository;

        public NotFoundFilter(IGenericRepository<T, TId> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //var idValue = context.ActionArguments.Values.FirstOrDefault();

            //var idKey = context.ActionArguments.Keys.First().ToLower();

            //if (idValue == null && !idKey.Contains("id"))
            //{
            //    await next();
            //    return;
            //}

            var idValue = context.ActionArguments.TryGetValue("id", out var idAsObject) ? idAsObject : null;

            #region Eğer isteğin body'sinde obje olursa
            //var objectValue = context.ActionArguments.Values.FirstOrDefault();

            //var idProperty = objectValue?.GetType().GetProperty("Id");
            //var objectIdValue = idProperty?.GetValue(objectValue);

            //if (objectIdValue is TId objectsId)
            //{
            //    var hasEntity = await _genericRepository.AnyAsync(objectsId);

            //    if (hasEntity)
            //    {
            //        await next();
            //        return;
            //    }

            //    var contextResult = ServiceResult.Fail($"Data bulunamamıştır.", HttpStatusCode.NotFound);

            //    context.Result = new NotFoundObjectResult(contextResult);

            //    return;
            //} 
            #endregion

            if (idAsObject is not TId id)
            {
                await next();
                return;
            }

            //if(!int.TryParse(idValue.ToString(), out int id))
            //{
            //    await next();
            //    return;
            //}   

            //if (idAsObject is not TId id)
            //{
            //    await next();
            //    return;
            //}

            var anyEntity = await _genericRepository.AnyAsync(id);

            if (anyEntity)
            {
                await next();
                return;
            }

            var entityName = typeof(T).Name;

            // action method name
            var actionName = context.ActionDescriptor.RouteValues["action"];

            var result = ServiceResult.Fail($"Data bulunamamıştır. ({entityName})({actionName})", HttpStatusCode.NotFound);

            context.Result = new NotFoundObjectResult(result);

            return;
        }
    }
}
