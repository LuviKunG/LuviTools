using LitJson;

namespace LuviKunG.Web.Socket
{
    public delegate void WebSocketEvent(JsonData json);
    public delegate void ConnectionCallback();
    public delegate void ConnectionCallbackError(int code, string reason, bool wasClean);
}