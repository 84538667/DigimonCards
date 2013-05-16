using System;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.Networking.Connectivity;
using Windows.ApplicationModel.Core;
/*
 * 据说像这个class这么写可以成功的与服务器建立tcp连接并且接收数据
 * 这个东西具体怎么用还没搞明白，不过应该有希望了！！！
 * 因为无法返回一个streamsocket连接，要是这么写的话，就需要在大厅 和 游戏时分别创建socket连接，
 * 比较麻烦，所以最好搞清楚怎么能够返回一个socket连接
 * 如果不能够创建静态函数建立连接的话 这个class完全没有必要了
 * 但是异步无返回值无法交互 恶心啊
 * 2013.5.13/0.04
 * */
namespace DigimonCard
{
    public sealed class Net{
        private StreamSocket s = new StreamSocket();

        private Net(){
        }
 
        
     /*   /// <summary>
        /// 连接使用 tcp 协议的服务端
        /// </summary>
        /// <param name="ip">服务端的ip地址</param>
        /// <param name="port">服务端的端口号</param>
        /// <returns></returns>
        public async void ConnectServer( string ip ,int port ) {
            
            await s.ConnectAsync(new HostName("www.baidu.com"), "80");
            
            if(s.Information != null ){
                await BeginReceived();
            }
        }

        private async Task BeginReceived()
        {

            //绑定已连接的StreamSocket到DataReader
            DataReader reader = new DataReader(s.InputStream);
            while (true)
            {
                try
                {
                    //尝试从StreamSocket 读取到数据 读取2个字节的数据（包头）
                    uint sizeFieldCount = await reader.LoadAsync(sizeof(UInt16));
                    if (sizeFieldCount != sizeof(UInt16))
                    {
                        return;
                    }
                }
                catch (Exception exception)
                {
                    return;
                }
                //临时处理读取器 的字节流数组
                byte[] tempByteArr;
                tempByteArr = new byte[2];

                //将刚才reader读取到的数据填充到tempByteArr
                reader.ReadBytes(tempByteArr);

                char startChar = System.BitConverter.ToChar(tempByteArr, 0);//获取到包头
                //如果不是包头
                if (!startChar.Equals(Convert.ToChar(2)))
                {
                    return;
                }

                //在读取下2个字节
                await reader.LoadAsync(sizeof(UInt16));
                reader.ReadBytes(tempByteArr);
                Int16 msgType = System.BitConverter.ToInt16(tempByteArr,0);//获取到消息类型
                //await ShowMsg("获取到的消息类型：" + msgType.ToString());

                tempByteArr = new byte[4];
                //在读取下4个字节
                await reader.LoadAsync(sizeof(uint));
                reader.ReadBytes(tempByteArr);
                uint dataSize = System.BitConverter.ToUInt32(tempByteArr, 0);//获取到包长度
                //await ShowMsg("获取到的数据包长度：" + dataSize.ToString());

                uint contentByteLenth = dataSize - 8;//内容字节流长度
                //根据长度读取内容字节流
                uint actualStringLength = await reader.LoadAsync(contentByteLenth);
                if (contentByteLenth != actualStringLength)
                {
                   //内容数据流未发送完成
                   // await ShowMsg("内容数据流未发送完成");
                    return;
                }

                //填充
                tempByteArr = new byte[contentByteLenth];
                reader.ReadBytes(tempByteArr);

                //await ShowMsg(System.Text.UnicodeEncoding.UTF8.GetString(tempByteArr, 0, int.Parse(contentByteLenth.ToString())));
            }
        }*/

        //想要获取连接应该在gamePage中添加这个udp连接吧。
        //这个东西很难静态调用的吧。


        /*http://www.devdiv.com/forum.php?mod=viewthread&tid=134558
 * 
 * 使用udp连接
 * 
 * //创建一个本地udp连接
 * DatagramSocket udpSocket = new DatagramSocker();、
 * //绑定本地端口
 * await updSocket.BindServiceNameAsync("5556");
 * 
 * //创建新的hostname
 * HostName remoteHost = new Host("192.168.1.1");
 * //打开一个至远程主机的连接。
 * await udpSocket.ConnectAsync(remoteHost,"5556");\
 * 
 * //发送数据，创建一个新的datawriter
 * DataWriter udpWriter = new DataWriter(updSocket.OutPutStream);
 * //将数据写入datawriter
 * udpWriter.WriteString("想发送的信息");
 * //发送数据
 * await udpWriter.StoreAsync();
 *
 */
        static DatagramSocket udpSocket;
        static DataWriter udpWriter;

        public async static void ConnectedToServer()
        {
            // 资料来自 http://www.devdiv.com/forum.php?mod=viewthread&tid=134558
            //创建一个本地连接
            udpSocket = new DatagramSocket();
            //绑定本地端口,服务器想要接收消息应该在这个端口。。
            await udpSocket.BindServiceNameAsync("5556");

            //创建新的hostName
            HostName remoteHost = new HostName("192.168.1.1");
            //打开一个至远程主机的连接，后面是远程端口号。。
            await udpSocket.ConnectAsync(remoteHost, "5556");

        }

        public async static void SendMessage(String s)
        {
            //创建一个新的datawriter
            udpWriter = new DataWriter(udpSocket.OutputStream);
            //写入数据
            udpWriter.WriteString(s);
            //发送数据
            await udpWriter.StoreAsync();

        }
        //发现好东西了！！！ http://tech.ddvip.com/2012-12/1354717966186616.html

        //是不是2个socket应该是一样的。。
        public void StartListenServer()
        {
            //直接添加监听就好了吧
            udpSocket.MessageReceived += MessageReceived;
        }

        //这个就能接受消息了，好简单。。。
        void MessageReceived(DatagramSocket socket, DatagramSocketMessageReceivedEventArgs eventArguments)
        {

            var reader = eventArguments.GetDataReader();
            reader.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
            String msg = reader.ReadString(reader.UnconsumedBufferLength);
            //如果写在gamepage 之后可以调用函数处理操作了。。
        }

        //查看是否连接了网络
        public static Boolean IsConnectedToInternet()
        {
            bool connected = false;

            ConnectionProfile cp = NetworkInformation.GetInternetConnectionProfile();

            if (cp != null)
            {
                NetworkConnectivityLevel cl = cp.GetNetworkConnectivityLevel();

                connected = cl == NetworkConnectivityLevel.InternetAccess;
            }

            return connected;
        }
    }
}
