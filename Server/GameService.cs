﻿using System;
using Newtonsoft.Json;
using WebSocketSharp;
using WebSocketSharp.Server;
using WebSocketSample.RPC;

namespace WebSocketSample.Server
{
    public class GameService : WebSocketBehavior
    {
        public event Action<string> OnPing;
        public event Action<string, LoginPayload> OnLogin;
        public event Action<string, PlayerUpdatePayload> OnPlayerUpdate;

        protected override void OnOpen()
        {
            Console.WriteLine("WebSocket opened.");
        }

        protected override void OnClose(CloseEventArgs e)
        {
            Console.WriteLine("WebSocket Close.");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            Console.WriteLine("WebSocket Message: " + e.Data);

            var header = JsonConvert.DeserializeObject<Header>(e.Data);
            Console.WriteLine("Header: " + header.Method);

            switch (header.Method)
            {
                case "ping":
                    {
                        OnPing(ID);
                        break;
                    }
                case "login":
                    {
                        var loginPayload = JsonConvert.DeserializeObject<Login>(e.Data).Payload;
                        OnLogin(ID, loginPayload);
                        break;
                    }
                case "player_update":
                    {
                        var playerUpdatePayload = JsonConvert.DeserializeObject<PlayerUpdate>(e.Data).Payload;
                        OnPlayerUpdate(ID, playerUpdatePayload);
                        break;
                    }
            }
        }

        protected override void OnError(ErrorEventArgs e)
        {
            Console.WriteLine("WebSocket Error: " + e);
        }
    }
}