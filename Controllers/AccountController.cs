using EpiScarpe_Co.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace EpiScarpe_Co.Controllers
{
    public class AccountController : Controller
    {
        private readonly string _connectionString = "Server=GABRIELE-PORTAT\\SQLEXPRESS; Initial Catalog=EpiScarpe; Integrated Security=true; TrustServerCertificate=True";


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
    }
}
