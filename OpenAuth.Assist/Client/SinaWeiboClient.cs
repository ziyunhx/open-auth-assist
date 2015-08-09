using ImitateLogin;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TNIdea.Common.Data;
using TNIdea.Common.Helper;

namespace OpenAuth.Assist.Client
{
    public class SinaWeiboClient : OpenAuthAssist
    {
		const string AUTH_URL = "https://api.weibo.com/oauth2/authorize";
		const string TOKEN_URL = "https://api.weibo.com/oauth2/access_token";
		const string API_URL = "https://api.weibo.com/2/";

		public string UID
		{
			get;
			set;
		}

		public SinaWeiboClient(string appKey, string appSecret, string callbackUrl, string accessToken = null, string uid = null)
			: base(appKey, appSecret, callbackUrl, accessToken)
		{
			ClientName = "SinaWeibo";
			UID = uid;

			if (!(string.IsNullOrEmpty(accessToken) && string.IsNullOrEmpty(uid)))
			{
				isAccessTokenSet = true;
			}
		}

		protected override string AuthorizationCodeUrl
		{
			get { return AUTH_URL; }
		}

		protected override string AccessTokenUrl
		{
			get { return TOKEN_URL; }
		}

		protected override string BaseApiUrl
		{
			get { return API_URL; }
		}

		public override string GetAuthorizationUrl()
		{
			var ub = new UriBuilder(AuthorizationCodeUrl);
			ub.Query = string.Format("client_id={0}&response_type=code&redirect_uri={1}", ClientId, Uri.EscapeDataString(CallbackUrl));
			return ub.ToString();
		}

		public override void GetAccessTokenByCode(string code)
		{
			var response = HttpPost(TOKEN_URL, new
			{
				client_id = ClientId,
				client_secret = ClientSecret,
				grant_type = "authorization_code",
				code = code,
				redirect_uri = CallbackUrl
			});

			if (response.StatusCode != HttpStatusCode.OK)
				return;

			var result = JObject.Parse(response.Content.ReadAsStringAsync().Result);
			if (result["access_token"] == null)
			{
				return;
			}
			AccessToken = result.Value<string>("access_token");
			UID = result.Value<string>("uid");

			isAccessTokenSet = true;
		}

		public override Task<HttpResponseMessage> HttpGetAsync(string api, Dictionary<string, object> parameters)
		{
			if (IsAuthorized)
			{
				if (parameters == null)
					parameters = new Dictionary<string, object>();

				if (!parameters.ContainsKey("source"))
				{
					parameters["source"] = ClientId;
				}

				if (!parameters.ContainsKey("access_token"))
				{
					parameters["access_token"] = AccessToken;
				}
			}
			return base.HttpGetAsync(api, parameters);
		}


		public override Task<HttpResponseMessage> HttpPostAsync(string api, Dictionary<string, object> parameters)
		{
			if (IsAuthorized)
			{
				if (parameters == null)
					parameters = new Dictionary<string, object>();

				if (!parameters.ContainsKey("source"))
				{
					parameters["source"] = ClientId;
				}

				if (!parameters.ContainsKey("access_token"))
				{
					parameters["access_token"] = AccessToken;
				}
			}

			return base.HttpPostAsync(api, parameters);
		}

        public override bool DoLogin(string userName, string password)
        {
            LoginHelper loginHelper = new LoginHelper();
            LoginResult loginResult = loginHelper.Login(userName, password, LoginSite.Weibo);

            if (loginResult.Result != ResultType.Success)
                return false;

            //rebuild the cookie collection and get the token.
            CookieContainer cookies = HttpHelper.ConvertToCookieContainer(loginResult.Cookies);
            HttpResponse authorize = HttpHelper.HttpRequest(GetAuthorizationUrl(), cookies: cookies);
            Match matchCode = Regex.Match(authorize.Url, "code=(?<code>[^&]+)");

            if (authorize.HttpCode == 200 && matchCode.Success && matchCode.Groups["code"].Success)
            {
                string code = matchCode.Groups["code"].Value;
                GetAccessTokenByCode(code);
                return true;
            }
            else
            {
                string postData = string.Format("action=submit&withOfficalFlag=0&ticket=&isLoginSina=&response_type=token&regCallback=&redirect_uri={0}&client_id={1}&state=&from=&userId={2}&passwd={3}&display=js", Uri.EscapeDataString(string.IsNullOrEmpty(CallbackUrl) ? "" : CallbackUrl), Uri.EscapeDataString(ClientId), Uri.EscapeDataString(userName), Uri.EscapeDataString(password));

                HttpResponse token = HttpHelper.HttpRequest(AUTH_URL, postData, cookies, null, GetAuthorizationUrl());

                if (token.HttpCode == 200)
                {
                    Match matchToken = Regex.Match(token.Content, "\"access_token\":\"(?<access_token>[^\"]+)\".*?uid\":\"(?<uid>[^\"]+)\"");

                    if (matchToken.Success && matchToken.Groups["access_token"].Success)
                    {
                        AccessToken = matchToken.Groups["access_token"].Value;
                        UID = matchToken.Groups["uid"].Value;

                        isAccessTokenSet = true;
                        return true;
                    }
                }
            }
            
            return false;
        }
    }
}
