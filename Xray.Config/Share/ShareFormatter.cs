using Xray.Config.Models;

namespace Xray.Config.Share;

public abstract class ShareFormatter
{
    public readonly string Name;

    public ShareFormatter(string name)
    {
        Name = name;
    }

    protected string GetRemark(Inbound inbound)
    {
        if (!string.IsNullOrEmpty(inbound.Tag))
        {
            return inbound.Tag;
        }

        var protocolStr = inbound.Protocol.ToString();
        if (!string.IsNullOrEmpty(protocolStr))
        {
            return protocolStr;
        }

        return "";
    }

    public string FromInbound(Inbound inbound, string email)
    {
        if (inbound is VlessInbound)
        {
            return FromInbound((VlessInbound)inbound, email);
        }

        if (inbound is VMessInbound)
        {
            return FromInbound((VMessInbound)inbound, email);
        }

        if (inbound is ShadowSocksInbound)
        {
            return FromInbound((ShadowSocksInbound)inbound, email);
        }

        if (inbound is SocksInbound)
        {
            return FromInbound((SocksInbound)inbound, email);
        }

        if (inbound is TrojanInbound)
        {
            return FromInbound((TrojanInbound)inbound, email);
        }

        throw new ArgumentException($"Unsupport link: {inbound.Protocol}");
    }

    public abstract string FromInbound(VlessInbound inbound, string email);
    public abstract string FromInbound(VlessInbound inbound, VlessClient client);

    public abstract string FromInbound(VMessInbound inbound, string email);
    public abstract string FromInbound(VMessInbound inbound, VMessClient client);

    public abstract string FromInbound(ShadowSocksInbound inbound, ShadowSocksClient client);
    public abstract string FromInbound(ShadowSocksInbound inbound, string email);
    
    public abstract string FromInbound(TrojanInbound inbound, TrojanClient client);
    public abstract string FromInbound(TrojanInbound inbound, string email);

    public abstract string FromInbound(SocksInbound inbound, SocksAccount account);
    public abstract string FromInbound(SocksInbound inbound, string username);

    public abstract Outbound ToOutbound(string config);
    public abstract VlessOutbound ToVlessOutbound(string config);
    public abstract VMessOutbound ToVMessOutbound(string config);
    public abstract ShadowSocksOutbound ToShadowSocksOutbound(string config);
    public abstract SocksOutbound ToSocksOutbound(string config);
    public abstract TrojanOutbound ToTrojanOutbound(string config);
    public abstract HysteriaOutbound ToHysteriaOutbound(string config);
}