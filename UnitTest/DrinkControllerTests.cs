using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApi.Controllers;
using System.Collections.Generic;
using Database;
using System.Linq;
using System.Web.Http.Results;
using WebApi.Models;
using System;
using System.Collections;

namespace UnitTest
{
    [TestClass]
    public class DrinkControllerTests
    {
        private DrinkServiceMock service;
        private DrinkController controller;

        [TestInitialize]
        public void Initialize()
        {
            this.service = new DrinkServiceMock();
            this.controller = new DrinkController(this.service);
        }

        [TestMethod]
        public void WhenDrinkGetOk()
        {
            this.service.Drinks = new Dictionary<string, int> { ["Pepsi"] = 1, ["Fanta"] = 2 };

            var actual = this.controller.Get("Pepsi").Result;

            Assert.IsInstanceOfType(actual, typeof(OkNegotiatedContentResult<DrinkModel>));

            var content = ((OkNegotiatedContentResult<DrinkModel>)actual).Content;

            Assert.AreEqual(1, content.Quantity);
        }

        [TestMethod]
        public void WhenDrinkGetNotFound()
        {
            this.service.Drinks = new Dictionary<string, int> { ["Pepsi"] = 1 };

            var actual = this.controller.Get("Fanta").Result;

            Assert.IsInstanceOfType(actual, typeof(NotFoundResult));
        }

        [TestMethod]
        public void WhenDrinkPostOk()
        {
            this.service.Drinks = new Dictionary<string, int> { ["Pepsi"] = 1 };

            var model = new DrinkModel { Quantity = 2 };
            var actual = this.controller.Post("Fanta", model).Result;

            Assert.IsInstanceOfType(actual, typeof(OkResult));

            Assert.AreEqual(2, this.service.Drinks.Count);
            Assert.AreEqual(2, this.service.Drinks["Fanta"]);
            Assert.AreEqual(1, this.service.Drinks["Pepsi"]);
        }

        [TestMethod]
        public void WhenDrinkPostBadRequest()
        {
            this.service.Drinks = new Dictionary<string, int> { ["Pepsi"] = 1, ["Fanta"] = 2 };

            var model = new DrinkModel { Quantity = 3 };
            var actual = this.controller.Post("Fanta", model).Result;

            Assert.IsInstanceOfType(actual, typeof(BadRequestErrorMessageResult));

            Assert.AreEqual(2, this.service.Drinks.Count);
            Assert.AreEqual(2, this.service.Drinks["Fanta"]);
            Assert.AreEqual(1, this.service.Drinks["Pepsi"]);
        }

        [TestMethod]
        public void WhenDrinkPutOk()
        {
            this.service.Drinks = new Dictionary<string, int> { ["Pepsi"] = 1, ["Fanta"] = 2 };

            var model = new DrinkModel { Quantity = 3 };
            var actual = this.controller.Put("Fanta", model).Result;

            Assert.IsInstanceOfType(actual, typeof(OkResult));

            Assert.AreEqual(2, this.service.Drinks.Count);
            Assert.AreEqual(3, this.service.Drinks["Fanta"]);
            Assert.AreEqual(1, this.service.Drinks["Pepsi"]);
        }

        [TestMethod]
        public void WhenDrinkPutNotFound()
        {
            this.service.Drinks = new Dictionary<string, int> { ["Pepsi"] = 1 };

            var model = new DrinkModel { Quantity = 2 };
            var actual = this.controller.Put("Fanta", model).Result;

            Assert.IsInstanceOfType(actual, typeof(NotFoundResult));

            Assert.AreEqual(1, this.service.Drinks.Count);
            Assert.AreEqual(1, this.service.Drinks["Pepsi"]);
        }

        [TestMethod]
        public void WhenDrinkDeleteOk()
        {
            this.service.Drinks = new Dictionary<string, int> { ["Pepsi"] = 1, ["Fanta"] = 2 };

            var actual = this.controller.Delete("Pepsi").Result;

            Assert.IsInstanceOfType(actual, typeof(OkResult));

            Assert.AreEqual(1, this.service.Drinks.Count);
            Assert.AreEqual(2, this.service.Drinks["Fanta"]);
        }

        [TestMethod]
        public void WhenDrinkDeleteNotFound()
        {
            this.service.Drinks = new Dictionary<string, int> { ["Pepsi"] = 1 };

            var actual = this.controller.Delete("Fanta").Result;

            Assert.IsInstanceOfType(actual, typeof(NotFoundResult));

            Assert.AreEqual(1, this.service.Drinks.Count);
            Assert.AreEqual(1, this.service.Drinks["Pepsi"]);
        }
    }
}
