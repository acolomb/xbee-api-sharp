using System;
using System.Collections.Concurrent;
using NLog;

namespace XBee
{
    public class XBeeResponseTracker
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public const byte NoResponseFrameId = 0;
        public const byte DefaultFrameId    = 1;
        public const byte MaxFrameId        = byte.MaxValue;

        private ConcurrentDictionary<byte, FrameReceivedHandler> callbacks;

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

        public XBeeResponseTracker()
        {
            callbacks = new ConcurrentDictionary<byte, FrameReceivedHandler>();
        }

        public void RegisterDefaultFrameHandler(FrameReceivedHandler handler)
        {
            callbacks.AddOrUpdate(NoResponseFrameId, handler,
                                  (key, existingVal) => { return handler; });
            logger.Debug("Registered handler {0} for responses with Frame ID {1}.", handler.Method.Name, NoResponseFrameId);
        }

        public byte RegisterResponseHandler(FrameReceivedHandler handler)
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
            FrameReceivedHandler handler;
            if (callbacks.TryRemove(frameId, out handler)) {
                // recycle frame ID so it grows slower
                NextFrameId = frameId;
            }
            logger.Debug("Removed frame handler {0} for Frame ID {1}.", handler.Method.Name, frameId);
        }
        
        public void HandleFrameReceived(object sender, FrameReceivedEventArgs args)
        {
            FrameReceivedHandler handler;
            if (callbacks.TryGetValue(args.Response.FrameId, out handler)) {
                handler.Invoke(this, args);
            }
        }
    }
}
