package main

import "C"

import (
	"encoding/base64"
	"native_lib/crypto_helpers"
)

//export Curve25519Genkey
func Curve25519Genkey(cKey *C.char) *C.char {
	key := C.GoString(cKey)
	response := crypto_helpers.Curve25519Genkey(key, base64.RawURLEncoding)

	return C.CString(response.ToString())
}

//export Curve25519GenkeyWG
func Curve25519GenkeyWG(cKey *C.char) *C.char {
	key := C.GoString(cKey)
	response := crypto_helpers.Curve25519Genkey(key, base64.StdEncoding)

	return C.CString(response.ToString())
}

//export ExecuteUUID
func ExecuteUUID(cInput *C.char) *C.char {
	input := C.GoString(cInput)
	response := crypto_helpers.ExecuteUUID(input)

	return C.CString(response.ToString())
}

//export ExecuteMLDSA65
func ExecuteMLDSA65(cInput *C.char) *C.char {
	input := C.GoString(cInput)
	response := crypto_helpers.ExecuteMLDSA65(input)

	return C.CString(response.ToString())
}

//export ExecuteMLKEM768
func ExecuteMLKEM768(cInput *C.char) *C.char {
	input := C.GoString(cInput)
	response := crypto_helpers.ExecuteMLKEM768(input)

	return C.CString(response.ToString())
}

//export ExecuteVLESSEnc
func ExecuteVLESSEnc() *C.char {
	response := crypto_helpers.ExecuteVLESSEnc()

	return C.CString(response.ToString())
}

func main() {}
