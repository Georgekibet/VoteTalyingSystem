﻿using System;

namespace Mobile.Common.Net
{
    public static class Timeouts
    {
        //10 Seconds
        private static readonly TimeSpan DEFAULT_HTTP_TIMEOUT = new TimeSpan(0, 0, 30);
        private static readonly TimeSpan FINITE_AlTERMASTERDATA_TIMEOUT=new TimeSpan(0,0,2,0);
        public static TimeSpan DefaultHttpTimeout()
        {
            return DEFAULT_HTTP_TIMEOUT;
        }
        public static TimeSpan AlterMAsteDataTimeOut()
        {
            return FINITE_AlTERMASTERDATA_TIMEOUT;
        }
    }
}