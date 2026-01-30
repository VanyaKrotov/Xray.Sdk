using Xray.Config.Models;

namespace Xray.Config.Share;

public abstract class ShareFormatter
{
    public readonly string Name;

    protected const string VLESS_PROTOCOL = "vless";
    protected const string VMESS_PROTOCOL = "vmess";
    protected const string TROJAN_PROTOCOL = "trojan";
    protected const string SHADOW_SOCKS_PROTOCOL = "ss";
    protected const string HYSTERIA2_PROTOCOL = "hysteria2";
    protected const string HYSTERIA_PROTOCOL = "hy";
    protected const string SOCKS_PROTOCOL = "socks";

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

    public Outbound ToOutbound(string config)
    {
        var separateIndex = config.IndexOf("://");
        if (separateIndex == -1)
        {
            throw new ArgumentException("Invalid share string format. Link must have `{protocol}://` prefix.");
        }

        var protocol = config.Substring(0, separateIndex);

        switch (protocol)
        {
            case VLESS_PROTOCOL:
                return ParseVless(config);

            case VMESS_PROTOCOL:
                return ParseVMess(config);

            case TROJAN_PROTOCOL:
                return ParseTrojan(config);

            case SOCKS_PROTOCOL:
                return ParseSocks(config);

            case SHADOW_SOCKS_PROTOCOL:
                return ParseShadowSocks(config);

            case HYSTERIA2_PROTOCOL:
            case HYSTERIA_PROTOCOL:
                return ParseHysteria(config);
        }

        throw new Exception($"Failure parsing. Unsupported protocol {protocol}.");
    }


    public abstract string FromInbound(VlessInbound inbound, string email);
    public abstract string FromInbound(VlessInbound inbound, VlessClient client);
    public abstract VlessOutbound ParseVless(string config);

    public abstract string FromInbound(VMessInbound inbound, string email);
    public abstract string FromInbound(VMessInbound inbound, VMessClient client);
    public abstract VMessOutbound ParseVMess(string config);

    public abstract string FromInbound(ShadowSocksInbound inbound, ShadowSocksClient client);
    public abstract string FromInbound(ShadowSocksInbound inbound, string email);
    public abstract ShadowSocksOutbound ParseShadowSocks(string config);

    public abstract string FromInbound(TrojanInbound inbound, TrojanClient client);
    public abstract string FromInbound(TrojanInbound inbound, string email);
    public abstract TrojanOutbound ParseTrojan(string config);

    public abstract string FromInbound(SocksInbound inbound, SocksAccount account);
    public abstract string FromInbound(SocksInbound inbound, string username);
    public abstract SocksOutbound ParseSocks(string config);

    public abstract HysteriaOutbound ParseHysteria(string config);
}