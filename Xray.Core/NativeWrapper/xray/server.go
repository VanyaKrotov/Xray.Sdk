package xray

import (
	"NativeWrapper/transfer"
	"encoding/json"
	"log"
	"runtime"
	"runtime/debug"

	_ "github.com/xtls/xray-core/app/proxyman/inbound"
	_ "github.com/xtls/xray-core/app/proxyman/outbound"
	"github.com/xtls/xray-core/core"
	iconf "github.com/xtls/xray-core/infra/conf"
	_ "github.com/xtls/xray-core/proxy/freedom"
	_ "github.com/xtls/xray-core/proxy/socks"
)

const (
	JsonParseError     int = 1
	LoadConfigError    int = 2
	InitXrayError      int = 3
	StartXrayError     int = 4
	XrayAlreadyStarted int = 5
)

func Start(jsonConfig string) (*core.Instance, *transfer.Response) {
	var cfg iconf.Config
	if err := json.Unmarshal([]byte(jsonConfig), &cfg); err != nil {
		return nil, transfer.NewResponse(JsonParseError, err.Error())
	}

	coreCfg, err := cfg.Build()
	if err != nil {
		return nil, transfer.NewResponse(LoadConfigError, err.Error())
	}

	instance, err := core.New(coreCfg)
	if err != nil {
		log.Printf("Xray init error: %v", err)

		return nil, transfer.NewResponse(InitXrayError, err.Error())
	}

	if err := instance.Start(); err != nil {
		instance.Close()
		log.Printf("Xray start error: %v", err)

		return nil, transfer.NewResponse(StartXrayError, err.Error())
	}

	return instance, transfer.SuccessResponse("Server started")
}

func Stop(instance *core.Instance) {
	instance.Close()
	instance = nil

	runtime.GC()
	debug.FreeOSMemory()
}
