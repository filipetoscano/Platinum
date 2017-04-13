using System;
using System.Collections.Generic;

namespace Platinum.Validation
{
    /// <summary />
    public class RuleSet
    {
        private Dictionary<string, FieldRule> _fieldRules = new Dictionary<string, FieldRule>();


        /// <summary />
        public RuleSet()
        {
        }


        /// <summary>
        /// Gets the name of the rule set.
        /// </summary>
        /// <remarks>
        /// Name is automatically built, based on the names of the IValidationRuleSet
        /// types which are added to the rule set.
        /// </remarks>
        public string Name
        {
            get;
            private set;
        }


        /// <summary />
        public void Add<T>() where T : IValidationRuleSet
        {
            Type type = typeof( T );
            Add( type );
        }


        /// <summary />
        public void Add( Type ruleSet )
        {
            #region Validations

            if ( ruleSet == null )
                throw new ArgumentNullException( nameof( ruleSet ) );

            #endregion

            // TODO: Check if implements IValidationRuleSet

            AddRuleSet( ruleSet );
        }


        /// <summary />
        private void AddRuleSet( Type ruleSet )
        {
            #region Validations

            if ( ruleSet == null )
                throw new ArgumentNullException( nameof( ruleSet ) );

            #endregion

            var rs = Activator.Create<IValidationRuleSet>( ruleSet );

            foreach ( var r in rs.Fields )
            {
                if ( _fieldRules.ContainsKey( r.Name ) == true )
                    _fieldRules[ r.Name ] = r;
                else
                    _fieldRules.Add( r.Name, r );
            }

            if ( this.Name == null )
                this.Name = ruleSet.Name;
            else
                this.Name = ruleSet.Name + ">" + this.Name;
        }


        /// <summary />
        public IEnumerable<KeyValuePair<string, FieldRule>> FieldRules
        {
            get
            {
                return _fieldRules;
            }
        }
    }
}
