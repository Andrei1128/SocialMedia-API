﻿using System.ComponentModel.DataAnnotations;

namespace FirstProject_API.Models.DTOs
{
    public class RegisterRequestDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
