using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventurousContacts.Models.Repository
{
    /// <summary>
    /// Implemented by different repository classes.
    /// Also, implements IDisposable for closing connection.
    /// </summary>
    public interface IRepository : IDisposable
    {
        void Add(Contact contact);
        void DeleteContact(int contactId);
        IQueryable<Contact> FindAllContacts();
        Contact GetContactById(int contactId);
        List<Contact> GetLastContacts(int count = 20);
        void Save();       
        void Update(Contact contact);
    }
}
