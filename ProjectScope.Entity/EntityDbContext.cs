using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectScope.Entity
{
    public class EntityDbContext:DbContext
    {
        public EntityDbContext(DbContextOptions<EntityDbContext> options) : base(options)
        {

        }   
        public DbSet<Student> Students { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Course> Course { get; set; }

    }
}
