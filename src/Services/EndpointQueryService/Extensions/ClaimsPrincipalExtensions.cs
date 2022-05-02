namespace System.Security.Claims
{
    public static class ClaimsPrincipalExtensions
    {
        public static string Subject(this ClaimsPrincipal principal) => principal.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
