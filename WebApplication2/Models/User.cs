using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models;

public partial class User
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "La contraseña es requerida")]
  
    [StringLength(255, ErrorMessage = "La contraseña debe tener entre {2} y {1} caracteres", MinimumLength = 6)]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Debe seleccionar un rol")]
    [Display(Name = "Rol")]
    public int RoleId { get; set; }

    public virtual Role? Role { get; set; }
}
