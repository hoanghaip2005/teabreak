using System.ComponentModel.DataAnnotations;
using App.Models;
using Microsoft.AspNetCore.Identity;

public class Roles : IdentityRole
{

    public ICollection<AppUser> Users { get; set; }

}