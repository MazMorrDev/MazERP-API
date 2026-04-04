namespace MazErpAPI.DTOs.Users;

public record class TokenDto
{
   public required string Token { get; init; }
   public required DateTime Expiration { get; init; }

   // By UserDto, the client can storage the information about who is logged 
   public required UserDto UserDto { get; init; }
}