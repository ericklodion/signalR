using ErickEspinosa.SignalR.ConsoleApp.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;

namespace ErickEspinosa.SignalR.ConsoleApp
{
    class Program
    {
        static HubConnection _connection;
        static string _userName;

        static void Main(string[] args)
        {
            Console.WriteLine("Seja bem vindo!");
            Console.WriteLine("Digite seu nome: ");

            while(String.IsNullOrEmpty(_userName))
                _userName = Console.ReadLine();

            InitializeConnect();

            while (true)
            {
                var message = Console.ReadLine();

                if (!String.IsNullOrEmpty(message))
                    _connection.SendAsync("newMessage", _userName, message);
            }
        }

        private static async void InitializeConnect()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44377/chat")
                .Build();

            _connection.On<string>("newUser", userName =>
            {
                var message = userName == _userName ? $"Você entrou na sala - Connection ID ({_connection.ConnectionId})" : $"{userName} acabou de entrar";
                Console.WriteLine(message);
            });

            _connection.On<string, string>("newMessage", (userName, message) =>
            {
                if(userName != _userName)
                    Console.WriteLine($"{userName}: {message}");
            });

            _connection.On<List<Message>>("previousMessages", (messages) =>
            {
                foreach (var message in messages)
                    Console.WriteLine($"{message.UserName}: {message.Text}");
            });

            await _connection.StartAsync();
            await _connection.SendAsync("newUser", _userName, _connection.ConnectionId);
        }
    }
}