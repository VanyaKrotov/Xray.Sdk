package xray

import (
	"net/http"
	"net/url"
	"strconv"
	"time"

	"NativeWrapper/transfer"
)

const (
	Localhost        string = "http://127.0.0.1:"
	PingTimeoutError int    = 100
	PingError        int    = 101
)

func PingConfig(jsonConfig string, port int, testingURL string) *transfer.Response {
	instance, response := StartServer(jsonConfig)
	if !response.IsSuccess() {
		return response
	}

	proxyUrl, err := url.Parse(Localhost + strconv.Itoa(port))
	if err != nil {
		instance.Close()

		return transfer.NewResponse(PingError, err.Error())
	}

	timeoutResult := PingProxy(testingURL, proxyUrl)

	instance.Close()

	return timeoutResult
}

func PingProxy(testUrl string, proxyUrl *url.URL) *transfer.Response {
	defaultTransport := &http.Transport{
		Proxy:               http.ProxyURL(proxyUrl),
		TLSHandshakeTimeout: time.Second * 5,
		DisableKeepAlives:   true,
	}

	return pingWithTransport(testUrl, defaultTransport)
}

func pingWithTransport(testUrl string, transport *http.Transport) *transfer.Response {
	start := time.Now()
	http.DefaultTransport = transport
	response, err := http.Head(testUrl)
	delay := time.Since(start).Milliseconds()
	if err != nil {
		return transfer.NewResponse(PingTimeoutError, err.Error())
	}

	if response.StatusCode == 204 {
		return transfer.SuccessResponse(strconv.FormatInt(delay, 10))
	}

	return transfer.NewResponse(PingTimeoutError, "Ping response error")
}

func Ping(port int, testUrl string) *transfer.Response {
	if port == 0 {
		defaultTransport := &http.Transport{
			TLSHandshakeTimeout: time.Second * 5,
			DisableKeepAlives:   true,
		}

		return pingWithTransport(testUrl, defaultTransport)
	}

	proxyUrl, err := url.Parse(Localhost + strconv.Itoa(port))
	if err != nil {
		return transfer.NewResponse(PingError, err.Error())
	}

	return PingProxy(testUrl, proxyUrl)
}
