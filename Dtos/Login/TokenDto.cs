namespace MazErpBack.Dtos.Login;

public class TokenDto
{
   public required string Token { get; set; }
   public required DateTime Expiration { get; set; }

   //todo: revisar agregar permisos o se hace el fetch desde otro endpoint distinto a login
}