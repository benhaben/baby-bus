// ImageLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Foundation;
using UIKit;
using Utilities.Touch;

namespace CrossUI.Touch.Dialog.Utilities
{
    /// <summary>
    ///    This interface needs to be implemented to be notified when an image
    ///    has been downloaded.   The notification will happen on the UI thread.
    ///    Upon notification, the code should call RequestImage again, this time
    ///    the image will be loaded from the on-disk cache or the in-memory cache.
    /// </summary>
    public interface IImageUpdated
    {
        void UpdatedImage(Uri uri);
    }

    /// <summary>
    ///   Network image loader, with local file system cache and in-memory cache
    /// </summary>
    /// <remarks>
    ///   By default, using the static public methods will use an in-memory cache
    ///   for 50 images and 4 megabyte total.   The behavior of the static methods 
    ///   can be modified by setting the public DefaultLoader property to a value
    ///   that the user configured.
    /// 
    ///   The instance methods can be used to create different imageloader with 
    ///   different properties.
    ///  
    ///   Keep in mind that the phone does not have a lot of memory, and using
    ///   the cache with the unlimited value (0) even with a number of items in
    ///   the cache can consume memory very quickly.
    /// 
    ///   Use the Purge method to release all the memory kept in the caches on
    ///   low memory conditions, or when the application is sent to the background.
    /// </remarks>
    public class ImageLoader
    {
        public static readonly string BaseDir =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "..");

        private const int MaxRequests = 6;
        private static readonly string PicDir;

        // Cache of recently used images
        private readonly LRUCache<Uri, UIImage> _cache;

        // A list of requests that have been issues, with a list of objects to notify.
        private static readonly Dictionary<Uri, List<IImageUpdated>> _pendingRequests;

        // A list of updates that have completed, we must notify the main thread about them.
        private static readonly HashSet<Uri> _queuedUpdates;

        // A queue used to avoid flooding the network stack with HTTP requests
        private static readonly Stack<Uri> _requestQueue;

        private static readonly NSString _nsDispatcher = new NSString("x");

        /// <summary>
        ///    This contains the default loader which is configured to be 500 images
        ///    up to 300 megs of memory.   Assigning to this property a new value will
        ///    change the behavior.   This property is lazyly computed, the first time
        ///    an image is requested.
        /// </summary>
        public static ImageLoader DefaultLoader;

        static ImageLoader()
        {
            PicDir = Path.Combine(BaseDir, "Library/Caches/Pictures.MonoTouch.Dialog/");

            if (!Directory.Exists(PicDir))
                Directory.CreateDirectory(PicDir);

            _pendingRequests = new Dictionary<Uri, List<IImageUpdated>>();
            _queuedUpdates = new HashSet<Uri>();
            _requestQueue = new Stack<Uri>();
        }

        /// <summary>
        ///   Creates a new instance of the image loader
        /// </summary>
        /// <param name="cacheSize">
        /// The maximum number of entries in the LRU cache
        /// </param>
        /// <param name="memoryLimit">
        /// The maximum number of bytes to consume by the image loader cache.
        /// </param>
        public ImageLoader(int cacheSize, int memoryLimit)
        {
            _cache = new LRUCache<Uri, UIImage>(cacheSize, memoryLimit, Sizer);
        }

        private static int Sizer(UIImage img)
        {
            var cg = img.CGImage;
            return (int)(cg.BytesPerRow * cg.Height);
        }

        /// <summary>
        ///    Purges the contents of the DefaultLoader
        /// </summary>
        public static void Purge()
        {
            if (DefaultLoader != null)
                DefaultLoader.PurgeCache();
        }

        /// <summary>
        ///    Purges the cache of this instance of the ImageLoader, releasing 
        ///    all the memory used by the images in the caches.
        /// </summary>
        public void PurgeCache()
        {
            _cache.Purge();
        }

        /// <summary>
        ///   Requests an image to be loaded using the default image loader
        /// </summary>
        /// <param name="uri">
        /// The URI for the image to load
        /// </param>
        /// <param name="notify">
        /// A class implementing the IImageUpdated interface that will be invoked when the image has been loaded
        /// </param>
        /// <returns>
        /// If the image has already been downloaded, or is in the cache, this will return the image as a UIImage.
        /// </returns>
        public static UIImage DefaultRequestImage(Uri uri, IImageUpdated notify)
        {
            if (uri == null)
            {
                return null;
            }
            if (DefaultLoader == null)
                DefaultLoader = new ImageLoader(500, 300 * 1024 * 1024);
            return DefaultLoader.RequestImage(uri, notify);
        }

