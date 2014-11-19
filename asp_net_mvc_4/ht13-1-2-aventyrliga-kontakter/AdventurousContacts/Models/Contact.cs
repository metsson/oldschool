using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdventurousContacts.Models
{
    [MetadataType(typeof(Contact_MetaData))]
    public partial class Contact
    {
        public Contact()
        {
            status = Status.NoChange;
        }        
        
        #region Contact status methods

        /// <summary>
        /// The status of the contact, set by ContactController
        /// </summary>
        private Status status;

        public void NewContact()
        {
            status = Status.New;
        }

        public void ModifiedContact()
        {
            status = Status.Modified;
        }

        public void DeletedContact()
        {
            status = Status.Deleted;
        }

        public bool HasStatus()
        {
            return status != Status.NoChange;
        }        
        
        public bool IsNew()
        {
            return status == Status.New;
        }        
        
        public bool IsModified()
        {
            return status == Status.Modified;
        }

        public bool IsDeleted()
        {
            return status == Status.Deleted;
        }

        #endregion
    }

    public class Contact_MetaData 
    {
        public int ContactID { get; set; }

        [Required(ErrorMessage="Vänligen ange en mailaddress.")]
        [MaxLength(50, ErrorMessage="Mailadressen får inte vara längre än 50 tecken.")]
        [DisplayName("Mailadress")]
        [EmailAddress(ErrorMessage="Vänligen ange en giltig mailadress.")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage="Vänligen ange ett förnamn.")]
        [MaxLength(50, ErrorMessage="Förnamnet får inte vara längre än 50 tecken.")]
        [DisplayName("Förnamn")]
        public string FirstName { get; set; }

        [Required(ErrorMessage="Vänligen ange ett efternamn.")]
        [MaxLength(50, ErrorMessage="Efternamnet får inte vara längre än 50 tecken.")]
        [DisplayName("Efternamn")]
        public string LastName { get; set; }
    }

    /// <summary>
    /// Used as flags by ContactController to render proper
    /// feedback message upon user action.
    /// </summary>
    public enum Status { NoChange, New, Modified, Deleted }
}