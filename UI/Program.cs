using Domain;
using Interfaces;
using Implementations;
using Microsoft.Practices.Unity;
using System;

namespace UI
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new UnityContainer();
#if (!AZURE)

            container.RegisterType<IRepository<Customer>, AzureRepository>();
#else
            container.RegisterType<IRepository<Customer>, DummyRepository>();
#endif

            IRepository<Customer> db = container.Resolve<IRepository<Customer>>();

            foreach (var cst in db.GetAll())
            {
                Console.WriteLine(cst.id + " - " + cst.Name + " " + cst.LastName);
            }

            Console.ReadLine();
        }
    }
}