        /// <summary>
        ///   Requests an image to be loaded from the network
        /// </summary>
        /// <param name="uri">
        /// The URI for the image to load
        /// </param>
        /// <param name="notify">
        /// A class implementing the IImageUpdated interface that will be invoked when the image has been loaded
        /// </param>
        /// <returns>
        /// If the image has already been downloaded, or is in the cache, this will return the image as a UIImage.
        /// </returns>
        public UIImage RequestImage(Uri uri, IImageUpdated notify)
        {
            UIImage ret;

            lock (_cache)
            {
                ret = _cache[uri];
                if (ret != null)
                {
                    return ret;
                }
            }

            lock (_requestQueue)
            {
                if (_pendingRequests.ContainsKey(uri))
                {

                    //notify can not be null, it's impossibale to notify a null
                    if (notify != null)
                    {
                        _pendingRequests[uri].Add(notify);
                    }
                    return null;
                }
            }

            string picfile = uri.IsFile ? uri.LocalPath : PicDir
                             + UtilitiesTouch.GetMd5String(Encoding.UTF8.GetBytes(uri.AbsoluteUri));

            if (File.Exists(picfile))
            {
                ret = UIImage.FromFile(picfile);
                if (ret != null)
                {
                    lock (_cache)
                    {
                        _cache[uri] = ret;
                    }
                    return ret;
                }
            }
            if (uri.IsFile)
                return null;

            //notify can be null, we can cache it howerver
            QueueRequest(uri, picfile, notify);
            return null;
        }

        private static void QueueRequest(Uri uri, string target, IImageUpdated notify)
        {

            lock (_requestQueue)
            {
                if (_pendingRequests.ContainsKey(uri) && notify != null)
                {
                    _pendingRequests[uri].Add(notify);
                    return;
                }
                else if (notify != null)
                {
                    var slot = new List<IImageUpdated>(4);
                    slot.Add(notify);
                    _pendingRequests[uri] = slot;
                }
                else
                {
                    //do not need to care _pendingRequests
                }

                if (_sPicDownloaders >= MaxRequests)
                    _requestQueue.Push(uri);
                else
                {
                    ThreadPool.QueueUserWorkItem(delegate
                        {
                            try
                            {
                                StartPicDownload(uri, target);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                        });
                }
            }
        }

        private static long _sPicDownloaders;

        private static void StartPicDownload(Uri uri, string target)
        {
            Interlocked.Increment(ref _sPicDownloaders);
            try
            {
                _StartPicDownload(uri, target);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("CRITICAL: should have never happened {0}", e);
            }
            //Util.Log ("Leaving StartPicDownload {0}", picDownloaders);
            Interlocked.Decrement(ref _sPicDownloaders);
        }

        private static void _StartPicDownload(Uri uri, string target)
        {
            do
            {
                bool downloaded = false;

                downloaded = Download(uri, target);
                if (!downloaded)
                    Console.WriteLine("Error fetching picture for {0} to {1}", uri, target);

                // Cluster all updates together
                bool doInvoke = false;

                lock (_requestQueue)
                {
                    if (downloaded)
                    {
                        _queuedUpdates.Add(uri);
                        // If this is the first queued update, must notify
                        if (_queuedUpdates.Count == 1)
                            doInvoke = true;
                    }
                    else
                    {
                        _pendingRequests.Remove(uri);
                    }

                    // Try to get more jobs.
                    if (_requestQueue.Count > 0)
                    {
                        uri = _requestQueue.Pop();
                        if (uri == null)
                        {
                            Console.Error.WriteLine("Dropping request {0} because url is null", uri);
                            _pendingRequests.Remove(uri);
                            uri = null;
                        }
                    }
                    else
                    {
                        uri = null;
                    }
                }
                if (doInvoke)
                    _nsDispatcher.BeginInvokeOnMainThread(NotifyImageListeners);
            } while (uri != null);
        }

        private static bool Download(Uri uri, string target)
        {
            var buffer = new byte[4 * 1024];

            try
            {
                var tmpfile = target + ".tmp";
                using (var file = new FileStream(tmpfile, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    var req = WebRequest.Create(uri) as HttpWebRequest;

                    using (var resp = req.GetResponse())
                    {
                        using (var s = resp.GetResponseStream())
                        {
                            int n;
                            while ((n = s.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                file.Write(buffer, 0, n);
                            }
                        }
                    }
                }
                File.Move(tmpfile, target);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Problem with {0} {1}", uri, e);
                return false;
            }
        }
        // Runs on the main thread
        private static void NotifyImageListeners()
        {
            lock (_requestQueue)
            {
                foreach (var quri in _queuedUpdates)
                {
                    var list = _pendingRequests[quri];
                    _pendingRequests.Remove(quri);
                    foreach (var pr in list)
                    {
                        try
                        {
                            if (pr != null)
                            {
                                pr.UpdatedImage(quri);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }
                _queuedUpdates.Clear();
            }
        }
    }
}