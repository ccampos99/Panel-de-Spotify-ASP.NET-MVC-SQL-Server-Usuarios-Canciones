using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SpotifyEFSRT.Models
{
    public class User
    {
        [Display(Name = "ID de Usuario")]
        public int UserId { get; set; }

        [Display(Name = "Nombre de Usuario")]
        public string Username { get; set; }

        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; }

        [Display(Name = "Hash de Contraseña")]
        public string PasswordHash { get; set; }

        [Display(Name = "Fecha de Creación")]
        public DateTime DateCreated { get; set; }

        // Relación con UserRoles
        [Display(Name = "ID de Rol")]
        public int RoleId { get; set; }

        [Display(Name = "Rol")]
        public UserRole Role { get; set; }

        // Relación con ActivityLogs
        [Display(Name = "Registros de Actividad")]
        public ICollection<ActivityLog> ActivityLogs { get; set; }
    }
}