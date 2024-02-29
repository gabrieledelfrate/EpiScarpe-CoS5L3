using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using EpiScarpe_Co.Models;

namespace EpiScarpe_Co.Controllers
{
    public class AccountController : Controller
    {
        private readonly string _connectionString = "Server=GABRIELE-PORTAT\\SQLEXPRESS; Initial Catalog=EpiScarpe; Integrated Security=true; TrustServerCertificate=True";

       
        [HttpPost]
        public IActionResult Register(CustomerViewModel model)
        {           
            string insertQuery = @"INSERT INTO Customers (Nome, Cognome, Indirizzo, Città, Nazione, DataNascita, NumeroCellulare, IndirizzoEmail, PasswordCriptata)
                               VALUES (@Nome, @Cognome, @Indirizzo, @Città, @Nazione, @DataNascita, @NumeroCellulare, @IndirizzoEmail, @PasswordCriptata)";

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
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Errore durante la registrazione.");
                        return View(model);
                    }
                }
            }
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                string query = "SELECT Id, IndirizzoEmail, PasswordCliente FROM Customers WHERE IndirizzoEmail = @Email AND PasswordCliente = @Password";

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
    }
}
