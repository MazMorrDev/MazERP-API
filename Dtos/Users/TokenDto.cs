namespace MazErpBack.Dtos.Users;

public class TokenDto
{
   public required string Token { get; set; }
   public required DateTime Expiration { get; set; }

   //TODO: revisar agregar permisos o se hace el fetch desde otro endpoint distinto a login
}