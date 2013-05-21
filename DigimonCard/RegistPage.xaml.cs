using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
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
    public sealed partial class RegistPage : Page
    {
        public RegistPage()
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            String nname = this.regist_name.Text;
            String npwd = this.regist_pwd.Password;
            String cpwd = this.confirm_pwd.Password;
            String chinese = "^[A-Za-z_0-9\u4e00-\u9fa5]+$";
            if (nname.Equals("") || npwd.Equals(""))
            {
                MessageDialog md = new MessageDialog("请输入用户名/密码哦~亲~", "错误");
                await md.ShowAsync();
            }
            else if (!npwd.Equals(cpwd))
            {
                MessageDialog md = new MessageDialog("两次密码不一致哦~亲~", "错误");
                await md.ShowAsync();
            }
            else if (nname.Length < 6)
            {
                MessageDialog md = new MessageDialog("账号长度请超过6位哦~亲~", "错误");
                await md.ShowAsync();
            }
            else if (npwd.Length < 6)
            {
                MessageDialog md = new MessageDialog("密码长度请超过6位哦~亲~", "错误");
                await md.ShowAsync();
            }
            else if (!Regex.IsMatch(nname, chinese))
            {
                MessageDialog md = new MessageDialog("用户名不能有奇奇怪怪的东西哦~亲~", "错误");
                await md.ShowAsync();
            }
            else
            {
                //异步登陆函数
                Regist(nname, npwd, cpwd);
            }
        }

        //异步登陆函数，用来执行检察用户名密码是否正确
        public async void Regist(String username, String password, String com_password)
        {
            //服务器url地址
            //Uri url = new Uri("http://168.63.151.29:3000/reg/");

            Uri url = new Uri("http://test.twtstudio.com:3000/reg/");
            //post请求键值对
            HttpContent con = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                {"username", username},
                {"password", password},
                {"com_password",com_password}
            });

            try
            {
                //实例化httpClient对象，用来执行异步http请求
                HttpClient client = new HttpClient();

                HttpResponseMessage res = await client.PostAsync(url, con);
                res.EnsureSuccessStatusCode();
                String resBody = await res.Content.ReadAsStringAsync();

                //处理获得的数据
                Handle(resBody);

            }
            catch (HttpRequestException e)
            {
            }
        }

        //处理获得的数据，此处应根据账号密码是否正确来判断跳转或者是重新输入

        private async void Handle(string resBody)
        {
            //this.regist_name.Text = resBody;

            if (resBody.Equals("username has already existed"))
            {
                MessageDialog md = new MessageDialog("账号已经存在了哦~亲~", "错误");
                await md.ShowAsync();
            }
            else if (resBody.Equals("success"))
            {
                MessageDialog md = new MessageDialog("亲~注册成功了哦，恭喜你啊~亲~", "错误");
                await md.ShowAsync();
                Frame.Navigate(typeof(LoginPage));
            }

        }


    }
}
