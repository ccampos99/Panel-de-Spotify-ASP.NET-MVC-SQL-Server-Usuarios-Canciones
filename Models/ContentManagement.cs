using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SpotifyEFSRT.Models
{
    public class ContentManagement
    {
        [Display(Name = "ID del Contenido")]
        public int ContentId { get; set; }
        [Display(Name = "Tipo de Contenido")]
        public string ContentType { get; set; }
        [Display(Name = "ID de Referencia de Contenido")]
        public int ContentIdRef { get; set; }
        [Display(Name = "Acción")]
        public string Action { get; set; }
        [Display(Name = "Fecha de Acción")]
        public DateTime ActionDate { get; set; }

        // Relación con Users
        [Display(Name = "ID del Usuario")]
        public int UserId { get; set; }
        [Display(Name = "Usuario")]
        public User User { get; set; }
    }
}