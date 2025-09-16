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
    public class PermissionsController : Controller
    {
        private static readonly string cadena = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;

        #region PermissionsList
        // Método para obtener la lista de permisos
        private IEnumerable<Permission> GetPermissions()
        {
            var listado = new List<Permission>();
            var sp = "usp_get_permissions"; // Nombre del procedimiento almacenado

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
                            var permission = new Permission()
                            {
                                PermissionId = Convert.ToInt32(dr["PermissionId"]),
                                PermissionName = Convert.ToString(dr["PermissionName"])
                            };
                            listado.Add(permission);
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
            var permissions = GetPermissions();
            return View(permissions.ToList());
        }
        #endregion

        #region CreatePermission
        // Método para agregar un nuevo permiso
        private string AddPermission(Permission permission)
        {
            var mensaje = "";
            var sp = "usp_insert_permission";

            try
            {
                using (SqlConnection cone = new SqlConnection(cadena))
                {
                    cone.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, cone))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PermissionName", permission.PermissionName);

                        var respuesta = cmd.ExecuteNonQuery();
                        mensaje = $"Se ha registrado {respuesta} permiso.";
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
            return View(new Permission());
        }

        [HttpPost]
        public ActionResult Create(Permission permission)
        {
            ViewBag.mensaje = AddPermission(permission);
            return View(permission);
        }
        #endregion

        #region UpdatePermission
        // Método para actualizar un permiso existente
        private string UpdatePermission(Permission permission)
        {
            var mensaje = "";
            var sp = "usp_update_permission";

            try
            {
                using (SqlConnection cone = new SqlConnection(cadena))
                {
                    cone.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, cone))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PermissionId", permission.PermissionId);
                        cmd.Parameters.AddWithValue("@PermissionName", permission.PermissionName);

                        var respuesta = cmd.ExecuteNonQuery();
                        mensaje = $"Se ha actualizado {respuesta} permiso(s) correctamente.";
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
            var permission = GetPermissions().FirstOrDefault(p => p.PermissionId == id);
            if (permission == null)
                return HttpNotFound();

            return View(permission);
        }

        [HttpPost]
        public ActionResult Edit(Permission permission)
        {
            ViewBag.mensaje = UpdatePermission(permission);
            return RedirectToAction("Index");
        }
        #endregion

        #region PermissionDetails
        // Método para obtener los detalles de un permiso por ID
        private Permission FindPermissionById(int id)
        {
            var sp = "usp_get_permission_by_id";
            Permission permission = null;

            try
            {
                using (SqlConnection cone = new SqlConnection(cadena))
                {
                    cone.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, cone))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PermissionId", id);

                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            permission = new Permission()
                            {
                                PermissionId = Convert.ToInt32(dr["PermissionId"]),
                                PermissionName = Convert.ToString(dr["PermissionName"])
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return permission;
        }

        public ActionResult Details(int id)
        {
            var permission = FindPermissionById(id);
            if (permission == null)
            {
                return HttpNotFound();
            }
            return View(permission);
        }
        #endregion

        #region DeletePermission
        // Método para eliminar un permiso por ID
        private string DeletePermission(int id)
        {
            string mensaje = "";
            var sp = "usp_delete_permission";

            try
            {
                using (SqlConnection cone = new SqlConnection(cadena))
                {
                    cone.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, cone))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PermissionId", id);

                        var respuesta = cmd.ExecuteNonQuery();
                        mensaje = $"Se ha eliminado {respuesta} permiso(s) correctamente.";
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
            ViewBag.mensaje = DeletePermission(id);
            return RedirectToAction("Index");
        }
        #endregion
    }
}