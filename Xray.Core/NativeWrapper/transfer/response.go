package transfer

import "fmt"

type Response struct {
	Code    int
	Message string
}

func NewResponse(code int, message string) *Response {
	return &Response{Code: code, Message: message}
}

func SuccessResponse(message string) *Response {
	return NewResponse(0, message)
}

func (r Response) ToString() string {
	return fmt.Sprintf("%d|%s", r.Code, r.Message)
}

func (r Response) IsSuccess() bool {
	return r.Code == 0
}
