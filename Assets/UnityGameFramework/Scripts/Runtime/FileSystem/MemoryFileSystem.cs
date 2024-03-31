//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.FileSystem;
using System;
using System.IO;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 内存文件系统流。
    /// </summary>
    public sealed class MemoryFileSystemStream : FileSystemStream, IDisposable
    {
        MemoryStream m_MemoryStream;
        public MemoryStream MemoryStream
        {
            get
            {
                return m_MemoryStream;
            }
            set
            {
                m_MemoryStream = value;
            }
        }
        public MemoryFileSystemStream(string fullPath, FileSystemAccess access, bool createNew, byte[] data)
        {
            if (access != FileSystemAccess.Read)
            {
                throw new GameFrameworkException(fullPath + "  MemoryFileSystemStream Only Read");
            }
            if (data == null || data.Length <= 0)
            {
                throw new GameFrameworkException(fullPath + "  MemoryFileSystemStream data != null && data.length > 0");
            }
            m_MemoryStream = new MemoryStream(data);
        }

        protected override long Position
        {
            get
            {
                return m_MemoryStream.Position;
            }
            set
            {
                m_MemoryStream.Position = value;
            }
        }

        protected override long Length
        {
            get
            {
                return m_MemoryStream.Length;
            }
        }

        protected override void Close()
        {
            m_MemoryStream.Close();
        }

        protected override void Flush()
        {
            m_MemoryStream.Flush();
        }

        protected override int Read(byte[] buffer, int startIndex, int length)
        {
            return m_MemoryStream.Read(buffer, startIndex, length);
        }

        protected override int ReadByte()
        {
            return m_MemoryStream.ReadByte();
        }

        protected override void Seek(long offset, SeekOrigin origin)
        {
            m_MemoryStream.Seek(offset, origin);
        }


        protected override void SetLength(long length)
        {
            m_MemoryStream.SetLength(length);
        }

        protected override void Write(byte[] buffer, int startIndex, int length)
        {
            m_MemoryStream.Write(buffer, startIndex, length);
        }

        protected override void WriteByte(byte value)
        {
            m_MemoryStream.WriteByte(value);
        }

        public void Dispose()
        {
            m_MemoryStream.Dispose();
        }
    }
}
