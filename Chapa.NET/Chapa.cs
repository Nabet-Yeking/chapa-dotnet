using Flurl;
using Flurl.Http;
using ChapaNET.ChapaDTOs;

namespace ChapaNET;
public enum TransactionStatus
{
    Completed, Incomplete, Error
}
public class Chapa
{
    ChapaConfig Config { get; set; }
    public static string GetUniqueRef() => "tx" + DateTime.Now.Ticks;
    public Chapa(string SECRET_KEY)
    {
        if (SECRET_KEY == null)
            throw new Exception("Secret Key can't be null");
        Config = new() { API_SECRET = SECRET_KEY };

        //Client = new RestClient(ChapaConfig.BASE_PATH)
        //    .AddDefaultHeader(ChapaConfig.AUTH_HEADER, "Bearer " + Config.API_SECRET)
        //    .UseNewtonsoftJson();
    }

    public Chapa(ChapaConfig config) : this(config.API_SECRET) { }
    
    public async Task<ChapaResponse> RequestAsync(ChapaRequest request)
    {
        var reqDict = new Dictionary<string, string?>()
        {
            {"email",request.Email},
            {"amount",request.Amount.ToString()},
            {"first_name",request.FirstName},
            {"last_name", request.LastName},
            {"tx_ref",request.TransactionReference},
            {"currency",request.Currency},
        };
        if (request.PhoneNo != null)
            reqDict.Add("phone_number", request.PhoneNo);
        if (request.CallbackUrl != null)
            reqDict.Add("callback_url", request.CallbackUrl);
        if (request.ReturnUrl != null)
            reqDict.Add("return_url", request.ReturnUrl);
        if (request.CustomTitle != null)
            reqDict.Add("customization[title]", request.CustomTitle);
        if (request.CustomDescription != null)
            reqDict.Add("customization[description]", request.CustomDescription);
        if (request.CustomLogo != null)
            reqDict.Add("customization[logo]", request.CustomLogo);

        var response = await "https://api.chapa.co/v1/transaction/initialize"
            .WithHeader(ChapaConfig.AUTH_HEADER, $"Bearer {Config.API_SECRET}")
            .PostJsonAsync(reqDict)
            .ReceiveJson();
    
        return new()
        {
            Status = response.status,
            Message = response.message,
            CheckoutUrl = response.data.checkout_url
        };
    }
    
    public async Task<ValidityReport?> VerifyAsync(string txRef)
    {
        try
        {
            var validityResponse = await $"https://api.chapa.co/v1/transaction/verify/{txRef}"
                        .WithHeader(ChapaConfig.AUTH_HEADER, $"Bearer {Config.API_SECRET}")
                        .GetJsonAsync<ValidityReport>();
            return validityResponse;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<IEnumerable<Bank>> GetBanksAsync()
    {
        var response =
            await "https://api.chapa.co/v1"
                .WithHeader(ChapaConfig.AUTH_HEADER, $"Bearer {Config.API_SECRET}")
                .AppendPathSegment("banks")
                .GetJsonAsync<BankResponse>();

        return response.data!;
    }
}