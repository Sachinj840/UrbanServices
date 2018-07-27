using Acr.UserDialogs;
using ComplaintBookApp.Model.RequestModels;
using ComplaintBookApp.Model.ResponseModels;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ComplaintBookApp.Helpers
{
    public class HttpClientHelper
    {
        #region Data Members
        protected readonly string _endpoint;
        protected readonly string _accesstoken;
        private string stringResult;
        private static bool userChoseOffline;
        private static bool isInternetConnected;
        private static bool isToastRunning;
        private CancellationTokenSource tokenSource;
        private CancellationTokenSource toastCancelationToken;

        #endregion

        #region Constructor
        public HttpClientHelper(string endpoint, string accesstoken)
        {
            _endpoint = endpoint;
            _accesstoken = accesstoken;
            tokenSource = new CancellationTokenSource();
            toastCancelationToken = new CancellationTokenSource();
            //HasGoodConnection();
            isInternetConnected = CrossConnectivity.Current.IsConnected;            
            if (!isInternetConnected)
            {
                SetupToast();
            }
        }

        #endregion

        #region Method
        public async Task<bool> HasGoodConnection()
        {
            //var addressPingping = DependencyService.Get<IAddressPing>();
            //var pingResult = await addressPingping.Ping("8.8.8.8");
            //await UserDialogs.Instance.AlertAsync($"Your ping time = {pingResult.RoundtripTime}, Status = {pingResult.Status}");
            return CrossConnectivity.Current.ConnectionTypes.Any(item => item == ConnectionType.WiFi);
        }

        private void SetupToast()
        {
            toastCancelationToken.Cancel();
            toastCancelationToken = new CancellationTokenSource();
            if (Device.RuntimePlatform == Device.iOS)
            {
                return;
            }

            Task.Run(async () =>
            {
                if (isToastRunning || userChoseOffline)
                {
                    return;
                }

                do
                {
                    if (CrossConnectivity.Current.IsConnected)
                    {
                        return;
                    }
                    isToastRunning = true;
                    var toastMessage = new ToastConfig(" No Internet Connection");
                    toastMessage.Duration = TimeSpan.FromSeconds(2);
                    var toastAction = new ToastAction
                    {
                        Text = "Dismiss",
                        Action = () =>
                        {
                            userChoseOffline = true;
                        }
                    };

                    toastMessage.Action = toastAction;

                    Device.BeginInvokeOnMainThread(() => UserDialogs.Instance.Toast(toastMessage));
                    await Task.Delay(TimeSpan.FromSeconds(5));
                } while (!isInternetConnected && isToastRunning && !userChoseOffline);

                isToastRunning = false;
            }, toastCancelationToken.Token);
        }
        public async Task<T> GetResponse<T>()
        {
            if (!isInternetConnected)
            {                
                SetupToast();
                return default(T);
            }

            Func<Task<T>> runTask = async () =>
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        var hasGoodConnection = await HasGoodConnection();
                        if (!hasGoodConnection)
                        {
                            httpClient.Timeout = TimeSpan.FromSeconds(30);
                        }

                        var endpoint = _endpoint;
                        var accessToken = _accesstoken;
                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        if (accessToken != "")
                        {
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                        }
                        var response = await httpClient.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead);
                        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            ExtendURLRequestModel objextend = new ExtendURLRequestModel();
                            objextend.rtoken = Settings.RenewalTokenSettings;
                            var renewjson = JsonConvert.SerializeObject(objextend);
                            var renewaltoken = await ExtendToken<TokenResponse>(renewjson);
                            Settings.AccessTokenSettings = renewaltoken.accessToken;
                            Settings.RenewalTokenSettings = renewaltoken.renewalToken;
                            httpClient.DefaultRequestHeaders.Clear();
                            httpClient.DefaultRequestHeaders.Accept.Clear();
                            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + renewaltoken.accessToken);
                            response = await httpClient.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead);
                            return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
                        }
                        stringResult = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        return JsonConvert.DeserializeObject<T>(stringResult);
                    }
                }
                catch (System.Net.Http.HttpRequestException requestException)
                {
                }
                catch (Exception exception)
                {
                    if (!App.WasHttpFailed)
                    {
                        App.WasHttpFailed = true;
                        return await GetResponse<T>();
                    }
                }
                finally
                {
                    App.WasHttpFailed = false;
                }

                return default(T);
            };

            return await Task.Run(runTask, tokenSource.Token);
        }
        
        public async Task<T> Login<T>(string jsonObject)
        {
            if (!isInternetConnected)
            {                
                SetupToast();
                return default(T);
            }

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient
                        .PostAsync(_endpoint, new StringContent(jsonObject, Encoding.UTF8, "application/json"))
                        .ConfigureAwait(false);
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        return default(T);
                    }

                    stringResult = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(stringResult);
                }
            }
            catch (System.Net.Http.HttpRequestException requestException)
            {
            }
            catch (Exception e)
            {
                if (!App.WasHttpFailed)
                {
                    App.WasHttpFailed = true;
                    return await Post<T>(jsonObject);
                }
            }
            finally
            {
                App.WasHttpFailed = false;
            }
            return default(T);
        }

        public async Task<T> Post<T>(string jsonobject)
        {
            if (!isInternetConnected)
            {
                SetupToast();
                return default(T);
            }
            using (var httpClient = new HttpClient())
            {
                var endpoint = _endpoint;
                var accessToken = _accesstoken;
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (accessToken != "")
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                }
                var response = await httpClient.PostAsync(endpoint, new StringContent(jsonobject, Encoding.UTF8, "application/json")).ConfigureAwait(false);
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ExtendURLRequestModel objextend = new ExtendURLRequestModel();
                    objextend.rtoken = Settings.RenewalTokenSettings;
                    var renewjson = JsonConvert.SerializeObject(objextend);
                    var renewaltoken = await ExtendToken<TokenResponse>(renewjson);
                    Settings.AccessTokenSettings = renewaltoken.accessToken;
                    Settings.RenewalTokenSettings = renewaltoken.renewalToken;
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + renewaltoken.accessToken);
                    response = await httpClient.PostAsync(endpoint, new StringContent(jsonobject, Encoding.UTF8, "application/json")).ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
                }
                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }            
        }

        private async Task<T> ExtendToken<T>(string jsonobject)
        {
            if (!isInternetConnected)
            {
                SetupToast();
                return default(T);
            }
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.AccessTokenSettings);
                var response = await httpClient.PostAsync(Constants.ApiUrls.Url_JwtAuth_extendtoken, new StringContent(jsonobject, Encoding.UTF8, "application/json")).ConfigureAwait(false);
                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
        }


        public async Task<List<T>> Get<T>()
        {            
            using (var httpClient = new HttpClient())
            {
                var endpoint = _endpoint;
                var accessToken = _accesstoken;
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (accessToken != "")
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                }
                var response = await httpClient.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                // Addedd on 2nd-March-2017 to extend access token
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ExtendURLRequestModel objextend = new ExtendURLRequestModel();
                    objextend.rtoken = Settings.RenewalTokenSettings;

                    var renewjson = JsonConvert.SerializeObject(objextend);
                    var renewaltoken = await ExtendToken<TokenResponse>(renewjson);
                    Settings.AccessTokenSettings = renewaltoken?.accessToken;
                    Settings.RenewalTokenSettings = renewaltoken?.renewalToken;
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + renewaltoken.accessToken);
                    response = await httpClient.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                }
                return JsonConvert.DeserializeObject<List<T>>(response.Content.ReadAsStringAsync().Result);
            }

        }

        public async Task<T> Get<T>(Dictionary<string, string> query)
        {
            if (!isInternetConnected)
            {
                SetupToast();
                return default(T);
            }
            using (var httpClient = new HttpClient())
            {
                var endpoint = _endpoint;
                var accessToken = _accesstoken;
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (accessToken != "")
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                }
                if (query != null)
                {
                    var querystr = CreateQueryString(query);
                    endpoint += "?" + querystr;
                }
                var response = await httpClient.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ExtendURLRequestModel objextend = new ExtendURLRequestModel();
                    objextend.rtoken = Settings.RenewalTokenSettings;
                    var renewjson = JsonConvert.SerializeObject(objextend);
                    var renewaltoken = await ExtendToken<TokenResponse>(renewjson);
                    Settings.AccessTokenSettings = renewaltoken.accessToken;
                    Settings.RenewalTokenSettings = renewaltoken.renewalToken;
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + renewaltoken.accessToken);
                    response = await httpClient.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead);
                    return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
                }
                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
        }

        public async Task<string> Delete<T>()
        {           
            using (var httpClient = new HttpClient())
            {
                var endpoint = _endpoint;
                var accessToken = _accesstoken;
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (accessToken != "")
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                }
                var response = await httpClient.DeleteAsync(endpoint).ConfigureAwait(false);
                // Addedd on 2nd-March-2017 to extend access token
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ExtendURLRequestModel objextend = new ExtendURLRequestModel();
                    objextend.rtoken = Settings.RenewalTokenSettings;

                    var renewjson = JsonConvert.SerializeObject(objextend);
                    var renewaltoken = await ExtendToken<TokenResponse>(renewjson);
                    Settings.AccessTokenSettings = renewaltoken?.accessToken;
                    Settings.RenewalTokenSettings = renewaltoken?.renewalToken;
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + renewaltoken.accessToken);
                    response = await httpClient.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                }
                return JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);
            }
        }


        public async Task<string> GetJsonString<T>(Dictionary<string, string> query)
        {            
            using (var httpClient = new HttpClient())
            {
                var endpoint = _endpoint;
                var accessToken = _accesstoken;
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (accessToken != "")
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                }
                if (query != null)
                {
                    var querystr = CreateQueryString(query);
                    endpoint += "?" + querystr;
                }
                var response = await httpClient.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ExtendURLRequestModel objextend = new ExtendURLRequestModel();
                    objextend.rtoken = Settings.RenewalTokenSettings;
                    var renewjson = JsonConvert.SerializeObject(objextend);
                    var renewaltoken = await ExtendToken<TokenResponse>(renewjson);
                    Settings.AccessTokenSettings = renewaltoken.accessToken;
                    Settings.RenewalTokenSettings = renewaltoken.renewalToken;
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + renewaltoken.accessToken);
                    response = await httpClient.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead);
                    return (response.Content.ReadAsStringAsync().Result);
                }
                return response.Content.ReadAsStringAsync().Result.ToString();
            }
        }

        public static string CreateQueryString(IDictionary<string, string> dict)
        {
            var list = new List<string>();
            foreach (var item in dict)
            {
                list.Add(item.Key + "=" + item.Value.ToString());
            }
            return string.Join("&", list);
        }
        protected HttpClient NewHttpClient()
        {
            return new HttpClient();
        }

    #endregion
    }
}
