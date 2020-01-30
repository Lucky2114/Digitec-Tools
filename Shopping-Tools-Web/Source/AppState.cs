using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping_Tools_Web.Source
{
    public class AppState
    {
        public event Action OnChange;

        public bool AppIsLoading { get; private set; }

        public void SetAppLoading(bool isLoading)
        {
            AppIsLoading = isLoading;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
