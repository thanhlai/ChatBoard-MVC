﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace ChatBoard.Hubs
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message, string avatar)
        {
            Clients.All.addNewMessageToPage(name, message, avatar);
        }
    }
}