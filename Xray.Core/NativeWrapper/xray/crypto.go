package xray

import (
	"NativeWrapper/transfer"
	"crypto/ecdh"
	"crypto/rand"
	"encoding/base64"
	"fmt"

	"lukechampine.com/blake3"
)

const (
	InvalidPrivateKeyLength int = 200
	GeneratePrivateKeyError int = 201
)

func Curve25519Genkey(inputKey string) *transfer.Response {
	encoding := base64.RawURLEncoding

	var privateKey []byte
	if len(inputKey) > 0 {
		privateKey, _ = encoding.DecodeString(inputKey)
		if len(privateKey) != 32 {
			return transfer.New(InvalidPrivateKeyLength, "Invalid length of X25519 private key.")
		}
	}

	privateKey, password, hash32, err := genCurve25519(privateKey)
	if err != nil {
		return transfer.New(GeneratePrivateKeyError, err.Error())
	}

	return transfer.Success(fmt.Sprintf("%v|%v|%v", encoding.EncodeToString(privateKey), encoding.EncodeToString(password), encoding.EncodeToString(hash32[:])))
}

func genCurve25519(inputPrivateKey []byte) (privateKey []byte, password []byte, hash32 [32]byte, returnErr error) {
	if len(inputPrivateKey) > 0 {
		privateKey = inputPrivateKey
	}

	if privateKey == nil {
		privateKey = make([]byte, 32)
		rand.Read(privateKey)
	}

	// Modify random bytes using algorithm described at:
	// https://cr.yp.to/ecdh.html
	// (Just to make sure printing the real private key)
	privateKey[0] &= 248
	privateKey[31] &= 127
	privateKey[31] |= 64

	key, err := ecdh.X25519().NewPrivateKey(privateKey)
	if err != nil {
		returnErr = err
		return
	}

	password = key.PublicKey().Bytes()
	hash32 = blake3.Sum256(password)
	return
}
