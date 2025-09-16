using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SpotifyEFSRT.Models
{
    public class UserRole
    {
        [Display(Name = "ID de Rol")]
        public int RoleId { get; set; }

        [Display(Name = "Nombre del Rol")]
        public string RoleName { get; set; }

        // Relación con Usuarios
        [Display(Name = "Usuarios")]
        public ICollection<User> Users { get; set; }

        // Relación con Permisos de Rol
        [Display(Name = "Permisos del Rol")]
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}