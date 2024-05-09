using ProjectScope.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectScope.Data.Repo
{
    public class SqlContactTable : IContact
    {
        private readonly EntityDbContext _dbContext;
        private Contact contact;

        public SqlContactTable(EntityDbContext entityDbContext)
        {
            _dbContext = entityDbContext;
        }
        public void Delete(int Id)
        {
            Student student = _dbContext.Students.FirstOrDefault(a => a.Id == Id);
            if (student != null)
            {
                _dbContext.Students.Remove(student);
                _dbContext.SaveChanges();
            }
        }

        public Contact Get(string Email)
        {
            return _dbContext.Contacts.FirstOrDefault(a => a.Email == Email);
        }

        public List<Contact> Getall()
        {
            return _dbContext.Contacts.ToList();
        }

        public Contact GetById(int Id)
        {
            return _dbContext.Contacts.FirstOrDefault(a => a.Id == Id);

        }

        public void Insert(Contact contactView)
        {
            _dbContext.Contacts.Add(contactView);
            _dbContext.SaveChanges();
        }

        public void SaveChanges(int id)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            _dbContext.Contacts.Add(contact);
            _dbContext.SaveChanges();
        }

        public void Update(Contact contactView)
        {
            _dbContext.Contacts.Update(contactView);
            _dbContext.SaveChanges();
        }
    }
}
