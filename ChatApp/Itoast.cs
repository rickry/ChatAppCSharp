using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApp
{
    public interface IToast
    {
        void LongAlert(string message);
        void ShortAlert(string message);
    }
}