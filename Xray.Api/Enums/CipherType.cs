namespace Xray.Api.Enums;

public enum CipherType
{
    Unknown = 0,
    AES_128_GCM = 5,
    AES_256_GCM = 6,
    CHACHA20_POLY1305 = 7,
    XCHACHA20_POLY1305 = 8,
    None = 9,
    Unrecognized = -1,
}
