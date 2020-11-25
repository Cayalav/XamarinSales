

namespace sales.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using sales.Model;
    using System.Threading.Tasks;
    using System.Net.Http;
    using Newtonsoft.Json;
    using System.Net.Http.Headers;
    using Plugin.Connectivity;
    using sales.Helpers;
    using sales.Model.util;

    public class ApiService
    {


        public async Task<Response> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Languages.TurnOnInternet
                };
            }

            var isReachable = await CrossConnectivity.Current.IsRemoteReachable("google.com");
            if (!isReachable)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Languages.NoInternet,
                };
            }

            return new Response
            {
                IsSuccess = true,
            };
        }

        public async Task<TokenResponse> GetToken(string urlBase, string username, string password)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);


                User user = new User
                {
                    username = username,
                    password = password
                };

                var request = JsonConvert.SerializeObject(user);

                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("/api/login",content);
                var resultJSON = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TokenResponse>(
                    resultJSON);
                return result;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Response> GetList<T>(string urlBase, string prefix, string controller) {

            try
            {

                var authData = string.Format("{0}:{1}", "andresgrana2", "123");
                var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
                client.BaseAddress = new Uri(urlBase);
                var url = $"{prefix}{controller}";
                var response = await client.GetAsync(url);
                var answer = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode) {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,

                    };
                }

                var list = JsonConvert.DeserializeObject<List<T>>(answer);

                return new Response
                {
                    IsSuccess = true,
                    Result = list,

                };
            }
            catch (Exception e)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = e.Message,

                };
            
            }
        
        }

        public async Task<Response> GetList<T>(string urlBase, string prefix, string controller, string accessTocken)
        {

            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessTocken);
                client.BaseAddress = new Uri(urlBase);
                var url = $"{prefix}{controller}";
                var response = await client.GetAsync(url);
                var answer = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,

                    };
                }

                var list = JsonConvert.DeserializeObject<List<T>>(answer);

                return new Response
                {
                    IsSuccess = true,
                    Result = list,

                };
            }
            catch (Exception e)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = e.Message,

                };

            }

        }


        public async Task<Response> GetList<T>(string urlBase, string prefix, string controller, int id, string accessTocken)
        {

            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessTocken);
                client.BaseAddress = new Uri(urlBase);
                var url = $"{prefix}{controller}/{id}";
                var response = await client.GetAsync(url);
                var answer = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,

                    };
                }

                var list = JsonConvert.DeserializeObject<List<T>>(answer);

                return new Response
                {
                    IsSuccess = true,
                    Result = list,

                };
            }
            catch (Exception e)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = e.Message,

                };

            }

        }

        public async Task<Response> Post<T>(string urlBase, string prefix, string controller, T model)
        {

            try
            {

                var request = JsonConvert.SerializeObject(model);

                var content = new StringContent(request, Encoding.UTF8,"application/json");

            

                var client = new HttpClient();
              
                client.BaseAddress = new Uri(urlBase);
                var url = $"{prefix}{controller}";



                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,

                    };
                }

                var obj = JsonConvert.DeserializeObject<T>(answer);

                return new Response
                {
                    IsSuccess = true,
                    Result = obj,

                };
            }
            catch (Exception e)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = e.Message,

                };

            }

        }

        public async Task<Response> Post<T>(string urlBase, string prefix, string controller, T model, string accessTocken)
        {

            try
            {

                var request = JsonConvert.SerializeObject(model);

                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessTocken);

                client.BaseAddress = new Uri(urlBase);
                var url = $"{prefix}{controller}";



                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,

                    };
                }

                var obj = JsonConvert.DeserializeObject<T>(answer);

                return new Response
                {
                    IsSuccess = true,
                    Result = obj,

                };
            }
            catch (Exception e)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = e.Message,

                };

            }

        }

        public async Task<Response> Delete(string urlBase, string prefix, string controller, long id)
        {

            try
            {

            
            

                var client = new HttpClient();
            
                client.BaseAddress = new Uri(urlBase);
                var url = $"{prefix}{controller}/{id}";
                var response = await client.DeleteAsync(url);
                var answer = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,

                    };
                }

               

                return new Response
                {
                    IsSuccess = true,

                };
            }
            catch (Exception e)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = e.Message,

                };

            }

        }

        public async Task<Response> Delete(string urlBase, string prefix, string controller, long id, string accessTocken)
        {

            try
            {

            

                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessTocken);
                client.BaseAddress = new Uri(urlBase);
                var url = $"{prefix}{controller}/{id}";
                var response = await client.DeleteAsync(url);
                var answer = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,

                    };
                }



                return new Response
                {
                    IsSuccess = true,

                };
            }
            catch (Exception e)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = e.Message,

                };

            }

        }


        public async Task<Response> Put<T>(string urlBase, string prefix, string controller, T model, long id)
        {

            try
            {

                var request = JsonConvert.SerializeObject(model);

                var content = new StringContent(request, Encoding.UTF8, "application/json");

                var authData = string.Format("{0}:{1}", "andresgrana2", "123");
                var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
                client.BaseAddress = new Uri(urlBase);
                var url = $"{prefix}{controller}/{id}";



                var response = await client.PutAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,

                    };
                }

                var obj = JsonConvert.DeserializeObject<T>(answer);

                return new Response
                {
                    IsSuccess = true,
                    Result = obj,

                };
            }
            catch (Exception e)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = e.Message,

                };

            }

        }

        public async Task<Response> Put<T>(string urlBase, string prefix, string controller, T model, long id, string accessTocken)
        {

            try
            {

                var request = JsonConvert.SerializeObject(model);

                var content = new StringContent(request, Encoding.UTF8, "application/json");

                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessTocken);
                client.BaseAddress = new Uri(urlBase);
                var url = $"{prefix}{controller}/{id}";



                var response = await client.PutAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,

                    };
                }

                var obj = JsonConvert.DeserializeObject<T>(answer);

                return new Response
                {
                    IsSuccess = true,
                    Result = obj,

                };
            }
            catch (Exception e)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = e.Message,

                };

            }

        }

        public async Task<Response> GetUser(string urlBase, string prefix, string controller, string email,  string accessToken)
        {
            try
            {
                var getUserRequest = new GetUserRequest
                {
                    email = email,
                };

                var request = JsonConvert.SerializeObject(getUserRequest);
                var content = new StringContent(request, Encoding.UTF8, "application/json");

                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var url = $"{prefix}{controller}";
                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var user = JsonConvert.DeserializeObject<UserRequest>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = user,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<FacebookResponse> GetFacebook(string accessToken)
        {
            var requestUrl = "https://graph.facebook.com/v2.8/me/?fields=name," +
                "picture.width(999),cover,age_range,devices,email,gender," +
                "is_verified,birthday,languages,work,website,religion," +
                "location,locale,link,first_name,last_name," +
                "hometown&access_token=" + accessToken;
            var httpClient = new HttpClient();
            var userJson = await httpClient.GetStringAsync(requestUrl);
            var facebookResponse =
                JsonConvert.DeserializeObject<FacebookResponse>(userJson);
            return facebookResponse;
        }



        public async Task<TokenResponse> LoginFacebook(string urlBase, string servicePrefix, string controller, FacebookResponse profile)
        {
            try
            {
                var request = JsonConvert.SerializeObject(profile);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var tokenResponse = await GetToken(
                    urlBase,
                    profile.Id,
                    profile.Id);
                return tokenResponse;
            }
            catch
            {
                return null;
            }
        }

    }

}
