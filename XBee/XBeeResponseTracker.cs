using System;
using System.Collections.Concurrent;
using NLog;

namespace XBee
{
    public delegate void ResponseReceivedHandler(XBeeResponseTracker sender, FrameReceivedEventArgs args);

    public class XBeeResponseTracker
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public const byte NoResponseFrameId = 0;
        public const byte DefaultFrameId    = 1;
        public const byte MaxFrameId        = byte.MaxValue;

        private ConcurrentDictionary<byte, ResponseReceivedHandler> callbacks;

        private byte currentFrameId = DefaultFrameId;
        private object currentFrameIdLock = new Object();

        private byte NextFrameId {
            get {
                byte current;
                lock (currentFrameIdLock) {
                    current = currentFrameId;
                    if (currentFrameId < MaxFrameId) {
                        ++currentFrameId;
                    } else {
                        currentFrameId = DefaultFrameId;
                    }
                }
                return current;
            }
            set {
                lock (currentFrameIdLock) {
                    currentFrameId = value;
                }
            }
        }

        public event ResponseReceivedHandler UnexpectedResponse;

        public XBeeResponseTracker()
        {
            callbacks = new ConcurrentDictionary<byte, ResponseReceivedHandler>();
        }

        public void RegisterDefaultFrameHandler(ResponseReceivedHandler handler)
        {
            callbacks.AddOrUpdate(NoResponseFrameId, handler,
                                  (key, existingVal) => { return handler; });
            logger.Debug("Registered handler {0} for responses with Frame ID {1}.", handler.Method.Name, NoResponseFrameId);
        }

        public byte RegisterResponseHandler(ResponseReceivedHandler handler)
        {
            if (callbacks.Count == MaxFrameId) {
                logger.Warn("No Frame ID available. Handler {0} will not be called.", handler.Method.Name);
                return NoResponseFrameId;
            }

            byte frameId;
            do {
                frameId = NextFrameId;
            } while (! callbacks.GetOrAdd(frameId, handler).Equals(handler));

            logger.Debug("Registered handler {0} for responses with Frame ID {1}.", handler.Method.Name, frameId);
            return frameId;
        }
        
        public void UnregisterResponseHandler(byte frameId)
        {
            ResponseReceivedHandler handler;
            if (callbacks.TryRemove(frameId, out handler)) {
                // recycle frame ID so it grows slower
                NextFrameId = frameId;
            }
            logger.Debug("Removed frame handler {0} for Frame ID {1}.", handler.Method.Name, frameId);
        }
        
        public void HandleFrameReceived(object sender, FrameReceivedEventArgs args)
        {
            ResponseReceivedHandler handler;
            if (callbacks.TryGetValue(args.Response.FrameId, out handler)) {
                handler(this, args);
            } else if (UnexpectedResponse != null) {
                UnexpectedResponse(this, args);
            }
        }
    }
}

