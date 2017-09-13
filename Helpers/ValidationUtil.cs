using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneMusix.Helpers
{
    /// <summary>
    /// Class with utility methods for validation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ValidationUtil<T>
    {
        /// <summary>
        /// Validates of an object is null or already contained in the List.
        /// </summary>
        /// <param name="ObjectName">Name of the Object to Insert.</param> 
        /// <param name="ListObjectName">Name of the object that is to be inserted in.</param>
        /// <param name="toValidate">Object to Insert.</param>
        /// <param name="validationList">List to Insert to.</param>
        /// <returns></returns>
        public bool insertValidation(string ObjectName, string ListObjectName,T toValidate,ObservableCollection<T> validationList)
        {
            if(toValidate == null)
            {
                throw new ArgumentNullException("Element is Null");
            }else if (validationList.Contains(toValidate))
            {
                Logger.Log( ObjectName + " is already contained in " + ListObjectName + ".");
                return false;
            }
            else
            {
                return (true);
            }
        }

    }
}
