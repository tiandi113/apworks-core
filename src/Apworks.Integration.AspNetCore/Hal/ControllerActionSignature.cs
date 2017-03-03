﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apworks.Integration.AspNetCore.Hal
{
    /// <summary>
    /// Represents the data structure that holds both controller/action names and the types of method parameters
    /// that can uniquely identify a controller action.
    /// </summary>
    /// <seealso cref="System.IEquatable{Apworks.Integration.AspNetCore.Hal.ControllerActionSignature}" />
    public sealed class ControllerActionSignature : IEquatable<ControllerActionSignature>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerActionSignature"/> class.
        /// </summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        public ControllerActionSignature(string controllerName, string actionName)
        {
            this.ControllerName = controllerName;
            this.ActionName = actionName;
            this.ParameterTypes = Type.EmptyTypes;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerActionSignature"/> class.
        /// </summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="parameterTypes">The parameter types.</param>
        public ControllerActionSignature(string controllerName, string actionName, IEnumerable<Type> parameterTypes)
        {
            this.ControllerName = controllerName;
            this.ActionName = actionName;
            this.ParameterTypes = new List<Type>(parameterTypes);
        }

        public string ControllerName { get; }

        public string ActionName { get; }

        public IEnumerable<Type> ParameterTypes { get; }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(ControllerActionSignature other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            var equals = !string.IsNullOrEmpty(this.ControllerName) &&
                !string.IsNullOrEmpty(this.ActionName) &&
                this.ControllerName.Equals(other.ControllerName, StringComparison.CurrentCultureIgnoreCase) &&
                this.ActionName.Equals(other.ActionName, StringComparison.CurrentCultureIgnoreCase) &&
                this.ParameterTypes.All(parameterType => other.ParameterTypes.Any(otherParameterType => otherParameterType.Equals(parameterType)));

            return equals;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            var result = this.ControllerName?.GetHashCode() ^
                this.ActionName?.GetHashCode() ^
                this.ParameterTypes?.GetHashCode();
            return result != null && result.HasValue ? result.Value : base.GetHashCode();
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator == (ControllerActionSignature a, ControllerActionSignature b)
        {
            if ((object)a == null)
            {
                return (object)b == null;
            }

            return a.Equals(b);
        }

        public static bool operator != (ControllerActionSignature a, ControllerActionSignature b)
        {
            return !(a == b);
        }

        public static implicit operator ControllerActionSignature (string src)
        {
            if (!src.Contains('.'))
            {
                throw new InvalidCastException("Cannot cast the given string into a ControllerActionSignature object.");
            }

            var ctrlActionNames = src.Split('.');
            return new ControllerActionSignature(ctrlActionNames[0], ctrlActionNames[1]);
        }
    }
}
