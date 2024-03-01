using EpiScarpe_Co.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                string query = "SELECT Id, IndirizzoEmail, PasswordCliente FROM Customers WHERE IndirizzoEmail = @Email AND PasswordCliente = @PasswordCliente";

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", model.IndirizzoEmail);
                        command.Parameters.AddWithValue("@PasswordCliente", model.PasswordCliente);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, reader["IndirizzoEmail"].ToString())                                
                            };

                                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                                var authProperties = new AuthenticationProperties
                                {
                                    
                                };

                                await HttpContext.SignInAsync(
                                    CookieAuthenticationDefaults.AuthenticationScheme,
                                    new ClaimsPrincipal(claimsIdentity),
                                    authProperties);

                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Credenziali non valide.");
                            }
                        }
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
public async Task<IActionResult> Register(CustomerViewModel model)
{
    if (ModelState.IsValid)
    {
        string insertQuery = @"INSERT INTO Customers (Nome, Cognome, Indirizzo, Città, Nazione, DataNascita, NumeroCellulare, IndirizzoEmail, PasswordCliente)
                   VALUES (@Nome, @Cognome, @Indirizzo, @Città, @Nazione, @DataNascita, @NumeroCellulare, @IndirizzoEmail, @PasswordCliente)";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@Nome", model.Nome);
                command.Parameters.AddWithValue("@Cognome", model.Cognome);
                command.Parameters.AddWithValue("@Indirizzo", model.Indirizzo);
                command.Parameters.AddWithValue("@Città", model.Città);
                command.Parameters.AddWithValue("@Nazione", model.Nazione);
                command.Parameters.AddWithValue("@DataNascita", model.DataNascita);
                command.Parameters.AddWithValue("@NumeroCellulare", model.NumeroCellulare);
                command.Parameters.AddWithValue("@IndirizzoEmail", model.IndirizzoEmail);
                command.Parameters.AddWithValue("@PasswordCliente", model.PasswordCliente);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    TempData["RegistrationSuccess"] = "Registrazione avvenuta con successo. Effettua il login.";

                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Errore durante la registrazione.");
                    return View(model);
                }
            }
        }
    }
    return View(model);
}

        [HttpGet]
        public IActionResult LoginAdministrators()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAdministrators(string Matricola, string PasswordAdministrator)
        {
            if (!string.IsNullOrEmpty(Matricola) && !string.IsNullOrEmpty(PasswordAdministrator))
            {
                string query = "SELECT Id, Matricola FROM Administrators WHERE Matricola = @Matricola AND PasswordAdministrator = @PasswordAdministrator";

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Matricola", Matricola);
                        command.Parameters.AddWithValue("@PasswordAdministrator", PasswordAdministrator);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, reader["Matricola"].ToString()),
                            
                        };

                                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                                var authProperties = new AuthenticationProperties
                                {
                                   
                                };

                                await HttpContext.SignInAsync(
                                    CookieAuthenticationDefaults.AuthenticationScheme,
                                    new ClaimsPrincipal(claimsIdentity),
                                    authProperties);

                                return RedirectToAction("Administrator", "Home");
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Credenziali non valide per l'amministratore.");
                            }
                        }
                    }
                }
            }

            return View();
        }

        public IActionResult Administrator()
        {
            var displayedProducts = GetDisplayedProductsFromDatabase();
            return View(displayedProducts);
        }

        [HttpPost]
        public IActionResult HideFromHome(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string updateQuery = "UPDATE Shoes SET IsDisplayedOnHomePage = 0 WHERE Id = @ProductId";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@ProductId", id);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            TempData["HideSuccess"] = "Prodotto nascosto dalla home con successo.";
                        }
                        else
                        {
                            TempData["HideError"] = "Errore durante la procedura di nascondere il prodotto dalla home.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["HideError"] = $"Errore: {ex.Message}";
            }

            return RedirectToAction("Administrator");
        }

        [HttpPost]
        [HttpPost]
        public IActionResult EditProduct(int productId, string newProductName, decimal newProductPrice, string newProductDescription, string newCoverImage, string newAdditionalImage1, string newAdditionalImage2)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string updateQuery = @"UPDATE Shoes 
                                  SET Name = @NewName, 
                                      Price = @NewPrice, 
                                      Description = @NewDescription, 
                                      CoverImage = @NewCoverImage, 
                                      AdditionalImage1 = @NewAdditionalImage1, 
                                      AdditionalImage2 = @NewAdditionalImage2 
                                  WHERE Id = @ProductId";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@ProductId", productId);
                        command.Parameters.AddWithValue("@NewName", newProductName);
                        command.Parameters.AddWithValue("@NewPrice", newProductPrice);
                        command.Parameters.AddWithValue("@NewDescription", newProductDescription);
                        command.Parameters.AddWithValue("@NewCoverImage", newCoverImage);
                        command.Parameters.AddWithValue("@NewAdditionalImage1", newAdditionalImage1);
                        command.Parameters.AddWithValue("@NewAdditionalImage2", newAdditionalImage2);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            TempData["EditSuccess"] = "Prodotto modificato con successo.";
                        }
                        else
                        {
                            TempData["EditError"] = "Errore durante la procedura di modifica del prodotto.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["EditError"] = $"Errore: {ex.Message}";
            }

            return RedirectToAction("Administrator");
        }




        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Home");
        }

    }
}
