using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccessControlSystem.Models
{
    public class VaultAdminIndexViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Название")]
        public string Name { get; set; }

        //м.б. поля со всякой статистикой
    }

    public class VaultAdminDetaisViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Название")]
        public string Name { get; set; }

        public string[] Users { get; set; }

        public string[] AccessLog { get; set; }
    }
}