﻿using TeCHeCerchi.Shared.Helpers;
using TeCHeCerchi.Shared.Interfaces;
using TeCHeCerchi.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TeCHeCerchi.Shared.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {


        private Random random;
        IMessageDialog messages;

        public WelcomeViewModel()
        {
            random = new Random();
            messages = ServiceContainer.Resolve<IMessageDialog>();
            var game = FileCache.ReadGameData();
            GameLoaded = !string.IsNullOrWhiteSpace(game);
            if (GameLoaded)
                Game = JsonConvert.DeserializeObject<Game>(game);
        }



        private bool gameLoaded = false;
        public const string GameLoadedPropertyName = "GameLoaded";

        public bool GameLoaded
        {
            get { return gameLoaded; }
            set
            {
                gameLoaded = value;
                OnPropertyChanged(GameLoadedPropertyName);
            }
        }

        public Game Game { get; set; }

        private ICommand loadGameCommand;

        /// <summary>
        /// Will load the next or current phase
        /// </summary>
        public ICommand LoadGameCommand
        {
            get
            {
                return loadGameCommand ??
                (loadGameCommand = new RelayCommand(() => ExecuteLoadGameCommand(), () => NotBusy));
            }
        }

        private const string GameUrl = "http://www.elea9003.cloud/Quest/game{0}.json";

#if TESTER
      public async Task ExecuteLoadGameCommand(int game = 0)

#else
        public async Task ExecuteLoadGameCommand()
#endif
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
#if TESTER
          var url = string.Format(GameUrl, game);
#else
                var url = string.Format(GameUrl, random.Next(0, 3));
                url = string.Format(GameUrl, 0);
#endif
                string result = "";
//#if __IOS__
//                //var client = new HttpClient(new ModernHttpClient.NativeMessageHandler());
//                //client.Timeout = new TimeSpan(0, 0, 15);
//                //result = await client.GetStringAsync(url);
//#else
//                //var client = new HttpClient();
//                //var request = (HttpWebRequest) WebRequest.Create(url);  
//                //using (WebResponse response = await request.GetResponseAsync())
//                //{
//                //    using (var stream = response.GetResponseStream())
//                //    {
//                //        using (StreamReader sr = new StreamReader(stream))
//                //        {
//                //             result = sr.ReadToEnd();
//                //        }
//                //    }
//                //}

//#endif
                var client = new HttpClient();
                client.Timeout = new TimeSpan(0, 0, 10);
                result = await client.GetStringAsync(url);
                Game = JsonConvert.DeserializeObject<Game>(result);

                await FileCache.SaveGameDataAsync(result);
                GameLoaded = true;
#if !__UNIFIED__
                Xamarin.Insights.Track("GameStarted");
#endif
            }
            catch (Exception ex)
            {
#if !__UNIFIED__
                Xamarin.Insights.Report(ex);
#endif
                messages.SendMessage("Not all who wander are lost...", "But you might be. Looks like you were unable to load TeCHe Cerchi because you dropped the connection. Please check if you have reception and try again.");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
