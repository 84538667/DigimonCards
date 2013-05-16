using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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

        private void register_Name_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            String nname = this.regist_name.Text;
            String npwd = this.regist_pwd.Password;
            String cpwd = this.confirm_pwd.Password;

            if (nname.Equals("") || npwd.Equals(""))
            {
                MessageDialog md = new MessageDialog("请输入用户名/密码", "错误");
                await md.ShowAsync();
            }
            else if (!npwd.Equals(cpwd))
            {
                MessageDialog md = new MessageDialog("两次密码不一致", "错误");
                await md.ShowAsync();
            }
            else
            {
                //异步登陆函数
                Regist(nname, npwd);
            }
        }

        //异步登陆函数，用来执行检察用户名密码是否正确
        public async void Regist(String username, String pwd)
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

        private void Handle(string resBody)
        {
            this.regist_name.Text = resBody;
            /*
             if(账号存在){
               提醒     
              }
             else{
                 注册成功
                Frame.Navigate(typeof(LoginPage));
             }
             */
        }

       
    }
}
