using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Platinum.Validation
{
    /// <summary />
    public class ValidationResult
    {
        private List<ActorException> _errors;


        /// <summary />
        public ValidationResult()
        {
            _errors = new List<ActorException>();
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
        public ReadOnlyCollection<ActorException> Errors
        {
            get
            {
                return _errors.AsReadOnly();
            }
        }


        /// <summary />
        public void AddError( ActorException error )
        {
            #region Validations

            if ( error == null )
                throw new ArgumentNullException( nameof( error ) );

            #endregion

            _errors.Add( error );
        }
    }
}
