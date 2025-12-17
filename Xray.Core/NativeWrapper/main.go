package main

import "C"
import (
	"sync"

	"github.com/xtls/xray-core/core"

	"NativeWrapper/transfer"
	"NativeWrapper/xray"
)

var (
	mu        sync.Mutex
	instances map[string]*core.Instance
)

func init() {
	instances = make(map[string]*core.Instance)
}

//export Start
func Start(cUuid *C.char, cJson *C.char) *C.char {
	mu.Lock()
	defer mu.Unlock()

	uuid := C.GoString(cUuid)
	if _, ok := instances[uuid]; ok {
		return C.CString(transfer.New(xray.XrayAlreadyStarted, "Xray server already started").ToString())
	}

	json := C.GoString(cJson)
	inst, resp := xray.Start(json)
	if resp.IsSuccess() {
		instances[uuid] = inst
	}

	return C.CString(resp.ToString())
}

//export Stop
func Stop(cUuid *C.char) *C.char {
	mu.Lock()
	defer mu.Unlock()

	uuid := C.GoString(cUuid)
	if instance, ok := instances[uuid]; ok {
		xray.Stop(instance)
		delete(instances, uuid)
	}

	return C.CString(transfer.Success("Server stopped").ToString())
}

//export IsStarted
func IsStarted(cUuid *C.char) C.int {
	mu.Lock()
	defer mu.Unlock()

	uuid := C.GoString(cUuid)
	if instance, ok := instances[uuid]; ok || !instance.IsRunning() {
		return 0
	}

	return 1
}

//export GetXrayCoreVersion
func GetXrayCoreVersion() *C.char {
	return C.CString(core.Version())
}

func main() {}
