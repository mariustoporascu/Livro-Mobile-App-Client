﻿using FoodDeliveryApp.Constants;
using FoodDeliveryApp.Models.AuthModels;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Services
{
    public class AuthService : IAuthController
    {
        private HttpClient _httpClient;
        private static string[] endPoint = { "delete","login","create","profile","userlocation","setpassword","changepassword",
        "resetpassword","sendtokenpassword","confirmemail","deletelocation"};

        public AuthService()
        {
            _httpClient = new HttpClient();
        }
        private void TryAddHeaders()
        {
            try
            {
                bool authkey = _httpClient.DefaultRequestHeaders.TryGetValues("authkey", out var val);
                bool authid = _httpClient.DefaultRequestHeaders.TryGetValues("authid", out var val2);
                if (!authid && !authkey)
                {
                    _httpClient.DefaultRequestHeaders.Add("authkey", App.UserInfo.LoginToken);
                    _httpClient.DefaultRequestHeaders.Add("authid", App.UserInfo.Email);
                }
                else
                {
                    _httpClient.DefaultRequestHeaders.Remove("authkey");
                    _httpClient.DefaultRequestHeaders.Remove("authid");
                    _httpClient.DefaultRequestHeaders.Add("authkey", App.UserInfo.LoginToken);
                    _httpClient.DefaultRequestHeaders.Add("authid", App.UserInfo.Email);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }
        public async Task<string> Execute(UserModel userModel, AuthOperations operation)
        {
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/auth/{endPoint[(int)operation]}");

            TryAddHeaders();
            return await sendRequest(userModel, uri);
        }
        private async Task<string> sendRequest(UserModel userModel, Uri uri)
        {

            var json = JsonConvert.SerializeObject(userModel);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync(uri, data);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var respInfo = await httpResponseMessage.Content.ReadAsStringAsync();
                return respInfo;
            }
            return string.Empty;
        }
    }
}
