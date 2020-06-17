﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using CL_LB1.Entities;
using CL_LB1.Data;
using WB_MVC.Controllers;
using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.AspNetCore.Http;

namespace CL_LBxUnitTest
{
    public class ProductControllerTests
    {

        DbContextOptions<ApplicationDbContext> _options;
        public ProductControllerTests()
        {
            _options =
            new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "aspnet-WB_MVC-Data")
            .Options;
        }

        [Theory]
        [MemberData(nameof(TestData.Params), MemberType = typeof(TestData))]
        public void ControllerGetsProperPage(int page, int qty, int id)
        {
            // Arrange
            //var controller = new ProductController(context);
            //controller._dishes = TestData.GetDishesList();
            //// Act
            //var result = controller.Index(pageNo:page, group:null) as ViewResult;
            //var model = result?.Model as List<Dish>;
            //// Assert
            //Assert.NotNull(model);
            //Assert.Equal(qty, model.Count);
            //Assert.Equal(id, model[0].DishId);

            // Arrange
            // Контекст контроллера
            var controllerContext = new ControllerContext();
            // Макет HttpContext
            var moqHttpContext = new Mock<HttpContext>();
            moqHttpContext.Setup(c => c.Request.Headers)
            .Returns(new HeaderDictionary());
            controllerContext.HttpContext = moqHttpContext.Object;
            //заполнить DB данными
            using (var context = new ApplicationDbContext(_options))
            {
                TestData.FillContext(context);
            }
            using (var context = new ApplicationDbContext(_options))
            {
                // создать объект класса контроллера
                var controller = new ProductController(context)
                { ControllerContext = controllerContext };
                // Act
                var result = controller.Index(group: null, pageNo: page) as ViewResult;
                var model = result?.Model as List<Dish>;
                // Assert
                Assert.NotNull(model);
                Assert.Equal(qty, model.Count);
                Assert.Equal(id, model[0].DishId);
            }
            // удалить базу данных из памяти
            using (var context = new ApplicationDbContext(_options))
            {
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void ControllerSelectsGroup()
        {
            //// arrange
            //var controller = new ProductController();
            //var data = TestData.GetDishesList();
            //controller._dishes = data;
            //var comparer = Comparer<Dish>
            //.GetComparer((d1, d2) => d1.DishId.Equals(d2.DishId));
            //// act
            //var result = controller.Index(2) as ViewResult;
            //var model = result.Model as List<Dish>;
            //// assert
            //Assert.Equal(2, model.Count);
            //Assert.Equal(data[2], model[0], comparer);
            // arrange


            // Контекст контроллера
            var controllerContext = new ControllerContext();
            // Макет HttpContext
            var moqHttpContext = new Mock<HttpContext>();
            moqHttpContext.Setup(c => c.Request.Headers)
            .Returns(new HeaderDictionary());
            controllerContext.HttpContext = moqHttpContext.Object;
            //заполнить DB данными
            using (var context = new ApplicationDbContext(_options))
            {
                TestData.FillContext(context);
            }
            using (var context = new ApplicationDbContext(_options))
            {
                var controller = new ProductController(context)
                { ControllerContext = controllerContext };
                var comparer = Comparer<Dish>.GetComparer((d1, d2) => d1.DishId.Equals(d2.DishId));
                // act
                var result = controller.Index(2) as ViewResult;
                var model = result.Model as List<Dish>;
                // assert
                Assert.Equal(2, model.Count);
                Assert.Equal(context.Dishes
                .ToArrayAsync()
                .GetAwaiter()
                .GetResult()[2], model[0], comparer);
            }
        }



    }
}