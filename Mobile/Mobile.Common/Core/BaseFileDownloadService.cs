using Android.App;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using Environment = Android.OS.Environment;
using Exception = Java.Lang.Exception;
using Uri = Android.Net.Uri;

namespace Mobile.Common.Core
{
    public abstract class BaseFileDownloadService<U> : BaseIntentService<U>
    {
        protected DownloadManager.Request CurrentRequest;
        private long downloadId;
        private DownloadManager downloadManager;
        protected string AbsolutePathToDownloadedFile { get; set; }

        [Obsolete]
        protected void Download(string title, string path, bool obsolete)
        {
            Console.WriteLine("Master Data Download URL: {0}", path);

            downloadManager = (DownloadManager)GetSystemService(DownloadService);
            CurrentRequest = new DownloadManager.Request(Uri.Parse(path));
            CurrentRequest.SetTitle(title);
            CurrentRequest.SetDestinationInExternalFilesDir(this, "", CreateLocalFileName());
            downloadId = downloadManager.Enqueue(CurrentRequest);

            MonitorStatus();
        }

        protected void Download(string title, string path)
        {
            Console.WriteLine("Master Data Download URL: {0}", path);

            //Use hhtp client
            try
            {
                var localFileName = CreateLocalFileName();
                var httpClient = SetupLocalClient(path);

                OnStatusUpdate(new DownloadStatusUpdate("In progress", 0));
                var result = httpClient.GetByteArrayAsync(path).Result;

                Console.WriteLine("The byte arraylenth is: .." + result.Length);
                var externalfileDir = ExternalFileDirectory();

                OnStatusUpdate(new DownloadStatusUpdate("In progress", 80));
                File.WriteAllBytes(externalfileDir + localFileName, result);

                Console.WriteLine("finished" + DateTime.Now);
                Console.WriteLine("file name full...: " + localFileName);

                AbsolutePathToDownloadedFile = externalfileDir + localFileName;
                OnStatusUpdate(new DownloadStatusUpdate("Download Complete", 100));
            }
            catch (Exception e)
            {
                Console.WriteLine("file name full...: failed...");
                Console.WriteLine(e);

                OnStatusUpdate(CreateFailureUpdate(1002));
            }
            catch (System.Exception e)
            {
                Console.WriteLine("file name full...: failed...");
                Console.WriteLine(e);
                OnStatusUpdate(CreateFailureUpdate(1002));
            }

            /*Todo : Modify MonitorStatus() to track download percentage*/
        }

        protected abstract string CreateLocalFileName();

        private void MonitorStatus()
        {
            var statusQuery = new DownloadManager.Query();
            statusQuery.SetFilterById(downloadId);
            var status = GetStatus(statusQuery);

            while (!status.Finished)
            {
                Thread.Sleep(500);
                status = GetStatus(statusQuery);
                OnStatusUpdate(status);
            }
        }

        public HttpClient SetupLocalClient(string uri)
        {
            var client = new HttpClient();
            //client.Timeout = TimeSpan.FromSeconds(90);
            //AJM TODO remove default is 100 seconds - integration tests timing out
            client.Timeout = TimeSpan.FromMinutes(10);
            client.BaseAddress = new System.Uri(uri);
            return client;
        }

