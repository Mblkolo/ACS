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

        public int? OpeningTime { get; set; }
        public int? CloseTime { get; set; }

        public string[] Users { get; set; }

        public string[] AccessLog { get; set; }
    }

    public class VaultAdminUsersAccessViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        public UserSelectViewModel[] Users { get; set; }
    }

    public class UserSelectViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        public bool IsSelected {get; set; }
    }
}