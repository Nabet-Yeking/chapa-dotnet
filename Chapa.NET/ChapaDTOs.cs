using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChapaNET.ChapaDTOs;

public class ChapaRequest
{
    public double Amount { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string TransactionReference { get; set; }
    public string? PhoneNo { get; set; }
    public string? Currency { get; set; }
    public string? CallbackUrl { get; set; }
    public string? ReturnUrl { get; set; }
    public string? CustomTitle { get; set; }
    public string? CustomDescription { get; set; }
    public string? CustomLogo { get; set; }

    public ChapaRequest(
          double amount
        , string email
        , string firstName, string lastName
        , string tx_ref
        , string? phoneNo = null
        , string? currency = "ETB"
        , string? callback_url = null
        , string? return_url = null
        , string? customTitle = null
        , string? customDescription = null
        , string? customLogo = null)
    {
        Amount = amount;
        Currency = currency;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        PhoneNo = phoneNo;
        TransactionReference = tx_ref;
        CallbackUrl = callback_url;
        ReturnUrl = return_url;
        CustomTitle = customTitle;
        CustomDescription = customDescription;
        CustomLogo = customLogo;
    }
}

public class ChapaResponse
{
    public string? Message { get; set; }
    public string? Status { get; set; }
    public string? CheckoutUrl { get; set; }
    public override string ToString() => JsonConvert.SerializeObject(this);
}

public class Bank
{
    public string ID;
    public string SwiftCode;
    public string Name;
    public int AccLen;
    public int CountryID;

    public Bank(string id, string swift, string name, int accLen, int country_id)
    {
        ID = id;
        SwiftCode = swift;
        Name = name;
        AccLen = accLen;
        CountryID = country_id;
    }

    public override string ToString()
    {
        return
        $@"ID: {ID}
        Name: {Name}
        Swift Code: {SwiftCode}
        AcctLen: {AccLen}
        Country ID: {CountryID}";
    }
}

public class ValidityReport
{
    public bool IsSuccess => status == "success";
    public string message { get; set; }
    public string status { get; set; }
    public Data data { get; set; }

    public class Data
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string currency { get; set; }
        public double amount { get; set; }
        public double charge { get; set; }
        public string mode { get; set; }
        public string method { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public string reference { get; set; }
        public string tx_ref { get; set; }
        public Customization customization { get; set; }
        public object meta { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }

    public class Customization
    {
        public string title { get; set; }
        public string description { get; set; }
        public string logo { get; set; }
    }
}

class BankResponse
{
    public string? message { get; set; }
    public IEnumerable<Bank>? data { get; set; }
}