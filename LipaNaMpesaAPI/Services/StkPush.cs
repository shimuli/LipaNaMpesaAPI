using LipaNaMpesaAPI.Dto;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using RestSharp;
using System.Threading.Tasks;

namespace LipaNaMpesaAPI.Services
{
    public class StkPush
    {
        public static bool MpesaStk(string PhoneNumber, string Amount, string Account, string businessCode, string consumerKey, string consumerSecret, string passKey, string description)
        {

            var BaseUrl = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("MpesaUrls")["SANDBOX_STK_BASEURL"];
            string TimeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(businessCode + passKey + TimeStamp));
            string token = accessToken(consumerKey, consumerSecret);
            var client = new RestClient(BaseUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + token);

            var param = new MpesaPaymentDto
            {
                Password = Password,
                BusinessShortCode = businessCode,
                Timestamp = TimeStamp,
                TransactionType = "CustomerPayBillOnline",
                Amount = Amount,
                PartyA = PhoneNumber,
                PartyB = businessCode,
                PhoneNumber = PhoneNumber,
                CallBackURL = "https://mydomain.com/path",
                AccountReference = Account,
                TransactionDesc = description

            };

            request.AddJsonBody(param);

            IRestResponse response = client.Execute(request);

            var mpesaResponse = response.IsSuccessful;
            Console.WriteLine(response.Content);

            return mpesaResponse;
        }

        public static string accessToken(string consumerKey, string consumerSecret)
        {
            var AccessTokenUrl = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("MpesaUrls")["SANDBOX_ACCESS_TOKEN_URL"];

            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create(AccessTokenUrl);

            tokenRequest.PreAuthenticate = true;
            string encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                               .GetBytes(consumerKey + ":" + consumerSecret));
            tokenRequest.Headers.Add("Authorization", "Basic " + encoded);
            tokenRequest.Accept = "application/json";

            HttpWebResponse Tokenresponse = (HttpWebResponse)tokenRequest.GetResponse();

            // Get the stream associated with the response.
            Stream TokenreceiveStream = Tokenresponse.GetResponseStream();

            // Display the status.
            Console.WriteLine(((HttpWebResponse)Tokenresponse).StatusDescription);

            // Open the stream using a StreamReader for easy access.
            StreamReader TokenreadStream = new(TokenreceiveStream, Encoding.UTF8);
            // Read the content.
            string responseFromServer = TokenreadStream.ReadToEnd();

            TokenreadStream.Close();
            Tokenresponse.Close();

            var splashInfo = JsonConvert.DeserializeObject<StkResponse>(responseFromServer);


            return splashInfo.AccessToken;
        }


    }
}
