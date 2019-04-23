using System;

namespace SmartHome
{
    public struct UnixTime
    {
        private readonly long m_iUnixTime;

        public UnixTime(long iUnixTime)
        {
            m_iUnixTime = iUnixTime;
        }

        public static implicit operator UnixTime(long iUnixTime)
        {
            return new UnixTime(iUnixTime);
        }

        public static implicit operator long(UnixTime time)
        {
            return time.m_iUnixTime;
        }

        public static implicit operator DateTime(UnixTime time)
        {
            return time.LocalDateTime;
        }

        public DateTime LocalDateTime
        {
            get { return DateTimeOffset.FromUnixTimeSeconds(m_iUnixTime).LocalDateTime; }
        }

        public DateTime UtcDateTime
        {
            get { return DateTimeOffset.FromUnixTimeSeconds(m_iUnixTime).UtcDateTime; }
        }

        public override string ToString()
        {
            return LocalDateTime.ToString();
        }
    }

}