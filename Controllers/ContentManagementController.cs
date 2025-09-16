using SpotifyEFSRT.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpotifyEFSRT.Controllers
{
    public class ContentManagementController : Controller
    {
        private static readonly string cadena = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;

        // Método para obtener la lista de contenido
        #region ContentManager
        private IEnumerable<ContentManagement> GetContentManagement()
        {
            var listado = new List<ContentManagement>();
            var sp = "usp_get_content_management"; // Nombre del procedimiento almacenado

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
                            var item = new ContentManagement()
                            {
                                ContentId = Convert.ToInt32(dr["ContentId"]),
                                ContentType = Convert.ToString(dr["ContentType"]),
                                ContentIdRef = Convert.ToInt32(dr["ContentIdRef"]),
                                Action = Convert.ToString(dr["Action"]),
                                ActionDate = Convert.ToDateTime(dr["ActionDate"]),
                                UserId = Convert.ToInt32(dr["UserId"]),
                            };
                            listado.Add(item);
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

        // Acción para la vista principal de contenido
        public ActionResult Index()
        {
            return View(GetContentManagement());
        }
        #endregion

        #region Registro

        private bool ExisteUsuario(int userId)
        {
            var sp = "usp_select_usuario"; // Nombre del stored procedure
            try
            {
                using (SqlConnection cone = new SqlConnection(cadena))
                {
                    cone.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, cone))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", userId);

                        // Ejecutar el comando y devolver si existe el usuario
                        var resultado = cmd.ExecuteScalar();
                        return (int)resultado > 0; // Retorna true si el usuario existe
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string Agregar(ContentManagement registro)
        {
            var mensaje = "";
            var sp = "usp_insert_content1"; 
            try
            {
                // Verificar si el usuario existe
                if (!ExisteUsuario(registro.UserId))
                {
                    throw new Exception("El UserId proporcionado no existe.");
                }

                using (SqlConnection cone = new SqlConnection(cadena))
                {
                    cone.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, cone))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ContentType", registro.ContentType);
                        cmd.Parameters.AddWithValue("@ContentIdRef", registro.ContentIdRef);
                        cmd.Parameters.AddWithValue("@UserId", registro.UserId);
                        cmd.Parameters.AddWithValue("@Action", registro.Action);
                        cmd.Parameters.AddWithValue("@ActionDate", registro.ActionDate);

                        // Devuelve un conteo de cuántas filas han sido afectadas
                        var respuesta = cmd.ExecuteNonQuery();
                        mensaje = $"Se ha registrado {respuesta} contenido.";
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
            // Aquí puedes cargar otros datos necesarios para la vista si los hay
            return View(new ContentManagement());
        }

        [HttpPost]
        public ActionResult Create(ContentManagement registro)
        {
            // Verificar si el usuario existe antes de intentar agregar el registro
            if (!ExisteUsuario(registro.UserId))
            {
                ModelState.AddModelError("UserId", "El UserId proporcionado no existe.");
                return View(registro);
            }
            registro.ActionDate = DateTime.Now;
            System.Diagnostics.Debug.WriteLine("Fecha actual asignada: " + registro.ActionDate);
            ViewBag.mensaje = Agregar(registro);
            return View(registro);
        }

        #endregion

        #region Update
        private string UpdateContent(ContentManagement registro)
        {
            var mensaje = "";
            var sp = "usp_update_content";

            try
            {
                using (SqlConnection cone = new SqlConnection(cadena))
                {
                    cone.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, cone))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ContentId", registro.ContentId);
                        cmd.Parameters.AddWithValue("@ContentType", registro.ContentType);
                        cmd.Parameters.AddWithValue("@ContentIdRef", registro.ContentIdRef);
                        cmd.Parameters.AddWithValue("@UserId", registro.UserId);
                        cmd.Parameters.AddWithValue("@Action", registro.Action);
                        cmd.Parameters.AddWithValue("@ActionDate", registro.ActionDate);

                        // Ejecuta el procedimiento almacenado
                        var respuesta = cmd.ExecuteNonQuery();
                        mensaje = $"Se actualizó {respuesta} contenido(s) correctamente.";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return mensaje;
        }

        public ActionResult Edit(int id = 0)
        {
            if (id == 0)
                return RedirectToAction("Index");

            // Recuperar el contenido existente
            var content = GetContentManagement().FirstOrDefault(c => c.ContentId == id);
            if (content == null)
                return HttpNotFound();

            return View(content);
        }

        [HttpPost]
        public ActionResult Edit(ContentManagement registro)
        {
            // Verificar si el modelo es válido
            if (!ModelState.IsValid)
            {
                return View(registro); // Si no es válido, devuelve la vista con el registro actual
            }
            // Verificar si el usuario existe antes de intentar actualizar el registro
            if (!ExisteUsuario(registro.UserId))
            {
                ModelState.AddModelError("UserId", "El UserId proporcionado no existe.");
                return View(registro); // Devuelve la vista con el mensaje de error
            }

            registro.ActionDate = DateTime.Now;

            // Intentar actualizar el contenido
            string mensaje = UpdateContent(registro);
            ViewBag.mensaje = mensaje;

            // Redirigir a Index después de guardar
            return RedirectToAction("Index");
        }
        #endregion

        #region Details
        private ContentManagement FindContentById(int id)
        {
            var sp = "usp_get_content_by_id"; // Nombre del procedimiento almacenado para obtener el contenido por ID
            ContentManagement contenido = null; // Inicializamos la variable

            try
            {
                using (SqlConnection cone = new SqlConnection(cadena))
                {
                    cone.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, cone))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ContentId", id); // Pasamos el ID como parámetro

                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            // Si encontramos un registro, creamos una instancia de ContentManagement
                            contenido = new ContentManagement()
                            {
                                ContentId = Convert.ToInt32(dr["ContentId"]),
                                ContentType = Convert.ToString(dr["ContentType"]),
                                ContentIdRef = Convert.ToInt32(dr["ContentIdRef"]),
                                Action = Convert.ToString(dr["Action"]),
                                ActionDate = Convert.ToDateTime(dr["ActionDate"]),
                                UserId = Convert.ToInt32(dr["UserId"]),
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return contenido; // Retornamos el contenido encontrado 
        }
        public ActionResult Details(int id)
        {
            var content = FindContentById(id); 
            if (content == null)
            {
                return HttpNotFound(); // Devuelve un error 404 si no se encuentra el contenido
            }
            return View(content);
        }
        #endregion

        #region Delete
        private string DeleteContent(int id)
        {
            string mensaje = "";
            var sp = "usp_delete_content";

            try
            {
                using (SqlConnection cone = new SqlConnection(cadena))
                {
                    cone.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, cone))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ContentId", id);

                        // Ejecuta el procedimiento almacenado
                        var respuesta = cmd.ExecuteNonQuery();
                        mensaje = $"Se ha eliminado {respuesta} contenido(s) correctamente.";
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
            ViewBag.mensaje = DeleteContent(id);
            return RedirectToAction("Index");
        }
        #endregion

    }
}