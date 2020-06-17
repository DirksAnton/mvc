using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CL_LB1.Entities;
using CL_LB1.Data;
using WB_MVC.Models;
using Microsoft.AspNetCore.Http;
using Moq;
using WB_MVC.Extensions;
using Microsoft.Extensions.Logging;

namespace WB_MVC.Controllers
{
    public class ProductController : Controller
    {

        ApplicationDbContext _context;

        ////public List<Dish> _dishes;
        ////List<DishGroup> _dishGroups;
        int _pageSize;

        //ILogger _logger;
        
        public ProductController(ApplicationDbContext context/*, ILogger<ProductController> logger*/)
        {
            _pageSize = 3;
            _context = context;
            //_logger = logger;
            //SetupData();
        }



        //public IActionResult Index()
        //{
        //    return View(_dishes);
        //}

        //public IActionResult Index(int pageNo = 1)
        //{
        //    var items = _dishes
        //    .Skip((pageNo - 1) * _pageSize)
        //    .Take(_pageSize)
        //    .ToList();
        //    return View(items);
        //}



        [Route("Catalog")]
        [Route("Catalog/Page_{pageNo}")]
        public IActionResult Index(int? group, int pageNo = 1)
        {

            // Контекст контроллера
            var controllerContext = new ControllerContext();
            // Макет HttpContext
            var moqHttpContext = new Mock<HttpContext>();
            moqHttpContext.Setup(c => c.Request.Headers)
            .Returns(new HeaderDictionary());
            controllerContext.HttpContext = moqHttpContext.Object;
            var controller = new ProductController(_context)
            { ControllerContext = controllerContext };

            // Поместить список групп во ViewData
            ViewData["Groups"] = _context.DishGroups;
            // Получить id текущей группы и поместить в TempData
            ViewData["CurrentGroup"] = group ?? 0;
            var dishesFiltered = _context.Dishes
                                .Where(d => !group.HasValue || d.DishGroupId == group.Value);
            //_logger.LogInformation($"info: group={group}, page={pageNo}");
            var model = ListViewModel<Dish>.GetModel(dishesFiltered, pageNo, _pageSize);
            if (Request.IsAjaxRequest())
                return PartialView("_listpartial", model);
            else
                return View(model);

        }

        /// <summary>
        /// Инициализация списков
        /// </summary>
        //private void SetupData()
        //{
        //    _dishGroups = new List<DishGroup>
        //        {
        //        new DishGroup {DishGroupId=1, GroupName="Стартеры"},
        //        new DishGroup {DishGroupId=2, GroupName="Салаты"},
        //        new DishGroup {DishGroupId=3, GroupName="Супы"},
        //        new DishGroup {DishGroupId=4, GroupName="Основные блюда"},
        //        new DishGroup {DishGroupId=5, GroupName="Напитки"},
        //        new DishGroup {DishGroupId=6, GroupName="Десерты"}
        //        };
        //    _dishes = new List<Dish>
        //        {
        //        new Dish {DishId = 1, DishName="Суп-харчо",
        //        Description="Очень острый, невкусный",
        //        Calories =200, DishGroupId=3, Image="Суп.jpg" },
        //        new Dish { DishId = 2, DishName="Борщ",
        //        Description="Много сала, без сметаны",
        //        Calories =330, DishGroupId=3, Image="Борщ.jpg" },
        //        new Dish { DishId = 3, DishName="Котлета пожарская",
        //        Description="Хлеб - 80%, Морковь - 20%",
        //        Calories =635, DishGroupId=4, Image="Котлета.jpg" },
        //        new Dish { DishId = 4, DishName="Макароны по-флотски",
        //        Description="С охотничьей колбаской",
        //        Calories =524, DishGroupId=4, Image="Макароны.jpg" },
        //        new Dish { DishId = 5, DishName="Компот",
        //        Description="Быстро растворимый, 2 литра",
        //        Calories =180, DishGroupId=5, Image="Компот.jpg" }
        //        };
        //}
    }
}