using EpiScarpe_Co.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Data.SqlClient;

namespace EpiScarpe_Co.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _connectionString = "Server=GABRIELE-PORTAT\\SQLEXPRESS; Initial Catalog=EpiScarpe; Integrated Security=true; TrustServerCertificate=True";


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public ActionResult Index()
        {
            var displayedProducts = GetDisplayedProductsFromDatabase();
            return View(displayedProducts);
        }

       public ActionResult Details(int id)
        {
            var product = GetProductDetailsFromDatabase(id);
            return View(product);
        }

        private List<Product> GetDisplayedProductsFromDatabase()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Shoes WHERE IsDisplayedOnHomePage = 1";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product product = new Product
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                Description = reader["Description"].ToString(),
                                CoverImage = reader["CoverImage"].ToString(),
                                AdditionalImage1 = reader["AdditionalImage1"].ToString(),
                                AdditionalImage2 = reader["AdditionalImage2"].ToString(),
                                IsDisplayedOnHomePage = Convert.ToBoolean(reader["IsDisplayedOnHomePage"])
                            };

                            products.Add(product);
                        }
                    }
                }
            }

            return products;
        }

        private Product GetProductDetailsFromDatabase(int id)
        {
            Product product = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Shoes WHERE Id = @ProductId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductId", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            product = new Product
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                Description = reader["Description"].ToString(),
                                CoverImage = reader["CoverImage"].ToString(),
                                AdditionalImage1 = reader["AdditionalImage1"].ToString(),
                                AdditionalImage2 = reader["AdditionalImage2"].ToString(),
                                IsDisplayedOnHomePage = Convert.ToBoolean(reader["IsDisplayedOnHomePage"])
                            };
                        }
                    }
                }
            }

            return product;
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
