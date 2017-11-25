using System;
using System.Threading.Tasks;
using Refit;


namespace stratfaucet
{
        public interface IReCaptchaAPI
        {
            [Post("/recaptcha/api/siteverify")]
            Task<CaptchaResponse> Verify([Body(BodySerializationMethod.UrlEncoded)] Captcha captcha);
        }

}
