﻿

#pragma checksum "F:\Users\王永淳\Desktop\DigimonCards\DigimonCard\MediaPlayPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E7E108E7CA159C058816268F8F83E08E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DigimonCard
{
    partial class MediaPlayPage : global::DigimonCard.Common.LayoutAwarePage, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 11 "..\..\MediaPlayPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).KeyUp += this.keyboard_Click;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 114 "..\..\MediaPlayPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.cardsInHand_click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 197 "..\..\MediaPlayPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.esc_fullscreen_click;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 188 "..\..\MediaPlayPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.sendBtn_click;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 194 "..\..\MediaPlayPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.pop_fold_Btn_Click;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 143 "..\..\MediaPlayPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.RangeBase)(target)).ValueChanged += this.timelineSlider_ValueChanged;
                 #line default
                 #line hidden
                #line 143 "..\..\MediaPlayPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).PointerEntered += this.slider_PointerEntered;
                 #line default
                 #line hidden
                #line 143 "..\..\MediaPlayPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).PointerCaptureLost += this.slider_PointerCaptureLost;
                 #line default
                 #line hidden
                break;
            case 7:
                #line 145 "..\..\MediaPlayPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.btnPlay_Click;
                 #line default
                 #line hidden
                break;
            case 8:
                #line 150 "..\..\MediaPlayPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.btnPause_Click;
                 #line default
                 #line hidden
                break;
            case 9:
                #line 155 "..\..\MediaPlayPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.btnStop_Click;
                 #line default
                 #line hidden
                break;
            case 10:
                #line 160 "..\..\MediaPlayPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).PointerPressed += this.btnMute_Click;
                 #line default
                 #line hidden
                break;
            case 11:
                #line 161 "..\..\MediaPlayPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.RangeBase)(target)).ValueChanged += this.volumeChanged_draged;
                 #line default
                 #line hidden
                break;
            case 12:
                #line 163 "..\..\MediaPlayPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.findDocuments;
                 #line default
                 #line hidden
                break;
            case 13:
                #line 168 "..\..\MediaPlayPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.btnFullScreenToggle_Click;
                 #line default
                 #line hidden
                break;
            case 14:
                #line 132 "..\..\MediaPlayPage.xaml"
                ((global::Windows.UI.Xaml.Controls.MediaElement)(target)).MediaOpened += this.videoElement_MediaOpened;
                 #line default
                 #line hidden
                #line 133 "..\..\MediaPlayPage.xaml"
                ((global::Windows.UI.Xaml.Controls.MediaElement)(target)).MediaEnded += this.videoMediaElement_MediaEnded;
                 #line default
                 #line hidden
                #line 134 "..\..\MediaPlayPage.xaml"
                ((global::Windows.UI.Xaml.Controls.MediaElement)(target)).MediaFailed += this.videoMediaElement_MediaFailed;
                 #line default
                 #line hidden
                #line 135 "..\..\MediaPlayPage.xaml"
                ((global::Windows.UI.Xaml.Controls.MediaElement)(target)).CurrentStateChanged += this.videoMediaElement_CurrentStateChanged;
                 #line default
                 #line hidden
                break;
            case 15:
                #line 99 "..\..\MediaPlayPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.back;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


