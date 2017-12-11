using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using HRMapp.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HRMapp_Tests
{
    [TestClass]
    public class ModelTestProductionTask
    {
        [TestMethod]
        public void ProductionTaskCtor1()
        {
            // vars
            var id = 1;
            var product = new Product(1);
            var name = "Taak1";
            var description = "Een omschrijving";
            var duration = new TimeSpan(3,0,0);
            var employees = new List<ProductionEmployee>();
            ProductionTask productionTask = null;

            // manipulation
            productionTask = new ProductionTask(
                id: id,
                product: product,
                name: name,
                description: description,
                duration: duration,
                employees: employees 
            );

            // checks
            Assert.AreNotEqual(null, productionTask);
            Assert.AreEqual(id, productionTask.Id);
            Assert.AreEqual(product.Id, productionTask.Product.Id);
            Assert.AreEqual(name, productionTask.Name);
            Assert.AreEqual(description, productionTask.Description);
            Assert.AreEqual(duration, productionTask.Duration);
            Assert.AreEqual(employees.Count, productionTask.Employees.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "A empty name was inappropiaty allowed")]
        public void ProductionTaskCtor2NoName()
        {
            // vars
            var id = 1;
            var product = new Product(1);
            var name = "";
            var description = "Een omschrijving";
            var duration = new TimeSpan(3, 0, 0);
            var employees = new List<ProductionEmployee>();
            ProductionTask productionTask = null;

            // manipulation
            productionTask = new ProductionTask(
                id: id,
                product: product,
                name: name,
                description: description,
                duration: duration,
                employees: employees
            );
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "A empty description was inappropiaty allowed")]
        public void ProductionTaskCtor3NoDescription()
        {
            // vars
            var id = 1;
            var product = new Product(1);
            var name = "Taak1";
            var description = "";
            var duration = new TimeSpan(3, 0, 0);
            var employees = new List<ProductionEmployee>();
            ProductionTask productionTask = null;

            // manipulation
            productionTask = new ProductionTask(
                id: id,
                product: product,
                name: name,
                description: description,
                duration: duration,
                employees: employees
            );
        }
    }
}
