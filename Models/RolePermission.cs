using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SpotifyEFSRT.Models
{
    public class RolePermission
    {
        [Display(Name = "ID de Permiso de Rol")]
        public int RolePermissionId { get; set; }

        // Relación con Roles
        [Display(Name = "ID de Rol")]
        public int RoleId { get; set; }
        
        [Display(Name = "Rol")]
        public UserRole Role { get; set; }

        // Relación con Permisos
        [Display(Name = "ID de Permiso")]
        public int PermissionId { get; set; }
        [Display(Name = "Permiso")]
        public Permission Permission { get; set; }
    }
}