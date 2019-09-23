namespace SSMSPlusSearch
{
    using EnvDTE80;
    using Microsoft.VisualStudio.Shell;
    using SSMSPlusSearch.Services;
    using System;

    public class SearchPlugin
    {

        private bool isRegistred = false;
        private SearchUi _searchUi;

        public SearchPlugin(SearchUi searchUi)
        {
            _searchUi = searchUi;
        }

        public void Register()
        {
            if (isRegistred)
            {
                throw new Exception("SearchPlugin is already registred");
            }

            isRegistred = true;
            _searchUi.Register();
        }
    }
}
