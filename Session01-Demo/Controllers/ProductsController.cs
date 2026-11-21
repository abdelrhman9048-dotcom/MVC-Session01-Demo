using Microsoft.AspNetCore.Mvc;
using Session01_Demo.Models;

namespace Session01_Demo.Controllers
{
    //[Route("[controller]")]
    public class ProductsController : Controller
    {
        #region Actions,ActionResult
        // BaseUrl/Products/Test
        //public string Test() // string --> ContentResult ---> HTML
        //   {
        //       return $"Hello from test";
        //   }

        [HttpGet] // BASEURL/Products/DownloadFile
        public IActionResult DownloadFile()
        {
            var bytes = System.IO.File.ReadAllBytes("wwwroot/files/notes.pdf");
            return new FileContentResult(bytes, "application/pdf");
        }


        [HttpGet]
        //BaseUrl/Products/GetProduts

        public IActionResult GetProducts()
        {
            var products = new List<Product>()
            {
                new Product(){Id=1 ,Name"Mobile"},
                new Product(){Id=2 ,Name"Tablet"}

            };
            return Json(products);
        }


        [NonAction]
        public IActionResult Test()
        {
            return View();
        }

        [HttpGet] // BASEURL/Products/RedirectToUrl
        public IActionResult RedirectToUrl()
        {
            return Redirect("https://www.google.com");
        }

        // BaseUrl/Products/GetProductById/10

        public IActionResult GetProductById(int id)
        {
            if (id < 0)
                return BadRequest();

            else if (id == 0)
                // return RedirectToAction("GetProducts");
                return RedirectToAction(nameof(GetProducts));

            else if (id < 10)
                return NotFound();

            else
                return Json(new Product() { Id = id, Name = "Laptop" });
        }
        #endregion

        #region Routing
        // BaseUrl/Controller/Action/Segment

        // Segment --> Static segment, variable segment, mixed segment

        // BaseUrl/Products/Test/Ten
        // BaseUrl/Products/Test/{Num}
        // BaseUrl/Products/Test/x{Num}


        // Param --> Segment, query param

        // BaseUrl/Products/Test?num=10


        // BaseUrl/Products/TestRouting
        // [HttpGet("Products/Test01")]
        // public IActionResult TestRouting()
        // {
        //     return Json($"Hello from testrouting");
        // }


        // BaseUrl/Products/TestRouting/10
        // [HttpGet("TestingRoute/Test{id:int}")]
        // public IActionResult TestRouting(int id)
        // {
        //     return Json($"Hello with id : {id}");
        // }


        // BaseUrl/Products/TestRouting/10?name=Mariam

        public IActionResult TestRouting(int id, string name)
        {
            return Json($"Hello from TestRouting : id -> {id}, name -> {name}");
        }
        #endregion

        #region Model Binding
        // Action --> Params [Simple data, Complex data (user defined data type), collection]

        [HttpGet]
        // BaseUrl/Products/GetById

        // simple data
        public IActionResult GetById(
            [FromQuery] int id,
            [FromQuery(Name = "id2")] int anotherId)
        {
            return Json(new { id = id, anotherId = anotherId });
        }

        // complex data ---> as query string [name of prop direct (or) name of param.name of prop ]
        public IActionResult Search(ProductFilter filter)
        {
            return Content("Search completed");
        }

        // Collection
        // BaseUrl/Products/GetByIds

        [HttpGet]
        public IActionResult GetByIds(int[] ids)
        {
            return Json(new
            {
                ids
            });
        }
        #endregion

    }
}
