namespace vta.loyalty.service
{
    public interface IUserActionManager
    {
        core.Account.User User { get; }
        bool IsExitsUser { get; }
        bool CanDo(core.Account.Action action);
        bool CanDo(string actionName, string controller, string area, string domainName = "");
    }
}