        private string ExternalFileDirectory()
        {
            var externalDir = Environment.ExternalStorageDirectory.AbsolutePath + "/.AgrimanagrCsv/";

            var dir = new Java.IO.File(externalDir);

            if (!dir.Exists())
                dir.Mkdir();
            else
            {
                try
                {
                    var files = dir.ListFiles();
                    foreach (var file in files)
                    {
                        file.Delete();
                    }
                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            Console.WriteLine("Directory created success..");
            return externalDir;
        }

        private DownloadStatusUpdate GetStatus(DownloadManager.Query statusQuery)
        {
            using (var cursor = downloadManager.InvokeQuery(statusQuery))
            {
                if (!cursor.MoveToNext())
                {
                    return new DownloadStatusUpdate("In progress", 0);
                }

                var status = cursor.GetInt(cursor.GetColumnIndex(DownloadManager.ColumnStatus));

                var reason = cursor.GetInt(cursor.GetColumnIndex(DownloadManager.ColumnReason));
                var bytesDownloaded = cursor.GetInt(cursor.GetColumnIndex(DownloadManager.ColumnBytesDownloadedSoFar));
                var totalBytes = cursor.GetInt(cursor.GetColumnIndex(DownloadManager.ColumnTotalSizeBytes));
                var percentDone = bytesDownloaded / (double)totalBytes;

                var downloadStatus = (DownloadStatus)status;

                cursor.Close();

                switch (downloadStatus)
                {
                    case DownloadStatus.Failed:
                        return CreateFailureUpdate(reason);

                    case DownloadStatus.Pending:
                    case DownloadStatus.Running:
                        return new DownloadStatusUpdate("In progress", percentDone);

                    case DownloadStatus.Paused:
                        Console.WriteLine("REASON IS...: " + reason);
                        return new DownloadStatusUpdate("Waiting for network", percentDone, paused: true);

                    case DownloadStatus.Successful:
                        return new DownloadStatusUpdate("Download Complete", 100);

                    default:
                        throw new System.Exception("Status not handled " + status);
                }
            }
        }

        private DownloadStatusUpdate CreateFailureUpdate(int reason)
        {
            var reasonText = "Unknown error";

            switch ((DownloadError)reason)
            {
                case DownloadError.CannotResume:
                    reasonText = "Can not resume download. Please try again";
                    break;

                case DownloadError.DeviceNotFound:
                    reasonText = "SD Card unavailable or not mounted";
                    break;

                case DownloadError.FileAlreadyExists:
                    reasonText = "A file with the same name already exists";
                    break;

                case DownloadError.FileError:
                    reasonText = "Unable to store file";
                    break;

                case DownloadError.HttpDataError:
                    reasonText = "Error when communciation with server (HTTP Error)";
                    break;

                case DownloadError.InsufficientSpace:
                    reasonText = "Can not complete download due to insuffienct space";
                    break;

                case DownloadError.TooManyRedirects:
                    reasonText = "Server is incorrectly configured (too many redirects)";
                    break;

                case DownloadError.UnhandledHttpCode:
                    reasonText = "Server responded with an invalid HTTP code";
                    break;

                case DownloadError.Unknown:
                    break;
            }

            Console.WriteLine("Received error code during download {0} - {1}", reason, reasonText);

            return new DownloadStatusUpdate(reasonText, failed: true);
        }

        public abstract void OnStatusUpdate(DownloadStatusUpdate statud);

        //Validate the URL before Calling Android's Download Manager.
        public bool UrlIsValid(string url)
        {
            try
            {
                var request = WebRequest.Create(url) as HttpWebRequest;
                request.Timeout = 5000;
                request.Method = "HEAD";

                var response = request.GetResponse() as HttpWebResponse;

                var statusCode = (int)response.StatusCode;
                if (statusCode >= 100 && statusCode < 406) //Good requests including 405 - method now allowed
                {
                    return true;
                }
                if (statusCode >= 500 && statusCode <= 510) //Server Errors
                {
                    Console.WriteLine("The remote server has thrown an internal error. Url is not valid: {0}", url);
                    return false;
                }
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    if (((HttpWebResponse)e.Response).StatusCode == HttpStatusCode.MethodNotAllowed)
                    {
                        //The URL exists but it doesn't support the method we used (head), which is expected
                        return true;
                    }
                }
                Console.WriteLine(e);
                return false;
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }
    }

    public class DownloadStatusUpdate
    {
        public DownloadStatusUpdate(string message, double progress = -1, bool failed = false, bool paused = false)
        {
            Failed = failed;
            Message = message;
            Progress = progress;
            Finished = Failed || progress == 100;
            Paused = paused;
        }

        public bool Failed { get; }
        public string Message { get; private set; }
        public double Progress { get; private set; }
        public bool Finished { get; }
        public bool Paused { get; private set; }
    }
}