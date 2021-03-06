﻿using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace MonoDragons.Core.Errors
{
    public class ReportErrorHandler : IErrorHandler
    {
        private readonly AppDetails _appDetails;
        private readonly string _postUrl;
        private bool _reportedFatalError = false;

        public ReportErrorHandler(AppDetails appDetails, string postUrl)
        {
            _appDetails = appDetails;
            _postUrl = postUrl;
        }

        public void Handle(Exception ex)
        {
            try
            {
                var inner = ex;
                while (inner.InnerException != null)
                    inner = inner.InnerException;
                
                if (!_reportedFatalError)
                    using (var client = new HttpClient())
                        client.PostAsync(
                            _postUrl,
                            new StringContent(JsonConvert.SerializeObject(new CrashDetail
                            {
                                ApplicationName = _appDetails.Name,
                                ApplicationVersion = _appDetails.Version,
                                ContextJson = JsonConvert.SerializeObject(new Context
                                {
                                    OS = _appDetails.OS,
                                    ErrorMessage = inner.Message
                                }),
                                StackTrace = inner.StackTrace
                            }), Encoding.UTF8, "application/json")).GetAwaiter().GetResult();
                _reportedFatalError = true;
            }
            catch (Exception)
            {
                // Ignore and keep on Trucking
            }
        }

        private class CrashDetail
        {
            public string ApplicationName { get; set; }
            public string ApplicationVersion { get; set; }
            public string ContextJson { get; set; }
            public string StackTrace { get; set; }
        }

        private class Context
        {
            public string OS { get; set; }
            public string ErrorMessage { get; set; }
        }
    }
}
