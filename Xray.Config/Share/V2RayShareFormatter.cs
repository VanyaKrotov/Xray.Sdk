using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using Xray.Config.Enums;
using Xray.Config.Models;
using Xray.Config.Utilities;

namespace Xray.Config.Share;

/// <summary>
/// Formatter for generate and parse shared links in v2Ray string format.
/// <para>Supported protocols: Vless, VMess, Trojan, ShadowSocks, Socks, Hysteria.</para>
/// </summary>
public class V2RayShareFormatter : ShareFormatter
{
    private static string DEFAULT_ADDRESS = "0.0.0.0";

    public V2RayShareFormatter() : base("v2ray") { }

    #region Vless
    /// <summary>
    /// Generate Vless share link from xray inbound and client.
    /// </summary>
    /// <param name="inbound">Vless inbound</param>
    /// <param name="client">Client to share</param>
    /// <returns>Vless transfer link</returns>
    public override string CreateLink(VlessInbound inbound, VlessClient client)
    {
        var stream = inbound.StreamSettings;
        var streamOptions = GetStreamOptions(stream);
        var security = stream.Security;
        var options = new VlessOptions()
        {
            Type = streamOptions.Type,
            Path = streamOptions.Path,
            Host = streamOptions.Host,
            Mode = streamOptions.Mode,
            Seed = streamOptions.Seed,
            Authority = streamOptions.Authority,
            ServiceName = streamOptions.ServiceName,
            HeaderType = streamOptions.HeaderType,
            Security = EnumStringConverter.ToString(security),
            Encryption = EnumStringConverter.ToString(inbound.Settings.Decryption),
        };

        if (security == StreamSecurity.Tls && stream.TlsSettings != null)
        {
            GetTlsOptions(stream.TlsSettings, options);
        }
        else if (security == StreamSecurity.Reality && stream.RealitySettings != null)
        {
            GetRealityOptions(stream.RealitySettings, options);
        }

        if (stream.Network == StreamNetwork.Raw)
        {
            options.Flow = EnumStringConverter.ToString(client.Flow);
        }

        var builder = new UriBuilder()
        {
            Scheme = VLESS_PROTOCOL,
            Fragment = GetRemark(inbound),
            UserName = client.Id,
            Password = "",
            Path = "",
            Port = (int)inbound.Port.Single!,
            Host = GetAddressOrDefault(inbound.Listen),
            Query = QueryUtilities.ToQuery(options),
        };

        return builder.ToString();
    }

    /// <summary>
    /// Generate Vless share link from xray inbound by client email.
    /// </summary>
    /// <param name="inbound">Vless inbound</param>
    /// <param name="email">Email of the client to share</param>
    /// <returns>Vless transfer link</returns>
    /// <exception cref="ArgumentException"></exception>
    public override string CreateLink(VlessInbound inbound, string email)
    {
        var client = inbound.Settings.Clients.FirstOrDefault(x => x.Email == email);
        if (client == null)
        {
            throw new ArgumentException($"Client for Email = {email} not found in inbound {inbound.Tag}");
        }

        return CreateLink(inbound, client);
    }

    /// <summary>
    /// Parse Vless share link to xray outbound.
    /// </summary>
    /// <param name="config">Vless transfer link</param>
    /// <returns>Vless outbound</returns>
    /// <exception cref="ArgumentException"></exception>
    public override VlessOutbound ParseVless(string config)
    {
        var uri = new Uri(config);
        if (uri.Scheme != VLESS_PROTOCOL)
        {
            throw new ArgumentException("Invalid protocol.");
        }

        var options = QueryUtilities.FromQuery<VlessOptions>(uri.Query);
        var streamSettings = ParseStreamSetting(options);

        if (streamSettings.Security == StreamSecurity.Tls)
        {
            streamSettings.TlsSettings = ParseTlsSetting(options);
        }
        else if (streamSettings.Security == StreamSecurity.Reality)
        {
            streamSettings.RealitySettings = ParseRealitySettings(options);
        }

        var remark = WebUtility.UrlDecode(uri.Fragment);

        return new VlessOutbound()
        {
            Tag = remark.Length > 0 ? remark.Substring(1) : $"out-{VLESS_PROTOCOL}-{uri.Port}",
            StreamSettings = streamSettings,
            Settings = new Outbound.VlessSettings()
            {
                Address = uri.Host,
                Id = uri.UserInfo,
                Port = uri.Port,
                Flow = TryParseEnumValue(options.Flow, XtlsFlow.None),
                Encryption = options.Encryption ?? "none",
            }
        };
    }

