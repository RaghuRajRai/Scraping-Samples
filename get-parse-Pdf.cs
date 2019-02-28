using System;
using System.Net;
using System.Text;
using iTextSharp;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace getPdf
{
    class Program
    {
        //Function to download and save pdf files
        public String DownloadPdf(String downloadLink, String storageLink)
        {
            //Although C# takes care of cleaning up resources, using ensures that it happens at a more predictable timing.
            using (WebClient wc = new WebClient())
            {
                wc.DownloadFile(downloadLink, storageLink);
            }
            //Now we read the pdf to a string
            StringBuilder text = new StringBuilder();
            //Parsing the pdf file
            using (PdfReader reader = new PdfReader(storageLink))
            {
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                }
            }
            //Saving to a String object
            String returnText = text.ToString();
            return returnText;
        }

        static void Main(string[] args)
        {
            Program test = new Program();
            String text = test.DownloadPdf("http://www.africau.edu/images/default/sample.pdf", @"E:\a.pdf");
            Console.WriteLine(text);
            Console.ReadLine();
        }
    }
}
