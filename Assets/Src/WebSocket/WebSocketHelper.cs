using UnityWebSocket;
using UnityEngine.Events;
namespace Dawn{

    public class WebSocketHelper
    {
        WebSocket socket;
        public UnityAction OnOpen;
        public UnityAction<string> OnRecvMessage;
        public UnityAction<int,string> OnClose;
        public UnityAction<string> OnError;
        public WebSocketHelper(string url){
            socket = new WebSocket(url);
            socket.OnOpen += Socket_OnOpen;
            socket.OnMessage += Socket_OnMessage;
            socket.OnClose += Socket_OnClose;
            socket.OnError += Socket_OnError;
            socket.ConnectAsync();
        }

        private void Socket_OnOpen(object sender, OpenEventArgs e)
        {
            if (OnOpen != null){
                OnOpen();
            }
        }

        private void Socket_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.IsBinary)
            {
            }
            else if (e.IsText)
            {
            }
            if (OnRecvMessage != null){
                OnRecvMessage(e.Data);
            }
        }

        private void Socket_OnClose(object sender, CloseEventArgs e)
        {
            if(OnClose != null){
                OnClose(((int)e.StatusCode),e.Reason);
            }
        }

        private void Socket_OnError(object sender, ErrorEventArgs e)
        {
            if (OnError != null){
                OnError(e.Message);
            }
        }
        public void Clear(){
            socket.OnOpen -= Socket_OnOpen;
            socket.OnMessage -= Socket_OnMessage;
            socket.OnClose -= Socket_OnClose;
            socket.OnError -= Socket_OnError;
            OnOpen = null;
            OnRecvMessage = null;
            OnClose = null;
            OnError = null;           
        }
    }
}