    #endregion

    #region VMess

    /// <summary>
    /// Generate VMess share link from xray inbound and client.
    /// </summary>
    /// <param name="inbound"VMess inbound></param>
    /// <param name="client">Client to share</param>
    /// <returns>VMess transfer link</returns>
    public override string CreateLink(VMessInbound inbound, VMessClient client)
    {
        var stream = inbound.StreamSettings;
        var options = new VMessTransferOptions()
        {
            Type = "none",
            Version = "2",
            Id = client.Id,
            Remark = GetRemark(inbound),
            Port = (int)inbound.Port.Single!,
            Address = GetAddressOrDefault(inbound.Listen),
            Network = EnumStringConverter.ToString(stream.Network),
            Tls = EnumStringConverter.ToString(stream.Security)
        };

        switch (stream.Network)
        {
            case StreamNetwork.Raw:
                {
                    var raw = stream.RawSettings ?? new();
                    var header = raw.Header;
                    if (header != null)
                    {
                        options.Type = EnumStringConverter.ToString(header.Type);
                    }

                    if (header != null && header is HttpSettingsHeaders)
                    {
                        var typedHeader = (HttpSettingsHeaders)header;

                        options.Path = typedHeader.Request.Path.First();
                        options.Host = SearchHost(typedHeader.Request.Headers);
                    }
                }
                break;

            case StreamNetwork.Kcp:
                {
                    var kcp = stream.KcpSettings ?? new();
                    var header = kcp.Header ?? new();

                    options.Path = kcp.Seed ?? "";
                    options.Type = EnumStringConverter.ToString(header.Type);
                }
                break;

            case StreamNetwork.Ws:
                {
                    var ws = stream.WSSettings ?? new();

                    options.Path = ws.Path ?? "";
                    options.Host = ws.Host == null ? SearchHost(ws.Headers) : ws.Host;
                }
                break;

            case StreamNetwork.Grpc:
                {
                    var grpc = stream.GRPCSettings ?? new();

                    options.Path = grpc.ServiceName;
                    options.Authority = grpc.Authority ?? "";

                    if (grpc.MultiMode)
                    {
                        options.Type = "multi";
                    }
                }
                break;

            case StreamNetwork.HttpUpgrade:
                {
                    var httpUpgrade = stream.HttpUpgradeSettings ?? new();

                    options.Path = httpUpgrade.Path ?? "";
                    options.Host = httpUpgrade.Host == null ? SearchHost(httpUpgrade.Headers) : httpUpgrade.Host;
                }
                break;

            case StreamNetwork.XHttp:
                {
                    var xhttp = stream.XHttpSettings ?? new();

                    options.Path = xhttp.Path ?? "";
                    options.Host = xhttp.Host == null ? SearchHost(xhttp.Headers) : xhttp.Host;
                    options.Mode = EnumStringConverter.ToString(xhttp.Mode);
                }
                break;
        }

        if (stream.Security == StreamSecurity.Tls && stream.TlsSettings != null)
        {
            var tls = stream.TlsSettings;
            if (tls.Alpn != null && tls.Alpn.Count > 0)
            {
                options.Alpn = string.Join(",", tls.Alpn);
            }

            if (tls.ServerName != null)
            {
                options.ServerName = tls.ServerName;
            }

            if (tls.Fingerprint != null)
            {
                options.Fingerprint = EnumStringConverter.ToString((Fingerprint)tls.Fingerprint);
            }

            if (tls.AllowInsecure)
            {
                options.AllowInsecure = true;
            }
        }

        return $"{VMESS_PROTOCOL}://{Base64.Encode(options.ToJson())}";
    }

    /// <summary>
    /// Generate VMess share link from xray inbound and client.
    /// </summary>
    /// <param name="inbound"VMess inbound></param>
    /// <param name="email">Email of the client to share</param>
    /// <returns>VMess transfer link</returns>
    public override string CreateLink(VMessInbound inbound, string email)
    {
        var client = inbound.Settings.Clients.FirstOrDefault(x => x.Email == email);
        if (client == null)
        {
            throw new ArgumentException($"Client for Email = {email} not found in inbound {inbound.Tag}");
        }

        return CreateLink(inbound, client);
    }

