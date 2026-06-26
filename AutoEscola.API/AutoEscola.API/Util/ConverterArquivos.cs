using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.IO.Image;
using System.IO;


namespace AutoEscola.API.Util
{
    public class ConverterArquivos
    {

        public string ConverterImagemBase64ParaPdfBase64(string base64Imagem)
        {
            // ✅ remove prefixo se existir
            if (base64Imagem.Contains(","))
                base64Imagem = base64Imagem.Split(',')[1];

            byte[] imageBytes = Convert.FromBase64String(base64Imagem);

            using (var ms = new MemoryStream())
            {
                using (var writer = new PdfWriter(ms))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new Document(pdf);

                        var imageData = ImageDataFactory.Create(imageBytes);
                        var image = new Image(imageData);

                        // ✅ ajustar tamanho para caber na página
                        image.SetAutoScale(true);

                        document.Add(image);
                        document.Close();
                    }
                }

                // ✅ retorna base64 do PDF 
                return Convert.ToBase64String(ms.ToArray());
            }
        }

    }
}
