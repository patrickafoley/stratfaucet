public class Recipient
{
    public string address { get; set; }

    public bool is_sent { get; set; }

    public bool is_error { get; set; }

    public string ip_address { get; set; }
    public string transactionId { get; set; }
    public string captcha { get; set;}
    public string errorText { get; set;}
    public int errorCount{ get; set; }

}
