using System;
using System.Runtime.InteropServices;

namespace Docnet.Core.Bindings
{
    internal sealed class FormWrapper : IDisposable
    {
        private readonly IntPtr _ptr;

        public FormWrapper(DocumentWrapper docWrapper)
        {
            var formInfo = new FPDF_FORMFILLINFO();

            _ptr = Marshal.AllocHGlobal(Marshal.SizeOf(formInfo));

            for (var i = 1; i <= 2; i++)
            {
                formInfo.version = i;

                Marshal.StructureToPtr(formInfo, _ptr, false);

                Instance = fpdf_view.FPDFDOCInitFormFillEnvironment(docWrapper.Instance, _ptr);

                if (Instance != null)
                {
                    break;
                }
            }
        }

        public FpdfFormHandleT Instance { get; private set; }

        public void Dispose()
        {
            if (Instance != null)
            {
                fpdf_view.FPDF_ExitFormFillEnvironment(Instance);
            }

            Marshal.FreeHGlobal(_ptr);

            Instance = null;
        }
    }
}