    /// <summary>
    /// Parse VMess share link to xray outbound.
    /// </summary>
    /// <param name="config">VMess transfer link</param>
    /// <returns>VMess outbound</returns>
    /// <exception cref="ArgumentException"></exception>
    public override VMessOutbound ParseVMess(string config)
    {
        var index = config.IndexOf(":");
        if (index == -1)
        {
            throw new ArgumentException("Invalid link format.");
        }

        var protocol = config.Substring(0, index);
        if (protocol != VMESS_PROTOCOL)
        {
            throw new ArgumentException("Invalid protocol.");
        }

        var data = config.Substring(index + 3);
        var options = VMessTransferOptions.FromJson(Base64.Decode(data));
        var streamSettings = new StreamSettings()
        {
            Network = TryParseEnumValue(options.Network, StreamNetwork.Raw),
            Security = TryParseEnumValue(options.Tls, StreamSecurity.None),
        };

        switch (streamSettings.Network)
        {
            case StreamNetwork.Raw:
                streamSettings.RawSettings = new RawSettings()
                {
                    Header = TryParseEnumValue(options.Type, HeadersType.None) == HeadersType.None ? new NoneSettingsHeaders() : new HttpSettingsHeaders()
                    {
                        Request = new HttpRequest()
                        {
                            Path = string.IsNullOrEmpty(options.Path) ? [] : [options.Path],
                            Headers = string.IsNullOrEmpty(options.Path) ? new NameValueCollection() : new NameValueCollection() {
                                    { "host", options.Host }
                                },
                        }
                    }
                };

                break;

            case StreamNetwork.Kcp:
                streamSettings.KcpSettings = new KcpSettings()
                {
                    Header = new KCPHeaders()
                    {
                        Type = TryParseEnumValue(options.Type, KcpHeaderType.None)
                    },
                    Seed = options.Path
                };

                break;

            case StreamNetwork.Ws:
                streamSettings.WSSettings = new WSSettings()
                {
                    Path = options.Path,
                    Host = options.Host,
                };

                break;

            case StreamNetwork.Grpc:
                streamSettings.GRPCSettings = new GRPCSettings()
                {
                    ServiceName = options.Path ?? "",
                    Authority = options.Authority ?? "",
                    MultiMode = options.Type == "multi"
                };

                break;

            case StreamNetwork.HttpUpgrade:
                streamSettings.HttpUpgradeSettings = new HttpUpgradeSettings()
                {
                    Path = options.Path,
                    Host = options.Host
                };

                break;

            case StreamNetwork.XHttp:
                streamSettings.XHttpSettings = new XHttpSettings()
                {
                    Path = options.Path,
                    Host = options.Host,
                    Mode = TryParseEnumValue(options.Mode, XHttpMode.Auto)
                };

                break;
        }

        if (streamSettings.Security == StreamSecurity.Tls)
        {
            streamSettings.TlsSettings = new TlsSettings()
            {
                ServerName = options.ServerName,
                Alpn = options.Alpn?.Split(",").ToList(),
                Fingerprint = TryParseEnumValue(options.Fingerprint, Fingerprint.None),
                AllowInsecure = options.AllowInsecure
            };
        }

        return new VMessOutbound()
        {
            Tag = string.IsNullOrEmpty(options.Remark) ? $"out-{VMESS_PROTOCOL}-{options.Port}" : options.Remark,
            StreamSettings = streamSettings,
            Settings = new Outbound.VMessSettings()
            {
                Address = options.Address,
                Port = options.Port,
                Id = options.Id,
                Security = TryParseEnumValue(options.ClientSecurity, VMessSecurity.None),
            },
        };
    }

    #endregion

    #region ShadowSocks

