namespace BlazorAzureADB2CApp1.Controllers;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("webhook")]
public class WebhookController : ControllerBase
{
    [HttpPost]
    public IActionResult ReceiveWebhook([FromBody] dynamic request, [FromHeader(Name = "x-line-signature")] string signature)
    {
        // LINE Messaging APIの署名を検証
        string channelSecret = "YOUR_CHANNEL_SECRET"; // LINEチャネルのシークレットを設定
        var body = request.ToString();

        if (!ValidateSignature(channelSecret, body, signature))
        {
            return Unauthorized(); // 401 Unauthorizedを返す
        }

        // Webhookのリクエスト内容をログに記録
        Console.WriteLine("Valid Webhook received: " + body);

        // 必要な処理をここに追加
        // 例: イベント処理やメッセージ送信など

        return Ok(); // 200 OKを返す
    }

    private bool ValidateSignature(string channelSecret, string body, string signature)
    {
        var encoding = new System.Text.UTF8Encoding();
        var key = encoding.GetBytes(channelSecret);

        using (var hmac = new System.Security.Cryptography.HMACSHA256(key))
        {
            var hash = hmac.ComputeHash(encoding.GetBytes(body));
            var computedSignature = Convert.ToBase64String(hash);
            return computedSignature == signature;
        }
    }
}
