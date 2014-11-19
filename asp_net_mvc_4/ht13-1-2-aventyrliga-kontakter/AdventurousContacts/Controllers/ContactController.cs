using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdventurousContacts.Models;
using AdventurousContacts.Models.Repository;

namespace AdventurousContacts.Controllers
{
    public class ContactController : Controller
    {
        #region IRepository handler and constructor

        private IRepository _repository;

        public ContactController()
            : this(new EFRepository())
        {
            // Chained        
        }

        public ContactController(IRepository repository)
        {
            _repository = repository;
        }

        #endregion

        #region List contacts

        // GET: /Contact/
        public ActionResult Index()
        {
            return View("Index", _repository.GetLastContacts(10));
        }

        #endregion

        #region Edit and delete contacts

        // GET
        public ActionResult Edit(int id = 0)
        {
            var contact = _repository.GetContactById(id);

            return View("Edit", contact);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Contact contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _repository.Update(contact);
                    _repository.Save();
                    contact.ModifiedContact();
                    return View("Action", contact);
                }
                catch (Exception)
                {
                    return RedirectToAction("Index");
                }
            }

            return View("Index");
        }

        // GET
        public ActionResult Delete(int id = 0)
        {
            var contact = _repository.GetContactById(id);
            return View("Delete", contact);

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var contact = _repository.GetContactById(id);
                _repository.DeleteContact(id);
                _repository.Save();
                contact.DeletedContact();
                return View("Action", contact);
            }
            catch (Exception)
            {
                // Eat
            }

            return RedirectToAction("Index");
        }

        #endregion

        #region Create a contact

        // GET
        public ActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstName, LastName, EmailAddress")]Contact contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _repository.Add(contact);
                    _repository.Save();
                    contact.NewContact();
                    TempData.Add("newContact", contact);
                    return RedirectToAction("Action");
                }
                catch (Exception)
                {
                    ModelState.AddModelError(String.Empty,
                        "Ett fel inträffade då kontakten skulle läggas till.");
                }
            }
            return View("Index");
        }

        public ActionResult Action() 
        {            
            return View("Action", TempData["newContact"]);
        }

        #endregion
    }
}
