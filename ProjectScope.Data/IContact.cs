using ProjectScope.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectScope.Data
{
    public interface IContact
    {
        void Insert(Contact contactView);
        void Update(Contact contactView);
        void Delete(int Id);
        List<Contact> Getall();
        Contact GetById(int Id);
        Contact Get(string Email);
        void SaveChanges();
    }
}