    /// <summary>
    /// Generate ShadowSocks share link from xray inbound and client.
    /// </summary>
    /// <param name="inbound">Xray inbound</param>
    /// <param name="client">Client to share</param>
    /// <returns>ShadowSocks transfer link</returns>
    public override string CreateLink(ShadowSocksInbound inbound, ShadowSocksClient client)
    {
        var stream = inbound.StreamSettings;
        var streamOptions = GetStreamOptions(stream);
        var options = new ShadowSocksOptions()
        {
            Type = streamOptions.Type,
            Path = streamOptions.Path,
            Host = streamOptions.Host,
            Mode = streamOptions.Mode,
            Seed = streamOptions.Seed,
            Authority = streamOptions.Authority,
            ServiceName = streamOptions.ServiceName,
            HeaderType = streamOptions.HeaderType,
        };

        if (stream.Security == StreamSecurity.Tls && stream.TlsSettings != null)
        {
            GetTlsOptions(stream.TlsSettings, options);
        }

        var methodString = inbound.Settings?.Method != null ? EnumStringConverter.ToString((EncryptionMethod)inbound.Settings.Method) : "";
        var part = $"{methodString}:{client.Password}";
        if (methodString.StartsWith("2022"))
        {
            part = $"{methodString}:{inbound.Settings?.Password ?? ""}:{client.Password}";
        }

        var builder = new UriBuilder()
        {
            Scheme = SHADOW_SOCKS_PROTOCOL,
            Path = "",
            UserName = Base64.Encode(part),
            Password = "",
            Port = (int)inbound.Port.Single!,
            Host = GetAddressOrDefault(inbound.Listen),
            Query = QueryUtilities.ToQuery(options),
            Fragment = GetRemark(inbound)
        };

        return builder.ToString();
    }

    /// <summary>
    /// Generate ShadowSocks share link from xray inbound by client email.
    /// </summary>
    /// <param name="inbound">Xray inbound</param>
    /// <param name="client">Client to share</param>
    /// <returns>ShadowSocks transfer link</returns>
    public override string CreateLink(ShadowSocksInbound inbound, string email)
    {
        var client = inbound.Settings?.Clients.FirstOrDefault(x => x.Email == email);
        if (client == null)
        {
            throw new ArgumentException($"Client for Email = {email} not found in inbound {inbound.Tag}");
        }

        return CreateLink(inbound, client);
    }

    /// <summary>
    /// Parse ShadowSocks share link to xray outbound.
    /// </summary>
    /// <param name="config">ShadowSocks transfer link</param>
    /// <returns>ShadowSocks outbound</returns>
    /// <exception cref="ArgumentException"></exception>
    public override ShadowSocksOutbound ParseShadowSocks(string config)
    {
        var uri = new Uri(config);
        if (uri.Scheme != SHADOW_SOCKS_PROTOCOL)
        {
            throw new ArgumentException("Invalid protocol.");
        }

        var options = QueryUtilities.FromQuery<ShadowSocksOptions>(uri.Query);
        var streamSettings = ParseStreamSetting(options);

        if (streamSettings.Security == StreamSecurity.Tls)
        {
            streamSettings.TlsSettings = ParseTlsSetting(options);
        }

        var remark = WebUtility.UrlDecode(uri.Fragment);
        var userData = Base64.Decode(uri.UserInfo).Split(":");

        return new ShadowSocksOutbound()
        {
            Tag = remark.Length > 0 ? remark.Substring(1) : $"out-{SHADOW_SOCKS_PROTOCOL}-{uri.Port}",
            StreamSettings = streamSettings,
            Settings = new Outbound.ShadowSocksSettings()
            {
                Port = uri.Port,
                Address = uri.Host,
                Password = string.Join(":", userData[1..]),
                Method = TryParseEnumValue(userData[0], EncryptionMethod.None),
            }
        };
    }

    #endregion

    #region  Trojan

    /// <summary>
    /// Generate Trojan share link from xray inbound and client.
    /// </summary>
    /// <param name="inbound">Trojan inbound</param>
    /// <param name="client">Client to share</param>
    /// <returns>Trojan transfer link</returns>
    public override string CreateLink(TrojanInbound inbound, TrojanClient client)
    {
        var stream = inbound.StreamSettings;
        var streamOptions = GetStreamOptions(stream);
        var security = stream.Security;
        var options = new TrojanOptions()
        {
            Type = streamOptions.Type,
            Path = streamOptions.Path,
            Host = streamOptions.Host,
            Mode = streamOptions.Mode,
            Seed = streamOptions.Seed,
            Authority = streamOptions.Authority,
            ServiceName = streamOptions.ServiceName,
            HeaderType = streamOptions.HeaderType,
            Security = EnumStringConverter.ToString(security),
        };

        if (security == StreamSecurity.Tls && stream.TlsSettings != null)
        {
            GetTlsOptions(stream.TlsSettings, options);
        }
        else if (security == StreamSecurity.Reality && stream.RealitySettings != null)
        {
            GetRealityOptions(stream.RealitySettings, options);
        }

        var builder = new UriBuilder()
        {
            Path = "",
            Password = "",
            Scheme = TROJAN_PROTOCOL,
            UserName = client.Password,
            Fragment = GetRemark(inbound),
            Port = (int)inbound.Port.Single!,
            Host = GetAddressOrDefault(inbound.Listen),
            Query = QueryUtilities.ToQuery(options),
        };

        return builder.ToString();
    }

