﻿

#pragma checksum "F:\Users\王永淳\Desktop\DigimonCards\DigimonCard\GameLobbyPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "09E4878CC91B761F7F9A177DBF8D2848"
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
    partial class GameLobbyPage : global::DigimonCard.Common.LayoutAwarePage, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 194 "..\..\GameLobbyPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).PointerPressed += this.NewRoomBtn_pressed;
                 #line default
                 #line hidden
                #line 194 "..\..\GameLobbyPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).PointerReleased += this.NewRoomBtn_released;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 195 "..\..\GameLobbyPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).PointerPressed += this.QuickJoinBtn_pressed;
                 #line default
                 #line hidden
                #line 195 "..\..\GameLobbyPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).PointerReleased += this.QuickJoinBtn_released;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 196 "..\..\GameLobbyPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.previousPageBtn_click;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 197 "..\..\GameLobbyPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.nextPageBtn_click;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 198 "..\..\GameLobbyPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.PageChanged;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 211 "..\..\GameLobbyPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.cardsInHand_click;
                 #line default
                 #line hidden
                break;
            case 7:
                #line 266 "..\..\GameLobbyPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).PointerPressed += this.isMute;
                 #line default
                 #line hidden
                break;
            case 8:
                #line 191 "..\..\GameLobbyPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.GoBack;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


