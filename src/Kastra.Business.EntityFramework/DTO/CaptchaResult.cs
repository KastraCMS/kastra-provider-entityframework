using System;

namespace Kastra.Business.DTO
{
    public class CaptchaResult
    {
        /// <summary>
        /// indicates if verify was successfully or not
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// timestamp of the captcha (ISO format yyyy-MM-dd'T'HH:mm:ssZZ)
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// the hostname of the site where the captcha was solved
        /// </summary>
        public string Hostname { get; set; }

        /// <summary>
        /// optional: whether the response will be credited
        /// </summary>
        public bool Credit { get; set; }

        /// <summary>
        /// string based error code array
        /// </summary>
        public string[] ErrorCodes { get; set; }
    }
}
