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
 
        
        /// <summary>
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
        }
    }
}
