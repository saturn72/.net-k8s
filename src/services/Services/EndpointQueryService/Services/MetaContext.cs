namespace EndpointQueryService.Services
{
    public record MetaContext : ActionContext
    {
        public override string ActionName => Consts.Endpoint.Actions.Meta;
    }
}
