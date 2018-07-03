﻿using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Microsoft.AspNetCore.DataProtection.Repositories;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Website.Session
{
    public class PsXmlRepository : IXmlRepository
    {
        private static IAmazonSimpleSystemsManagement _client;

        public PsXmlRepository(IAmazonSimpleSystemsManagement client)
        {
            _client = client;
        }

        public IReadOnlyCollection<XElement> GetAllElements()
        {
            var request = new GetParametersByPathRequest
            {
                Path = "/CookieEncryptionKey"
            };

            var response = _client.GetParametersByPathAsync(request).Result;
            var result = new List<XElement>(response.Parameters.Count);

            response.Parameters.ForEach(x => result.Add(XElement.Parse(x.Value)));
            
            return result;
        }

        public void StoreElement(XElement element, string friendlyName)
        {
            var request = new PutParameterRequest
            {
                Name = "/CookieEncryptionKey/" + friendlyName,
                Value = element.ToString(),
                Description = "Key-" + friendlyName,
                Overwrite = true,
                Type = ParameterType.String
            };

            _client.PutParameterAsync(request);
        }
    }
}