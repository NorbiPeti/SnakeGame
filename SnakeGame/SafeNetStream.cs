using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class SafeNetStream : Stream
    {
        private NetworkStream _ns;
        //public event EventHandler ErrorEvent;
        public TcpClient ErrorStopClient;
        public Thread ErrorStopThread;
        public override bool CanRead
        {
            get { return _ns.CanRead; }
        }

        public override bool CanSeek
        {
            get { return _ns.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return _ns.CanWrite; }
        }

        public override void Flush()
        {
            try
            {
                _ns.Flush();
            }
            catch (IOException)
            {
                /*if (ErrorEvent != null)
                    ErrorEvent(this, EventArgs.Empty);*/
                OnError();
            }
        }

        public override long Length
        {
            get { return _ns.Length; }
        }

        public override long Position
        {
            get
            {
                return _ns.Position;
            }
            set
            {
                _ns.Position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            try
            {
                return _ns.Read(buffer, offset, count);
            }
            catch (IOException)
            {
                OnError();
                return 0;
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _ns.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            try
            {
                _ns.SetLength(value);
            }
            catch (IOException)
            {
                OnError();
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            try
            {
                _ns.Write(buffer, offset, count);
            }
            catch(IOException)
            {
                OnError();
            }
        }

        public SafeNetStream(NetworkStream stream, TcpClient errorstopclient, Thread errorstopthread)
        {
            _ns = stream;
            ErrorStopClient = errorstopclient;
            ErrorStopThread = errorstopthread;
        }

        private void OnError()
        {
            try
            {
                if (ErrorStopClient != null)
                    ErrorStopClient.Close();
            }
            catch { }
            try
            {
                if (ErrorStopThread != null)
                    ErrorStopThread.Abort();
            }
            catch { }
        }
    }
    public static class SafeNetStreamExt
    {
        public static SafeNetStream ToSafeNetStream(this NetworkStream stream, TcpClient errorstopclient = null, Thread errorstopthread = null)
        {
            return new SafeNetStream(stream, errorstopclient, errorstopthread);
        }
    }
}
