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

            logger.Debug("Frame ID {0}: Registered handler {1} for responses.", frameId, handler.Method.Name);
            return frameId;
        }
        
        public void UnregisterResponseHandler(byte frameId)
        {
            ResponseReceivedHandler handler;
            if (callbacks.TryRemove(frameId, out handler)) {
                // recycle frame ID so it grows slower
                NextFrameId = frameId;
            }
            logger.Debug("Frame ID {0}: Removed frame handler {1}.", frameId, handler.Method.Name);
        }
        
        public void HandleFrameReceived(object sender, FrameReceivedEventArgs args)
        {
            ResponseReceivedHandler handler;
            if (callbacks.TryGetValue(args.Response.FrameId, out handler)) {
                logger.Debug("Frame ID {0}: Calling handler {1}.", args.Response.FrameId, handler.Method.Name);
                handler(this, args);
            } else if (UnexpectedResponse != null) {
                UnexpectedResponse(this, args);
            }
        }
    }
}

