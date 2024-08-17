using Application.Common.Interfaces;
using Microsoft.Extensions.Options;
using Domain;
using Newtonsoft.Json;

namespace Infrastructure.Services;

public class RecaptchaService : IRecaptchaService
{
    private readonly RecaptchaConfigOptions _configOptions;

    public RecaptchaService
        (
            IOptions<RecaptchaConfigOptions> configuration
        )
    {
        _configOptions = configuration.Value;
    }

    public async Task<bool> ConfirmRecaptchaV3(string token, string? ip)
    {

        var result = false;
        var client = new HttpClient();
        var rq = new HttpRequestMessage(HttpMethod.Post, _configOptions.V3.SiteVerify);
        // Tạo FormUrlEncodedContent với dữ liệu
        var formContent = new FormUrlEncodedContent(new Dictionary<string, string?>()
            {
                ["secret"] = _configOptions.V3.SecretKey,
                ["response"] = token,
                ["remoteip"] = ip,
            });
            rq.Content = formContent;
            // Gửi rq
            var response = await client.SendAsync(rq);
            // Xử lý response
            if (response.IsSuccessStatusCode)
            {
                // Đọc nội dung response
                var content = await response.Content.ReadAsStringAsync();

                var responseDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
                //{
                //    "success": true | false,
                //    "challenge_ts": timestamp,  // timestamp of the challenge load (ISO format yyyy-MM-dd'T'HH:mm:ssZZ)
                //    "hostname": string, // the package name of the app where the reCAPTCHA was solved
                //    "score": double, 
                //    "action": string, 
                //    "error-codes": [...]        // optional
                //}
                result = bool.Parse(responseDictionary["success"].ToString());
            }
        return result;
    }
}