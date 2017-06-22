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
    public class CartControllerTests
    {
        private DrinkServiceMock service;
        private CartController controller;

        [TestInitialize]
        public void Initialize()
        {
            this.service = new DrinkServiceMock();
            this.controller = new CartController(this.service);
        }

        [TestMethod]
        public void WhenCartGetOk()
        {
            this.service.Drinks = new Dictionary<string, int> { ["Pepsi"] = 1, ["Fanta"] = 2 };

            var actual = this.controller.Get().Result;

            Assert.IsInstanceOfType(actual, typeof(OkNegotiatedContentResult<CartModel[]>));

            var content = ((OkNegotiatedContentResult<CartModel[]>)actual).Content.ToList();

            Assert.AreEqual(2, content.Count);
            Assert.AreEqual("Fanta", content[0].Drink);
            Assert.AreEqual(2, content[0].Quantity);
            Assert.AreEqual("Pepsi", content[1].Drink);
            Assert.AreEqual(1, content[1].Quantity);
        }

        [TestMethod]
        public void WhenCartPostOk()
        {
            this.service.Drinks = new Dictionary<string, int> { ["Pepsi"] = 1 };

            var model = new CartModel { Drink = "Fanta", Quantity = 2 };
            var actual = this.controller.Post(model).Result;

            Assert.IsInstanceOfType(actual, typeof(OkResult));

            Assert.AreEqual(2, this.service.Drinks.Count);
            Assert.AreEqual(2, this.service.Drinks["Fanta"]);
            Assert.AreEqual(1, this.service.Drinks["Pepsi"]);
        }

        [TestMethod]
        public void WhenCartPostBadRequest()
        {
            this.service.Drinks = new Dictionary<string, int> { ["Pepsi"] = 1, ["Fanta"] = 2 };

            var model = new CartModel { Drink = "Fanta", Quantity = 3 };
            var actual = this.controller.Post(model).Result;

            Assert.IsInstanceOfType(actual, typeof(BadRequestErrorMessageResult));

            Assert.AreEqual(2, this.service.Drinks.Count);
            Assert.AreEqual(2, this.service.Drinks["Fanta"]);
            Assert.AreEqual(1, this.service.Drinks["Pepsi"]);
        }

        [TestMethod]
        public void WhenCartPutOk()
        {
            this.service.Drinks = new Dictionary<string, int> { ["Pepsi"] = 1, ["Fanta"] = 2 };

            var model = new CartModel { Drink = "Fanta", Quantity = 3 };
            var actual = this.controller.Put(model).Result;

            Assert.IsInstanceOfType(actual, typeof(OkResult));

            Assert.AreEqual(2, this.service.Drinks.Count);
            Assert.AreEqual(3, this.service.Drinks["Fanta"]);
            Assert.AreEqual(1, this.service.Drinks["Pepsi"]);
        }

        [TestMethod]
        public void WhenCartPutNotFound()
        {
            this.service.Drinks = new Dictionary<string, int> { ["Pepsi"] = 1 };

            var model = new CartModel { Drink = "Fanta", Quantity = 3 };
            var actual = this.controller.Put(model).Result;

            Assert.IsInstanceOfType(actual, typeof(BadRequestErrorMessageResult));

            Assert.AreEqual(1, this.service.Drinks.Count);
            Assert.AreEqual(1, this.service.Drinks["Pepsi"]);
        }

        [TestMethod]
        public void WhenCartDeleteOk()
        {
            this.service.Drinks = new Dictionary<string, int> { ["Pepsi"] = 1, ["Fanta"] = 2 };

            var actual = this.controller.Delete().Result;

            Assert.IsInstanceOfType(actual, typeof(OkResult));

            Assert.AreEqual(0, this.service.Drinks.Count);
        }
    }
}
