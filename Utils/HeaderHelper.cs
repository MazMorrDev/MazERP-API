namespace MazErpBack.Utils;

public class HeaderHelper(IHttpContextAccessor httpContextAccessor)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    public int GetCompanyIdFromHeader()
    {
        var companyIdHeader = _httpContextAccessor.HttpContext?.Request.Headers["companyId"].FirstOrDefault();

        if (string.IsNullOrEmpty(companyIdHeader))
        {
            throw new UnauthorizedAccessException("CompanyId is required in headers");
        }

        if (!int.TryParse(companyIdHeader, out int companyId))
        {
            throw new UnauthorizedAccessException("Invalid CompanyId format");
        }

        return companyId;
    }
}