    /// <summary>
    /// Generate Trojan share link from xray inbound by client email.
    /// </summary>
    /// <param name="inbound">Trojan inbound</param>
    /// <param name="client">Client to share</param>
    /// <returns>Trojan transfer link</returns>
    public override string CreateLink(TrojanInbound inbound, string email)
    {
        var client = inbound.Settings?.Clients.FirstOrDefault(x => x.Email == email);
        if (client == null)
        {
            throw new ArgumentException($"Client for Email = {email} not found in inbound {inbound.Tag}");
        }

        return CreateLink(inbound, client);
    }

    /// <summary>
    /// Parse Trojan share link to xray outbound.
    /// </summary>
    /// <param name="config">Trojan transfer link</param>
    /// <returns>Trojan outbound</returns>
    /// <exception cref="ArgumentException"></exception>
    public override TrojanOutbound ParseTrojan(string config)
    {
        var uri = new Uri(config);
        if (uri.Scheme != TROJAN_PROTOCOL)
        {
            throw new ArgumentException("Invalid protocol.");
        }

        var options = QueryUtilities.FromQuery<TrojanOptions>(uri.Query);
        var streamSettings = ParseStreamSetting(options);

        if (streamSettings.Security == StreamSecurity.Tls)
        {
            streamSettings.TlsSettings = ParseTlsSetting(options);
        }
        else if (streamSettings.Security == StreamSecurity.Reality)
        {
            streamSettings.RealitySettings = ParseRealitySettings(options);
        }

        var remark = WebUtility.UrlDecode(uri.Fragment);

        return new TrojanOutbound()
        {
            Tag = remark.Length > 0 ? remark.Substring(1) : $"out-{TROJAN_PROTOCOL}-{uri.Port}",
            StreamSettings = streamSettings,
            Settings = new Outbound.TrojanSettings()
            {
                Address = uri.Host,
                Port = uri.Port,
                Password = uri.UserInfo
            }
        };
    }

    #endregion

    #region Socks

    // TODO: need check handing stream settings in transfer link
    /// <summary>
    /// Generate Socks share link from xray inbound and client.
    /// </summary>
    /// <param name="inbound">Socks inbound</param>
    /// <param name="account">Client to share</param>
    /// <returns>Socks transfer link</returns>
    public override string CreateLink(SocksInbound inbound, SocksAccount account)
    {
        var uriBuilder = new UriBuilder()
        {
            Path = "",
            Scheme = SOCKS_PROTOCOL,
            UserName = account.User,
            Password = account.Password,
            Fragment = GetRemark(inbound),
            Port = (int)inbound.Port.Single!,
            Host = GetAddressOrDefault(inbound.Listen),
        };

        return uriBuilder.Uri.ToString();
    }

    /// <summary>
    /// Generate Trojan share link from xray inbound by client email.
    /// </summary>
    /// <param name="inbound">Socks inbound</param>
    /// <param name="username">Client to share</param>
    /// <returns>Socks transfer link</returns>
    /// <exception cref="ArgumentException"></exception>
    public override string CreateLink(SocksInbound inbound, string username)
    {
        var client = inbound.Settings?.Accounts.FirstOrDefault(x => x.User == username);
        if (client == null)
        {
            throw new ArgumentException($"Client for Username = {username} not found in inbound {inbound.Tag}");
        }

        return CreateLink(inbound, client);
    }

