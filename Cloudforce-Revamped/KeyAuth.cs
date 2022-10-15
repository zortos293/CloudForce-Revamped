using System;
using System.Security.Cryptography;
using System.Collections.Specialized;
using System.Text;
using System.Net;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading;
using System.Collections.Generic;

namespace KeyAuth
{
    public class api
    {
        public string name, ownerid, secret, version;
        /// <summary>
        /// Set up your application credentials in order to use keyauth
        /// </summary>
        /// <param name="name">Application Name</param>
        /// <param name="ownerid">Your OwnerID, can be found in your account settings.</param>
        /// <param name="secret">Application Secret</param>
        /// <param name="version">Application Version, if version doesnt match it will open the download link you set up in your application settings and close the app, if empty the app will close</param>
        public api(string name, string ownerid, string secret, string version)
        {

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(ownerid) || string.IsNullOrWhiteSpace(secret) || string.IsNullOrWhiteSpace(version))
            {
                error("Application not setup correctly. Please watch video link found in Program.cs");
                Environment.Exit(0);
            }

            this.name = name;

            this.ownerid = ownerid;

            this.secret = secret;

            this.version = version;
        }

        #region structures
        [DataContract]
        private class response_structure
        {
            [DataMember]
            public bool success { get; set; }

            [DataMember]
            public string sessionid { get; set; }

            [DataMember]
            public string contents { get; set; }

            [DataMember]
            public string response { get; set; }

            [DataMember]
            public string message { get; set; }

            [DataMember]
            public string download { get; set; }

            [DataMember(IsRequired = false, EmitDefaultValue = false)]
            public user_data_structure info { get; set; }

            [DataMember(IsRequired = false, EmitDefaultValue = false)]
            public app_data_structure appinfo { get; set; }

            [DataMember]
            public List<msg> messages { get; set; }

            [DataMember]
            public List<users> users { get; set; }
        }

        public class msg
        {
            public string message { get; set; }
            public string author { get; set; }
            public string timestamp { get; set; }
        }

        public class users
        {
            public string credential { get; set; }
        }

        [DataContract]
        private class user_data_structure
        {
            [DataMember]
            public string username { get; set; }

            [DataMember]
            public string ip { get; set; }
            [DataMember]
            public string hwid { get; set; }
            [DataMember]
            public string createdate { get; set; }
            [DataMember]
            public string lastlogin { get; set; }
            [DataMember]
            public List<Data> subscriptions { get; set; } // array of subscriptions (basically multiple user ranks for user with individual expiry dates
        }

