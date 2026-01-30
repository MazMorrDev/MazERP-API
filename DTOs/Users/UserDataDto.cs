namespace MazErpBack.DTOs.Users;

public record class UserDataDto
{
    public required int Id { get; init; }
    public required string UserName {get; init;}
}
