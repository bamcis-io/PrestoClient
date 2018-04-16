using System;
using System.Collections.Generic;
using System.Text;

namespace BAMCIS.PrestoClient.Model.SPI
{
    /// <summary>
    /// From com.facebook.presto.spi.StandardErrorCode.java
    /// 
    /// Not done.
    /// </summary>
    public enum StandardErrorCode
    {
        GENERIC_USER_ERROR = 0x0000_0000,
        SYNTAX_ERROR = 0x0000_0001,
        ABANDONED_QUERY = 0x0000_0002,
        NOT_SUPPORTED = 0x0000_000D,
        INVALID_SESSION_PROPERTY = 0x0000_000E
    }
}