        [DataContract]
        private class app_data_structure
        {
            [DataMember]
            public string numUsers { get; set; }
            [DataMember]
            public string numOnlineUsers { get; set; }
            [DataMember]
            public string numKeys { get; set; }
            [DataMember]
            public string version { get; set; }
            [DataMember]
            public string customerPanelLink { get; set; }
            [DataMember]
            public string downloadLink { get; set; }
        }
        #endregion
        private string sessionid, enckey;
        bool initzalized;
        /// <summary>
        /// Initializes the connection with keyauth in order to use any of the functions
        /// </summary>
        public void init()
        {
            enckey = encryption.sha256(encryption.iv_key());
            var init_iv = encryption.sha256(encryption.iv_key());
            var values_to_upload = new NameValueCollection
            {
                ["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("init")),
                ["ver"] = encryption.encrypt(version, secret, init_iv),
                ["hash"] = checksum(Process.GetCurrentProcess().MainModule.FileName),
                ["enckey"] = encryption.encrypt(enckey, secret, init_iv),
                ["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(name)),
                ["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(ownerid)),
                ["init_iv"] = init_iv
            };

            var response = req(values_to_upload);

            if (response == "KeyAuth_Invalid")
            {
                error("Application not found");
                Environment.Exit(0);
            }

            response = encryption.decrypt(response, secret, init_iv);

            var json = response_decoder.string_to_generic<response_structure>(response);

            load_response_struct(json);
            if (json.success)
            {
                load_app_data(json.appinfo);
                sessionid = json.sessionid;
                initzalized = true;
            }
            else if (json.message == "invalidver")
            {
                app_data.downloadLink = json.download;
            }

        }
        /// <summary>
        /// Registers the user using a license and gives the user a subscription that matches their license level
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="pass">Password</param>
        /// <param name="key">License</param>
        public void register(string username, string pass, string key)
        {
            if (!initzalized)
            {
                error("Please initzalize first");
                Environment.Exit(0);
            }

            string hwid = WindowsIdentity.GetCurrent().User.Value;

            var init_iv = encryption.sha256(encryption.iv_key());

            var values_to_upload = new NameValueCollection
            {
                ["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("register")),
                ["username"] = encryption.encrypt(username, enckey, init_iv),
                ["pass"] = encryption.encrypt(pass, enckey, init_iv),
                ["key"] = encryption.encrypt(key, enckey, init_iv),
                ["hwid"] = encryption.encrypt(hwid, enckey, init_iv),
                ["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(sessionid)),
                ["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(name)),
                ["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(ownerid)),
                ["init_iv"] = init_iv
            };

            var response = req(values_to_upload);

            response = encryption.decrypt(response, enckey, init_iv);
            var json = response_decoder.string_to_generic<response_structure>(response);
            load_response_struct(json);
            if (json.success)
                load_user_data(json.info);
        }
        /// <summary>
        /// Authenticates the user using their username and password
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="pass">Password</param>
        public void login(string username, string pass)
        {
            if (!initzalized)
            {
                error("Please initzalize first");
                Environment.Exit(0);
            }

            string hwid = WindowsIdentity.GetCurrent().User.Value;

            var init_iv = encryption.sha256(encryption.iv_key());

            var values_to_upload = new NameValueCollection
            {
                ["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("login")),
                ["username"] = encryption.encrypt(username, enckey, init_iv),
                ["pass"] = encryption.encrypt(pass, enckey, init_iv),
                ["hwid"] = encryption.encrypt(hwid, enckey, init_iv),
                ["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(sessionid)),
                ["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(name)),
                ["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(ownerid)),
                ["init_iv"] = init_iv
            };

            var response = req(values_to_upload);

            response = encryption.decrypt(response, enckey, init_iv);
            var json = response_decoder.string_to_generic<response_structure>(response);
            load_response_struct(json);
            if (json.success)
                load_user_data(json.info);
        }

        public void web_login()
        {
            if (!initzalized)
            {
                error("Please initzalize first");
                Environment.Exit(0);
            }

            string hwid = WindowsIdentity.GetCurrent().User.Value;

            string datastore, datastore2, outputten;

            HttpListener listener = new HttpListener();

            outputten = "handshake";
            outputten = "http://localhost:1337/" + outputten + "/";

            listener.Prefixes.Add(outputten);

            listener.Start();

            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse responsepp = context.Response;

            responsepp.AddHeader("Access-Control-Allow-Methods", "GET, POST");
            responsepp.AddHeader("Access-Control-Allow-Origin", "*");
            responsepp.AddHeader("Via", "hugzho's big brain");
            responsepp.AddHeader("Location", "your kernel ;)");
            responsepp.AddHeader("Retry-After", "never lmao");
            responsepp.Headers.Add("Server", "\r\n\r\n");

            listener.AuthenticationSchemes = AuthenticationSchemes.Negotiate;
            listener.UnsafeConnectionNtlmAuthentication = true;
            listener.IgnoreWriteExceptions = true;

            string data = request.RawUrl;

            datastore2 = data.Replace("/handshake?user=", "");
            datastore2 = datastore2.Replace("&token=", " ");

            datastore = datastore2;

            string user = datastore.Split()[0];
            string token = datastore.Split(' ')[1];

            var values_to_upload = new NameValueCollection
            {
                ["type"] = "login",
                ["username"] = user,
                ["token"] = token,
                ["hwid"] = hwid,
                ["sessionid"] = sessionid,
                ["name"] = name,
                ["ownerid"] = ownerid
            };

            var response = req_unenc(values_to_upload);

            var json = response_decoder.string_to_generic<response_structure>(response);
            load_response_struct(json);

            bool success = true;

            if (json.success)
            {
                load_user_data(json.info);

                responsepp.StatusCode = 420;
                responsepp.StatusDescription = "SHEESH";
            }
            else
            {
                Console.WriteLine(json.message);
                responsepp.StatusCode = (int)HttpStatusCode.OK;
                responsepp.StatusDescription = json.message;
                success = false;
            }

            byte[] buffer = Encoding.UTF8.GetBytes("Whats up?");

            responsepp.ContentLength64 = buffer.Length;
            System.IO.Stream output = responsepp.OutputStream;
            output.Write(buffer, 0, buffer.Length);

            Thread.Sleep(1250);

            listener.Stop();

            if (!success)
                Environment.Exit(0);

        }

        /// <summary>
        /// Use Buttons from KeyAuth Customer Panel
        /// </summary>
        /// <param name="button">Button Name</param>

        public void button(string button)
        {
            if (!initzalized)
            {
                error("Please initzalize first");
                Environment.Exit(0);
            }

            HttpListener listener = new HttpListener();

            string output;

            output = button;
            output = "http://localhost:1337/" + output + "/";

            listener.Prefixes.Add(output);

            listener.Start();

            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse responsepp = context.Response;

            responsepp.AddHeader("Access-Control-Allow-Methods", "GET, POST");
            responsepp.AddHeader("Access-Control-Allow-Origin", "*");
            responsepp.AddHeader("Via", "hugzho's big brain");
            responsepp.AddHeader("Location", "your kernel ;)");
            responsepp.AddHeader("Retry-After", "never lmao");
            responsepp.Headers.Add("Server", "\r\n\r\n");

            responsepp.StatusCode = 420;
            responsepp.StatusDescription = "SHEESH";

            listener.AuthenticationSchemes = AuthenticationSchemes.Negotiate;
            listener.UnsafeConnectionNtlmAuthentication = true;
            listener.IgnoreWriteExceptions = true;

            listener.Stop();
        }

        /// <summary>
        /// Gives the user a subscription that has the same level as the key
        /// </summary>
        /// <param name="username">Username of the user thats going to get upgraded</param>
        /// <param name="key">License with the same level as the subscription you want to give the user</param>
        public void upgrade(string username, string key)
        {
            if (!initzalized)
            {
                error("Please initzalize first");
                Environment.Exit(0);
            }

            string hwid = WindowsIdentity.GetCurrent().User.Value;

            var init_iv = encryption.sha256(encryption.iv_key());

            var values_to_upload = new NameValueCollection
            {
                ["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("upgrade")),
                ["username"] = encryption.encrypt(username, enckey, init_iv),
                ["key"] = encryption.encrypt(key, enckey, init_iv),
                ["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(sessionid)),
                ["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(name)),
                ["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(ownerid)),
                ["init_iv"] = init_iv
            };

            var response = req(values_to_upload);

            response = encryption.decrypt(response, enckey, init_iv);
            var json = response_decoder.string_to_generic<response_structure>(response);
            json.success = false;
            load_response_struct(json);
        }

        /// <summary>
        /// Authenticate without using usernames and passwords
        /// </summary>
        /// <param name="key">Licence used to login with</param>
        public void license(string key)
        {
            if (!initzalized)
            {
                error("Please initzalize first");
                Environment.Exit(0);
            }

            string hwid = WindowsIdentity.GetCurrent().User.Value;

            var init_iv = encryption.sha256(encryption.iv_key());

            var values_to_upload = new NameValueCollection
            {
                ["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("license")),
                ["key"] = encryption.encrypt(key, enckey, init_iv),
                ["hwid"] = encryption.encrypt(hwid, enckey, init_iv),
                ["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(sessionid)),
                ["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(name)),
                ["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(ownerid)),
                ["init_iv"] = init_iv
            };

            var response = req(values_to_upload);

            response = encryption.decrypt(response, enckey, init_iv);

            var json = response_decoder.string_to_generic<response_structure>(response);
            load_response_struct(json);
            if (json.success)
                load_user_data(json.info);
        }
        /// <summary>
        /// checks if the current session is validated or not
        /// </summary>
        public void check()
        {
            if (!initzalized)
            {
                error("Please initzalize first");
                Environment.Exit(0);
            }
            var init_iv = encryption.sha256(encryption.iv_key());

            var values_to_upload = new NameValueCollection
            {
                ["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("check")),
                ["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(sessionid)),
                ["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(name)),
                ["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(ownerid)),
                ["init_iv"] = init_iv
            };

            var response = req(values_to_upload);

            response = encryption.decrypt(response, enckey, init_iv);
            var json = response_decoder.string_to_generic<response_structure>(response);
            load_response_struct(json);
        }
        /// <summary>
        /// Change the data of an existing user variable, *User must be logged in*
        /// </summary>
        /// <param name="var">User variable name</param>
        /// <param name="data">The content of the variable</param>
        public void setvar(string var, string data)
        {
            if (!initzalized)
            {
                error("Please initzalize first");
                Environment.Exit(0);
            }

            var init_iv = encryption.sha256(encryption.iv_key());

            var values_to_upload = new NameValueCollection
            {
                ["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("setvar")),
                ["var"] = encryption.encrypt(var, enckey, init_iv),
                ["data"] = encryption.encrypt(data, enckey, init_iv),
                ["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(sessionid)),
                ["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(name)),
                ["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(ownerid)),
                ["init_iv"] = init_iv
            };

            var response = req(values_to_upload);

            response = encryption.decrypt(response, enckey, init_iv);
            var json = response_decoder.string_to_generic<response_structure>(response);
            load_response_struct(json);
        }
        /// <summary>
        /// Gets the an existing user variable
        /// </summary>
        /// <param name="var">User Variable Name</param>
        /// <returns>The content of the user variable</returns>
        public string getvar(string var)
        {

            if (!initzalized)
            {
                error("Please initzalize first");
                Environment.Exit(0);
            }

            var init_iv = encryption.sha256(encryption.iv_key());

            var values_to_upload = new NameValueCollection
            {
                ["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("getvar")),
                ["var"] = encryption.encrypt(var, enckey, init_iv),
                ["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(sessionid)),
                ["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(name)),
                ["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(ownerid)),
                ["init_iv"] = init_iv
            };

            var response = req(values_to_upload);

            response = encryption.decrypt(response, enckey, init_iv);
            var json = response_decoder.string_to_generic<response_structure>(response);
            load_response_struct(json);
            if (json.success)
                return json.response;
            return null;
        }
        /// <summary>
        /// Bans the current logged in user
        /// </summary>
        public void ban()
        {
            if (!initzalized)
            {
                error("Please initzalize first");
                Environment.Exit(0);
            }

            var init_iv = encryption.sha256(encryption.iv_key());

            var values_to_upload = new NameValueCollection
            {
                ["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("ban")),
                ["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(sessionid)),
                ["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(name)),
                ["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(ownerid)),
                ["init_iv"] = init_iv
            };

            var response = req(values_to_upload);

            response = encryption.decrypt(response, enckey, init_iv);
            var json = response_decoder.string_to_generic<response_structure>(response);
            load_response_struct(json);
        }
        /// <summary>
        /// Gets an existing global variable
        /// </summary>
        /// <param name="varid">Variable ID</param>
        /// <returns>The content of the variable</returns>
        public string var(string varid)
        {
            if (!initzalized)
            {
                error("Please initzalize first");
                Environment.Exit(0);
            }

            string hwid = WindowsIdentity.GetCurrent().User.Value;

            var init_iv = encryption.sha256(encryption.iv_key());

            var values_to_upload = new NameValueCollection
            {
                ["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("var")),
                ["varid"] = encryption.encrypt(varid, enckey, init_iv),
                ["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(sessionid)),
                ["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(name)),
                ["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(ownerid)),
                ["init_iv"] = init_iv
            };

            var response = req(values_to_upload);

            response = encryption.decrypt(response, enckey, init_iv);
            var json = response_decoder.string_to_generic<response_structure>(response);
            load_response_struct(json);
            if (json.success)
                return json.message;
            return null;
        }
        /// <summary>
        /// Fetch usernames of online users
        /// </summary>
        /// <returns>ArrayList of usernames</returns>
        public List<users> fetchOnline()
        {
            if (!initzalized)
            {
                error("Please initzalize first");
                Environment.Exit(0);
            }

            var init_iv = encryption.sha256(encryption.iv_key());

            var values_to_upload = new NameValueCollection
            {
                ["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("fetchOnline")),
                ["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(sessionid)),
                ["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(name)),
                ["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(ownerid)),
                ["init_iv"] = init_iv
            };

            var response = req(values_to_upload);

            response = encryption.decrypt(response, enckey, init_iv);
            var json = response_decoder.string_to_generic<response_structure>(response);
            load_response_struct(json);

            if (json.success)
                return json.users;
            return null;
        }
        /// <summary>
        /// Gets the last 20 sent messages of that channel
        /// </summary>
        /// <param name="channelname">The channel name</param>
        /// <returns>the last 20 sent messages of that channel</returns>
        public List<msg> chatget(string channelname)
        {
            if (!initzalized)
            {
                error("Please initzalize first");
                Environment.Exit(0);
            }

            var init_iv = encryption.sha256(encryption.iv_key());

            var values_to_upload = new NameValueCollection
            {
                ["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("chatget")),
                ["channel"] = encryption.encrypt(channelname, enckey, init_iv),
                ["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(sessionid)),
                ["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(name)),
                ["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(ownerid)),
                ["init_iv"] = init_iv
            };

            var response = req(values_to_upload);

            response = encryption.decrypt(response, enckey, init_iv);

            var json = response_decoder.string_to_generic<response_structure>(response);
            load_response_struct(json);
            if (json.success)
            {
                    return json.messages;
            }
            return null;
        }
        /// <summary>
        /// Sends a message to the given channel name
        /// </summary>
        /// <param name="msg">Message</param>
        /// <param name="channelname">Channel Name</param>
        /// <returns>If the message was sent successfully, it returns true if not false</returns>
        public bool chatsend(string msg, string channelname)
        {
            if (!initzalized)
            {
                error("Please initzalize first");
                Environment.Exit(0);
            }

            var init_iv = encryption.sha256(encryption.iv_key());

            var values_to_upload = new NameValueCollection
            {
                ["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("chatsend")),
                ["message"] = encryption.encrypt(msg, enckey, init_iv),
                ["channel"] = encryption.encrypt(channelname, enckey, init_iv),
                ["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(sessionid)),
                ["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(name)),
                ["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(ownerid)),
                ["init_iv"] = init_iv
            };

            var response = req(values_to_upload);

            response = encryption.decrypt(response, enckey, init_iv);
            var json = response_decoder.string_to_generic<response_structure>(response);
            load_response_struct(json);
            if (json.success)
                return true;
            return false;
        }
        /// <summary>
        /// Checks if the current ip address/hwid is blacklisted
        /// </summary>
        /// <returns>If found blacklisted returns true if not false</returns>
        public bool checkblack()
        {
            if (!initzalized)
            {
                error("Please initzalize first");
                Environment.Exit(0);
            }
            string hwid = WindowsIdentity.GetCurrent().User.Value;

            var init_iv = encryption.sha256(encryption.iv_key());

            var values_to_upload = new NameValueCollection
            {
                ["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("checkblacklist")),
                ["hwid"] = encryption.encrypt(hwid, enckey, init_iv),
                ["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(sessionid)),
                ["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(name)),
                ["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(ownerid)),
                ["init_iv"] = init_iv
            };

            var response = req(values_to_upload);

            response = encryption.decrypt(response, enckey, init_iv);
            var json = response_decoder.string_to_generic<response_structure>(response);
            load_response_struct(json);
            if (json.success)
                return true;
            return false;
        }
        /// <summary>
        /// Sends a request to a webhook that you've added in the dashboard in a safe way without it being showed for example a http debugger
        /// </summary>
        /// <param name="webid">Webhook ID</param>
        /// <param name="param">Parameters</param>
        /// <param name="body">Body of the request, empty by default</param>
        /// <param name="conttype">Content type, empty by default</param>
        /// <returns>the webhook's response</returns>
        public string webhook(string webid, string param, string body = "", string conttype = "")
        {
            if (!initzalized)
            {
                error("Please initzalize first");
                Environment.Exit(0);
                return null;
            }

            var init_iv = encryption.sha256(encryption.iv_key());

            var values_to_upload = new NameValueCollection
            {
                ["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("webhook")),
                ["webid"] = encryption.encrypt(webid, enckey, init_iv),
                ["params"] = encryption.encrypt(param, enckey, init_iv),
                ["body"] = encryption.encrypt(body, enckey, init_iv),
                ["conttype"] = encryption.encrypt(conttype, enckey, init_iv),
                ["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(sessionid)),
                ["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(name)),
                ["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(ownerid)),
                ["init_iv"] = init_iv
            };

            var response = req(values_to_upload);

            response = encryption.decrypt(response, enckey, init_iv);

            var json = response_decoder.string_to_generic<response_structure>(response);
            load_response_struct(json);
            if (json.success)
                return json.response;
            return null;
        }
        /// <summary>
        /// KeyAuth acts as proxy and downlods the file in a secure way
        /// </summary>
        /// <param name="fileid">File ID</param>
        /// <returns>The bytes of the download file</returns>
        public byte[] download(string fileid)
        {
            if (!initzalized)
            {
                error("Please initzalize first. File is empty since no request could be made.");
                Environment.Exit(0);
            }

            var init_iv = encryption.sha256(encryption.iv_key());

            var values_to_upload = new NameValueCollection
            {

                ["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("file")),
                ["fileid"] = encryption.encrypt(fileid, enckey, init_iv),
                ["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(sessionid)),
                ["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(name)),
                ["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(ownerid)),
                ["init_iv"] = init_iv
            };

            var response = req(values_to_upload);

            response = encryption.decrypt(response, enckey, init_iv);

            var json = response_decoder.string_to_generic<response_structure>(response);
            load_response_struct(json);
            if (json.success)
                return encryption.str_to_byte_arr(json.contents);
            return null;
        }
        /// <summary>
        /// Logs the IP address,PC Name with a message, if a discord webhook is set up in the app settings, the log will get sent there and the dashboard if not set up it will only be in the dashboard
        /// </summary>
        /// <param name="message">Message</param>
        public void log(string message)
        {
            if (!initzalized)
            {
                error("Please initzalize first");
                Environment.Exit(0);
            }

            var init_iv = encryption.sha256(encryption.iv_key());
            var values_to_upload = new NameValueCollection
            {
                ["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("log")),
                ["pcuser"] = encryption.encrypt(Environment.UserName, enckey, init_iv),
                ["message"] = encryption.encrypt(message, enckey, init_iv),
                ["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(sessionid)),
                ["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(name)),
                ["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(ownerid)),
                ["init_iv"] = init_iv
            };

            req(values_to_upload);
        }
        public static string checksum(string filename)
        {
            string result;
            using (MD5 md = MD5.Create())
            {
                using (FileStream fileStream = File.OpenRead(filename))
                {
                    byte[] value = md.ComputeHash(fileStream);
                    result = BitConverter.ToString(value).Replace("-", "").ToLowerInvariant();
                }
            }
            return result;
        }
        public static void error(string message)
        {
            Process.Start(new ProcessStartInfo("cmd.exe", $"/c start cmd /C \"color b && title Error && echo {message} && timeout /t 5\"")
            {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            });
            Environment.Exit(0);
        }
        private static string req(NameValueCollection post_data)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    var raw_response = client.UploadValues("https://keyauth.win/api/1.0/", post_data);

                    return Encoding.Default.GetString(raw_response);
                }
            }
            catch (WebException webex)
            {
                var response = (HttpWebResponse)webex.Response;
                switch (response.StatusCode)
                {
                    case (HttpStatusCode)429: // client hit our rate limit
                        error("You're connecting too fast to loader, slow down.");
                        Environment.Exit(0);
                        return "";
                    default: // site won't resolve. you should use keyauth.uk domain since it's not blocked by any ISPs
                        error("Connection failure. Please try again, or contact us for help.");
                        Environment.Exit(0);
                        return "";
                }
            }
        }

        /// <summary>
        /// Created for Web Login
        /// </summary>
        private static string req_unenc(NameValueCollection post_data)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    var raw_response = client.UploadValues("https://keyauth.win/api/1.1/", post_data);

                    return Encoding.Default.GetString(raw_response);
                }
            }
            catch (WebException webex)
            {
                var response = (HttpWebResponse)webex.Response;
                switch (response.StatusCode)
                {
                    case (HttpStatusCode)429: // client hit our rate limit
                        Thread.Sleep(1000);
                        return req(post_data);
                    default: // site won't resolve. you should use keyauth.uk domain since it's not blocked by any ISPs
                        error("Connection failure. Please try again, or contact us for help.");
                        Environment.Exit(0);
                        return "";
                }
            }
        }

        #region app_data
        public app_data_class app_data = new app_data_class();

        public class app_data_class
        {
            public string numUsers { get; set; }
            public string numOnlineUsers { get; set; }
            public string numKeys { get; set; }
            public string version { get; set; }
            public string customerPanelLink { get; set; }
            public string downloadLink { get; set; }
        }

        private void load_app_data(app_data_structure data)
        {
            app_data.numUsers = data.numUsers;
            app_data.numOnlineUsers = data.numOnlineUsers;
            app_data.numKeys = data.numKeys;
            app_data.version = data.version;
            app_data.customerPanelLink = data.customerPanelLink;
        }
        #endregion

        #region user_data
        public user_data_class user_data = new user_data_class();

        public class user_data_class
        {
            public string username { get; set; }
            public string ip { get; set; }
            public string hwid { get; set; }
            public string createdate { get; set; }
            public string lastlogin { get; set; }
            public List<Data> subscriptions { get; set; } // array of subscriptions (basically multiple user ranks for user with individual expiry dates
        }
        public class Data
        {
            public string subscription { get; set; }
            public string expiry { get; set; }
            public string timeleft { get; set; }
        }

        private void load_user_data(user_data_structure data)
        {
            user_data.username = data.username;
            user_data.ip = data.ip;
            user_data.hwid = data.hwid;
            user_data.createdate = data.createdate;
            user_data.lastlogin = data.lastlogin;
            user_data.subscriptions = data.subscriptions; // array of subscriptions (basically multiple user ranks for user with individual expiry dates 
        }
        #endregion


        // Expiry Days Left
        public string expirydaysleft()
        {
            if (!initzalized)
            {
                error("Please initzalize first");
                Environment.Exit(0);
            }

            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Local);
            dtDateTime = dtDateTime.AddSeconds(long.Parse(user_data.subscriptions[0].expiry)).ToLocalTime();
            TimeSpan difference = dtDateTime - DateTime.Now;
            return Convert.ToString(difference.Days + " Days " + difference.Hours + " Hours Left");
        }

        #region response_struct
        public response_class response = new response_class();

        public class response_class
        {
            public bool success { get; set; }
            public string message { get; set; }
        }

        private void load_response_struct(response_structure data)
        {
            response.success = data.success;
            response.message = data.message;
        }
        #endregion

        private json_wrapper response_decoder = new json_wrapper(new response_structure());
    }

    public static class encryption
    {
        public static string byte_arr_to_str(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        // BROKEN
        public static byte[] str_to_byte_arr(string hex)
        {
            try
            {
                int NumberChars = hex.Length;
                byte[] bytes = new byte[NumberChars / 2];
                for (int i = 0; i < NumberChars; i += 2)
                    bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
                return bytes;
            }
            catch
            {
                Console.WriteLine("\n\n  The session has ended, open program again.");
                Thread.Sleep(3500);
                Environment.Exit(0);
                return null;
            }
        }

        public static string encrypt_string(string plain_text, byte[] key, byte[] iv)
        {
            Aes encryptor = Aes.Create();

            encryptor.Mode = CipherMode.CBC;
            encryptor.Key = key;
            encryptor.IV = iv;

            using (MemoryStream mem_stream = new MemoryStream())
            {
                using (ICryptoTransform aes_encryptor = encryptor.CreateEncryptor())
                {
                    using (CryptoStream crypt_stream = new CryptoStream(mem_stream, aes_encryptor, CryptoStreamMode.Write))
                    {
                        byte[] p_bytes = Encoding.Default.GetBytes(plain_text);

                        crypt_stream.Write(p_bytes, 0, p_bytes.Length);

                        crypt_stream.FlushFinalBlock();

                        byte[] c_bytes = mem_stream.ToArray();

                        return byte_arr_to_str(c_bytes);
                    }
                }
            }
        }

        public static string decrypt_string(string cipher_text, byte[] key, byte[] iv)
        {
            Aes encryptor = Aes.Create();

            encryptor.Mode = CipherMode.CBC;
            encryptor.Key = key;
            encryptor.IV = iv;

            using (MemoryStream mem_stream = new MemoryStream())
            {
                using (ICryptoTransform aes_decryptor = encryptor.CreateDecryptor())
                {
                    using (CryptoStream crypt_stream = new CryptoStream(mem_stream, aes_decryptor, CryptoStreamMode.Write))
                    {
                        byte[] c_bytes = str_to_byte_arr(cipher_text);

                        crypt_stream.Write(c_bytes, 0, c_bytes.Length);

                        crypt_stream.FlushFinalBlock();

                        byte[] p_bytes = mem_stream.ToArray();

                        return Encoding.Default.GetString(p_bytes, 0, p_bytes.Length);
                    }
                }
            }
        }

        public static string iv_key() =>
            Guid.NewGuid().ToString().Substring(0, Guid.NewGuid().ToString().IndexOf("-", StringComparison.Ordinal));

        public static string sha256(string r) =>
            byte_arr_to_str(new SHA256Managed().ComputeHash(Encoding.Default.GetBytes(r)));

        public static string encrypt(string message, string enc_key, string iv)
        {
            byte[] _key = Encoding.Default.GetBytes(sha256(enc_key).Substring(0, 32));

            byte[] _iv = Encoding.Default.GetBytes(sha256(iv).Substring(0, 16));

            return encrypt_string(message, _key, _iv);
        }

        public static string decrypt(string message, string enc_key, string iv)
        {
            byte[] _key = Encoding.Default.GetBytes(sha256(enc_key).Substring(0, 32));

            byte[] _iv = Encoding.Default.GetBytes(sha256(iv).Substring(0, 16));

            return decrypt_string(message, _key, _iv);
        }
    }

    public class json_wrapper
    {
        public static bool is_serializable(Type to_check) =>
            to_check.IsSerializable || to_check.IsDefined(typeof(DataContractAttribute), true);

        public json_wrapper(object obj_to_work_with)
        {
            current_object = obj_to_work_with;

            var object_type = current_object.GetType();

            serializer = new DataContractJsonSerializer(object_type);

            if (!is_serializable(object_type))
                throw new Exception($"the object {current_object} isn't a serializable");
        }

        public object string_to_object(string json)
        {
            var buffer = Encoding.Default.GetBytes(json);

            //SerializationException = session expired

            using (var mem_stream = new MemoryStream(buffer))
                return serializer.ReadObject(mem_stream);
        }

        public T string_to_generic<T>(string json) =>
            (T)string_to_object(json);

        private DataContractJsonSerializer serializer;

        private object current_object;
    }
}
