using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using AdventurousContacts.Models.DataModels;

namespace AdventurousContacts.Models.Repository
{
    public class EFRepository : IRepository
    {
        #region Fields

        /// <summary>
        /// Used by IDisposable methods
        /// </summary>
        private bool _disposed = false;

        private AdventurousContactsEntities _entities = new AdventurousContactsEntities();

        #endregion 

        #region IRepository methods

        #region Select methods

        /// <summary>
        /// Retrieve all contacts from the table.
        /// </summary>
        /// <returns>Data source of Contact objects</returns>
        public IQueryable<Contact> FindAllContacts()
        {
            return _entities.Contacts.AsQueryable();
        }

        /// <summary>
        /// Retrieve a specific post from the table given the ContactId.
        /// </summary>
        /// <param name="contactId">int Contact.ContactId</param>
        /// <returns>Contact object</returns>
        public Contact GetContactById(int contactId)
        {
            return _entities.Contacts.Find(contactId);
        }

        /// <summary>
        /// Retrieve the (default 20) last posts from the table.
        /// </summary>
        /// <param name="count">int amount of posts, preset to 20</param>
        /// <returns>List of Contact objects</returns>
        public List<Contact> GetLastContacts(int count = 20)
        {
            return _entities.Contacts.OrderByDescending(c => c.ContactID)
                .Take(count).ToList();
        }

        #endregion 

        #region Update, delete and save

        /// <summary>
        /// Add a new post of Contact type into the table.
        /// </summary>
        /// <param name="contact">Contact object</param>
        public void Add(Contact contact)
        {
            _entities.Contacts.Add(contact);
        }

        /// <summary>
        /// Delete a post from the table.
        /// </summary>
        /// <param name="contact">Contact object</param>
        public void DeleteContact(int contactId)
        {
            var match = _entities.Contacts.Find(contactId);
            _entities.Contacts.Remove(match);
        }
        
        /// <summary>
        /// Update a post in the table.
        /// </summary>
        /// <param name="contact">Contact object</param>
        public void Update(Contact contact)
        {
            _entities.Entry(contact).State = EntityState.Modified;
        }

        /// <summary>
        /// Save EF action
        /// </summary>
        public void Save()
        {
            _entities.SaveChanges();
        }

        #endregion        

        #endregion

        #region IDisposable methods
        
        /// <summary>
        /// Invokes Dispose(bool disposing) and GC to release sources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Used by Dispose()
        /// </summary>
        /// <param name="disposing">bool Release or not</param>
        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _entities.Dispose();
                }
            }
            _disposed = true;
        }

        #endregion
    }
}