    /// <summary>
    /// Parse Socks share link to xray outbound.
    /// </summary>
    /// <param name="config">Socks transfer link</param>
    /// <returns>Socks outbound</returns>
    /// <exception cref="ArgumentException"></exception>
    public override SocksOutbound ParseSocks(string config)
    {
        var uri = new Uri(config);
        if (uri.Scheme != SOCKS_PROTOCOL)
        {
            throw new ArgumentException("Invalid protocol.");
        }

        var remark = WebUtility.UrlDecode(uri.Fragment);
        var userData = uri.UserInfo.Split(":");

        return new SocksOutbound()
        {
            Tag = remark.Length > 0 ? remark.Substring(1) : $"out-{SOCKS_PROTOCOL}-{uri.Port}",
            Settings = new Outbound.SocksSettings()
            {
                Address = uri.Host,
                Port = uri.Port,
                User = userData[0],
                Password = userData[1]
            }
        };
    }

    #endregion

    #region Hysteria

    // Unsupported
    public override HysteriaOutbound ParseHysteria(string config)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region  private utilities

    private void GetTlsOptions(TlsSettings tlsSettings, TransferOptions options)
    {
        var alpns = tlsSettings.Alpn ?? new();
        if (alpns.Count > 0)
        {
            options.Alpn = string.Join(",", alpns);
        }

        if (tlsSettings.ServerName != null)
        {
            options.ServerName = tlsSettings.ServerName;
        }

        if (tlsSettings.Fingerprint != null)
        {
            options.Fingerprint = EnumStringConverter.ToString((Fingerprint)tlsSettings.Fingerprint);
        }

        if (tlsSettings.AllowInsecure)
        {
            options.AllowInsecure = "1";
        }
    }

    private void GetRealityOptions(RealitySettings reality, RealityTransferOptions options)
    {
        if (reality.ServerNames.Count > 0)
        {
            options.ServerName = reality.ServerNames.ElementAt(RandomUtilities.GetInRange(0, reality.ServerNames.Count));
        }

        if (reality.Password != null)
        {
            options.PublicKey = reality.Password;
        }

        if (reality.ShortIds != null && reality.ShortIds.Count > 0)
        {
            options.ShortId = reality.ShortIds.ElementAt(RandomUtilities.GetInRange(0, reality.ShortIds.Count));
        }

        if (!string.IsNullOrEmpty(reality.Mldsa65Verify))
        {
            options.Mldsa65Verify = reality.Mldsa65Verify;
        }

        options.Fingerprint = EnumStringConverter.ToString(reality.Fingerprint);
        options.SpiderX = $"/{RandomUtilities.Seq(15)}";
    }

    private StreamOptions GetStreamOptions(StreamSettings stream)
    {
        var options = new StreamOptions()
        {
            Type = EnumStringConverter.ToString(stream.Network),
        };

        switch (stream.Network)
        {
            case StreamNetwork.Raw:
                {
                    var raw = stream.RawSettings ?? new();
                    var header = raw.Header;
                    if (header != null && header is HttpSettingsHeaders)
                    {
                        var typedHeader = (HttpSettingsHeaders)header;

                        options.Path = typedHeader.Request.Path.FirstOrDefault();
                        options.Host = SearchHost(typedHeader.Request.Headers);
                        options.HeaderType = "http";
                    }
                }
                break;

            case StreamNetwork.Kcp:
                {
                    var kcp = stream.KcpSettings ?? new();
                    var header = kcp.Header ?? new();

                    options.HeaderType = EnumStringConverter.ToString(header.Type);
                    options.Seed = kcp.Seed;
                }
                break;

            case StreamNetwork.Ws:
                {
                    var ws = stream.WSSettings ?? new();

                    options.Path = ws.Path;
                    options.Host = ws.Host == null ? SearchHost(ws.Headers) : ws.Host;
                }
                break;

            case StreamNetwork.Grpc:
                {
                    var grpc = stream.GRPCSettings ?? new();

                    options.ServiceName = grpc.ServiceName;
                    options.Authority = grpc.Authority;

                    if (grpc.MultiMode)
                    {
                        options.Mode = "multi";
                    }
                }
                break;

            case StreamNetwork.HttpUpgrade:
                {
                    var httpUpgrade = stream.HttpUpgradeSettings ?? new();

                    options.Path = httpUpgrade.Path;
                    options.Host = httpUpgrade.Host == null ? SearchHost(httpUpgrade.Headers) : httpUpgrade.Host;
                }
                break;

            case StreamNetwork.XHttp:
                {
                    var xhttp = stream.XHttpSettings ?? new();

                    options.Path = xhttp.Path;
                    options.Host = xhttp.Host == null ? SearchHost(xhttp.Headers) : xhttp.Host;
                    options.Mode = EnumStringConverter.ToString(xhttp.Mode);
                }
                break;
        }


        return options;
    }

