using Interfaces;
using System.Collections.Generic;
using System;
using Domain;
using System.Linq;
using System.Threading.Tasks;

namespace Implementations
{
    public class DummyRepository: IRepository<Customer>
    {
        private List<Customer> _db;

        public DummyRepository() {
            _db = new List<Customer>();
            _db.Add(new Customer() {
                id = "1",
                Name = "Angel (Fake)",
                LastName = "Taborda",
                Email = "atabordachinea@gmail.com"
            });

            _db.Add(new Customer()
            {
                id = "2",
                Name = "Nathaly (Fake)",
                LastName = "Rodriguez",
                Email = "nathy@domain.com"
            });

            _db.Add(new Customer()
            {
                id = "3",
                Name = "Adam (Fake)",
                LastName = "Mumma",
                Email = "adam@domain.com"
            });
        }

        public async Task<bool> Add(Customer obj)
        {
            try
            {
                _db.Add(obj);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Delete(string id)
        {
            try
            {
                var cust = _db.Find(x => x.id == id);
                if (cust != null)
                {
                    _db.Remove(cust);
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Customer> GetAll()
        {
            return _db;
        }

        public Customer GetById(string id)
        {
            return _db.Where(x => x.id == id).FirstOrDefault();
        }
    }
}
