using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    /*
         Paragraph:报表中的文本
         Image:报表中的图片
         PdfPTable:表格
         PdfPCell:单元格
    */
    public class PDFHelper
    {
        public static Document CreateDocument(string filePath)
        {
            Document doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
            doc.Open();
            return doc;
        }
    }
}
