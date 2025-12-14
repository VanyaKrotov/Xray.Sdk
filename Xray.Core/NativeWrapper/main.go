package main

import "C"
import (
	"sync"

	"github.com/xtls/xray-core/core"

	"NativeWrapper/transfer"
	"NativeWrapper/xray"
)

var (
	instance *core.Instance
	mu       sync.Mutex
)

//export StartServer
func StartServer(jsonConfig *C.char) *C.char {
	mu.Lock()
	defer mu.Unlock()

	if instance != nil {
		return C.CString(transfer.NewResponse(xray.XrayAlreadyStarted, "XRay server already started").ToString())
	}

	goJSON := C.GoString(jsonConfig)
	inst, resp := xray.StartServer(goJSON)
	if !resp.IsSuccess() {
		return C.CString(resp.ToString())
	}

	instance = inst

	return C.CString(resp.ToString())
}

//export StopServer
func StopServer() *C.char {
	mu.Lock()
	defer mu.Unlock()

	if instance != nil {
		xray.StopServer(instance)
		instance = nil
	}

	return C.CString(transfer.SuccessResponse("Server stopped").ToString())
}

//export IsStarted
func IsStarted() C.int {
	mu.Lock()
	defer mu.Unlock()

	if instance == nil || !instance.IsRunning() {
		return 0
	}

	return 1
}

//export PingConfig
func PingConfig(jsonConfig *C.char, port int, testingURL *C.char) *C.char {
	goJSON := C.GoString(jsonConfig)
	url := C.GoString(testingURL)
	result := xray.PingConfig(goJSON, port, url)

	return C.CString(result.ToString())
}

//export Ping
func Ping(port C.int, testingURL *C.char) *C.char {
	url := C.GoString(testingURL)
	ping := xray.Ping(int(port), url)

	return C.CString(ping.ToString())
}

//export GetXrayCoreVersion
func GetXrayCoreVersion() *C.char {
	return C.CString(core.Version())
}

func main() {}
