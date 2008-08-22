//C# .NET 2.0 example by Phil Thompson - https://support.zendesk.com/forums/1/entries/6448
//Note - does not support the external_id attribute.
using System.Text
using System.Security.Cryptography 

protected void Page_Load(object sender, EventArgs e)
{
  string sFullName = Profile.GetPropertyValue("UserName").ToString();
  string sEmail    = Profile.GetPropertyValue("EmailAddress").ToString();

  string sToken     = "";
  string sReturnURL = "https://youraccount.zendesk.com/access/remote/";
  string sURL       = "";

  string sMessage = "";
  string sDigest  = "";

  sMessage = sFullName + sEmail + sToken + Request.QueryString.Get("timestamp");
  sDigest = Md5(sMessage);

  sURL = sReturnURL + "?name=" + Server.UrlEncode(sFullName) +
    "&email=" + Server.UrlEncode(sEmail) +
    "&timestamp=" + Request.QueryString.Get("timestamp") +
    "&hash=" + sDigest;

  Response.Redirect(sURL);
}

public string Md5(string strChange)
{
  //Change the syllable into UTF8 code
  byte[] pass = Encoding.UTF8.GetBytes(strChange);

  MD5 md5 = new MD5CryptoServiceProvider();
  md5.ComputeHash(pass);
  string strPassword = ByteArrayToHexString(md5.Hash);
  return strPassword;
}

public static string ByteArrayToHexString(byte[] Bytes)
{
  // important bit, you have to change the byte array to hex string or zenddesk will reject
  StringBuilder Result;
  string HexAlphabet = "0123456789abcdef";

  Result = new StringBuilder();

  foreach (byte B in Bytes)
  {
    Result.Append(HexAlphabet[(int)(B >> 4)]);
    Result.Append(HexAlphabet[(int)(B & 0xF)]);
  }
  
  return Result.ToString();
}
