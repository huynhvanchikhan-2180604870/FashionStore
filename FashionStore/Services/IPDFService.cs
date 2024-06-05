using DinkToPdf;

namespace FashionStore.Services
{
    public interface IPDFService
    {
        public byte[] GenaratePDF(string contentHTML, Orientation orientation = Orientation.Portrait, PaperKind paperKind = PaperKind.A4);
    }
}