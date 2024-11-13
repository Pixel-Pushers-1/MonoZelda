using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MonoZelda.Dungeons.Loader
{
    public class HTTPRoomStream : IRoomLoader
    {
        private static readonly HttpClient httpClient = new();
        private string _uri;

        public HTTPRoomStream(string uri)
        {
            _uri = uri;
        }

        public Stream Load(string roomName)
        {
            return DownloadAsync($"{_uri}{roomName}").Result;
        }

        private static async Task<Stream> DownloadAsync(string url)
        {
            try
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStreamAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading CSV: {ex.Message}");
                return null;
            }
        }
    }
}
