﻿using System;
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

        public VaultAccessViewModel[] AccessLog { get; set; }
    }

    public class VaultAdminUsersAccessViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        public UserSelectViewModel[] Users { get; set; }
    }

    public class VaultAdminOpenHoursViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Начало работы")]
        public int OpeningTime { get; set; }

        [Display(Name = "Завершение работы")]
        public int ClosingTime { get; set; }
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

    public class VaultAccessViewModel
    {
        public virtual int Id { get; set; }
        public virtual string VaultName { get; set; }
        public virtual DateTime AccessTime { get; set; }
        public virtual bool IsSuccess { get; set; }
        public virtual string UserInfo { get; set; }
    }
}