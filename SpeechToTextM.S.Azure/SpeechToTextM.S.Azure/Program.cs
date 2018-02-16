using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SpeechToTextMSCSAPICSharpConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            string requestUri = @"https://speech.platform.bing.com/speech/recognition/interactive/cognitiveservices/v1?language=en-US&format=detailed";

            HttpWebRequest request = null;
            request = (HttpWebRequest)HttpWebRequest.Create(requestUri);
            request.SendChunked = true;
            request.Accept = @"application/json;text/xml";
            request.Method = "POST";
            request.ProtocolVersion = HttpVersion.Version11;
            request.ContentType = @"audio/wav; codec=audio/pcm; samplerate=16000";
            request.Headers["Ocp-Apim-Subscription-Key"] = "2205bbb9e258499cb341a6a04225d48b";


            // Send an audio file by 1024 byte chunks
            FileStream fs;
            using (fs = new FileStream(@"c:\test\fine.wav", FileMode.Open, FileAccess.Read))
            {

                /*
                * Open a request stream and write 1024 byte chunks in the stream one at a time.
                */
                byte[] buffer = null;
                int bytesRead = 0;
                using (Stream requestStream = request.GetRequestStream())
                {
                    /*
                    * Read 1024 raw bytes from the input audio file.
                    */
                    buffer = new Byte[checked((uint)Math.Min(1024, (int)fs.Length))];
                    while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        requestStream.Write(buffer, 0, bytesRead);
                    }

                    // Flush
                    requestStream.Flush();
                }
            }

            /*
* Get the response from the service.
*/
            Console.WriteLine("Response:");
            using (WebResponse response = request.GetResponse())
            {
                Console.WriteLine(((HttpWebResponse)response).StatusCode);

                string responseString;
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    responseString = sr.ReadToEnd();
                }

                Console.WriteLine(responseString);
                Console.ReadLine();
            }






        }
    }
}