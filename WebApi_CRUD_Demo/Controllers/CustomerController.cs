using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi_CRUD_Demo.Models;
using WebApi_CRUD_Demo.Models.ViewModel;

namespace WebApi_CRUD_Demo.Controllers
{
    public class CustomerController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetAllCustomers()
        {
            IList<CustomerViewModel> customers = null;
            using(var x= new WebApiDemoDBEntities())
            {
                customers = x.Customers.Select(c => new CustomerViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    Address = c.Address,
                    Phone = c.Phone
                }).ToList<CustomerViewModel>();
                if (customers.Count == 0)
                {
                    return NotFound();
                }
                return Ok(customers);
            }
        }
        [HttpPost]
        public IHttpActionResult CreateCustomer(CustomerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data.");
            }
            using (var customer = new WebApiDemoDBEntities())
            {
                customer.Customers.Add(new Customer()
                {
                    Name = model.Name,
                    Phone = model.Phone,
                    Address = model.Address,
                    Email = model.Email

                });
                customer.SaveChanges();
            }
            return Ok();
        }

        [HttpPut]
        public IHttpActionResult UpdateCustomer(CustomerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data.");
            }
            using (var customer = new WebApiDemoDBEntities())
            {
              var checkExistingCustomer= customer.Customers.Where(c => c.Id == model.Id).FirstOrDefault<Customer>();
                if (checkExistingCustomer != null)
                {
                    checkExistingCustomer.Name = model.Name;    
                    checkExistingCustomer.Phone = model.Phone;
                    checkExistingCustomer.Address = model.Address;
                    checkExistingCustomer.Email = model.Email;
                    customer.SaveChanges();
                }
                else
                {
                    return NotFound();
                }             
            }
            return Ok();
        }
        [HttpDelete]
        public IHttpActionResult DeleteCustomer(int id)
        {
            if(id <= 0)
            {
                return BadRequest("Invaid Customer Id");
            }
            using (var x = new WebApiDemoDBEntities())
            {
                var customer=x.Customers.Where(c=>c.Id == id).FirstOrDefault();
                x.Entry(customer).State=System.Data.Entity.EntityState.Deleted;
                x.SaveChanges();
            }
            return Ok();
        }
    }
}
