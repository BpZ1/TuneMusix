using System;
using System.Collections.ObjectModel;

namespace TuneMusix.Helpers
{
    /// <summary>
    /// Class with utility methods for validation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class ValidationUtil
    {
        /// <summary>
        /// Validates of an object is null or already contained in the List.
        /// </summary>
        /// <param name="toValidate">Object to Insert.</param>
        /// <param name="validationList">List to Insert to.</param>
        /// <returns></returns>
        public static bool NotContainedOrNull<T>(T toValidate, ObservableCollection<T> validationList)
        {
            if(toValidate == null)
            {
                throw new ArgumentNullException("Element is Null");
            }
            else if (validationList != null)
            {
                if (validationList.Contains(toValidate))
                {
                    return false;
                }
                return true;
            }
            else
            {
                throw new ArgumentException("Argument is null");
            }
        }

    }
}
