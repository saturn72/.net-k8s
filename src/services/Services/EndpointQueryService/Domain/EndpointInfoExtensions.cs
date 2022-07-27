namespace EndpointQueryService.Domain
{
    public static class EndpointInfoExtensions
    {
        public static bool IsValid(this EndpointInfo info)
        {
            return info != default &&
                !info.Path.StartsWith(EndpointInfo.PathDelimiter) &&
                !info.Path.EndsWith(EndpointInfo.PathDelimiter);
        }
    }
}
