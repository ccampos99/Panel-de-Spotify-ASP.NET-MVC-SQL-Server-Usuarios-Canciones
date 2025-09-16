using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpotifyEFSRT.Models;

namespace SpotifyEFSRT.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        private static readonly string cadena = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;

        #region UsersList
        // Método para obtener la lista de usuarios
        private IEnumerable<User> GetUsers()
        {
            var listado = new List<User>();
            var sp = "usp_get_users"; // Nombre del procedimiento almacenado

            try
            {
                using (SqlConnection cone = new SqlConnection(cadena))
                {
                    cone.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, cone))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            var user = new User()
                            {
                                UserId = Convert.ToInt32(dr["UserId"]),
                                Username = Convert.ToString(dr["Username"]),
                                Email = Convert.ToString(dr["Email"]),
                                RoleId = Convert.ToInt32(dr["RoleId"]),
                                DateCreated = Convert.ToDateTime(dr["DateCreated"]),
                            };
                            listado.Add(user);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return listado;
        }

        public ActionResult Index()
        {
            return View(GetUsers());
        }
        #endregion

        #region CreateUser
        // Método para agregar un nuevo usuario
        private string AddUser(User user)
        {
            var mensaje = "";
            var sp = "usp_insert_user";

            try
            {
                using (SqlConnection cone = new SqlConnection(cadena))
                {
                    cone.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, cone))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", user.Username);
                        cmd.Parameters.AddWithValue("@Email", user.Email);
                        cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                        cmd.Parameters.AddWithValue("@RoleId", user.RoleId);

                        var respuesta = cmd.ExecuteNonQuery();
                        mensaje = $"Se ha registrado {respuesta} usuario.";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return mensaje;
        }

        public ActionResult Create()
        {
            return View(new User());
        }

        [HttpPost]
        public ActionResult Create(User user)
        {
            ViewBag.mensaje = AddUser(user);
            return View(user);
        }
        #endregion

        #region UpdateUser
        private string UpdateUser(User user)
        {
            var mensaje = "";
            var sp = "usp_update_user";

            try
            {
                using (SqlConnection cone = new SqlConnection(cadena))
                {
                    cone.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, cone))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", user.UserId);
                        cmd.Parameters.AddWithValue("@Username", user.Username);
                        cmd.Parameters.AddWithValue("@Email", user.Email);
                        cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                        cmd.Parameters.AddWithValue("@RoleId", user.RoleId);

                        var respuesta = cmd.ExecuteNonQuery();
                        mensaje = $"Se ha actualizado {respuesta} usuario(s) correctamente.";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return mensaje;
        }

        public ActionResult Edit(int id)
        {
            var user = GetUsers().FirstOrDefault(u => u.UserId == id);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            ViewBag.mensaje = UpdateUser(user);
            return RedirectToAction("Index");
        }
        #endregion

        #region UserDetails
        private User FindUserById(int id)
        {
            var sp = "usp_get_user_by_id";
            User user = null;

            try
            {
                using (SqlConnection cone = new SqlConnection(cadena))
                {
                    cone.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, cone))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", id);

                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            user = new User()
                            {
                                UserId = Convert.ToInt32(dr["UserId"]),
                                Username = Convert.ToString(dr["Username"]),
                                Email = Convert.ToString(dr["Email"]),
                                RoleId = Convert.ToInt32(dr["RoleId"]),
                                DateCreated = Convert.ToDateTime(dr["DateCreated"]),
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return user;
        }

        public ActionResult Details(int id)
        {
            var user = FindUserById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        #endregion

        #region DeleteUser
        private string DeleteUser(int id)
        {
            string mensaje = "";
            var sp = "usp_delete_user";

            try
            {
                using (SqlConnection cone = new SqlConnection(cadena))
                {
                    cone.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, cone))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", id);

                        var respuesta = cmd.ExecuteNonQuery();
                        mensaje = $"Se ha eliminado {respuesta} usuario(s) correctamente.";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return mensaje;
        }

        public ActionResult Delete(int id)
        {
            ViewBag.mensaje = DeleteUser(id);
            return RedirectToAction("Index");
        }
        #endregion
    }
}