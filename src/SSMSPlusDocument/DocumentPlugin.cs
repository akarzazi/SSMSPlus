using SSMSPlusDocument.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMSPlusDocument
{
    public class DocumentPlugin
    {
        DocumentUi _documentUi;
        private bool isRegistred = false;

        public DocumentPlugin(DocumentUi documentUi)
        {
            _documentUi = documentUi;
        }


        public void Register()
        {
            if (isRegistred)
            {
                throw new Exception("DocumentPlugin is already registred");
            }

            isRegistred = true;
            _documentUi.Register();
        }
    }
}
