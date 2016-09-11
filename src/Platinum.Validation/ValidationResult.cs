using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Platinum.Validation
{
    public class ValidationResult
    {
        private List<ValidationException> _errors;


        public ValidationResult()
        {
            _errors = new List<ValidationException>();
        }


        /// <summary>
        /// Gets whether the validated object was valid.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return _errors.Count == 0;
            }
        }


        /// <summary>
        /// Gets the collection of errors.
        /// </summary>
        public ReadOnlyCollection<ValidationException> Errors
        {
            get
            {
                return _errors.AsReadOnly();
            }
        }


        public void AddError( ValidationException error )
        {
            #region Validations

            if ( error == null )
                throw new ArgumentNullException( nameof( error ) );

            #endregion

            _errors.Add( error );
        }
    }
}
