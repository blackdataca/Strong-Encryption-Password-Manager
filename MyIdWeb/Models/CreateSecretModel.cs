﻿using System.ComponentModel.DataAnnotations;

namespace MyIdWeb.Models;

public class SecretDetailModel
{
    [Required]
    [MaxLength(50)]
    public string Site { get; set; }

    [MaxLength(50)]
    [Display(Name = "User")]
    public string UserName { get; set; }

    [MaxLength(50)]
    public string Password { get; set; }

    [MaxLength(500)]
    public string Memo { get; set; }

}
