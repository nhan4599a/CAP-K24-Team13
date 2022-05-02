using GUI.Payments.Momo.Cryptography;
using GUI.Payments.Momo.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GUI.Payments.Momo.Processor
{
    public class MomoWalletProcessor : IDisposable
    {
        public MomoWalletSecurity Security { get; set; }

        public HttpClient Client { get; set; }

        public MomoWalletPaymentMode Mode { get; set; }

        public MomoWalletProcessor(MomoWalletSecurity security, HttpClient httpClient, MomoWalletPaymentMode mode)
        {
            Security = security;
            Client = httpClient;
            Mode = mode;
        }

        public MomoWalletProcessor(MomoWalletSecurity security, HttpClient httpClient) 
            : this(security, httpClient, MomoWalletPaymentMode.Test)
        { }

        public async Task<MomoWalletCaptureResponse> Execute(MomoWalletCaptureRequest request)
        {
            var content = JsonContent.Create(request);
            var endpoint = GetMomoWalletEndpoint(Mode);
            var response = await Client.PostAsync(endpoint, content);
            if (response.Content == null || !response.IsSuccessStatusCode)
                throw new Exception("Something went wrong!");
            var message = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<MomoWalletCaptureResponse>(message);
        }

        public static string GetMomoWalletEndpoint(MomoWalletPaymentMode mode)
        {
            return "https://" + (mode == MomoWalletPaymentMode.Test ? "test-payment.momo.vn" : "payment.momo.vn") + "/v2/gateway/api/create";
        }

        public void Dispose()
        {
            Client.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
