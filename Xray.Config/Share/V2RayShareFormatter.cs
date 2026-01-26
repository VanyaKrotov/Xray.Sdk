using Xray.Config.Models;

namespace Xray.Config.Share;

public class V2RayShareFormatter : ShareFormatter
{
    public V2RayShareFormatter() : base("v2ray") { }

    public override string FromInbound(VlessInbound inbound)
    {
        var builder = new UriBuilder()
        {
            Scheme = "socks",
            Fragment = GetRemark(inbound),
            Host = $"{inbound.Listen}:{inbound.Port}",
            
        };


        return builder.Uri.ToString();
    }

    public override string FromInbound(VMessInbound inbound)
    {
        throw new NotImplementedException();
    }

    public override string FromInbound(ShadowSocksInbound inbound)
    {
        throw new NotImplementedException();
    }

    public override string FromInbound(SocksInbound inbound)
    {
        throw new NotImplementedException();
    }

    public override string FromInbound(TrojanInbound inbound)
    {
        throw new NotImplementedException();
    }

    public override HysteriaOutbound ToHysteriaOutbound(string config)
    {
        throw new NotImplementedException();
    }


    public override Outbound ToOutbound(string config)
    {
        throw new NotImplementedException();
    }

    public override ShadowSocksOutbound ToShadowSocksOutbound(string config)
    {
        throw new NotImplementedException();
    }

    public override SocksOutbound ToSocksOutbound(string config)
    {
        throw new NotImplementedException();
    }

    public override TrojanOutbound ToTrojanOutbound(string config)
    {
        throw new NotImplementedException();
    }

    public override VlessOutbound ToVlessOutbound(string config)
    {
        throw new NotImplementedException();
    }

    public override VMessOutbound ToVMessOutbound(string config)
    {
        throw new NotImplementedException();
    }
}