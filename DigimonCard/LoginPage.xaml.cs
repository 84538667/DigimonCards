using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace DigimonCard
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        private String pname;
        private String ppwd;
        private String resBody = "1";
        public LoginPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。Parameter
        /// 属性通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private void isClick(object sender, RoutedEventArgs e)
        {

        }

        private void player_regist_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegistPage));
        }

        //登陆Button点击事件
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            pname = this.player_name.Text;
            ppwd = this.player_pwd.Password;

            if (!pname.Equals("") && !ppwd.Equals(""))
            {
                //异步登陆函数
                login(pname, ppwd);
                this.Frame.Navigate(typeof(GameLobbyPage));
            }
            else
            {
                MessageDialog md = new MessageDialog("请输入用户名/密码", "错误");
                await md.ShowAsync();
            }
        }

        //异步登陆函数，用来执行检察用户名密码是否正确
        public async void login(String username, String pwd)
        {
            //服务器url地址
            Uri url = new Uri("http://service.twtstudio.com/phone/android/gpa.php");
            //post请求键值对
            HttpContent con = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                {"username", username},
                {"pwd", pwd}
            });
            try
            {
                //实例化httpClient对象，用来执行异步http请求
                HttpClient client = new HttpClient();

                //以下需要修改根据用户名密码获取返回事件，没写呢
                //此处调用Get方法，
                //传递参数需要使用post方式，现在去学习怎么使用。。。
                //HttpResponseMessage res = await client.GetAsync(url);
                //据说这样可以post 试一下
                //好开心啊，终于可以post了，上午九点左右开始，现在13：11分，
                //一上午过去了 就写了5行代码 嘎嘎！！！！
                //2013.5.12/13.11

                HttpResponseMessage res = await client.PostAsync(url, con);
                res.EnsureSuccessStatusCode();
                resBody = await res.Content.ReadAsStringAsync();

                //处理获得的数据
                Handle(resBody);

            }
            catch (HttpRequestException e)
            {
            }
        }

        //处理获得的数据，此处应根据账号密码是否正确来判断跳转或者是重新输入

        private void Handle(string resBody)
        {
            this.player_name.Text = resBody;
            //Frame.Navigate(typeof(WaitingRoom));
        }
    }

}
