using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SiteMonitoringApp
{
    /// <summary>
    /// Extension class for Web API controllers.
    /// </summary>
    public static class ControllerExtensions
    {
        /// <summary>
        /// Extension method to validate view model. Used in unit tests to trigger validation.
        /// </summary>
        /// <typeparam name="TViewModel">view model class</typeparam>
        /// <typeparam name="TController">controller class</typeparam>
        /// <param name="controller">controller</param>
        /// <param name="viewModelToValidate">view model to validate</param>
        public static void ValidateViewModel<TViewModel, TController>(this TController controller, TViewModel viewModelToValidate)
            where TController : Controller
        {
            var validationContext = new ValidationContext(viewModelToValidate, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(viewModelToValidate, validationContext, validationResults, true);
            foreach (var validationResult in validationResults)
            {
                controller.ModelState.AddModelError(validationResult.MemberNames.FirstOrDefault() ?? string.Empty, validationResult.ErrorMessage);
            }
        }
    }
}
