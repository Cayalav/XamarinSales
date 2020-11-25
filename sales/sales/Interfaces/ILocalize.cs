

namespace sales.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    public interface ILocalize
    {

         CultureInfo GetCurrentCultureInfo();

        void SetLocale(CultureInfo ci);
    }
}
