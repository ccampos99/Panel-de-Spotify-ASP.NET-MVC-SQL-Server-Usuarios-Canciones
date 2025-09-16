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
    public class RolesController : Controller
    {
        private static readonly string cadena = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;

        #region RolesList
        // Método para obtener la lista de roles
        private IEnumerable<UserRole> GetRoles()
        {
            var listado = new List<UserRole>();
            var sp = "usp_get_roles"; // Nombre del procedimiento almacenado

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
                            var role = new UserRole()
                            {
                                RoleId = Convert.ToInt32(dr["RoleId"]),
                                RoleName = Convert.ToString(dr["RoleName"])
                            };
                            listado.Add(role);
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
            return View(GetRoles());
        }
        #endregion

        #region CreateRole
        // Método para agregar un nuevo rol
        private string AddRole(UserRole role)
        {
            var mensaje = "";
            var sp = "usp_insert_role";

            try
            {
                using (SqlConnection cone = new SqlConnection(cadena))
                {
                    cone.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, cone))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RoleName", role.RoleName);

                        var respuesta = cmd.ExecuteNonQuery();
                        mensaje = $"Se ha registrado {respuesta} rol.";
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
            return View(new UserRole());
        }

        [HttpPost]
        public ActionResult Create(UserRole role)
        {
            ViewBag.mensaje = AddRole(role);
            return View(role);
        }
        #endregion

        #region UpdateRole
        // Método para actualizar un rol existente
        private string UpdateRole(UserRole role)
        {
            var mensaje = "";
            var sp = "usp_update_role";

            try
            {
                using (SqlConnection cone = new SqlConnection(cadena))
                {
                    cone.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, cone))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RoleId", role.RoleId);
                        cmd.Parameters.AddWithValue("@RoleName", role.RoleName);

                        var respuesta = cmd.ExecuteNonQuery();
                        mensaje = $"Se ha actualizado {respuesta} rol(es) correctamente.";
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
            var role = GetRoles().FirstOrDefault(r => r.RoleId == id);
            if (role == null)
                return HttpNotFound();

            return View(role);
        }

        [HttpPost]
        public ActionResult Edit(UserRole role)
        {
            ViewBag.mensaje = UpdateRole(role);
            return RedirectToAction("Index");
        }
        #endregion

        #region RoleDetails
        // Método para obtener los detalles de un rol por ID
        private UserRole FindRoleById(int id)
        {
            var sp = "usp_get_role_by_id";
            UserRole role = null;

            try
            {
                using (SqlConnection cone = new SqlConnection(cadena))
                {
                    cone.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, cone))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RoleId", id);

                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            role = new UserRole()
                            {
                                RoleId = Convert.ToInt32(dr["RoleId"]),
                                RoleName = Convert.ToString(dr["RoleName"])
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return role;
        }

        public ActionResult Details(int id)
        {
            var role = FindRoleById(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }
        #endregion

        #region DeleteRole
        // Método para eliminar un rol por ID
        private string DeleteRole(int id)
        {
            string mensaje = "";
            var sp = "usp_delete_role";

            try
            {
                using (SqlConnection cone = new SqlConnection(cadena))
                {
                    cone.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, cone))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RoleId", id);

                        var respuesta = cmd.ExecuteNonQuery();
                        mensaje = $"Se ha eliminado {respuesta} rol(es) correctamente.";
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
            ViewBag.mensaje = DeleteRole(id);
            return RedirectToAction("Index");
        }
        #endregion
    }
}