    private StreamSettings ParseStreamSetting(TransferOptions options)
    {
        var streamSettings = new StreamSettings()
        {
            Network = TryParseEnumValue(options.Type, StreamNetwork.Raw),
            Security = TryParseEnumValue(options.Security, StreamSecurity.None)
        };

        switch (streamSettings.Network)
        {
            case StreamNetwork.Raw:
                streamSettings.RawSettings = new RawSettings()
                {
                    Header = TryParseEnumValue(options.HeaderType, HeadersType.None) == HeadersType.None ? new NoneSettingsHeaders() : new HttpSettingsHeaders()
                    {
                        Request = new HttpRequest()
                        {
                            Path = string.IsNullOrEmpty(options.Path) ? [] : [options.Path],
                            Headers = string.IsNullOrEmpty(options.Path) ? new NameValueCollection() : new NameValueCollection() {
                                    { "host", options.Host }
                                },
                        }
                    }
                };

                break;

            case StreamNetwork.Kcp:
                streamSettings.KcpSettings = new KcpSettings()
                {
                    Header = new KCPHeaders()
                    {
                        Type = TryParseEnumValue(options.HeaderType, KcpHeaderType.None)
                    },
                    Seed = options.Path
                };

                break;

            case StreamNetwork.Ws:
                streamSettings.WSSettings = new WSSettings()
                {
                    Path = options.Path,
                    Host = options.Host,
                };

                break;

            case StreamNetwork.Grpc:
                streamSettings.GRPCSettings = new GRPCSettings()
                {
                    ServiceName = options.ServiceName ?? "",
                    Authority = options.Authority ?? "",
                    MultiMode = options.Mode == "multi"
                };

                break;

            case StreamNetwork.HttpUpgrade:
                streamSettings.HttpUpgradeSettings = new HttpUpgradeSettings()
                {
                    Path = options.Path,
                    Host = options.Host
                };

                break;

            case StreamNetwork.XHttp:
                streamSettings.XHttpSettings = new XHttpSettings()
                {
                    Path = options.Path,
                    Host = options.Host,
                    Mode = TryParseEnumValue(options.Mode, XHttpMode.Auto)
                };

                break;
        }

        return streamSettings;
    }

    private TlsSettings ParseTlsSetting(TransferOptions options) => new TlsSettings()
    {
        ServerName = options.ServerName,
        EchConfigList = options.EchConfigList,
        Alpn = options.Alpn?.Split(",").ToList(),
        AllowInsecure = options.AllowInsecure == "1",
        Fingerprint = TryParseEnumValue(options.Fingerprint, Fingerprint.None),
    };

    private RealitySettings ParseRealitySettings(RealityTransferOptions options) => new RealitySettings()
    {
        Password = options.PublicKey,
        Fingerprint = TryParseEnumValue(options.Fingerprint, Fingerprint.Chrome),
        ServerName = options.ServerName ?? "",
        ShortId = options.ShortId ?? "",
        SpiderX = options.SpiderX ?? "",
        Mldsa65Verify = options.Mldsa65Verify ?? "",
    };

    private string SearchHost(NameValueCollection headers)
    {
        var values = headers.GetValues("host");

        return values?.FirstOrDefault() ?? "";
    }

    private string GetAddressOrDefault(string? listen) => string.IsNullOrEmpty(listen) ? DEFAULT_ADDRESS : listen;

    private T TryParseEnumValue<T>(string? value, T defaultValue) where T : struct, Enum => value == null ? defaultValue : EnumStringConverter.TryParse(value, defaultValue);
    private T? TryParseEnumValue<T>(string? value) where T : struct, Enum => value == null ? null : EnumStringConverter.TryParse<T>(value);

    #endregion
}

class StreamOptions
{
    public string? Path { get; set; }
    public string? Host { get; set; }
    public string? HeaderType { get; set; }
    public string? Seed { get; set; }
    public string? ServiceName { get; set; }
    public string? Authority { get; set; }
    public string? Mode { get; set; }
    public string? Type { get; set; }
}

class VMessTransferOptions
{
    [JsonPropertyName("add")]
    public required string Address { get; set; }

    [JsonPropertyName("port")]
    public required int Port { get; set; }

    [JsonPropertyName("v")]
    public string Version { get; set; } = "2";

