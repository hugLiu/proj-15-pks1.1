using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.Com.Wcf
{

    /// <summary>
    /// 
    /// </summary>
    public class CustomCertificateValidator : X509CertificateValidator
    {
        public override void Validate(System.Security.Cryptography.X509Certificates.X509Certificate2 certificate)
        {
            if (certificate == null)
            {
                throw new ArgumentNullException("X509认证证书为空！");
            }
            //if (certificate.Thumbprint != "‎F55A7309EE9FFFD05B9F355F2798BC8C09AFB311".ToUpper())
            //{
            //    throw new System.IdentityModel.Tokens.SecurityTokenException("Certificate Validation Error!");
            //}

            //throw new NotImplementedException();
        }
    }
}
