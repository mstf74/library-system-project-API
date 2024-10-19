using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Models.CustomAttributes
{
    public class LessCopiesAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return null;

            int less = (int) value;
            var Copies = (Book)validationContext.ObjectInstance; 
            if(less <= Copies.NumberOfCopies)
                return ValidationResult.Success;
            return new ValidationResult("The available Copies must be less than Number of copies");
        }
    }
}
