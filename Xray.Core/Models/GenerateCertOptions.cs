namespace Xray.Core.Models;

public class GenerateCertOptions
{
    /// <summary>
    /// The domain name for the certificate.
    /// </summary>
    public required ICollection<string> Domains { get; set; }

    /// <summary>
    /// The common name for the certificate.
    /// </summary>
    public string CommonName { get; set; } = string.Empty;

    /// <summary>
    /// The organization name for the certificate.
    /// </summary>
    public string Organization { get; set; } = string.Empty;

    /// <summary>
    /// Whether this certificate is a CA.
    /// </summary>
    public bool IsCA { get; set; }

    /// <summary>
    /// Expire time of the certificate. Default value 3 months.
    /// </summary>
    public string Expire { get; set; } = string.Empty;
}