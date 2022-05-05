using GUI.Payments.Abstraction;
using GUI.Payments.Momo.Cryptography;
using GUI.Payments.Momo.Models;
using Newtonsoft.Json;
using Shared.Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GUI.Payments.Momo.Processor
{
    public class MomoWalletProcessor : IPaymentProcessor, IDisposable
    {
        public MomoWalletSecurity Security { get; set; }

        public HttpClient Client { get; set; }

        public MomoWalletPaymentMode Mode { get; set; }

        public MomoWalletProcessor(MomoWalletSecurity security, HttpClient httpClient, MomoWalletPaymentMode mode)
        {
            Security = security;
            Client = httpClient;
            Client.DefaultRequestHeaders.Add("Accept", "application/json");
            Mode = mode;
        }

        public MomoWalletProcessor(MomoWalletSecurity security, HttpClient httpClient) 
            : this(security, httpClient, MomoWalletPaymentMode.Test)
        { }

        public async Task<PaymentResponse> ExecuteAsync(PaymentRequest request)
        {
            if (request.GetType() != typeof(MomoWalletCaptureRequest))
                throw new NotSupportedException();
            var momoRequest = request as MomoWalletCaptureRequest;
            Security.SignRequest(momoRequest);
            var content = JsonContent.Create(momoRequest);
            var endpoint = GetMomoWalletEndpoint(Mode);
            var response = await Client.PostAsync(endpoint, content);
            if (response.Content == null)
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
