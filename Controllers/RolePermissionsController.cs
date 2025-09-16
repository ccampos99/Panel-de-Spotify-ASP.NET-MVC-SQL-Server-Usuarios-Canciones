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
    public class RolePermissionsController : Controller
    {
        private static readonly string cadena = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;

        #region RolePermissionsList
        // Método para obtener la lista de permisos asignados a roles
        private IEnumerable<RolePermission> GetRolePermissions()
        {
            var listado = new List<RolePermission>();
            var sp = "usp_get_role_permissions"; // Nombre del procedimiento almacenado

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
                            var rolePermission = new RolePermission()
                            {
                                RolePermissionId = Convert.ToInt32(dr["RolePermissionId"]),
                                RoleId = Convert.ToInt32(dr["RoleId"]),
                                PermissionId = Convert.ToInt32(dr["PermissionId"]),
                                Role = new UserRole
                                {
                                    RoleId = Convert.ToInt32(dr["RoleId"]),
                                    RoleName = Convert.ToString(dr["RoleName"])
                                },
                                Permission = new Permission
                                {
                                    PermissionId = Convert.ToInt32(dr["PermissionId"]),
                                    PermissionName = Convert.ToString(dr["PermissionName"])
                                }
                            };
                            listado.Add(rolePermission);
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
            return View(GetRolePermissions());
        }
        #endregion

        #region AssignPermission
        // Método para asignar un permiso a un rol
        private string AssignPermission(RolePermission rolePermission)
        {
            var mensaje = "";
            var sp = "usp_assign_permission_to_role";

            try
            {
                using (SqlConnection cone = new SqlConnection(cadena))
                {
                    cone.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, cone))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RoleId", rolePermission.RoleId);
                        cmd.Parameters.AddWithValue("@PermissionId", rolePermission.PermissionId);

                        var respuesta = cmd.ExecuteNonQuery();
                        mensaje = "Se ha asignado el permiso correctamente.";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return mensaje;
        }

        public ActionResult Assign()
        {
            return View(new RolePermission());
        }

        [HttpPost]
        public ActionResult Assign(RolePermission rolePermission)
        {
            ViewBag.mensaje = AssignPermission(rolePermission);
            return RedirectToAction("Index");
        }
        #endregion

        #region RemovePermission
        // Método para quitar un permiso de un rol
        private string RemovePermission(RolePermission rolePermission)
        {
            var mensaje = "";
            var sp = "usp_remove_permission_from_role";

            try
            {
                using (SqlConnection cone = new SqlConnection(cadena))
                {
                    cone.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, cone))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RoleId", rolePermission.RoleId);
                        cmd.Parameters.AddWithValue("@PermissionId", rolePermission.PermissionId);

                        var respuesta = cmd.ExecuteNonQuery();
                        mensaje = "Se ha eliminado el permiso correctamente.";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return mensaje;
        }

        public ActionResult Remove(int roleId, int permissionId)
        {
            var rolePermission = new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId
            };
            ViewBag.mensaje = RemovePermission(rolePermission);
            return RedirectToAction("Index");
        }
        #endregion
    }
}