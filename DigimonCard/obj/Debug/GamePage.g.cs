﻿

#pragma checksum "F:\Users\el1ven2013\Documents\DigimonCards\DigimonCard\GamePage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4FB99DCCAC127685064C21507347464E"
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
    partial class GamePage : global::DigimonCard.Common.LayoutAwarePage, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 99 "..\..\GamePage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.sendBtn_click;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 105 "..\..\GamePage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.pop_fold_Btn_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 80 "..\..\GamePage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).PointerPressed += this.hostReadyBtn_pressed;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 81 "..\..\GamePage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).PointerPressed += this.challengerReadyBtn_pressed;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 71 "..\..\GamePage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.GoBack;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


