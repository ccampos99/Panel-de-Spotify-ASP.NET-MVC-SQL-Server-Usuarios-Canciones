using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SpotifyEFSRT.Models
{
    public class Permission
    {
        [Display(Name = "ID de Permiso")]
        public int PermissionId { get; set; }

        [Display(Name = "Nombre del Permiso")]
        public string PermissionName { get; set; }

        // Relación con Permisos de Rol
        [Display(Name = "Permisos del Rol")]
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}