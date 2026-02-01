namespace MazErpBack.DTOs.Users;

public record class TokenDto
{
   public required string Token { get; init; }
   public required DateTime Expiration { get; init; }

   // By UserDto, the client(frontend) can storage the information about who is logged 
   public required UserDto UserDto { get; init; }

   //TODO: revisar agregar permisos o se hace el fetch desde otro endpoint distinto a login
}