    [JsonPropertyName("allowInsecure")]
    public bool AllowInsecure { get; set; }

    [JsonPropertyName("fp")]
    public string Fingerprint { get; set; } = string.Empty;

    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("alpn")]
    public string? Alpn { get; set; }

    [JsonPropertyName("sni")]
    public string? ServerName { get; set; }

    [JsonPropertyName("scy")]
    public string? ClientSecurity { get; set; }

    [JsonPropertyName("ps")]
    public string? Remark { get; set; }

    [JsonPropertyName("path")]
    public string Path { get; set; } = string.Empty;

    [JsonPropertyName("tls")]
    public string Tls { get; set; } = "none";

    [JsonPropertyName("net")]
    public required string Network { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("host")]
    public string Host { get; set; } = string.Empty;

    [JsonPropertyName("aid")]
    public string Aid { get; set; } = "0";

    [JsonPropertyName("authority")]
    public string? Authority { get; set; }

    [JsonPropertyName("mode")]
    public string? Mode { get; set; }

    private static readonly JsonSerializerOptions _serializeOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    public string ToJson() => JsonSerializer.Serialize(this, _serializeOptions);

    public static VMessTransferOptions FromJson(string json) => JsonSerializer.Deserialize<VMessTransferOptions>(json, _serializeOptions)!;
}

static class QueryUtilities
{
    private static JsonSerializerOptions _options = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    public static string ToQuery(object model)
    {
        var json = JsonSerializer.Serialize(model, _options);
        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json)!;

        return $"?{string.Join("&", dict.Select(x => $"{x.Key}={x.Value}"))}";
    }

    public static T FromQuery<T>(string query) where T : new()
    {
        var validQuery = query.Trim();
        if (string.IsNullOrWhiteSpace(query))
        {
            throw new ArgumentException("Invalid query argument");
        }

        if (validQuery.StartsWith("?"))
        {
            validQuery = validQuery.Substring(1);
        }

        var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var pairs = validQuery.Split('&', StringSplitOptions.RemoveEmptyEntries);

        foreach (var pair in pairs)
        {
            var parts = pair.Split('=', 2);
            var key = HttpUtility.UrlDecode(parts[0]);
            var value = parts.Length > 1 ? HttpUtility.UrlDecode(parts[1]) : "";

            dict[key] = value;
        }

        return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(dict, _options), _options)!;
    }
}

class TransferOptions
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("path")]
    public string? Path { get; set; }

    [JsonPropertyName("host")]
    public string? Host { get; set; }

    [JsonPropertyName("headerType")]
    public string? HeaderType { get; set; }

    [JsonPropertyName("seed")]
    public string? Seed { get; set; }

    [JsonPropertyName("serviceName")]
    public string? ServiceName { get; set; }

    [JsonPropertyName("authority")]
    public string? Authority { get; set; }

    [JsonPropertyName("mode")]
    public string? Mode { get; set; }

    [JsonPropertyName("security")]
    public string? Security { get; set; }

    [JsonPropertyName("alpn")]
    public string? Alpn { get; set; }

    [JsonPropertyName("sni")]
    public string? ServerName { get; set; }

    [JsonPropertyName("ech")]
    public string? EchConfigList { get; set; }

    [JsonPropertyName("fp")]
    public string? Fingerprint { get; set; }

    [JsonPropertyName("allowInsecure")]
    public string? AllowInsecure { get; set; }
}

class RealityTransferOptions : TransferOptions
{

    [JsonPropertyName("flow")]
    public string? Flow { get; set; }

    [JsonPropertyName("pbk")]
    public string? PublicKey { get; set; }

    [JsonPropertyName("pqv")]
    public string? Mldsa65Verify { get; set; }

    [JsonPropertyName("sid")]
    public string? ShortId { get; set; }

    [JsonPropertyName("spx")]
    public string? SpiderX { get; set; }
}

class VlessOptions : RealityTransferOptions
{
    [JsonPropertyName("encryption")]
    public string? Encryption { get; set; }
}

class TrojanOptions : RealityTransferOptions { }

class ShadowSocksOptions : TransferOptions { }

static class Base64
{
    public static string Encode(string value) => Convert.ToBase64String(Encoding.UTF8.GetBytes(value));

    public static string Decode(string value) => Encoding.UTF8.GetString(Convert.FromBase64String(value));
}