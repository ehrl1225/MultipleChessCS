
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MultipleChessCs.Domain.Player;

[Index(nameof(Username), IsUnique = true)]
public class Player
{
    [Key]
    public int Id {get; set;}

    [Required]
    public string Username {get; set;} = string.Empty;

    [Required]
    public string PasswordHash {get; set;} = string.Empty;


}